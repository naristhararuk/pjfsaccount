<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MenuBottom.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.MenuBottom" 
    EnableTheming ="true"
%>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:UpdatePanel ID="updPanelContent" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelContent" DynamicLayout="true" EnableViewState="False">
    <ProgressTemplate>
        <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
    </ProgressTemplate>
</asp:UpdateProgress>
                
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin:0;padding:0;">
<tr>
    <td>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin:0;padding:0;" >
        <tr>
            <td align="left" style="width:12px"><div id="imgFooterLeft" runat="server" class="FooterLeft"></div></td>
            <td class="FooterLine" align="left" valign="middle">
                
                <table border="0" cellpadding="0" cellspacing="0" style="margin:0;padding:0;">
                <tr align="left">
                    <asp:Repeater ID="rptMainMenu" runat="server" OnItemCommand="rptMainMenu_ItemCommand">
                    <ItemTemplate>
                    
                    <td valign="middle" align="left">
                        <asp:Label  ID="ctlContentID" runat="server" Text='<%# Eval("NodeId")%>' style="display:none;"></asp:Label>
                        <asp:LinkButton ID="ctlContent" Font-Underline="false" CommandName="ShowService" runat="server" Text='<%# Eval("Header")%>' SkinID="SkCtlMenuBottom"/>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    
                    </ItemTemplate>
                    </asp:Repeater>
                </tr>
                </table>
                
            </td>
            <td align="left" style="width:12px"><div id="imgFooterRight" runat="server" class="FooterRight"></div></td>
        </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="right"><div id="imgCopyRight" runat="server" class="CopyRight"></div></td>
        </tr>
        </table>
    </td>
</tr>
</table>

</ContentTemplate>
</asp:UpdatePanel>