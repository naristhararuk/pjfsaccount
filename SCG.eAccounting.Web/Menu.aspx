<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/ProgramsPages.Master" 
    CodeBehind="Menu.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Menu" 
    EnableTheming="true" 
    StylesheetTheme="Default" 
%>

<%@ Register Src="~/UserControls/ServiceShowCase.ascx" TagName="ServiceShowCase" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOV/SS.DB/AnnouncementInfo2.ascx" TagName="AnnouncementInfo2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/WelcomeShow.ascx" TagName="WelcomeShow" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/InboxSearchResult/HomeSummary.ascx" TagName="HomeSummary" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <%--<div id="dvWelcomeMsg" runat="server" align="left">
        <uc3:welcomeshow ID="WelcomeShow2" runat="server"/>   
    </div>--%>
    <div id="divHomeSummary" runat="server" align="left">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table" >
            <tr>
                <td style="width:10px">&nbsp;</td>
                <td>
                    <uc4:HomeSummary ID="HomeSummary" runat="server" />
                </td>
            </tr>
        </table>        
    </div>
</asp:Content>
