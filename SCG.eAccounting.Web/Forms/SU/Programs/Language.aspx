<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="Language.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Language" 
EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1"%>
<%@Register assembly="SS.Standard.UI" namespace="SS.Standard.UI" tagprefix="ss" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<script type="text/javascript" language="javascript">
		function pageLoad() 
		{
			var mpe = $find("mpe");
			mpe.add_shown(onShown);
			mpe.add_hidden(onHidden);
			var hdField = document.getElementById('<%= hdModalPopupState.ClientID %>');
			var shown = (hdField.value == "shown");
			if (shown)
			{
				mpe.show();
			}
		}

		function onShown() 
		{
		    var hdField = document.getElementById('<%= hdModalPopupState.ClientID %>');
			hdField.value = "shown";
		}

		function onHidden() 
		{
		    var hdField = document.getElementById('<%= hdModalPopupState.ClientID %>');
			hdField.value = "hidden";

//			var listLangObj = document.getElementById('<%= this.Master.FindControl("LanguageFlag1").FindControl("CtlEnglishFlagButton").ClientID %>');
//			listLangObj.outerHTML = listLangObj.outerHTML.replace('hidden', 'visible');

//			var listLangObjThai = document.getElementById('<%= this.Master.FindControl("LanguageFlag1").FindControl("CtlThaiFlagButton").ClientID %>');
//			listLangObjThai.outerHTML = listLangObjThai.outerHTML.replace('hidden', 'visible');

		}
	</script>
    <table style="font-family:Tahoma;font-size:10;color:Red"></table>
    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                    DynamicLayout="true" EnableViewState="False">
                    <ProgressTemplate>
                        <uc3:SCGLoading ID="SCGLoading1"  runat="server" />                   
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
                <ss:BaseGridView ID="ctlLanguageGrid" runat="server" AutoGenerateColumns="False" CssClass="table"
                        EnableInsert="False" ReadOnly="true" DataKeyNames="LanguageID" OnRequestData="RequestData"
                        InsertRowCount="1" AllowPaging="True" AllowSorting="true" SaveButtonID="" 
                        OnRowCommand="ctlLanguageGrid_RowCommand" OnDataBound="ctlLanguageGrid_DataBound"
                        onrequestcount="ctlLanguageGrid_RequestCount" Width="100%" SelectedRowStyle-BackColor="#6699FF">
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
                        <asp:TemplateField HeaderText="Language" SortExpression="LanguageName">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblLanguage" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Language Code" SortExpression="LanguageCode">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblLanguageCode" runat="server" Text='<%# Bind("LanguageCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image Path" SortExpression="ImagePath">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblImagePath" runat="server" Text='<%# Bind("ImagePath") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Comment">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblComment" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>                      
                        <asp:TemplateField HeaderText="Active" SortExpression="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                    CommandName="UserEdit" ToolTip='<%# GetProgramMessage("Edit")%>' />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
							<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
						</EmptyDataTemplate>
						<EmptyDataRowStyle Width="100%" HorizontalAlign="Center" />
                    </ss:BaseGridView>
                <div id="divButton" runat="server" align="left">
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

    <asp:Panel ID="ctlLanguageFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlLanguageFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
				<p>
					<asp:Label ID="lblCapture" Font-Names="Tahoma" Font-Size="Small" runat="server" Text="Manage User Data" Width="160px"></asp:Label>
				</p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelLanguageForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="UpdatePanelLanguageFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelLanguageForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />  
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlLanguageForm" runat="server" OnItemCommand="ctlLanguageForm_ItemCommand"
									DataKeyNames="LanguageID" oniteminserting="ctlLanguageForm_ItemInserting"   
									onitemupdating="ctlLanguageForm_ItemUpdating" OnDataBound="ctlLanguageForm_DataBound" 
                                    onmodechanging="ctlLanguageForm_ModeChanging" CssClass="table">
                                    <EditItemTemplate>
                                        <table>
                                        <tr>
                                            <td align="right"><%# GetProgramMessage("LanguageName")%> :</td>
                                            <td align="left">
                                                <asp:Label ID="ctlLanguage" runat="server" Text='<%# GetProgramMessage("LanguageName") %>'></asp:Label>
                                                <asp:Label ID="ctlTxtNameReq" runat="server" Text="*" style="color:Red;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"><%# GetProgramMessage("LanguageCode")%> :</td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlLanguageCode"   SkinID="SkCtlTextboxCenter" runat="server" MaxLength="50" Text='<%# Bind("LanguageCode") %>'/>
                                                <asp:Label ID="lblTxtCodeReq" style="color:Red;" Text="*" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <div runat="server" id="ctlImageArea">
                                        <tr>
		                                    <td align="right"><%# GetProgramMessage("Image")%> :</td>
		                                    <td align="left">
			                                    <asp:Image ID="ctlImage" runat="server" /><br />
		                                    </td>
	                                    </tr>
	                                    </div>
	                                    <tr>
		                                    <td align="right"><%# GetProgramMessage("NewImage")%> :</td>
		                                    <td align="left">
			                                    <asp:FileUpload ID="ctlImageFile" SkinID="SkCtlFileUpload" runat="server" />
		                                    </td>
	                                    </tr>
                                        <tr>
                                            <td align="right" valign="top"><%# GetProgramMessage("Comment")%> :</td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlComment" ctlTextbox="ctlTextboMultiLine" TextMode="MultiLine" MaxLength="500" runat="server" Text='<%# Bind("Comment") %>' SkinID="SkCtlTextboxMultiLine"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ctlLblActive" runat="server" Text='<%# GetProgramMessage("Active")%>'></asp:Label> :
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                            <br />
                                            <div style="vertical-align:middle">
                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                    ValidationGroup="ValidateFormView" CommandName="Update" ToolTip='<%# GetProgramMessage("Save") %>'></asp:ImageButton>
                                                <span class="spanSeparator">| </span>
                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                    CommandName="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'></asp:ImageButton>
                                            </div>
                                            </td>
                                        </tr>
                                    </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                        <tr>
                                            <td align="right"><%# GetProgramMessage("LanguageName")%> :</td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlLanguage" runat="server" SkinID="SkCtlTextboxLeft" MaxLength="200" Text='<%# Bind("LanguageName") %>'/>
                                                <asp:Label ID="ctlTxtNameReq" runat="server" Text="*" style="color:Red;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"><%# GetProgramMessage("LanguageCode")%> :</td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlLanguageCode" runat="server" MaxLength="50" Text='<%# Bind("LanguageCode") %>' SkinID="SkCtlTextboxCenter" />
                                                <asp:Label ID="lblTxtCodeReq" style="color:Red;" Text="*" runat="server"></asp:Label>
                                            </td>
                                        </tr>
	                                    <tr>
		                                    <td align="right"><%# GetProgramMessage("NewImage")%> :</td>
		                                    <td align="left">
			                                    <asp:FileUpload ID="ctlImageFile" SkinID="SkCtlFileUpload" runat="server" />
		                                    </td>
	                                    </tr>
                                        <tr>
                                            <td align="right" valign="top"><%# GetProgramMessage("Comment")%> :</td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlComment" MaxLength="500" runat="server" Text='<%# Bind("Comment") %>' SkinID="SkCtlTextboxMultiLine" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"><%# GetProgramMessage("Active")%> :</td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                            <br />
                                            <div style="vertical-align:middle">
                                                 <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                    ValidationGroup="ValidateFormView" CommandName="Insert" ToolTip='<%# GetProgramMessage("Save") %>'></asp:ImageButton>
                                                <span class="spanSeparator">| </span>                                                
                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                    CommandName="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'></asp:ImageButton>
                                            </div>
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
									<spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Language.Error" />
								</font>
							</td>
						</tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display:none" 
		meta:resourcekey="lnkDummyResource1" />
    <asp:HiddenField ID="hdModalPopupState" runat="server" />
    <ajaxToolkit:ModalPopupExtender 
		ID="ctlLanguageModalPopupExtender" runat="server" BehaviorID="mpe"
		TargetControlID="lnkDummy"
        PopupControlID="ctlLanguageFormPanel" 
        BackgroundCssClass="modalBackground" 
        CancelControlID="lnkDummy"
        DropShadow="true" 
        RepositionMode="None" 
        PopupDragHandleControlID="ctlLanguageFormPanelHeader" />
</asp:Content>
