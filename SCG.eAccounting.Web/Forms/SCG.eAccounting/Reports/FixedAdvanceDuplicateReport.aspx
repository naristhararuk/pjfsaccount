<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" EnableEventValidation="false"
 CodeBehind="FixedAdvanceDuplicateReport.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.FixedAdvanceDuplicateReport" 
 EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register src="~/UserControls/Report/FixedAdvanceDuplicateCriteria.ascx" tagname="FixedAdvanceDuplicateCriteria" tagprefix="uc1"%>
<%@ Register src="~/UserControls/ReportViewers.ascx" tagname="ReportViewers" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

        <table width="100%">
            <tr>
                <td>
                    <uc1:FixedAdvanceDuplicateCriteria ID="ctlFixedAdvanceDuplicateCriteria" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton" onclick="ctlSearch_Click"/>
                </td>
            </tr>
        </table>
    <center>
        <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table">
                    <tr>
                        <td align="left" style="color: Red;">
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                            </spring:ValidationSummary>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>

    <table width="100%" style="height:700px" class="table">
        <tr>
            <td>
                <uc2:ReportViewers 
                        ID="FixedAdvanceDuplicate_Viewer" 
                        runat="server" 
                        ReportFolderPath="eAccountingReport" 
                        ReportName="AdvanceInPeriodReport"                         
                        ReportHeight="550" SetLayoutHeightType="Pixel" 
                        ReportWidth="100" SetLayoutWidthType="Percentage" 
                        DocumentMapCollapsed="true"
                        HideParameterOnForm="true"
                        Visible="true"
                        />
            </td>
        </tr>
    </table>

</asp:Content>
