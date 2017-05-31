<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TAField.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.TAField" %>
<%@ Register src="TALookup.ascx" tagname="TALookup" tagprefix="uc1" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left" valign="middle" style="width:20%">
            <asp:Label ID="ctlTANoText" runat="server" Text="Travelling Authorization"></asp:Label>&nbsp;:&nbsp;
        </td>
        <td align="left" valign="middle" style="width:20%">
            <asp:LinkButton ID="ctlTANoDetail" runat="server" Text="N/A"></asp:LinkButton>
        </td>
        <td align="left">
            <asp:ImageButton ID="ctlTANoLookup" runat="server" OnClick="ctlTANoLookup_Click"
                SkinID="SkCtlQuery" ToolTip="Search" />
        </td>
    </tr>
</table>
<uc1:TALookup ID="ctlTALookup" runat="server" isMultiple="false" />

