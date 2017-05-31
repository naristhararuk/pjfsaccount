<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementInfo.aspx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SU.Programs.AnnouncementInfo" %>

<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" style="width: 609px; font-family: Tahoma;" cellspacing="0" cellpadding="0">
            <tr align="left">
                <td style="vertical-align: top;" align="center">
                    <table border="0" style="width: 100%;">
                        <tr>
                            <td align="left" style="padding-left: 20px;">
                                <asp:Label ID="ctlHeader" runat="server" Text="News" Font-Size="Large" ForeColor="Gray"
                                    Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 20px;">
                                <asp:Image ID="ctlImageLine" Width="100%" Height="4px" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="lineNews"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 20px;">
                                <asp:Repeater ID="rptAnnouncement" runat="server" OnItemDataBound="rptAnnouncement_ItemDataBound">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" valign="top" style="width: 10%">
                                                    <asp:Panel ID="ctlPanelGroup" runat="server" Style="text-align: center; vertical-align: middle;">
                                                        <asp:Label ID="ctlDayOfMonth" ForeColor="WhiteSmoke" Font-Bold="true" Font-Size="Smaller"
                                                            Font-Names="Verdana" runat="server" Width="100%" Style="text-align: center; vertical-align: top;"
                                                            BackColor="Transparent"></asp:Label><br />
                                                        <asp:Label ID="ctlMonthOfYear" runat="server" ForeColor="WhiteSmoke" Font-Bold="true"
                                                            Font-Size="Smaller" Font-Names="Verdana" Width="100%" Style="text-align: center;
                                                            vertical-align: top;" BackColor="Transparent"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:Label ID="ctlAnnouncementHeader" Font-Names="Tahoma" Font-Size="Small" runat="server"
                                                        Text='<%# Eval("AnnouncementHeader") %>'></asp:Label>
                                                    <asp:Image ID="ctlImageNew" runat="server" />
                                                    <br />
                                                    <br />
                                                    <asp:LinkButton ID="ctlReadMore" Font-Names="Tahoma" Font-Size="Small" Font-Underline="false"
                                                        runat="server" Text='<%#GetMessage("ReadMoreText")%>'></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
