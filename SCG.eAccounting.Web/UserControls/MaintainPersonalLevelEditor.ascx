<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaintainPersonalLevelEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.MaintainPersonalLevelEditor" %>
<asp:Panel ID="ctlMaintainPersonalLevelEditor" runat="server" Style="display: none"
    CssClass="modalPopup" Width="500">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlMaintainPersonalLevelEditorFormHeader" CssClass="table" runat="server"
                    Style="cursor: move; border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditMaintainPersonalLevelHeader" runat="server" Text='$Header$'
                        SkinID="SkFieldCaptionLabel" Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

                <table width="100%">
                    <tr>
                        <td align="left">
                            <fieldset style="width: 90%" id="fdsMaintainPersonalLevel">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlPersonalLevelLabel" Text="$Personal Level$:" SkinID="SkFieldCaptionLabel"
                                                runat="server"></asp:Label>
                                            <font color="red">*</font>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlPersonalLevel" SkinID="SkCtlTextboxLeft" MaxLength="20" runat="server"
                                                Text="" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlDescriptionLabel" Text="$Description$:" SkinID="SkFieldCaptionLabel"
                                                runat="server"></asp:Label>
                                            <font color="red">*</font>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                                Text="" Width="250px" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlOrdinalLabel" Text="$Ordinal$:" SkinID="SkFieldCaptionLabel"
                                                runat="server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlOrdinal" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                                Text="" Width="150px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlActiveLabel" Text="$Active$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="ctlActive" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table width="100%" class="table">
                                <tr>
                                    <td align="left" style="width: 60%">
                                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkSaveButton" OnClick="ctlAdd_Click" />
                                        <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                                            OnClick="ctlCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <font color="red">
                                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="SuUserPersonalLevel.Error" />
                                        </font>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlMaintainPersonalLevelEditorModalPopupExtender"
    runat="server" TargetControlID="lnkDummy" PopupControlID="ctlMaintainPersonalLevelEditor"
    BackgroundCssClass="modalBackground" CancelControlID="lnkDummy" RepositionMode="None"
    PopupDragHandleControlID="ctlMaintainPersonalLevelEditorFormHeader" />
