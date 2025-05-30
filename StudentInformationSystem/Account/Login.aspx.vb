Imports System.Security.Cryptography
Imports Npgsql

' Code-behind for the Login page
Partial Public Class Login
    Inherits System.Web.UI.Page

    ' Runs when the page is first loaded
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Set the URL for the registration link
        RegisterHyperLink.NavigateUrl = "~/Account/Register.aspx"
    End Sub

    ' Handles login button click
    Protected Sub LogIn(sender As Object, e As EventArgs)
        If IsValid Then
            ' Read and sanitize user input
            Dim usernameInput As String = Email.Text.Trim()
            Dim passwordInput As String = Password.Text.Trim()
            Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection")?.ConnectionString

            ' If connection string is missing, show error
            If String.IsNullOrEmpty(connStr) Then
                FailureText.Text = "Connection string 'SupabaseConnection' not found."
                ErrorMessage.Visible = True
                Return
            End If

            Try
                Using conn As New NpgsqlConnection(connStr)
                    conn.Open()

                    ' Query to get the stored password hash and role for the user
                    Dim sql As String = "SELECT password_hash, role FROM users WHERE username = @username"
                    Using cmd As New NpgsqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@username", usernameInput)

                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Dim storedHash As String = reader("password_hash").ToString()
                                Dim role As String = reader("role").ToString()

                                ' Verify password using PBKDF2 hash check
                                If VerifyPassword(passwordInput, storedHash) Then
                                    ' Login success: save user info in session
                                    Session("username") = usernameInput
                                    Session("role") = role.ToLower()
                                    Response.Redirect("~/Default.aspx") ' Redirect to homepage
                                    Return
                                End If
                            End If
                        End Using
                    End Using

                    ' If no match or invalid password
                    FailureText.Text = "Incorrect username or password."
                    ErrorMessage.Visible = True

                End Using

            Catch ex As Exception
                ' Handle and show database or connection error
                FailureText.Text = "Login error: " & ex.Message
                ErrorMessage.Visible = True
            End Try
        End If
    End Sub

    ' Password verification using PBKDF2 hash (Rfc2898DeriveBytes)
    Private Function VerifyPassword(inputPassword As String, storedHash As String) As Boolean
        ' Split the stored hash into salt and actual hash
        Dim parts = storedHash.Split(":"c)
        If parts.Length <> 2 Then Return False

        Dim salt = Convert.FromBase64String(parts(0))
        Dim stored = Convert.FromBase64String(parts(1))

        ' Derive hash from input password using the same salt
        Using pbkdf2 As New Rfc2898DeriveBytes(inputPassword, salt, 10000)
            Dim inputHash = pbkdf2.GetBytes(32) ' 32 bytes = 256-bit hash
            ' Compare byte sequences
            Return stored.SequenceEqual(inputHash)
        End Using
    End Function
End Class
