<%@ Control Language="C#" EnableTheming="true" AutoEventWireup="true" CodeBehind="ConfirmSubmit.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ConfirmSubmit" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:Panel ID="ctlPanelConfirmSubmit" runat="server" Width="300px" BackColor="White"
    Style="display: none;">
    <asp:Panel ID="ctlInvoiceFormHeader" CssClass="table" runat="server" Style="cursor: move;
        border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="lblCapture" runat="server" Text='$Confirm Submit$'></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="ctlContent" runat="server">
        <asp:UpdatePanel ID="ctlUpdatePanelConfirmSubmit" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelConfirmSubmit"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <center>
                    <table class="table">
                        <tr>
                            <td align="center">
                                <asp:Label ID="ctlConfirmMessage" runat="server" SkinID="SkGeneralLabel" Text="คุณต้องการ submit เอกสารนี้หรือไม่ ?" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="ctlOkButton" runat="server" Text="$Send Document$" OnClick="ctlOkButton_Click" />                                
                                <asp:Button ID="ctlCancel" runat="server" Text="$Save Draft$" OnClick="ctlCancel_Click" />
                                <asp:Button ID="ctlSaveAndAddAdvance" runat="server" Text="$Save and Add Advance$" OnClick="ctlSaveAndAddAdvance_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:LinkButton ID="ConfirmlnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ctlConfirmModalPopupExtender" runat="server"
    TargetControlID="ConfirmlnkDummy" PopupControlID="ctlPanelConfirmSubmit" BackgroundCssClass="modalBackground"
    CancelControlID="ConfirmlnkDummy" DropShadow="true" RepositionMode="None" />
