<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="SCG.eAccounting.Web.SignIn" EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Src="~/UserControls/LOV/SS.DB/AnnouncementInfo.ascx" TagName="AnnouncementInfo" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        var DialogObjID;
        var DialogObjhref;
        var Dialogresutlt;
        var Dialogobj1;
        var aaa;

        function OK() {
            document.getElementById('ctl_Password_Textbox').focus();
        }

function enter()
{
if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {

    document.getElementById('ctl_SignIn').click();
        return false;
    }
    else
    {
     return true;
    }

}
function showNew(obj) {
    window.open("ShowDoc.aspx", "window");
   
}
</script>
</head>
<body>


  <div align="center">
    <form id="FormLogin" runat="server" >    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

   
        
    <asp:Panel ID="PanelASP" runat="server" Style="display: none" CssClass="modalPopup">
                <table width="100%" border="0">
                    <tr>
                        <td width="92%">
                            <asp:Label ID="DialogHeader" runat="server" class="modalHeader" ></asp:Label>
                        </td>
                        <td width="8%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="DialogTopic" runat="server" CssClass="modalTopic"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:Label ID="ctl_ErrorMessage_Label" runat="server" Visible="false" CssClass="modalMsg"></asp:Label><br />
                           
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            <div id="divSolution" runat="server">
                                <asp:Label ID="DialogSolution" runat="server" Width="100%" CssClass="modalSolution" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <div id="divOk" runat="server">
                                <asp:Button ID="DialogOkButton" runat="server" Text="$OK$" Width="80px" OnClientClick="OK()" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
         
    <asp:Panel ID="Panel2" runat="server" SkinID="SkLogin" >
       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
      
       
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" 
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <div ID="ProgressPanel" class="ProgressOverlay" runat="server">
                        <div class="ProgressContainer">
                            <div class="ProgressHeader"></div>
                            <div class="ProgressBody">
                                  <asp:ImageButton ID="Image1" runat="server" SkinID="SkProgress" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>   
        <asp:Table ID="Table1" runat="server">
            <asp:TableHeaderRow></asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle"><asp:Label ID="ctl_UserName_Label" runat="server" Text="$UserName$" CssClass="loginLabel" style="vertical-align:middle; text-align:right;"></asp:Label>&nbsp;&nbsp;</asp:TableCell>
                    <asp:TableCell><asp:TextBox ID="ctl_UserName_Textbox" runat="server" Text="admin" CssClass="loginTextbox"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle"><asp:Label ID="ctl_Password_Label" runat="server" Text="$Password$"  CssClass="loginLabel" style="vertical-align:middle; text-align:right;"></asp:Label>&nbsp;&nbsp;</asp:TableCell>
                    <asp:TableCell><asp:TextBox ID="ctl_Password_Textbox" runat="server" TextMode="Password" Text="admin" CssClass="loginTextbox" ></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                    <asp:TableCell><asp:ImageButton ID="ctl_SignIn" runat="server"  SkinID="SkCtlSignIn" ToolTip="$ToolTipCtlSignIn$" onclick="ctl_SignIn_Click"  /><asp:ImageButton ID="ctl_Cancel" runat="server" SkinID="SkCtlCancel" ToolTip="$ToolTipCtlCancel$" onclick="ctl_Cancel_Click" /></asp:TableCell>
                </asp:TableRow>
            <asp:TableFooterRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" ForeColor="Red">
                </asp:TableCell>
            </asp:TableFooterRow>
      
        </asp:Table>
                 </ContentTemplate> 
       <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ctl_SignIn" EventName="Click" />
       </Triggers>
         </asp:UpdatePanel> 
        <br /><br />
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table>
            <tr><td><asp:Label ID="Label1" runat="server" Text="ประกาศ" Font-Bold="true"  Font-Size="Larger" ForeColor="#9966ff"></asp:Label></td></tr>
            <tr><td><uc1:AnnouncementInfo ID="AnnouncementInfo1" runat="server" zIndex="10002" /></td></tr>
        </table>
       
        <%--<asp:Table ID="Table2" runat="server">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
              <asp:Label ID="Label1" runat="server" Text="ประกาศ" Font-Bold="true"  Font-Size="Larger" ForeColor="#9966ff"></asp:Label><br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
          
            <asp:TableCell HorizontalAlign="Left">
                <asp:Label ID="Label2" runat="server" Text="วันหยุดปีใหม่ และงานประจำปี"></asp:Label> <asp:ImageButton ID="ImageButton1" ImageUrl="~/App_Themes/Default/images/icon/construction_cone_48.png" Width="30" Height="30" OnClientClick="showNew()" ToolTip="วันหยุดปีใหม่ และงานประจำปี" runat="server" />
            <br />
                <asp:Label ID="Label3" runat="server" Text="ประกาศรายชื่อผู้โชคดี พักผ่อนที่ญี่ปุ่น"></asp:Label> <asp:ImageButton ID="ImageButton2" ImageUrl="~/App_Themes/Default/images/icon/Futon (bed).png" Width="30" Height="30" runat="server" ToolTip="ประกาศรายชื่อผู้โชคดี พักผ่อนที่ญี่ปุ่น" />
            <br />
                <asp:Label ID="Label4" runat="server" Text="สัมนา วิชาการ ว่าด้วยเรื่องกฏหมายกับธุรกิจซอฟแวร์"></asp:Label> <asp:ImageButton ID="ImageButton3" ImageUrl="~/App_Themes/Default/images/icon/bluewhales 075.png" Width="30" Height="30" runat="server"  ToolTip="สัมนา วิชาการ ว่าด้วยเรื่องกฏหมายกับธุรกิจซอฟแวร์" />
            <br />
            </asp:TableCell>s
        </asp:TableRow>
        </asp:Table>--%>
        
         </ContentTemplate>
         <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButton1" EventName="Click" />
         </Triggers>--%>
        </asp:UpdatePanel>
    </asp:Panel>
    
    <asp:Panel ID="PanelModal" runat="server" Style="display: none" >
                <ajaxtoolkit:ModalPopupExtender ID="ModalPopupExtenderMsg" runat="server" BehaviorID="programmaticModalPopupBehavior"
                    TargetControlID="PanelModal" PopupControlID="PanelASP" 
                    OkControlID="DialogOkButton" BackgroundCssClass="modalBackground"
                    DropShadow="true"   />
               
            </asp:Panel>
            
  
       
      
                  
       
        
  
    </form>
    </div>
  
    </body>
</html>
