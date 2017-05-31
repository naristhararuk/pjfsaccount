<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageHeader.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.ManageHeader" EnableTheming="true"
    StylesheetTheme="Default" ValidateRequest="false" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdPanelMasterGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdProgressGrid" runat="server" AssociatedUpdatePanelID="ctlUpdPanelMasterGrid"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlNodeGridView" runat="server" AutoGenerateColumns="False"
                OnRequestData="RequestData1" ReadOnly="true" HeaderStyle-CssClass="GridHeader"
                EnableInsert="false" SelectedRowStyle-BackColor="#6699FF"
                InsertRowCount="1" DataKeyNames="Nodeid" CssClass="table" AllowPaging="true"
                AllowSorting="true" OnDataBound="ctlNodeGridView_DataBound" OnRowCommand="ctlNodeGridView_RowCommand"
                OnRequestCount="RequestCount1" Width="100%">
                <Columns>
 
                    <asp:TemplateField HeaderText="NodeType" SortExpression="n.NodeType">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlNodeType" runat="server" Text='<%# Eval("NodeType") %>' CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Wrap="false" Width="20%" />
                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="NodeOrderNo" SortExpression="n.NodeOrderNo">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlNodeOrderNo" runat="server" Text='<%# Eval("NodeOrderNo") %>' Style="padding-right: 5px;" CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" Width="15%" />
                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="n.Comment">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>' CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="n.Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActiveChkBox" runat="server" Checked='<%# Eval("Active") %>'
                                Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" Width="75px" />
                        <HeaderStyle HorizontalAlign="Center" Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="EditNode" ToolTip='<%# GetProgramMessage("EditNode") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="5%" />
                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="ctlUpdPanelContentGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server" />
            <asp:UpdateProgress ID="ctlUpdProgressContentGrid" runat="server" AssociatedUpdatePanelID="ctlUpdPanelContentGrid"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset id="ctlFieldSetDetailGridView" runat="server" style="width: 100%; text-align: center;">
                <legend id="ctlLegendDetailGridView" style="color: #4E9DDF;">
                    <asp:Label ID="lblDetailHeader" runat="server" Text="$Display Setting$"></asp:Label>
                </legend>
                <table border="0" cellpadding="0" cellspacing="0" width="98%">
                    <tr>
                        <td align="center">
                            <ss:BaseGridView ID="ctlContentGrid" runat="server" AutoGenerateColumns="false" CssClass="table"
                                ReadOnly="true" DataKeyNames="LanguageId, ContentId, NodeId" Width="100%" OnRowDataBound="ctlContentGrid_RowDataBound"
                                OnRowCommand="ctlContentGrid_RowCommand" HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Language">
                                        <ItemTemplate>
                                            <%# Eval("LanguageName")%></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header">
                                        <ItemTemplate>
                                            <%# Eval("Header") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="31%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" Width="31%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comment">
                                        <ItemTemplate>
                                            <%# Eval("Comment") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="75px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                CommandName="Select" ToolTip='<%# GetProgramMessage("EditNode") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </ss:BaseGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" ToolTip="Close"
                                OnClick="ctlCancel_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlNodeFormPanel" runat="server" CssClass="modalPopup" Style="display: none"
        Width="600px">
        <asp:Panel ID="ctlNodeFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlCapture" runat="server" SkinID="SkCtlLabel" Text="Manage Node Data"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpdatePanelNodeForm" runat="server" ChildrenAsTriggers="true"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div align="center" style="display: block;">
                    <asp:UpdateProgress ID="ctlUpdProgressForm" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelNodeForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table border="0" cellpadding="0" cellspacing="0" class="table">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlNodeForm" runat="server" DataKeyNames="NodeId" OnItemInserting="ctlNodeForm_ItemInserting"
                                    OnItemUpdating="ctlNodeForm_ItemUpdating" OnModeChanging="ctlNodeForm_ModeChanging"
                                    OnItemCommand="ctlNodeForm_ItemCommand" OnDataBound="ctlNodeForm_DataBound">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                  
                                                <td align="left">
                                                    <asp:TextBox Visible="false" ID="ctlNodeHeader"  MaxLength="4" runat="server" Text='<%# Eval("NodeHeaderid") %>' />
                                 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Ordinal")%>
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox  ID="ctlNodeOrderNo" MaxLength="4" runat="server" Text='<%# Eval("NodeOrderNo") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                   
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox Visible="false" ID="ctlNodeType" runat="server" Text='<%# Eval("NodeType") %>' MaxLength="20" />
                                                </td>
                                            </tr>
                                            <tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("Image") %> :</td>
												<td align="left" style="width:400px;">
													<asp:Image ID="ctlImage" runat="server" /><br />
												</td>
											</tr>
											<tr>
												<td align="right" style="width:200px;"><%# GetProgramMessage("NewImage") %> :</td>
												<td align="left" style="width:400px;">
													<asp:FileUpload ID="ctlImageFile" runat="server" Width="243px" SkinID="SkCtlFileUpload" />
												</td>
											</tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <%# GetProgramMessage("Description") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlNodeComment" SkinID="SkCtlTextboxMultiLine" runat="server" Text='<%# Eval("Comment") %>'
                                                        Width="240px" Height="50px" MaxLength="500" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);" />
                                                    <%--<asp:Label ID="lblTxtCommentReq" style="color:Red;" Text="*" runat="server"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlNodeActive" runat="server" Checked='<%# Eval ("Active") %>'
                                                        Style="padding: 0px;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                        SkinID="SkCtlFormSave" ToolTip='<%# GetProgramMessage("SaveToolTip") %>' ValidationGroup="ValidateFormView" />
                                                    <asp:ImageButton ID="ctlCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                        SkinID="SkCtlFormCancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                   
                                </asp:FormView>
                            </td>
                        </tr>
                        <tr>
                            <td>

                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender1" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlNodeFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlNodeFormPanelHeader" />
    <asp:Panel ID="ctlContentFormPanel" runat="server" CssClass="modalPopup" Style="display: none"
        Width="720" >
        <asp:Panel ID="ctlContentFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblContentHeader" runat="server" SkinID="SkCtlLabel" Text="Manage Content Data"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="ctlUpddatePanelContentForm" runat="server" ChildrenAsTriggers="true"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div align="center" style="display: block;">
                    <asp:UpdateProgress ID="ctlUpdateProgressContentForm" runat="server" AssociatedUpdatePanelID="ctlUpddatePanelContentForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading4"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table border="0" cellpadding="0" cellspacing="0" class="table" width="100%">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlContentForm" runat="server" DataKeyNames="ContentId" OnItemInserting="ctlContentForm_ItemInserting"
                                    OnItemUpdating="ctlContentForm_ItemUpdating" OnModeChanging="ctlContentForm_ModeChanging"
                                    OnItemCommand="ctlContentForm_ItemCommand" OnDataBound="ctlContentForm_DataBound">
                                    <EditItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right" style="width: 140px;">
                                                    <%# GetProgramMessage("HeaderText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:TextBox ID="ctlContentHeader" runat="server" MaxLength="1000" Text='<%# Eval("Header") %>'
                                                        Width="400px"></asp:TextBox>
                                                    <asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" style="width: 140px;">
                                                    <%# GetProgramMessage("ContentText") %>
                                                    :
                                                </td>
                                                <td align="left" valign="top">
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" style="padding-left: 3px;">
                                                                <FCKeditorV2:FCKeditor ID="ctlFCK" BasePath="~/fckeditor/" Width="600" Height="300" Value='<%# Eval("Content") %>'
                                                                    ToolbarSet="Custom" runat="server">
                                                                </FCKeditorV2:FCKeditor>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                &nbsp;<asp:Label ID="lblTxtBodyReq" style="color:Red; vertical-align:top;" Text="*" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" style="width: 140px;">
                                                    <%# GetProgramMessage("DescriptionText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:TextBox ID="ctlContentComment" runat="server" Text='<%# Eval("Comment") %>'
                                                        Width="600px" Height="50px" MaxLength="500" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 140px;">
                                                    <%# GetProgramMessage("ActiveText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlContentActive" runat="server" Checked='<%# Eval ("Active") %>'
                                                        Style="padding: 0px;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:ImageButton ID="ctlContentUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                        SkinID="SkCtlFormSave" ToolTip='<%# GetProgramMessage("SaveToolTip") %>' ValidationGroup="ValidateFormView" />
                                                    <asp:ImageButton ID="ctlContentCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                        SkinID="SkCtlFormCancel" ToolTip='<%# GetProgramMessage("CancelToolTip") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right" style="width: 140px;">
                                                    <%# GetProgramMessage("HeaderText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:TextBox ID="ctlContentHeader" runat="server" MaxLength="1000" Width="360px"></asp:TextBox>
                                                    <!--<asp:Label ID="lblTxtHeaderReq" style="color:Red;" Text="*" runat="server"></asp:Label>-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" style="width: 140px;">
                                                    <%# GetProgramMessage("ContentText") %>
                                                    :
                                                </td>
                                                <td align="left" valign="top">
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" style="padding-left: 3px;">
                                                                <FCKeditorV2:FCKeditor ID="ctlFCK" BasePath="~/fckeditor/" Width="480px" ToolbarSet="Custom"
                                                                    runat="server">
                                                                </FCKeditorV2:FCKeditor>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                &nbsp;<!--<asp:Label ID="lblTxtBodyReq" style="color:Red; vertical-align:top;" Text="" runat="server"></asp:Label>-->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" style="width: 140px;">
                                                    <%# GetProgramMessage("DescriptionText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:TextBox ID="ctlContentComment" runat="server" Text='<%# Eval("Comment") %>'
                                                        Width="360px" Height="50px" MaxLength="500" onkeypress="return IsMaxLength(this, 500);"
                                                        onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 140px;">
                                                    <%# GetProgramMessage("ActiveText") %>
                                                    :
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:CheckBox ID="ctlContentActive" runat="server" Checked="true" Style="padding: 0px;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                   <%-- <asp:ImageButton ID="ctlInsert" runat="server" CommandName="Insert" SkinID="SkCtlFormSave"
                                                        ToolTip='<%# GetProgramMessage("SaveToolTip") %>' />
                                                    <asp:ImageButton ID="ctlCancel" runat="server" CommandName="Cancel" SkinID="SkCtlFormCancel"
                                                        ToolTip='<%# GetProgramMessage("CancelToolTip") %>' />--%>
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
                                    <spring:ValidationSummary ID="ctlValidationSummaryContent" runat="server" Provider="Content.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender2" runat="server" TargetControlID="lnkDummy2"
        PopupControlID="ctlContentFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy2"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlContentFormPanelHeader" />
                                        <font color="red" style="text-align: left">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Node.Error" />
                                </font>
</asp:Content>
