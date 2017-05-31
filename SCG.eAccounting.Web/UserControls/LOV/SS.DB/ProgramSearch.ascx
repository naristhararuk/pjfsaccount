<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramSearch.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.ProgramSearch" EnableTheming="true"%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>
<uc1:PopupCaller ID="ctlProgramSearchLookupPopupCaller" runat="server" Hide="true" Width="610" Height="480" OnNotifyPopupResult="ctlProgramSearchLookupPopupCaller_NotifyPopupResult"/>

