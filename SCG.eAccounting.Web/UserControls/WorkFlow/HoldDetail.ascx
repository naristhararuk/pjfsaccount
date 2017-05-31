<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HoldDetail.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.HoldDetail" EnableViewState="true" %>
<asp:Panel ID="pnApproveRejection" runat="server" Width="100%" BackColor="White">
    <asp:UpdatePanel ID="UpdatePanelSearchProgram" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <table class="table" width="100%">
                    <tr>
                        <td>
                            <fieldset id="PHold" style="background-color:#edeff5">
                                <legend id="Legend1" class="table" style="color: #4E9DDF">
                                    <asp:Label ID="lblPHold" runat="server" SkinID="SkCtlLabel" Text="Hold"></asp:Label>
                                </legend>
                                <center>
                                <table style="width: 98%">
                                    <tr>
                                        <td width="10%" valign="top">
                                            <asp:Label ID="lblIdentify" SkinID="SkCtlLabel" runat="server" Text="Identify : "></asp:Label>
                                        </td>
                                        <td width="44%">
                                            <asp:ListBox ID="ctlLeftIdentify" SkinID="SkCtlListBox" runat="server" Rows="7"
                                                Width="100%" >
                                            </asp:ListBox>
                                        </td>
                                        <td width="2%">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="ctlCopyAllToRight" runat="server" Text="&gt;&gt;" Width="30px" OnClick="ctlCopyAllToRight_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="ctlCopyToRight" runat="server" Text="&gt;" Width="30px" OnClick="ctlCopyToRight_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="ctlCopyToLeft" runat="server" Text="&lt;" Width="30px" OnClick="ctlCopyToLeft_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="ctlCopyAllToLeft" runat="server" Text="&lt;&lt;" Width="30px" OnClick="ctlCopyAllToLeft_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="44%">
                                            <asp:ListBox ID="ctlRightIdentify" SkinID="SkCtlListBox" runat="server" Rows="7"
                                                Width="100%"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblRemark" SkinID="SkCtlLabel" runat="server" Text="Remark : "></asp:Label>
                                        </td>
                                        <td colspan="3" align="left">
                                            <asp:TextBox ID="ctlRemark" runat="server" SkinID="SkCtlTextboxMultiLine" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 200);" onkeyup="return IsMaxLength(this, 200);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                </center>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
