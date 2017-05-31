<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" EnableEventValidation="false" 
 CodeBehind="FixedAdvanceCompareReport.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.FixedAdvanceCompareReport" 
 EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register src="~/UserControls/Report/FixedAdvanceCompareCriteria.ascx" tagname="FixedAdvanceCompareCriteria" tagprefix="uc1"%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="~/UserControls/ReportViewers.ascx" tagname="ReportViewers" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<table width="100%">
    <tr>
        <td>
            <uc1:FixedAdvanceCompareCriteria ID="ctlFixedAdvanceCompareCriteria" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:ImageButton runat="server" ID="ctlPreview" ToolTip="Preview" SkinID="SkSearchButton" onclick="ctlPreview_Click"/>
          <%--  <asp:ImageButton runat="server" ID="ctlPrint" ToolTip="Print" SkinID="SkCtlPrint" onclick="ctlPrint_Click"/>--%>
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
                <uc5:ReportViewers 
                        ID="FixedAdvanceCompareReport_Viewer" 
                        runat="server" 
                        ReportFolderPath="eAccountingReport" 
                        ReportName="FixedAdvanceCompareDetailReportV2"                         
                        ReportHeight="550" SetLayoutHeightType="Pixel" 
                        ReportWidth="100" SetLayoutWidthType="Percentage" 
                        DocumentMapCollapsed="true"
                        HideParameterOnForm="true"
                        Visible="true"
                        />
            </td>
        </tr>
        <tr>
            <td>
                <uc5:ReportViewers 
                        ID="FixedAdvanceCompareReportGraph_Viewer" 
                        runat="server" 
                        ReportFolderPath="eAccountingReport" 
                        ReportName="FixedAdvanceCompareGraphReportV2"                         
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
