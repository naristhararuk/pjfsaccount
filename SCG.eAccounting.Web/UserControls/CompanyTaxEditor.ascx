<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyTaxEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CompanyTaxEditor" %>
<%@ Register Src="LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc1" %>
<asp:Panel ID="ctlCompanyTaxEditor" runat="server" Style="display: none" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Panel ID="ctlCompanyTaxFormHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black" Width="100%">
                    <p><asp:Label ID="ctlCompanyTaxHeader" runat="server" SkinID="SkFieldCaptionLabel" Text="Add/Edit Company Tax Rate"
                        Width="100%"></asp:Label></p>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlCompanyTaxUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: block;" align="center">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td align="center">
                            <table class="table" width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlCompanyLabel" Text="Company" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlCompanyReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <uc1:CompanyField id="ctlCompanyField" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlRateLabel" Text="Rate" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlRateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlRate" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"
                                                                    Text='<%# Bind("Rate") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlRateNonDeductTextLabel" Text="Rate Non-Deduct" SkinID="SkGeneralLabel"
                                            runat="server"></asp:Label>
                                        &nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlRateNonDeduct" runat="server" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" SkinID="SkNumberTextBox"
                                                                    Text='<%# Bind("RateNonDeduct") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlUseParentRateLabel" runat="server" SkinID="SkGeneralLabel" Text="UseParentRate"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left" colspan="2" style="margin-left: 40px">
                                        <asp:CheckBox ID="ctlUseParentRate" AutoPostBack="true" OnCheckedChanged="ctlUseParentRate_CheckedChanged" runat="server" Checked='<%# Eval("UseParentRate") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlEditDisableLabel" runat="server" SkinID="SkGeneralLabel" Text="Disable"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left" colspan="2" style="margin-left: 40px">
                                        <asp:CheckBox ID="ctlDisable" runat="server" Checked='<%# Eval("Disable") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                            ToolTip='<%# GetProgramMessage("Save") %>' ValidationGroup="ValidateFormView"
                                            OnClick="ctlSave_Click1" Text="Update"></asp:ImageButton>
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Cancel") %>' OnClick="ctlCancel_Click" Text="Cancel">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <font color="red">
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="CompanyTax.Error" />
                            </font>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlCompanyTaxModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlCompanyTaxEditor" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    RepositionMode="None" />
