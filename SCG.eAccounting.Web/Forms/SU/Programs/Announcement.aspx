<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
	CodeBehind="Announcement.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Announcement"
	EnableTheming="true" StylesheetTheme="Default" ValidateRequest="false" %>
	
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
	<asp:UpdatePanel ID="ctlUpdatePanelAnnnouncementGrid" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:UpdateProgress ID="ctlUpdProgressGrid" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAnnnouncementGrid"
				DynamicLayout="true" EnableViewState="False">
				<ProgressTemplate>
					<uc3:SCGLoading ID="SCGLoading1" runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<fieldset style="width:500px" id="fdsSearch">
				<legend id="legSearch" style="color:#4E9DDF">
					<asp:Label ID="ctlSearchBox" runat="server" SkinID="SkCtlQuery" ></asp:Label>
				</legend>      
				<table border="0">
					<tr>
						<td align="right" style="width:250px">
							<asp:Label ID="ctlAnnouncementGroupText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Announcement Group :$"></asp:Label>
						</td>
						<td align="left" style="width:250px">
							 <asp:DropDownList ID="ctlDdlAnnouncementGroup" runat="server" 
								 DataTextField="Text" DataValueField="Id" AppendDataBoundItems="true" 
								 ondatabound="ctlDdlAnnouncementGroup_DataBound">
							 </asp:DropDownList>
							 <asp:Label ID="lblAnnouncementGroupReq" style="color:Red;" Text="*" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td colspan="2" align="center">
							<asp:LinkButton ID="lnkSearch" runat="server" onclick="lnkSearch_Click" Text="$Search$"></asp:LinkButton>
						</td>
					</tr>
				</table>
			</fieldset>
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td align="left">
						<asp:Label ID="ctlLblAnnouncementGroup" runat="server"></asp:Label>
						<asp:HiddenField ID="hdCtlLblAnnouncementGroupId" runat="server" />
					</td>
				</tr>
			</table>
			<ss:BaseGridView ID="ctlAnnouncementGrid" runat="server" AutoGenerateColumns="False"
				OnRequestData="RequestData1" ReadOnly="true" EnableInsert="false" SelectedRowStyle-BackColor="#6699FF"
				InsertRowCount="1" DataKeyNames="Announcementid" CssClass="table" 
				AllowPaging="true" AllowSorting="true" ondatabound="ctlAnnouncementGrid_DataBound" 
				HeaderStyle-CssClass="GridHeader"
				onrowcommand="ctlAnnouncementGrid_RowCommand" Width="100%"
				onrequestcount="ctlAnnouncementGrid_RequestCount">
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
					<asp:TemplateField HeaderText="Announcement Header" SortExpression="AnnouncementHeader">
						<ItemTemplate>
							<asp:LinkButton ID="ctlAnnouncementHeader" runat="server" Text='<%# Eval("AnnouncementHeader") %>' CommandName="Select" />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="true" Width="30%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Effective Date" SortExpression="EffectiveDate">
						<ItemTemplate>
							<asp:Label ID="ctlEffDate" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveDate").ToString()) %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="15%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Last Display Date" SortExpression="LastDisplayDate">
						<ItemTemplate>
							<asp:Label ID="ctlEndDate" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("LastDisplayDate").ToString()) %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="15%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description" SortExpression="Comment">
						<ItemTemplate>
							<asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Left" Wrap="true" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Active" SortExpression="Active">
						<ItemTemplate>
							<asp:CheckBox ID="ctlActiveChkBox" runat="server" Checked='<%# Eval("Active") %>' Enabled="false"></asp:CheckBox>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="5%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
					<asp:TemplateField ShowHeader="False">
						<ItemTemplate>
							<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="EditAnnouncement" ToolTip='<%# GetProgramMessage("EditAnnouncement") %>' />
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="False" Width="5%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
			</ss:BaseGridView>
			<div id="divButton" runat="server" visible = "false"  >
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
	<asp:UpdatePanel ID="ctlUpddatePanelAnnouncementLangGrid" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
		<uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
			<asp:UpdateProgress ID="ctlUpdProgressLangGrid" runat="server" AssociatedUpdatePanelID="ctlUpddatePanelAnnouncementLangGrid"
				DynamicLayout="true" EnableViewState="False">
				<ProgressTemplate>
					<uc3:SCGLoading ID="SCGLoading2" runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<fieldset id="ctlFieldSetDetailGridView" runat="server" style="width:100%; text-align:center;">
			<legend id="ctlLegendDetailGridView" style="color:#4E9DDF;">
				<asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$"></asp:Label>
			</legend>
			<table border="0" cellpadding="0" cellspacing="0" width="98%">
				<tr>
					<td align="center">
						<ss:BaseGridView ID="ctlAnnouncementLangGrid" runat="server" AutoGenerateColumns="false"
							CssClass="table" ReadOnly="true" DataKeyNames="LanguageId, AnnouncementId"
							onrowcommand="ctlAnnouncementLangGrid_RowCommand" Width="100%">
							<Columns>
								<asp:TemplateField HeaderText="Language">
									<ItemTemplate><%# Eval("LanguageName")%></ItemTemplate>
									<ItemStyle HorizontalAlign="Center" Width="100px" />
									<HeaderStyle HorizontalAlign="Center" Width="100px" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Announcement Header">
									<ItemTemplate>
										<asp:Label ID="ctlAnnouncemetHeader" runat="server" Text='<%# Eval("AnnouncementHeader") %>'></asp:Label>
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
										<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="Select" ToolTip='<%# GetProgramMessage("EditAnnouncementLang") %>' />
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
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<asp:Panel ID="ctlAnnouncementFormPanel" runat="server" CssClass="modalPopup" Style="display: none" Width="600px">
		<asp:Panel ID="ctlAnnouncementFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
			<div>
				<p>
					<asp:Label ID="ctlCapture" runat="server" SkinID="SkFieldCaptionLabel" Text="Manage Announcement Data"></asp:Label>
				</p>
			</div>
		</asp:Panel>
		<asp:UpdatePanel ID="ctlUpdatePanelAnnouncementForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
			<ContentTemplate>
				<div align="center" style="display: block;">
					<asp:UpdateProgress ID="ctlUpdProgressForm" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAnnouncementForm" DynamicLayout="true" EnableViewState="False">
						<ProgressTemplate>
							<uc3:SCGLoading ID="SCGLoading3" runat="server" />
						</ProgressTemplate>
					</asp:UpdateProgress>
					<table border="0" cellpadding="0" cellspacing="0" class="table">
						<tr>
							<td align="center">
								<asp:FormView ID="ctlAnnouncementForm" runat="server" 
									DataKeyNames="AnnouncementId" oniteminserting="ctlAnnouncementForm_ItemInserting" 
									onitemupdating="ctlAnnouncementForm_ItemUpdating" 
									onmodechanging="ctlAnnouncementForm_ModeChanging" 
									onitemcommand="ctlAnnouncementForm_ItemCommand" ondatabound="ctlAnnouncementForm_DataBound">
									<EditItemTemplate>
										<table>
											<tr>
												<td align="right"><%# GetProgramMessage("AnnouncementHeaderText") %> :</td>
												<td align="left">
													<asp:Label ID="ctlTxtHeader" runat="server" />
													<%--<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("EffectiveDateText") %> :</td>
												<td align="left">
													<table border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left">
																<uc1:Calendar ID="ctlCalEffectiveDate" runat="server" SkinID="SkCtlCalendar" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveDate")) %>' />		
															</td>
															<td align="left">
																<asp:Label ID="lblCalEffectiveDateReq" style="color:Red;" Text="*" runat="server"></asp:Label>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("LastDisplayDateText") %> :</td>
												<td align="left">
													<table border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left">
																<uc1:Calendar ID="ctlCalLastDisplayDate" runat="server" SkinID="SkCtlCalendar" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("LastDisplayDate")) %>' />															
															</td >
															<td align="left">
																<asp:Label ID="lblCalLastDisplayDateReq" style="color:Red;" Text="*" runat="server"></asp:Label>
															</td >
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" SkinID="SkCtlTextboxMultiLine"  runat="server" Text='<%# Eval("Comment") %>' Width="240px" Height="50px" 
														MaxLength="500" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" />
													<%--<asp:Label ID="lblTxtCommentReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
													<asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>' style="padding:0px;" />
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
												<td align="right"><%# GetProgramMessage("AnnouncementHeaderText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtHeader"  runat="server" Width="200px" MaxLength="350" SkinID="SkCtlTextboxLeft" />
													<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("EffectiveDateText") %> :</td>
												<td align="left">
													<table border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left">
																<uc1:Calendar ID="ctlCalEffectiveDate" SkinID="SkCtlCalendar" runat="server" />
															</td>
															<td align="left">
																<asp:Label ID="lblCalEffectiveDateReq" style="color:Red;" Text="*" runat="server"></asp:Label>		
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("LastDisplayDateText") %> :</td>
												<td align="left">
													<table border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left">
																<uc1:Calendar ID="ctlCalLastDisplayDate" SkinID="SkCtlCalendar" runat="server" />
															</td>
															<td align="left">
																<asp:Label ID="lblCalLastDisplayDateReq" style="color:Red;" Text="*" runat="server"></asp:Label>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													<asp:TextBox ID="ctlTxtComment" runat="server" Width="240px" Height="50px" SkinID="SkCtlTextboxMultiLine" MaxLength="500"
														 onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" />
													<%--<asp:Label ID="lblTxtCommentReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
												</td>
											</tr>
											<tr>
												<td align="right"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
													<asp:CheckBox ID="ctlActive" runat="server" Checked="true" style="padding:0px;" />
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
									<spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Announcement.Error" />
								</font>
							</td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</asp:Panel>
	<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
	<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender1" runat="server"
		TargetControlID="lnkDummy" PopupControlID="ctlAnnouncementFormPanel" BackgroundCssClass="modalBackground"
		CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlAnnouncementFormPanelHeader" />
		
		
	<asp:Panel ID="ctlAnnouncementLangFormPanel" runat="server" CssClass="modalPopup" Style="display: none" Width="720px">
		<asp:Panel ID="ctlAnnouncementLangFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
			<div>
				<p>
					<asp:Label ID="lblAnnouncementLangHeader" runat="server" Text="Manage Announcement Language Data"></asp:Label>
				</p>
			</div>
		</asp:Panel>
		<asp:UpdatePanel ID="ctlUpddatePanelAnnouncementLangForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
			<ContentTemplate>
				<div align="center" style="display: block;">
					<asp:UpdateProgress ID="ctlUpdateProgressLangForm" runat="server" 
						AssociatedUpdatePanelID="ctlUpddatePanelAnnouncementLangForm" DynamicLayout="true" 
						EnableViewState="False">
						<ProgressTemplate>
							<uc3:SCGLoading ID="SCGLoading4" runat="server" />
						</ProgressTemplate>
					</asp:UpdateProgress>
					<table border="0" cellpadding="0" cellspacing="0" class="table" width="100%">
						<tr>
							<td align="center">
								<asp:FormView ID="ctlAnnouncementLangForm" runat="server" DataKeyNames="Id" 
									oniteminserting="ctlAnnouncementLangForm_ItemInserting" 
									onitemupdating="ctlAnnouncementLangForm_ItemUpdating" 
									onmodechanging="ctlAnnouncementLangForm_ModeChanging" 
									onitemcommand="ctlAnnouncementLangForm_ItemCommand" 
									ondatabound="ctlAnnouncementLangForm_DataBound">
									<EditItemTemplate>
										<table border="0" cellpadding="0" cellspacing="0" width="100%">
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("AnnouncementHeaderText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtHeader" runat="server" MaxLength="350" Text='<%# Eval("AnnouncementHeader") %>' Width="360px"></asp:TextBox>
													<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:140px;"><%# GetProgramMessage("AnnouncementBodyText") %> :</td>
												<td align="left" valign="top">
													<%--<asp:TextBox ID="ctlTxtBody" runat="server" Text='<%# Eval("AnnouncementBody") %>' Width="300px"
														TextMode="MultiLine" onkeypress="return IsMaxLength(this, 3000);" onkeyup="return IsMaxLength(this, 3000);" Height="100px" />--%>
													<table border="0" width="100%" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left" style="padding-left:3px;">
																<fckeditorv2:fckeditor id="ctlFCK" BasePath="~/fckeditor/" Width="480px" Value='<%# Eval("AnnouncementBody") %>' ToolbarSet="Custom" runat="server"></fckeditorv2:fckeditor>
															</td>
															<td align="left" valign="top">
																&nbsp;<asp:Label ID="lblTxtBodyReq" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("AnnouncementFooterText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtFooter" runat="server" Text='<%# Eval("AnnouncementFooter") %>' Width="360px" MaxLength="350"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:140px;"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtComment" runat="server" Text='<%# Eval("Comment") %>' Width="360px" Height="50px" MaxLength="500"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
													<asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval ("Active") %>' style="padding:0px;" />
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
										<table border="0" cellpadding="0" cellspacing="0" width="100%">
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("AnnouncementHeaderText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtHeader" runat="server" MaxLength="350" Width="360px"></asp:TextBox>
													<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:140px;"><%# GetProgramMessage("AnnouncementBodyText") %> :</td>
												<td align="left" valign="top">
													<%--<asp:TextBox ID="ctlTxtBody" runat="server" Width="300px" 
														TextMode="MultiLine" onkeypress="return IsMaxLength(this, 3000);" onkeyup="return IsMaxLength(this, 3000);" Height="100px" />--%>
													<table border="0" width="100%" cellpadding="0" cellspacing="0">
														<tr>
															<td align="left" style="padding-left:3px;">
																<fckeditorv2:fckeditor id="ctlFCK" BasePath="~/fckeditor/" Width="480px" ToolbarSet="Custom" runat="server" ></fckeditorv2:fckeditor>	
															</td>
															<td align="left" valign="top">
																&nbsp;<asp:Label ID="lblTxtBodyReq" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("AnnouncementFooterText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtFooter" runat="server" Width="360px" MaxLength="350"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" style="width:140px;"><%# GetProgramMessage("DescriptionText") %> :</td>
												<td align="left">
													&nbsp;<asp:TextBox ID="ctlTxtComment" runat="server" MaxLength="500" Width="360px" Height="50px"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="right" style="width:140px;"><%# GetProgramMessage("ActiveText") %> :</td>
												<td align="left">
													&nbsp;<asp:CheckBox ID="ctlActive" runat="server" Checked="true" style="padding:0px;" />
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
									<spring:ValidationSummary ID="ctlValidationSummaryLang" runat="server" Provider="AnnouncementLang.Error" />
								</font>
							</td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</asp:Panel>
	<asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" />
	<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender2" runat="server"
		TargetControlID="lnkDummy2" PopupControlID="ctlAnnouncementLangFormPanel" BackgroundCssClass="modalBackground"
		CancelControlID="lnkDummy2" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlAnnouncementLangFormPanelHeader" />
</asp:Content>
