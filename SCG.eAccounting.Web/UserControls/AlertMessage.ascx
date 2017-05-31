<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertMessage.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.AlertMessage" EnableTheming="true"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

 <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
     <asp:Panel ID="PanelShowMessage" runat="server" Width="450px" Height="350px"  BackColor="White">
     <table style="width:100%;height:100%;margin-top: 0px;" border="2" >
       
            <tr>
                <td align="left" valign="top"  style="height:83px;" >
                <div style="float:left"> <asp:Image runat="server" ID="ctlMsgTypeImage" SkinID="SkMsgType" 
                        ImageUrl="~/App_Themes/Default/images/MsgConfirmation.gif" /></div>
               <div style="float:left"><asp:Label ID="ctlMsgHeaderLabel" runat="server" /></div>
                    
                </td>
                <td align="right" valign="top" class="style1">
                 <asp:ImageButton ID="ctlMsgCloseImageButton" runat="server"  ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif"
                        onclick="ctlMsgCloseImageButton_Click" />    
                </td>
            </tr>
       
            <tr>
                <td colspan="2" >
                
                    <table style="width:100%;height:100%; margin-top: 0px;" >
                        <tr>
                            <td>
                                <asp:Label ID="ctlMsgTopicLabel" runat="server" ></asp:Label>
                            </td>
                        </tr>
                          <tr>
                            <td>
                            
                                <asp:Label ID="ctlMsgBodyLabel" runat="server" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                
                </td>
            </tr>
       
            <tr>
                <td colspan="2" align="center">
                 
                    <asp:ImageButton ID="ctlMsgOKImageButton"  runat="server"  ImageUrl="~/App_Themes/Default/images/Button/ok.gif"
                        onclick="ctlMsgOKImageButton_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ctlMsgCancelImageButton" runat="server"  ImageUrl="~/App_Themes/Default/images/Button/cancel.gif"
                        onclick="ctlMsgCancelImageButton_Click" 
                         />
                </td>
            </tr>
      
     </table>
 </asp:Panel>
          <asp:Panel ID="PanelMsgExtender" runat="server">
          </asp:Panel>
              <AjaxToolkit:ModalPopupExtender ID="ctlPanelModalPopupExtender" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true"
                  DynamicServicePath="" BackgroundCssClass="modalBackground" Enabled="True" TargetControlID="PanelMsgExtender" PopupControlID="PanelShowMessage">
              </AjaxToolkit:ModalPopupExtender>
          
            
        </ContentTemplate>
    </asp:UpdatePanel>
