<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="PanelLogin.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.PanelLogin" 
    EnableTheming="true"
%>

<%@ Register Src="LogIn.ascx"               TagName="LogIn"                 TagPrefix="uc2" %>
<%@ Register Src="LanguageFlag.ascx"        TagName="LanguageFlag"          TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"   TagPrefix="AjaxToolkit" %>

<table border="0" cellpadding="0" cellspacing="0" style="vertical-align:top;height:100%;width:100%;">
<tr>
    <td align="left">
        <uc2:LogIn ID="LogIn1" runat="server" />
    </td>
    <td align="center" class="backGroundLanguageLogIn" >
        <table border="0">
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><uc3:LanguageFlag ID="LanguageFlag1" runat="server" /></td>
        </tr>
        </table>
    </td>
</tr>
</table>
