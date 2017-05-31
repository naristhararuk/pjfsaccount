<%@ Page 
    Language="C#" 
    MasterPageFile="~/ProgramsPages.Master"  
    AutoEventWireup="true" 
    CodeBehind="Parameter.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SS.DB.Programs.Parameter" 
    Title="Parameter Setup" 
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

<%-- DbParameterGroup --%>
<asp:UpdatePanel ID="UpdatePanelParamaterGroupGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>      
    <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>  
        <asp:UpdateProgress ID="UpdatePanelParameterGroupGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelParamaterGroupGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <ss:BaseGridView ID="ctlGridParameterGroup" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="True"
                DataKeyNames        = "GroupNo" 
                OnRequestCount      = "ctlParameterGroup_RequestCount" 
                OnRequestData       = "ctlParameterGroup_RequestData"
                OnRowCommand        = "ctlGridParameterGroup_RowCommand" 
                OnDataBound         = "ctlGridParameterGroup_DataBound" 
                onpageindexchanged  = "ctlGridParameterGroup_PageIndexChanged"
                CssClass="table"
                SelectedRowStyle-BackColor="#6699FF" HeaderStyle-CssClass="GridHeader"> 
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>                    
                    <asp:TemplateField HeaderText="GroupName" SortExpression="DbParameterGroup.GroupName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlGroupName" CommandName="Select" runat="server" Text='<%# Bind("GroupName")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                          
                    <asp:TemplateField HeaderText="Comment" SortExpression="DbParameterGroup.Comment" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlComment" runat="server" Text='<%# Bind("Comment")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>             
                </Columns>
            </ss:BaseGridView>           
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<br />
<%-- DbParameter --%>
<asp:UpdatePanel ID="UpdatePanelParamaterGridView" runat="server" UpdateMode="Conditional">
<ContentTemplate>        
        <asp:UpdateProgress ID="UpdatePanelParameterGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelParamaterGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <fieldset id="ctlFieldSetDetailGridView" runat="server" style="width:100%; text-align:center;">
	        <legend id="ctlLegendDetailGridView" style="color:#4E9DDF;">
		        <asp:Label ID="lblDetailHeader" runat="server" Text="$Parameter$" SkinID="SkGeneralLabel"></asp:Label>
	        </legend> 
	        <table border="0" cellpadding="0" cellspacing="0" width="98%" class="table">
		        <tr>
			        <td align="center">
                        <ss:BaseGridView ID="ctlGridParameter" runat="server" Width="100%" AutoGenerateColumns="False"
                            EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="True"
                            DataKeyNames        = "Id" 
                            OnRequestCount      = "ctlParameter_RequestCount" 
                            OnRequestData       = "ctlParameter_RequestData"
                            OnRowCommand        = "ctlGridParameter_RowCommand" 
                            OnDataBound         = "ctlGridParameter_DataBound" 
                            onpageindexchanged  = "ctlGridParameter_PageIndexChanged"
                            CssClass="table"
                            SelectedRowStyle-BackColor="#6699FF" HeaderStyle-CssClass="GridHeader"> 
                            <AlternatingRowStyle CssClass="GridItem" />
                            <RowStyle CssClass="GridAltItem" />
                            <Columns>                               
                                <asp:TemplateField HeaderText="Configuration Name" SortExpression="DbParameter.ConfigurationName" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlConfigurationName" runat="server" Text='<%# Bind("ConfigurationName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Value" SortExpression="DbParameter.ParameterValue" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlParameterValue" runat="server" Text='<%# Bind("ParameterValue")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comment" SortExpression="DbParameter.Comment" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlComment" runat="server" Text='<%# Bind("Comment")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parameter Type$" SortExpression="DbParameter.ParameterType" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlParameterType" runat="server" Text='<%# Bind("ParameterType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>            
                                <asp:TemplateField ShowHeader="False" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="ParameterEdit" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>    
                </tr>
            </table>
        </fieldset>
    </ContentTemplate>    
 </asp:UpdatePanel>

<%-- --------- --%>
<%-- Grid View --%>
<%-- --------- --%>

<%-- Grid View DbParameter --%>
<asp:Panel ID="ctlParameterFormPanel" runat="server" Style="display: none" CssClass="modalPopup" Width="500px">
    <asp:Panel ID="ctlParameterFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlParameterHeader" runat="server" Text="Manage Parameter Data" Width="160px" SkinID="SkGeneralLabel"></asp:Label>
            </p>
        </div>
    </asp:Panel>            
    <asp:UpdatePanel ID="UpdatePanelParameterForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>         
        <div align="center" style="display: block;">            
            <asp:UpdateProgress ID="UpdatePanelParameterFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelParameterForm" DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>     
            <table border="0" cellpadding="0" cellspacing="0" class="table">
                <tr>
                    <td align="center">
                        <asp:FormView ID="ctlParameterFormView" runat="server" DataKeyNames="Id" 
                            OnDataBound     = "ctlParameterFormView_DataBound"
                            OnItemCommand   = "ctlParameterFormView_ItemCommand" 
                            OnItemUpdating  = "ctlParameterFormView_ItemUpdating"
                            OnModeChanging  = "ctlParameterFormView_ModeChanging">                            
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlGroupNameLabel" runat="server" Text='<%# GetProgramMessage("$Group$") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlGroupName" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlConfigurationNameLabel" runat="server" Text='<%# GetProgramMessage("$Name$") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlConfigurationName" runat="server" Text='<%# Bind("ConfigurationName") %>' ></asp:Label>
                                        </td>
                                    </tr>         
                                    <asp:Panel ID="ctlTextPanel" runat="server">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ctlParamterValueLabel" runat="server" Text='<%# GetProgramMessage("$Parameter Value$") %>'></asp:Label>&nbsp;:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlParameterValueText" runat="server" Text='<%# Bind("ParameterValue") %>' MaxLength="400" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="ctlIntegerPanel" runat="server">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ctlParameterValueIntLabel" runat="server" Text='<%# GetProgramMessage("$Parameter Value Int$") %>'></asp:Label>&nbsp;:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlParameterValueInt" runat="server" Text='<%# Bind("ParameterValue") %>' MaxLength="400" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="ctlDatePanel" runat="server">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ctlParameterValueDateLabel" runat="server" Text='<%# GetProgramMessage("$Parameter Value Date$") %>'></asp:Label>&nbsp;:
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="ctlParameterValueDate" runat="server" style="display:none" Text='<%# Bind("ParameterValue") %>'></asp:Label> 
                                                <uc1:calendar ID="ctlCalendar" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# GetProgramMessage("$Comment$") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlComment" runat="server" Text='<%# Bind("Comment") %>' ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlParameterTypeLabel" runat="server" Text='<%# GetProgramMessage("$Parameter Type$") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlParameterType" runat="server" Text='<%# Bind("ParameterType") %>' ></asp:Label>
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
                        </asp:FormView>
                    </td>
                </tr>
                <tr>
					<td>
						<font color="red" style="text-align: left">
							<spring:ValidationSummary ID="ctlValidationSummaryParameter" runat="server" Provider="Parameter.Error" />
						</font>
					</td>
				</tr>
            </table> 
        </div>
        </ContentTemplate>    
    </asp:UpdatePanel>
</asp:Panel>

<%-- ----------- --%>
<%-- Link Button --%>
<%-- ----------- --%>

<%-- Link Button DbParameter --%>        
<asp:LinkButton ID="lnkDummyParameter" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlParameterModalPopupExtender" runat="server" TargetControlID="lnkDummyParameter"
        PopupControlID="ctlParameterFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummyParameter"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlParameterFormPanelHeader" /> 
</asp:Content>