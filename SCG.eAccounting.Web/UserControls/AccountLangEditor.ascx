<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountLangEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.AccountLangEditor" %>
<style type="text/css">
    .style1
    {
        width: 31px;
    }
    .style2
    {
        width: 22px;
    }
    .style3
    {
        width: 64px;
    }
</style>
<asp:Panel ID="ctlAccountLangEditor" runat="server" Style="display: block" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlAccountLangFormHeader" runat="server" Style="cursor: move; border: solid 1px Gray;
                    color: Black" Width="100%">
                    <asp:Label ID="ctlAccountLang" SkinID="SkFieldCaptionLabel" runat="server" Text='$Header$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlAccountLangUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="left">
                        <fieldset id="fdsPB">
                            <table width="100%" border="0">
                                <tr>
                                    <td align="left"  style="width:90px">
                                        <asp:Label ID="ctlExpenseCodeLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Code$'
                                            runat="server"></asp:Label>
                                        <asp:Label ID="ctlExpenseCodeRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                        <asp:Label ID="ctlColonExpenseCode" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlExpenseCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"
                                            Text="" Width="100px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                        <asp:Label ID="ctlTypeExpense" SkinID="SkFieldCaptionLabel" Text='$Active$:' runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlActive" SkinID="SkGeneralCheckBox" runat="server" Checked="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                    </td>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="width: 200px">
                                                    <asp:CheckBox ID="ctlSaveAsDebtor" SkinID="SkGeneralCheckBox" Text='$บันทึกค่าใช้จ่ายเป็นลูกหนี้$:'
                                                        TextAlign="Right" runat="server" Checked="false" />
                                                </td>
                                                <td align="left" style="width:150px">
                                                    <asp:Label ID="ctlSpecialGLLabel" SkinID="SkFieldCaptionLabel" Text='$Special GL$'
                                                        runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ctlspecialGL" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="10"
                                                        Text="" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                    </td>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="width: 170px">
                                                    <asp:CheckBox ID="ctlSaveAsVendor" SkinID="SkGeneralCheckBox" Text='Save As Vendor:'
                                                        TextAlign="Right" runat="server" Checked="false" />
                                                </td>
                                                <%--<td class="style1">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="ctlSpecialGLLabel" SkinID="SkFieldCaptionLabel" Text='$Special GL$'
                                                        runat="server"></asp:Label>
                                                </td>
                                                <td class="style3">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ctlspecialGL" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="10"
                                                        Text="" Width="100px" />
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                    </td>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="width: 200px">
                                                    <asp:CheckBox ID="ctlDomesticRecommend" SkinID="SkGeneralCheckBox" Text='$ค่าใช้จ่ายแนะนำในประเทศ$:'
                                                        TextAlign="Right" runat="server" Checked="false" />
                                                </td>
                                                <td align="left" style="width:150px">
                                                    <asp:Label ID="ctlSAPSpecialGLAssignmentLabel" SkinID="SkFieldCaptionLabel" Text='$SpecialGL Assignment$'
                                                        runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ctlSAPSpecialGLAssignment" SkinID="SkCtlTextboxLeft" runat="server"
                                                        MaxLength="10" Text="" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                    </td>
                                    <td style="width: 180px">
                                        <asp:CheckBox ID="ctlForeignRecommend" SkinID="SkGeneralCheckBox" Text='$ค่าใช้จ่ายแนะนำต่างประเทศ$:'
                                            TextAlign="Right" runat="server" Checked="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                        <asp:Label ID="ctlTaxCodeLabel" SkinID="SkFieldCaptionLabel" Text='$Tax Code$' runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlTaxCode" runat="server" SkinID="SkGeneralDropdown" Width="100px">
                                            <asp:ListItem Text="None" Value="0" />
                                            <asp:ListItem Text="Require" Value="1" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                        <asp:Label ID="ctlCostCenterLabel" SkinID="SkFieldCaptionLabel" Text='$Cost Center$'
                                            runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlCostCenter" runat="server" SkinID="SkGeneralDropdown" Width="100px">
                                            <asp:ListItem Text="None" Value="0" />
                                            <asp:ListItem Text="Require" Value="1" />
                                            <asp:ListItem Text="Optional" Value="2" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                        <asp:Label ID="ctlInternalOrderLabel" SkinID="SkFieldCaptionLabel" Text='$Internal Order$'
                                            runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlInternalOrder" runat="server" SkinID="SkGeneralDropdown"
                                            Width="100px">
                                            <asp:ListItem Text="None" Value="0" />
                                            <asp:ListItem Text="Require" Value="1" />
                                            <asp:ListItem Text="Optional" Value="2" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:90px">
                                        <asp:Label ID="ctlSaleOrderLabel" SkinID="SkFieldCaptionLabel" Text='$Sale Order$'
                                            runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlSaleOrder" runat="server" SkinID="SkGeneralDropdown" Width="100px">
                                            <asp:ListItem Text="None" Value="0" />
                                            <asp:ListItem Text="Require" Value="1" />
                                            <asp:ListItem Text="Optional" Value="2" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                                    <br />
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <ss:BaseGridView ID="ctlAccountLangGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" DataKeyNames="LanguageID,ExpenseGroupID"
                            SelectedRowStyle-BackColor="#6699FF" OnRequestData="RequestData" OnDataBound="ExpenseLangEditor_DataBound"
                            Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlUserProfileCodeLabel" runat="server" Text='<%# Eval("LanguageName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctrDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                            Text='<%# Eval("AccountName")%>' Width="100%" />
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctrComment" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="500"
                                            Text='<%# Eval("Comment")%>' Width="100%" />
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Eval("Active") %>' runat="server" Enabled="true" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table width="100%" class="table">
                            <tr>
                                <td align="left" style="width: 60%">
                                    <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkSaveButton" OnClick="ctlAdd_Click" />
                                    <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                                        OnClick="ctlCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <font color="red">
                                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Account.Error" />
                                    </font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="ctlAccountLangModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlAccountLangEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlAccountLangEditor" />
