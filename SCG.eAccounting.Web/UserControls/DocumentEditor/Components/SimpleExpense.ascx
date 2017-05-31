<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleExpense.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.SimpleExpense"
    EnableTheming="true" %>
<%@ Register Src="../../DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc4" %>
<%@ Register Src="../../LOV/SCG.DB/AccountLabelLookup.ascx" TagName="AccountLabelLookup"
    TagPrefix="uc5" %>
<%@ Register Src="../../LOV/SCG.DB/CostCenterLabelLookup.ascx" TagName="CostCenterLabelLookup"
    TagPrefix="uc6" %>
<%@ Register Src="../../LOV/SCG.DB/IOLabelLookup.ascx" TagName="IOLabelLookup" TagPrefix="uc7" %>


<%@ Register Src="../../LOV/SCG.DB/AccountField.ascx" TagName="AccountField"
    TagPrefix="uc8" %>
<%@ Register Src="../../LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField"
    TagPrefix="uc9" %>
<%@ Register Src="../../LOV/SCG.DB/IOAutoCompleteLookup.ascx" TagName="IOAutoCompleteLookup" 
    TagPrefix="uc10" %>

<ss:InlineScript ID="ctlInlineScript" runat="server">
<script language="javascript" type="text/javascript">
    function calAmountTHB(currencyAmtID, exchangeRateID, amtThbID) {
        var exchangeRate;
        var currencyAmt;

        if (exchangeRateID.value != "")
            exchangeRate = parseFloat(exchangeRateID.value.replace(/\,/g, '')).toFixed(5);
        else
            exchangeRate = 0;
        if (currencyAmtID.value != "" || currencyAmtID.value != 0)
            currencyAmt = parseFloat(currencyAmtID.value.replace(/\,/g, '')).toFixed(2);
        else {
            exchangeRate = 0;
            currencyAmt = 1;
        }
        amtThbID.value = parseFloat(exchangeRate * currencyAmt).toFixed(2);
        formatCurrency(amtThbID);
    }
</script>
</ss:InlineScript>
<asp:UpdatePanel ID="ctlUpdateSimpleExpenseGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="ctlType" runat="server" />
        <ss:BaseGridView ID="ctlSimpleExpenseGridView" OnRowDataBound="ctlSimpleExpenseGridView_RowDatabound" OnDataBound="ctlSimpleExpenseGridView_Databound"
            runat="server" AutoGenerateColumns="False" EnableInsert="False" InsertRowCount="1"
            SaveButtonID="" Width="100%" CssClass="Grid" ShowFooterWhenEmpty="false" ShowHeaderWhenEmpty="false">
            <HeaderStyle CssClass="GridHeader" />
            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <Columns>
                <asp:TemplateField HeaderText="Cost Center">
                    <ItemTemplate>
                        <uc9:CostCenterField ID="ctlCostCenterLabelLookup" runat="server" DisplayCaption="false" />
                        <asp:HiddenField ID="ctlCostCenter" runat="server" Value='<%# Eval("CostCenterID") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expense Code">
                    <ItemTemplate>
                        <uc8:AccountField ID="ctlAccountLabelLookup" runat="server" />
                        <asp:HiddenField ID="ctlAccountID" runat="server" Value='<%# Eval("AccountID") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="325px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Internal Order">
                    <ItemTemplate>
                        <uc10:IOAutoCompleteLookup ID="ctlIOLabelLookup" runat="server" DisplayCaption="false"/>
                        <asp:HiddenField ID="ctlIOID" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="250px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:TextBox ID="ctlTxtDescription" SkinID="SkGeneralTextBox" runat="server" MaxLength="50"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"/>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Currency">
                    <ItemTemplate>
                        <uc4:CurrencyDropdown ID="ctlCurrencyDropdown" runat="server" IsExpense="true" OnNotifyCurrencyChanged="currencyDropdown_NotifyCurrencyChanged"/>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="ctlTxtCurrencyAmount" SkinID="SkNumberTextBox" Width="80px" runat="server"
                            Text='<%# DataBinder.Eval(Container.DataItem, "CurrencyAmount", "{0:#,##0.00}") %>' OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Exchange Rate">
                    <ItemTemplate>
                        <asp:TextBox ID="ctlTxtExchangeRate" SkinID="SkNumberTextBox" Width="80px" runat="server"
                            Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>' OnKeyPress="return(currencyFormat(this, ',', '.', event, 12,5));"
                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AmountTHB">
                    <ItemTemplate>
                        <asp:TextBox ID="ctlTxtAmountTHB" SkinID="SkNumberTextBox" Width="80px" runat="server" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>' OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ref No.">
                    <ItemTemplate>
                        <asp:TextBox ID="ctlTxtRefNo" SkinID="SkGeneralTextBox" runat="server" Width="80px"
                            MaxLength="30"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </ss:BaseGridView>
    </ContentTemplate>
</asp:UpdatePanel>
