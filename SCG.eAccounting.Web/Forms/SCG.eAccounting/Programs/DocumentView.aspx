<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="DocumentView.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.DocumentView"
    EnableTheming="true" StylesheetTheme="Default" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>
<%@ Register Src="~/UserControls/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%--<%@ Register Src="~/UserControls/WorkFlow/AdvanceWorkFlowMonitor.ascx" TagName="AdvanceWorkFlowMonitor"
    TagPrefix="uc2" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<asp:HiddenField ID="ctlMsgForceLock" Value="$MsgForceLock$"  runat="server" />
<asp:HiddenField ID
="ctlMsgUnLock" Value="$MsgUnLock$" runat="server" />
<asp:HiddenField ID="ctlMsgLockByOther" Value="$MsgLockByOther$" runat="server" />
<asp:HiddenField ID="ctlMsgShowDocumentViewLock" Value="$ctlMsgShowDocumentViewLock$" runat="server" />
<asp:HiddenField ID="ctlMsgDocumentAvilable" Value="$ctlMsgDocumentAvilable$" runat="server" />
    <table width="100%" border="0">
        <tr>
            <td colspan="2">
                <asp:Panel ID="ctlWorkFlowMonitorPanel" runat="server" />
                <%--<uc2:AdvanceWorkFlowMonitor ID="AdvanceWorkFlowMonitor" runat="server" />
                </asp:Panel>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="ctlDocumentEditorPanel" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 30%">
                            <asp:UpdatePanel ID="ctlDocumentEventUpdatePanel" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="UpdateAuthurizedEventProgress" runat="server" AssociatedUpdatePanelID="ctlDocumentEventUpdatePanel"
                                        DynamicLayout="false" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:Panel ID="ctlDocumentEventPanel" runat="server">
                                        <asp:RadioButtonList ID="ctlAuthurizedEvent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ctlAuthurizedEvent_SelectedIndexChanged"
                                            SkinID="SkCtlAuthorizedEventList">
                                        </asp:RadioButtonList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 30px">
                        </td>
                        <td>
                            <asp:UpdatePanel ID="ctlDocumentEventUserControlUpdatePanel" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="ctlDocumentEventUserControlPanel" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:UpdatePanel ID="ctlToolBarUpdatePanel" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlToolBarUpdatePanel"
                            DynamicLayout="false" EnableViewState="true">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div id="ctlDivReadOnlyButton" runat="server">
                            <div id="ctlDivShowDocumentViewLock" style="text-align:center" >
                            <table>
                                <tr>
                                    <td colspan="4">
                                        <asp:CheckBox ID="ctlDocumentViewLock" runat="server" SkinID="SkGeneralCheckBox" Text="Lock this document" AutoPostBack="True" oncheckedchanged="ctlDocumentViewLock_CheckedChanged" style="display:none" />
                                       <br /> <asp:Label ID="ctlShowDocumentViewLock" SkinID="SkGeneralLabel" ForeColor="Red" runat="server" Text="" style="display:none" ></asp:Label> 
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <table>
                                
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkSubmitButton" ToolTip="Submit"
                                            OnClick="ctlSubmit_Click" />
                                    </td>
                                    <%--<td valign="middle"> | </td>--%>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlPrint" runat="server" SkinID="SkPrintButton" ToolTip="Print"
                                            OnClick="ctlPrint_Click" />
                                    </td>
                                    <%--<td valign="middle"> | </td>--%>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" ToolTip="Delete"
                                            OnClick="ctlDelete_Click" OnClientClick="return confirm('Are you sure to delete this document?');"/>
                                    </td>
                                    <%--<td valign="middle"> | </td>--%>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" ToolTip="Edit"
                                            OnClick="ctlEdit_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ctlCopy" runat="server" SkinID="SkCopyButton" ToolTip="Copy"
                                            OnClick="ctlCopy_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ctlPrintPayIn" runat="server" SkinID="SkPrintPayInButton" ToolTip="Print Pay In"
                                            OnClick="ctlPrintPayIn_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="ctlDivReadWriteButton" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlSave" runat="server" SkinID="SkSaveButton" ToolTip="Save"
                                            OnClick="ctlSave_Click" />
                                    </td>
                                    <td valign="middle">
                                        |
                                    </td>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCancelButton" ToolTip="Cancel"
                                            OnClick="ctlCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table border="0" width="400px;">
                            <tr>
                                <td align="left" style="color: Red; font-family: Tahoma; font-size: 8pt;">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                                    </spring:ValidationSummary>
                                    <spring:ValidationSummary ID="ctlWorkFlowValidationSummary" runat="server" Provider="WorkFlow.Error">
                                    </spring:ValidationSummary>
                                    <spring:ValidationSummary ID="ctlEmailValidationSummary" runat="server" Provider="Email.Error">
                                    </spring:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    
        <!-- Confirm Popup Document Locked  -->
    
    <asp:Panel ID="ctlPnConfirmDocumentLocked" runat="server" Width="600px" style="display:none;" BackColor="White">

    <asp:Panel ID="ctlPnConfirmDocumentLockedHeader" CssClass="table" runat="server" Style="border:solid 1px Gray;color:Black">
	<div>
	    <table width="100%">
	    <tr>
            <td align="left" valign="top" width="100%">
                <div style="cursor:move;width:100%">
                    <asp:Label ID="lblDocViewLockCapture" SkinID="SkGeneralLabel" runat="server" Text='$ConfirmationDocumentLocked$' />
                </div>
            </td>
            <td align="right" valign="top" >
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="DoClose"
                />
            </td>
        </tr>
	    </table>
	</div>
    </asp:Panel>
	    
    <asp:UpdatePanel ID="UpdatePanelConfirmDocumentLocked" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
    <asp:UpdateProgress ID="UpdatePanelConfirmDocumentLockedProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelConfirmDocumentLocked"
        DynamicLayout="false" EnableViewState="true">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress> 
                
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Label ForeColor="Red" SkinID="SkGeneralLabel" ID="ctlWarningText" runat="server" Text=""></asp:Label><br />
                <%--<asp:Label ForeColor="Red" SkinID="SkGeneralLabel" ID="ctlWarningText1" runat="server" Text=""></asp:Label>--%>
            </td>
       
        </tr>
      <tr>
        <td>
            &nbsp;
        </td>
      </tr>
        <tr>
            <td align="center">
                <asp:Button ID="ctlDoLock" runat="server" SkinID="SkGeneralButton" Text="$ForceLock$" OnClick="DoLock"  />
                <asp:Button ID="ctlDoCancel" runat="server" SkinID="SkGeneralButton" Text="$Cancel$" OnClick="DoCancel" />
            </td>
        </tr>
       
    </table>
    
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ctlDoLock" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ctlDoCancel" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<asp:LinkButton ID="LinkButtonConfirmLocked" runat="server" style="visibility:hidden"/>

<AjaxToolkit:ModalPopupExtender ID="ctlDocumentLockedPopupExtender" runat="server" 
    RepositionMode="None"
    DropShadow="true" 
    TargetControlID="LinkButtonConfirmLocked"
    BackgroundCssClass="modalBackground"
	CancelControlID="ImageButton1"
    PopupControlID="ctlPnConfirmDocumentLocked"
    PopupDragHandleControlID="ctlPnConfirmDocumentLockedHeader" />
    
    <!-- Confirm Popup Document Locked  -->
</asp:Content>
