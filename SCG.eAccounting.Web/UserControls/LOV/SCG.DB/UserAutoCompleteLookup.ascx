<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="UserAutoCompleteLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserAutoCompleteLookup" %>
<%@ Register Src="UserProfileTextBoxAutoComplete.ascx" TagName="UserProfileTextBoxAutoComplete"
    TagPrefix="uc1" %>
<%@ Register src="UserProfileLookup.ascx" tagname="UserProfileLookup" tagprefix="uc2" %>
<asp:UpdatePanel ID="ctlUpdatePanelUser" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="noborder" id="ctlContainer" border="0" runat="server" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <uc1:UserProfileTextBoxAutoComplete ID="ctlUserTextBoxAutoComplete" runat="server"
                        OnNotifyPopupResult="ctlUserTextBoxAutoComplete_NotifyPopupResult" />
                        <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                        <asp:HiddenField ID="ctlExpID" runat="server" />
                        <asp:HiddenField ID="ctlUserNameID" runat="server" />
                </td>
                <td align="left">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td align="left">
                    <asp:Label ID="ctlUserName" runat="server" SkinID="SkGeneralLabel" Style="display: none;" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    </ContentTemplate>
</asp:UpdatePanel>