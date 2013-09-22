<%@ Page Language="C#" MasterPageFile="~/WebCrawlerSite.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebCrawler.Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
            <div>
                <asp:Panel ID="WelcomePanel" runat="server" BackColor="#3366CC" ForeColor="#FFFFCC">
                    Welcome to the Web Crawler, I am growing
                </asp:Panel>
            </div>
            <asp:Panel ID="DataPanel" runat="server">
                Please input the URL
            </asp:Panel>
            <p>
                <asp:TextBox ID="TextBoxUrl" runat="server" Width="420px"></asp:TextBox>
            </p>
            <p>
                Please input the depth
            </p>
            <asp:TextBox ID="TextBoxDepth" runat="server" Width="418px"></asp:TextBox>
            <p>
                <asp:Button ID="StartCrawler" runat="server" OnClick="StartCrawlerClick" Text="StartCrawler" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </p>
</asp:Content>
