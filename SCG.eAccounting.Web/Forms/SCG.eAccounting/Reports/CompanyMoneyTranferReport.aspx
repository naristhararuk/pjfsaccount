<%@ Page Title=""
Language="C#" 
MasterPageFile="~/ProgramsPages.Master"
AutoEventWireup="true" 
CodeBehind="CompanyMoneyTranferReport.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.CompanyMoneyTranferReport" 
EnableTheming="true"
StylesheetTheme="Default"
%>
<%@ Register src="~/UserControls/CalendarOfDueDate.ascx" tagname="CalendarOfDueDate" tagprefix="uc1" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" tagname="CompanyTextboxAutoComplete" tagprefix="uc2" %>
<%@ Register src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc3" %>
<%@ Register src="~/UserControls/ReportViewers.ascx" tagname="ReportViewers" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            height: 43px;
        }
        .style3
        {
            height: 56px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <center>
        <table style="text-align:left" class="table">
            <tr>
            <td class="style2"><asp:Label Text="From Posting Date *: " ID="ctlLabelFrom" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
            <td class="style2">
                <uc3:Calendar ID="Calendar1" runat="server" />
                </td>
            <td class="style2"><asp:Label Text="To Posting Date *: " ID="ctlLabelTo" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
            <td class="style2">
                <uc3:Calendar ID="Calendar2" runat="server" />
                </td>
            </tr>
            <tr>
            <td><asp:Label Text="Company : " ID="ctlLabelCompany" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
            <td>
                <uc2:CompanyTextboxAutoComplete ID="CompanyTextboxAutoComplete1" 
                    runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan=4 style="text-align:center;" class="style3">
                    <asp:Button runat="server" Text="Print" SkinID ="SkCtlButton" 
                                ID="ctlButtonPrint" onclick="ctlButtonPrint_Click" Width="98px"/>
                </td>
            </tr>
        </table>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="Export.Error">
                </spring:ValidationSummary>
            </font>
    </center>
</asp:Content>