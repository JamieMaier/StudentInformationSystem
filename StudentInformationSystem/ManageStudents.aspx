<%@ Page Title="Manage Students" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStudents.aspx.vb" Inherits="StudentInformationSystem.ManageStudents" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <%-- Page title --%>
        <h2 class="text-center mb-4">Manage Students</h2>

        <%-- Label for displaying messages (success, error) --%>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block text-center" />

        <%-- Input form for student data --%>
        <div class="mb-3 row">
            <%-- First Name input --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First Name" />
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                    ErrorMessage="First Name is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- Last Name input --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last Name" />
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                    ErrorMessage="Last Name is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- Email input (email format) --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- Enrollment Date input (date picker) --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtEnrollmentDate" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtEnrollmentDate"
                    ErrorMessage="Date is required." CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <%-- CRUD buttons for student management --%>
        <div class="mb-3">
            <%-- Add new student --%>
            <asp:Button ID="btnAddStudent" runat="server" CssClass="btn btn-success" Text="Add Student"
                OnClientClick="return confirm('Are you sure you want to add this student?');"
                OnClick="btnAddStudent_Click" />

            <%-- Update selected student (disabled by default) --%>
            <asp:Button ID="btnUpdateStudent" runat="server" CssClass="btn btn-warning" Text="Update Student"
                OnClientClick="return confirm('Yes, I want to update this student?');"
                OnClick="btnUpdateStudent_Click" Enabled="False" />

            <%-- Delete selected student (disabled by default) --%>
            <asp:Button ID="btnDeleteStudent" runat="server" CssClass="btn btn-danger" Text="Delete Student"
                OnClientClick="return confirm('Are you sure you want to delete this student?');"
                OnClick="btnDeleteStudent_Click" Enabled="False" />

            <%-- Clear input fields --%>
            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear Fields"
                OnClientClick="return confirm('Yes, I want to clear the fields?');"
                OnClick="btnClear_Click" />
        </div>

        <%-- Hidden field to track selected student ID --%>
        <asp:HiddenField ID="hfStudentId" runat="server" />

        <%-- GridView for displaying all students --%>
        <asp:GridView ID="gvStudents" runat="server" CssClass="table table-striped table-bordered"
                      AutoGenerateColumns="False" OnSelectedIndexChanged="gvStudents_SelectedIndexChanged"
                      DataKeyNames="id">
            <Columns>
                <%-- Student ID column --%>
                <asp:BoundField DataField="id" HeaderText="ID" />
                <%-- First Name column --%>
                <asp:BoundField DataField="first_name" HeaderText="First Name" />
                <%-- Last Name column --%>
                <asp:BoundField DataField="last_name" HeaderText="Last Name" />
                <%-- Email column --%>
                <asp:BoundField DataField="email" HeaderText="Email" />
                <%-- Enrollment Date column, formatted as yyyy-MM-dd --%>
                <asp:BoundField DataField="enrollment_date" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                <%-- Select button for editing/deleting --%>
                <asp:CommandField ShowSelectButton="True" SelectText="Select" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
