<%@ Page 
Language="C#" 
MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true" 
CodeBehind="Province.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SS.DB.Programs.Province" 
Title="Untitled Page" 
StylesheetTheme="Default"
%>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="calendar" TagPrefix="uc1" %>
<%@ Register Assembly="SS.Standard.UI" Namespace="SS.Standard.UI" TagPrefix="ss" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<%-- ------------ --%>
<%-- Main Program --%>
<%-- ------------ --%>
<asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        
        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <ss:BaseGridView ID="ctlGridProvince" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="True"
                DataKeyNames        = "ProvinceId" 
                OnRequestCount      = "RequestCount" 
                OnRequestData       = "RequestData"
                OnRowCommand        = "ctlGridProvince_RowCommand" 
                OnDataBound         = "ctlGridProvince_DataBound" 
                onpageindexchanged  = "ctlGridProvince_PageIndexChanged"
                CssClass="table"
                SelectedRowStyle-BackColor="#6699FF" >
                <Columns>
                    
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="ProvinceId" SortExpression="DbProvince.ProvinceId" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlLinkProvinceId" CommandName="Select" runat="server" Text='<%# Bind("ProvinceId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                                        
                    <asp:TemplateField HeaderText="ProvinceName" SortExpression="DbProvinceLang.ProvinceName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblProvinceName" runat="server" Text='<%# Bind("ProvinceName")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="RegionName" SortExpression="DbRegionLang.RegionName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblRegionName" runat="server" Text='<%# Bind("RegionName")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Comment" SortExpression="DbProvince.Comment" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlChkComment" runat="server" Text='<%# Bind("Comment")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Active" SortExpression="DbProvince.Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false">
                            </asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="False" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="ProvinceEdit" ToolTip='<%# GetProgramMessage("EditProvince") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                    
                </Columns>
            </ss:BaseGridView>
            
            <div id="divButton" runat="server" align="left">
				<table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton ID="ctlBtnAddProvince" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlBtnAddProvince_Click"/>
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlBtnDeleteProvince" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlBtnDeleteProvince_Click"/>
						</td>
					</tr>
				</table>
            </div>               
            
    </ContentTemplate>

</asp:UpdatePanel>

<%-- ------------ --%>
<%-- Grid View    --%>
<%-- ------------ --%>
<asp:Panel ID="ctlProvinceFormPanel" runat="server" Style="display: block" CssClass="modalPopup" Width="500px">

    <asp:Panel ID="ctlProvinceFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlLblProvinceFormPanelHeader" runat="server" Text="$Manage Province Data$" Width="160px"></asp:Label>
            </p>
        </div>
    </asp:Panel>
            
    <asp:UpdatePanel ID="UpdatePanelProvinceForm" runat="server" UpdateMode="Conditional">

        <ContentTemplate> 
        
        <div align="center" style="display: block;">
            
            <asp:UpdateProgress ID="UpdatePanelProvinceFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelProvinceForm" DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
                        
            <table border="0" cellpadding="0" cellspacing="0" class="TableInFormView">
                <tr>
                    <td align="center">
                        <asp:FormView ID="ctlProvinceFormView" runat="server" DataKeyNames="ProvinceId" 
                            OnDataBound     = "ctlProvinceFormView_DataBound"
                            OnItemCommand   = "ctlProvinceFormView_ItemCommand" 
                            OnItemInserting = "ctlProvinceFormView_ItemInserting"
                            OnItemUpdating  = "ctlProvinceFormView_ItemUpdating" 
                            OnModeChanging   = "ctlProvinceFormView_ModeChanging">
                            
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblProvinceID" runat="server" Text='<%# GetProgramMessage("ProvinceId") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlLblProvinceIDShow" runat="server" />
                                            <asp:Label ID="lblOrganizationReq1" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblProvinceName" runat="server" Text='<%# GetProgramMessage("ProvinceName") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlLblProvinceNameShow" runat="server" />
                                            <asp:Label ID="Label1" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>    
                                        
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblRegionId" runat="server" Text='<%# GetProgramMessage("RegionID") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ctlCmbRegionId" SkinID="SkCtlDropDownList" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblComment" runat="server" Text='<%# GetProgramMessage("Comment") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtComment" runat="server" Text='<%# Bind("Comment") %>' MaxLength="500" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="chkActiveLabel" runat="server" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Update" ToolTip='<%# GetProgramMessage("UpdateProvince") %>'></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" ToolTip='<%# GetProgramMessage("CancelProvince") %>'></asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                                        
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblProvinceID" runat="server" Text='<%# GetProgramMessage("ProvinceId") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlLblProvinceIDShow" runat="server" Text = '<%# GetProgramMessage("AUTO") %>' />
                                            <asp:Label ID="Label2" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblProvinceName" runat="server" Text='<%# GetProgramMessage("ProvinceName") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtProvinceName" MaxLength="200" runat="server"></asp:TextBox>
                                            <asp:Label ID="Label1" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblRegionId" runat="server" Text='<%# GetProgramMessage("RegionID") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ctlCmbRegionId" SkinID="SkCtlDropDownList" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblComment" runat="server" Text='<%# GetProgramMessage("Comment") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtComment" runat="server" MaxLength="500" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="chkActiveLabel" runat="server" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked='true' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Insert" ToolTip='<%# GetProgramMessage("InsertProvince") %>'></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" ToolTip='<%# GetProgramMessage("CancelProvince") %>'></asp:ImageButton>
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
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Province.Error" />
                        </font>
                    </td>
                </tr>
            </table>
                        
        </div>
        
        </ContentTemplate>
    
    </asp:UpdatePanel>

