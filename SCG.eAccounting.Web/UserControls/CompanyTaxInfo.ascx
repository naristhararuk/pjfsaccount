<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyTaxInfo.ascx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.UserControls.CompanyTaxInfo" 
%>
<%@ Register src="CompanyTaxEditor.ascx" tagname="CompanyTaxEditor" tagprefix="uc1" %>
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
<asp:Panel ID="ctlPanelCompanyTaxInfo" runat="server" Style="display:none">
    <br />
    <asp:Label runat="server" ID="Label1" SkinID="SkGeneralLabel" Text="Tax Code : "></asp:Label>
    <asp:Label runat="server" ID="lblTaxText" SkinID="SkGeneralLabel"></asp:Label>
    <br />
    <asp:Label runat="server" ID="ctlLabelHead" SkinID="SkGeneralLabel" Text="Company Rate Information :"></asp:Label>
    <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <ss:BaseGridView ID="ctlGridRole" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="true"
                DataKeyNames="ID" OnRequestData="RequestData" CssClass="table" HeaderStyle-CssClass="GridHeader"
                OnDataBound="Grid_DataBound" OnRowCommand="ctlTaxGrid_RowCommand" SelectedRowStyle-BackColor="#6699FF" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Code" SortExpression="CompanyCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblCompanyCode" runat="server" Text='<%# Bind("CompanyCode")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name" SortExpression="CompanyName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblCompanyName" runat="server" Text='<%# Bind("CompanyName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rate", "{0:#,##0.0000}") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate Non-Deduct" SortExpression="RateNonDeduct" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblRateNonDeduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RateNonDeduct", "{0:#,##0.0000}") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Use Parent Rate" SortExpression="UseParentRate" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlUseParentRate" Checked='<%# Bind("UseParentRate") %>' runat="server"
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Disable" SortExpression="Disable" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlDisable" Checked='<%# Bind("Disable") %>' runat="server" Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-BackColor="Transparent" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" CommandName="RoleEdit"
                                CausesValidation="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <uc1:CompanyTaxEditor ID="ctlCompanyTaxEditor" runat="server" />
            <br />
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr align="left">
                    <td>
                        <asp:ImageButton ID="ctlButtonAdd" SkinID="SkAddButton" Text="$Add$" runat="server"
                            OnClick="Add_Click" />
                        <asp:ImageButton ID="ctlButtonDelete" SkinID="SkDeleteButton" Text="$Delete$" runat="server"
                            OnClick="Delete_Click" OnClientClick="return confirm('Are you sure to delete this row?');" />
                        <asp:ImageButton ID="ctlButtonCancel" OnClick="ctlButtonCancel_Click" SkinID="SkCancelButton"
                            Text="$Cancel$" runat="server" />
                    </td>
                </tr>
            </table>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="CompanyTax.Error">
                </spring:ValidationSummary>
            </font>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
