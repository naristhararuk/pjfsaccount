<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupRequireDocumentAttachment.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.PopupRequireDocumentAttachment"
    EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<asp:Panel ID="ctlPanelWarningPopup" runat="server" Width="300px" Height="120px"
    BackColor="Yellow" Style="display: none;">
    <asp:Panel ID="ctlFormHeader" CssClass="table" runat="server" Style="cursor: move;
        color: Black; margin: 5;">
        <div>
            <p>
                <asp:Label ID="lblCapture" runat="server" Text='Warning Message' Font-Size="Medium"
                    ForeColor="Red"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="ctlContent" runat="server">
        <asp:UpdatePanel ID="ctlUpdatePanelWarningPopup" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelWarningPopup"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <center>
                    <table class="table" height="85%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="ctlWarningMessage" runat="server" SkinID="SkGeneralLabel" Text="" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ctlOkButton" runat="server" ImageUrl="~/App_Themes/Default/images/ok_btn.png" OnClick="ctlOkButton_Click" />
                                <%--<asp:Button ID="ctlOkButton" runat="server" Width="50px" Text="OK" OnClick="ctlOkButton_Click" />   --%>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:LinkButton ID="ConfirmlnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ctlWarningPopupModalPopupExtender" runat="server"
    TargetControlID="ConfirmlnkDummy" PopupControlID="ctlPanelWarningPopup" BackgroundCssClass="modalBackground"
    CancelControlID="ConfirmlnkDummy" DropShadow="true" RepositionMode="None" />
