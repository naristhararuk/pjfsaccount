<%@ Control Language="C#" 
AutoEventWireup="true" 
CodeBehind="ForgetPassword.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.ForgetPassword" 
EnableTheming="true" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc1" %>

<style type="text/css">
    .style1
    {
        width: 31%;
    }
</style>

<asp:Panel ID="pnChangePassword" runat="server" Width="400px" BackColor="White" Style="display:none" >
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnChangePasswordHeader" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;cursor:move;">
            <asp:Label ID="lblSpace1" runat="server" Text="$ForgetPassword$"></asp:Label>
            </asp:Panel>
        </td>
        <td align="right" valign="top" >
            <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="imgClose_Click" />
        </td>
    </tr>
    </table>
            
    <asp:UpdatePanel ID="UpdatePanelChangePassword" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="UpdatePanelChangePasswordProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelChangePassword"
                DynamicLayout="true" EnableViewState="true">
                <ProgressTemplate>
                    <uc1:SCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <fieldset id="Fieldset1" style="width: 90%" class="table">
            
            <table>
            <tr>
                <td align="right" style="width: 20%">
                    <asp:Label ID="ctlUserNameCodeLabel" runat="server" Text="$EnterUserId$"></asp:Label>
                </td>
                <td align="left" class="style1">
                    <asp:TextBox ID="ctlUserName" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="ctlConfirm" runat="server" Text="$Submit$" OnClick="ctlConfirm_Click" Width="100px"/>
                    <asp:Button ID="ctlCancel" runat="server" Text="$Cancel$" OnClick="ctlCancel_Click"  Width="100px"/>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <font color="red">
                        <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="User.Error">
                        </spring:ValidationSummary>
                    </font>
                </td>
            </tr>
            </table>
                
            </fieldset>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender 
    ID="ModalPopupExtender1" 
    runat="server" 
    TargetControlID="lnkDummy"
    PopupControlID="pnChangePassword" 
    BackgroundCssClass="modalBackground" 
    CancelControlID="lnkDummy"
    DropShadow="true" 
    RepositionMode="None" 
    PopupDragHandleControlID="pnChangePasswordHeader"
    />
    
    <asp:Panel ID="pnMsg" runat="server" Width="400px" BackColor="White" Style="display:none" >
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnMsgHeader" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;cursor:move;">
            <asp:Label ID="lblSpace2" runat="server" Text="$ForgetPassword$"></asp:Label>
            </asp:Panel>
        </td>
        <td align="right" valign="top" >
            <asp:ImageButton ID="imgClose1" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="imgClose1_Click" />
        </td>
    </tr>
    </table>
            
    <fieldset id="Fieldset2" style="width: 90%" class="table">
    
    <table width="100%">
    <tr>
        <td align="center" style="width: 30%">
            <asp:Image ID="Image1" ImageUrl="~/App_Themes/Default/images/true.png" runat="server" />
        </td>
        <td align="left" class="style1" style="width: 70%">
            <asp:Label ID="lblMsg" runat="server" Text="$ForgetPasswordIsComplete$"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="2" style="width: 100%">
            <asp:Button ID="ctlBtnClose" runat="server" Text="$Close$" OnClick="ctlBtnClose_Click"  Width="100px"/>
        </td>
    </tr>
    </table>
        
    </fieldset>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummyMsg" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender 
    ID="ModalPopupMsg" 
    runat="server" 
    TargetControlID="lnkDummyMsg"
    PopupControlID="pnMsg" 
    BackgroundCssClass="modalBackground" 
    CancelControlID="lnkDummyMsg"
    DropShadow="true" 
    RepositionMode="None" 
    PopupDragHandleControlID="pnMsgHeader"
    />

