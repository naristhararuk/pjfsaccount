<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.AV.AdvanceLookup" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/PopupCaller.ascx" TagName="PopupCaller" TagPrefix="uc1" %>
<uc1:PopupCaller ID="ctlAdvanceLookupPopupCaller" runat="server" Hide="true" Width="640"
    OnNotifyPopupResult="ctlAdvanceLookupPopupCaller_NotifyPopupResult" />
<asp:TextBox ID="ctlAdvanceNo" SkinID="SkCtlTextboxLeft" runat="server" Style="display: none;"></asp:TextBox>
<asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server" Style="display: none;"></asp:TextBox>
