<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MileageCom2.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.MileageCom2" %>
<ss:BaseGridView ID="ctlMileageGrid" ReadOnly="true" runat="server" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center"
    EnableInsert="False" InsertRowCount="1" SaveButtonID="" Width="100%" CssClass="table" ShowFooter="true">
    <Columns>
        <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="True" SortExpression="Date" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="LocationFrom" HeaderText="Location From" ReadOnly="True"
            SortExpression="Location" />
        <asp:BoundField DataField="LocationTo" HeaderText="Location To" SortExpression="LocationTo" ReadOnly="True" />
        <asp:TemplateField>
            <HeaderTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td colspan="2" align="center">Car Meter<br /><hr /></td>
                    </tr>
                    <tr>
                        <td align="center" style="width:15%">Start</td>
                        <td align="center" style="width:15%">End</td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td align="right" style="width:15%"><%# Eval("CarMeterStart")%></td>
                        <td align="right" style="width:15%"><%# Eval("CarMeterEnd")%></td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">Total Distance (km.)</td>
                    </tr>
                </table>
            </FooterTemplate>
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField>
        <HeaderTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td colspan="3" align="center">Distance (km./day)<br /><hr /></td>
                    </tr>
                    <tr>
                        <td align="center" style="width:15%">Total</td>
                        <td align="center" style="width:15%">Adjust</td>
                        <td align="center" style="width:20%">Net</td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td align="right" style="width:15%"><%# Eval("Total") %></td>
                        <td align="right" style="width:15%"><%# Eval("Adjust")%></td>
                        <td align="right" style="width:20%"><%# Eval("Net")%></td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <table width="100%">
                    <tr>
                        <td align="right" style="width:15%">100</td>
                        <td align="right" style="width:15%">20</td>
                        <td align="right" style="width:20%">80</td>
                    </tr>
                </table>
            </FooterTemplate>
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
            <asp:LinkButton ID="ctlEdit" runat="server" Text="Edit"/>
            <asp:LinkButton ID="ctlDelete" runat="server" Text="Delete"/>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" Width="75px" />
            <ItemStyle HorizontalAlign="Center" Wrap="false"  Width="75px"/>
        </asp:TemplateField>
    </Columns>
</ss:BaseGridView>
