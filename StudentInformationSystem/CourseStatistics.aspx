<%@ Page Title="Course Statistics" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseStatistics.aspx.vb" Inherits="StudentInformationSystem.CourseStatistics" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Course Statistics</h2>

        <%-- Bar Chart --%>
        <div class="my-5">
            <h4>Number of Students per Course</h4>
            <canvas id="barChart" width="800" height="400"></canvas>
        </div>

        <%-- Pie Chart --%>
        <div class="my-5">
            <h4>Course Format Distribution</h4>
            <canvas id="formatChart" width="800" height="400"></canvas>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>
