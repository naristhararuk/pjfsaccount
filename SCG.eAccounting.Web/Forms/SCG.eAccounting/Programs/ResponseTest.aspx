<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="ResponseTest.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ResponseTest" %>
<%@ Register src="~/UserControls/WorkFlow/OverRole.ascx" tagname="OverRole" tagprefix="uc1" %>
<%@ Register src="~/UserControls/WorkFlow/HoldDetail.ascx" tagname="HoldDetail" tagprefix="uc2" %>
<%@ Register src="~/UserControls/WorkFlow/RejectDetail.ascx" tagname="RejectDetail" tagprefix="uc4" %>
<%@ Register src="~/UserControls/DocumentEditor/Components/History.ascx" tagname="History" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:OverRole ID="ctlOverRole" runat="server" />
    
    <asp:Button ID="ctlOverRoleSubmit" runat="server" Text="Over Role Submit" 
        onclick="ctlOverRoleSubmit_Click" />
    <br />
    <uc2:HoldDetail ID="HoldDetail1" runat="server" />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Over Role Submit" 
        onclick="ctlHoldDetail_Click" />
    <br />
    <br />
    <br />
    <uc4:RejectDetail ID="RejectDetail1" runat="server" />
    <br />
    <uc3:History ID="History1" runat="server" />
    <br />
    <br />
    <asp:DropDownList ID="ctlEmailPetternDdl" runat="server">
    <asp:ListItem Text="EM01" Value="1"></asp:ListItem>
    <asp:ListItem Text="EM02" Value="2"></asp:ListItem>
    <asp:ListItem Text="EM03" Value="3"></asp:ListItem>
    <asp:ListItem Text="EM04" Value="4"></asp:ListItem>
    <asp:ListItem Text="EM05" Value="5"></asp:ListItem>
    <asp:ListItem Text="EM06" Value="6"></asp:ListItem>
    <asp:ListItem Text="EM07" Value="7"></asp:ListItem>
    <asp:ListItem Text="EM08" Value="8"></asp:ListItem>
    <asp:ListItem Text="EM09" Value="9"></asp:ListItem>
    <asp:ListItem Text="EM10" Value="10"></asp:ListItem>
    <asp:ListItem Text="EM11" Value="11"></asp:ListItem>
    <asp:ListItem Text="EM12" Value="12"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="ctlSendEmail" runat="server" Text="SendEmail" 
        onclick="ctlSendEmail_Click" />
</asp:Content>
