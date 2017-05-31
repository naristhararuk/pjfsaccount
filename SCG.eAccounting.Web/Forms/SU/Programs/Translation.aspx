<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master"
    CodeBehind="Translation.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Translation"
    EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="A">
    <table width="100%">
        <tr>
            <td align="left" style="width: 45%">
                <fieldset style="width: 90%" id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlProgramCodeLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$ProgramCode$"></asp:Label>
                                <asp:Label ID="ctlColonProgramCode" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlProgramCode" SkinID="SkGeneralTextBox" Width="150" MaxLength="50"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="ctlSymbolLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Symbol$"></asp:Label>
                                <asp:Label ID="ctlColonSymbol" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlSymbol" SkinID="SkGeneralTextBox" Width="200" MaxLength="200"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlControlLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Control$"></asp:Label>
                                <asp:Label ID="ctlColonControl" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlControlName" SkinID="SkGeneralTextBox" Width="200" MaxLength="200"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlTranslateSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlTranslateSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlGlobalTranslateGrid" runat="server" AutoGenerateColumns="False"
                OnRequestData="RequestData" OnRequestCount="RequestCount" ReadOnly="true" EnableInsert="false"
                InsertRowCount="1" DataKeyNames="TranslateId" CssClass="table" AllowPaging="true"
                AllowSorting="true" OnRowCommand="ctlGlobalTranslateGrid_RowCommand" OnDataBound="ctlGlobalTranslateGrid_DataBound"
                SelectedRowStyle-BackColor="#6699FF" Width="100%" OnPageIndexChanged="ctlGlobalTranslateGrid_PageIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <HeaderStyle Width="25px" HorizontalAlign="Center" />
                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ProgramCode" SortExpression="ProgramCode">
                        <ItemTemplate>
                            <asp:Label ID="ctlProgramCode" runat="server" Text='<%# Eval("ProgramCode") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="150px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TranslateSymbol" SortExpression="TranslateSymbol">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlTranslateSymbol" runat="server" Text='<%# Eval("TranslateSymbol") %>'
                                CommandName="Select" />
                        </ItemTemplate>
                        <ItemStyle Width="225px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TranslateControl" SortExpression="TranslateControl">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlTranslateControl" runat="server" Text='<%# Eval("TranslateControl") %>'
                                CommandName="Select" />
                        </ItemTemplate>
                        <ItemStyle Width="225px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="Comment" ItemStyle-Wrap="true" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" Visible="false"
                                CommandName="TranslateEdit" ToolTip='<%# GetProgramMessage("EditTranslate") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="45px" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="display:none">
                <table border="0">
                    <tr>
                        <td valign="middle">
                            <asp:ImageButton runat="server" ID="ctlAddNew" Visible="false" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                        </td>
                        <td valign="middle">
                            |
                        </td>
                        <td valign="middle">
                            <asp:ImageButton ID="ctlDelete" runat="server" Visible="false" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="ctlUpdatePanelTranslateLangGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server" />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelTranslateLangGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 100%" id="ctlFieldSetDetailGridView" runat="server" visible="False"
                enableviewstate="True">
                <legend id="ctlLegendDetailGridView" runat="server" style="color: #4E9DDF" visible="True">
                    <asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$" /></legend>
                <center>
                    <ss:BaseGridView ID="ctlTranslateLangGrid" runat="server" AutoGenerateColumns="false"
                        CssClass="table" ReadOnly="true" DataKeyNames="LanguageId,TranslateId" Width="98%"
                        OnDataBound="ctlTranslateLangGrid_DataBound" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Language">
                                <ItemTemplate>
                                    <%# Eval("LanguageName")%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TranslateWord">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlTranslateWord" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="500" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                        Text='<%# Eval("TranslateWord") %>' Width="98%" />
                                </ItemTemplate>
                                <ItemStyle Width="31%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlCommentTranslateLang" SkinID="SkCtlTextboxLeft" runat="server"
                                        MaxLength="500" Text='<%# Eval("Comment") %>' Width="98%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlActiveTranslateLang" runat="server" Checked='<%# Eval("Active") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="75px" />
                                <HeaderStyle Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </ss:BaseGridView>
                </center>
                <table border="0">
                    <tr>
                        <td valign="middle">
                            <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click"
                                Visible="false" />
                        </td>
                        <td valign="middle">
                            |
                        </td>
                        <td valign="middle">
                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click"
                                Text="Cancel" Visible="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlTranslateFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlTranslateFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlCapture" runat="server" Text="Manage Global Translate Data" Width="200px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpdatePanelTranslateForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ctlUpdatePanelTranslateFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelTranslateForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table class="table" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlTranslateForm" runat="server" DataKeyNames="TranslateId" OnDataBound="ctlTranslateForm_DataBound"
                                    OnItemCommand="ctlTranslateForm_ItemCommand" OnItemInserting="ctlTranslateForm_Inserting"
                                    OnItemUpdating="ctlTranslateForm_Updating" OnModeChanging="ctlTranslateForm_ModeChanging">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlProgramLabel" runat="server" Text='<%# GetProgramMessage("$ProgramCode$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <%--<asp:DropDownList ID="ctlProgramList" SkinID="SkCtlDropDownList" runat="server">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="ctlProgramCodeText" runat="server" MaxLength="200" SkinID="SkGeneralTextBox" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlSymbolLabel" runat="server" Text='<%# GetProgramMessage("$Symbol$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSymbol" SkinID="SkGeneralTextBox" runat="server" MaxLength="200"
                                                        Text='<%# Eval("TranslateSymbol") %>' />
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlTranslateControlLabel" runat="server" Text='<%# GetProgramMessage("$TranslateControl$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTranslateControl" SkinID="SkGeneralTextBox" runat="server" MaxLength="200"
                                                        Text='<%# Eval("TranslateControl") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("$Comment$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkGeneralTextBox" runat="server" TextMode="MultiLine"
                                                        Height="50px" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" Text='<%# Eval("Comment") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveLabel" runat="server" Text='<%# GetProgramMessage("$Active$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("UpdateToolTip") %>'>
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>'>
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlProgramLabel" runat="server" Text='<%# GetProgramMessage("$ProgramCode$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <%--<asp:DropDownList ID="ctlProgramList" runat="server" SkinID="SkCtlDropDownList">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="ctlProgramCodeText" runat="server" MaxLength="200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlSymbolLabel" runat="server" Text='<%# GetProgramMessage("$Symbol$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSymbol" runat="server" MaxLength="200" SkinID="SkCtlTextboxLeft" />
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlTranslateControlLabel" runat="server" Text='<%# GetProgramMessage("$TranslateControl$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTranslateControl" runat="server" MaxLength="200" SkinID="SkCtlTextboxLeft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("$Comment$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine"
                                                        Height="50px" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveLabel" runat="server" Text='<%# GetProgramMessage("$Active$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CommandName="Insert"
                                                        Text="Insert" ToolTip='<%# GetProgramMessage("SaveToolTip") %>'></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CommandName="Cancel"
                                                        Text="Cancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>'></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:FormView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:ValidationSummary ID="vsSummary" runat="server" Style="text-align: left" Width="250px"
                                    ValidationGroup="ValidateFormView" />--%>
                                <font color="red" style="text-align: left">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="GlobalTranslate.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlTranslateModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlTranslateFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlTranslateFormPanelHeader" />
</asp:Content>
