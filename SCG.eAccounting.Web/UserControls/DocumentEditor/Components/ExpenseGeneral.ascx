<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseGeneral.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ExpenseGeneral" %>
<%@ Register Src="SimpleExpense.ascx" TagName="SimpleExpense" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCaller.ascx" TagName="PopupCaller" TagPrefix="uc1" %>
<asp:UpdatePanel ID="ctlUpdatePanelExpenseGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelExpenseGeneral"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:HiddenField ID="ctlType" runat="server" />
        <asp:UpdatePanel ID="ctlUpdatePanelExpenseInvoice" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Repeater ID="ctlRepeater" runat="server" OnItemCommand="ctlRepeater_ItemCommand"
                    OnItemDataBound="ctlRepeater_ItemDataBound">
                    <ItemTemplate>
                        <table width="100%" border="0" class="Grid" cellpadding="0" cellspacing="0">
                            <tr class="GridHeader">
                                <th width="5%" align="center">
                                    <asp:Label ID="ctlSeqLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Seq") %>' />
                                </th>
                                <th width="15%" align="center">
                                    <asp:Label ID="ctlInvoiceNoLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("InvoiceNo") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlInvoiceDateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("InvoiceDate") %>' />
                                </th>
                                <th width="20%" align="center">
                                    <asp:Label ID="ctlVendorLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Vendor") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlTotalAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("BaseAmount") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlVatAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VATAmount") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlWhtAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("WHTAmount") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlNetAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("NetAmount") %>' />
                                </th>
                                <th width="10%" align="center">
                                    <asp:Label ID="ctlActionLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Action") %>' />
                                </th>
                            </tr>
                            <tr class="GridItem">
                                <td align="center">
                                    <asp:Literal Mode="Encode" ID="ctlSeq" runat="server" SkinID="SkOrderLabel" />
                                </td>
                                <td align="center">
                                    <asp:Literal Mode="Encode" ID="ctlInvoiceNo" SkinID="SkCodeLabel" runat="server"
                                        Text='<%# Eval("InvoiceNo") %>' />
                                </td>
                                <td align="center">
                                    <asp:Label ID="ctlInvoiceDate" SkinID="SkCalendarLabel" runat="server" Text='<%# DisplayInvoiceDate(Container.DataItem) %>' />
                                </td>
                                <td align="left">
                                    <asp:Label ID="ctlVendor" runat="server" Text='<%# DisplayVendor(Container.DataItem) %>'
                                        SkinID="SkGeneralLabel" />
                                </td>
                                <td align="right">
                                    <asp:Literal Mode="Encode" ID="ctlTotalAmount" SkinID="SkNumberLabel" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:Literal Mode="Encode" ID="ctlVatAmount" SkinID="SkNumberLabel" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "VATAmount", "{0:#,##0.00}") %>' />
                                </td>
                                <td align="right">
                                    <asp:Literal Mode="Encode" ID="ctlWhtAmount" SkinID="SkNumberLabel" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "WHTAmount", "{0:#,##0.00}") %>' />
                                </td>
                                <td align="right">
                                    <asp:Literal Mode="Encode" ID="ctlNetAmount" SkinID="SkNumberLabel" runat="server" />
                                </td>
                                <td align="center">
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="ctlEdit" runat="server" Text="Edit" CommandName="EditInovice"
                                            CommandArgument='<%# Eval("InvoiceID") %>' />
                                        <asp:LinkButton ID="ctlDelete" runat="server" CommandName="DeleteInvoice" Text="Delete"
                                            CommandArgument='<%# Eval("InvoiceID") %>' OnClientClick="return confirm('Are you sure ?');" />
                                        <asp:LinkButton ID="ctlView" runat="server" Text="View" CommandName="ViewInvoice"
                                            CommandArgument='<%# Eval("InvoiceID") %>' />
                                        <uc1:PopupCaller ID="ctlViewPopupCaller" runat="server" Width="850" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                                        <uc1:PopupCaller ID="ctlEditPopupCaller" runat="server" Width="850" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9" align="right">
                                    <ss:BaseGridView ID="ctlInvoiceItem" ReadOnly="true" runat="server" CssClass="Grid"
                                        AutoGenerateColumns="false" Width="95%" HeaderStyle-HorizontalAlign="Center"
                                        OnDataBound="ctlInvoiceItem_DataBound">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cost Center">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Cost Center")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCostCenter" runat="server" Text='<%# DisplayCostCenter(Container.DataItem) %>'
                                                        SkinID="SkCodeLabel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expense Code">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Expense Code")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlAccountCode" runat="server" Text='<%# DisplayAccount(Container.DataItem) %>'
                                                        SkinID="SkCodeLabel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Internal Order">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Internal Order")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlInternalOrder" runat="server" Text='<%# DisplayIO(Container.DataItem) %>'
                                                        SkinID="SkCodeLabel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Description")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlDescription" runat="server" Text='<%# Eval("Description") %>'
                                                        SkinID="SkGeneralLabel"></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Currency")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCurrency" runat="server" Text='<%# DisplayCurrency(Container.DataItem) %>'
                                                        SkinID="SkCodeLabel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlCurrencyAmount" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ExchangeRate">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("ExchangeRate")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTxtExchangeRate" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AmountLocalCurrency">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountLocalCurrency" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "LocalCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AmountMainCurrency">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountMainCurrency" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AmountTHB">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("AmountTHB")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmount" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ref No.">
                                                <HeaderTemplate>
                                                    <%# GetProgramMessage("Ref No.")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTxtRefNo" runat="server" SkinID="SkGeneralLabel"
                                                        Text='<%# Eval("ReferenceNo") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ctlEditItem" runat="server" Text="Edit" CommandName="EditItem"
                                                        Visible='<%# ShowButton %>' />
                                                    <asp:LinkButton ID="ctlDeleteItem" runat="server" Text="Delete" CommandName="DeleteItem"
                                                        Visible='<%# ShowButton %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </ss:BaseGridView>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table border="0" id="ctlExpenseGeneral" runat="server" width="100%">
            <tr>
                <td align="center">
                    <uc2:SimpleExpense ID="ctlSimpleExpense" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <div id="ctlDivExchangeRateLocalInfo" runat="server" style="display: none">
                        <table width="60%" class="table">
                            <tr>
                                <td align="right" style="width:30%">
                                    <asp:Label ID="ctlExRateLocalCurrencyLabel" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                                <td style="width:5%"></td>
                                <td align="right" style="width:30%">
                                    <asp:Label ID="ctlExchangeRateLocal" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlExchangeRateLocalUnit" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="ctlDivExchangeRateMainInfo" runat="server" style="display: none">
                        <table width="60%" class="table">
                            <tr>
                                <td align="right" style="width:30%">
                                    <asp:Label ID="ctlExRateMainCurrencyLabel" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                                <td style="width:5%"></td>
                                <td align="right" style="width:30%">
                                    <asp:Label ID="ctlExchangeRateMain" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlExchangeRateMainUnit" SkinID="SkFieldCaptionLabel" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="ctlDivAddExpense" runat="server">
                        <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                        <asp:ImageButton ID="ctlAddPerdiem" runat="server" SkinID="SkPerdiemButton" Text="$Perdiem$"
                            OnClick="ctlAddPerdiem_Click" Width="80px" />
                        <asp:ImageButton ID="ctlAddGeneralExpense" runat="server" SkinID="SkGeneralExpenseButton"
                            Text="$General Expense$" OnClick="ctlAddGeneralExpense_Click" Width="130px" />
                        <asp:ImageButton ID="ctlAddMilage" runat="server" SkinID="SkMileageButton" Text="$Mileage$"
                            OnClick="ctlAddMilage_Click" Width="86px" />
                        <uc1:PopupCaller ID="ctlPerdiemPopupCaller" runat="server" Width="850" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                        <uc1:PopupCaller ID="ctlInvoicePopupCaller" runat="server" Width="850" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                        <uc1:PopupCaller ID="ctlMileagePopupCaller" runat="server" Width="850" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: Red;">
                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                    </spring:ValidationSummary>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
