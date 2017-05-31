<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserGroupLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserGroupLookup" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlUserGroupLookupPopupCaller" runat="server" Hide="true" Width="650" Height="500" OnNotifyPopupResult="ctlUserGroupLookupPopupCaller_NotifyPopupResult"/>
