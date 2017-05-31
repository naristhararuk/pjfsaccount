<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpensesViewByAccount.ascx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ExpensesViewByAccount" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:UpdatePanel ID="ctlUpdatePanelViewByAccount" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewByAccount"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div align="left">
            <table border="0" width="100%">
                <tr align="center">
                    <td align="center" colspan="2">
                        <asp:HiddenField ID="ctlType" runat="server" />
                        <ss:BaseGridView ID="ctlExpenseItemGrid" runat="server" AutoGenerateColumns="False"
                            EnableInsert="False" InsertRowCount="1" Width="100%" CssClass="Grid" OnRequestCount="RequestCount"
                            OnRequestData="RequestData" AllowPaging="true" OnDataBound="ctlExpenseItemGrid_DataBound"
                            ShowPageSizeDropDownList="false" 
                            onrowdatabound="ctlExpenseItemGrid_RowDataBound">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Item" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblSeq" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expense Code" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblExpenseCode" runat="server" Text='<%# Eval("ExpenseCode")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Center" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblCostCenter" runat="server" Text='<%# Eval("CostCenterCode")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Internal Order" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblInternalOrder" runat="server" Text='<%# Eval("IONumber")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlDescription" runat="server" Text='<%# Eval("Description") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblCurrencySymbol" runat="server" Text='<%# Eval("CurrencySymbol")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblCurrencyAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CurrencyAmount", "{0:#,##0.00}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ExchangeRate" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.0000}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(FinalCurrency)" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblAmountLocalCurrency" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LocalCurrencyAmount", "{0:#,##0.00}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(THB)" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ReferenceNo" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Literal Mode="Encode" ID="ctlLblReferenceNo" runat="server" Text='<%# Eval("ReferenceNo")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <div id="ctlDivTotal" runat="server">
                        <table width="100%">
                        <tr>
                        <td align="right"><asp:Label ID="ctlTotalLabel" runat="server" Text="$Total$" SkinID="SkFieldCaptionLabel" /></td>
                        <td align="right" style="width:10%"><asp:Label ID="ctlTotalAmountTHB" runat="server" SkinID="SkNumberLabel" /></td>
                        </tr>
                        </table>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div id="ctlDivSummary" runat="server">
                <table border="1" cellpadding="0" cellspacing="0" width="40%">
                    <tr>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlTotalAmountLabel" runat="server" Text="$Amount$" SkinID="SkFieldCaptionLabel" />
                            <asp:Label ID="ctlTotalAmountRequired" runat="server" SkinID="SkRequiredLabel" />
                        </td>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlVatAmountLabel" runat="server" Text="$VAT Amount$" SkinID="SkFieldCaptionLabel" />
                            <asp:Label ID="ctlVatAmountRequired" runat="server" SkinID="SkRequiredLabel" />
                        </td>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlWHTAmountLabel" runat="server" Text="$WHT Amount$" SkinID="SkFieldCaptionLabel" />
                            <asp:Label ID="ctlWHTAmountRequired" runat="server" SkinID="SkRequiredLabel" />
                        </td>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlNetAmountLabel" runat="server" Text="$Net Amount$" SkinID="SkFieldCaptionLabel" />
                            <asp:Label ID="ctlNetAmountRequired" runat="server" SkinID="SkRequiredLabel" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlAmount" runat="server" SkinID="SkNumberLabel" />
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlVatAmount" runat="server" SkinID="SkNumberLabel" />
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlWHTAmount" runat="server" SkinID="SkNumberLabel" />
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlNetAmount" runat="server" SkinID="SkNumberLabel" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
