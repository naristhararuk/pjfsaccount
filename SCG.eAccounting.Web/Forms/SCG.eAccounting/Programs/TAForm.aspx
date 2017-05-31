﻿<%@ Page Title="Travelling Authorizer Form" Language="C#" MasterPageFile="~/ProgramsPages.Master"
    AutoEventWireup="true" CodeBehind="TAForm.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.TAForm"
    StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register Src="~/UserControls/DocumentEditor/TADocumentEditor.ascx" TagName="TADocumentEditor"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

    <script language="javascript" type="text/javascript">
    function confirm_save() {
        var hiddenConfirm = document.getElementById('<%= ctlHiddenConfirm.ClientID %>');
        if (confirm('คุณต้องการ submit เอกสารนี้หรือไม่') == true) {
            hiddenConfirm.value = "ok"
            return true;
        }
        else {
            hiddenConfirm.value = "cancel";
            return false;
        }
    }
    </script>

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:TADocumentEditor ID="ctlTADocumentEditor" runat="server" />
                <asp:HiddenField ID="ctlHiddenConfirm" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="ctlUpdatePanelReadWriteButton" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div id="ctlDivReadWriteButton" runat="server">
                            <table width="100%">
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlSave" runat="server" SkinID="SkCtlGridSave" ToolTip="Save"
                                            OnClick="ctlSave_Click" />
                                    </td>
                                    <td valign="middle">
                                        |
                                    </td>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlGridCancel" ToolTip="Cancel"
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
                <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table border="0" width="400px">
                            <tr>
                                <td align="left" style="color: Red; font-family: Tahoma; font-size: 10pt;">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error" />
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
