<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HolidayEditor.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.HolidayEditor" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<asp:Panel ID="ctlHolidayEditor" runat="server" Style="display: block" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlHolidayFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlHolidayHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='$Header$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlHolidayUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: block;" align="center">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td align="center">
                        <asp:HiddenField ID="ctlHolidayProfileIDHidden" runat="server" />
                            <table class="table" width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlDateLabel" Text="$Date$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlDateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <uc1:Calendar ID="ctlDate" runat="server" SkinID="SkCtlCalendar" DateValue='<%# Eval("Date") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlDescriptionLabel" Text="$Description$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                       &nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                            Text='<%# Bind("Description") %>' Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ctlInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                            OnClick="ctlInsert_Click" SkinID="SkCtlFormSave" Text="Update" />
                                        <asp:ImageButton ID="ctlCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            OnClick="ctlCancel_Click" SkinID="SkCtlFormCancel" Text="Cancel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <font color="red">
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Holiday.Error" />
                            </font>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlHolidayModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlHolidayEditor" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    RepositionMode="None" PopupDragHandleControlID="ctlHolidayEditor" />

