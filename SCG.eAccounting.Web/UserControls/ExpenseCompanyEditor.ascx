<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseCompanyEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ExpenseCompanyEditor" %>
<%@ Register Src="LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc2" %>
<asp:Panel ID="ctlPanelExpenseCompanyEditor" runat="server" Style="display: block"
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlExpenseCompanyEditorFormHeader" CssClass="table" runat="server"
                    Style="cursor: move; border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditExpenseCompany" SkinID="SkFieldCaptionLabel" runat="server"
                        Text='$Header$' Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="table">
                <tr>
                    <td align="left">
                        <asp:Label ID="Label3" Text="$Company Code$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        <asp:Label ID="ctlCompanyReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                    </td>
                    <td align="left" style="margin-left: 40px">
                        <asp:Label ID="ctlCompanyCode" runat="server" SkinID="SkGeneralLabel" />
                        <asp:Label ID="ctlCompanyName" runat="server" SkinID="SkGeneralLabel" />
                        <uc2:CompanyField id="ctlCompanyField" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="Label1" Text="$Use Parent$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                      &nbsp:&nbsp
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="ctlUseParent" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="Label4" Text="$Tax Code$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        &nbsp:&nbsp
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ctlTaxCode" runat="server" SkinID="SkGeneralDropdown" Width="100px">
                            <asp:ListItem Text="None" Value="0" />
                            <asp:ListItem Text="Require" Value="1" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="Label6" Text="$Cost Center$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        &nbsp:&nbsp
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
                    <td align="left">
                        <asp:Label ID="Label8" Text="$Internal Order$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        &nbsp:&nbsp
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
                    <td align="left">
                        <asp:Label ID="Label10" Text="$Sale Order$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        &nbsp:&nbsp
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ctlSaleOrder" runat="server" SkinID="SkGeneralDropdown" Width="100px">
                            <asp:ListItem Text="None" Value="0" />
                            <asp:ListItem Text="Require" Value="1" />
                            <asp:ListItem Text="Optional" Value="2" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table width="100%" class="table">
                            <tr>
                                <td align="left" style="width: 100%">
                                    <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkSaveButton" OnClick="ctlAdd_Click" />
                                    <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                                        OnClick="ctlCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <font color="red">
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ExpenseCompany.Error" />
                        </font>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="ctlExpenseCompanyModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlPanelExpenseCompanyEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlPanelExpenseCompanyEditor" />
