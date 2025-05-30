<%@ Page Title="Home" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="StudentInformationSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Page-specific CSS styles for the homepage --%>
    <style>
        /* Full-screen hero section with background image and centered content */
        .hero-section {
            position: relative;
            background-image: url('https://images.unsplash.com/photo-1571260899304-425eee4c7efc'); /* Background image URL */
            background-size: cover;         /* Cover entire section */
            background-position: center;    /* Center the image */
            height: 90vh;                   /* Take 90% of the viewport height */
            display: flex;                 /* Flexbox for vertical/horizontal centering */
            align-items: center;
            justify-content: center;
            color: white;                  /* Text color */
        }

        /* Semi-transparent overlay box for the title and subtitle */
        .overlay-box {
            background-color: rgba(0, 0, 0, 0.6); /* Dark translucent background */
            padding: 40px;                        /* Inner spacing */
            border-radius: 10px;                  /* Rounded corners */
            text-align: center;                   /* Centered text */
            max-width: 700px;                     /* Limit box width */
        }

        /* Styling for the main heading */
        .overlay-box h1 {
            font-size: 3rem;
            font-weight: bold;
            margin-bottom: 20px;
        }

        /* Styling for the subtitle/description */
        .overlay-box p {
            font-size: 1.2rem;
        }
    </style>

    <%-- Hero section HTML structure --%>
    <div class="hero-section">
        <div class="overlay-box">
            <%-- Main heading of the homepage --%>
            <h1>Student Information System</h1>
            <%-- Subheading/description text --%>
            <p>Your central place to manage students, courses, and enrollments.</p>
        </div>
    </div>
</asp:Content>
