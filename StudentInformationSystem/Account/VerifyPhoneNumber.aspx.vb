﻿Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin

Partial Public Class VerifyPhoneNumber
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
        Dim phonenum = Request.QueryString("PhoneNumber")
        Dim code = manager.GenerateChangePhoneNumberToken(User.Identity.GetUserId(), phonenum)
        PhoneNumber.Value = phonenum
    End Sub

    Protected Sub Code_Click(sender As Object, e As EventArgs)
        If Not ModelState.IsValid Then
            ModelState.AddModelError("", "Ungültiger Code.")
            Return
        End If

        Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
        Dim signInManager = Context.GetOwinContext().Get(Of ApplicationSignInManager)()

        Dim result = manager.ChangePhoneNumber(User.Identity.GetUserId(), PhoneNumber.Value, Code.Text)

        If result.Succeeded Then
            Dim userInfo = manager.FindById(User.Identity.GetUserId())

            If userInfo IsNot Nothing Then
                signInManager.SignIn(userInfo, isPersistent:=False, rememberBrowser:=False)
                Response.Redirect("/Account/Manage?m=AddPhoneNumberSuccess")
            End If
        End If

        ' Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten. Formular erneut anzeigen.
        ModelState.AddModelError("", "Fehler beim Überprüfen des Telefons.")
    End Sub
End Class
