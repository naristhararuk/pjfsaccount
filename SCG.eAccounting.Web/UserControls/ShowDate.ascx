<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ShowDate.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ShowDate" 
    EnableTheming="true"
%>

<input id="txtLangID" type="hidden" runat="server" />
<asp:Label ID="lblDate" runat="server" Text="" SkinID="SkCtlUserInfo"></asp:Label>
<asp:Label ID="lblColon" runat="server" Text=" " SkinID="SkCtlUserInfo"></asp:Label>
<asp:Label ID="lblTime" runat="server" Text="" SkinID="SkCtlUserInfo"></asp:Label>
