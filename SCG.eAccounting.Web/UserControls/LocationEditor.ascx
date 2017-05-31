<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LocationEditor" %>
<asp:Panel ID="ctlLocationEditor" runat="server" Style="display: none" CssClass="modalPopup">
    <asp:Panel ID="ctlLocationEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
        background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlLocation" runat="server" SkinID="SkFieldCaptionLabel" Text='$Manage Location Data$'
                    Width="100%"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlLocationUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 90%" id="ctlLocationFieldSet">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlLocationCodeLabel" runat="server" Text="$LocationCode$"></asp:Label></font>
                            :<font color="red">
                        </td>
                        <td align="left">
                            <asp:TextBox ID="ctlLocationCode" SkinID="SkGeneralTextBox" Style="text-align: center;"
                                runat="server" MaxLength="4" />
                            <asp:Label ID="ctlLocationCodeLabelDisplay" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                            :<font color="red"></font>
                        </td>
                        <td align="left" class="style1">
                            <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                Text="" Width="235px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlDefaultPBLabel" runat="server" Text="Default PB"></asp:Label>
                            :<font color="red"></font>
                        </td>
                        <td align="left" class="style1">
                            <asp:DropDownList ID="ctlDefaultPBDropdown" runat="server" MaxLength="100" Width="235px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlActiveLabel" runat="server" Text="$Active$"></asp:Label>
                            :
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="ctlActive" runat="server" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlCheckBoxAllowImportExpenseLabel" runat="server" Text="$IsAllowisAllowImportExpense$"></asp:Label>
                            :
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="ctlCheckBoxAllowImportExpense" runat="server" Checked="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkCtlFormNewRow"
                            OnClick="ctlAdd_Click" />
                        <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCtlFormCancel"
                            OnClick="ctlCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <font color="red">
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Location.Error" />
                        </font>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="location" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="ctlLocationModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlLocationEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlLocationEditor" />
