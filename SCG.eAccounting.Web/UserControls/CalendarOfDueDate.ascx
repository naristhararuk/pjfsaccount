<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="CalendarOfDueDate.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CalendarOfDueDate" %>
    <ss:InlineScript runat="server">
        <script type="text/javascript">
            function CalculateReqDateOfRemit(reqDateObjID, dueDateObjID, incrementDate) {
                var actionField = jq('#<%= ctlReturnAction.ClientID %>');
                var valueField = jq('#<%= ctlReturnValue.ClientID %>');
                
                var beginDateObj = event.srcElement;
                var reqDateObj = document.getElementById(reqDateObjID);
                var dueDateObj = document.getElementById(dueDateObjID);
                if (beginDateObj.value != '')
                    dueDateObj.innerText = AddDate(beginDateObj.value, incrementDate);
                else
                    dueDateObj.innerText = '';
                //dueDateObj.innerText = reqDateObj.value;
                valueField.val(dueDateObj.innerText);
                actionField.val(dueDateObj.innerText);
                
                <%= Page.GetPostBackEventReference(ctlReturnAction) %>;
            }
        </script>
    </ss:InlineScript>
<table border="0" cellpadding="0" id="ctlCalendarTable" runat="server" cellspacing="0">
    <tr>
        <td valign="middle">
        <asp:HiddenField ID="ctlReturnAction" runat="server" OnValueChanged="ctlReturnAction_ValueChanged" />
        <asp:HiddenField ID="ctlReturnValue" runat="server" />
            <asp:TextBox ID="txtDate" SkinID="SkCalendarTextBox" runat="server" Width="75px"
                AutoPostBack="true"></asp:TextBox>
        </td>
        <td valign="middle">
            &nbsp;<asp:ImageButton runat="Server" ID="imgDate" SkinID="SkCtlCalendar" AlternateText="Click to show calendar"/>
            <asp:Label ID="ctlDueDate" runat="server"></asp:Label>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate"
                PopupButtonID="imgDate" SkinID="SkCtlCalendar">
            </ajaxToolkit:CalendarExtender>
        </td>
    </tr>
</table>
