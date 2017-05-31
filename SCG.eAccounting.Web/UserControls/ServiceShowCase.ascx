<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="ServiceShowCase.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.ServiceShowCase" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Panel ID="PanelServiceMsg" runat="server" CssClass="backgroundPanelServiceMsg">
<table border="0" style="width:308px;font-family:Tahoma;" cellspacing="0" cellpadding="0" class="backGroundService" >
<tr align="left">
    <td style="vertical-align:top;" align="center">
        <br />
        <table border="0" style="width:280px;">
        <tr >
            <td align="left">
            
                <asp:UpdatePanel ID="updPanelContent" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                
                <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelContent" DynamicLayout="true" EnableViewState="False">
                    <ProgressTemplate>
                        <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                 
                <table border="0" style="width:280px;">
                <tr >
                    <td align="left">
                    <asp:Label ID="ctlNode1" Font-Bold="true" runat="server"></asp:Label>
                    <asp:Image ID="ctlImageLine1" ImageUrl="~/App_Themes/Default/images/empty.gif" runat="server" CssClass="lineService"/>
                    <asp:Label ID="ctlContent1" Font-Names="Tahoma" Font-Size="Small" runat="server" ></asp:Label>
                   </td>
                </tr>
                </table>
                <br />
                
                </ContentTemplate>
                </asp:UpdatePanel>
                       
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true" EnableViewState="False">
                    <ProgressTemplate>
                        <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    
                <asp:Repeater ID="rptContent" runat="server" OnItemDataBound="rptContent_ItemDataBound" OnItemCommand="rptContent_ItemCommand">
                <ItemTemplate>
                    <table width="270px" style="font-size:small" class="ServiceRow" cellpadding="0" cellspacing="0">
                    <tr align="left">
                        <td width="60px" height="45px" valign="middle" align="center">
                            <asp:Image ID="ctlImage" runat="server" />&nbsp;&nbsp;&nbsp;
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label  ID="ctlContentID" runat="server" Text='<%# Eval("NodeId")%>' style="display:none;"></asp:Label>
                            <asp:LinkButton ID="ctlContent" Font-Underline="false" CommandName="ShowService" runat="server" Text='<%# Eval("Header")%>' />
                        </td>
                    </tr>
                    </table>
                </ItemTemplate>
                </asp:Repeater>
                <br />
                
                </ContentTemplate>
                </asp:UpdatePanel>
            
            </td>
        </tr>
        </table>
        
    </td>
</tr>
</table>
    
</asp:Panel>