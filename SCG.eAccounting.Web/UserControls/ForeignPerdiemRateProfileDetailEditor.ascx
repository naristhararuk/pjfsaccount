<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForeignPerdiemRateProfileDetailEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ForeignPerdiemRateProfileDetailEditor" %>
<asp:Panel ID="ctlForeignPerdiemRateProfileDetailEditor" runat="server" Style="display: none"
    CssClass="modalPopup" Width="750">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlForeignPerdiemRateProfileDetailEditorFormHeader" CssClass="table"
                    runat="server" Style="cursor: move; border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditForeignPerdiemRateProfileDetailHeader" runat="server" Text='$Header$'
                        SkinID="SkFieldCaptionLabel" Width="100%"></asp:Label></p>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="left">
                        <fieldset style="width: 90%" id="fdsForeignPerdiemRateProfileDetail">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlPersonalLevelLabel" Text="$Personal Level$:" SkinID="SkFieldCaptionLabel"
                                            runat="server"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlPersonalLevelDropdown" runat="server" MaxLength="100" Width="235px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlZoneLabel" Text="$Zone$:" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlZoneDropdown" runat="server" MaxLength="100" Width="235px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlOfficialRateLabel" Text="$Official Rate$:" SkinID="SkFieldCaptionLabel"
                                            runat="server"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlOfficialRate" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 18, 2));"
                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Text="" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlExtraRateLabel" Text="$Extra Rate$:" SkinID="SkFieldCaptionLabel"
                                            runat="server"></asp:Label>
                                        <font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlExtraRate" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 18, 2));"
                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Text="" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlInternationalStaffRateLabel" Text="$International Staff Rate$:"
                                            SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlInternationalStaffRate" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 18, 2));"
                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Text="" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlSCGStaffRateLabel" Text="$SCG Staff Rate$:" SkinID="SkFieldCaptionLabel"
                                            runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlSCGStaffRate" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 18, 2));" OnKeyDown="disablePasteOption();"
                                            OnKeyUp="disablePasteOption();" Text="" Width="150px" />
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
                                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ForeignPerdiemRateProfileDetail.Error" />
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
<ajaxToolkit:ModalPopupExtender ID="ctlForeignPerdiemRateProfileDetailEditorModalPopupExtender"
    runat="server" TargetControlID="lnkDummy" PopupControlID="ctlForeignPerdiemRateProfileDetailEditor"
    BackgroundCssClass="modalBackground" CancelControlID="lnkDummy" RepositionMode="None"
    PopupDragHandleControlID="ctlForeignPerdiemRateProfileEditorDetailFormHeader" />
