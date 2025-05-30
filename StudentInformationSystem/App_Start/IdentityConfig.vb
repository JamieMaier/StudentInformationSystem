Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security

Public Class EmailService
    Implements IIdentityMessageService
    Public Function SendAsync(message As IdentityMessage) As Task Implements IIdentityMessageService.SendAsync
        ' Hier den E-Mail-Dienst einfügen, um eine E-Mail-Nachricht zu senden.
        Return Task.FromResult(0)
    End Function
End Class

Public Class SmsService
    Implements IIdentityMessageService
    Public Function SendAsync(message As IdentityMessage) As Task Implements IIdentityMessageService.SendAsync
        ' Hier den SMS-Dienst einfügen, um eine Textnachricht zu senden.
        Return Task.FromResult(0)
    End Function
End Class

' Konfigurieren des in dieser Anwendung verwendeten Anwendungsbenutzer-Managers. UserManager wird in ASP.NET Identity definiert und von der Anwendung verwendet.
Public Class ApplicationUserManager
    Inherits UserManager(Of ApplicationUser)
    Public Sub New(store As IUserStore(Of ApplicationUser))
        MyBase.New(store)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationUserManager), context As IOwinContext) As ApplicationUserManager
        Dim manager = New ApplicationUserManager(New UserStore(Of ApplicationUser)(context.[Get](Of ApplicationDbContext)()))
        ' Konfigurieren der Überprüfungslogik für Benutzernamen.
        manager.UserValidator = New UserValidator(Of ApplicationUser)(manager) With {
          .AllowOnlyAlphanumericUserNames = False,
          .RequireUniqueEmail = True
        }

        ' Konfigurieren der Überprüfungslogik für Kennwörter.
        manager.PasswordValidator = New PasswordValidator() With {
          .RequiredLength = 6,
          .RequireNonLetterOrDigit = True,
          .RequireDigit = True,
          .RequireLowercase = True,
          .RequireUppercase = True
        }
        ' Registrieren von Anbietern für zweistufige Authentifizierung. Diese Anwendung verwendet telefonische und E-Mail-Nachrichten zum Empfangen eines Codes zum Überprüfen des Benutzers.
        ' Sie können Ihren eigenen Anbieter erstellen und hier einfügen.
        manager.RegisterTwoFactorProvider("Telefoncode", New PhoneNumberTokenProvider(Of ApplicationUser)() With {
          .MessageFormat = "Ihr Sicherheitscode lautet {0}"
        })
        manager.RegisterTwoFactorProvider("E-Mail-Code", New EmailTokenProvider(Of ApplicationUser)() With {
          .Subject = "Sicherheitscode",
          .BodyFormat = "Ihr Sicherheitscode lautet {0}"
        })

        ' Standardeinstellungen für Benutzersperren konfigurieren
        manager.UserLockoutEnabledByDefault = True
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5)
        manager.MaxFailedAccessAttemptsBeforeLockout = 5

        manager.EmailService = New EmailService()
        manager.SmsService = New SmsService()
        Dim dataProtectionProvider = options.DataProtectionProvider
        If dataProtectionProvider IsNot Nothing Then
            manager.UserTokenProvider = New DataProtectorTokenProvider(Of ApplicationUser)(dataProtectionProvider.Create("ASP.NET Identity"))
        End If
        Return manager
    End Function
End Class

Public Class ApplicationSignInManager
    Inherits SignInManager(Of ApplicationUser, String)
    Public Sub New(userManager As ApplicationUserManager, authenticationManager As IAuthenticationManager)
        MyBase.New(userManager, authenticationManager)
    End Sub

    Public Overrides Function CreateUserIdentityAsync(user As ApplicationUser) As Task(Of ClaimsIdentity)
        Return user.GenerateUserIdentityAsync(DirectCast(UserManager, ApplicationUserManager))
    End Function

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationSignInManager), context As IOwinContext) As ApplicationSignInManager
        Return New ApplicationSignInManager(context.GetUserManager(Of ApplicationUserManager)(), context.Authentication)
    End Function
End Class
