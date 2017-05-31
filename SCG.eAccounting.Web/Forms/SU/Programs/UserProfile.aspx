<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.UserProfile"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="../../../UserControls/UserProfileEditor.ascx" TagName="UserProfileEditor"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ApproverEditor.ascx" TagName="ApproverEditor"
    TagPrefix="uc2" %>
<%@ Register Src="../../../UserControls/InitiatorEditor.ascx" TagName="InitiatorEditor"
    TagPrefix="uc3" %>
    <%@ Register Src="../../../UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc5" %>
<%@ Register Src="../../../UserControls/LOV/SS.DB/SupervisorUserField.ascx" TagName="SupervisorUserField"
    TagPrefix="uc6" %>
<%@ Register Src="../../../UserControls/LOV/SCG.DB/LocationUserField.ascx" TagName="LocationUserField"
    TagPrefix="uc4" %>
<%@ Register Src="../../../UserControls/LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdateUserProfilePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <uc1:UserProfileEditor ID="ctlUserProfileEditor" runat="server" IsAdmin="false" ShowScrollBar="false" />
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ctlApproverButton" runat="server" Text="$Approver$" OnClick="ctlApproverButton_Click" />
                        <asp:Button ID="ctlInitiatorButton" runat="server" Text="$Initiator$" OnClick="ctlInitiatorButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:ApproverEditor ID="ctlApproverEditor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc3:InitiatorEditor ID="ctlInitiatorEditor" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
