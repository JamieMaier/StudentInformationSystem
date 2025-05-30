' Master page class for the site, providing shared functionality across all pages
Public Class SiteMaster
    Inherits MasterPage

    ' Constants used for Anti-XSRF protection
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"

    ' Variable to store the current Anti-XSRF token value
    Private _antiXsrfTokenValue As String

    ' Event handler for page initialization
    Protected Sub Page_Init(sender As Object, e As EventArgs)
        ' Attempt to retrieve the Anti-XSRF token from the request cookie
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid

        ' If the token is valid, use it and set it as the ViewStateUserKey
        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' If no valid token exists, generate a new one and store it in a new HttpOnly cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie As New HttpCookie(AntiXsrfTokenKey) With {
                .HttpOnly = True,
                .Value = _antiXsrfTokenValue
            }

            ' Set cookie to be secure if SSL is required and the request is secure
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
                responseCookie.Secure = True
            End If

            ' Add the cookie to the response
            Response.Cookies.[Set](responseCookie)
        End If

        ' Register PreLoad event handler for further XSRF validation
        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    ' Event handler for PreLoad stage of the page lifecycle
    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            ' On first load, store the Anti-XSRF token and username in ViewState
            ViewState(AntiXsrfTokenKey) = _antiXsrfTokenValue
            ViewState(AntiXsrfUserNameKey) = If(Session("username"), String.Empty)
        Else
            ' On postback, validate that the stored token and username match expected values
            If DirectCast(ViewState(AntiXsrfTokenKey), String) <> _antiXsrfTokenValue OrElse
               DirectCast(ViewState(AntiXsrfUserNameKey), String) <> (If(Session("username"), String.Empty)) Then
                Throw New InvalidOperationException("Error validating Anti-XSRF token.")
            End If
        End If
    End Sub

    ' Event handler for Page Load event
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Role-based menu visibility setup
            If Session("role") IsNot Nothing Then
                Dim role As String = Session("role").ToString().Trim().ToLower()

                ' Find the relevant placeholder controls on the master page
                Dim phAdminEnrollments As PlaceHolder = CType(FindControl("phAdminEnrollments"), PlaceHolder)
                Dim phStudentEnroll As PlaceHolder = CType(FindControl("phStudentEnroll"), PlaceHolder)

                ' Show/hide controls based on the user's role
                If role = "admin" Then
                    If phAdminEnrollments IsNot Nothing Then phAdminEnrollments.Visible = True
                    If phStudentEnroll IsNot Nothing Then phStudentEnroll.Visible = False
                ElseIf role = "user" Then
                    If phAdminEnrollments IsNot Nothing Then phAdminEnrollments.Visible = False
                    If phStudentEnroll IsNot Nothing Then phStudentEnroll.Visible = True
                End If
            End If
        End If
    End Sub
End Class
