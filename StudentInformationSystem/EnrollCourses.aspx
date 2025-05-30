<%@ Page Title="Enroll Courses" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="EnrollCourses.aspx.vb" Inherits="StudentInformationSystem.EnrollCourses" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Heading for the page --%>
    <h2>Available Courses</h2>

    <%-- GridView to display list of courses --%>
    <asp:GridView 
        ID="CoursesGrid" 
        runat="server" 
        AutoGenerateColumns="False" 
        DataKeyNames="course_id" 
        OnRowCommand="CoursesGrid_RowCommand" 
        CssClass="table table-striped">
        
        <%-- Define specific columns to be displayed --%>
        <Columns>
            <%-- Display course name --%>
            <asp:BoundField DataField="course_name" HeaderText="Course Name" />
            <%-- Display format (e.g., online, in-person) --%>
            <asp:BoundField DataField="format" HeaderText="Format" />
            <%-- Display name of the instructor --%>
            <asp:BoundField DataField="instructor" HeaderText="Instructor" />
            <%-- Button to trigger enrollment --%>
            <asp:ButtonField ButtonType="Button" CommandName="Enroll" Text="Enroll" />
        </Columns>
    </asp:GridView>

    <br />

    <%-- Label to show success or error messages --%>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />

</asp:Content>
