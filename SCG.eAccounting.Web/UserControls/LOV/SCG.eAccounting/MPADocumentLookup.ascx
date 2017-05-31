<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPADocumentLookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting.MPALookup" %>
<%@ Register Src="~/UserControls/Shared/PopupCaller.ascx" TagName="PopupCaller" TagPrefix="uc1" %>
<uc1:PopupCaller ID="ctlMPALookupPopupCaller" runat="server" Hide="true" Width="640"
    OnNotifyPopupResult="ctlMPADocumentLookupPopupCaller_NotifyPopupResult" />