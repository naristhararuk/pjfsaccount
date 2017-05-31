<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Menu" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="A">
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlMenuGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRequestData="RequestData" OnRequestCount="RequestCount" ReadOnly="true" EnableInsert="false"
                InsertRowCount="1" DataKeyNames="MenuID" CssClass="table" AllowPaging="true" AllowSorting="true"
                OnRowCommand="ctlMenuGrid_RowCommand" SelectedRowStyle-BackColor="#6699FF" OnDataBound="ctlMenuGrid_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="3%" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MenuCode" ItemStyle-Width="130px" SortExpression="SuMenu.MenuCode">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlMenuCode" runat="server" Text='<%# Eval("MenuCode") %>' CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MenuMainCode" ItemStyle-Width="130px" SortExpression="Main.MenuCode">
                        <ItemTemplate>
                            <asp:Label ID="ctlMenuMainCode" runat="server" Text='<%# Eval("MenuMainCode") %>' CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MenuName" ItemStyle-Width="200px" SortExpression="SuMenuLang.MenuName">
                        <ItemTemplate>
                            <asp:Label ID="ctlMenuName" runat="server" Text='<%# Eval("MenuName") %>' CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="MenuName" HeaderText="MenuName" />--%>
                    <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="SuMenu.Comment"/>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="MenuEdit" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left">
                <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" />
                <span class="spanSeparator">| </span>
                <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="ctlUpdatePanelMenuLangGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
        <fieldset id="fdsMenuLang" style="vertical-align:middle;border-color:Gray;border-style:solid;border-width:1px;font-family:Tahoma;font-size:small;width:100%;" runat="server"> 
            <center>
            <br />
            <ss:BaseGridView ID="ctlMenuLangGrid" runat="server" AutoGenerateColumns="false" Width="98%"
                CssClass="table" ReadOnly="true" DataKeyNames="LanguageId,MenuId" onrowdatabound="ctlMenuLangGrid_RowDataBound">
                <HeaderStyle HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField HeaderText="Language">
                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                        <ItemTemplate>
                            <asp:Label ID="ctlLanguageName" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Eval("LanguageName")%>'></asp:Label>  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MenuName">
                    <ItemStyle Width="31%"/>
                        <ItemTemplate>
                            <asp:TextBox ID="ctlMenuName" Width="98%" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Eval("MenuName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment">
                    <HeaderStyle Width="48%"/>
                        <ItemTemplate>
                            <asp:TextBox ID="ctlCommentMenuLang" SkinID="SkCtlTextboxLeft" Width="98%" runat="server" Text='<%# Eval("Comment") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active">
                    <ItemStyle HorizontalAlign="Center" Width="9%" />
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActiveMenuLang" runat="server" Checked='<%# Eval("Active") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            <div id="div1" runat="server" align="center">
            <table width="98%"><tr align="left"><td>
                <asp:ImageButton runat="server" ID="ctlSubmit" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" />
                <span class="spanSeparator">| </span>
                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
            </td></tr></table>
            </div>
            </center>
        </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlMenuFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlMenuFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlCapture" runat="server" Text="Manage Menu Data" Width="200px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpdatePanelMenuForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ctlUpdatePanelMenuFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMenuForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table class="table" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlMenuForm" runat="server" DataKeyNames="MenuId" OnDataBound="ctlMenuForm_DataBound"
                                    OnItemCommand="ctlMenuForm_ItemCommand" OnItemInserting="ctlMenuForm_Inserting"
                                    OnItemUpdating="ctlMenuForm_Updating" OnModeChanging="ctlMenuForm_ModeChanging">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuHeader" runat="server" Text="$MenuCode$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuCode" SkinID="SkCtlTextboxLeft" runat="server" Width="195px" Text='<%# Eval("MenuCode") %>'/>
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuCode"
                                                        runat="server" ErrorMessage="Required MenuCode" ControlToValidate="ctlMenuCode" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuNameHeader" runat="server" Text="$MenuName$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuName" runat="server" Width="195px" Text='<%# Eval("MenuName") %>'/>
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuName"
                                                        runat="server" ErrorMessage="Required MenuName" ControlToValidate="ctlMenuName" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlProgramHeader" runat="server" Text="$ProgramID$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlProgramID" SkinID="SkCtlDropDownList" runat="server" Width="200px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMainMenuHeader" runat="server" Text="$MainMenuCode$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlMainMenuID" SkinID="SkCtlDropDownList" runat="server" Width="200px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuSeqHeader" runat="server" Text="$MenuSeq$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuSeq" SkinID="SkCtlTextboxLeft" Width="195px" runat="server" Text='<%# Eval("MenuSeq") %>' />
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuSeq"
                                                        runat="server" ErrorMessage="Required MenuSeq" ControlToValidate="ctlMenuSeq" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlCommentHeader" runat="server" Text="$Comment$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" Width="200px" Text='<%# Eval("Comment") %>' TextMode="MultiLine" Rows="3" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveHeader" runat="server" Text="$Active$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>'/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave"
                                                        CommandName="Update" Text="Update"  ValidationGroup="ValidateFormView" CausesValidation="true"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel"
                                                        CommandName="Cancel" Text="Cancel" CausesValidation="false"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuCodeHeader" runat="server" Text="$MenuCode$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuCode" SkinID="SkCtlTextboxLeft" runat="server" Text=""/>
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuCode"
                                                        runat="server" ErrorMessage="Required MenuCode" ControlToValidate="ctlMenuCode" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuNameHeader" runat="server" Text="$MenuName$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuName" runat="server" Width="195px" Text='<%# Eval("MenuName") %>'/>
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuName"
                                                        runat="server" ErrorMessage="Required MenuName" ControlToValidate="ctlMenuName" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlProgramHeader" runat="server" Text="$ProgramID$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlProgramID" runat="server" SkinID="SkCtlDropDownList">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMainMenuIDHeader" runat="server" Text="$MainMenuCode$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlMainMenuID" runat="server" SkinID="SkCtlDropDownList">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlMenuSeqHeader" runat="server" Text="$MenuSeq$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlMenuSeq" runat="server" SkinID="SkCtlTextboxLeft"/>
                                                    <asp:RequiredFieldValidator ID="vldRequireMenuSeq"
                                                        runat="server" ErrorMessage="Required MenuSeq" ControlToValidate="ctlMenuSeq" ValidationGroup="ValidateFormView" Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlCommentHeader" runat="server" Text="$Comment$ :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine" Rows="3" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveHeader" runat="server" Text="$Active$ :"></asp:Label></td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CommandName="Insert"
                                                        Text="Insert" ValidationGroup="ValidateFormView" CausesValidation="true"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CommandName="Cancel"
                                                        Text="Cancel" CausesValidation="False"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:FormView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <font color="red" style="text-align: left">
                                    <asp:ValidationSummary ID="vsSummary" runat="server" Style="text-align: left" Width="250px"
                                    ValidationGroup="ValidateFormView" />
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Menu.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" 
		meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlMenuModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlMenuFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlMenuFormPanelHeader" />
</asp:Content>
