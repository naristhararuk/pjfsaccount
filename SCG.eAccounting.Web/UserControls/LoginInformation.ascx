<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="LoginInformation.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.LoginInformation" 
    EnableTheming ="true"
%>
<style type="text/css">
    .style1
    {
        text-decoration: underline;
        font-family: Tahoma;
        font-weight: 700;
        font-size: 7pt;
    }
</style>

<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:Label ID="ctlWebStatisticLabel" runat="server" SkinID="SkCtlLoginInfomationHead">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;&nbsp;&nbsp;</td>
        <td style="text-align:left;">
            <asp:Label ID="ctlYouVisitorNumberLabel" runat="server" Text="Visitors"  SkinID="SkCtlLoginInfomationDetail" >
            </asp:Label>&nbsp;:&nbsp;
            
            <asp:Label ID="ctlYouVisitorNumber" runat="server" SkinID="SkCtlLoginInfomationDetail">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;&nbsp;&nbsp;</td>
        <td style="text-align:left">
            <asp:Label ID="ctlCurrentUserOnlineLabel" runat="server" Text="Online Users" SkinID="SkCtlLoginInfomationDetail" >
            </asp:Label>&nbsp;:&nbsp;
            
            <asp:Label ID="ctlCurrentUserOnline" runat="server"  SkinID="SkCtlLoginInfomationDetail">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;&nbsp;&nbsp;</td>
        <td style="text-align:left">
            <asp:Label ID="ctlTodayUseLoginLabel" runat="server" Text="Today Visitors" SkinID="SkCtlLoginInfomationDetail" >
            </asp:Label>&nbsp;:&nbsp;
            <asp:Label ID="ctlTodayUseLogin" runat="server"  SkinID="SkCtlLoginInfomationDetail">
            </asp:Label>
        </td>
    </tr>
</table>