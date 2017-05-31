<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SCGLoading.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.SCGLoading" 
    EnableTheming="true"
%>

<asp:Panel ID="PanelProgress" CssClass="ProgressOverlay" runat="server" >
<div class="ProgressContainer">
    <div class="ProgressHeader">
    <table border="0">
        <tr valign="middle">
            <td>
                <img alt="Loading" src='<%= ResolveUrl("~/App_Themes/Default/Images/Loading/loading.gif")%>' />
            </td>
            <td>
                <img alt="Loading" src='<%= ResolveUrl("~/App_Themes/Default/Images/Loading/SCGLoading.png")%>' />
            </td>
        </tr>
    </table>
    </div>
</div>
</asp:Panel>

<%--<asp:Panel ID="PanelProgress" CssClass="ProgressOverlay" runat="server">
    <div class="ProgressContainer">
        <div class="ProgressHeader">
            <asp:Label ID="lblLoading" runat="server" Text="$lblLoading$"></asp:Label>
        </div>
        <div class="ProgressBody">
            <asp:ImageButton ID="Image1" runat="server" SkinID="SkProgress" />
        </div>
    </div>
</asp:Panel>--%>