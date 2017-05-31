<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CADocumentLookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting.CALookup" %>
<%@ Register Src="~/UserControls/Shared/PopupCaller.ascx" TagName="PopupCaller" TagPrefix="uc1" %>
<uc1:PopupCaller ID="ctlCALookupPopupCaller" runat="server" Hide="true" Width="640"
    OnNotifyPopupResult="ctlCADocumentLookupPopupCaller_NotifyPopupResult" />