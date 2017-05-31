<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostCenterEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CostCenterEditor" EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc2" %>
<%@ Register Src="LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" TagName="CompanyTextboxAutoComplete"
    TagPrefix="uc1" %>
<asp:HiddenField ID="dummy" runat="server" />
<ajaxtoolkit:modalpopupextender id="ctlModalPopupExtender" runat="server" targetcontrolid="dummy"
    popupcontrolid="ctlPanel" backgroundcssclass="modalBackground" cancelcontrolid=""
    repositionmode="None" popupdraghandlecontrolid="ctlFormPanelHeader">
</ajaxtoolkit:modalpopupextender>
<asp:Panel runat="server" ID="ctlPanel" CssClass="modalPopup" Style="display: none;">
    <asp:Panel ID="ctlFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="lblCapture" SkinID="SkCtlLabel" runat="server" Text="Add /Edit Cost Center"></asp:Label>
            </p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <br />
            <div runat="server" id="ctlDivAddEdit" align="left" style="width: 100%">
                <fieldset id="ctlFieldsetDetail" style="border-color: Black; border-width: thin">
                    <br />
                    <table width="100%" cellpadding="0" style="text-align: left" cellspacing="0">
                        <tr>
                            <td width="200">
                                <asp:Label runat="server" Text="$Cost Center$" ID="ctlLabelCostCenterCode" SkinID="SkCtlLabel"></asp:Label>
                                <asp:Label ID="Label6" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <asp:TextBox MaxLength="20" runat="server" ID="ctlTextBoxCostCenterCode" SkinID="SkGeneralTextBox"></asp:TextBox>
                                <asp:Label runat="server" Text="" ID="ctlCostCenterCodeLabel" Visible="false" SkinID="SkCtlLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Decription$" ID="ctlLabelDescription" SkinID="SkCtlLabel"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <asp:TextBox MaxLength="100" runat="server" ID="ctlTextBoxDescription" SkinID="SkGeneralTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Company Code$" ID="ctlLabelCompanyCode" SkinID="SkCtlLabel"></asp:Label>
                                <asp:Label ID="Label1" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <uc1:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Valid Date$" ID="ctlLabelValid" SkinID="SkCtlLabel"></asp:Label>
                                <asp:Label ID="Label2" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <uc2:Calendar ID="ctlCalendarValid" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Expire Date$" ID="ctlLabelExpire" SkinID="SkCtlLabel"></asp:Label>
                                <asp:Label ID="Label3" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <uc2:Calendar ID="ctlCalendarExpire" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="$Lock$" ID="ctlLabelLock" SkinID="SkCtlLabel"></asp:Label>&nbsp:&nbsp                                
                            </td>
                            <td>
                                <asp:CheckBox runat="server" Text="Actual Primary Costs" ID="ctlChkLock" SkinID="SkGeneralCheckBox" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Active/Inactive" ID="ctlLabelActive" SkinID="SkCtlLabel"></asp:Label>&nbsp:&nbsp                                
                            </td>
                            <td>
                                <asp:CheckBox runat="server" Text="Active" ID="ctlChkActive" SkinID="SkGeneralCheckBox" />
                            </td>
                        </tr>
                        <tr>
                            <td width="200">
                                <asp:Label runat="server" Text="$Business Area$" ID="ctlBusinessAreaLabel" SkinID="SkCtlLabel"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <asp:TextBox MaxLength="4" runat="server" ID="ctlBusinessArea" SkinID="SkGeneralTextBox"></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td width="200">
                                <asp:Label runat="server" Text="$Profit Center$" ID="ctlProfitCenterLabel" SkinID="SkCtlLabel"></asp:Label>&nbsp:&nbsp
                            </td>
                            <td>
                                <asp:TextBox MaxLength="10" runat="server" ID="ctlProfitCenter" SkinID="SkGeneralTextBox"></asp:TextBox>
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
                    <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="CostCenter.Error">
                    </spring:ValidationSummary>
                </font>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Panel>
