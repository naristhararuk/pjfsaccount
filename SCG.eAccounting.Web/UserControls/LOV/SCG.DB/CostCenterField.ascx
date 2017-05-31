<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostCenterField.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CostCenterField" EnableTheming="true" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterTextBoxAutoComplete.ascx" TagName="Auto"
    TagPrefix="uc2" %>
<asp:UpdatePanel ID="ctlUpdatePanelCostCenter" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="ctlContainer" runat="server" border="0" cellpadding="0"
            cellspacing="0" class="noborder">
            <tr>
                <td align="left">
                    <uc2:Auto ID="ctlAutoCostCenter" runat="server" OnNotifyPopupResult="ctlAutoCostCenter_NotifyPopupResult"
                        CompanyId='<%# this.CompanyId %>' />
                    <asp:Label ID="ctlMode" runat="server" Style="display: none"></asp:Label>
                    <asp:HiddenField ID="ctlCostCenterCode" runat="server" />
                    <asp:HiddenField ID="ctlCostCenterID" runat="server" />
                    <asp:HiddenField ID="ctlCompanyID" runat="server" />
                </td>
                <td align="left">
                        <asp:ImageButton  runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td align="left">
                    <asp:Label ID="ctlDescription" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
