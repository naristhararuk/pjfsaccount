<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="Time.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.Time" %>

<table border="0" cellpadding="0" cellspacing="0">
<tr>
    <td valign="middle">
        <asp:TextBox ID="txtTime" SkinID="SkCalendarTextBox" runat="server" Width="75px"></asp:TextBox>
        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender" 
                runat="server" 
                MaskType="Time"  
                TargetControlID="txtTime" 
                Mask="99:99"
                MessageValidatorTip="false"
                OnFocusCssClass="MaskedEditFocus" 
                OnInvalidCssClass="MaskedEditError" />
       <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender"
                runat="server"
                TargetControlID="txtTime" 
                 FilterType ="Numbers" --/>--%> 
    </td>
</tr>
</table>
