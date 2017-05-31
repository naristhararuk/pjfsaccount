<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="UserInfo.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.UserInfo" 
    EnableTheming ="true"
%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<style type="text/css">
    .style1
    {
        width: 119px;
    }
</style>

<table style="vertical-align:middle" border="0">
    <tr style="vertical-align:middle">
    <td style="vertical-align:middle">
        <asp:Image ID="ctl_UserInfo_Image" runat="server" CssClass="icon" ImageUrl="~/App_Themes/Default/images/icon/iui.gif" />
        <asp:Label ID="ctlWelcomeText" Height="6" runat="server" SkinID="SkCtlUserInfo"></asp:Label> 
        <asp:Label ID="ctl_Loged_FullName_Label" Height="6" runat="server" SkinID="SkCtlUserInfo"></asp:Label>
    </td>
    <td>&nbsp;</td>
    <td style="vertical-align:middle"><asp:LinkButton ID="ctlButtonChangePassword" runat="server" Text="Change Password" onclick="ctlButtonChangePassword_Click"></asp:LinkButton></td>
    <td>&nbsp;</td>
    <td style="vertical-align:middle"><asp:LinkButton ID="ctlLingButtonSignOut" runat="server" onclick="ctlLingButtonSignOut_Click">Logout</asp:LinkButton></td>
    <td>&nbsp;</td>
    <td style="vertical-align:middle"><asp:Image ID="ctlFlagImage" runat="server" ImageUrl="~/App_Themes/Default/images/eng.gif"  /> </td>
    <td>&nbsp;</td>
    <td style="vertical-align:middle"><asp:DropDownList ID="ctlDropDownListChangeLanguage" runat="server" AutoPostBack="true"   onselectedindexchanged="ctlDropDownListChangeLanguage_SelectedIndexChanged"></asp:DropDownList></td>
    </tr>
</table>


 <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
     <asp:Panel ID="PanelChangepassword" runat="server" Width="450px" style="display:none" BackColor="White">
     <table width="100%">
        <tr>
            <td class="style1">
            &nbsp;
            </td>
           <td align="right" valign="top">
                 <asp:ImageButton ID="ctlMsgCloseImageButton" runat="server"  ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif"
                        onclick="ctlMsgCloseImageButton_Click" />    
                </td>
            
        </tr>
        <tr>
            <td class="style1" align="right"> <asp:Label ID="Label1" runat="server" Text="Old Password"></asp:Label></td>
            <td align="left">
                <asp:TextBox ID="ctlOldPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
                    <td class="style1" align="right"><asp:Label ID="Label2" runat="server" Text="New Password"></asp:Label></td>
                      <td align="left">
                <asp:TextBox ID="ctlNewPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
            </td>

        </tr>
        <tr>
                    <td class="style1" align="right"><asp:Label ID="Label3" runat="server" Text="Confirm Password"></asp:Label></td>
                      <td align="left">
                <asp:TextBox ID="ctlConfirmPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
            </td>

        </tr>
        <tr >
            <td  colspan="2" align="center">
                <asp:Button ID="ctlUpdate" runat="server" Text="Save" onclick="ctlUpdate_Click" />
               
                
            </td>
            
        </tr>
         <tr >
            <td  colspan="2" align="center" style="color:Red;">
                <spring:ValidationSummary ID="vsSummary" runat="server" Provider="ChangePassword.Error"></spring:ValidationSummary>
                <%--<asp:Label ID="ctlErrorValidationLabel" runat="server" Text="" ForeColor="Red"></asp:Label>--%>
            </td>
        </tr>
     </table>
 </asp:Panel>
          <asp:Panel ID="PanelChangepasswordExtender" runat="server">
          </asp:Panel>
              <AjaxToolkit:ModalPopupExtender ID="ctlPanelModalPopupExtender" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true"
                  DynamicServicePath="" BackgroundCssClass="modalBackground" Enabled="True" TargetControlID="PanelChangepasswordExtender" PopupControlID="PanelChangepassword">
              </AjaxToolkit:ModalPopupExtender>
          
            
        </ContentTemplate>
    </asp:UpdatePanel>