<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="Division.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Division" EnableTheming="true"
    StylesheetTheme="Default" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 500px" id="fdsSearch">
                <legend id="legSearch" style="color: #4E9DDF"><asp:Label ID="ctlSearchBox" runat="server" Text="$SearchBox$" /></legend>
                <table border="0">
                    <tr>
                        <td align="right" style="width: 250px">
                            <asp:Label ID="ctlOrganizationSearch" runat="server" Text="$Organization$" /> :
                        </td>
                        <td align="left" style="width: 250px">
                            <asp:DropDownList ID="ctlOrganizationList" SkinID="SkCtlDropDownList" runat="server">
                            </asp:DropDownList>
                            <asp:Label ID="lblAnnouncementGroupReq" Style="color: Red;" Text="*" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:LinkButton ID="ctlSearch" runat="server" OnClick="ctlSearchButton_Click" Text="$Search$"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <ss:BaseGridView ID="ctlDivisionGrid" runat="server" AutoGenerateColumns="False"
                Width="100%" OnRequestData="RequestData" OnRequestCount="RequestCount" ReadOnly="true"
                EnableInsert="false" InsertRowCount="1" DataKeyNames="DivisionId" CssClass="table"
                AllowPaging="true" AllowSorting="true" OnRowCommand="ctlDivisionGrid_RowCommand"
                OnDataBound="ctlDivisionGrid_DataBound" SelectedRowStyle-BackColor="#6699FF"
                OnPageIndexChanged="ctlDivisionGrid_PageIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                        <HeaderStyle Width="25px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DivisionName" SortExpression="DivisionName">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'
                                CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Comment" HeaderText="Comment" />--%>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="DivisionEdit" ToolTip='<%# GetProgramMessage("EditDivision") %>'/>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
            <div id="divButton" runat="server">
				<table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" />		
						</td>
					</tr>
				</table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="ctlUpdatePanelDivisionLangGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelDivisionLangGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 100%" id="ctlFieldSetDetailGridView" runat="server" 
                visible="False" enableviewstate="True">
                <legend id="ctlLegendDetailGridView" runat="server" style="color: #4E9DDF" 
                    enableviewstate="True"><asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$" /></legend>
                <center>
                    <ss:BaseGridView ID="ctlDivisionLangGrid" runat="server" AutoGenerateColumns="false"
                        CssClass="table" ReadOnly="true" Width="98%" DataKeyNames="LanguageId,DivisionId"
                        OnDataBound="ctlDivisionLangGrid_DataBound" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Language">
                                <ItemTemplate>
                                    <%# Eval("LanguageName")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DivisionName">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlDivisionName" SkinID="SkCtlTextBoxLeft" runat="server" MaxLength="200" Text='<%# Eval("DivisionName") %>'
                                        Width="98%" />
                                </ItemTemplate>
                                <ItemStyle Width="31%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment">
                                <ItemTemplate>
                                    <asp:TextBox ID="ctlCommentDivisionLang" runat="server"  MaxLength="500" Text='<%# Eval("Comment") %>'
                                        Width="98%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlActiveDivisionLang" runat="server" Checked='<%# Eval("Active") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="75px" />
                                <ItemStyle HorizontalAlign="Center" Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </ss:BaseGridView>
                </center>
                <table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" Visible="false" />
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlDetailCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlDetailCancel_Click" Visible="false" />
						</td>
					</tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlDivisionFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlDivisionFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlCapture" runat="server" Text="Manage Division Data" Width="200px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpdatePanelDivisionForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ctlUpdatePanelDivisionFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelDivisionForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table class="table" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlDivisionForm" runat="server" DataKeyNames="DivisionId" OnDataBound="ctlDivisionForm_DataBound"
                                    OnItemCommand="ctlDivisionForm_ItemCommand" OnItemInserting="ctlDivisionForm_Inserting"
                                    OnItemUpdating="ctlDivisionForm_Updating" OnModeChanging="ctlDivisionForm_ModeChanging">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlDivisionNameLabel" runat="server" Text='<%# GetProgramMessage("$DivisionName$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="ctlDivisionName" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlOrganization" runat="server" Text='<%# GetProgramMessage("$Organization$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlOrganizeList" runat="server" SkinID="SkCtlDropDownList">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblOrganizationReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("$Comment$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" SkinID="SkCtlTextboxMultiLine" TextMode="MultiLine" Height="50px" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);" Width="250px" Text='<%# Eval("Comment")%>' />
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
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("UpdateToolTip") %>'></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>'></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlDivisionNameLabel" runat="server" Text='<%# GetProgramMessage("$DivisionName$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlDivisionName" runat="server" MaxLength="200" SkinID="SkCtlTextboxLeft"/>
                                                    <asp:Label ID="lblDivisionNameReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlOrganization" runat="server" Text='<%# GetProgramMessage("$Organization$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlOrganizeList" runat="server" SkinID="SkCtlDropDownList">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="ctlOrganizerLabel" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("$Comment$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" SkinID="SkCtlTextboxMultiLine" TextMode="MultiLine" Height="50px" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlActiveLabel" runat="server" Text='<%# GetProgramMessage("$Active$") %>' />
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActive" Checked="true" runat="server" />
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
                                <font color="red" style="text-align: left">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Division.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlDivisionModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlDivisionFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlDivisionFormPanelHeader" />
</asp:Content>
