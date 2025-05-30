<%@ Page Title="Manage Enrollments" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageEnrollments.aspx.vb" Inherits="StudentInformationSystem.ManageEnrollments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <%-- Page Heading --%>
        <h2>Enrolled Students</h2>

        <%-- Label for displaying error/success messages --%>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />

        <%-- GridView to list enrolled students and allow unenrollment --%>
        <asp:GridView ID="EnrollmentsGrid" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-bordered"
                      DataKeyNames="student_id,course_id"
                      OnRowDeleting="EnrollmentsGrid_RowDeleting">
            <Columns>
                <%-- First name of the student --%>
                <asp:BoundField DataField="first_name" HeaderText="First Name" />
                <%-- Last name of the student --%>
                <asp:BoundField DataField="last_name" HeaderText="Last Name" />
                <%-- Student's email address --%>
                <asp:BoundField DataField="student_email" HeaderText="Email" />
                <%-- Name of the enrolled course --%>
                <asp:BoundField DataField="course_name" HeaderText="Course Name" />
                <%-- Date of enrollment, formatted as YYYY-MM-DD --%>
                <asp:BoundField DataField="enrollment_date" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" />
                <%-- Delete (Unenroll) button for each row --%>
                <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="Unenroll" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