</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlProvinceModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlProvinceFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlProvinceFormPanelHeader" />

<%-- ------------------- --%>
<%-- Grid AccountLang    --%>
<%-- ------------------- --%>
<asp:UpdatePanel ID="ctlProvinceLangUpdatePanel" runat="server" UpdateMode="Conditional">

<ContentTemplate>
    
    <asp:UpdateProgress ID="UpdatePanelProgramLangFormProgress" runat="server" AssociatedUpdatePanelID="ctlProvinceLangUpdatePanel" DynamicLayout="true" EnableViewState="False">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <fieldset style="width:100%;text-align:Center" id="ctlProvinceLangLangFds" runat="server" visible="false">
    <legend id="ctlLegendDetailGridView" style="color:#4E9DDF">
    <asp:Label ID="ctlLblTitleLang" runat="server" Text="$Province Language Setup$" Width="160px"></asp:Label>
    </legend> 
    
        <ss:BaseGridView ID="ctlProvinceLangGrid" runat="server"  AutoGenerateColumns="false" Width="98%"
        CssClass="table" DataKeyNames="LanguageId" ReadOnly="true" 
        ondatabound="ctlProvinceLangGrid_DataBound">
        
        <Columns>
            <asp:TemplateField HeaderText="LanguageName" HeaderStyle-HorizontalAlign="Center" SortExpression="LanguageName">
                <ItemTemplate>
                    <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="100px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Province Name" HeaderStyle-HorizontalAlign="Center" SortExpression="ProvinceName">
                <ItemTemplate>
                    <asp:TextBox ID="ctlProvinceName" runat="server" Width="95%" MaxLength="350" Text='<%# Bind("ProvinceName") %>' />
                </ItemTemplate>
                <ItemStyle Width="31%" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                <ItemTemplate>
                    <asp:TextBox ID="ctlComment" runat="server" Width="95%" MaxLength="500" Text='<%# Bind("Comment") %>'></asp:TextBox>
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
    
    <div id="ctlButtonGridDetail" runat="server" align="left">
		<table border="0">
			<tr>
				<td valign="middle">
					<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" Text="Submit" ToolTip="Save" Visible="false" />
				</td>
				<td valign="middle"> | </td>
				<td valign="middle">
					<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" Text="Cancel" ToolTip="Cancel" Visible="false"/>
				</td>
			</tr>
		</table>
    </div>
               
    </fieldset>
    
</ContentTemplate>

</asp:UpdatePanel>

</asp:Content>
