<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="UserInfoWelcome.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.UserInfoWelcome" 
    EnableTheming="true" 
%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%--<%@ Register src="ChangePassword.ascx" tagname="ChangePassword" tagprefix="uc2" %>--%>

<table>
    <tr>
        <td valign="bottom">
            <asp:Image ID="ctl_UserInfo_Image" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="iconUser" Width="35px" Height="35px" />
        </td>
        <td>
            <table border="0">
                <tr>
                    <td>
                        <asp:Label ID="ctlWelcomeText" runat="server" SkinID="SkCtlUserInfo"> </asp:Label>
                        &nbsp;
                        <asp:Label ID="ctl_Loged_FullName_Label" runat="server" SkinID="SkCtlUserInfo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ctlCompanyText" runat="server" SkinID="SkCtlUserInfo"> </asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
        <td>
            <table border="0">
            <tr>
                <td>
                    <asp:LinkButton ID="ctlLingButtonSignOut" runat="server" OnClick="ctlLingButtonSignOut_Click" SkinID="SkCtlUserInfoLink" Text="Logout"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="ctlButtonChangePassword" runat="server" Text="Change Password" OnClick="ctlButtonChangePassword_Click" SkinID="SkCtlUserInfoLink"></asp:LinkButton>
                </td>
            </tr>
            </table>
        </td>
        <td>&nbsp;</td>
       <%-- <td>
            <table border="0">
                <tr>
                    <td>
                        <asp:LinkButton ID="ctlArchiveButton" runat="server" OnClick="ctlArchiveButton_Click" Text="Archive" SkinID="SkCtlUserInfoLink"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="ctlBackButton" runat="server" OnClick="ctlBackButton_Click" Text="Back To e-Xpense" SkinID="SkCtlUserInfoLink"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>--%>
        <td>&nbsp;</td>
    </tr>
</table>

<%--<uc2:ChangePassword ID="ChangePassword" runat="server" />--%>

<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>