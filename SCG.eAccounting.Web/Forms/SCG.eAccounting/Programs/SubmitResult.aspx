<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="SubmitResult.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.SubmitResult" EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register src="~/UserControls/StaticAlertMessage.ascx" tagname="AlertMessage" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

    <table width="100%" style="height:300px;vertical-align:middle" border="0">
        <tr align="center">
            <td>
                <asp:Label ID="ctlSubmitResult" runat="server" SkinID="SkCtlUserInfo"/><br />
                <asp:Label ID="ctlClickText" runat="server" Text="Click " SkinID="SkCtlUserInfo" />
                <asp:HyperLink ID="ctlLinkToSubmittedDocument" runat="server" Font-Names="Tahoma" Font-Size="10"/>
                <asp:Label ID="ctlViewReq" runat="server" SkinID="SkCtlUserInfo"/>
            </td>
        </tr>
    </table>      
</asp:Content>
