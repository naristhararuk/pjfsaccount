<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationUserLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationUserLookup" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>
<asp:HiddenField ID="ctlCompanyId" runat="server" />
<uc1:PopupCaller ID="ctlLocationUserLookupPopupCaller" runat="server" Hide="true" Width="620" Height="480" OnNotifyPopupResult="ctlLocationUserLookupPopupCaller_NotifyPopupResult"/>