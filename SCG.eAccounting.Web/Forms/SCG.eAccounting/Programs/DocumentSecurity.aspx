<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="DocumentSecurity.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.DocumentSecurity"  EnableTheming="true" StylesheetTheme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<br />
<br />
<br />
<div align="center">
    <table border="0">
    <tr>
        <td>
            <asp:Label ID="ctlClickText" runat="server" Text="You have no permission to<br> view this document." SkinID="SkCtlUserInfo" />
        </td>
    </tr>
    <tr>
        <td>
        <asp:HyperLink ID="ctlLinkHomePage" runat="server" Font-Names="Tahoma" Font-Size="10"/>
        </td>
    </tr>
    </table>
</div>


</asp:Content>
