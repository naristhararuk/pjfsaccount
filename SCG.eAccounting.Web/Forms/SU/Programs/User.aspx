<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="User.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.User" EnableTheming="true"  
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register assembly="SS.Standard.UI" namespace="SS.Standard.UI" tagprefix="ss" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
				</ProgressTemplate>
			</asp:UpdateProgress>
			<ss:BaseGridView ID="ctlUserGrid" runat="server" AutoGenerateColumns="false" CssClass="table"
                EnableInsert="False" ReadOnly="true" DataKeyNames="UserID" DataSourceID="SuUserDataSource" 
                InsertRowCount="1" AllowPaging="True" SaveButtonID="" OnRowCommand="ctlUserGrid_RowCommand"
                OnDataBound="ctlUserGrid_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Name">
                        <ItemTemplate>
							<asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EffDate">
                        <ItemTemplate>
							<asp:Label ID="lblEffDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffDate").ToString()) %>'></asp:Label>
						</ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EndDate" >
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EndDate").ToString()) %>'></asp:Label>
						</ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ChangePassword" >
                        <ItemTemplate>
							<asp:CheckBox ID="chkChangePassword" runat="server" Checked='<%# Bind("ChangePassword") %>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Organization">
                        <ItemTemplate>
                           <asp:Label ID="ctlOrganization" runat="server" Text='<%# Bind("OrganizationName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Division">
                        <ItemTemplate>
                            <asp:Label ID="ctlDivision" runat="server" Text='<%# Bind("DivisionName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Language">
                        <ItemTemplate>
                            <asp:Label ID="ctlLanguage" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="ctlComment" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="UserEdit" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left">
                <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" />
                <span class="spanSeparator"> | </span>
                <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Button ID="Button1" runat="server" Text="TH" onclick="Button1_Click" Width="0"/>
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="EN" Width="0"/>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:ObjectDataSource ID="SuUserDataSource" runat="server"
        SelectMethod="GetTranslatedList" TypeName="SS.SU.Query.ISuUserQuery"
        OnObjectCreating="SuUserDataSource_ObjectCreating"
        OnUpdating="SuUserDataSource_Updating" 
        OnInserting="SuUserDataSource_Inserting" 
		onselecting="SuUserDataSource_Selecting">
		<SelectParameters>
			<asp:Parameter Name="languageID" Type="Int16" />
		</SelectParameters>
    </asp:ObjectDataSource>
    <%--<asp:ObjectDataSource ID="SuUserDataSource" runat="server" 
        SelectMethod="GetUserList" TypeName="SS.SU.Query.Hibernate.SuUserQuery"
        OnObjectCreating="SuUserDataSource_ObjectCreating"
        OnUpdating="SuUserDataSource_Updating" 
        OnInserting="SuUserDataSource_Inserting" 
		onselecting="SuUserDataSource_Selecting">
		<SelectParameters>
			<asp:Parameter Name="languageID" Type="Int16" />
		</SelectParameters>
    </asp:ObjectDataSource>--%>
    <asp:ObjectDataSource ID="SuOrganizationDataSource" runat="server" 
		OnObjectCreating="SuOrganizationDataSource_ObjectCreating"
        SelectMethod="GetTranslatedList" 
		TypeName="SS.SU.Query.ISuOrganizationQuery"
        OnSelecting="SuOrganizationDataSource_Selecting">
        <SelectParameters>
            <asp:Parameter Name="languageID" Type="Int16" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="DbLanguageDataSource" runat="server" OnObjectCreating="DbLanguageDataSource_ObjectCreating"
        SelectMethod="FindAll" TypeName="SS.DB.Query.IDbLanguageQuery"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="SuDivisionDataSource" runat="server" 
		onobjectcreating="SuDivisionDataSource_ObjectCreating" 
		onselecting="SuDivisionDataSource_Selecting" 
		SelectMethod="GetTranslatedList" 
		TypeName="SS.SU.Query.Hibernate.SuDivisionQuery">
		<SelectParameters>
			<asp:Parameter Name="languageID" Type="Int16" />
		</SelectParameters>
	</asp:ObjectDataSource>
	
    <asp:Panel ID="ctlUserFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlUserFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
				<p>
					<asp:Label ID="lblCapture" runat="server" Text="Manage User Data" Width="160px"></asp:Label>
				</p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelUserForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="UpdatePanelUserFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelUserForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlUserForm" runat="server" OnItemCommand="ctlUserForm_ItemCommand"
									OnDataBound="ctlUserForm_DataBound" DataKeyNames="Userid" oniteminserting="ctlUserForm_ItemInserting" 
									onmodechanging="ctlUserForm_ModeChanging" onitemupdating="ctlUserForm_ItemUpdating">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlUserNameLabel" runat="server" Text="$UserName$"></asp:Label>:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlUserName" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Bind("UserName") %>' Width="132px" />
                                                    <asp:RequiredFieldValidator ID="rfvTxtCode" runat="server" ControlToValidate="ctlUserName"
                                                        ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Password$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlPassword" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Bind("Password") %>' TextMode="Password"
                                                        Width="132px" />
                                                    <asp:RequiredFieldValidator ID="rfvTxtName" runat="server" ErrorMessage="RequiredFieldValidator"
                                                        ControlToValidate="ctlPassword" ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $EffDate$ :
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="ctlEffDate" runat="server" SkinID="SkCtlCalendar"  
                                                        DateValue='<%# Eval("EffDate") %>' />
                                                    <asp:CustomValidator ID="cvEffDate" runat="server" ControlToValidate="ctlEffDate"
                                                        ValidationGroup="ValidateFormView">*</asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $EndDate$ :
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="ctlEndDate" runat="server" SkinID="SkCtlCalendar"  
                                                        DateValue='<%# Eval("EndDate") %>'  />
                                                    <asp:CustomValidator ID="cvEndDate" runat="server" ControlToValidate="ctlEndDate"
                                                        ValidationGroup="ValidateFormView">*</asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Fail Time$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlFailtime" SkinID="SkCtlTextboxLeft" runat="server" Text='<%# Bind("FailTime") %>' Width="132px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $ChangePassword$ :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox runat="server" ID="ctlChangePassword" SkinID="SkCtlTextboxLeft" Checked='<%# Bind("ChangePassword") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Comment$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" Text='<%# Bind("Comment") %>' Width="132px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Organization$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlOrganization" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True"  SelectedValue='<%# Eval("Organization.OrganizationID") %>'
                                                        DataSourceID="SuOrganizationDataSource" DataTextField="Text" DataValueField="Id">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Division$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlDivision" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True" 
														SelectedValue='<%# Eval("Division.DivisionID") %>'
														DataSourceID="SuDivisionDataSource" DataTextField="Text" DataValueField="Id">
														<asp:ListItem Text="$Pleaseselect$"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Language$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlLanguage" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True" DataSourceID="DbLanguageDataSource"
                                                        DataTextField="LanguageName" DataValueField="Languageid" SelectedValue='<%# Eval("Language.LanguageID") %>' >
                                                        <asp:ListItem  Text="$Pleaseselect$"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Active$ :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    $UserName$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlUserName" runat="server" SkinID="SkCtlTextboxLeft" Width="132px" />
                                                    <asp:RequiredFieldValidator ID="rfvTxtCode" runat="server" ControlToValidate="ctlUserName"
                                                        ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Password$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlPassword" SkinID="SkCtlTextboxLeft" runat="server" TextMode="Password"
                                                        Width="132px" />
                                                    <asp:RequiredFieldValidator ID="rfvTxtName" runat="server" ErrorMessage="RequiredFieldValidator"
                                                        ControlToValidate="ctlPassword" ValidationGroup="ValidateFormView">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $EffDate$ :
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="ctlEffDate" runat="server" SkinID="SkCtlCalendar"/>
                                                    <asp:CustomValidator ID="cvEffDate" runat="server" ControlToValidate="ctlEffDate"
                                                        ValidationGroup="ValidateFormView">*</asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $EndDate$ :
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="ctlEndDate" runat="server" SkinID="SkCtlCalendar"/>
                                                    <asp:CustomValidator ID="cvEndDate" runat="server" ControlToValidate="ctlEndDate"
                                                        ValidationGroup="ValidateFormView">*</asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Fail Time$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlFailtime" SkinID="SkCtlTextboxLeft" runat="server" Width="132px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $ChangePassword$ :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox runat="server" ID="ctlChangePassword" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Comment$ :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" Width="132px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Organization$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlOrganization" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SuOrganizationDataSource" DataTextField="Text" DataValueField="Id">
                                                        <asp:ListItem Value="1">-- Please select --</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Division$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlDivision" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True"
                                                        DataSourceID="SuDivisionDataSource" DataTextField="Text" DataValueField="Id">
                                                        <asp:ListItem Value="1">-- Please select --</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Language$ :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ctlLanguage" SkinID="SkCtlDropDownList" runat="server" AppendDataBoundItems="True" 
														DataSourceID="DbLanguageDataSource"
                                                        DataTextField="LanguageName" DataValueField="Languageid">
                                                        <asp:ListItem Value="1">-- Please select --</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    $Active$ :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" 
														CausesValidation="True" ValidationGroup="ValidateFormView" 
														CommandName="Insert" Text="Insert"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" 
														CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:FormView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="vsSummary" runat="server" Style="text-align: left" Width="250px"
                                    ValidationGroup="ValidateFormView" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display:none" />
    <ajaxToolkit:ModalPopupExtender 
		ID="ctlUserModalPopupExtender" runat="server" 
		TargetControlID="lnkDummy"
        PopupControlID="ctlUserFormPanel" 
        BackgroundCssClass="modalBackground" 
        CancelControlID="lnkDummy"
        DropShadow="true" 
        RepositionMode="None" 
        PopupDragHandleControlID="ctlUserFormPanelHeader" />
</asp:Content>
