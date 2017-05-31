<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyLookUp.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CompanyLookUp" EnableTheming="true"%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlCompanyLookupPopupCaller" runat="server" Hide="true" Width="610" Height="480" OnNotifyPopupResult="ctlCompanyLookupPopupCaller_NotifyPopupResult"/>
