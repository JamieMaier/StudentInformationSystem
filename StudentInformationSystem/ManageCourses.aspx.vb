Imports Npgsql

' Backend logic for managing courses (add, update, delete, select)
Public Class ManageCourses
    Inherits Page

    ' Connection string to the Supabase PostgreSQL database
    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

    ' Initial page load: load course data into GridView
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadGrid() ' Only load data if the page is not a postback (i.e., first load)
        End If
    End Sub

    ' Fetch all courses from the database and bind them to the GridView
    Private Sub LoadGrid()
        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("SELECT course_id, course_name, ects, hours, format, instructor FROM courses ORDER BY course_id", conn)
            Dim da As New NpgsqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            gvCourses.DataSource = dt
            gvCourses.DataBind()
        End Using
    End Sub

    ' Add a new course to the database
    Protected Sub btnAddCourse_Click(sender As Object, e As EventArgs)
        If Not ValidateInput() Then Exit Sub ' Exit if input is invalid

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("INSERT INTO courses (course_name, ects, hours, format, instructor) VALUES (@name, @ects, @hours, @format, @instructor)", conn)
            cmd.Parameters.AddWithValue("@name", txtCourseName.Text)
            cmd.Parameters.AddWithValue("@ects", Convert.ToInt32(txtEcts.Text))
            cmd.Parameters.AddWithValue("@hours", Convert.ToInt32(txtHours.Text))
            cmd.Parameters.AddWithValue("@format", txtFormat.Text)
            cmd.Parameters.AddWithValue("@instructor", txtInstructor.Text)
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl) ' Refresh the page to reflect changes
    End Sub

    ' Update the selected course based on course ID
    Protected Sub btnUpdateCourse_Click(sender As Object, e As EventArgs)
        If hfCourseId.Value = "" Then Exit Sub ' No course selected
        If Not ValidateInput() Then Exit Sub   ' Invalid input

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("UPDATE courses SET course_name=@name, ects=@ects, hours=@hours, format=@format, instructor=@instructor WHERE course_id=@id", conn)
            cmd.Parameters.AddWithValue("@name", txtCourseName.Text)
            cmd.Parameters.AddWithValue("@ects", Convert.ToInt32(txtEcts.Text))
            cmd.Parameters.AddWithValue("@hours", Convert.ToInt32(txtHours.Text))
            cmd.Parameters.AddWithValue("@format", txtFormat.Text)
            cmd.Parameters.AddWithValue("@instructor", txtInstructor.Text)
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfCourseId.Value))
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl) ' Refresh the page
    End Sub

    ' Delete the selected course based on course ID
    Protected Sub btnDeleteCourse_Click(sender As Object, e As EventArgs)
        If hfCourseId.Value = "" Then Exit Sub ' No course selected

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("DELETE FROM courses WHERE course_id=@id", conn)
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfCourseId.Value))
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl) ' Refresh the page
    End Sub

    ' Clear all form fields and reset the UI
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    ' Helper method to reset input fields and UI state
    Private Sub ClearForm()
        txtCourseName.Text = ""
        txtEcts.Text = ""
        txtHours.Text = ""
        txtFormat.Text = ""
        txtInstructor.Text = ""
        hfCourseId.Value = ""
        btnUpdateCourse.Enabled = False
        btnDeleteCourse.Enabled = False
        lblMessage.Text = ""
        lblMessage.CssClass = ""
    End Sub

    ' Triggered when a row is selected in the GridView
    ' Populates form fields with selected course's data
    Protected Sub gvCourses_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim row As GridViewRow = gvCourses.SelectedRow
        hfCourseId.Value = row.Cells(0).Text
        txtCourseName.Text = row.Cells(1).Text
        txtEcts.Text = row.Cells(2).Text
        txtHours.Text = row.Cells(3).Text
        txtFormat.Text = row.Cells(4).Text
        txtInstructor.Text = row.Cells(5).Text
        btnUpdateCourse.Enabled = True
        btnDeleteCourse.Enabled = True
    End Sub

    ' Validates the user input fields
    ' Returns True if valid, otherwise False
    Private Function ValidateInput() As Boolean
        If String.IsNullOrWhiteSpace(txtCourseName.Text) Then Return False
        If String.IsNullOrWhiteSpace(txtEcts.Text) OrElse Not IsNumeric(txtEcts.Text) Then Return False
        If String.IsNullOrWhiteSpace(txtHours.Text) OrElse Not IsNumeric(txtHours.Text) Then Return False
        If String.IsNullOrWhiteSpace(txtFormat.Text) Then Return False
        If String.IsNullOrWhiteSpace(txtInstructor.Text) Then Return False
        Return True
    End Function
End Class
