<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="ExpenseStatementReport.aspx.cs" 
	Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.ExpenseStatementReport" EnableTheming="true" StylesheetTheme="Default" EnableEventValidation="false"   %>
<%@ Register src="~/UserControls/Report/ExpenseStatementCriteria.ascx" tagname="ExpenseStatementCriteria" tagprefix="uc1"%>
<%@ Register src="~/UserControls/ReportViewers.ascx" tagname="ReportViewers" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<%--<asp:UpdatePanel ID="ctlUpdatePanelCriteria" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <table width="100%" border=0>
            <tr>
                <td>
                    <uc1:ExpenseStatementCriteria ID="ctlExpenseStatementCriteria" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                     <asp:ImageButton runat="server" ID="ctlPreview" ToolTip="Preview" SkinID="SkSearchButton" onclick="ctlPreview_Click"/>
                     <asp:ImageButton runat="server" ID="ctlPrint" ToolTip="Print" SkinID="SkCtlPrint" onclick="ctlPrint_Click"/>
                </td>
            </tr>
        </table>
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
<table width="100%" style="height:700px" class="table">
        <tr>
            <td>
                <uc2:ReportViewers 
                        ID="ExpenseStatement_Viewer" 
                        runat="server" 
                        ReportFolderPath="eAccountingReport" 
                        ReportName="EmployeeExpenseStatement"                         
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