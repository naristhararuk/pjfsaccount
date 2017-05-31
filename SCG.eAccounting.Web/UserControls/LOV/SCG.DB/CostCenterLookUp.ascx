<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostCenterLookUp.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CostCenterLookUp" %>
    
<%@ Register src="../../Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc1" %>

<%--<ss:InlineScript ID="InlineScript1" runat="server">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

</ss:InlineScript>--%>

<%--<asp:LinkButton ID="lnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ctlCostCenterLookupModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="pnCostCenterSearch" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" />--%>
<asp:HiddenField ID="ctlCompanyId" runat="server" />
<uc1:PopupCaller ID="ctlCostCenterPopupCaller" runat="server" Hide="true" Width="610" Height="480" OnNotifyPopupResult="ctlCostCenterPopupCaller_NotifyPopupResult"/>