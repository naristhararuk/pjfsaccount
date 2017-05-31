<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mileage.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Mileage" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/InvoiceForm.ascx" TagName="InvoiceForm" TagPrefix="uc2" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="~/UserControls/Shared/PopupCaller.ascx" tagname="PopupCaller" tagprefix="uc5" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField"
    TagPrefix="uc10" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/IOAutoCompleteLookup.ascx" TagName="IOAutoCompleteLookup"
    TagPrefix="uc7" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/AccountField.ascx" TagName="AccountField"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/LOV/SCG.eAccounting/CADocumentLookup.ascx" TagName="CALookup"
    TagPrefix="uc9" %>
<%--<asp:LinkButton ID="lnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlPanelMileagePopup" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlPanelMileageHeader" />--%>
<asp:Panel ID="ctlPanelMileagePopup" runat="server" BackColor="White">
    <asp:Panel ID="ctlPanelMileageHeader" CssClass="table" runat="server" Style="cursor: move; border: solid 1px Gray; color: Black">
        <div>
            <p><asp:Label ID="ctlLabelMileageCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="ctlContent" runat="server">
        <table border="0" style="width: 98%" class="table" align="center">
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelMileageData" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdatePanelMileageDataProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMileageData"
                                DynamicLayout="true" EnableViewState="true">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <fieldset id="ctlFieldSetMileageData" class="table">
                                <table border="0" class="table" >
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left" colspan="4">
                                        <asp:Panel ID="ctlPanelCostcenterExpenseCode" runat="server">
                                            <table>
                                                <tr>
                                                    <td align="left" style="width: 150px;">
                                                        <asp:Label ID="ctlCostCenterLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="Cost Center"></asp:Label>
                                                        <asp:Label ID="ctlCostCenterReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <uc6:CostCenterField ID="ctlCostCenterField" runat="server" OnOnObjectLookUpReturn="ctlCostCenterField_OnObjectLookUpReturn" />
                                                        <ss:LabelExtender ID="ctlCostCenterFieldExtender" runat="server" LinkControlID="ctlCostCenterField"
                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.CostCenter %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="ctlAccountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="Expense Code"></asp:Label>
                                                        <asp:Label ID="ctlAccountReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <uc8:AccountField ID="ctlAccountField" runat="server" />
                                                        <ss:LabelExtender ID="ctlAccountFieldLabelExtender1" runat="server" LinkControlID="ctlAccountField"
                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.AccountCode %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="ctlInternalOrderLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                            Text="Internal Order"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <uc7:IOAutoCompleteLookup ID="ctlIOAutoCompleteLookup" runat="server" />
                                                        <ss:LabelExtender ID="ctlIOAutoCompleteLookupLabelExtender2" runat="server" LinkControlID="ctlIOAutoCompleteLookup"
                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InternalOrder %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left" style="width: 100px;">
                                            <asp:Label ID="ctlLabelOwner" runat="server" Text="$Owner$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <asp:Label ID="ctlLabelOwnerStar" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td align="left" style="width: 150px;">
                                            <asp:DropDownList ID="ctlDropDownListOwner" runat="server" Width="152px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ctlDropDownListOwner_SelectedIndexChanged" SkinID="SkGeneralDropdown">
                                            </asp:DropDownList>
                                            <ss:LabelExtender ID="ctlDropDownListOwnerExtender" runat="server" LinkControlID="ctlDropDownListOwner"
                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>
                                        </td>
                                        <td style="width: 100px;">
                                            &nbsp;
                                        </td>
                                        <td align="left" style="width: 150px;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left" >
                                            <asp:Label ID="ctlLabelTypeOfCar" runat="server" Text="$Type of Car$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <asp:Label ID="ctlLabelTypeOfCarStar" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td align="left" >
                                            <asp:DropDownList ID="ctlDropDownListTypeOfCar" runat="server" Width="152px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ctlDropDownListTypeOfCar_SelectedIndexChanged" SkinID="SkGeneralDropdown">
                                            </asp:DropDownList>
                                            <ss:LabelExtender ID="ctlDropDownListTypeOfCarLabelExtender" runat="server" LinkControlID="ctlDropDownListTypeOfCar"
                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left" >
                                            <asp:Label ID="ctlLabelCarLicenseNo" runat="server" Text="$Car License No.$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <asp:Label ID="ctlLabelCarLicenseNoStar" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td align="left" >
                                            <asp:TextBox ID="ctlTextBoxCarLicenseNo" runat="server" SkinID="SkCtlTextboxCenter"
                                                Width="152px" MaxLength="10" OnTextChanged="ctlCarLicenseNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlTextBoxCarLicenseNoLabelExtender" runat="server" LinkControlID="ctlTextBoxCarLicenseNo"
                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="ctlLabelPermissionNo" runat="server" Text="$Permission No.$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTextBoxPermissionNo" runat="server" Width="152px" MaxLength="20"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlTextBoxPermissionNoLabelExtender" runat="server" LinkControlID="ctlTextBoxPermissionNo"
                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="left" >
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <asp:UpdatePanel ID="ctlUpdatePanelEmployeeDetail" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="divMileageSubDataForCompany" runat="server">
                                                  <tr>
                                                                <td align="left" style="width: 5%">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 100px;">
                                                                    <asp:Label ID="ctlLabelTotalAmount" runat="server" Text="$Total Amount$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                    <asp:Label ID="ctlLabelTotalAmountStar" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                                </td>
                                                                <td align="left" style="width: 150px;">
                                                                    <asp:TextBox ID="ctlTextBoxTotalAmount" runat="server" SkinID="SkNumberTextBox" Width="152px"
                                                                        MaxLength="8" Style="text-align: right;" OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                                                                        OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                    <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                                        TargetControlID="ctlTextBoxTotalAmount" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                    <ss:LabelExtender ID="ctlTextBoxTotalAmountLabelExtender" runat="server" LinkControlID="ctlTextBoxTotalAmount"
                                                                        InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                    </ss:LabelExtender>
                                                                </td>
                                                                <td style="width: 100px;">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 150px;">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                            </div>
                                            <div id="divEmployeeDetail" runat="server">
                                                                                    <!-- MileageRateRivition -->
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Information" runat="server" Text="$Current information$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            &nbsp;:&nbsp;
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:CheckBox ID="ctlSelectChangeMileageRate" runat="server" OnCheckedChanged="ctlSelectChangeMileageRate_CheckedChanged"
                                                AutoPostBack="true" />
                                            <ss:LabelExtender ID="LabelExtender90" runat="server" LinkControlID="ctlSelectChangeMileageRate"
                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            &nbsp;
                                        </td>
                                        <td colspan="4" >
                                            <table>
                                                <tr>
                                                    <td align="left" width="100px">
                                                        <asp:Label ID="companyLabel" runat="server" Text="Company" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                        <asp:HiddenField ID="defaultcompanyId" runat="server" />
                                                        &nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" width="200px">
                                                        <asp:TextBox ID="companyText" runat="server" SkinID="SkNumberTextBox" Width="200px"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <ss:LabelExtender ID="LabelExtender1" runat="server" LinkControlID="companyText" Width="200px"
                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left" style="width: 100px">
                                                        <div id="overrideMileageRate" runat="server">
                                                            <asp:Label ID="ChangecompanyLabel" runat="server" Text="CompanySelector" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                            <asp:Label ID="reqCompany" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                        </div>
                                                    </td>
                                                    <td align="left" style="width: 250px">
                                                        <div id="DivCompanyField" runat="server">
                                                            <uc10:CompanyField ID="ctlCompanyField" runat="server" HideCompanyName="true" />
                                                            <ss:LabelExtender ID="LabelExtender2" runat="server" LinkControlID="ctlCompanyField" Width="200px"
                                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                            </ss:LabelExtender>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="PersonalLevelText" runat="server" Text="Personal Level" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                        &nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" style="width: 5%">
                                                        <asp:TextBox ID="ctlPersonalLevelGroup" runat="server" SkinID="SkNumberTextBox" Width="152px"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <ss:LabelExtender ID="LabelExtender3" runat="server" LinkControlID="ctlPersonalLevelGroup"
                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <div id="DivSelectPersonalLevelGroup" runat="server">
                                                            <asp:Label ID="SelectPersonalLevelGroup" runat="server" Text="SelectorPersonalLevel"
                                                                SkinID="SkFieldCaptionLabel"></asp:Label>
                                                            <asp:Label ID="reqPersonalLevel" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <div id="DivDropdownListPersonal" runat="server">
                                                            <asp:DropDownList ID="ctlMileagRateRivitionDropDownList" runat="server" Width="152px"
                                                                AutoPostBack="True" SkinID="SkGeneralDropdown">
                                                            </asp:DropDownList>
                                                            <ss:LabelExtender ID="LabelExtender4" runat="server" LinkControlID="ctlMileagRateRivitionDropDownList"
                                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                            </ss:LabelExtender>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <div id="DivObjectiveValue" runat="server">
                                                            <asp:Label ID="LabelNote" runat="server" Text="Objective" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                            <asp:Label ID="reqObjective" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                        </div>
                                                    </td>
                                                    <td align="left" style="width: 5%">
                                                        <div id="DivctlObjectiveValue" runat="server">
                                                            <asp:TextBox ID="ctlObjectiveValue" runat="server" SkinID="SkGeneralTextBox" Width="220px"
                                                                Height="50px" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 300);"
                                                                Style="margin-left: 1px" onkeyup="return IsMaxLength(this, 300);"></asp:TextBox>
                                                            <ss:LabelExtender ID="LabelExtender5" runat="server" LinkControlID="ctlObjectiveValue"
                                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                            </ss:LabelExtender>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>




                                    <!-- MileageRateRivition -->
                                    <tr>
                                        <td align="center" style="width: 100%" colspan="5">
                                            <asp:UpdatePanel ID="ctlUpdatePanelMileageSubData" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:UpdateProgress ID="MileageSubDataUpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMileageSubData"
                                                        DynamicLayout="true" EnableViewState="true">
                                                        <ProgressTemplate>
                                                            <uc4:SCGLoading ID="SCGLoading2" runat="server" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                    <asp:Panel ID="ctlPanelMileageSubDataForEmployee" runat="server" BackColor="White"
                                                        Width="100%">
                                                        <table border="0" class="table" style="width: 100%">
                                                            <tr>
                                                                <td align="left" style="width: 5%">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 35%" colspan="3">
                                                                    <asp:Label ID="ctlLabelAsDeterminedRate" runat="server" Text="$As Determined Rate -$"
                                                                        SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                    &nbsp;:&nbsp;
                                                                </td>
                                                                <td align="left" style="width: 5%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" style="width: 100%" colspan="5">
                                                                    <table border="0" class="table" style="width: 100%">
                                                                        <tr>
                                                                            <td align="left" style="width: 10%">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 40%">
                                                                                <asp:Label ID="ctlLabelKmRate" runat="server" Text="" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                                &nbsp;:&nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 35%">
                                                                                <asp:TextBox ID="ctlTextBoxKmRate" runat="server" SkinID="SkNumberTextBox" Width="152px" 
                                                                                    MaxLength="8" Style="text-align: right;background-color: Silver;" OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));"
																					OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                                                    TargetControlID="ctlTextBoxKmRate" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                                <ss:LabelExtender ID="ctlTextBoxKmRateLabelExtender" runat="server" LinkControlID="ctlTextBoxKmRate"
                                                                                    InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                                </ss:LabelExtender>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 5%">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 10%">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 40%">
                                                                                <asp:Label ID="ctlLabelExceedRate" runat="server" Text=""
                                                                                    SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                                &nbsp;:&nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 35%">
                                                                                <asp:TextBox ID="ctlTextBoxExceedingRate" runat="server" SkinID="SkNumberTextBox"
                                                                                    Width="152px" MaxLength="8" Style="text-align: right;background-color: Silver;" OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));"
																					OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                                    TargetControlID="ctlTextBoxExceedingRate" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                                <ss:LabelExtender ID="ctlTextBoxExceedingRateLabelExtender" runat="server" LinkControlID="ctlTextBoxExceedingRate"
                                                                                    InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                                </ss:LabelExtender>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td align="left" style="width: 5%">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <%--<asp:Panel ID="ctlPanelMileageSubDataForCompany" runat="server" Width="100%" BorderWidth="0">
                                                        <table border="0" cellspacing="0" class="table" style="width:100%">
                                                            
                                                        </table>
                                                    </asp:Panel>--%>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ctlDropDownListTypeOfCar" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListTypeOfCar" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelExpenseGeneral" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelExpenseGeneral"
                                DynamicLayout="true" EnableViewState="False">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading8" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:HiddenField ID="ctlType" runat="server" />
                            <fieldset id="ctlCAField" runat="server" class="table">
                                <legend id="CAHearderLine" style="color: #4E9DDF">
                                    <asp:Label ID="ctlCAHearder" runat="server" Text="$HEADER$" />
                                </legend>
                            <table border="0" id="ctlExpenseGeneral" width="100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ctlAddExpenseMPA" runat="server" SkinID="SkAddButton" OnClick="ctlAddExpenseCA_Click" />
                                        <%-- OnClick="ctlctlAddExpenseMPA_Click"--%>
                                        <uc9:CALookup ID="ctlCADocumentLookup" runat="server" isMultiple="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <ss:BaseGridView ID="ctlExpeseCAGridView" runat="server" DataKeyNames="CADocumentID , WorkflowID"
                                            EnableInsert="False" InsertRowCount="1" ShowMsgDataNotFound="false" SaveButtonID=""
                                            AutoGenerateColumns="false" Width="100%" CssClass="Grid" OnRowDataBound="ctlExpeseCAGridView_DataBound"
                                            ShowHeaderWhenEmpty="true" OnRowCommand="ctlctlAddExpenseCA_RowCommand">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="No.">
                                                    <ItemTemplate>
                                                        <asp:Literal Mode="Encode" ID="ctlNoLabel" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CA No.">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlGridDocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>'
                                                            CommandName="PopupDocument"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ctlGridDescription" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Request Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ctlGridDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" ToolTip="Delete"
                                                            CommandName="DeleteExpensesCA" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </ss:BaseGridView>
                                    </td>
                                </tr>
                            </table>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelMileageItemData" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgressMileageItemData" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMileageItemData"
                                DynamicLayout="true" EnableViewState="true">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading3" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Panel ID="ctlPanelMileageItemData" CssClass="table" runat="server">
                                <table width="100%" border="0" class="table" align="left">
                                    <tr>
                                        <td>
                                            <fieldset id="ctlFieldSetModeManage" class="table" runat="server">
                                                <%--style="width:90%"--%>
                                                <table border="0" class="table">
                                                    <tr>
                                                        <td>
                                                            <table width="700px" border="0" class="table">
                                                                <tr>
                                                                    <td align="center" style="width: 10%">
                                                                        <asp:Label ID="ctlLabelDate" runat="server" Text="$Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        <asp:Label ID="Label2" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 25%">
                                                                        <asp:Label ID="ctlLabelLocationFrom" runat="server" Text="$Location From$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        <asp:Label ID="Label3" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 25%">
                                                                        <asp:Label ID="ctlLabelLocationTo" runat="server" Text="$Location To$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        <asp:Label ID="Label4" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 20%">
                                                                        <asp:Label ID="ctlLabelCarMeterStart" runat="server" Text="$Car Meter - Start$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        <asp:Label ID="Label5" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 20%">
                                                                        <asp:Label ID="ctlLabelCarMeterEnd" runat="server" Text="$Car Meter - End$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        <asp:Label ID="Label6" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" style="width: 10%">
                                                                        <uc1:Calendar ID="ctlCalendarDate" runat="server" />
                                                                        <ss:LabelExtender ID="ctlCalendarDateToLabelExtender" runat="server" LinkControlID="ctlCalendarDate"
                                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td align="center" style="width: 25%">
                                                                        <asp:TextBox ID="ctlTextBoxLocationFrom" runat="server" Width="95%" MaxLength="100"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlTextBoxLocationFromLabelExtender" runat="server" LinkControlID="ctlTextBoxLocationFrom"
                                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td align="center" style="width: 25%">
                                                                        <asp:TextBox ID="ctlTextBoxLocationTo" runat="server" Width="95%" MaxLength="100"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlTextBoxLocationToLabelExtender" runat="server" LinkControlID="ctlTextBoxLocationTo"
                                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td align="center" style="width: 20%">
                                                                        <asp:TextBox ID="ctlTextBoxCarMeterStart" runat="server" MaxLength="6" SkinID="SkNumberTextBox"
                                                                            Style="text-align: right; width: 95%" />
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                                                            TargetControlID="ctlTextBoxCarMeterStart" FilterType="Custom, Numbers" />
                                                                        <ss:LabelExtender ID="ctlTextBoxCarMeterStartLabelExtender" runat="server" LinkControlID="ctlTextBoxCarMeterStart"
                                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td align="center" style="width: 20%">
                                                                        <asp:TextBox ID="ctlTextBoxCarMeterEnd" runat="server" MaxLength="6" SkinID="SkNumberTextBox"
                                                                            Style="text-align: right; width: 95%" />
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                            TargetControlID="ctlTextBoxCarMeterEnd" FilterType="Custom, Numbers" />
                                                                        <ss:LabelExtender ID="ctlTextBoxCarMeterEndLabelExtender" runat="server" LinkControlID="ctlTextBoxCarMeterEnd"
                                                                            InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="ctlPanelAdjustedForCompany" CssClass="table" runat="server" Width="120px">
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Label ID="ctlLabelAdjust" runat="server" Text="$Adjusted (k.m.)$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:TextBox ID="ctlTextBoxAdjust" runat="server" MaxLength="21" SkinID="SkNumberTextBox"
                                                                                Style="text-align: right; width: 95%" OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                                                TargetControlID="ctlTextBoxAdjust" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                            <ss:LabelExtender ID="ctlTextBoxAdjustLabelExtender" runat="server" LinkControlID="ctlTextBoxAdjust"
                                                                                InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                                            </ss:LabelExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ctlAddItem" runat="server" SkinID="SkAddButton" CausesValidation="True"
                                                ToolTip="$Add$" CommandName="AddItem" Text="&Add&" OnClick="ctlAddItem_Click">
                                            </asp:ImageButton>
                                            <asp:ImageButton ID="ctlUpdateItem" runat="server" SkinID="SkSaveButton" CausesValidation="True"
                                                ToolTip="$Update$" CommandName="UpdateItem" Text="&Update&" OnClick="ctlUpdateItem_Click">
                                            </asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancelItem" runat="server" SkinID="SkCancelButton" CausesValidation="False"
                                                ToolTip="$Cancel$" CommandName="CancelItem" Text="&Cancel&" OnClick="ctlCancelItem_Click">
                                            </asp:ImageButton>
                                            <asp:HiddenField ID="ctlHiddenFieldMileageItemId" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <font color="red">
                                                <spring:ValidationSummary ID="ctlValidationSummaryAddItem" runat="server" Provider="MileageItem.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ctlMileageGrid" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress3" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                                    DynamicLayout="true" EnableViewState="true">
                                    <ProgressTemplate>
                                        <uc4:SCGLoading ID="SCGLoading4" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <ss:BaseGridView ID="ctlMileageGrid" Width="100%" runat="server" ShowFooter="true"
                                    AllowSorting="true" AutoGenerateColumns="false" CssClass="table" DataKeyNames="ExpenseMileageItemID"
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="GridHeader" OnRowCommand="ctlMileageGrid_RowCommand"
                                    OnRowDataBound="ctlMileageGrid_RowDataBound" OnDataBound="ctlMileageGrid_DataBound">
                                    <AlternatingRowStyle CssClass="GridItem" />
                                    <RowStyle CssClass="GridAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" SortExpression="TravelDate">
                                            <ItemTemplate>
                                                <asp:Literal Mode="Encode" ID="ctlLabelTravelDateItem" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("TravelDate").ToString()) %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location From" SortExpression="LocationFrom">
                                            <ItemTemplate>
                                                <asp:Literal Mode="Encode" ID="ctlLabelLocationFromItem" runat="server" Text='<%# Bind("LocationFrom") %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location To" SortExpression="LocationTo">
                                            <ItemTemplate>
                                                <asp:Literal Mode="Encode" ID="ctlLabelLocationToItem" runat="server" Text='<%# Bind("LocationTo") %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Car Meter" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td colspan="2" style="text-align: center;">
                                                            <asp:Label ID="ctlMileageCarMeterGridHeader" runat="server" Text='Car Meter'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; width: 50%">
                                                            <asp:Label ID="ctlCarMeterStartHeader" runat="server" Text='Start'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center; width: 50%">
                                                            <asp:Label ID="ctlCarMeterEndHeader" runat="server" Text='End'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td style="text-align: right; width: 50%">
                                                            <asp:Literal Mode="Encode" ID="ctlCarMeterStartItem" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CarMeterStart", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 50%">
                                                            <asp:Literal Mode="Encode" ID="ctlCarMeterEndItem" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CarMeterEnd", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="ctlLabelFooter" runat="server" Text='<%# GetProgramMessage("Total") %>'
                                                    SkinID="SkFieldCaptionLabel"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Distance (k.m./day)" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <table width="100%" class="table" style="border-style: solid; border-width: 1px;
                                                    border-color: #FFFFFF">
                                                    <tr>
                                                        <td colspan="3" style="text-align: center;">
                                                            <asp:Label ID="ctlLabelHeaderDistanceForEmployee" runat="server" Text='Distance (k.m./day)'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; width: 30%">
                                                            <asp:Label ID="ctlLabelHeaderDistanceTotalForEmployee" runat="server" Text='Total'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center; width: 30%">
                                                            <asp:Label ID="ctlLabelHeaderDistanceFirst100KmForEmployee" runat="server" Text='1-100 km.'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center; width: 40%">
                                                            <asp:Label ID="ctlLabelHeaderDistanceExceed100KmForEmployee" runat="server" Text='Exceeding 100 km.'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td style="text-align: right; width: 30%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemDistanceTotalForEmployee" runat="server"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DistanceTotal", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 30%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemDistanceFirst100KmForEmployee" runat="server"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DistanceFirst100Km", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 40%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemDistanceExceed100KmForEmployee" runat="server"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DistanceExceed100Km", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td style="text-align: right; width: 30%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterDistanceTotalForEmployee" runat="server"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 30%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterDistanceFirst100KmForEmployee" runat="server"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 40%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterDistanceExceed100KmForEmployee" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Distance (k.m./day)" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td colspan="3" style="text-align: center;">
                                                            <asp:Label ID="ctlLabelHeaderDistanceForCompany" runat="server" Text='Distance (k.m./day)'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; width: 33%">
                                                            <asp:Label ID="ctlLabelHeaderDistanceTotalForCompany" runat="server" Text='Total'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center; width: 33%">
                                                            <asp:Label ID="ctlLabelHeaderAdjustedForCompany" runat="server" Text='Adjusted'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center; width: 33%">
                                                            <asp:Label ID="ctlLabelHeaderNetForCompany" runat="server" Text='Net'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemDistanceTotalForCompany" runat="server"
                                                                SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "DistanceTotal", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemAdjustedForCompany" runat="server" SkinID="SkNumberLabel"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DistanceAdjust", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelItemNetForCompany" runat="server" SkinID="SkNumberLabel"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DistanceNet", "{0:#,##0}") %>'></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterDistanceTotalForCompany" runat="server"
                                                                SkinID="SkNumberLabel"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterAdjustedForCompany" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: right; width: 33%">
                                                            <asp:Literal Mode="Encode" ID="ctlLabelFooterNetForCompany" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" CommandName="EditItem" />
                                                <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CommandName="DeleteItem" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="180px" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </ss:BaseGridView>
                            </center>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ctlAddItem" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlUpdateItem" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlCancelItem" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelCalculate" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgressCalculate" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCalculate"
                                DynamicLayout="true" EnableViewState="true">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading5" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Panel ID="ctlPanelCalculateForEmployee" CssClass="table" runat="server" Width="80%">
                                <br />
                                <br />
                                <table border="0" class="table" width="100%" align="center">
                                    <tr>
                                        <td align="center" valign="middle" colspan="2">
                                           <div width="80%" align="center" style="background-color: #baddfb; display:none">
                                                <asp:Label ID="ctlLabelCalculateTotalAmountSummary" runat="server" Width="95%" Height="20px"
                                                    Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                    BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                <asp:Label ID="ctlLabelCalculateAllowanceSummary" runat="server" Width="95%" Height="20px"
                                                    Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                    BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                <asp:Label ID="ctlLabelCalculateOverAllowanceSummary" runat="server" Width="95%"
                                                    Height="20px" Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                    BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                            </div>
                                            <div width="90%" align="center" style="background-color: #baddfb">
                                                <table border="0" class="table" width="90%" align="center">
                                                 <tr>
                                                        <td style="width: 50%" align="left"></td>
                                                        <td style="width: 14%" align="center">
                                                            <asp:Label ID="ctlTotalDistance" runat="server" SkinID="SkFieldCaptionLabel" Text="$TotalDistance$" />
                                                        </td>
                                                        <td style="width: 14%" align="center">
                                                            <asp:Label ID="ctlRateLabelHeader" runat="server" SkinID="SkFieldCaptionLabel" Text="$Rate$" />
                                                        </td>
                                                        <td style="width: 13%" align="center">
                                                            <asp:Label ID="ctlAmountTHBLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$AmountTHB$" />
                                                        </td>
                                                </tr>
                                                 <tr>
                                                        <td style="width: 50%" align="left">
                                                            <asp:Label ID="ctlDistanceFirst100Km" runat="server" Text="$DistanceFirst100Km$"
                                                                SkinID="SkFieldCaptionLabel" />
                                                        </td>
                                                     <td style="width:14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateDistanceFirst100Km" runat="server" Width="100%" Height="20px"
                                                                Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label> 
                                                     </td>
                                                     <td style="width:14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateFirst100KmRateForDistanceFirst100Km" runat="server"
                                                                Width="100%" Height="20px" Style="text-align: right; background-color: Silver;
                                                                vertical-align: middle;" BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label> 
                                                     </td>
                                                     <td style="width:13%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateAmountForDistanceFirst100Km" runat="server" Width="100%"
                                                                Height="20px" Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                        <td style="width: 50%" align="left">
                                                            <asp:Label ID="ctlDistanceExceed100Km" runat="server" Text="$DistanceExceed100Km$"
                                                                SkinID="SkFieldCaptionLabel" />
                                                        </td>
                                                     <td style="width:14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateDistanceExceed" runat="server" Width="100%" Height="20px"
                                                                Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                     </td>
                                                     <td style="width:14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateExceedRateForDistanceExceed" runat="server" Width="100%"
                                                                Height="20px" Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                     </td>
                                                     <td style="width:13%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateAmountForDistanceExceed" runat="server" Width="100%"
                                                                Height="20px" Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                        <td style="width: 50%" align="left">
                                                            <asp:Label ID="ctlMileageAllowanceForEmployeeLabel" runat="server" Text="$MileageAllowanceForEmployees$"
                                                                SkinID="SkFieldCaptionLabel" />
                                                        </td>
                                                     	<td style="width:14%" align="right"></td>
                                                        <td style="width: 14%" align="right"></td>
                                                        <td style="width: 14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateTotalAmount" runat="server" Width="98%" Height="20px"
                                                                Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                        </td>
                                                 </tr>
                                                 <tr>
                                                        <td style="width: 50%" align="left">
                                                            <asp:Label ID="ctlLabelff" runat="server" Text="$LessMileage$" SkinID="SkFieldCaptionLabel" />
                                                        </td>
                                                     <td style="width:14%" align="right"></td>
                                                        <td style="width: 14%" align="right"></td>
                                                        <td style="width: 14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateAllowance" runat="server" Width="98%" Height="20px"
                                                                Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                     </td>
                                                  </tr>
                                                 <tr>
                                                        <td style="width: 50%">
                                                            <asp:Label ID="Label14" runat="server" Text="$Allowance$" SkinID="SkFieldCaptionLabel" />
                                                        </td>
                                                     <td style="width:14%" align="right"></td>
                                                        <td style="width: 14%" align="right"></td>
                                                        <td style="width: 14%" align="right">
                                                        <asp:Label ID="ctlLabelCalculateOverAllowance" runat="server" Width="98%" Height="20px"
                                                                Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                                BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                                      </td>
                                                   </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="ctlPanelCalculateForCompany" CssClass="table" runat="server" Width="30%">
                                <br></br>
                                <div align="left" style="background-color: #baddfb">
                                    <table border="0" class="table" width="90%" cellspacing="0" cellpadding="0" align="center">
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="70%">
                                                <asp:Label ID="ctlLabelReimburseMentAmountName" runat="server" Text="$จำนวนเงินที่สามารถเบิกได้$"
                                                    SkinID="SkFieldCaptionLabel"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="ctlLabelReimBurseMentAmount" runat="server" Width="95%" Height="20px"
                                                    Style="text-align: right; background-color: Silver; vertical-align: middle;"
                                                    BorderStyle="Solid" BorderWidth="1px" SkinID="SkNumberLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlCalculateForEmployee" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlCalculateForCompany" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ctlMileageGrid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="ctlMileageGrid" EventName="RowDataBound" />
                            <asp:AsyncPostBackTrigger ControlID="ctlAddCommit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlEditCommit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="ctlUpdatePanelButtonControl" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgressButtonControl" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelButtonControl"
                                DynamicLayout="true" EnableViewState="true">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading6" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <table border="0" class="table" width="100%" cellspacing="0" cellpadding="0" align="center">
                                <tr>
                                    <td align="left" width="5%">
                                        <%--<asp:ImageButton ID="ctlDeclareVAT" runat="server" SkinID="SkAddButton" ToolTip="$Declare VAT$"
                                            OnClick="ctlDeclareVAT_Click" />--%>
                                    </td>
                                    <td align="center" width="95%">
                                        <asp:ImageButton ID="ctlCalculateForEmployee" runat="server" SkinID="SkCalculateButton"
                                            Text="$Calculate$" ToolTip="$Calculate$" OnClick="ctlCalculateForEmployee_Click" />
                                        <asp:ImageButton ID="ctlCalculateForCompany" runat="server" SkinID="SkCalculateButton"
                                            Text="$Calculate$" ToolTip="$Calculate$" OnClick="ctlCalculateForCompany_Click" />
                                        <asp:ImageButton ID="ctlAddCommit" runat="server" SkinID="SkSaveButton" ToolTip="$Save$"
                                            OnClick="ctlAddCommit_Click" />
                                        <asp:ImageButton ID="ctlEditCommit" runat="server" SkinID="SkSaveButton" ToolTip="$Save$"
                                            OnClick="ctlEditCommit_Click" />
                                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" ToolTip="$Delete$"
                                            OnClick="ctlDelete_Click" />
                                        <asp:ImageButton ID="ctlPopupCancel" runat="server" SkinID="SkCancelButton" ToolTip="$Cancel$"
                                            OnClick="ctlPopupCancel_Click" />
                                        <asp:ImageButton ID="ctlViewClose" runat="server" SkinID="SkCancelButton" ToolTip="$Close$"
                                            OnClick="ctlViewClose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="ctlUpdatePanelBottom" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <font color="red">
                                <spring:ValidationSummary ID="ctlValidationSummaryCommit" runat="server" Provider="Mileage.Error" />
                            </font>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlAddCommit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlEditCommit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="ctlUpdatePanelGridInvoice" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="ctlUpdateProgressGridInvoice" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridInvoice"
                                DynamicLayout="true" EnableViewState="true">
                                <ProgressTemplate>
                                    <uc4:SCGLoading ID="SCGLoading7" runat="server" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Panel ID="ctlPanelGridInvoice" CssClass="table" runat="server" Width="60%">
                                <br/><br/>
                                <div align="center" style="background-color: #baddfb">
                                    <table align="center" border="0" cellpadding="0" cellspacing="0" class="table" 
                                        width="100%">
                                        <tr height="50px" valign="middle">
                                            <td align="left">
                                                <asp:Label ID="ctlLabelReimBurseMentAmountNameForInvoice" runat="server" 
                                                    Text="เบิกได้"></asp:Label>
                                            </td>
                                            <td align="left" width="38%">
                                                <asp:Label ID="ctlLabelReimBurseMentAmountForInvoice" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3">
                                                <ss:BaseGridView ID="ctlInvoiceGrid" runat="server" AllowSorting="true" 
                                                    AutoGenerateColumns="false" CssClass="table" DataKeyNames="InvoiceID" 
                                                    HeaderStyle-HorizontalAlign="Center" OnRowCommand="ctlInvoiceGrid_RowCommand"
                                                    OnRowDataBound="ctlInvoiceGrid_RowDataBound" ShowFooter="true" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Invoice">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="ctlInvoiceNoInvoiceGridHeader" runat="server" Text="Invoice$"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ctlInvoiceNoInvoiceGridItem" runat="server" Mode="Encode" 
                                                                    SkinID="SkGeneralLabel" 
                                                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindInvoiceNo(Eval("InvoiceNo").ToString()) %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Literal ID="ctlInvoiceNoInvoiceGridFooter" runat="server" Mode="Encode" 
                                                                    SkinID="SkGeneralLabel" Text="Total"></asp:Literal>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ctlNetAmountInvoiceGridItem" runat="server" Mode="Encode" 
                                                                    SkinID="SkNumberLabel" 
                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NetAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Literal ID="ctlNetAmountInvoiceGridFooter" runat="server" Mode="Encode" 
                                                                    SkinID="SkNumberLabel"></asp:Literal>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="250px" />
                                                            <FooterStyle HorizontalAlign="Right" Width="250px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <uc5:PopupCaller ID="ctlEditInvoicePopup" runat="server" 
                                                                    ButtonSkinID="SkEditButton" 
                                                                    OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
                                                                <asp:ImageButton ID="ctlEdit" runat="server" CommandName="EditItem" 
                                                                    SkinID="SkEditButton" />
                                                                <asp:ImageButton ID="ctlDelete" runat="server" CommandName="DeleteItem" 
                                                                    SkinID="SkDeleteButton" />
                                                                <%--<asp:ImageButton ID="ctlView" runat="server" SkinID="SkQueryButton" CommandName="ViewItem" />--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="180px" Wrap="false" />
                                                            <ItemStyle HorizontalAlign="Center" Width="180px" Wrap="false" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </ss:BaseGridView>
                                            </td>
                                        </tr>
                                        <tr height="50px" valign="middle">
                                            <td align="left">
                                                <asp:Label ID="ctlLabelRemainingName" runat="server" 
                                                    SkinID="SkFieldCaptionLabel" Text="$Remaining$"></asp:Label>
                                            </td>
                                            <td align="left" width="38%">
                                                <asp:Label ID="ctlLabelRemaining" runat="server" SkinID="SkNumberLabel"></asp:Label>
                                            </td>
                                            <td>
                                                <uc5:PopupCaller ID="ctlDeclareVATPopupCaller" runat="server" 
                                                    OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" Width="850" />
                                                <asp:ImageButton ID="ctlDeclareVAT" runat="server" 
                                                    OnClick="ctlDeclareVAT_Click" SkinID="SkDeclareVATButton" 
                                                    ToolTip="$Declare VAT$" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br/><br/>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlDropDownListOwner" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ctlMileageGrid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="ctlMileageGrid" EventName="RowDataBound" />
                            <asp:AsyncPostBackTrigger ControlID="ctlInvoiceGrid" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="ctlInvoiceGrid" EventName="RowDataBound" />
                            <asp:AsyncPostBackTrigger ControlID="ctlAddCommit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ctlEditCommit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
			<uc5:PopupCaller ID="ctlInvoicePopupCaller" ButtonSkinID="SkGeneralExpenseButton" runat="server" OnNotifyPopupResult="ctlPopupCaller_NotifyPopupResult" />
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Panel>
