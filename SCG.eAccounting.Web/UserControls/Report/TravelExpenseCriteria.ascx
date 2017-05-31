<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="TravelExpenseCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.TravelExpenseCriteria" 
EnableTheming="true"%>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" tagname="CompanyField" tagprefix="uc1" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register src="../LOV/SCG.DB/TALookupField.ascx" tagname="TALookupField" tagprefix="uc4" %>
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
                <asp:Label ID="ctlFromDateText" runat="server" Text="$From Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc3:Calendar id="ctlFromDate" runat="server"></uc3:Calendar>        
            </td>
            <td>
                <asp:Label ID="ctlToDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc3:Calendar id="ctlToDate" runat="server"></uc3:Calendar>     
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromTravelDateText" runat="server" Text="$From Travel Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc3:Calendar id="ctlFromTravelDate" runat="server"></uc3:Calendar>        
            </td>
            <td>
                <asp:Label ID="ctlToTravelDateText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc3:Calendar id="ctlToTravelDate" runat="server"></uc3:Calendar>     
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlFromTaNoText" runat="server" Text="$From TA No.$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc4:TALookupField ID="ctlFromTaNo" runat="server" />
            </td>
            <td>
                <asp:Label ID="ctlToTaNoText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc4:TALookupField ID="ctlToTaNo" runat="server" />
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlFromTravellerText" runat="server" Text="$From Traveller$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:UserAutoCompleteLookup ID="ctlFromTraveller" runat="server"></uc2:UserAutoCompleteLookup>        
            </td>
            <td>
                <asp:Label ID="ctlToTravellerText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc2:UserAutoCompleteLookup id="ctlToTraveller" runat="server"></uc2:UserAutoCompleteLookup>     
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlTaStatusText" runat="server" Text="$Status$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlTAStatus" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="0">ALL</asp:ListItem>
                    <asp:ListItem Value="1">Open Travel by TA / Trip</asp:ListItem>
                    <asp:ListItem Value="2">Complete Travel by TA / Trip</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
