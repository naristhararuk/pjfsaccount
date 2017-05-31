<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="Calendar.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.Calendar" %>
<table border="0" cellpadding="0" id="ctlCalendarTable" cellspacing="0" runat="server">
    <tr>
        <td valign="middle">
            <asp:TextBox ID="txtDate" SkinID="SkCalendarTextBox" runat="server" Width="75px"></asp:TextBox>
        </td>
        <td valign="middle">
            <asp:ImageButton style="margin-left:2px !important" runat="Server" ID="imgDate" SkinID="SkCtlCalendar" AlternateText="Click to show calendar"/>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate"
                PopupButtonID="imgDate" SkinID="SkCtlCalendar">
            </ajaxToolkit:CalendarExtender>
            <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" 
                runat="server" 
                MaskType="Date"  
                TargetControlID="txtDate" 
                Mask="99/99/9999"
                MessageValidatorTip="false"
                OnFocusCssClass="MaskedEditFocus" 
                OnInvalidCssClass="MaskedEditError" />
            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" 
                runat="server" 
                ControlToValidate="txtDate" 
                ControlExtender="MaskedEditExtender1" 
                Display="Dynamic" 
                TooltipMessage="" 
                IsValidEmpty="false" 
                EmptyValueMessage="" 
                InvalidValueMessage="" />   --%>
        </td>
    </tr>
</table>
