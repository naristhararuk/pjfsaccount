<%@ Page Language="C#" AutoEventWireup="true" 
CodeBehind="AnnouncementPopup.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.AnnouncementPopup" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Announcement</title>
    </head>
<body>
  <div align="center">
    <form id="FormLogin" runat="server" > 
    <asp:Repeater ID="rptAnnouncementDetail" runat="server" 
        onitemdatabound="rptAnnouncementDetail_ItemDataBound">
        <ItemTemplate>
        <fieldset id="fdsSearch" style="border-color:Gray;border-style:solid;border-width:1px;font-family:Tahoma;font-size:small;">
	        <legend id="legSearch" style="color:#4E9DDF;font-family:Tahoma;font-size:small;">
	        
	        <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("AnnouncementGroupName") %>' 
					meta:resourcekey="lblGroupResource1" Font-Size="Large" ForeColor="Gray" Font-Bold="true"></asp:Label></legend>
            <table width="100%" style="text-align:left;">
              <tr valign="top">
              <td >
                <table style="vertical-align:top;">
                <tr>
                <td>
                <asp:Panel ID="ctlPanelGroup" runat="server" Style="text-align: center; vertical-align: middle;">
                    <asp:Label ID="ctlDayOfMonth" ForeColor="WhiteSmoke" Font-Bold="true" Font-Size="Smaller"
                    Font-Names="Verdana" runat="server" Width="100%" Style="text-align: center; vertical-align: top;"
                    BackColor="Transparent"></asp:Label><br />
                    <asp:Label ID="ctlMonthOfYear" runat="server" ForeColor="WhiteSmoke" Font-Bold="true"
                    Font-Size="Smaller" Font-Names="Verdana" Width="100%" Style="text-align: center;
                    vertical-align: top;" BackColor="Transparent"></asp:Label>
                    </asp:Panel>
                <%--<asp:Image ID="ctlImage" runat="server" meta:resourcekey="ctlImageResource1" />--%>
                </td>
                <td><asp:Label ID="lblHeader" runat="server" Text='<%# Eval("AnnouncementHeader") %>' meta:resourcekey="lblHeaderResource1"></asp:Label>
              <asp:Image ID="ctlImageHeader" runat="server" meta:resourcekey="ctlImageHeaderResource1" /></td>
                </tr>
                </table>
              </td></tr>
              <tr><td><hr /></td></tr>
              <tr><td><asp:Label ID="lblItem" runat="server" 
					  Text='<%# Eval("AnnouncementBody") %>' meta:resourcekey="lblItemResource1"></asp:Label></td></tr>
              <tr><td><asp:Label ID="Label1" runat="server" 
					  Text='<%# Eval("AnnouncementFooter") %>' meta:resourcekey="Label1Resource1"></asp:Label></td></tr>
            </table>
            </fieldset>
        </ItemTemplate>
    </asp:Repeater>
    </form>
    </div>
    </body>
</html>
