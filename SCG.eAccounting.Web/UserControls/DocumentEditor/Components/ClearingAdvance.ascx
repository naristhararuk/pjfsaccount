<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClearingAdvance.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ClearingAdvance" %>
<%@ Register Src="../../LOV/SCG.DB/TALookup.ascx" TagName="TALookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/Advance.ascx" TagName="Advance"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOV/AV/AdvanceLookup.ascx" TagName="AdvanceLookup"
    TagPrefix="uc4" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc5" %>

<asp:UpdatePanel ID="ctlUpdatePanelExpenseGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelExpenseGeneral"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc5:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:HiddenField ID="ctlType" runat="server" />
        <table border="0" id="ctlExpenseGeneral" width="100%">
            <tr>
                <td align="left" style="width: 80%;" colspan="4">
                    <table width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:Label ID="ctlTANoText" SkinID="SkFieldCaptionLabel" runat="server" Text="$TravellingAutherization$"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="ctlTANoLabel" SkinID="SkGeneralLabel" runat="server" Text="N/A"></asp:Label>
                                <asp:LinkButton ID="ctlTALinkButton" runat="server" SkinID="SkCtlLinkButton"></asp:LinkButton>
                            </td>
                            <td style="width: 3%">
                                <asp:ImageButton ID="ctlTANoLookup" runat="server" SkinID="SkTALookupButton" OnClick="ctlTANoLookup_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ctlDeleteTA" runat="server" SkinID="SkDeleteButton" OnClick="ctlDeleteTA_Click" />
                                <uc1:TALookup ID="ctlTALookup" runat="server" isQueryForExpense="true" isMultiple="false"/>
                                <%--<ss:LabelExtender ID="ctlTALookupExtender" runat="server" LinkControlID="ctlTALookup"
                                    InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                </ss:LabelExtender>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <div id="ctlControlPanel" runat="server">
                <tr>
                    <td align="left" style="width: 20%">
                        <asp:Label ID="ctlPurposeLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Purpose$"></asp:Label>
                        <asp:Label ID="ctlPurposeReq" runat="server" SkinID="SkRequiredLabel" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td align="left" colspan="3" valign="middle" style="width: 83%">
                        <asp:CheckBox ID="ctlBusinessChk" Text="Business" SkinID="SkGeneralCheckBox" runat="server" />&nbsp;
                        <ss:LabelExtender ID="ctlBusinessChkLabelExtender" runat="server" LinkControlID="ctlBusinessChk"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                        <asp:CheckBox ID="ctlTrainingChk" Text="Training" SkinID="SkGeneralCheckBox" runat="server" />&nbsp;
                        <ss:LabelExtender ID="ctlTrainingChkLabelExtender" runat="server" LinkControlID="ctlTrainingChk"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                        <asp:CheckBox ID="ctlOtherChk" Text="Other" SkinID="SkGeneralCheckBox" runat="server" />&nbsp;
                        <ss:LabelExtender ID="ctlOtherChkLabelExtender" runat="server" LinkControlID="ctlOtherChk"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                        <asp:TextBox ID="ctlOther" SkinID="SkGeneralTextBox" runat="server" Width="300px"
                            MaxLength="100"></asp:TextBox>
                        <ss:LabelExtender ID="ctlOtherLabelExtender" runat="server" LinkControlID="ctlOther"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%">
                        <asp:Label ID="ctlFromDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$From Date$"></asp:Label>
                        <asp:Label ID="ctlFromDateReq" SkinID="SkRequiredLabel" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td align="left">
                        <uc3:Calendar ID="ctlFromDateCal" runat="server" />
                        <ss:LabelExtender ID="ctlFromDateCalLabelExtender" runat="server" LinkControlID="ctlFromDateCal"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%">
                        <asp:Label ID="ctlToDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$To Date$"></asp:Label>
                        <asp:Label ID="ctlToDateReq" SkinID="SkRequiredLabel" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td align="left">
                        <uc3:Calendar ID="ctlToDateCal" runat="server" />
                        <ss:LabelExtender ID="ctlToDateCalLabelExtender" runat="server" LinkControlID="ctlToDateCal"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="ctlCountryLabel" runat="server" Text="$Country$" SkinID="SkFieldCaptionLabel"></asp:Label><asp:Label
                            ID="ctlCountryReq" SkinID="SkRequiredLabel" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td align="left">
                        <asp:TextBox ID="ctlCountry" SkinID="SkGeneralTextBox" runat="server" MaxLength="100"></asp:TextBox>
                        <ss:LabelExtender ID="ctlCountryLabelExtender" runat="server" LinkControlID="ctlCountry"
                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="ctlPersonLevelLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Personal Level$"></asp:Label>&nbsp;:&nbsp;&nbsp;
                    </td>
                    <td align="left">
                        <asp:Label ID="ctlPersonLevel" SkinID="SkGeneralTextBox" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="ctlExchangeRateForPerdiemLabel" SkinID="SkFieldCaptionLabel" runat="server"
                            Text="$Exchange Rate for Perdiem Calculation$"></asp:Label>&nbsp;:&nbsp;&nbsp;
                        <asp:Label ID="ctlExchangeRateForPerdiem" SkinID="SkNumberTextBox" runat="server"></asp:Label>
                    </td>
                    <td align="left" style="width: 20%">
                    </td>
                    <td align="left">
                    </td>
                </tr>
            </div>
            <tr>
                <td colspan="2">
                    <%--<asp:Button ID="ctlAddAdvance" OnClick="ctlAddAdvance_Click" Text="Add Advance" runat="server"
                        SkinID="SkGeneralButton" />--%>
                        <asp:ImageButton ID="ctlAddAdvance" runat="server" SkinID="SkAddButton" OnClick="ctlAddAdvance_Click"/>
                    <uc4:AdvanceLookup ID="ctlAdvanceLookup" runat="server" isMultiple="true" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <ss:BaseGridView ID="ctlAdvanceGridView" runat="server" OnRowCommand="ctlAdvanceGridview_RowCommand"
                        OnRowDataBound="ctlAdvanceGridview_RowDataBound" AutoGenerateColumns="False"
                        OnDataBound="ctlAdvanceGridview_DataBound" DataKeyNames="AdvanceID,RemittanceID,WorkflowID"
                        EnableInsert="False" InsertRowCount="1" 
                        ShowMsgDataNotFound = "false"
                        SaveButtonID="" Width="100%" CssClass="Grid">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <Columns>
                            <asp:TemplateField HeaderText="No.">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlNoLabel" runat="server"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Advance No.">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ctlLbtnAdvanceNo" SkinID="SkCtlLinkButton" runat="server" Text='<%# Bind("AdvanceNo") %>' CommandName="PopupDocument"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctllblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="cltLblDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDateOfRemittance")) %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount(VND)">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblLocalAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LocalCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount(USD)">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblMainAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount(THB)">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Literal>
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
                <td align="left" colspan="4">
                    <ss:BaseGridView ID="ctlRemittanceGridview" runat="server" OnRowCommand="ctlRemittanceGridview_RowCommand"
                        OnRowDataBound="ctlRemittanceGridview_RowDataBound" AutoGenerateColumns="False"
                        OnDataBound="ctlRemittanceGridview_DataBound" 
                        ShowMsgDataNotFound = "false"
                        DataKeyNames="RemittanceID,WorkflowID" EnableInsert="False"
                        InsertRowCount="1" SaveButtonID="" Width="100%" CssClass="Grid">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <Columns>
                            <asp:TemplateField HeaderText="No.">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlNoLabel" runat="server"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remittance No.">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ctlLbtnRemittanceNo" SkinID="SkCtlLinkButton" runat="server" CommandName="PopupDocument" Text='<%# Bind("RemittanceNo")%>'></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PaymentType">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctllblPaymentType" runat="server" Text='<%# Bind("PaymentType") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctllblCurrency" runat="server" Text='<%# Bind("Currency") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Foreign Currency Advanced">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblForeignCurrencyAdvanced" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ForeignCurrencyAdvanced", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExchangeRate">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Foreign Currency Remitted">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblForeignCurrencyRemitted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ForeignCurrencyRemitted", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RemittedAmount(USD)">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblAmountMain" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemittanceAmountMainCurrency", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RemittedAmount(THB)">
                                <ItemTemplate>
                                    <asp:Literal Mode="Encode" ID="ctlLblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemittanceAmountTHB", "{0:#,##0.00}") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </ss:BaseGridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
