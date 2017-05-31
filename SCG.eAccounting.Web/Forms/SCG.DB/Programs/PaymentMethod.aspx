<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="PaymentMethod.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.PaymentMethod"
    Title="" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" width="45%">
                <fieldset id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 50%">
                                <asp:Label ID="ctlPaymentMethodCodeLabel" runat="server" Text="$PaymentMethodCode$" ></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:TextBox ID="ctlPaymentMethodCodeCri" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="5" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 50%">
                                <asp:Label ID="ctlPaymentMethodNameLabel" runat="server" Text="$PaymentMethodName$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:TextBox ID="ctlPaymentMethodNameCri" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left" width="60%">
                <asp:ImageButton runat="server" ID="ctlPaymentMethodSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlPaymentMethodSearch_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelPaymentMethodGridview" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelPaymentMethodGridview"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <ss:BaseGridView ID="ctlPaymentMethodGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="PaymentMethodID"
                            EnableInsert="False" ReadOnly="true" SelectedRowStyle-BackColor="#6699FF" Width="100%"
                            OnRowCommand="ctlPaymentMethodGrid_RowCommand" OnRequestCount="RequestCount"
                            OnRequestData="RequestData">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="PaymentMethod Code" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentMethodCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPaymentMethodCodeLabel" runat="server" Text='<%# Bind("PaymentMethodCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PaymentMethod Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentMethodName">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPaymentMethodNameLabel" runat="server" Text='<%# Bind("PaymentMethodName") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("EditPaymentMethod") %>' CommandName="PaymentMethodEdit" />
                                        <asp:ImageButton ID="ctlDeletePaymentMethod" runat="server" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" ToolTip='<%# GetProgramMessage("DeletePaymentMethod") %>'
                                            OnClientClick="return confirm('Are you sure delete this row');" CommandName="PaymentMethodDelete" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                    runat="server"></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>
                        <div id="divButton" runat="server" style="vertical-align: middle;">
                            <table style="text-align: center;">
                                <tr>
                                    <td>
                                        <asp:ImageButton runat="server" ID="ctlPaymentMethodAddNew" SkinID="SkCtlFormNewRow"
                                            OnClick="ctlPaymentMethodAddNew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="ctlPaymentMethodFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlPaymentMethodFormPanelHeader" runat="server" Style="cursor: move;
                        background-color: #DDDDDD; border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture" SkinID="SkFieldCaptionLabel" runat="server" Text="Manage Payment Method Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ctlUpdatePanelPaymentMethodForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelPaymentMethodFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelPaymentMethodForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlPaymentMethodForm" runat="server" DataKeyNames="PaymentMethodID"
                                                OnItemCommand="ctlPaymentMethodForm_ItemCommand" OnItemInserting="ctlPaymentMethodForm_ItemInserting"
                                                OnItemUpdating="ctlPaymentMethodForm_ItemUpdating" OnModeChanging="ctlPaymentMethodForm_ModeChanging"
                                                OnDataBound="ctlPaymentMethodForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("PaymentMethodCode")%><font color="red"><asp:Label ID="ctlPaymentMethodCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlPaymentMethodCode" SkinID="SkCtlTextboxCenter" runat="server"
                                                                    MaxLength="5" Text='<%# Bind("PaymentMethodCode")%>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("PaymentMethodName")%><font color="red"><asp:Label ID="ctlPaymentMethodNameRequired"
                                                                    runat="server"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlPaymentMethodName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                                    Text='<%# Bind("PaymentMethodName") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("Active") %>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("UpdatePaymentMethod") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Update" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("CancelPaymentMethod") %>' CommandName="Cancel"
                                                                    Text="Cancel"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("PaymentMethodCode")%><font color="red"><asp:Label ID="ctlPaymentMethodCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlPaymentMethodCode" SkinID="SkCtlTextboxCenter" runat="server"
                                                                    MaxLength="5" Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("PaymentMethodName")%><font color="red"><asp:Label ID="ctlPaymentMethodNameRequired"
                                                                    runat="server"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlPaymentMethodName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%# GetProgramMessage("Active") %>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("InsertPaymentMethod") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("CancelPaymentMethod") %>' CommandName="Cancel"
                                                                    Text="Cancel"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </InsertItemTemplate>
                                            </asp:FormView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <font color="red">
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="PaymentMethod.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
                <ajaxToolkit:ModalPopupExtender ID="ctlPaymentMethodModalPopupExtender" runat="server"
                    TargetControlID="lnkDummy" PopupControlID="ctlPaymentMethodFormPanel" BackgroundCssClass="modalBackground"
                    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlPaymentMethodFormPanelHeader" />
            </td>
        </tr>
    </table>
</asp:Content>
