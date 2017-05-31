<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DbRejectReasonEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.FormEditor.SCG.DB.DbRejectReasonEditor" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:Panel ID="ctlReasonFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
    Width="500px">
    <asp:Panel ID="ctlReasonFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlCapture" runat="server" SkinID="SkFieldCaptionLabel" Text="Manage Reason Reject" Width="200px"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlUpdatePanelReasonForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: block;" align="center">
                <asp:UpdateProgress ID="ctlUpdatePanelFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelReasonForm"
                    DynamicLayout="true" EnableViewState="False">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table class="table" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlReasonCodeLabel" runat="server" SkinID="SkGeneralLabel" Text="$ReasonCode$" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlReasonCode" runat="server" MaxLength="20" SkinID="SkGeneralTextBox"
                                            Style="text-align: center;" Text='<%# Bind("ReasonCode") %>' />
                                        <font color="red">
                                            <asp:Label ID="ctlReasonCodeRequired" runat="server" Text="*"></asp:Label></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlRequestTypeLabel" runat="server" SkinID="SkGeneralLabel" Text="$Request Type$" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlRequestTypeDropdown" runat="server" SkinID="SkGeneralDropDown" OnSelectedIndexChanged="ctlRequestTypeDropdown_OnSelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="ctlDocumentTypeLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlStateEventLabel" runat="server" Text="$StateEvent ID$" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlStateEventDropdown" runat="server" SkinID="SkGeneralDropDown">
                                        </asp:DropDownList>
                                        <asp:Label ID="ctlStateEventDropdownLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlRequireLabel" runat="server" Text="$Require$" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlCommentChk" runat="server" Text="$Comment$" Checked='<%# Bind("RequireComment") %>' />
                                        <asp:CheckBox ID="ctlConfirmRejectionChk" runat="server" Text="$Confirm Rejection$"
                                            Checked='<%# Bind("RequireConfirmReject") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlActiveLabel" runat="server" Text="$Active$" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlActivechk" runat="server" Checked='<%# Bind("Active") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <ss:BaseGridView ID="ctlReasonLangGrid" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grid" ReadOnly="true" Width="98%" DataKeyNames="LanguageID,ReasonID"
                                            OnDataBound="ctlReasonLangGrid_DataBound" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                            <InsertRowStyle CssClass="GridItem" HorizontalAlign="left" />
                                            <FooterStyle CssClass="GridItem" HorizontalAlign="left" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Language">
                                                    <ItemTemplate>
                                                        <%# Eval("LanguageName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    <HeaderStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ctlDescription" SkinID="SkGeneralTextBox" runat="server" MaxLength="200"
                                                            Text='<%# Eval("ReasonDetail") %>' Width="98%" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="31%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comment">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ctlCommentLang" runat="server" SkinID="SkGeneralTextBox" MaxLength="500"
                                                            Text='<%# Eval("Comment") %>' Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ctlActiveLang" runat="server" Checked='<%# Eval("Active") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="75px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </ss:BaseGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" OnClick="ctlAdd_Click"
                                            Text="Insert" ToolTip="Submit"></asp:ImageButton>
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click"
                                            Text="Cancel" ToolTip="Cancel"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <font color="red" style="text-align: left">
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ReasonReject.Error" />
                            </font>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlReasonModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlReasonFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlReasonFormPanelHeader" />
