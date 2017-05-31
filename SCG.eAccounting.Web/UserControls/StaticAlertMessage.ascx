<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StaticAlertMessage.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.StaticAlertMessage" %>

<script language="jscript" type="text/javascript">
    function hideMessage() {
        document.getElementById('<%= PanelShowMessage.ClientID %>').style.display = 'none';
    }
</script>
<asp:Panel ID="PanelShowMessage" runat="server" Width="60%" BackImageUrl="~/App_Themes/Default/images/bg.jpg">
    <table width="100%">
        <tr>
            <td align="left" valign="top" style="width:10%"><asp:Image ID="ctlAlertImg" runat="server" /></td>
            <td align="center" valign="bottom">
                <asp:Label ID="ctlMessageError" runat="server" Text="" SkinID="SkCtlMsgError" Visible="false"></asp:Label>
                <asp:Label ID="ctlMessageInformation" runat="server" Text="" SkinID="SkCtlMsgInformation" Visible="false"></asp:Label>
            </td>
            <td align="right" valign="top" style="width:10%">
                <img id="ctlCloseImg" runat="server" src="~/App_Themes/Default/images/icon/BtDelete.gif" alt="" style="cursor:hand;" onclick="hideMessage();"/>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center" valign="top">
                </td>
        </tr>     
    </table>
</asp:Panel>
