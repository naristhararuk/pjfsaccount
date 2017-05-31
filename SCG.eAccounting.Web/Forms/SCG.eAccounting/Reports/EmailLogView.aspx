<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true" CodeBehind="EmailLogView.aspx.cs" StylesheetTheme="Default"
Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.EmailLogView" EnableTheming="true"%>
<%@ Register src="~/UserControls/Report/EmailLog.ascx" tagname="EmailLog" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="A" runat="server">
<asp:UpdatePanel ID="ctlUpdatePanelCriteria" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" border=0>
            <tr>
                <td>
                    <uc1:EmailLog ID="ctlEmailLog" runat="server"/>
                </td>
            </tr>
        </table>
     </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>