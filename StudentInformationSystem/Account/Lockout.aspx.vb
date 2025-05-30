Namespace StudentInformationSystem.Account
    Partial Public Class Lockout
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            ' Clear the session and log out
            Session.Clear()
            Session.Abandon()

            ' Redirect to Default.aspx
            Response.Redirect("~/Default.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Sub
    End Class
End Namespace
