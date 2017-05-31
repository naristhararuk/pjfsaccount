<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true" CodeBehind="SummaryExpenseReport.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.SummaryExpenseReport" 
EnableTheming="true"
StylesheetTheme="Default"%>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" tagname="CompanyTextboxAutoComplete" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<asp:UpdatePanel ID="UpdatePanelSummaryExpenseReport" runat="server" UpdateMode="Conditional"></asp:UpdatePanel>
    <ContentTemplate>
        <center>
            <table style="text-align:left" class="table">
                <tr>
                    <td><asp:Label Text="Document Type : " ID="ctlDocumentType" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
                    <td> <asp:DropDownList runat="server" SkinID="SkGeneralDropdown"  ID= "ctlDropDownDocType">
                            <asp:ListItem Value="ALL">ALL</asp:ListItem>
                            <asp:ListItem Value="RMT">Remittance </asp:ListItem>
                            <asp:ListItem Value="EXD">Expense (Domestic)</asp:ListItem>
                            <asp:ListItem Value="EXF">Expense (Foreign)</asp:ListItem>
                            <asp:ListItem Value="EHR">EHR</asp:ListItem>
                            <asp:ListItem Value="ADF">Advance (Domestic)</asp:ListItem>
                            <asp:ListItem Value="ADV">Advance (Foreign)</asp:ListItem>     
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Document Date From : " ID="ctlDocumentDate" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
                    <td><uc1:Calendar ID="ctlFromDocDate" runat="server"></uc1:Calendar></td>
                    <td><asp:Label Text="To : " ID="ctlToDate" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
                    <td><uc1:Calendar ID="ctlToDocDate" runat="server"></uc1:Calendar></td>
                </tr>
                <tr>
                    <td><asp:Label Text="Company : " ID="ctlLabelCompany" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
<%--                        <asp:Label id="ctlLableCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>
                    <td><uc2:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete" runat="server"/>
                </tr>
                <tr>  
                    <td></td>
                    <td><asp:Button ID="btnAddCompany" runat="server" Text="Add" SkinID ="SkCtlButton" onclick="btnAddCompany_Click" /></td>
                </tr> 
                <tr>
                    <td></td>
                    <td><asp:TextBox runat="server" Width="200" Height="80" MaxLength="1000" ID="txtCompanyList" ></asp:TextBox></td>
                </tr>      
                       
            </table>
            <table>
            <tr>
                <td><asp:Button runat="server" Text="Export" SkinID ="SkCtlButton" ID="ctlButtonExport" onclick="ctlButtonExport_Click"/></td>
                </tr>  
            </table>
        </center>
    </ContentTemplate>
</asp:Content>

