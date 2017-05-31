<%@ Page 
Language="C#" 
MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true" 
CodeBehind="Bank.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Bank" 
Title="Bank Setup" 
StylesheetTheme="Default" meta:resourcekey="PageResource1"
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
        
        <ss:BaseGridView ID="ctlGridBank" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="True"
                DataKeyNames        = "BankId" 
                OnRequestCount      = "RequestCount" 
                OnRequestData       = "RequestData"
                OnRowCommand        = "ctlGridBank_RowCommand" 
                OnDataBound         = "ctlGridBank_DataBound" 
                onpageindexchanged  = "ctlGridBank_PageIndexChanged"
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
                    
                    <asp:TemplateField HeaderText="BankNo" SortExpression="DbBank.BankNo" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlLinkBankNo" CommandName="Select" runat="server" Text='<%# Bind("BankNo")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Comment" SortExpression="DbBank.Comment" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLinkComment" runat="server" Text='<%# Bind("Comment")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Active" SortExpression="DbBank.Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false">
                            </asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="False" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="BankEdit" ToolTip='<%# GetProgramMessage("EditBank") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            
            <div id="divButton" runat="server" align="left">
				<table border="0">
					<tr>
						<td valign="middle">
							<asp:ImageButton ID="ctlBtnAddBank" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlBtnAddBank_Click"/>
						</td>
						<td valign="middle"> | </td>
						<td valign="middle">
							<asp:ImageButton ID="ctlBtnDeleteBank" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlBtnDeleteBank_Click"/>
						</td>
					</tr>
				</table>
            </div>        
            
    </ContentTemplate>

</asp:UpdatePanel>

<%-- ------------ --%>
<%-- Grid View    --%>
<%-- ------------ --%>
<asp:Panel ID="ctlBankFormPanel" runat="server" Style="display: block" CssClass="modalPopup" Width="500px">

    <asp:Panel ID="ctlBankFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlLblBankFormPanelHeader" runat="server" Text="$Manage Bank Data$" Width="160px"></asp:Label>
            </p>
        </div>
    </asp:Panel>
            
    <asp:UpdatePanel ID="UpdatePanelBankForm" runat="server" UpdateMode="Conditional">

        <ContentTemplate> 
        
        <div align="center" style="display: block;">
            
            <asp:UpdateProgress ID="UpdatePanelBankFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelBankForm" DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
                        
            <table border="0" cellpadding="0" cellspacing="0" class="TableInFormView">
                <tr>
                    <td align="center">
                        <asp:FormView ID="ctlBankFormView" runat="server" DataKeyNames="BankId" 
                            OnDataBound     = "ctlBankFormView_DataBound"
                            OnItemCommand   = "ctlBankFormView_ItemCommand" 
                            OnItemInserting = "ctlBankFormView_ItemInserting"
                            OnItemUpdating  = "ctlBankFormView_ItemUpdating" 
                            OnModeChanging   = "ctlBankFormView_ModeChanging">
                            
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblBankNo" runat="server" Text='<%# GetProgramMessage("BankNo") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtBankNo" runat="server" Text='<%# Bind("BankNo") %>' MaxLength="10" />
                                            <asp:Label ID="lblOrganizationReq0" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblComment" runat="server" Text='<%# GetProgramMessage("BankComment") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtComment" runat="server" Text='<%# Bind("Comment") %>' MaxLength="500" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="chkActiveLabel" runat="server" Text='<%# GetProgramMessage("BankActive") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("UpdateBank") %>' ></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelBank") %>' ></asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                                        
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblBankNo" runat="server" Text='<%# GetProgramMessage("BankNo") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtBankNo" runat="server" MaxLength="10" />
                                            <asp:Label ID="lblOrganizationReq0" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblComment" runat="server" Text='<%# GetProgramMessage("BankComment") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtComment" runat="server" MaxLength="500" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="chkActiveLabel" runat="server" Text='<%# GetProgramMessage("BankActive") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked=true />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Insert" Text="Insert" ToolTip='<%# GetProgramMessage("InsertBank") %>' ></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelBank") %>' ></asp:ImageButton>
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

<%-- ------------------- --%>
<%-- Grid BankLang    --%>
<%-- ------------------- --%>
<asp:UpdatePanel ID="ctlBankLangUpdatePanel" runat="server" UpdateMode="Conditional">

<ContentTemplate>
    
    <asp:UpdateProgress ID="UpdatePanelProgramLangFormProgress" runat="server" AssociatedUpdatePanelID="ctlBankLangUpdatePanel" DynamicLayout="true" EnableViewState="False">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <fieldset style="width:100%;text-align:Center" id="ctlBankLangLangFds" runat="server" visible="false">
    <legend id="ctlLegendDetailGridView" style="color:#4E9DDF">
    <asp:Label ID="ctlLblTitleEditLang" runat="server" Text="$Bank Language Setup$" Width="160px"></asp:Label>
    </legend> 
    
        <ss:BaseGridView ID="ctlBankLangGrid" runat="server"  AutoGenerateColumns="false" Width="98%"
        CssClass="table" DataKeyNames="LanguageId" ReadOnly="true" 
        ondatabound="ctlBankLangGrid_DataBound">
        
        <Columns>
            <asp:TemplateField HeaderText="LanguageName" HeaderStyle-HorizontalAlign="Center" SortExpression="LanguageName">
                <ItemTemplate>
                    <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="100px" HorizontalAlign="Center" />
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Bank Name" HeaderStyle-HorizontalAlign="Center" SortExpression="BankName">
                <ItemTemplate>
                    <asp:TextBox ID="ctlBankName" runat="server" Width="95%" MaxLength="350" Text='<%# Bind("BankName") %>' />
                </ItemTemplate>
                <ItemStyle Width="31%" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ABBR Name" HeaderStyle-HorizontalAlign="Center" SortExpression="ABBRName">
                <ItemTemplate>
                    <asp:TextBox ID="ctlABBRName" runat="server" Width="95%" MaxLength="350" Text='<%# Bind("ABBRName") %>' />
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


<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlBankModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlBankFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlBankFormPanelHeader" />
</asp:Content>
