<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MileageRateRevisionLookUp.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting.MRLookUp" %>
<%@ Register Src="~/UserControls/Shared/PopupCaller.ascx" TagName="PopupCaller" TagPrefix="uc1" %>
<uc1:PopupCaller ID="ctlMRLookUpPopupCaller" runat="server" Hide="true" Width="640" height="120"
    OnNotifyPopupResult="ctlMRLookUp_NotifyPopupResult" />