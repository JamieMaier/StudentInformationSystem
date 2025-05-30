Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin

Partial Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Property StatusMessage() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub Forgot(sender As Object, e As EventArgs)
        If IsValid Then
            ' E-Mail-Adresse des Benutzers überprüfen
            Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
            Dim user As ApplicationUser = manager.FindByName(Email.Text)
            If user Is Nothing OrElse Not manager.IsEmailConfirmed(user.Id) Then
                FailureText.Text = "Der Benutzer ist nicht vorhanden oder wurde nicht bestätigt."
                ErrorMessage.Visible = True
                Return
            End If
            ' Weitere Informationen zum Aktivieren der Kontobestätigung und Kennwortzurücksetzung finden Sie unter https://go.microsoft.com/fwlink/?LinkID=320771
            ' E-Mail mit dem Code und die Umleitung zur Seite zum Zurücksetzen des Kennworts senden
            ' Dim code = manager.GeneratePasswordResetToken(user.Id)
            ' Dim callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request)
            ' manager.SendEmail(user.Id, "Kennwort zurücksetzen", "Bitte setzen Sie Ihr Kennwort zurück. Klicken Sie dazu <a href=""" & callbackUrl & """>hier</a>.")
            loginForm.Visible = False
            DisplayEmail.Visible = True
        End If
    End Sub
End Class