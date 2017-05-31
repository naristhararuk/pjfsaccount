<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountLookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.AccountLookup" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlExpenseLookupPopupCaller" runat="server" Hide="true" Width="610" Height="480" OnNotifyPopupResult="ctlUserProfileLookupPopupCaller_NotifyPopupResult"/>
