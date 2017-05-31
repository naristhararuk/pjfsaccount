<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ViewPostEditDate.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ViewPost.ViewPostEditDate" 
    EnableTheming ="true"
%>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>

<asp:Panel ID="pnViewPostShow1" runat="server" Width="400px" BackColor="White" Style="display: none">
    
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnViewPostShowHeader1" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;cursor:move;">
            <asp:Label ID="lblSpace1" runat="server" Text='&nbsp;&nbsp;Change Posting Date for Reverse ...'></asp:Label>
            </asp:Panel>
        </td>
    </tr>
    </table>
    
	<asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	    
	    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelSearchAccount"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <table width="400px">
            <tr align="center">
                <td>
                   <table width="100%">
                   <tr>
                        <td style="width:50%" align="right">
                            <asp:Label ID="ctlLblPostingDateForReverse" SkinID="SkFieldCaptionLabel" runat="server" Text="Posting Date for Reverse"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="width:50%" align="left">
                            <uc1:Calendar ID="ctlPostingDateForReverse" runat="server"/>
                        </td>
                   </tr>
                   </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnOK"       runat="server" Text="OK"     OnClick="btnOK_Click"    Width="100px"/>
                    <asp:Button ID="btnCancel"   runat="server" Text="Close"     OnClick="btnCancel_Click"    Width="100px"/>
                </td>
            </tr>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummy1" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1ShowMessage" runat="server" 
	TargetControlID="lnkDummy1"
	PopupControlID="pnViewPostShow1"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy1"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="pnViewPostShowHeader1"
	zIndex="10002"
	/>
