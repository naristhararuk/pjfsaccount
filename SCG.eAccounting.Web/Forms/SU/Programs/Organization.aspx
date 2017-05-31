<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="Organization.aspx.cs" 
	Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Organization" EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
	<asp:UpdatePanel ID="ctlUpdatePanelOrganizationGrid" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:UpdateProgress ID="ctlUpdateProgressOrganizationGrid" runat="server" 
				AssociatedUpdatePanelID="ctlUpdatePanelOrganizationGrid"
				DynamicLayout="true" EnableViewState="False">
				<ProgressTemplate>
					<uc3:SCGLoading ID="SCGLoading1"  runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<ss:BaseGridView ID="ctlOrganizationGrid" runat="server" AutoGenerateColumns="False"
				ReadOnly="true" EnableInsert="false" SelectedRowStyle-BackColor="#6699FF"
				InsertRowCount="1" DataKeyNames="OrganizationId" CssClass="table" 
				AllowPaging="true" AllowSorting="true" ondatabound="ctlOrganizationGrid_DataBound" 
				onrowcommand="ctlOrganizationGrid_RowCommand" Width="100%"
				onrequestcount="ctlOrganizationGrid_RequestCount" 
				onrequestdata="ctlOrganizationGrid_RequestData">
				<Columns>
					<asp:TemplateField HeaderText="Select">
						<HeaderTemplate>
							<asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" style="text-align:center;" />
						</HeaderTemplate>
						<ItemTemplate>
							<asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" style="text-align:center;" />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="center" Width="25px" />
						<HeaderStyle HorizontalAlign="center" Width="25px" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Organization Name" SortExpression="OrganizationName">
						<ItemTemplate>
							<asp:LinkButton ID="ctlOrganizationName" runat="server" Text='<%# Eval("OrganizationName") %>' CommandName="Select" />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="false" Width="40%" />
						<HeaderStyle HorizontalAlign="Center" Width="40%" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description" SortExpression="Comment">
						<ItemTemplate>
							<asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="false"/>
						<HeaderStyle HorizontalAlign="Center"/>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Active" SortExpression="org.Active">
						<ItemTemplate>
							<asp:CheckBox ID="ctlActiveChkBox" runat="server" Checked='<%# Eval("Active") %>' Enabled="false"></asp:CheckBox>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="10%" />
						<HeaderStyle HorizontalAlign="Center" Width="10%" />
					</asp:TemplateField>
					<asp:TemplateField ShowHeader="False">
						<ItemTemplate>
							<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" 
								CommandName="EditOrganization" ToolTip='<%# GetProgramMessage("EditOrganization") %>' />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="False" Width="10%" />
						<HeaderStyle HorizontalAlign="Center" Width="10%" />
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
			</ss:BaseGridView>
			<div id="divButton" runat="server" align="left" style="vertical-align:middle;" visible="false">
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
			<font color="red" style="text-align:left;">
				<spring:ValidationSummary ID="deleteValidationSummary" runat="server" Provider="DeleteOrganization.Error"></spring:ValidationSummary>
			</font>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:UpdatePanel ID="ctlUpddatePanelOrganizationLangGrid" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
		<uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
			<asp:UpdateProgress ID="ctlUpdateProgressOrganizationLangGrid" runat="server" 
				AssociatedUpdatePanelID="ctlUpddatePanelOrganizationLangGrid"
				DynamicLayout="true" EnableViewState="False">
				<ProgressTemplate>
					<uc3:SCGLoading ID="SCGLoading2"  runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<fieldset id="fdsDetailGridView" runat="server" style="width:100%; text-align:center;">
			<legend id="lgDetailHeader" style="color:#4E9DDF;">
				<asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$"></asp:Label>
			</legend>
			<table border="0" cellpadding="0" cellspacing="0" width="98%">
				<tr>
					<td align="center">
						<ss:BaseGridView ID="ctlOrganizationLangGrid" runat="server" AutoGenerateColumns="false"
							CssClass="table" ReadOnly="true" DataKeyNames="LanguageId, OrganizationId"
							onrowcommand="ctlOrganizationLangGrid_RowCommand" Width="100%" HorizontalAlign="Center">
							<Columns>
								<asp:TemplateField HeaderText="Language">
									<ItemTemplate><%# Eval("LanguageName")%></ItemTemplate>
									<ItemStyle HorizontalAlign="Center" Width="100px" />
									<HeaderStyle HorizontalAlign="Center" Width="100px" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Organization Name">
									<ItemTemplate>
										<asp:Label ID="ctlLblOrganizationName" runat="server" Text='<%# Eval("OrganizationName") %>'></asp:Label>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Left" Wrap="true" />
									<HeaderStyle HorizontalAlign="Center" />
								</asp:TemplateField>
								<%--<asp:TemplateField HeaderText="Description">
									<ItemTemplate>
										<asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>--%>
								<asp:TemplateField HeaderText="Active">
									<ItemTemplate>
										<asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled="false" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" Width="75px" />
									<HeaderStyle HorizontalAlign="Center" Width="75px" />
								</asp:TemplateField>
								<asp:TemplateField ShowHeader="False">
									<ItemTemplate>
										<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" 
											CausesValidation="False" CommandName="Select" ToolTip='<%# GetProgramMessage("EditOrganizationLang") %>' />
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
						<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" ToolTip="Close" onclick="ctlCancel_Click" />
					</td>
				</tr>
			</table>
			</fieldset>
			<%--<asp:Button ID="ctlSubmit" runat="server" Text="Submit" OnClick="ctlSubmit_Click"/>--%>
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<asp:Panel ID="ctlOrganizationFormPanel" runat="server" CssClass="modalPopup" 
		Style="display: none" Width="500px">
		<asp:Panel ID="ctlOrganizationFormPanelHeader" runat="server" 
			Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
			<div>
				<p>
					<asp:Label ID="ctlCapture" runat="server" Text="Manage Organization Data"></asp:Label>
				</p>
			</div>
		</asp:Panel>
		<asp:UpdatePanel ID="ctlUpdatePanelOrganizationForm" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<div align="center" style="display: block;">
					<asp:UpdateProgress ID="ctlUpdateProgressForm" runat="server" 
						AssociatedUpdatePanelID="ctlUpdatePanelOrganizationForm" DynamicLayout="true" 
						EnableViewState="False">
						<ProgressTemplate>
							<uc3:SCGLoading ID="SCGLoading3"  runat="server" />
						</ProgressTemplate>
					</asp:UpdateProgress>
					<table border="0" cellpadding="0" cellspacing="0" class="table">
						<tr>
							<td align="center">
								<asp:FormView ID="ctlOrganizationForm" runat="server" 
									DataKeyNames="OrganizationId" oniteminserting="ctlOrganizationForm_ItemInserting" 
									onitemupdating="ctlOrganizationForm_ItemUpdating" 
									onmodechanging="ctlOrganizationForm_ModeChanging" 
									onitemcommand="ctlOrganizationForm_ItemCommand" ondatabound="ctlOrganizationForm_DataBound">
									<EditItemTemplate>
										<table>
											<tr>
												<td align="right"><%# GetProgramMessage("OrganizationNameText") %> :</td>
												<td align="left">
													<asp:Label ID="ctlTxtOrgName" runat="server" />
													<%--<asp:Label ID="ctlLblTxtOrgNameReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" SkinID="SkCtlTextboxMultiLine" runat="server" Text='<%# Eval("Comment") %>' Height="50px" 
														Width="240px" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" />
													<%--<asp:Label ID="lblTxtCommentReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
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
												<td align="right"><%# GetProgramMessage("OrganizationNameText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtOrgName" SkinID="SkCtlTextboxLeft" runat="server" Width="132px" MaxLength="200" />
													<asp:Label ID="ctlTxtOrgNameReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" SkinID="SkCtlTextboxMultiLine" runat="server" Width="240px" MaxLength="500" 
														TextMode="MultiLine" Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" />
													<%--<asp:Label ID="lblTxtCommentReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
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
									<spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Organization.Error" />
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
	<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender1" runat="server"
		TargetControlID="lnkDummy" PopupControlID="ctlOrganizationFormPanel" BackgroundCssClass="modalBackground"
		CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlOrganizationFormPanelHeader" />




	<asp:Panel ID="ctlOrganizationLangFormPanel" runat="server" CssClass="modalPopup" 
		Style="display: none" Width="600px">
		<asp:Panel ID="ctlOrganizationLangFormPanelHeader" runat="server" 
			Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
			<div>
				<p>
					<asp:Label ID="lblOrganizationLangHeader" runat="server" Text="Manage Organization Language Data"></asp:Label>
				</p>
			</div>
		</asp:Panel>
		<asp:UpdatePanel ID="ctlUpddatePanelOrganizationLangForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
			<ContentTemplate>
				<div align="center" style="display: block;">
					<asp:UpdateProgress ID="ctlUpdateProgressLangForm" runat="server" 
						AssociatedUpdatePanelID="ctlUpddatePanelOrganizationLangForm" DynamicLayout="true" 
						EnableViewState="False">
						<ProgressTemplate>
							<uc3:SCGLoading ID="SCGLoading4"  runat="server" />
						</ProgressTemplate>
					</asp:UpdateProgress>
					<table border="0" cellpadding="0" cellspacing="0" class="table">
						<tr>
							<td align="center">
								<asp:FormView ID="ctlOrganizationLangForm" runat="server" 
									DataKeyNames="Id" 
									oniteminserting="ctlOrganizationLangForm_ItemInserting" 
									onitemupdating="ctlOrganizationLangForm_ItemUpdating" 
									onmodechanging="ctlOrganizationLangForm_ModeChanging" 
									onitemcommand="ctlOrganizationLangForm_ItemCommand" 
									ondatabound="ctlOrganizationLangForm_DataBound">
									<EditItemTemplate>
										<table>
											<tr>
												<td align="right"><%# GetProgramMessage("OrganizationNameText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtOrgName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="200" Text='<%# Eval("OrganizationName") %>' Width="300px"></asp:TextBox>
													<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("AddressText") %> :</td>
												<td align="left" valign="top">
													<asp:TextBox ID="ctlTxtAddress" runat="server" Text='<%# Eval("Address") %>' Width="300px" SkinID="SkCtlTextboxMultiLine"
														TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Height="100px" />
													<asp:Label ID="lblTxtAddress" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ProvinceText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtProvince" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Eval("Province") %>' Width="180px" MaxLength="100"></asp:TextBox>
													<asp:Label ID="lblTxtProvince" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("CountryText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtCountry" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Eval("Country") %>' Width="180px" MaxLength="100"></asp:TextBox>
													<asp:Label ID="lblTxtCountry" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("PostalNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtPostal" SkinID="SkCtlTextboxLeft" runat="server" onkeypress="return isKeyInt();" Text='<%# Eval("Postal") %>' Width="180px" MaxLength="10"></asp:TextBox>
													<asp:Label ID="lblTxtPostal" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("TelephoneNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtTelephone" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Text='<%# Eval("Organization.Telephone") %>' Width="120px" MaxLength="20"></asp:TextBox>
													<%# GetProgramMessage("ExtensionText") %>
													<asp:TextBox ID="ctlTxtTelephoneExt" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Text='<%# Eval("Organization.TelephoneExt") %>' Width="50px" MaxLength="10"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("FaxNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtFax" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Text='<%# Eval("Organization.Fax") %>' Width="120px" MaxLength="20"></asp:TextBox>
													<%# GetProgramMessage("ExtensionText") %>
													<asp:TextBox ID="ctlTxtFaxExt" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Text='<%# Eval("Organization.FaxExt") %>' Width="50px" MaxLength="10"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("EmailText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtEmail" runat="server" SkinID="SkCtlTextboxLeft" Text='<%# Eval("Organization.Email") %>' Width="180px" MaxLength="100"></asp:TextBox>
													<asp:RegularExpressionValidator ID="revCtlTxtEmail" runat="server" ControlToValidate="ctlTxtEmail" Display="None" Text="*"
														ValidationExpression="(([;]\s\w)*(([a-zA-Z0-9]+([-_.a-zA-Z0-9]+)*[-_a-zA-Z0-9]+)|[a-zA-Z0-9])@[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*\.[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*)*"
														ValidationGroup="ValidateFormView">
													</asp:RegularExpressionValidator>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" runat="server" SkinID="SkCtlTextboxMultiLine" Text='<%# Eval("Comment") %>' 
														MaxLength="500" TextMode="MultiLine" Width="300px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
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
												<td align="right"><%# GetProgramMessage("OrganizationNameText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtOrgName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
													<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("AddressText") %> :</td>
												<td align="left" valign="top">
													<asp:TextBox ID="ctlTxtAddress" runat="server" Width="300px" SkinID="SkCtlTextboxMultiLine"
														TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Height="100px" />
													<asp:Label ID="lblTxtAddress" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ProvinceText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtProvince" SkinID="SkCtlTextboxLeft" runat="server" Width="180px" MaxLength="100"></asp:TextBox>
													<asp:Label ID="lblTxtProvince" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("CountryText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtCountry" SkinID="SkCtlTextboxLeft" runat="server" Width="180px" MaxLength="100"></asp:TextBox>
													<asp:Label ID="lblTxtCountry" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("PostalNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtPostal" SkinID="SkCtlTextboxLeft" runat="server" onkeypress="return isKeyInt();" Width="180px" MaxLength="10"></asp:TextBox>
													<asp:Label ID="lblTxtPostal" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("TelephoneNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtTelephone" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Width="120px" MaxLength="20"></asp:TextBox>
													<%# GetProgramMessage("ExtensionText") %>
													<asp:TextBox ID="ctlTxtTelephoneExt" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Width="50px" MaxLength="10"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("FaxNoText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtFax" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Width="120px" MaxLength="20"></asp:TextBox>
													<%# GetProgramMessage("ExtensionText") %>
													<asp:TextBox ID="ctlTxtFaxExt" runat="server" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();" Width="50px" MaxLength="10"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("EmailText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtEmail" runat="server" SkinID="SkCtlTextboxLeft" Width="180px" MaxLength="100"></asp:TextBox>
													<asp:RegularExpressionValidator ID="revCtlTxtEmail" runat="server" ControlToValidate="ctlTxtEmail" Display="None" Text="*"
														ValidationExpression="(([;]\s\w)*(([a-zA-Z0-9]+([-_.a-zA-Z0-9]+)*[-_a-zA-Z0-9]+)|[a-zA-Z0-9])@[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*\.[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*)*"
														ValidationGroup="ValidateFormView">
													</asp:RegularExpressionValidator>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" SkinID="SkCtlTextboxMultiLine" runat="server" MaxLength="500" Width="300px" TextMode="MultiLine"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
													<asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
												</td>
											</tr>
											<tr>
												<td align="center" colspan="2">
													<asp:ImageButton ID="ctlInsert" runat="server" CommandName="Insert" 
														SkinID="SkCtlFormSave" ToolTip='<%# GetProgramMessage("SaveToolTip") %>' ValidationGroup="ValidateFormView" />
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
									<asp:ValidationSummary ID="ctlValidateFormViewVS" runat="server" ValidationGroup="ValidateFormView" />
									<spring:ValidationSummary ID="ctlValidationSummaryLang" runat="server" Provider="OrganizationLang.Error" />
								</font>
							</td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</asp:Panel>
	<asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" 
		meta:resourcekey="lnkDummy2Resource1" />
	<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender2" runat="server"
		TargetControlID="lnkDummy2" PopupControlID="ctlOrganizationLangFormPanel" BackgroundCssClass="modalBackground"
		CancelControlID="lnkDummy2" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlOrganizationLangFormPanelHeader" />
</asp:Content>
