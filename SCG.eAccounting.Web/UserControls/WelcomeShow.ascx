<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeShow.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.WelcomeShow" %>
<asp:Panel ID="ctlPanel" HorizontalAlign="Left"  runat="server" >
<center>
<table class="backgroundWelcomeMsg">
<tr><td style="width:3%">&nbsp;</td>
<td align="Left" valign="top"><br /><br />
<table style="width:50%;font-family:Tahoma;vertical-align:top;">
<tr>
<td colspan="2" 
        style="font-family:Tahoma;color:#74E6FE;font-size:50px;font-size:larger;font-weight:bold;vertical-align:top">
<asp:Label ID="ctlHeader" runat="server" ></asp:Label>
</td>
</tr>
<tr><td><asp:Image ID="ctlImageLine" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="lineWelcome" runat="server" /></td>
<td></td>
</tr>
<tr>
<td align="left"><asp:Label id="ctlContent" Style="white-space:normal;text-align:left;" Width="500px" runat="server" ></asp:Label></td>
<td></td>
</tr>
</table></td></tr></table>
</center>
</asp:Panel>
<!--
<asp:Panel ID="Panel1" runat="server">
<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="40" height="50" title="scg">
  <param name="movie" value="c01.swf" />
  <param name="quality" value="high" />
  <embed src="~/App_Themes/Default/images/c01.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="40" height="50"></embed>
</object>
</asp:Panel>
-->
