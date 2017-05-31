<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorLookUp.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.VendorLookUp" %>
    
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlVendorLookupPopupCaller" runat="server" Hide="true" Width="790" Height="520" OnNotifyPopupResult="ctlVendorLookupPopupCaller_NotifyPopupResult"/>
