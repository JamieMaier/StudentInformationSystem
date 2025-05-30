<%@ Page Language="VB" AutoEventWireup="true" %>

<script runat="server">
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' Clear user-specific session data
        Session("username") = Nothing
        Session("role") = Nothing
        Session.Clear()     ' Clear all session variables
        Session.Abandon()   ' Abandon the current session completely

        ' Manually expire the ASP.NET session cookie to ensure a new session is created on next request
        If Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
            Dim cookie As New HttpCookie("ASP.NET_SessionId")
            cookie.Expires = DateTime.Now.AddYears(-1)  ' Set expiration in the past
            Response.Cookies.Add(cookie)
        End If

        ' Redirect the user to the homepage (Default.aspx) after logout
        ' "False" prevents Response.Redirect from ending the thread immediately
        Response.Redirect("~/Default.aspx", False)
        
        ' Ensures the request is properly completed after the redirect
        Context.ApplicationInstance.CompleteRequest()
    End Sub
</script>
