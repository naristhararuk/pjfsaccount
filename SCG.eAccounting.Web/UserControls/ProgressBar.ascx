<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressBar.ascx.cs" EnableTheming="true"
    Inherits="SCG.eAccounting.Web.UserControls.ProgressBar" %>
   
<asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" DynamicLayout="true" EnableViewState="False">
    <ProgressTemplate>
        <asp:Panel ID="PanelProgress" CssClass="ProgressOverlay" runat="server">
            <div class="ProgressContainer">
                <div class="ProgressHeader">
                    <asp:Label ID="lblLoading" runat="server" Text="$lblLoading$"></asp:Label>
                </div>
                <div class="ProgressBody">
                    <asp:ImageButton ID="Image1" runat="server" SkinID="SkProgress" />
                </div>
            </div>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
