<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendFollowUpEmail.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.SendFollowUpEmail" EnableTheming="true"%>
<%@ Register src="~/Usercontrols/LOV/SCG.DB/UserProfileLookup.ascx" tagname="UserProfileLookup" tagprefix="uc1" %>
<asp:Panel ID="pnEmailSearch" runat="server" Width="550px" BackColor="White" Style="display:none">
<asp:UpdatePanel ID="ctlUpdatePanelEmailLog" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
        <table class="table"  border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:200px">
                <asp:Label ID="ctlToText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td  style="width:350px">
                <asp:Label ID="ctlTo" runat="server" SkinID="SkGeneralLabel" Width="200px"></asp:Label>
                <asp:Label ID="ctlUserID" runat="server" style="display:none"></asp:Label>
                <asp:Label ID="ctlRequestNo" runat="server" style="display:none"></asp:Label>
                <asp:Label ID="ctlCreatorID" runat="server" style="display:none"></asp:Label>
                <asp:Label ID="ctlEmailType" runat="server" style="display:none"></asp:Label>
                <asp:Label ID="ctlDocumentID" runat="server" style="display:none"></asp:Label>
                <asp:Label ID="ctlAdvanceDocumentID" runat="server" style="display:none"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlCCText" runat="server" Text="$CC$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="ctlCC" runat="server" SkinID="SkGeneralTextBox" 
                    TextMode="MultiLine" Height="100px" Width="300px" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                <asp:ImageButton ID="ctlAdd" runat="server" SkinID="SkCtlFormNewRow" onclick="ctlUserProfileLookup_Click"/>
                <uc1:UserProfileLookup ID="ctlUserProfileLookup" runat="server"/>
            </td>
        </tr>
        <tr>
           <td>
                <asp:Label ID="ctlRemarkText" runat="server" Text="$Remark$" SkinID="SkFieldCaptionLabel"></asp:Label>
           </td> 
           <td>
                <asp:TextBox ID="ctlRemark" runat="server" SkinID="SkGeneralTextBox" 
                    TextMode="MultiLine" Height="100px" Width="300px" MaxLength="100" onkeypress="return IsMaxLength(this, 100);" onkeyup="return IsMaxLength(this, 100);"></asp:TextBox>
           </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:ImageButton id="ctlSend" runat="server" ToolTip="Send" OnClick="ctlSend_Click" SkinID="SkSendButton"/>
                <asp:Label ID="ctlLine" runat="server" Text="|"></asp:Label>
			    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" /></td>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="color:Red;font:Tahoma;">
                <spring:ValidationSummary ID="ValidationSummarySendEmail" runat="server" Provider="SendMailComplete" />
            </td>
        </tr>
        </table>

    
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Panel> 
<asp:LinkButton ID="lnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkDummy"
    PopupControlID="pnEmailSearch" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    DropShadow="true" RepositionMode="None" />
