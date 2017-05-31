<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseGroupEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ExpenseGroupEditor" %>
<asp:Panel ID="ctlExpenseGroupEditor" runat="server" Style="display: none" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlExpenseGroupEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditExpenseGroup" SkinID="SkFieldCaptionLabel" runat="server"
                        Text='$Header$' Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlExpenseGroupUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="left">
                        <fieldset style="width: 90%" id="fdsPB">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlExpenseGroupLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Group$'
                                            runat="server"></asp:Label>
                                            
                                            <asp:Label ID="ctlExpenseGroupRequired" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                        <asp:Label ID="ctlColonExpenseGroup" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlExpenseGroup" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"
                                            Text="" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlActiveLabel" SkinID="SkFieldCaptionLabel" Text='$Active$:' runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <ss:BaseGridView ID="ctlExpenseEditorGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" DataKeyNames="LanguageID,ExpenseGroupID"
                            OnDataBound="ExpenseEditor_DataBound" SelectedRowStyle-BackColor="#6699FF" OnRowCommand="Expense_RowCommand"
                            OnRequestData="RequestData" Width="100%">
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
                                        <asp:TextBox ID="ctrDescription"  runat="server" MaxLength="100"
                                            Text='<%# Eval("Description")%>' Width="100%" />
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctrComment"  runat="server" MaxLength="500"
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
                                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Expense.Error" />
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
<ajaxToolkit:ModalPopupExtender ID="ctlExpenseEditorModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlExpenseGroupEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlExpenseGroupEditor" />
