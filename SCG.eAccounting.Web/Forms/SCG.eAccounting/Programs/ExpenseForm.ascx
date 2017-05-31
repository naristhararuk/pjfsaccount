<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseForm.ascx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ExpenseForm" EnableTheming="true" %>
<%@ Register Src="~/UserControls/DocumentEditor/ExpenseDocumentEditor.ascx"
    TagName="ExpenseDocumentEditor" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<table width="100%">
    <tr>
        <td>
            <uc1:ExpenseDocumentEditor ID="X" runat="server" Visible="True" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelReadwriteButton" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdatePanelButtonProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelReadwriteButton"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    <div id="ctlDivReadWriteButton" runat="server">
                        <table width="100%">
                            <tr>
                                <td valign="middle">
                                    <asp:ImageButton ID="ctlSave" runat="server" SkinID="SkSaveButton" ToolTip="Save"
                                        OnClick="ctlSave_Click" />
                                </td>
                                <td valign="middle">
                                    |
                                </td>
                                <td valign="middle">
                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCancelButton" ToolTip="Cancel"
                                        OnClick="ctlCancel_Click" />
                                </td>
                                <td valign="middle" style="width: 100%">
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgressValidation" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelReadwriteButton"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    <table border="0" width="400px;">
                        <tr>
                            <td align="left" style="color: Red; font-family: Tahoma; font-size: 10pt;">
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                                </spring:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
