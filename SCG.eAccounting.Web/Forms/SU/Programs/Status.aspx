<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
CodeBehind="Status.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Status" 
EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/StaticAlertMessage.ascx" tagname="AlertMessage" tagprefix="uc2" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<br />
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="true">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlStatusGrid" runat="server" AutoGenerateColumns="False"
                OnRequestData="RequestData" OnRequestCount="RequestCount" ReadOnly="true" EnableInsert="false"
                InsertRowCount="1" DataKeyNames="StatusID" CssClass="table" AllowPaging="true" AllowSorting="true"
                EnableViewState="true" Width="100%" SelectedRowStyle-BackColor="#6699FF" OnDataBound="ctlStatusGrid_DataBound"
                OnRowCommand="ctlStatusGrid_RowCommand" OnPageIndexChanged="ctlStatusGrid_PageIndexChanged">
               <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
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
                    
                    <asp:BoundField DataField="GroupStatus" HeaderText="GroupStatus" SortExpression="GroupStatus" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" />
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlStatus" runat="server" Text='<%# Eval("Status") %>'
                                CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" HeaderStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75px" HeaderText="Active" SortExpression="Active" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="StatusEdit" ToolTip='<%# GetProgramMessage("Edit")%>'/>
                        </ItemTemplate>
                        <ItemStyle Width="45px" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="text-align:left;width:100%">
				<table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click"/>
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click"/>
						</td>
					</tr>
				</table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="ctlUpdatePanelStatusLangGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div id="divMessage" runat="server" style="display:none">
            <uc2:AlertMessage ID="AlertMessage1" runat="server"/>
        </div>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelStatusLangGridView"
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
                    <ss:BaseGridView ID="ctlStatusLangGrid" runat="server" AutoGenerateColumns="false"
                        CssClass="table" ReadOnly="true" DataKeyNames="LanguageId,StatusID" Width="98%"
                        OnDataBound="ctlStatusLangGrid_DataBound" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Language">
                                <ItemTemplate>
                                    <%# Eval("LanguageName")%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StatusDesc">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlStatusDesc" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="200" Text='<%# Eval("StatusDesc") %>'
                                        Width="98%" />
                                </ItemTemplate>
                                <ItemStyle Width="31%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlCommentStatusLang" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="500" Text='<%# Eval("Comment") %>'
                                        Width="98%"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlActiveStatusLang" runat="server" Checked='<%# Eval("Active") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="75px" />
                                <HeaderStyle Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </ss:BaseGridView>
                </center>
                <div id="div1" runat="server" style="text-align:left;width:100%">
                <table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" Visible="false" ToolTip='<%# GetProgramMessage("Save") %>'/>
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" Text="Cancel" Visible="false" ToolTip='<%# GetProgramMessage("Cancel") %>'/>
						</td>
					</tr>
                </table>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlStatusFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlStatusFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div style="text-align:left">
                <p>
                    <asp:Label ID="ctlStatusFormHeader" runat="server" Text="$Manage Status Language$" Width="200px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpdatePanelStatusForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ctlUpdatePanelStatusFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelStatusForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table class="table" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlStatusForm" runat="server" DataKeyNames="StatusID" OnDataBound="ctlStatusForm_DataBound"
                                    OnItemCommand="ctlStatusForm_ItemCommand" OnItemInserting="ctlStatusForm_Inserting"
                                    OnItemUpdating="ctlStatusForm_Updating" OnModeChanging="ctlStatusForm_ModeChanging">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlGroupStatusLabel" runat="server" Text='<%# GetProgramMessage("GroupStatus") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlGroupStatus" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="200" Text='<%# Eval("GroupStatus") %>' />
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequiredGroupStatus" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlStatusLabel" runat="server" Text='<%# GetProgramMessage("Status") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlStatus" runat="server" MaxLength="5" SkinID="SkCtlTextboxLeft" Text='<%# Eval("Status") %>'/>
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequiredStatus" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("Comment") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine" Height="50px" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);" Width="250px" Text='<%# Eval("Comment") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveLabel" runat="server" Text='<%# GetProgramMessage("Active") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("Save") %>'></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlGroupStatusLabel" runat="server" Text='<%# GetProgramMessage("GroupStatus") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlGroupStatus" runat="server" MaxLength="15" SkinID="SkCtlTextboxLeft"/>
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequiredGroupStatus" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlStatusLabel" runat="server" Text='<%# GetProgramMessage("Status") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlStatus" runat="server" MaxLength="5" SkinID="SkCtlTextboxLeft"/>
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequiredStatus" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("Comment") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine" Height="50px" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveLabel" runat="server" Text='<%# GetProgramMessage("Active") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CommandName="Insert"
                                                        Text="Insert" ToolTip='<%# GetProgramMessage("Save") %>'></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CommandName="Cancel"
                                                        Text="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'></asp:ImageButton>
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
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Status.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlStatusModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlStatusFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlStatusFormPanelHeader" />
</asp:Content>