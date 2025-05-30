Namespace StudentInformationSystem
    Partial Public Class Logout
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            ' Clear session
            Session("username") = Nothing
            Session("role") = Nothing
            Session.Clear()
            Session.Abandon()

            ' Ensure session ID is reset
            If Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
                Dim cookie As New HttpCookie("ASP.NET_SessionId")
                cookie.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie)
            End If

            ' Critical: redirect and finish the request
            Response.Redirect("~/Default.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Sub
    End Class
End Namespace
