<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TAWorkflowMonitor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.TAWorkflowMonitor" %>
<table width="100%" align="center" cellpadding="0" cellspacing="0" style="background-color:#ff9999">
    <tr align="center" id="workflowState" runat="server">
        <td width="35%">
            <asp:Label ID="requester" runat="server" Text="Requester" SkinID="SkCtlLabel" />
        </td>
        <td width="35%">
            <asp:Label ID="initialApprove" runat="server" Text="Initial & Approve" SkinID="SkCtlLabel"/>
        </td>
        <td width="30%">
            <asp:Label ID="complete" runat="server" Text="Complete"  SkinID="SkCtlLabel"/>
        </td>
    </tr>
</table>
