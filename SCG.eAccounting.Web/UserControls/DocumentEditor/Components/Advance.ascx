<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Advance.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Advance" %>
<div align="center">
    <table id="ctlAdvanceTable" runat="server">
        <tr>
            <td>
                <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                <ss:BaseGridView ID="ctlAdvanceGridView" runat="server" OnRowCommand="ctlAdvanceGridview_RowCommand"
                    OnRowDataBound="ctlAdvanceGridview_RowDataBound" AutoGenerateColumns="False" OnDataBound="ctlAdvanceGridview_DataBound"
                    DataKeyNames="AdvanceID,RemittanceID" EnableInsert="False" InsertRowCount="1" 
                    SaveButtonID="" Width="100%" CssClass="Grid">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <asp:Label ID="ctlNoLabel" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Advance No.">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlLbtnAdvanceNo" runat="server" Text='<%# Bind("DocumentNo") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="ctllblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requester">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblRequester" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receiver">
                            <ItemTemplate>
                                <asp:Label ID="cltLblReceiver" runat="server" Text='<%# Bind("ReceiverName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date">
                            <ItemTemplate>
                                <asp:Label ID="cltLblDueDate" runat="server" Text='<%# Bind("DueDateOfRemittance") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount(THB)">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" ToolTip="Delete"
                                    CommandName="DeleteAdvance" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
            </td>
        </tr>
        <tr>
            <td>
                <%--<table id="ctlAddAdvanceTable" runat="server">
				<tr align="left">
					<td align="left">
						<asp:DropDownList ID="ctlDdlAdvance" runat="server" Width="100px"></asp:DropDownList>
					</td>
					<td  align="left">
						<asp:Button ID="ctlBtnAdd" runat="server" Text="Add" />
					</td>
				</tr>
			</table>--%>
            </td>
        </tr>
    </table>
</div>
