<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="MaintainRate.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.MaintainRate"
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx"
    TagName="CurrencyDropdown" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <script type="text/javascript">

        function sumExchangeRate(fromAmount, toAmount, exRate) {
            var numFormat = new NumberFormat(0);
            var FromAmount = document.getElementById(fromAmount);
            var tempFromAmount = parseFloat('0' + FromAmount.value.replace(/\,/g, '')).toFixed(2);
            var ToAmount = document.getElementById(toAmount);
            var tempToAmount = parseFloat('0' + ToAmount.value.replace(/\,/g, '')).toFixed(2);
            numFormat.setNumber(parseFloat(tempToAmount / tempFromAmount).toFixed(5));
            numFormat.setPlaces(5);
            document.getElementById(exRate).value = numFormat.toFormatted();
            
        }
    </script>
    <asp:UpdatePanel ID="ctlUpdatePanelMaintainRateGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMaintainRateGrid"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:scgloading id="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlMaintain" runat="server" AutoGenerateColumns="false" CssClass="table"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="Pbid" SelectedRowStyle-BackColor="#6699FF"
                            OnRowCommand="ctlMaintain_RowCommand" OnRequestCount="RequestCount" OnDataBound="PB_DataBound"
                            OnRequestData="RequestData" Width="100%" HorizontalAlign="Left">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="PB Code" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%"
                                    SortExpression="DbPB.PBCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPBCodeLabel" runat="server" Text='<%# Bind("PbCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Height="40%" SortExpression="DbPBLang.Description">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="ctlLinkBtn" Text="Maintain Rate" CausesValidation="False"
                                            CommandName="MaintainRate" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100%" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
            </table>
            <div runat="server" id="ctlDivMaintainInfo" style="display: none">
                <table width="100%" class="table">
                    <tr>
                        <td align="left" style="width: 35%">
                            <fieldset  id="MTInfo" title="Rate Information" class="table">
                                <table width="60%" border="0" class="table">
                                    <tr>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlDateLabel" runat="server" Text="$Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlFromCurrency" runat="server" Text="$MainCurrency$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlFromAmount" runat="server" Text="$FromAmount$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlToCurrency" runat="server" Text="$Currency$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlToAmount" runat="server" Text="$ToAmount$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                        <th align="center" width="3%">
                                            <asp:Label ID="ctlExchangeRate" runat="server" Text="$ExchangeRate$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <uc1:Calendar ID="ctlDatePicker" runat="server" />
                                        </td>
                                        <td align="left">
                                            <uc2:CurrencyDropdown ID="CurrencyDropdown1"  runat="server" IsExpense="true" IsAdvanceFR="false"  />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlFromAmountTextBox" runat="server" 
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 2));" Style="text-align: right;"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                TargetControlID="ctlFromAmountTextBox" FilterType="Custom, Numbers" ValidChars=".,-" />
                                        </td>
                                        <td align="left">
                                            <uc2:CurrencyDropdown ID="CurrencyDropdown2" runat="server" IsExpense="true" IsAdvanceFR="false" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlToAmountTextBox" runat="server" onblur="javascript:sumExchangeRate();" 
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 2));" Style="text-align: right;"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                TargetControlID="ctlToAmountTextBox" FilterType="Custom, Numbers" ValidChars=".,-" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlExchangeRateTextbox" runat="server"  OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 5));"
                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Style="text-align: right;" Enabled="false"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                TargetControlID="ctlToAmountTextBox" FilterType="Custom, Numbers" ValidChars=".,-" />
                                        </td>
                                    </tr>
                                    <tr>                               
                                        <td colspan="6" align="right">                                 
                                            <asp:ImageButton runat="server" ID="ctrAdd" ToolTip="Add" SkinID="SkAddButton" OnClick="ctlAdd_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="400px">
                                    <tr>
                                        <td>
                                            <font color="red" style="text-align: left" class="table">
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="PB.Error">
                                                </spring:ValidationSummary>
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" class="table">
                                    <tr>
                                        <td>
                                            <ss:BaseGridView ID="ctlMaintainInfo" runat="server" AutoGenerateColumns="false"
                                                CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="Pbid" SelectedRowStyle-BackColor="#6699FF"
                                                 OnRequestCount="RequestCountInfo" Width="100%" 
                                                OnRequestData="RequestDataInfo" HorizontalAlign="Left">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%"
                                                        SortExpression="DbPbRate.EffectiveDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlDateLabel" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveDate")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="9%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main Currency" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="7%" SortExpression="FromCurrencySymbol">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlFromCurrencyLabel" runat="server" Text='<%# Bind("FromCurrencySymbol") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%"
                                                        SortExpression="DbPbRate.FromAmount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlFromAmountLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FromAmount", "{0:#,##0.00}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Height="7%" SortExpression="ToCurrencySymbol">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlToCurrencyLabel" runat="server" Text='<%# Bind("ToCurrencySymbol") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="12%"
                                                        SortExpression="DbPbRate.ToAmount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlToAmountLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ToAmount", "{0:#,##0.00}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exchange Rate" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="12%"
                                                        SortExpression="DbPbRate.ExchangeRate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlExchangeRateLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update By" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="14%"
                                                        SortExpression="DbPbRate.UpdateBy">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlUpdateByLabel" runat="server" Text='<%# Bind("UpdateBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="14%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="15%"
                                                        SortExpression="DbPbRate.UpdDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlUpdDateLabel" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("UpdDate"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </ss:BaseGridView>
                                        </td>
                                    </tr>
                                </table>
            </div>
            </fieldset> </td> </tr> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
