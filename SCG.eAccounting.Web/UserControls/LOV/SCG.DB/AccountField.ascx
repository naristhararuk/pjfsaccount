<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountField.ascx.cs"
	Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.AccountField" EnableTheming="true" %>
	
<%@ Register Src="AccountTextBoxAutoComplete.ascx" TagName="AccountTextBoxAutoComplete" TagPrefix="uc2" %>

<asp:UpdatePanel ID="ctlUpdatePanelAccount" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="noborder" id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <uc2:AccountTextBoxAutoComplete ID="ctlAccountTextBoxAutoComplete" runat="server" OnNotifyPopupResult="ctlAccountTextBoxAutoComplete_NotifyPopupResult" />
                    <%--<asp:TextBox ID="ctlAccouncodeText" runat="server"></asp:TextBox>--%>
                    <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                    <asp:Label ID="ctlAccountID" runat="server" Style="display: none;"></asp:Label>
                </td>
                <td align="left">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td align="left">
                    <asp:Label ID="ctlAccountName" runat="server" SkinID="SkGeneralLabel"></asp:Label>                    
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>