<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    EnableTheming="true" CodeBehind="ExpenseFormFR.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ExpenseFormFR"
    StylesheetTheme="Default" EnableEventValidation="false" %>

<%@ Register Src="ExpenseForm.ascx" TagName="ExpenseForm" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:ExpenseForm ID="X" runat="server" Mode="2" />
</asp:Content>
