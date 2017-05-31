<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FixedAdvanceDuplicateCriteria.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.Report.FixedAdvanceDuplicateCriteria" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" tagname="CompanyField" tagprefix="uc1" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/LocationField.ascx" tagname="LocationField" tagprefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/ServiceTeam.ascx" TagName="ServiceTeam" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ActorData.ascx" TagName="ActorData" TagPrefix="uc7" %>
<asp:UpdatePanel ID="UpdatePanelVendor" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:60%" class="table">
    <legend id="legSearch" style="color:#4E9DDF" class="table"><asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
    </legend>
    <table class="table" width="600px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="ctlGroupText" runat="server" Text="$Group$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlBUDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlCompanyText" runat="server" Text="$Company$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <uc1:CompanyField ID="ctlCompanyField" runat="server" />
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
        <%--<tr align="left">
            <td>
                <asp:Label ID="ctlApproverDataText" runat="server" Text ="$From Approver$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3" align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelApprover" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <uc4:UserAutoCompleteLookup ID="ctlApproverData" runat="server"/>
                </ContentTemplate>
             </asp:UpdatePanel>
            </td>
        </tr>--%>
        <tr>
            <td>
                <asp:Label ID="ctlFromDueDateText" runat="server" Text="$From Due (Date)$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <uc3:Calendar ID="ctlFromDateCalendar" runat="server"></uc3:Calendar>
            </td>
            <td>
                <asp:Label ID="ctlToDueDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <uc3:Calendar ID="ctlToDateCalendar" runat="server"></uc3:Calendar>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
