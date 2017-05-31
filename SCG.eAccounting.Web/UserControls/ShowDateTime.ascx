<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ShowDateTime.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ShowDateTime" 
    EnableTheming = "true"
%>

<asp:UpdatePanel ID="PanelDate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Timer ID="time" runat="server" Enabled="False" Interval="1000" 
            ontick="time_Tick">
        </asp:Timer>
        <asp:Label ID="lblDate" runat="server" Text="" SkinID="SkCtlUserInfo"></asp:Label>
        <asp:Label ID="lblColon" runat="server" Text=" " SkinID="SkCtlUserInfo"></asp:Label>
        <asp:Label ID="lblTime" runat="server" Text="" SkinID="SkCtlUserInfo"></asp:Label>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="time" EventName="Tick" />
    </Triggers>
</asp:UpdatePanel>