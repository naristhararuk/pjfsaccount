<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationField.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationField" EnableTheming="true" %>
<asp:UpdatePanel ID="UpdatePanelLocation" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="table" id="ctlContainer" runat="server" cellpadding ="0" cellspacing="0">
            <tr>
                <td align="left" style="width: 150px">
                    <asp:TextBox ID="ctlLocation" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                </td>
                <td>
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" onclick="ctlSearch_Click" />
                    <asp:HiddenField ID="ctlLocationId" runat="server"></asp:HiddenField>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
