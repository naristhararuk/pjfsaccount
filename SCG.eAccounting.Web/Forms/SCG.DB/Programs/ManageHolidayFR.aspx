<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageHolidayFR.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageHolidayFR"
    StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="Holiday.ascx" TagName="Holiday" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <uc1:Holiday ID="Holiday" runat="server" AdvanceTypeForm="2"/>
</asp:Content>

