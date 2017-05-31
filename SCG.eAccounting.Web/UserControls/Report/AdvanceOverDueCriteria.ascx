<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="AdvanceOverDueCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.AdvanceOverDueCriteria" 
EnableTheming="true"%>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" tagname="CompanyField" tagprefix="uc1" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/LocationField.ascx" tagname="LocationField" tagprefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/ServiceTeam.ascx" TagName="ServiceTeam" TagPrefix="uc6" %>
<asp:UpdatePanel ID="UpdatePanelVendor" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:60%" class="table">
    <legend id="legSearch" style="color:#4E9DDF" class="table"><asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
    </legend>
    <table class="table" width="600px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="ctlCompanyText" runat="server" Text="$Company$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <uc1:CompanyField ID="ctlCompanyField" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromLocationText" runat="server" Text="$From Location's Requester$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:LocationField id="ctlFromLocationField" runat="server"></uc2:LocationField>        
            </td>
            <td>
                <asp:Label ID="ctlToLocationText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc2:LocationField id="ctlToLocationField" runat="server"></uc2:LocationField>   
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromDueDateText" runat="server" Text="$From Due (Date)$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <uc3:Calendar ID="ctlFromDueDateCalendar" runat="server"></uc3:Calendar>
            </td>
            <td>
                <asp:Label ID="ctlToDueDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <uc3:Calendar ID="ctlToDueDateCalendar" runat="server"></uc3:Calendar>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromAdvanceAmountText" runat="server" Text="$Advance Amount$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <asp:TextBox ID="ctlFromAdvanceAmount" runat="server" SkinID="SkNumberTextBox"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="ctlToAdvanceAmountText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <asp:TextBox ID="ctlToAdvanceAmount" runat="server" SkinID="SkNumberTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromOverdueDayText" runat="server" Text="$From Overdue (day)$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <asp:TextBox ID="ctlFromOverdueDay" runat="server" SkinID="SkNumberTextBox"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="ctltoOverdueDayText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <asp:TextBox ID="ctlToOverdueDay" runat="server" SkinID="SkNumberTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlDocumentTypeText" runat="server" Text="$Advance Type$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlDocumentType" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="ALL">ALL</asp:ListItem>
                    <asp:ListItem Value="DM">Domestic</asp:ListItem>
                    <asp:ListItem Value="FR">Foreign</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlRequesterDataText" runat="server" Text ="$From Requester$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3" align="left">
                <uc4:UserAutoCompleteLookup ID="ctlRequesterData" runat="server"/>		
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
