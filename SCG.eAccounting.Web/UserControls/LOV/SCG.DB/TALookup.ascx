<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TALookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.TALookup" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlTALookupPopupCaller" runat="server" Hide="true" Width="620" OnNotifyPopupResult="ctlTALookupPopupCaller_NotifyPopupResult"/>