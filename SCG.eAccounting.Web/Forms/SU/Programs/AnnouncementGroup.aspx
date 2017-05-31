<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
	CodeBehind="AnnouncementGroup.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.AnnouncementGroup"
	EnableTheming="true" StylesheetTheme="Default" %>
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
		}
	</script>
	<asp:UpdatePanel ID="ctlUpdPanelAnnGrp" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:UpdateProgress ID="ctlUpdProgressGrid" runat="server" AssociatedUpdatePanelID="ctlUpdPanelAnnGrp"
				DynamicLayout="true" EnableViewState="False">
				<ProgressTemplate>
					<uc3:SCGLoading ID="SCGLoading1" runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<ss:BaseGridView ID="ctlAnnouncementGroupGrid" runat="server" 
			AutoGenerateColumns="False"
				OnRequestData="RequestData1" ReadOnly="true" Width="100%"
				 EnableInsert="false" SelectedRowStyle-BackColor="#6699FF"
				InsertRowCount="1" DataKeyNames="AnnouncementGroupid" CssClass="table"
				AllowPaging="true" AllowSorting="true" 
				HeaderStyle-CssClass="GridHeader"
				ondatabound="ctlAnnouncementGroupGrid_DataBound" 
				onrowcommand="ctlAnnouncementGroupGrid_RowCommand" onrequestcount="ctlAnnouncementGroupGrid_RequestCount">
				<Columns>
					<asp:TemplateField HeaderText="Select">
						<HeaderTemplate>
							<asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Width="25px" />
						<HeaderStyle HorizontalAlign="Center" Width="25px" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Announcement Group Name" SortExpression="AnnouncementGroupName">
						<ItemTemplate>
							<asp:LinkButton ID="ctlAnnouncementGroupName" runat="server" Text='<%# Eval("AnnouncementGroupName") %>' CommandName="Select" />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="false" Width="25%" />
						<HeaderStyle HorizontalAlign="Center" Width="25%" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Display Order" SortExpression="DisplayOrder">
						<ItemTemplate>
							<asp:Label ID="ctlDisplayOrder" runat="server" Text='<%# Eval("DisplayOrder") %>' style="padding-right:5px;"></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="10%" />
						<HeaderStyle HorizontalAlign="Center" Width="10%" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Image" SortExpression="ag.ImagePath">
						<ItemTemplate>
							<asp:Label ID="ctlImagePath" runat="server" Text='<%# Eval("ImagePath") %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="false" Width="20%" />
						<HeaderStyle HorizontalAlign="Center" Width="20%" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description" SortExpression="ag.Comment">
						<ItemTemplate>
							<asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="true" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Active" SortExpression="ag.Active">
						<ItemTemplate>
							<asp:CheckBox ID="ctlActiveChkBox" runat="server" Checked='<%# Eval("Active") %>' Enabled="false"></asp:CheckBox>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="10%" />
						<HeaderStyle HorizontalAlign="Center" Width="10%" />
					</asp:TemplateField>
					<asp:TemplateField ShowHeader="False">
						<ItemTemplate>
							<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" 
								CommandName="EditAnnouncementGroup" ToolTip='<%# GetProgramMessage("EditAnnouncementGroup") %>' />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="False" Width="5%" />
						<HeaderStyle HorizontalAlign="Center" Width="5%" />
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
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
	<asp:UpdatePanel ID="ctlUpdPanelAnnGrpLang" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
		<uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
			<fieldset id="ctlFieldSetDetailGridView" runat="server" style="width:100%; text-align:center;">
				<legend id="ctlLegendDetailGridView" style="color:#4E9DDF;">
					<asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$"></asp:Label>
				</legend>
				<table border="0" cellpadding="0" cellspacing="0" width="98%">
					<tr>
						<td align="center">
							<ss:BaseGridView ID="ctlAnnouncementGroupLangGrid" runat="server" AutoGenerateColumns="false"
								CssClass="table" ReadOnly="true" DataKeyNames="LanguageId, AnnouncementGroupId" 
								Width="100%" onrowdatabound="ctlAnnouncementGroupLangGrid_RowDataBound" HorizontalAlign="Center">
								<Columns>
									<asp:TemplateField HeaderText="Language">
										<ItemTemplate><%# Eval("LanguageName")%></ItemTemplate>
										<ItemStyle HorizontalAlign="Center" Width="100px" />
										<HeaderStyle HorizontalAlign="Center" Width="100px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Announcement Group Name">
										<ItemTemplate>
											<asp:TextBox ID="ctlAnnouncementGroupName" runat="server" SkinID="SkCtlTextboxLeft" Text='<%# Eval("AnnouncementGroupName") %>' Width="95%" MaxLength="300" />
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Left" Width="31%" Wrap="true" />
										<HeaderStyle HorizontalAlign="Center" Width="31%" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Description">
										<ItemTemplate>
											<asp:TextBox ID="ctlComment" runat="server" SkinID="SkCtlTextboxLeft" Text='<%# Eval("Comment") %>' Width="95%" MaxLength="500" />
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Left" Wrap="true" />
										<HeaderStyle HorizontalAlign="Center" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Active">
										<ItemTemplate>
											<asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' />
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Center" Width="75px" />
										<HeaderStyle HorizontalAlign="Center" Width="75px" />
									</asp:TemplateField>
								</Columns>
							</ss:BaseGridView>
						</td>
					</tr>
					<tr>
						<td align="left">
							<div id="divDetailGridViewButton" runat="server">
								<table border="0">
									<tr>
										<td valign="middle">
											<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" />
										</td>
										<td valign="middle"> | </td>
										<td valign="middle">
											<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
				</table>
			</fieldset>
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<asp:Panel ID="ctlAnnGroupFormPanel" runat="server" CssClass="modalPopup" 
		Style="display: none" Width="600px">
		<asp:Panel ID="ctlAnnGroupFormPanelHeader" runat="server" 
			Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
			<div>
				<p>
					<asp:Label ID="ctlCapture" runat="server" Text="Manage Announcement Group Data"></asp:Label>
				</p>
			</div>
		</asp:Panel>
		<asp:UpdatePanel ID="ctlUpdPanelAnnGrpForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
			<ContentTemplate>
				<div align="center" style="display: block;">
					<asp:UpdateProgress ID="ctlUpdProgressForm" runat="server" 
						AssociatedUpdatePanelID="ctlUpdPanelAnnGrpForm" DynamicLayout="true" 
						EnableViewState="False">
						<ProgressTemplate>
							<uc3:SCGLoading ID="SCGLoading2" runat="server" />
						</ProgressTemplate>
					</asp:UpdateProgress>
					<table border="0" cellpadding="0" cellspacing="0" class="table">
						<tr>
							<td align="center">
								<asp:FormView ID="ctlAnnouncementGroupForm" runat="server" 
									DataKeyNames="AnnouncementGroupId" oniteminserting="ctlAnnouncementGroupForm_ItemInserting" 
									onitemupdating="ctlAnnouncementGroupForm_ItemUpdating" 
									onmodechanging="ctlAnnouncementGroupForm_ModeChanging" 
									onitemcommand="ctlAnnouncementGroupForm_ItemCommand" ondatabound="ctlAnnouncementGroupForm_DataBound">
									<EditItemTemplate>
										<table>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("AnnouncementGroupNameText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:Label ID="ctlTxtName" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("DisplayOrderText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="5" SkinID="SkCtlTextboxLeft" 
														onkeypress="return isKeyInt();" Text='<%# Eval("DisplayOrder") %>'></asp:TextBox>
													<asp:Label ID="lblTxtDisplayOrderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:200px;"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:TextBox ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>' Width="240px" Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
														TextMode="MultiLine" MaxLength="500" SkinID="SkCtlTextboxMultiLine" />
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("ImageText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:Image ID="ctlImage" runat="server" /><br />
													<%--<asp:Label ID="lblCtlImageFileReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
													<%--<asp:RegularExpressionValidator ID="revImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView"
														ValidationExpression="^.+\.((jpg)|(JPG)|(JPEG)|(jpeg)|(gif)|(GIF))$">*</asp:RegularExpressionValidator>--%>
													<%--<asp:RequiredFieldValidator ID="rfvImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>--%>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("NewImageText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:FileUpload ID="ctlImageFile" runat="server" Width="243px" SkinID="SkCtlFileUpload" />
													<%--<asp:Label ID="lblCtlImageFileReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
													<%--<asp:RegularExpressionValidator ID="revImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView"
														ValidationExpression="^.+\.((jpg)|(JPG)|(JPEG)|(jpeg)|(gif)|(GIF))$">*</asp:RegularExpressionValidator>--%>
													<%--<asp:RequiredFieldValidator ID="rfvImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>--%>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>' />
												</td>
											</tr>
											<tr>
												<td align="center" colspan="2">
													<asp:ImageButton ID="ctlUpdate" runat="server" CausesValidation="True" 
														CommandName="Update" SkinID="SkCtlFormSave" ToolTip='<%# GetProgramMessage("SaveToolTip") %>'
														ValidationGroup="ValidateFormView" />
													<asp:ImageButton ID="ctlCancel" runat="server" CausesValidation="False" 
														CommandName="Cancel" SkinID="SkCtlFormCancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>' />
												</td>
											</tr>
										</table>
									</EditItemTemplate>
									<InsertItemTemplate>
										<table>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("AnnouncementGroupNameText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:TextBox ID="ctlTxtName" runat="server" MaxLength="300" SkinID="SkCtlTextboxLeft"></asp:TextBox>
													<asp:Label ID="ctlTxtNameReq" runat="server" Text="*" style="color:Red;"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("DisplayOrderText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="5"  SkinID="SkCtlTextboxLeft"
														onkeypress="return isKeyInt();"></asp:TextBox>
													<asp:Label ID="lblTxtDisplayOrderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:200px;"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:TextBox ID="ctlComment" runat="server" Width="240px" Height="50px" MaxLength="500" TextMode="MultiLine"
													SkinID="SkCtlTextboxMultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" />
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("ImageText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:FileUpload ID="ctlImageFile" runat="server" Width="243px" SkinID="SkCtlFileUpload" />
													<%--<asp:Label ID="lblCtlImageFileReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
													<%--<asp:RegularExpressionValidator ID="revImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView"
														ValidationExpression="^.+\.((jpg)|(JPG)|(JPEG)|(jpeg)|(gif)|(GIF))$">*</asp:RegularExpressionValidator>--%>
													<%--ValidationExpression="^.+\.(([jJ][pP][eE]?[gG])|([gG][iI][fF]))$"--%>
													<%--<asp:RequiredFieldValidator ID="rfvImageFile" runat="server" Display="none"
														ControlToValidate="ctlImageFile" ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>--%>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left" style="width:400px;">
													<asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
												</td>
											</tr>
											<tr>
												<td align="center" colspan="2">
													<asp:ImageButton ID="ctlInsert" runat="server" CommandName="Insert" 
														SkinID="SkCtlFormSave" ToolTip='<%# GetProgramMessage("SaveToolTip") %>' />
													<asp:ImageButton ID="ctlCancel" runat="server" CommandName="Cancel" 
														SkinID="SkCtlFormCancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>' />
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
									<%--<asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="ValidateFormView" />--%>
									<spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="AnnouncementGroup.Error" />
								</font>
							</td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</asp:Panel>
	<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
	<asp:HiddenField ID="hdModalPopupState" runat="server" Value="" />
	<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender" runat="server" BehaviorID="mpe"
		TargetControlID="lnkDummy" PopupControlID="ctlAnnGroupFormPanel" BackgroundCssClass="modalBackground"
		CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlAnnGroupFormPanelHeader" />
</asp:Content>