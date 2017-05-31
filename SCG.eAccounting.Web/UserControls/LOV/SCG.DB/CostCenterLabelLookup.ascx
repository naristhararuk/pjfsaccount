<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostCenterLabelLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CostCenterLabelLookup"
    EnableTheming="false" %>
<%@ Register Src="CostCenterLookUp.ascx" TagName="CostCenterLookUp" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelCostCenterSimple" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table" cellspacing="0" cellpadding="0" border="0" class="noborder">
            <tr>
                <td align="center">
                    <asp:Label ID="ctlCostCenter" runat="server" SkinID="SkCodeLabel"></asp:Label>
                </td>
                <td style="width:10%">
                    <asp:ImageButton runat="server" ID="ctlSearchCostCenter" SkinID="SkLookupButton"
                        OnClick="ctlSearchCostCenter_Click" />
                </td>
            </tr>
        </table>
        <uc1:CostCenterLookUp ID="ctlCostCenterLookUp" runat="server" isMultiple="false" />
    </ContentTemplate>
</asp:UpdatePanel>

