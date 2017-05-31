<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountLabelLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.AccountLabelLookup" EnableTheming="false" %>
<%@ Register Src="AccountLookup.ascx" TagName="AccountLookup" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelAccountSimple" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td align="left">
                    <asp:Label ID="ctlExpenseCode" runat="server" SkinID="SkCodeLabel"></asp:Label>
                </td>
                <td style="width:10%">
                    <asp:ImageButton runat="server" ID="ctlSearchExpenseCode" SkinID="SkLookupButton"
                        OnClick="ctlSearchExpenseCode_Click" />
                </td>
            </tr>
        </table>
        <uc1:AccountLookup ID="ctlExpenseLookup" runat="server" isMultiple="false" />
    </ContentTemplate>
</asp:UpdatePanel>
