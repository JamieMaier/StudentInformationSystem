Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Owin

Partial Public Class Startup

    ' Weitere Informationen zur Konfiguration der Authentifizierung finden Sie unter https://go.microsoft.com/fwlink/?LinkId=301883
    Public Sub ConfigureAuth(app As IAppBuilder)
        'Konfigurieren des db-Kontexts, des Benutzer-Managers und des Anmelde-Managers für die Verwendung einer einzelnen Instanz pro Anforderung.
        app.CreatePerOwinContext(AddressOf ApplicationDbContext.Create)
        app.CreatePerOwinContext(Of ApplicationUserManager)(AddressOf ApplicationUserManager.Create)
        app.CreatePerOwinContext(Of ApplicationSignInManager)(AddressOf ApplicationSignInManager.Create)

        ' Anwendung für die Verwendung eines Cookies zum Speichern von Informationen für den angemeldeten Benutzer aktivieren
        app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            .Provider = New CookieAuthenticationProvider() With {
                .OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(Of ApplicationUserManager, ApplicationUser)(
                    validateInterval:=TimeSpan.FromMinutes(30),
                    regenerateIdentity:=Function(manager, user) user.GenerateUserIdentityAsync(manager))},
            .LoginPath = New PathString("/Account/Login")})
        ' Ein Cookie zum vorübergehenden Speichern von Informationen zu einem Benutzer verwenden, der sich mit dem Anmeldeanbieter eines Drittanbieters anmeldet
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' Aktiviert die Anwendung für das vorübergehende Speichern von Benutzerinformationen beim Überprüfen der zweiten Stufe im zweistufigen Authentifizierungsvorgang.
        app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5))

        ' Aktiviert die Anwendung für das Speichern der zweiten Anmeldeüberprüfungsstufe (z. B. Telefon oder E-Mail).
        ' Wenn Sie diese Option aktivieren, wird Ihr zweiter Überprüfungsschritt während des Anmeldevorgangs auf dem Gerät gespeichert, von dem aus Sie sich angemeldet haben.
        ' Dies ähnelt der RememberMe-Option bei der Anmeldung.
        app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie)

        ' Auskommentierung der folgenden Zeilen aufheben, um die Anmeldung mit Anmeldeanbietern von Drittanbietern zu ermöglichen
        'app.UseMicrosoftAccountAuthentication(
        '    clientId:= "",
        '    clientSecret:= "")

        'app.UseTwitterAuthentication(
        '   consumerKey:= "",
        '   consumerSecret:= "")

        'app.UseFacebookAuthentication(
        '   appId:= "",
        '   appSecret:= "")

        'app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
        '   .ClientId = "",
        '   .ClientSecret = ""})
    End Sub
End Class
