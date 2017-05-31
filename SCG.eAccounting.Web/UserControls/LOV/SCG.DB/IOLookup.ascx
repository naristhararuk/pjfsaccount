<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="IOLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.IOLookup" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>
<uc1:PopupCaller ID="ctlIOLookupPopupCaller" runat="server" Hide="true" Width="640" OnNotifyPopupResult="ctlIOLookupPopupCaller_NotifyPopupResult"/>
<asp:Label ID="ctlMode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCompanyCode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCompanyId" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCostCenterCode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCostCenterId" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlIONumber" runat="server" Style="display: none;" />
