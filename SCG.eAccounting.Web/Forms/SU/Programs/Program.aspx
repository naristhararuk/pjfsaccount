<%@ Page Title="" StylesheetTheme="Default" Language="C#" MasterPageFile="~/ProgramsPages.Master"
    AutoEventWireup="true" CodeBehind="Program.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Program" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <ss:BaseGridView ID="ctlProgramGrid" runat="server"  AutoGenerateColumns="false" CssClass="Grid" AllowSorting="true"
                AllowPaging="true" DataKeyNames="ProgramID" EnableInsert="False" ReadOnly="true" OnRowCommand="ctlProgramGrid_RowCommand"
                OnRowDataBound="ctlProgramGrid_RowDataBound" OnRequestCount="RequestCount" OnRequestData="RequestData"
                SelectedRowStyle-BackColor="#6699FF" 
                ondatabound="ctlProgramGrid_DataBound" 
                onpageindexchanged="ctlProgramGrid_PageIndexChanged" >
                <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
                <Columns>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlSelectAllChk" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelectChk" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Program Code" HeaderStyle-HorizontalAlign="Center" SortExpression="ProgramCode">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlProgramCodeLabel" runat="server" Text='<%# Bind("ProgramCode") %>'
                                CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="42%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# Bind("Comment") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="50%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                        <ItemTemplate>
                           <asp:CheckBox ID="ctlProgramActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false"/>
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" ToolTip='<%# GetProgramMessage("Edit") %>'
                                CommandName="UserEdit" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>' runat="server"></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="vertical-align:middle;">
            <table style="text-align:center;"><tr><td><asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" /></td>
            <td><span class="spanSeparator" style="vertical-align:top;">| </span></td>
            <td><asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" /></td></tr></table>
                
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlProgramFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlProgramFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text="Manage Program Data"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelProgramForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="UpdatePanelProgramFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelProgramForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlProgramForm" runat="server" DataKeyNames="ProgramID" OnItemCommand="ctlProgramForm_ItemCommand"
                                    OnItemInserting="ctlProgramForm_ItemInserting" OnItemUpdating="ctlProgramForm_ItemUpdating"
                                    OnModeChanging="ctlProgramForm_ModeChanging" 
                                    ondatabound="ctlProgramForm_DataBound">
                                    <EditItemTemplate>
                                        <table class="table">
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("ProgramCode") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlProgramCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="50" Text='<%# Bind("ProgramCode") %>'
                                                        Width="250px" />
                                                       <font color="red"><asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <%# GetProgramMessage("Comment") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine" SkinID="SkCtlTextboxMultiLine"  Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Text='<%# Bind("Comment") %>' Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" ToolTip='<%# GetProgramMessage("Update") %>'
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update"></asp:ImageButton>
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
                                                    <%# GetProgramMessage("ProgramCode") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlProgramCode" SkinID="SkCtlTextboxCenter" MaxLength="50" runat="server" Width="250px" />
                                                    <font color="red"><asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <%# GetProgramMessage("Comment") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine"  Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %> :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" ToolTip='<%# GetProgramMessage("Insert") %>'
                                                        ValidationGroup="ValidateFormView" CommandName="Insert" Text="Insert"></asp:ImageButton>
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
                                <%--<asp:ValidationSummary ID="vsSummary" runat="server" Style="text-align: left" Width="250px"
                                    ValidationGroup="ValidateFormView" />--%>
                                <font color="red">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Program.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlProgramLanguageUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
            <asp:UpdateProgress ID="UpdatePanelProgramLangFormProgress" runat="server" AssociatedUpdatePanelID="ctlProgramLanguageUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <br />
            <fieldset style="width:100%;text-align:Center" id="ctlProgramLangFds" runat="server" visible="false">
			<legend id="ctlLegendDetailGridView" style="color:#4E9DDF"><asp:Label ID="ctlLegendDetailGridViewLabel" runat="server" Text="$DisPlay Setting$"></asp:Label></legend> 
            <ss:BaseGridView ID="ctlProgramLanguageGrid" runat="server" AutoGenerateColumns="false" Width="98%" 
                CssClass="table" DataKeyNames="LanguageId" OnDataBound="ctlProgramLangGrid_DataBound" ReadOnly="true" >
                <Columns>
                    <asp:TemplateField HeaderText="Language Name" HeaderStyle-HorizontalAlign="Center" SortExpression="LanguageName">
                        <ItemTemplate>
                            <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Program Name" HeaderStyle-HorizontalAlign="Center" SortExpression="ProgramName">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlProgramName" SkinID="SkCtlTextboxLeft" runat="server" Width="95%" MaxLength="200" Text='<%# Bind("ProgramName") %>' />
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
            <div id="ctlDetailButton" runat="server" style="text-align:left;">
            <table style="text-align:center;">
            <tr>
            <td>
            <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" Text="Submit" Visible="false" />
            </td>
            <td>
            <span class="spanSeparator" style="vertical-align:top;">| </span>
            </td>
            <td>
            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" Text="Cancel" Visible="false" />
            </td>
            </tr>
            </table>
            </div>
            <br />
            </fieldset>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" 
		meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlProgramModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlProgramFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlProgramFormPanelHeader" />
</asp:Content>
