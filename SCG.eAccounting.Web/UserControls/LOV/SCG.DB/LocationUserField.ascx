<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationUserField.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationUserField" %>
<%@ Register Src="LocationTextBoxAutocomplete.ascx" TagName="LocationTextBoxAutocomplete" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelLocation" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table" id="ctlContainer" runat="server" border="0" cellpadding="0"
            cellspacing="0">
            <tr>
                <td align="left">
                    <uc1:LocationTextBoxAutocomplete ID="ctlLocationTextBoxAutoComplete" OnNotifyPopupResult="ctlAutoLocation_NotifyPopupResult" LocationID='<%# this.LocationID %>' runat="server" />
                    <asp:Label ID="ctlMode" runat="server" Style="display: none"></asp:Label>
                    <asp:HiddenField ID="ctlLocationName" runat="server" />
                    <asp:HiddenField ID="ctlLocationID" runat="server" />
                    <asp:HiddenField ID="ctlCompanyID" runat="server" />
                </td>
                <td align="left">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                </td>
                <td>
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