<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IOAutoCompleteLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.IOAutoCompleteLookup" EnableTheming="true" %>
<%@ Register Src="IOTextBoxAutoComplete.ascx" TagName="IOTextBoxAutoComplete" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelIO" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="ctlContainer" runat="server" class="noborder" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <uc1:IOTextBoxAutoComplete ID="ctlIOTextBoxAutoComplete" runat="server" OnNotifyPopupResult="ctlIOTextBoxAutoComplete_NotifyPopupResult" />
					<asp:Label ID="ctlIOID" runat="server" Style="display:none;"></asp:Label>
					<asp:Label ID="ctlMode" runat="server" Style="display:none;"></asp:Label>
					<asp:HiddenField ID="ctlCostCenterID" runat="server" />
					<asp:HiddenField ID="ctlCompanyID" runat="server" />
                </td>
                <td align="left">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td align="left">                    
                     <asp:Label ID="ctlIODescription" runat="server" SkinID="SkGeneralLabel"></asp:Label>   
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
