<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyField.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CompanyField" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" TagName="CompanyAutoComplete"
    TagPrefix="uc2" %>
<asp:UpdatePanel ID="ctlUpdatePanelCompany" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="ctlCompanyID" runat="server" Style="display: none;"></asp:Label>
        <table id="ctlContainerTable" runat="server" cellpadding="0" cellspacing="0" class="table" border="0">
            <tr>
                <td valign="middle" align="left">
                    <uc2:CompanyAutoComplete ID="ctlCompanyAutocomplete" runat="server" OnNotifyPopupResult="ctlCompanyCode_NotifyPopupResult" />
                </td>
                <td valign="middle" align="left" style="width: 30px;">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td valign="middle" align="left">
                    <asp:Label ID="ctlCompanyName" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                    <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
