<%@ Page Title="" Language="C#" StylesheetTheme="Default" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="Zone.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SS.DB.Programs.Zone" %>
<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
 <asp:UpdatePanel ID="ctlZoneUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlZoneUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <ss:BaseGridView ID="ctlZoneGridView" runat="server" AutoGenerateColumns="false"
                CssClass="table" AllowSorting="true" AllowPaging="true" DataKeyNames="ZoneID"
                EnableInsert="False" ReadOnly="true" OnRowCommand="ctlZoneGridView_RowCommand"
                OnRowDataBound="ctlZoneGridView_RowDataBound" OnRequestCount="RequestCount" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                OnDataBound="ctlZoneGridView_DataBound" OnPageIndexChanged="ctlZoneGridView_PageIndexChanged">
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
                    <asp:TemplateField HeaderText="Zone Name" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="ZoneName">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlZoneName" Width="98%" runat="server" CommandName="Select" Text='<%# Bind("ZoneName") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="25%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:Label ID="ctlAccNameLabel" runat="server" Text='<%# Bind("Comment") %>' />
                        </ItemTemplate>  
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlRecieptTypeActive" Checked='<%# Bind("Active") %>' runat="server"
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlZoneEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="UserEdit" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                        runat="server"></asp:Label></EmptyDataTemplate><EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="vertical-align: middle;">
                <table style="text-align:center;">
                <tr><td>
                <asp:ImageButton runat="server" ID="ctlAddNew" ToolTip='<%# GetProgramMessage("Add") %>' SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" /></td>
                <td><span class="spanSeparator">| </span></td>
                <td><asp:ImageButton ID="ctlZoneDelete" ToolTip='<%# GetProgramMessage("Delete") %>' runat="server" SkinID="SkCtlGridDelete" 
                    OnClick="ctlDelete_Click" /></td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="ValidationSummaryDiv" style="width:100%;text-align:center;" runat="server">
                    <font color="red">
                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="FnReceiptType.Error" />
                    </font>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="ctlZoneLangUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server" />
            <asp:UpdateProgress ID="UpdatePanelZoneLangFormProgress" runat="server" AssociatedUpdatePanelID="ctlZoneLangUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 100%; text-align: Center" id="ctlZoneLangFds" runat="server"
                visible="false">
                <legend id="ctlLegendDetailGridView" style="color: #4E9DDF">
                    <asp:Label ID="ctlLegendDetailGridViewLabel" runat="server" Text="$DisPlay Setting$"></asp:Label></legend>
                    <ss:BaseGridView ID="ctlZoneLangGrid" runat="server" AutoGenerateColumns="false"
                    Width="98%" CssClass="table" DataKeyNames="LanguageId" OnDataBound="ctlZoneLangGrid_DataBound"
                    ReadOnly="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Language Name" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="LanguageName">
                            <ItemTemplate>
                                <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label></ItemTemplate><ItemStyle Width="100px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReceiveType Name" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="ZoneName">
                            <ItemTemplate>
                                <asp:TextBox ID="ctlZoneName" SkinID="SkCtlTextboxLeft" runat="server" Width="95%"
                                    MaxLength="200" Text='<%# Bind("ZoneName") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="31%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                            <ItemTemplate>
                                <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxLeft" runat="server" Width="95%"
                                    MaxLength="500" Text='<%# Bind("Comment") %>'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <div id="ctlZoneLangButton" runat="server" style="text-align: left;" visible="false">
                    <table style="text-align: center;">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" 
                                    Text="Submit" onclick="ctlSubmit_Click" />
                            </td>
                            <td>
                                <span class="spanSeparator" style="vertical-align: top;">| </span>
                            </td>
                            <td>
                                <asp:ImageButton ID="ctlCancel" runat="server"  SkinID="SkCtlFormCancel" 
                                    Text="Cancel" onclick="ctlCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </fieldset>
           
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:Panel ID="ctlZoneFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlZoneFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text="Manage Zone Data" Width="160px"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlZoneFormUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ZoneFormProgressctlUpdatePanel" runat="server" AssociatedUpdatePanelID="ctlZoneFormUpdatePanel"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlZoneForm" runat="server" DataKeyNames="ZoneID,ZoneLangID" OnItemCommand="ctlZoneForm_ItemCommand"
                                    OnItemInserting="ctlZoneForm_ItemInserting" OnItemUpdating="ctlZoneForm_ItemUpdating"
                                    OnModeChanging="ctlZoneForm_ModeChanging" 
                                    ondatabound="ctlZoneForm_DataBound">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("ZoneName") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="ctlZoneNameLabel" runat="server" SkinID="SkCtlTextboxLeft" Text='<%# Bind("ZoneName") %>'/>
                                                       <font color="red"><asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine"  Height="50px" SkinID="SkCtlTextboxMultiLine"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Width="250px" Text='<%# Bind("Comment") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Bind("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlZoneUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" ToolTip='<%# GetProgramMessage("Update") %>'
                                                        ValidationGroup="ValidateFormView" CommandName="Update" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlZoneCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False" ToolTip='<%# GetProgramMessage("Cancel") %>'
                                                        CommandName="Cancel" Text="Cancel"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("ZoneName") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlZoneName" MaxLength="50" runat="server" Width="250px" SkinID="SkCtlTextboxLeft" />
                                                    <font color="red"><asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine"  Height="50px" SkinID="SkCtlTextboxLeft"
														onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);" Width="250px"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                   <%# GetProgramMessage("Active") %> &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlZoneUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True" 
                                                        ValidationGroup="ValidateFormView" CommandName="Insert" Text="Insert"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlZoneCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False" 
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
                                    <spring:ValidationSummary ID="ValidationSummary1" runat="server" Provider="Currency.Error" />
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
    <ajaxToolkit:ModalPopupExtender ID="ctlZoneModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlZoneFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlZoneFormPanelHeader" />
</asp:Content>
