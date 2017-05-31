<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="LoginError.aspx.cs" Inherits="SCG.eAccounting.Web.LoginError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<asp:Literal ID="Msg" runat="server"/>
<asp:LinkButton ID="linkButtonBack" runat="server" onclick="Back_Click"></asp:LinkButton>
</asp:Content>
