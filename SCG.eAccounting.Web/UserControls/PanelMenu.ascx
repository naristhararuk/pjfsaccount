<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="PanelMenu.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.PanelMenu" 
    EnableTheming="true"
%>

<%@ Register src="Menu.ascx"            tagname="Menu"              tagprefix="uc1" %>
<%@ Register src="UserInfoWelcome.ascx" tagname="UserInfoWelcome"   tagprefix="uc4" %>
<%@ Register src="LanguageFlag.ascx"    tagname="LanguageFlag"      tagprefix="uc3" %>

<table border="0" cellpadding="0" cellspacing="0" style="vertical-align:top;height:100%;width:100%;">
<tr>
    <td align="left">
        <uc1:Menu ID="Menu1" runat="server" SkinID="SkMenu" Orientation="Horizontal"/>
    </td>
    <td align="right">
        <uc4:UserInfoWelcome ID="UserInfoWelcome1" runat="server"/>
    </td>
    <td align="center" class="backGroundLanguage" >
        <table border="0">
            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td><uc3:LanguageFlag ID="LanguageFlag1" runat="server" /></td>
            </tr>
        </table>
    </td>
</tr>
</table>
