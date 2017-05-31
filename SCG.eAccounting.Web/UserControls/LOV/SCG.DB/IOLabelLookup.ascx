<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IOLabelLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.IOLabelLookup" EnableTheming="false" %>
<%@ Register Src="IOLookup.ascx" TagName="IOLookup" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelIOSimple" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td align="center">
                    <asp:Label ID="ctlIO" runat="server" SkinID="SkCodeLabel"></asp:Label>
                </td>
                <td style="width: 10%">
                    <asp:ImageButton runat="server" ID="ctlSearchIO" SkinID="SkLookupButton" OnClick="ctlSearchIO_Click" />
                </td>
            </tr>
        </table>
        <uc1:IOLookup ID="ctlIOLookup" runat="server" isMultiple="false" />
    </ContentTemplate>
</asp:UpdatePanel>
