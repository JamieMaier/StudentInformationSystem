<%@ Page Language="VB" AutoEventWireup="false" %>

<script runat="server">
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' Kill the session
        Session.Clear()
        Session.Abandon()
        Session("username") = Nothing

        ' Extra: clear session cookie (in case it sticks)
        If Not Request.Cookies("ASP.NET_SessionId") Is Nothing Then
            Dim cookie As New HttpCookie("ASP.NET_SessionId")
            cookie.Expires = DateTime.Now.AddDays(-1)
            Response.Cookies.Add(cookie)
        End If

        ' Force redirect — bulletproof way
        Response.BufferOutput = True
        Response.Redirect("~/Default.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
</script>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Logging out...</title>
</head>
<body>
    <p>Logging you out... redirecting.</p>
</body>
</html>
