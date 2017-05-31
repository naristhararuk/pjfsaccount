<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Invoice" EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register src="../../Shared/PopupCallback.ascx" tagname="PopupCallback" tagprefix="uc1" %>
<%@ Register src="InvoiceForm.ascx" tagname="InvoiceForm" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <uc1:PopupCallback ID="PopupCallback1" runat="server" />
<uc2:InvoiceForm ID="ctlInvoiceForm" runat="server" />
</asp:Content>
