<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="DocumentFollowupCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.DocumentFollowupCriteria" 
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
                <asp:Label ID="ctlFromLocationText" runat="server" Text="$From Location$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:LocationField id="ctlFromLocationField" runat="server"></uc2:LocationField>        
            </td>
            <td>
                <asp:Label ID="ctlToLocationText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc2:LocationField id="ctlToLocationField" runat="server" ></uc2:LocationField>   
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromDateText" runat="server" Text="$From Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <uc3:Calendar ID="ctlFromDateCalendar" runat="server"></uc3:Calendar>
            </td>
            <td>
                <asp:Label ID="ctlToDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <uc3:Calendar ID="ctlToDateCalendar" runat="server"></uc3:Calendar>
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlCreatorDataText" runat="server" Text="$From Creator$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3" align="left">
                <uc4:UserAutoCompleteLookup ID="ctlCreatorData" runat="server" DisplayCaption="true"/>		
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlRequesterDataText" runat="server" Text ="$From Requester$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3" align="left">
                <uc4:UserAutoCompleteLookup ID="ctlRequesterData" runat="server" DisplayCaption="true"/>		
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlCostCenterText" runat="server" Text ="$Cost Center$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan ="3">
                <uc5:CostCenterField id ="ctlCostCenterField" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlServiceTeamText" runat="server" Text="$Service Team$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3">
                <%--<uc6:ServiceTeam id ="ctlServiceTeam" runat="server"/>--%>
                <asp:DropDownList ID="ctlServiceTeam" runat="server" SkinID="SkGeneralDropdown"></asp:DropDownList>
            </td>
        </tr>
        <%--<tr visible="false">
            <td>
                <asp:Label ID="ctlStatusText" runat="server" Text="$Status$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlStatus" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="0">ALL</asp:ListItem>
                    <asp:ListItem Value="1">Wait for Receive</asp:ListItem>
                    <asp:ListItem Value="2">Received</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td colspan="4">
                <asp:CheckBox ID="ctlCheckSerachOnly" SkinID="SkGeneralCheckBox" runat="server" Text="$Search only$" Checked ="true"></asp:CheckBox>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
