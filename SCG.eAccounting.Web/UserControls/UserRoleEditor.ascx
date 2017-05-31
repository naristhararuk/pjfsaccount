<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.UserRoleEditor" EnableTheming="true" %>
<script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/global.js") %>"></script>
<asp:HiddenField ID="dummy" runat="server" />
<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender" runat="server" TargetControlID="dummy"
    PopupControlID="ctlPanel" BackgroundCssClass="modalBackground" CancelControlID=""
    RepositionMode="None" PopupDragHandleControlID="ctlFormPanelHeader">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="ctlPanel" CssClass="modalPopup" Style="display: none;">
    <asp:Panel ID="ctlFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="lblCapture" SkinID="SkCtlLabel" runat="server" Text="Add /Edit User Group"></asp:Label>
            </p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlPanelUserRole" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div runat="server" id="ctlDivAddEdit" align="left" style="width: 100%">
                <fieldset id="ctlFieldsetDetail" style="border-color: Black; border-width: thin">
                    <br />
                    <table width="100%" cellpadding="0" style="text-align: left" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Group Code $" ID="ctlLabelGroupCode" SkinID="SkCtlLabel"></asp:Label>
                                <font color="red">
                                    <asp:Label ID="ctlServiceTeamCodeRequired" runat="server" Text="*"></asp:Label></font>
                                :
                            </td>
                            <td>
                                <asp:TextBox MaxLength="20" runat="server" ID="ctlTextBoxGroupCode" SkinID="SkGeneralTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Group Name $" ID="ctlLabelGroupName" SkinID="SkCtlLabel"></asp:Label>
                                <font color="red">
                                    <asp:Label ID="Label1" runat="server" Text="*"></asp:Label></font> :
                            </td>
                            <td>
                                <asp:TextBox MaxLength="100" runat="server" ID="ctlTextBoxGroupName" SkinID="SkGeneralTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Active :$" ID="ctlLabelActive" SkinID="SkCtlLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" ID="ctlCheckboxActive" Text="$Active$" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="table">
                                    <%--<legend><b>Use Special Pay-In Slip</b></legend>--%>
                                    <table width="100%" border="0" class="table">
                                        <tr>
                                            <td align="left" style="width: 40%">
                                                <asp:Label ID="UseCustomizationLimitAmountLabel2" runat="server" SkinID="SkCtlLabel" Text='Use Customization Limit Amount'></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 60%">
                                                <asp:CheckBox ID="ctlUseCustomizationLimitAmount" runat="server" OnCheckedChanged="ctlUseCustomizationLimitAmount_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="ctlSpecialUseCustomizationLimitAmount" runat="server" Visible="false">
                                        <table width="100%" border="0" class="table">
                                            <tr>
                                                <td align="left" style="width: 40%">
                                                    <asp:Label ID="ctlLimitAmountForVerifierChangeLabel" runat="server" SkinID="SkCtlLabel" Text='Amount For Verifier Change'></asp:Label>
                                                    :
                                                </td>
                                                <td align="left" style="width: 60%">
                                                    <asp:TextBox ID="ctlLimitAmountForVerifierChange" MaxLength="20" SkinID="SkCtlTextboxLeft"
                                                        runat="server" OnKeyPress="return(currencyFormat(this, ',', '.', event, 16));"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="ctlLabelEmProcess" SkinID="SkCtlLabel" Text="$Employee Processing Team's Information :$"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <fieldset style="border-color: Black; border-width: thin; width: 95%">
                                    <br />
                                    <table width="100%" cellpadding="0" style="text-align: left" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" Text="$Receive Document$" ID="ctlCheckboxRecieveDoc" />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" Text="$Verify Document$" ID="ctlCheckboxVerifyDoc" />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" Text="$Approve Verify Document$"
                                                    ID="ctlCheckboxApproveVerifyDoc" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" Text="Allow Multiple Approve"
                                                    AutoPostBack="True" OnCheckedChanged="ctlCheckboxAllowMultipleApproveAccountant_Checked"
                                                    ID="ctlCheckboxAllowMultipleApproveAccountant" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelVerifyLimit" SkinID="SkCtlLabel" Text="$Verify Limit :$"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelApproveVerifyLimit" SkinID="SkCtlLabel" Text="$Approve Verify Limit :$"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelVerifyMin" SkinID="SkCtlLabel" Text="$Min :$ "></asp:Label>
                                                <asp:TextBox runat="server" ID="ctlTextBoxVerifyMin" MaxLength="14" SkinID="SkNumberTextBox"
                                                    OnKeyPress="return(currencyFormat(this, ',', '.', event, 16));">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelApproveVerifyMin" SkinID="SkCtlLabel" Text="$Min :$ "></asp:Label>
                                                <asp:TextBox runat="server" ID="ctlTextBoxApproveVerifyMin" MaxLength="14" SkinID="SkNumberTextBox"
                                                    OnKeyPress="return(currencyFormat(this, ',', '.', event, 16));"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelVerifyMax" SkinID="SkCtlLabel" Text="$Max :$ "></asp:Label>
                                                <asp:TextBox runat="server" ID="ctlTextBoxVerifyMax" MaxLength="16" SkinID="SkNumberTextBox"
                                                    OnKeyPress="return(currencyFormat(this, ',', '.', event, 16));"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="ctlLabelApproveVerifyMax" SkinID="SkCtlLabel" Text="$Max :$ "></asp:Label>
                                                <asp:TextBox runat="server" ID="ctlTextBoxApproveVerifyMax" MaxLength="16" SkinID="SkNumberTextBox"
                                                    OnKeyPress="return(currencyFormat(this, ',', '.', event, 16));"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="ctlLabelPayProcess" SkinID="SkCtlLabel" Text="$Payment Processing Team's Information :$"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <fieldset style="border-color: Black; border-width: thin; width: 95%">
                                    <br />
                                    <table width="100%" cellpadding="0" style="text-align: left" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ctlCheckBoxVerifyPayment" SkinID="SkCtlCheckBox" runat="server"
                                                    Text="$Verify Payment$" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ctlCheckBoxApproveVerifyPayment" SkinID="SkCtlCheckBox" runat="server"
                                                    Text="$Approve Verify Payment$" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ctlCheckBoxCounterCashier" SkinID="SkCtlCheckBox" runat="server"
                                                    Text="$Counter Cashier$" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ctlCheckBoxAllowMultipleApprovePayment" AutoPostBack="True" OnCheckedChanged="ctlCheckBoxAllowMultipleApprovePayment_Checked"
                                                    SkinID="SkCtlCheckBox" runat="server" Text="Allow Multiple Approve" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <br />
                </fieldset>
                <br />
                <asp:ImageButton ID="ctlButtonSave" SkinID="SkSaveButton" Text="$Save$" runat="server"
                    OnClick="ctlButtonSave_Click" />
                <asp:ImageButton ID="ctlButtonCancel" SkinID="SkCancelButton" Text="$Cancel$" runat="server"
                    OnClick="ctlButtonCancel_Click" />
                <br />
                <font color="red">
                    <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="Role.Error">
                    </spring:ValidationSummary>
                </font>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
