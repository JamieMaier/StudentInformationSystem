Imports Npgsql

' Code-behind for the ManageEnrollments page, used to view and remove student enrollments
Partial Public Class ManageEnrollments
    Inherits System.Web.UI.Page

    ' Runs when the page is loaded
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Only allow access if the user is an admin
            If Session("role") Is Nothing OrElse Session("role").ToString().Trim().ToLower() <> "admin" Then
                lblMessage.Text = "Access denied. Admins only."
                EnrollmentsGrid.Visible = False
                Return
            End If

            ' Load all enrollments from the database
            LoadEnrollments()
        End If
    End Sub

    ' Loads enrollment data from the database and binds it to the GridView
    Private Sub LoadEnrollments()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()

            ' SQL query to fetch enrolled students with their course details
            Dim sql As String = "
                SELECT 
                    e.student_id,
                    e.course_id,
                    s.first_name, 
                    s.last_name, 
                    s.email AS student_email, 
                    c.course_name, 
                    e.enrollment_date
                FROM enrollments e
                INNER JOIN students s ON e.student_id = s.id
                INNER JOIN courses c ON e.course_id = c.course_id
                ORDER BY e.enrollment_date DESC;"

            Using cmd As New NpgsqlCommand(sql, conn)
                Using reader = cmd.ExecuteReader()
                    ' Load result into a DataTable and bind it to the GridView
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    EnrollmentsGrid.DataSource = dt
                    EnrollmentsGrid.DataBind()
                End Using
            End Using
        End Using
    End Sub

    ' Triggered when the "Unenroll" button is clicked in the GridView
    Protected Sub EnrollmentsGrid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        ' Retrieve student and course IDs from the selected row
        Dim studentId As Integer = Convert.ToInt32(EnrollmentsGrid.DataKeys(e.RowIndex).Values("student_id"))
        Dim courseId As Integer = Convert.ToInt32(EnrollmentsGrid.DataKeys(e.RowIndex).Values("course_id"))

        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()

            ' SQL command to delete the specific enrollment record
            Dim deleteSql As String = "DELETE FROM enrollments WHERE student_id = @student_id AND course_id = @course_id"

            Using cmd As New NpgsqlCommand(deleteSql, conn)
                cmd.Parameters.AddWithValue("@student_id", studentId)
                cmd.Parameters.AddWithValue("@course_id", courseId)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                ' Show confirmation message if deletion was successful
                If rowsAffected > 0 Then
                    lblMessage.ForeColor = System.Drawing.Color.Green
                    lblMessage.Text = "Student successfully unenrolled."
                Else
                    lblMessage.ForeColor = System.Drawing.Color.Red
                    lblMessage.Text = "Unenroll failed. Please try again."
                End If
            End Using
        End Using

        ' Reload the updated list of enrollments
        LoadEnrollments()
    End Sub
End Class
