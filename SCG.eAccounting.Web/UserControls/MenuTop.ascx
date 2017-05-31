<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MenuTop.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.MenuTop" 
    EnableTheming="true"
%>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:UpdatePanel ID="updPanelContent" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelContent" DynamicLayout="true" EnableViewState="False">
    <ProgressTemplate>
        <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
    </ProgressTemplate>
</asp:UpdateProgress>

<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
    <td align="left"><asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="logoSCGLogIn" /></td>
    <td align="right">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr align="left">
            <asp:Repeater ID="rptMainMenu" runat="server" OnItemCommand="rptMainMenu_ItemCommand">
            <ItemTemplate>
            
            <td><asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif" CssClass="menuSeperateLogIn"/></td>
            <td>&nbsp;&nbsp;</td>
            <td valign="middle" align="left">
                <asp:Label  ID="ctlContentID" runat="server" Text='<%# Eval("NodeId")%>' style="display:none;"></asp:Label>
                <asp:LinkButton ID="ctlContent" Font-Underline="false" CommandName="ShowService" runat="server" Text='<%# Eval("Header")%>' SkinID="SkCtlMenuTop"/>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        
            </ItemTemplate>
            </asp:Repeater>
        </tr>
        </table>
    </td>
</tr>
</table>

</ContentTemplate>
</asp:UpdatePanel>