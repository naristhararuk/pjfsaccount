<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseInfoEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ExpenseInfoEditor" %>
<%@ Register Src="AccountLangEditor.ascx" TagName="AccountLangEditor" TagPrefix="uc1" %>
<%@ Register Src="CompanyInfoEditor.ascx" TagName="CompanyInfoEditor" TagPrefix="uc2" %>
<asp:Panel ID="ctlExpenseInfoEditor" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlExpenseInfoUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 100%"  title="Expense's Information" id="ctlExpenseInfoFieldSet" runat="server" visible="False"
                enableviewstate="True">
              <legend id="ctlLegendExpenseInfo" runat="server" style="color: #4E9DDF"  visible="True">
              <asp:Label ID="ctlDetailHeaderExpenseInformation" runat="server" Text="$Expense's Infomation$" /></legend>
                <ss:BaseGridView ID="ctlExpenseInfoGrid" runat="server" AutoGenerateColumns="false"
                    OnRowCommand="Expense_RowCommand" DataKeyNames="ExpenseGroupID,LanguageID,AccountID"
                    CssClass="table" Width="100%" SelectedRowStyle-BackColor="#6699FF" OnPageIndexChanged="ExpenseInfoGrid_PageIndexChanged"
                    OnRequestCount="RequestCount" OnRequestData="RequestData" AllowSorting="true"
                    AllowPaging="true">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="Expense Code"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" SortExpression="AccountCode">
                            <ItemTemplate>
                                <asp:Label ID="ctlExpenseCode" runat="server" Text='<%# Bind("AccountCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="65%" SortExpression="AccountName">
                            <ItemTemplate>
                                <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("AccountName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="65%" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" SortExpression="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <table border="0">
                                    <tr>
                                        <td align="center" valign="middle">
                                            <asp:LinkButton ID="ctlCompany" Text="Company" CommandName="Company" runat="server" />
                                        </td>
                                        <td align="center" valign="middle">
                                            <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                                CausesValidation="False" CommandName="ExpenseInfoEdit" />
                                        </td>
                                        <td align="center" valign="middle">
                                            <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                                CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                                CommandName="ExpenseInfoDelete" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <asp:ImageButton runat="server" ID="ctlAddExpenseInfo" ToolTip="Add" SkinID="SkAddButton"
                    OnClick="addExpense_Click" ImageAlign="Left" />
                <asp:ImageButton runat="server" ID="ctlCloseExpenseInfo" ToolTip="Close" SkinID="SkCtlFormCancel"
                    OnClick="closeExpense_Click" ImageAlign="Left" />
                <asp:HiddenField ID="epgID" runat="server" />
                <asp:HiddenField ID="account" runat="server" />
            </fieldset>
            <br />
            <uc2:CompanyInfoEditor ID="ctlCompanyInfoEditor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<uc1:AccountLangEditor ID="ctlAccountLangEditor" runat="server" />
