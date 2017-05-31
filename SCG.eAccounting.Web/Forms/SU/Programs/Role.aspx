<%@ Page Title="Role Program" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master"
    CodeBehind="Role.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Role" EnableTheming="true"
    StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="calendar" TagPrefix="uc1" %>
<%@ Register Assembly="SS.Standard.UI" Namespace="SS.Standard.UI" TagPrefix="ss" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="A">
    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlRoleGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                DataKeyNames="RoleId" EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="True" OnRequestCount="RequestCount" OnRequestData="RequestData"
                OnRowCommand="ctlRoleGrid_RowCommand" OnDataBound="ctlRoleGrid_DataBound" CssClass="table"
                SelectedRowStyle-BackColor="#6699FF" 
                onpageindexchanged="ctlRoleGrid_PageIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlRoleName" CommandName="Select" runat="server" Text='<%# Bind("RoleName")%>'></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="25%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="ctlComment" runat="server" Text='<%# Bind("Comment")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />--%>
                    <asp:TemplateField HeaderText="Update Date" SortExpression="UpdDate" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("UpdDate").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="13%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="r.Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false">
                            </asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="center" />
                    </asp:TemplateField>
                    <%--<asp:CheckBoxField DataField="Active" HeaderText="Active" SortExpression="Active" />--%>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" ToolTip='<%# GetProgramMessage("Edit") %>'
                                CommandName="RoleEdit" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="35px" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>' runat="server"></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left">
            <table style="text-align:center;">
            <tr>
            <td>
                <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" /></td>
                <td><span class="spanSeparator">| </span></td>
                <td><asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" /></td>
                </tr>
                </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="ctlRoleFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlRoleFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text="Manage Role Data" Width="160px"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelRoleForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div align="center" style="display: block;">
                    <asp:UpdateProgress ID="UpdatePanelRoleFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelRoleForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table border="0" cellpadding="0" cellspacing="0" class="TableInFormView">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlRoleForm" runat="server" DataKeyNames="Roleid" OnDataBound="ctlRoleForm_DataBound"
                                    OnItemCommand="ctlRoleForm_ItemCommand" OnItemInserting="ctlRoleForm_ItemInserting"
                                    OnItemUpdating="ctlRoleForm_ItemUpdating" OnModeChanging="ctlRoleForm_ModeChanging">
                                    <EditItemTemplate>
                                        <table class="table">
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("RoleName") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="ctlRoleName" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <%# GetProgramMessage("Comment") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" Text='<%# Bind("Comment") %>' TextMode="MultiLine"  Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" ToolTip='<%# GetProgramMessage("Update") %>'
                                                        CommandName="Update" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False" ToolTip='<%# GetProgramMessage("Cancel") %>'
                                                        CommandName="Cancel" Text="Cancel"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table class="table">
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("RoleName") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlRoleName" SkinID="SkCtlTextboxLeft" MaxLength="350" runat="server" Width="250px"></asp:TextBox>
                                                    <asp:Label ID="lblOrganizationReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <%# GetProgramMessage("Comment") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine"  Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" ToolTip='<%# GetProgramMessage("Insert") %>'
                                                        CommandName="Insert" Text="Insert"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False" ToolTip='<%# GetProgramMessage("Cancel") %>'
                                                        CommandName="Cancel" Text="Cancel"></asp:ImageButton>
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
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Role.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlRoleLangUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
            <asp:UpdateProgress ID="UpdatePanelProgramLangFormProgress" runat="server" AssociatedUpdatePanelID="ctlRoleLangUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width:100%;text-align:Center" id="ctlRoleLangLangFds" runat="server" visible="false">
			<legend id="ctlLegendDetailGridView" style="color:#4E9DDF"><asp:Label ID="ctlLegendDetailGridviewLabel" runat="server" Text="$DisPlay Setting$"></asp:Label></legend> 
            <ss:BaseGridView ID="ctlRoleLangGrid" runat="server"  AutoGenerateColumns="false" Width="98%"
                CssClass="table" DataKeyNames="LanguageId" ReadOnly="true" 
                ondatabound="ctlRoleLangGrid_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Language Name" HeaderStyle-HorizontalAlign="Center" SortExpression="LanguageName">
                        <ItemTemplate>
                            <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role Name" HeaderStyle-HorizontalAlign="Center" SortExpression="RoleName">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlRoleName" SkinID="SkCtlTextboxLeft" runat="server" Width="95%" MaxLength="350" Text='<%# Bind("RoleName") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="31%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxLeft" runat="server" Width="95%" MaxLength="500" Text='<%# Bind("Comment") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            <div id="ctlButtonGridDetail" runat="server" style="text-align:left;">
            <table style="text-align:center;">
            <tr>
            <td>
            <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" Text="Submit" Visible="false" /></td>
            <td><span class="spanSeparator">| </span></td>
            <td><asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" Text="Cancel" Visible="false" /></td>
            </tr>
            </table>
            </div>
            </fieldset>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" 
		meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlRoleModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlRoleFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlRoleFormPanelHeader" />
</asp:Content>
