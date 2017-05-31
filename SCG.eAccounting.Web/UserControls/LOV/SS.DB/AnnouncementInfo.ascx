<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementInfo.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.AnnouncementInfo" %>
<asp:Repeater ID="rptAnnouncementGroup" runat="server"    
    onitemdatabound="rptAnnouncementGroup_ItemDataBound">
<ItemTemplate>
<fieldset id="fdsSearch" style="border-color:Gray;border-style:solid;border-width:1px">
	<legend id="legSearch" style="color:#4E9DDF;font-family:Tahoma;font-size:small;"><asp:Image ID="ctlImage" runat="server" />&nbsp;<%# Eval("AnnouncementGroupName")%></legend>
    <table border="0" style="width:480px;font-family:Tahoma;" cellspacing="0" cellpadding="0">
        <tr align="left"><td>
            <%--<asp:Image ID="ctlImage" runat="server" />
        <asp:Label ID="lblAnnouncementGroupName" Font-Bold="true" runat="server" Text='<%# Eval("AnnouncementGroupName")%>'></asp:Label>--%>
        <hr />
        <asp:Repeater ID="rptAnnouncement" runat="server" onitemdatabound="rptAnnouncement_ItemDataBound">
        <ItemTemplate>
            <table><tr align="left" style="font-size:small"><td>
                <asp:LinkButton ID="ctlAnnouncement" runat="server"><%# Eval("AnnouncementHeader")%></asp:LinkButton>
                <asp:Image ID="imgNew" runat="server" />
                </td></tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    </td></tr>
</table>
</fieldset>
<br />
</ItemTemplate>
</asp:Repeater> 

