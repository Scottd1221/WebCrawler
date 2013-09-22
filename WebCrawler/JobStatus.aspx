<%@ Page Language="C#" MasterPageFile="~/WebCrawlerSite.Master" AutoEventWireup="true" CodeBehind="JobStatus.aspx.cs" Inherits="WebCrawler.JobStatus" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div id="SearchJob" style="text-align: right:">
        <asp:TextBox ID="SearchTextBox" runat="server" Width="300px" OnTextChanged="SearchTextBox_TextChanged"></asp:TextBox>
        <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchJob"></asp:Button>
    </div>
    <div id="JobTable">
        <asp:GridView runat="server" ID="RunsGridView" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="15" AllowPaging="True" ClientIDMode="Static" Width="99%" OnPageIndexChanging="RunsGridView_PageIndexChanging" OnRowDataBound="RunsGridView_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" Height="30px" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>
        <div id="blankDiv"><a href="Index.aspx"><span></span></a>&nbsp;&nbsp;&nbsp; </div>
    <div id="indexDiv" style="font-size: medium; background-color: #008080">
        <a href="Index.aspx"><span style="color:white; border:solid; margin-left:auto; margin-right:auto">View the Index of the Resources</span></a></div>
</asp:Content>
