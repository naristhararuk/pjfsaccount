<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="TestMasterGrid.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.TestMasterGrid"
    EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/MasterGrid.ascx" TagName="MasterGrid" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:MasterGrid ID="ctlMasterGrid1" runat="server" DataKeyNames="DivisionId" />
</asp:Content>
