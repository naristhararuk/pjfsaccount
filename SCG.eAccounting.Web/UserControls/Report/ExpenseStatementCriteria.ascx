<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="ExpenseStatementCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.ExpenseStatementCriteria" 
EnableTheming="true"%>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc2" %>
<asp:UpdatePanel ID="UpdatePanelExpenseStatement" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:60%" class="table">
    <legend id="legSearch" style="color:#4E9DDF" class="table"><asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
    </legend>
    <table class="table" width="600px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="ctlFromEmployeeText" runat="server" Text="$From Employee ID$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc1:UserAutoCompleteLookup ID="ctlFromEmployeeID" runat="server" />
            </td>
            <td>
                <asp:Label ID="ctlToEmployeeText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc1:UserAutoCompleteLookup ID="ctlToEmployeeID" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromDueDateText" runat="server" Text="$From Due Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:Calendar ID="ctlFromDueDate" runat="server" />
            </td>
            <td>
                <asp:Label ID="ctlToDueDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:Calendar ID="ctlToDueDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlDocumentStatusText" runat="server" Text="$Document Status$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlDocumentStatus" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="ALL">ALL</asp:ListItem>
                    <asp:ListItem Value="Complete">Completed</asp:ListItem>
                    <asp:ListItem Value="Hold">Hold</asp:ListItem>
                    <asp:ListItem Value="Reject">Reject</asp:ListItem>
                    <asp:ListItem Value="WaitApprove">Wait for Approve</asp:ListItem>
                    <asp:ListItem Value="WaitApproveVerify">Wait for Approve Verify</asp:ListItem>
                    <asp:ListItem Value="WaitInitial">Wait for Initial</asp:ListItem>
                    <asp:ListItem Value="WaitPayment">Wait for Payment</asp:ListItem>
                    <asp:ListItem Value="WaitReceive">Wait for Receive Document</asp:ListItem>
                    <asp:ListItem Value="WaitRemittance">Wait for Remittance</asp:ListItem>
                    <asp:ListItem Value="WaitVerify">Wait for Verify</asp:ListItem>              
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
