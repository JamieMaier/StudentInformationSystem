Imports System.Security.Principal
Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        If HttpContext.Current.Request.IsAuthenticated Then
            Dim authCookie As HttpCookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)

            If authCookie IsNot Nothing Then
                Dim authTicket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)

                If authTicket IsNot Nothing AndAlso Not authTicket.Expired Then
                    Dim identity As New FormsIdentity(authTicket)
                    Dim principal As New GenericPrincipal(identity, Nothing)
                    HttpContext.Current.User = principal
                End If
            End If
        End If
    End Sub
End Class
