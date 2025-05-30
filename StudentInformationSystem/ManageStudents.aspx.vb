Imports Npgsql

' Backend logic for managing student records in the system
Public Class ManageStudents
    Inherits Page

    ' Read connection string from Web.config
    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

    ' Page load handler – executed only on first load (not postbacks)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtEnrollmentDate.Text = Date.Today.ToString("yyyy-MM-dd") ' Default date input to today
            LoadGrid() ' Populate the GridView with student records
        End If
    End Sub

    ' Loads student data from the database into the GridView
    Private Sub LoadGrid()
        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("SELECT id, first_name, last_name, email, enrollment_date FROM students ORDER BY id", conn)
            Dim da As New NpgsqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            gvStudents.DataSource = dt
            gvStudents.DataBind()
        End Using
    End Sub

    ' Adds a new student record
    Protected Sub btnAddStudent_Click(sender As Object, e As EventArgs)
        If Not ValidateInput() Then Exit Sub ' Skip if validation fails

        Dim enrollmentDate As Date = Date.Parse(txtEnrollmentDate.Text)

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("INSERT INTO students (first_name, last_name, email, enrollment_date) VALUES (@first, @last, @mail, @date)", conn)
            cmd.Parameters.AddWithValue("@first", txtFirstName.Text)
            cmd.Parameters.AddWithValue("@last", txtLastName.Text)
            cmd.Parameters.AddWithValue("@mail", txtEmail.Text)
            cmd.Parameters.AddWithValue("@date", enrollmentDate)
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl) ' Reload page to reflect changes
    End Sub

    ' Updates an existing student record
    Protected Sub btnUpdateStudent_Click(sender As Object, e As EventArgs)
        If hfStudentId.Value = "" Then Exit Sub
        If Not ValidateInput() Then Exit Sub

        Dim enrollmentDate As Date = Date.Parse(txtEnrollmentDate.Text)

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("UPDATE students SET first_name=@first, last_name=@last, email=@mail, enrollment_date=@date WHERE id=@id", conn)
            cmd.Parameters.AddWithValue("@first", txtFirstName.Text)
            cmd.Parameters.AddWithValue("@last", txtLastName.Text)
            cmd.Parameters.AddWithValue("@mail", txtEmail.Text)
            cmd.Parameters.AddWithValue("@date", enrollmentDate)
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfStudentId.Value))
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl)
    End Sub

    ' Deletes the selected student record
    Protected Sub btnDeleteStudent_Click(sender As Object, e As EventArgs)
        If hfStudentId.Value = "" Then Exit Sub

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()
            Dim cmd As New NpgsqlCommand("DELETE FROM students WHERE id=@id", conn)
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfStudentId.Value))
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect(Request.RawUrl)
    End Sub

    ' Clears all input fields and resets UI state
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    ' Helper method to reset all form fields and UI elements
    Private Sub ClearForm()
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtEmail.Text = ""
        txtEnrollmentDate.Text = Date.Today.ToString("yyyy-MM-dd")
        hfStudentId.Value = ""
        btnUpdateStudent.Enabled = False
        btnDeleteStudent.Enabled = False
        lblMessage.Text = ""
        lblMessage.CssClass = ""
    End Sub

    ' Handles row selection from the GridView and fills the input fields with selected student data
    Protected Sub gvStudents_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim row As GridViewRow = gvStudents.SelectedRow
        hfStudentId.Value = row.Cells(0).Text
        txtFirstName.Text = row.Cells(1).Text
        txtLastName.Text = row.Cells(2).Text
        txtEmail.Text = row.Cells(3).Text
        txtEnrollmentDate.Text = DateTime.Parse(row.Cells(4).Text).ToString("yyyy-MM-dd")
        btnUpdateStudent.Enabled = True
        btnDeleteStudent.Enabled = True
    End Sub

    ' Validates form inputs before database operations
    Private Function ValidateInput() As Boolean
        ' First name required
        If String.IsNullOrWhiteSpace(txtFirstName.Text) Then Return False
        ' Last name required
        If String.IsNullOrWhiteSpace(txtLastName.Text) Then Return False
        ' Email required and must match a simple email pattern
        If String.IsNullOrWhiteSpace(txtEmail.Text) OrElse Not Regex.IsMatch(txtEmail.Text, "^[^@\s]+@[^@\s]+\.[^@\s]+$") Then Return False
        ' Date required and must be a valid date
        If String.IsNullOrWhiteSpace(txtEnrollmentDate.Text) OrElse Not Date.TryParse(txtEnrollmentDate.Text, New Date()) Then Return False
        Return True
    End Function
End Class
