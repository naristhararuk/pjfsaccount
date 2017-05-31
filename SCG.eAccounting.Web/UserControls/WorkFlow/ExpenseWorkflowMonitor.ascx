<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseWorkflowMonitor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.ExpenseWorkflowMonitor" %>
<table width="100%" align="center" cellpadding="0" cellspacing="0" style="background-color:#ff9999">
    <tr align="center" id="workflowState" runat="server">
        <td width="20%">
            <asp:Label ID="requester" runat="server" Text="Requester" SkinID="SkCtlLabel" />
        </td>
        <td width="20%">
            <asp:Label ID="initialApprove" runat="server" Text="Initial & Approve" SkinID="SkCtlLabel"/>
        </td>
        <td width="20%">
            <asp:Label ID="verify" runat="server" Text="Verify" SkinID="SkCtlLabel"/>
        </td>
        <td width="20%">
            <asp:Label ID="payment" runat="server" Text="Payment" SkinID="SkCtlLabel" />
        </td>
        <td width="20%">
            <asp:Label ID="complete" runat="server" Text="Complete"  SkinID="SkCtlLabel"/>
        </td>
    </tr>
</table>
