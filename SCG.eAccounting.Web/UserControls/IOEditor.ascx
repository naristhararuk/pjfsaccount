<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IOEditor.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.IOEditor" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc2" %>
<%@ Register Src="LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc3" %>
<asp:Panel ID="ctlIOEditor" runat="server" Style="display: block" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlIOFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlIOHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='$Header$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlIOUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="UpdatePanelPaymentMethodFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelPaymentMethodForm"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <asp:Panel ID="Panel1" CssClass="ProgressOverlay" runat="server">
                        <div class="ProgressContainer">
                            <div class="ProgressHeader">
                                <asp:Label ID="ctlUserFormLoading" runat="server" Text="$lblLoading$"></asp:Label>
                            </div>
                            <div class="ProgressBody">
                                <asp:ImageButton ID="ctlPaymentMethodFormLoadingImage" runat="server" SkinID="SkProgress" />
                            </div>
                        </div>
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <div style="display: block;" align="center">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td align="center">
                            <table class="table" width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlIONumberLabel" Text="$IO Number$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlIONumberReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlIONumber" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                            Text='<%# Bind("IONumber")%>' Width="250px" />
                                        <asp:Label ID="ctlIONumberLabelDisplay" Text='<%# Bind("IONumber")%>' SkinID="SkGeneralLabel"
                                            runat="server" Visible=false></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlIOTypeLabel" Text="$IO Type$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlIOTypeReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlIOType" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                            Text='<%# Bind("IOType") %>' Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlIOTextLabel" Text="$IO Text$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlIOTextReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlIOText" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                            Text='<%# Bind("IOText") %>' Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label3" Text="$Company Code$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="ctlCompanyReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left" colspan="2" style="margin-left: 40px">
                                        <asp:Label ID="ctlCompanyCode" runat="server" SkinID="SkGeneralLabel" />
                                        <asp:Label ID="ctlCompanyName" runat="server" SkinID="SkGeneralLabel" />
                                        <uc2:CompanyField ID="ctlCompanyField" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label4" Text="$Cost Center$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label6" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left" colspan="2" style="margin-left: 40px">
                                        <uc3:CostCenterField ID="ctlCostCenterField" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label1" Text="$Valid date$" SkinID="SkGeneralLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <uc1:Calendar ID="ctlCalEffectiveDate" runat="server" SkinID="SkCtlCalendar" DateValue='<%# Eval("EffectiveDate") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label2" Text="$Expire Date$" SkinID="SkGeneralLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <uc1:Calendar ID="ctlCalLastDisplayDate" runat="server" SkinID="SkCtlCalendar" DateValue='<%# Eval("ExpireDate") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label5" Text="$Active$" SkinID="SkGeneralLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlIOActive" runat="server" Checked='<%# Eval("Active") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlBusinessAreaLabel" Text="$Business Area$" SkinID="SkGeneralLabel"
                                            runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlBusinessArea" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="4"
                                            Text='<%# Bind("BusinessArea")%>' Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlProfitCenterLabel" Text="$Profit Center$" SkinID="SkGeneralLabel"
                                            runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlProfitCenter" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="10"
                                            Text='<%# Bind("ProfitCenter")%>' Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ctlInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                            OnClick="ctlInsert_Click1" SkinID="SkCtlFormSave" Text="Update" />
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
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="InternalOrder.Error" />
                            </font>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlIOModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlIOEditor" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    RepositionMode="None" PopupDragHandleControlID="ctlIOEditor" />
