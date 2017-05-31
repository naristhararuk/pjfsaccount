 <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TALookupField.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.TALookupField" %>

<asp:UpdatePanel ID="ctlUpdatePanelTa" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" valign="middle" style="width:20%">
                <asp:TextBox ID="ctlTANoDetail" runat="server"></asp:TextBox>
                <asp:Label ID="ctlTAID" runat="server" style="display:none" ></asp:Label>
            </td>
            <td align="left">
                <asp:ImageButton ID="ctlTANoLookup" runat="server" OnClick="ctlTANoLookup_Click"
                    SkinID="SkCtlQuery" ToolTip="Search" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>


