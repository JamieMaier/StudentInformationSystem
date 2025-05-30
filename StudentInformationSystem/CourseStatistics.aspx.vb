Imports Npgsql

' Backend code for the CourseStatistics.aspx page
Partial Public Class CourseStatistics
    Inherits System.Web.UI.Page

    ' On first load (not postback), trigger both chart generation methods
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadBarChart()
            LoadPieChart()
        End If
    End Sub

    ' Generates bar chart: Number of students per course
    Private Sub LoadBarChart()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Dim courseNames As New List(Of String)()
        Dim studentCounts As New List(Of Integer)()

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()

            ' Query: count students per course
            Dim cmd As New NpgsqlCommand("
                SELECT c.course_name, COUNT(e.student_id) AS student_count
                FROM courses c
                LEFT JOIN enrollments e ON c.course_id = e.course_id
                GROUP BY c.course_name
                ORDER BY c.course_name;", conn)

            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    courseNames.Add(reader("course_name").ToString())
                    studentCounts.Add(Convert.ToInt32(reader("student_count")))
                End While
            End Using
        End Using

        ' Dynamically build JS for Chart.js bar chart
        Dim js As New StringBuilder()
        js.AppendLine("const ctxBar = document.getElementById('barChart').getContext('2d');")
        js.AppendLine("new Chart(ctxBar, {")
        js.AppendLine("type: 'bar',")
        js.AppendLine("data: {")
        js.AppendLine($"labels: [{String.Join(",", courseNames.Select(Function(c) $"""{c}"""))}],")
        js.AppendLine("datasets: [{ label: 'Students', data: [" & String.Join(",", studentCounts) & "], backgroundColor: 'rgba(54, 162, 235, 0.6)', borderColor: 'rgba(54, 162, 235, 1)', borderWidth: 1 }]")
        js.AppendLine("}, options: { scales: { y: { beginAtZero: true }}}});")

        ' Register script to render the chart on client side
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "barChartScript", js.ToString(), True)
    End Sub

    ' Generates pie chart: Course format distribution
    Private Sub LoadPieChart()
        Dim formats As New List(Of String)()
        Dim counts As New List(Of Integer)()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

        Using conn As New NpgsqlConnection(connStr)
            conn.Open()

            ' Query: count how many courses are of each format
            Dim cmd As New NpgsqlCommand("SELECT format, COUNT(*) AS cnt FROM courses GROUP BY format", conn)

            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    formats.Add(reader("format").ToString())
                    counts.Add(Convert.ToInt32(reader("cnt")))
                End While
            End Using
        End Using

        ' Dynamically build JS for Chart.js pie chart
        Dim js As New StringBuilder()
        js.AppendLine("const ctxPie = document.getElementById('formatChart').getContext('2d');")
        js.AppendLine("new Chart(ctxPie, {")
        js.AppendLine("type: 'pie',")
        js.AppendLine("data: {")
        js.AppendLine($"labels: [{String.Join(",", formats.Select(Function(f) $"""{f}"""))}],")
        js.AppendLine("datasets: [{ data: [" & String.Join(",", counts) & "], backgroundColor: ['#ff6384','#36a2eb','#ffce56'] }]")
        js.AppendLine("}});")

        ' Register script to render the chart on client side
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "pieChartScript", js.ToString(), True)
    End Sub
End Class
