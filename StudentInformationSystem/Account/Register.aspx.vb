Imports System.Security.Cryptography
Imports Npgsql

Partial Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub CreateUser_Click(sender As Object, e As EventArgs)
        If Page.IsValid Then
            Dim usernameInput As String = Email.Text.Trim()
            Dim passwordInput As String = Password.Text.Trim()
            Dim roleInput As String = Role.SelectedValue.Trim().ToLower() ' Expected: "admin" or "user"

            Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection")?.ConnectionString

            If String.IsNullOrEmpty(connStr) Then
                ErrorMessage.Text = "Connection string 'SupabaseConnection' not found."
                Return
            End If

            Try
                Using conn As New NpgsqlConnection(connStr)
                    conn.Open()

                    ' 🔒 Only require student email if registering as user
                    If roleInput = "user" Then
                        Dim checkStudentCmd As New NpgsqlCommand("SELECT COUNT(*) FROM students WHERE email = @e", conn)
                        checkStudentCmd.Parameters.AddWithValue("@e", usernameInput)
                        Dim studentExists As Integer = CInt(checkStudentCmd.ExecuteScalar())

                        If studentExists = 0 Then
                            ErrorMessage.Text = "This email is not recognized as a student. Please contact an admin or use your student email."
                            Return
                        End If
                    End If

                    ' ✅ Check for existing user
                    Dim checkUserCmd As New NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @u", conn)
                    checkUserCmd.Parameters.AddWithValue("@u", usernameInput)
                    Dim userExists As Integer = CInt(checkUserCmd.ExecuteScalar())

                    If userExists > 0 Then
                        ErrorMessage.Text = "This email is already registered."
                        Return
                    End If

                    ' ✅ Hash password securely
                    Dim hashedPassword As String = HashPassword(passwordInput)

                    ' ✅ Create new user
                    Dim insertUserCmd As New NpgsqlCommand("INSERT INTO users (username, password_hash, role) VALUES (@u, @p, @r)", conn)
                    insertUserCmd.Parameters.AddWithValue("@u", usernameInput)
                    insertUserCmd.Parameters.AddWithValue("@p", hashedPassword)
                    insertUserCmd.Parameters.AddWithValue("@r", roleInput)
                    insertUserCmd.ExecuteNonQuery()

                    ' ✅ Success: redirect
                    Response.Redirect("~/Account/Login.aspx")
                End Using

            Catch ex As Exception
                ErrorMessage.Text = "Registration failed: " & ex.Message
            End Try
        End If
    End Sub

    ' 🔐 Hash the password using PBKDF2 with salt
    Private Function HashPassword(password As String) As String
        Dim salt(15) As Byte ' 128-bit salt
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using

        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, 10000)
            Dim hash = pbkdf2.GetBytes(32)
            Return Convert.ToBase64String(salt) & ":" & Convert.ToBase64String(hash)
        End Using
    End Function
End Class
