<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverRole.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.OverRole" %>
<asp:Panel ID="ctlOverRolePanel" runat="server" Width="100%" BackColor="White">
    <asp:UpdatePanel ID="UpdatePanelOverRole" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <fieldset id="POverRole" style="vertical-align:middle;background-color:#edeff5">
                            <legend id="Legend1" class="table" style="color: #4E9DDF">
                                <asp:Label ID="lblPOverRole" runat="server" SkinID="SkGeneralLabel" Text="Over Role"></asp:Label>
                            </legend>
                            <center>
                                <ss:BaseGridView ID="ctlOverRoleGridview" runat="server" Height="98%" Width="98%" OnRowDataBound="ctlOverRoleGridView_RowDataBound"
                                    CssClass="Grid" ReadOnly="true" HeaderStyle-CssClass="GridHeader" AutoGenerateColumns="false"
                                    DataKeyNames="UserID,InitiatorID">
                                    <AlternatingRowStyle CssClass="GridItem" />
                                    <RowStyle CssClass="GridAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No.">
                                            <ItemTemplate>
                                                <asp:Label ID="ctlNoLabel" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Initiator">
                                            <ItemTemplate>
                                                <asp:Label ID="ctlInitiatorLabel" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="ctlEmailLabel" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SMS">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ctlSMSChk" runat="server" Enabled="false" Checked='<%# Bind("SMS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Skip">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ctlSkipChk" runat="server" Checked='<%# Bind("isSkip") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Skip's Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="ctlSkipReasonText" runat="server" Text='<%# Bind("SkipReason") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </ss:BaseGridView>
                            </center>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
