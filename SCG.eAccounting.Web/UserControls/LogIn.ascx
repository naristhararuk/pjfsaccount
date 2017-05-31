<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="LogIn.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.LogIn" 
%>

<%--<%@ Register Src="ForgetPassword.ascx" TagName="ForgetPassword" TagPrefix="uc1" %>--%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%--<%@ Register src="ChangePassword.ascx" tagname="ChangePassword" tagprefix="uc2" %>--%>

<script type="text/javascript">
    var DialogObjID;
    var DialogObjhref;
    var Dialogresutlt;
    var Dialogobj1;
    var aaa;

    function OK() {
        document.getElementById('ctl_Password_Textbox').focus();
    }

    function enter() {
    	if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) 
        {
        	var loginButton = document.getElementById('<%= imgLogin.ClientID %>');
        	if (loginButton != null) 
        	{
        		loginButton.click();
        		return false;
        	}
        }

        return true;
    }
    function showNew(obj) {
        window.open("ShowDoc.aspx", "window");

    }
</script>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td valign="middle">
            <asp:Label ID="ctlLblUserId" runat="server" SkinID="SkGeneralLabel" Text='$User Id :$'></asp:Label>&nbsp;
        </td>
        <td>
            <asp:TextBox ID="ctlUserName" runat="server" SkinID="SkGeneralTextBox" Style="border-style: dashed;" MaxLength="20"></asp:TextBox>&nbsp;&nbsp;
        </td>
        <td valign="middle">
            <asp:Label ID="ctlLblPassword" runat="server" SkinID="SkGeneralLabel" Text='$Password :$'></asp:Label>&nbsp;
        </td>
        <td>
            <asp:TextBox ID="ctlPassword" runat="server" SkinID="SkGeneralTextBox" Style="border-style: dashed;" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>&nbsp;
            <asp:ImageButton ID="imgLogin" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="btnLogIn" OnClick="imgLogin_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="forgetPassword" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="btnForgetPassword"
                OnClick="forgetPassword_Click" />
            &nbsp;

        </td>
    </tr>
</table>

<%--<uc1:ForgetPassword ID="ctlChangePassword" runat="server" />--%>
<%--<uc2:ChangePassword ID="ChangePassword" runat="server" />--%>


<asp:Panel ID="pnViewPostShow1" runat="server" Width="400px" BackColor="White" Style="display: none">
    
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnViewPostShowHeader1" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;cursor:move;">
            <asp:Label ID="lblSpace1" runat="server" Text='&nbsp;&nbsp;LOG IN ...'></asp:Label>
            </asp:Panel>
        </td>
        <td align="right" valign="top" >
            <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="imgClose_Click" />
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
                   <asp:Label ID="ctlErrorValidationLabel" SkinID="SkGeneralLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose"       runat="server" Text="Close"     OnClick="btnClose_Click"    Width="100px"/>
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
	/>
	
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>

