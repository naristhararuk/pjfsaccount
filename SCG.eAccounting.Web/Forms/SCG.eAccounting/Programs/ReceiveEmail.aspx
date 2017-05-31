<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="ReceiveEmail.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ReceiveEmail" EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register src="~/UserControls/StaticAlertMessage.ascx" tagname="AlertMessage" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<div width="100%" align="center" style="display:none">
    <asp:Label ID="ctlReceiveResult" runat="server"/>
   
   
</div>
 <div id="divMessage" runat="server" style="display:none">
    <uc2:AlertMessage ID="AlertMessage1" runat="server"/>
</div>
    
</asp:Content>
