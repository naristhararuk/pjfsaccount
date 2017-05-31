<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserProfileLookup" EnableTheming="true" %>

<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<uc1:PopupCaller ID="ctlUserProfileLookupPopupCaller" runat="server" Hide="true" Width="620" Height="500" OnNotifyPopupResult="ctlUserProfileLookupPopupCaller_NotifyPopupResult"/>
