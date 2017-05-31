<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentDetail.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.PaymentDetail" %>
<%@ Register Src="../../DropdownList/SCG.DB/CounterCashier.ascx" TagName="CounterCashier"
    TagPrefix="uc2" %>
<%@ Register Src="../../DropdownList/SCG.DB/StatusDropdown.ascx" TagName="StatusDropdown"
    TagPrefix="uc1" %>
<style type="text/css">
    .style1
    {
        width: 63%;
    }
</style>
<asp:UpdatePanel ID="ctlUpdatePanelPaymentDetail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table border="0" id="ctlPaymentDetailTable" runat="server">
            <tr>
                <td valign="top">
                    <fieldset id="ctlFieldSetDetailGridView" runat="server" style="text-align: center;"
                        class="table">
                        <legend id="ctlLegendDetailGridView" style="color: #4E9DDF;">
                            <asp:Label ID="lblDetailHeader" runat="server" SkinID="SkFieldCaptionLabel" Text="$Payment Detail$ :"></asp:Label>
                            <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                        </legend>
                        <asp:HiddenField ID="ctlType" runat="server" />
                        <table border="0" style="width: 100%">
                            <tr>
                                <td valign="top" style="width: 45%">
                                    <div id="DivTotalSummaryForThailand" runat="server">
                                        <table width="100%" border="0" style="height:auto">
                                            <tr>
                                                <td align="left" style="width: 45%">
                                                    <asp:Label ID="ctlLblTotalExpense" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("LblTotalExpense") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="ctlTotalExpense" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 45%">
                                                    <asp:Label ID="ctlLblTotalAdvance" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("LblTotalAdvance") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="ctlTotalAdvance" runat="server" SkinID="SkNumberLabel" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 45%" valign="top">
                                                    <asp:Label ID="ctlRemitted" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("LblTotalRemittance") %>' /><br />
                                                    <asp:Label ID="ctlMsgSummary" runat="server" SkinID="SkFieldCaptionLabel" Text="$Diffrence Amount$" />
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlTotalRemitted" runat="server" SkinID="SkNumberLabel" /><br />
                                                    <asp:Label ID="ctlDifferenceAmount" runat="server" SkinID="SkNumberLabel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="DivTotalSummaryForRepOffice" runat="server" align="left">
                                        <ajaxToolkit:TabContainer ID="X" runat="server" ActiveTabIndex="0" Height="80">
                                            <ajaxToolkit:TabPanel runat="server" ID="ctlLocalCurrencyTab" HeaderText="LocalCurrency"
                                                SkinID="SkTab">
                                                <ContentTemplate>
                                                    <table width="100%" class="table">
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalExpenseLocal" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalExpense") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalExpenseLocal" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalAdvanceLocal" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalAdvance") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalAdvanceLocal" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%" valign="top">
                                                                <asp:Label ID="ctlLblTotalRemittanceLocal" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("LblTotalRemittance") %>' /><br />
                                                                <asp:Label ID="ctlLblDifferenceAmtLocal" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text="$Diffrence Amount$" />
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="ctlTotalRemittanceLocal" runat="server" SkinID="SkNumberLabel"/><br />
                                                                <asp:Label ID="ctlDifferenceAmtLocal" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </ajaxToolkit:TabPanel>
                                            <ajaxToolkit:TabPanel runat="server" ID="ctlMainCurrencyTab" HeaderText="MainCurrency"
                                                SkinID="SkTab">
                                                <ContentTemplate>
                                                    <table width="100%" class="table">
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalExpenseMainCurrency" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalExpense") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalExpenseMainCurrency" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalAdvanceMainCurrency" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalAdvance") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalAdvanceMainCurrency" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%" valign="top">
                                                                <asp:Label ID="ctlLblTotalRemittanceMainCurrency" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("LblTotalRemittance") %>' /><br />
                                                                <asp:Label ID="ctlLblDifferenceAmtMainCurrency" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text="$Diffrence Amount$" />
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="ctlTotalRemittanceMainCurrency" runat="server" SkinID="SkNumberLabel" /><br />
                                                                <asp:Label ID="ctlDifferenceAmtMainCurrency" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </ajaxToolkit:TabPanel>
                                            <ajaxToolkit:TabPanel runat="server" ID="ctlTHBCurrencyTab" HeaderText="THB" SkinID="SkTab">
                                                <ContentTemplate>
                                                    <table width="100%" class="table">
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalExpenseTHB" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalExpense") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalExpenseTHB" runat="server" SkinID="SkNumberLabel"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%">
                                                                <asp:Label ID="ctlLblTotalAdvanceTHB" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text='<%# GetProgramMessage("LblTotalAdvance") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTotalAdvanceTHB" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 45%" valign="top">
                                                                <asp:Label ID="ctlLblTotalRemittanceTHB" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("LblTotalRemittance") %>' /><br />
                                                                <asp:Label ID="ctlLblDifferenceAmtTHB" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text="$Diffrence Amount$" />
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="ctlTotalRemittanceTHB" runat="server" SkinID="SkNumberLabel" /><br />
                                                                <asp:Label ID="ctlDifferenceAmtTHB" runat="server" SkinID="SkNumberLabel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </ajaxToolkit:TabPanel>
                                        </ajaxToolkit:TabContainer>
                                    </div>
                                </td>
                                <td valign="top" style="width: 55%">
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 45%">
                                                <asp:Label ID="ctlLblServiceTeam" SkinID="SkFieldCaptionLabel" runat="server" Text="$Service Team$"></asp:Label>
                                                <asp:Label ID="ctlLblServiceTeamReq" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left" style="width: 55%">
                                                <asp:DropDownList ID="ctlDdlServiceTeam" runat="server" Width="180px" SkinID="SkGeneralDropdown">
                                                </asp:DropDownList>
                                                <ss:LabelExtender ID="ctlDdlServiceTeamLabelExtender" runat="server" LinkControlID="ctlDdlServiceTeam"
                                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 45%">
                                                <asp:Label ID="ctlLblPaymentType" runat="server" SkinID="SkFieldCaptionLabel" Text="$Payment Type$"></asp:Label>
                                                <asp:Label ID="ctlLblPaymentTypeReq" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left" style="width: 55%">
                                                <asp:DropDownList ID="ctlDdlPaymentType" SkinID="SkGeneralDropdown" Width="180px"
                                                    OnSelectedIndexChanged="ctlLblPaymentType_SelectedIndexChanged" runat="server"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <ss:LabelExtender ID="ctlDdlPaymentTypeLabelExtender" runat="server" LinkControlID="ctlDdlPaymentType"
                                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 45%">
                                                <div id="divCounterCashierLabel">
                                                    <asp:Panel ID="ctlPanelCounterCashier" runat="server">
                                                        <asp:Label ID="ctlLblCounterCashier" runat="server" SkinID="SkFieldCaptionLabel"
                                                            Text="$Counter Cashier$"></asp:Label>
                                                        <asp:Label ID="ctlLblCounterCashierReq" runat="server" Text="*" Style="color: Red;" />&nbsp;:&nbsp;
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td align="left" style="width: 55%">
                                                <div id="divCounterCashier">
                                                    <asp:Panel ID="ctlPanelDdlCounterCashier" runat="server">
                                                        <%--<uc2:CounterCashier ID="ctlDdlCounterCashier" runat="server" />--%>
                                                        <asp:DropDownList ID="ctlDdlCounterCashier" runat="server" SkinID="SkGeneralDropdown" OnSelectedIndexChanged="ctlDdlCounterCashier_SelectedIndexChanged" AutoPostBack ="true"></asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlDdlCounterCashierLabelExtender" runat="server" LinkControlID="ctlDdlCounterCashier"
                                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.CounterCashier %>'></ss:LabelExtender>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:Panel ID="Panel3" runat="server">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <p>
                                                        <asp:Label ID="ctlDateLabel" runat="server" Visible="false" Text="$Date$" SkinID="SkFieldCaptionLabel" />
                                                        <asp:Label ID="ctlDate" runat="server" SkinID="SkGeneralLabel" />
                                                        <asp:Label ID="ctlBankNameLabel" runat="server" Visible="false" Text="$Bank$" SkinID="SkFieldCaptionLabel" />
                                                        <asp:Label ID="ctlBankName" runat="server" SkinID="SkGeneralLabel" />
                                                        <asp:Label ID="ctlAccountBankNameLabel" Visible="false" runat="server" Text="$Account No.$"
                                                            SkinID="SkFieldCaptionLabel" />
                                                        <asp:Label ID="ctlAccountBankName" runat="server" SkinID="SkGeneralLabel" />
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
