<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="PerdiemPopup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.PerdiemPopup" EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register src="Perdiem.ascx" tagname="Perdiem" tagprefix="uc1" %>
<%@ Register src="../../Shared/PopupCallback.ascx" tagname="PopupCallback" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <uc1:PopupCallback ID="PopupCallback1" runat="server" />
    <uc1:Perdiem ID="ctlPerdiem" runat="server" />
</asp:Content>
