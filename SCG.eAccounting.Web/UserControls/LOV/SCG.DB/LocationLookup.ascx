<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationLookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationLookup" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<asp:HiddenField ID="ctlCompanyId" runat="server" />
<uc1:PopupCaller ID="ctlLocationLookupPopupCaller" runat="server" Hide="true" Width="620" OnNotifyPopupResult="ctlLocationLookupPopupCaller_NotifyPopupResult"/>