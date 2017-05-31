<%@ Page Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
	CodeBehind="MileageForm.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.MileageForm"
	EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc1" %>
<%@ Register Src="Mileage.ascx" TagName="Mileage" TagPrefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
	<uc1:PopupCallback ID="PopupCallback1" runat="server" />
	<uc2:Mileage ID="ctlMileage" runat="server" />
</asp:Content>
