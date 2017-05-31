<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseFormInvoice.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ExpenseFormInvoice" %>
<asp:UpdatePanel ID="ctlUpdatePanelActorData" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Repeater ID="ctlRepeater" runat="server" OnItemCommand="ctlRepeater_ItemCommand" OnItemDataBound="ctlRepeater_ItemDataBound">
            <ItemTemplate>
            <table width="100%" border="0" class="table">
            <tr>
                <th width="5%" align="center">Seq</th>
                <th width="15%" align="center">Invoice No.</th>
                <th width="10%" align="center">Invoice Date</th>
                <th width="20%" align="center">Vendor</th>
                <th width="10%" align="center">Base Amount</th>
                <th width="10%" align="center">VAT Amount</th>
                <th width="10%" align="center">WHT Amount</th>
                <th width="10%" align="center">Net Amount</th>
                <th width="10%" align="center">Action</th>
            </tr>
            <tr>
            <td align="center"><%# Eval("Seq") %></td>
            <td align="center"><%# Eval("InvoiceNo") %></td>
            <td align="center"><%# Eval("InvoiceDate") %></td>
            <td align="left"><%# Eval("Vendor") %></td>
            <td align="right"><%# Eval("BaseAmount") %></td>
            <td align="right"><%# Eval("VATAmount")%></td>
            <td align="right"><%# Eval("WHTAmount")%></td>
            <td align="right"><%# Eval("NetAmount")%></td>
            <td align="center">
            <div style="text-align:center">
                <asp:LinkButton ID="ctlEdit" runat="server" Text="Edit" CommandName="Edit" />
                <asp:LinkButton ID="ctlDelete" runat="server" Text="Delete" CommandName="Delete"/>
            </div>
            </td>
            </tr>
            <tr>
            <td colspan="9" align="right">
            <ss:BaseGridView ID="ctlInvoiceItem" ReadOnly="true" runat="server" CssClass="table"
            AutoGenerateColumns="false" Width="95%" HeaderStyle-HorizontalAlign="Center">
                  <Columns>
                    <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="AccountCode" HeaderText="Account Code" HeaderStyle-Width="22%"/>
                    <asp:BoundField DataField="InternalOrder" HeaderText="Internal Order" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="25%"/>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="10%" />
                    <asp:BoundField DataField="RefNo" HeaderText="Reference No." HeaderStyle-Width="11%" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlEditItem" runat="server" Text="Edit" CommandName="EditItem" Visible='<%# ShowButton %>'/>
                            <asp:LinkButton ID="ctlDeleteItem" runat="server" Text="Delete" CommandName="DeleteItem" Visible='<%# ShowButton %>'/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                </Columns>
                </ss:BaseGridView>
            </td>
            </tr>
            </table>
           </ItemTemplate>
           <SeparatorTemplate><br /></SeparatorTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
