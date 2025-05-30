Imports System.Net
Imports System.Net.Mail
Imports Npgsql

Partial Public Class EnrollCourses
    Inherits System.Web.UI.Page

    ' Executes when the page is first loaded (not on postback)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadCourses() ' Load the list of available courses into the GridView
        End If
    End Sub

    ' Loads all available courses from the database and binds them to the GridView
    Private Sub LoadCourses()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("SELECT course_id, course_name, format, instructor FROM courses", conn)
            Using reader = cmd.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                CoursesGrid.DataSource = dt
                CoursesGrid.DataBind()
            End Using
        End Using
    End Sub

    ' Handles the "Enroll" button click in the GridView
    Protected Sub CoursesGrid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Enroll" Then
            ' Get index of the clicked row
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            ' Get course ID from DataKeys
            Dim courseId As Integer = Convert.ToInt32(CoursesGrid.DataKeys(rowIndex).Value)
            ' Get user email from session
            Dim userEmail As String = Session("username")

            ' Ensure user is logged in
            If String.IsNullOrEmpty(userEmail) Then
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Text = "Login required."
                Return
            End If

            Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
            Using conn As New NpgsqlConnection(connStr)
                conn.Open()

                ' Get student ID based on the login email (case insensitive)
                Dim getStudentIdCmd As New NpgsqlCommand("SELECT id FROM students WHERE LOWER(email) = LOWER(@e)", conn)
                getStudentIdCmd.Parameters.AddWithValue("@e", userEmail)
                Dim studentIdObj = getStudentIdCmd.ExecuteScalar()

                ' Show error if no matching student is found
                If studentIdObj Is Nothing Then
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Text = "No matching student found for your login email."
                    Return
                End If

                Dim studentId As Integer = CInt(studentIdObj)

                ' Check if student is already enrolled in the course
                Dim checkCmd As New NpgsqlCommand("SELECT COUNT(*) FROM enrollments WHERE student_id = @sid AND course_id = @cid", conn)
                checkCmd.Parameters.AddWithValue("@sid", studentId)
                checkCmd.Parameters.AddWithValue("@cid", courseId)
                If CInt(checkCmd.ExecuteScalar()) > 0 Then
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Text = "You are already enrolled in this course."
                    Return
                End If

                ' Insert the new enrollment record
                Dim insertCmd As New NpgsqlCommand("INSERT INTO enrollments (student_id, course_id, enrollment_date) VALUES (@sid, @cid, CURRENT_DATE)", conn)
                insertCmd.Parameters.AddWithValue("@sid", studentId)
                insertCmd.Parameters.AddWithValue("@cid", courseId)
                insertCmd.ExecuteNonQuery()

                lblMessage.ForeColor = Drawing.Color.Green
                lblMessage.Text = "Successfully enrolled in course!"

                ' Send confirmation email to the student
                SendConfirmationEmail(userEmail, courseId)
            End Using
        End If
    End Sub

    ' Sends a confirmation email to the student after successful enrollment
    Private Sub SendConfirmationEmail(toEmail As String, courseId As Integer)
        ' Set up SMTP client with Brevo credentials and settings
        Dim smtpClient As New SmtpClient("smtp-relay.brevo.com", 587)
        smtpClient.EnableSsl = True
        smtpClient.Credentials = New NetworkCredential("8e4506001@smtp-brevo.com", "dVXRZQKc5sENUJgm")

        Dim courseName As String = "(unknown)"
        ' Retrieve the course name for use in the email body
        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("SELECT course_name FROM courses WHERE course_id = @id", conn)
            cmd.Parameters.AddWithValue("@id", courseId)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                courseName = result.ToString()
            End If
        End Using

        ' Create and configure the email message
        Dim mail As New MailMessage()
        mail.From = New MailAddress("jamie.justin.maier@gmail.com", "Student Information System")
        mail.To.Add(toEmail)
        mail.Subject = "Enrollment Confirmation"
        mail.Body = $"Dear Student," & vbCrLf & vbCrLf &
                    $"You have successfully enrolled in the course: {courseName} on {DateTime.Now:yyyy-MM-dd}." & vbCrLf &
                    "Thank you for using our platform." & vbCrLf & vbCrLf &
                    "Best regards," & vbCrLf & "Your Student System"

        ' Attempt to send the email and display feedback
        Try
            smtpClient.Send(mail)
            lblMessage.Text &= "<br/><span style='color:gray'>📧 Confirmation email sent to " & toEmail & "</span>"
        Catch ex As Exception
            lblMessage.Text &= "<br/><span style='color:gray'>⚠ Email error: " & ex.Message & "</span>"
        End Try
    End Sub

End Class
