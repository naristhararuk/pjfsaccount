<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpensesMPA.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ExpensesMPA" %>
<%@ Register Src="../../LOV/SCG.DB/TALookup.ascx" TagName="TALookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/Advance.ascx" TagName="Advance"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOV/SCG.eAccounting/MPADocumentLookup.ascx" TagName="MPALookup"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc5" %>
<asp:UpdatePanel ID="ctlUpdatePanelExpenseGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelExpenseGeneral"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc5:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:HiddenField ID="ctlType" runat="server" />
        <fieldset id="ctlMPAField" runat="server" class="table">
        <legend id="ctlLegendSearchCriteria" style="color: #4E9DDF">
            <asp:Label ID="ctlMPAHeader" runat="server" Text="$HEADER$" />
        </legend>
        <table border="0" id="ctlExpenseGeneral" width="100%">
            <tr>
                <td>
                    <asp:ImageButton ID="ctlAddExpenseMPA" runat="server" SkinID="SkAddButton" OnClick="ctlctlAddExpenseMPA_Click" />
                    <%--OnClick="ctlAddAdvance_Click"--%>
                    <uc4:MPALookup ID="ctlMPADocumentLookup" runat="server" isMultiple="true" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <ss:BaseGridView ID="ctlExpeseMPAGridView" runat="server" DataKeyNames="MPADocumentID , WorkflowID"
                        EnableInsert="False" InsertRowCount="1" ShowMsgDataNotFound="false" SaveButtonID=""
                        AutoGenerateColumns="false" Width="100%" CssClass="Grid" OnRowDataBound="ctlExpeseMPAGridView_DataBound" 
                        ShowHeaderWhenEmpty="true" OnRowCommand="ctlctlAddExpenseMPA_RowCommand" >
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
                            <asp:TemplateField HeaderText="MPA No.">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ctlGridDocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>' CommandName="PopupDocument"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate>
                                    <asp:Label ID="ctlGridDescription" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Request Date">
                                <ItemTemplate>
                                    <asp:Label ID="ctlGridDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" ToolTip="Delete"
                                        CommandName="DeleteExpensesMPA" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </ss:BaseGridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
