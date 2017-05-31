<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageFlag.ascx.cs" EnableTheming="true"  Inherits="SCG.eAccounting.Web.UserControls.LanguageFlag" %>
<table border="0" id="FlagLanguage"><tr><td>
    <asp:ImageButton ID="CtlEnglishFlagButton" runat="server" 
        SkinID="SkCtlEnglishFlagButton" onclick="CtlEnglishFlagButton_Click" 
        ImageUrl="~/App_Themes/Default/images/empty.gif" />

</td>
<td>
    &nbsp;
</td>
    <td>
<asp:ImageButton ID="CtlThaiFlagButton" runat="server" SkinID="SkCtlThaiFlagButton" 
            onclick="CtlThaiFlagButton_Click" 
            ImageUrl="~/App_Themes/Default/images/empty.gif" />

    </td>
</tr>
</table>
