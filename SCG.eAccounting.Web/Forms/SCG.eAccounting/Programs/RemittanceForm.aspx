<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="RemittanceForm.aspx.cs" EnableTheming="true" StylesheetTheme="Default"
    EnableEventValidation="false" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.RemittanceForm" %>

<%@ Register Src="~/UserControls/DocumentEditor/RemittanceDocumentEditor.ascx"
    TagName="RemittanceDocumentEditor" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

    <script type="text/javascript" language="javascript">
        function confirm_save() {
            var hiddenContirm = document.getElementById('<%= ctlHiddenConfirm.ClientID %>');
            if (confirm('คุณต้องการ submit เอกสารนี้หรือไม่') == true) {
                hiddenContirm.value = "ok"
                return true;
            }
            else {
                hiddenContirm.value = "cancel";
                return false;
            }
        }
    </script>

    <asp:HiddenField ID="ctlHiddenConfirm" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <uc1:RemittanceDocumentEditor ID="ctlRemittanceDocumentEditor" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="ctlUpdatePanelReadwriteButton" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div id="ctlDivReadWriteButton" runat="server">
                            <table width="100%">
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlSave" runat="server" SkinID="SkSaveButton" OnClick="ctlRemittanceSave_Click"
                                            ToolTip="Save" />
                                    </td>
                                    <td valign="middle">
                                        |
                                    </td>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCancelButton" OnClick="ctlRemittanceCancel_Click"
                                            ToolTip="Cancel" />
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
                <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table border="0" width="400px;">
                            <tr>
                                <td align="left" style="color: Red; font-family: Tahoma; font-size: 10pt;">
                                    <spring:ValidationSummary ID="ctlValidationSummary"  runat="server" Provider="Provider.Error">
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
</asp:Content>
