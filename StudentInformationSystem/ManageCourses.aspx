<%@ Page Title="Manage Courses" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCourses.aspx.vb" Inherits="StudentInformationSystem.ManageCourses" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <%-- Page Heading --%>
        <h2 class="text-center mb-4">Manage Courses</h2>

        <%-- Message label for success or error messages --%>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block text-center" />

        <%-- Input fields for course details --%>
        <div class="mb-3 row">
            <%-- Course Name input --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" placeholder="Course Name" />
                <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" ControlToValidate="txtCourseName"
                    ErrorMessage="Course Name is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- ECTS input --%>
            <div class="col-md-2">
                <asp:TextBox ID="txtEcts" runat="server" CssClass="form-control" placeholder="ECTS" />
                <asp:RequiredFieldValidator ID="rfvEcts" runat="server" ControlToValidate="txtEcts"
                    ErrorMessage="ECTS is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- Hours input --%>
            <div class="col-md-2">
                <asp:TextBox ID="txtHours" runat="server" CssClass="form-control" placeholder="Hours" />
                <asp:RequiredFieldValidator ID="rfvHours" runat="server" ControlToValidate="txtHours"
                    ErrorMessage="Hours are required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <%-- Format input (optional) --%>
            <div class="col-md-2">
                <asp:TextBox ID="txtFormat" runat="server" CssClass="form-control" placeholder="Format (e.g. online)" />
            </div>

            <%-- Instructor input (optional) --%>
            <div class="col-md-3">
                <asp:TextBox ID="txtInstructor" runat="server" CssClass="form-control" placeholder="Instructor" />
            </div>
        </div>

        <%-- Action buttons --%>
        <div class="mb-3">
            <%-- Add Course button with confirmation --%>
            <asp:Button ID="btnAddCourse" runat="server" CssClass="btn btn-success" Text="Add Course"
                OnClientClick="return confirm('Are you sure you want to add this course?');"
                OnClick="btnAddCourse_Click" />

            <%-- Update Course button, initially disabled --%>
            <asp:Button ID="btnUpdateCourse" runat="server" CssClass="btn btn-warning" Text="Update Course"
                OnClientClick="return confirm('Yes, I want to update this course?');"
                OnClick="btnUpdateCourse_Click" Enabled="False" />

            <%-- Delete Course button, initially disabled --%>
            <asp:Button ID="btnDeleteCourse" runat="server" CssClass="btn btn-danger" Text="Delete Course"
                OnClientClick="return confirm('Are you sure you want to delete this course?');"
                OnClick="btnDeleteCourse_Click" Enabled="False" />

            <%-- Clear Fields button --%>
            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear Fields"
                OnClientClick="return confirm('Yes, I want to clear the fields?');"
                OnClick="btnClear_Click" />
        </div>

        <%-- Hidden field to store selected course ID --%>
        <asp:HiddenField ID="hfCourseId" runat="server" />

        <%-- GridView to display all courses --%>
        <asp:GridView ID="gvCourses" runat="server" CssClass="table table-striped table-bordered"
                      AutoGenerateColumns="False" OnSelectedIndexChanged="gvCourses_SelectedIndexChanged"
                      DataKeyNames="course_id">
            <Columns>
                <%-- Course ID column --%>
                <asp:BoundField DataField="course_id" HeaderText="ID" />
                <%-- Course Name column --%>
                <asp:BoundField DataField="course_name" HeaderText="Course Name" />
                <%-- ECTS column --%>
                <asp:BoundField DataField="ects" HeaderText="ECTS" />
                <%-- Hours column --%>
                <asp:BoundField DataField="hours" HeaderText="Hours" />
                <%-- Format column --%>
                <asp:BoundField DataField="format" HeaderText="Format" />
                <%-- Instructor column --%>
                <asp:BoundField DataField="instructor" HeaderText="Instructor" />
                <%-- Select button for editing or deleting --%>
                <asp:CommandField ShowSelectButton="True" SelectText="Select" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
