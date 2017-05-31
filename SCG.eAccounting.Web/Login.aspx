<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/LoginPage.Master"  CodeBehind="Login.aspx.cs" Inherits="SCG.eAccounting.Web.Login" EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register Src="~/UserControls/ServiceShowCase.ascx" TagName="ServiceShowCase" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOV/SS.DB/AnnouncementInfo2.ascx" TagName="AnnouncementInfo2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/WelcomeShow.ascx" TagName="WelcomeShow" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ForgetPassword.ascx" TagName="ForgetPassword" TagPrefix="uc4" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%@ Register Src="UserControls/PanelLogin.ascx" TagName="PanelLogin" TagPrefix="uc4" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolderWelcomeMsg">
    <uc4:PanelLogin ID="PanelLogin" runat="server" />
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="ContentPlaceHolderService">

<asp:UpdatePanel ID="updPanelShow" runat="server" UpdateMode="Conditional">
<ContentTemplate>

    <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelShow"
        DynamicLayout="true" EnableViewState="False">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
            
    
    <table border="0" cellpadding="0" cellspacing="0">
    <tr valign="top">
        <td>
        
            <div id="dvWelcomeMsg" runat="server">
            <uc3:WelcomeShow ID="WelcomeShow2" runat="server"/> 
            <br />
            <uc2:AnnouncementInfo2 ID="AnnouncementInfo2" runat="server"/>
            </div>
            
            <div id="dvContent" runat="server">
            <table style="background-image:url(App_Themes/Default/images/Welcome.png);width:628px;height:500px;vertical-align:top;background-repeat:no-repeat;">
            <tr>
                <td style="width:3%">&nbsp;</td>
                <td align="Left" valign="top"><br /><br />
                    <table style="width:50%;font-family:Tahoma;vertical-align:top;">
                    <tr>
                        <td colspan="2" style="font-family:Tahoma;color:#74E6FE;font-size:50px;font-size:larger;font-weight:bold;vertical-align:top">
                            <asp:Label ID="ctlHeader" runat="server" ></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                             <br />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label id="ctlContent" Style="white-space:normal;text-align:left;color:White;font-size:small;" Width="500px" runat="server" ></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    </table>
                </td>
            </tr>
            </table>
            </div>
    
        </td>
        <td>
            <br />
            <uc1:ServiceShowCase ID="ServiceShowCase1" runat="server"/>
        </td>
    </tr>
    </table>

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
