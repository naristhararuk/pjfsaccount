<%@ Page 
    Language="C#" 
    MasterPageFile="~/ProgramsPages.Master" 
    AutoEventWireup="true" 
    CodeBehind="Inbox.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Inbox" 
    Title="Untitled Page" 
    StylesheetTheme="Default" meta:resourcekey="PageResource1"
    EnableTheming="true" 
%>
<%@ Register src="UserControls/InboxSearchResult/SearchResult.ascx" tagname="SearchResult" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<div style="vertical-align:top">
    <table width="300px">
        <tr>
            <td valign="middle" align="right">
                <asp:Label ID="ctlDocStatus" runat="server" Text="Document Status" SkinID="SkCtlLabel"></asp:Label></td>
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:DropDownList ID="DropDownList1" runat="server" Height="30px" Width="130px">
                    <asp:ListItem>Draft</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <BR />
    
    <uc1:SearchResult ID="SearchResult1" runat="server" />
   
</div>

</asp:Content>
