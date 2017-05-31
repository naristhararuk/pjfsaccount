<%@ Page 
    Language="C#" 
    MasterPageFile="~/ProgramsPages.Master" 
    AutoEventWireup="true" 
    CodeBehind="Search.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Search" 
    Title="Untitled Page" 
    StylesheetTheme="Default" meta:resourcekey="PageResource1"
    EnableTheming="true"
%>
<%@ Register src="UserControls/InboxSearchResult/SearchResultCriteria.ascx" tagname="SearchResultCriteria" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:SearchResultCriteria ID="SearchResultCriteria1" runat="server" />
</asp:Content>
