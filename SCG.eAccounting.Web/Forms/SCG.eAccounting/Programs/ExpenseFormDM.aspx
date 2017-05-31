<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ExpenseFormDM.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ExpenseFormDM"
    Title="Untitled Page" StylesheetTheme="Default" meta:resourcekey="PageResource1"
    EnableTheming="true" EnableEventValidation="false" %>

<%@ Register Src="ExpenseForm.ascx" TagName="ExpenseForm" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:ExpenseForm ID="X" runat="server" Mode="1"/>
</asp:Content>
