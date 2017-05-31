<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Perdiem.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Perdiem"
    EnableTheming="true" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/AccountField.ascx" TagName="ExpenseField"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/IOAutoCompleteLookup.ascx" TagName="InternalLookup"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Time.ascx" TagName="Time" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc7" %>
<%--<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
	TargetControlID="lnkDummy"
	PopupControlID="ctlPanelPerdiemPopup"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="ctlPanelPerdiemHeader" />--%>
<ss:InlineScript runat="server">
    <script type="text/javascript">
        function hideDiv(divID) {
            var divElement = document.getElementById(divID);
            if (divElement.style.display != "none") {
                divElement.style.display = "none";
            }
            else {
                divElement.style.display = "block";
            }
        }

        window.onload = function () {
            setInterval('blinkIt()', 500)
        }
        function blinkIt() {
            if (!document.all) return;
            else {
                for (i = 0; i < document.all.tags('blink').length; i++) {
                    s = document.all.tags('blink')[i];
                    if (s.innerText.length > 0)
                        s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
                }
            }
        }
    </script>
</ss:InlineScript>
<asp:Panel ID="ctlPanelPerdiemPopup" runat="server" BackColor="White" Width="800px"
    Style="display: block">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" class="table" style="width: 100%" align="center">
                <tr>
                    <td>
                        <asp:Panel ID="ctlPanelPerdiemHeader" CssClass="table" runat="server" Style="cursor: move;
                            border: solid 1px Gray; color: Black">
                            <div>
                                <p>
                                    <asp:Label ID="ctlLabelPerdiemCapture" SkinID="SkFieldCaptionLabel" runat="server"
                                        Text='$Header$' Width="210px" /></p>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="ctlPanelContent" runat="server">
                <table border="0" class="table" style="width: 95%" align="center">
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="ctlUpdatePanelPrediemParentData" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="ctlFieldSetPrediemParentData" style="width: 700px" class="table">
                                        <table border="0" class="table" style="width: 100%">
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelCostcenterFeildName" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Cost center$" />
                                                    <asp:Label ID="Label6" runat="server" SkinID="SkRequiredLabel" />
                                                    :
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <uc1:CostCenterField ID="ctlCostCenterField" runat="server" />
                                                    <ss:LabelExtender ID="ctlCostCenterFieldExtender" runat="server" LinkControlID="ctlCostCenterField"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.CostCenter %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelExpenseField" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Expense Code$" />
                                                    <asp:Label ID="Label7" runat="server" SkinID="SkRequiredLabel" />
                                                    :
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <uc2:ExpenseField ID="ctlExpenseField" runat="server" />
                                                    <ss:LabelExtender ID="ctlExpenseFieldExtender" runat="server" LinkControlID="ctlExpenseField"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.AccountCode %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelInternalLookup" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Internal Order$" />
                                                    :
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <uc3:InternalLookup ID="ctlInternalLookup" runat="server" isMultiple="false" />
                                                    <ss:LabelExtender ID="ctlInternalLookupExtender" runat="server" LinkControlID="ctlInternalLookup"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InternalOrder %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label onclick="hideDiv('Sales');">
                                                        [+]</label>
                                                </td>
                                            </tr>
                                            <tr id="Sales" style="display: none;">
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelSaleOrder" SkinID="SkFieldCaptionLabel" runat="server" Text="$Sale Order$" />
                                                    :
                                                    <br />
                                                    <br />
                                                    <asp:Label ID="ctlLabelSaleItem" runat="server" SkinID="SkFieldCaptionLabel" Text="$Sale Item$"
                                                        Visible="true" />
                                                    :
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <asp:TextBox ID="ctlTextBoxSaleOrder" runat="server" SkinID="SkGeneralTextBox" MaxLength="10"
                                                        Width="100px" />
                                                    <ss:LabelExtender ID="LabelExtender1" runat="server" LinkControlID="ctlTextBoxSaleOrder"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                    <br />
                                                    <br />
                                                    <asp:TextBox ID="ctlTextBoxSaleItem" runat="server" MaxLength="6" SkinID="SkGeneralTextBox"
                                                        Width="100px" />
                                                    <ss:LabelExtender ID="LabelExtender2" runat="server" InitialFlag="<%# this.Mode %>"
                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                        LinkControlID="ctlTextBoxSaleItem" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                </td>
                                                <td align="left" style="width: 63%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelDescription" SkinID="SkFieldCaptionLabel" runat="server" Text="$Description$" />:
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <asp:TextBox ID="ctlTextboxDescription" runat="server" SkinID="SkGeneralTextBox"
                                                        MaxLength="50" Width="350px" />
                                                    <ss:LabelExtender ID="LabelExtender3" runat="server" LinkControlID="ctlTextboxDescription"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Panel ID="ctlPanelExchangeRate" runat="server">
                                                        <table border="0" style="width: 100%">
                                                            <tr>
                                                                <td align="left" style="width: 250px">
                                                                    <asp:Label ID="ctlLabelExchangeRateForPerdiemCalculateName" SkinID="SkFieldCaptionLabel"
                                                                        runat="server" Text="$Exchange Rate for Perdiem Calculate$" />
                                                                    :
                                                                </td>
                                                                <td align="left" style="width: 63%">
                                                                    <asp:Label ID="ctlLabelExchangeRateForPerdiemCalculate" SkinID="SkNumberLabel" runat="server"
                                                                        Width="100px" Height="20px" Style="background-color: Silver; vertical-align: middle;
                                                                        text-align: right" BorderStyle="Solid" BorderWidth="1px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table style="width: 100%; margin: 0">
                                                                        <td align="left" style="width: 250px">
                                                                            <asp:Label ID="ctlExchangeRateName" SkinID="SkFieldCaptionLabel" runat="server" Text="$Exchange Rate$" />
                                                                            :
                                                                        </td>
                                                                        <td style="width: 10%" align="right">
                                                                            <asp:TextBox ID="ctlExchangeRate" runat="server" SkinID="SkNumberTextBox" Width="100px"
                                                                                Style="vertical-align: middle; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 5));"
                                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlExchangeRateLabelExtender" runat="server" LinkControlID="ctlExchangeRate"
                                                                                SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                                        </td>
                                                                        <td colspan="3">
                                                                        </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table style="width: 100%; margin: 0">
                                                                        <tr>
                                                                            <td align="left" style="width: 250px">
                                                                                <asp:Label ID="ctlLabelPerdiemRateNormalZone" SkinID="SkFieldCaptionLabel" runat="server"
                                                                                    Text="$Perdiem Rate Normal Zone$" />
                                                                                :
                                                                            </td>
                                                                            <td style="width: 10%" align="right">
                                                                                <asp:TextBox ID="ctlTextBoxPerdiemRateNormalZoneUSD" runat="server" SkinID="SkNumberTextBox"
                                                                                    Style="width: 100px; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"
                                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                                <ss:LabelExtender ID="LabelExtender7" runat="server" LinkControlID="ctlTextBoxPerdiemRateNormalZoneUSD"
                                                                                    SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.PerdiemRate %>'></ss:LabelExtender>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="ctlNormalUSDUnit" SkinID="SkFieldCaptionLabel" runat="server" Text="$USD/Day$" />
                                                                            </td>
                                                                            <td style="width: 10%" align="right">
                                                                                <asp:TextBox ID="ctlTextBoxPerdiemRateNormalZoneTHB" runat="server" SkinID="SkNumberTextBox"
                                                                                    MaxLength="7" Style="width: 100px; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"
                                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                                <ss:LabelExtender ID="LabelExtender10" runat="server" LinkControlID="ctlTextBoxPerdiemRateNormalZoneTHB"
                                                                                    SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.PerdiemRate %>'></ss:LabelExtender>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="ctlNormalTHBUnit" SkinID="SkFieldCaptionLabel" runat="server" Text="$THB/Day$" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table style="width: 100%; margin: 0">
                                                                        <tr>
                                                                            <td align="left" style="width: 250px">
                                                                                <asp:Label ID="ctlLabelPerdiemRateHighZone" SkinID="SkFieldCaptionLabel" runat="server"
                                                                                    Text="$Perdiem Rate High Zone$" />
                                                                                :
                                                                            </td>
                                                                            <td style="width: 10%" align="right">
                                                                                <asp:TextBox ID="ctlTextBoxPerdiemRateHighZoneUSD" runat="server" SkinID="SkNumberTextBox"
                                                                                    Style="width: 100px; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"
                                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                                <ss:LabelExtender ID="LabelExtender6" runat="server" LinkControlID="ctlTextBoxPerdiemRateHighZoneUSD"
                                                                                    SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.PerdiemRate %>'></ss:LabelExtender>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="ctlHighUSDUnit" SkinID="SkFieldCaptionLabel" runat="server" Text="$USD/Day$" />
                                                                            </td>
                                                                            <td style="width: 10%" align="right">
                                                                                <asp:TextBox ID="ctlTextBoxPerdiemRateHighZoneTHB" runat="server" SkinID="SkNumberTextBox"
                                                                                    MaxLength="7" Style="width: 100px; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"
                                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server"
                                                                         TargetControlID="ctlTextBoxPerdiemRateHighZoneTHB" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                                <ss:LabelExtender ID="LabelExtender8" runat="server" LinkControlID="ctlTextBoxPerdiemRateHighZoneTHB"
                                                                                    SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.PerdiemRate %>'></ss:LabelExtender>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="ctlHighTHBUnit" SkinID="SkFieldCaptionLabel" runat="server" Text="$THB/Day$" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table width="100%">
                                                                        <tr id="ctlThaiZoneRowVisible" runat="server">
                                                                            <td align="left" style="width: 250px">
                                                                                <asp:Label ID="ctlPerdiemRateThaiZoneLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                                    Text="$Perdiem Rate Thai Zone$" />
                                                                                :
                                                                            </td>
                                                                            <td style="width: 10%" align="right">
                                                                                <asp:TextBox ID="ctlPerdiemRateThaiZoneTextBox" runat="server" SkinID="SkNumberTextBox"
                                                                                    Style="width: 100px; text-align: right" OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"
                                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                                 TargetControlID="ctlTextBoxPerdiemRateHighZoneUSD" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                                                <ss:LabelExtender ID="ctlPerdiemRateThaiZoneTextBoxExtender" runat="server" LinkControlID="ctlPerdiemRateThaiZoneTextBox"
                                                                                    SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.PerdiemRate %>'></ss:LabelExtender>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="ctlThaiZoneUnit" SkinID="SkFieldCaptionLabel" runat="server" Text="$USD/Day$" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="ctlPerdiemRateTR">
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelPerdiemRate" SkinID="SkFieldCaptionLabel" runat="server" Text="Perdiem Rate" />
                                                    <asp:Label ID="Label4" runat="server" SkinID="SkRequiredLabel" />
                                                    :
                                                </td>
                                                <td style="width: 63%" align="left">
                                                    <asp:TextBox ID="ctlTextboxPerdiemRate" runat="server" SkinID="SkNumberTextBox" Width="100px"
                                                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));" OnKeyDown="disablePasteOption();"
                                                        OnKeyUp="disablePasteOption();" />
                                                    <%--<ajaxToolkit:FilteredTextBoxExtender ID="ctlFilteredTextBoxExtender" runat="server"
                                             TargetControlID="ctlTextboxPerdiemRate" FilterType="Custom, Numbers" ValidChars=".," />--%>
                                                    <ss:LabelExtender ID="LabelExtender4" runat="server" LinkControlID="ctlTextboxPerdiemRate"
                                                        SkinID="SkNumberLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                    <asp:Label ID="ctlPerdiemRateUnit" runat="server" Text="$THB/Day$" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 250px">
                                                    <asp:Label ID="ctlLabelReferenceNo" SkinID="SkFieldCaptionLabel" runat="server" Text="$Reference No$" />
                                                    :
                                                </td>
                                                <td align="left" style="width: 63%">
                                                    <asp:TextBox ID="ctlTextboxReferenceNo" runat="server" SkinID="SkGeneralTextBox"
                                                        MaxLength="30" Width="100px" />
                                                    <ss:LabelExtender ID="LabelExtender5" runat="server" LinkControlID="ctlTextboxReferenceNo"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.Mode %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ctlAddItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlUpdateItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlCancelItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlPerdiemGrid" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlCommit" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlImageButtonPerdiemDetail" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ctlPerdiemDetailGrid" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td>
                            <image id="ctlremarkDomestic" runat="server" src="../../../App_Themes/Default/images/alert.gif"
                                style="height: 30px" />
                            <font color="red">
                                <asp:Label ID="ctlnoteDomestic" runat="server" SkinID="SkGeneralLabel" Text="$Note Domestic$"
                                    Style="font-size: 14px" />
                            </font>
                        </td>
                        <tr>
                            <td>
                                <image id="ctlremarkForeign" runat="server" src="../../../App_Themes/Default/images/alert.gif"
                                    style="height: 30px" />
                                <font color="red">
                                    <asp:Label ID="ctlNoteForeign" runat="server" SkinID="SkGeneralLabel" Text="$Note Foreign$"
                                        Style="font-size: 14px" />
                                </font>
                            </td>
                        </tr>
                    </tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ctlUpdatePanelModeManager" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="ctlPanelModeManage" runat="server" CssClass="table">
                                        <table align="left" border="0" class="table" width="880px">
                                            <tr>
                                                <td>
                                                    <fieldset id="ctlFieldSetModeManage" runat="server" class="table">
                                                        <table border="0" class="table">
                                                            <tr>
                                                                <td>
                                                                    <table border="0" class="table">
                                                                        <tr>
                                                                            <td align="center" style="width: 85px">
                                                                                <asp:Label ID="ctlLabelFromDate" runat="server" Text="$From Date$" />
                                                                                <font color="red">
                                                                                    <asp:Label ID="ctlLabelStar1" runat="server" Text="*" />
                                                                                </font>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="width: 85px">
                                                                                <uc4:Calendar ID="ctlCalendarFromDate" runat="server" Skin="SkCalendarTextBox" />
                                                                                <ss:LabelExtender ID="ctlCalendarFromDateLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                    LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                    LinkControlID="ctlCalendarFromDate" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:Panel ID="ctlPanelFormTime" runat="server" CssClass="table">
                                                                        <table border="0" class="table">
                                                                            <tr>
                                                                                <td align="center" style="width: 80px">
                                                                                    <asp:Label ID="Label1" runat="server" Text="$Time$" />
                                                                                    <font color="red">
                                                                                        <asp:Label ID="Label2" runat="server" Text="*" />
                                                                                    </font>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" style="width: 80px">
                                                                                    <uc5:Time ID="ctlTimeFromTime" runat="server" />
                                                                                    <ss:LabelExtender ID="ctlTimeFromTimeLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlTimeFromTime" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td>
                                                                    <table border="0" class="table">
                                                                        <tr>
                                                                            <td align="center" style="width: 80px">
                                                                                <asp:Label ID="ctlLabelToDate" runat="server" Text="$To Date$" />
                                                                                <font color="red">
                                                                                    <asp:Label ID="ctlLabelStar3" runat="server" Text="*" />
                                                                                </font>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="width: 80px">
                                                                                <uc4:Calendar ID="ctlCalendarToDate" runat="server" SkinID="SkCalendarTextBox" />
                                                                                <ss:LabelExtender ID="ctlCalendarToDateLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                    LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                    LinkControlID="ctlCalendarToDate" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:Panel ID="ctlPanelToTime" runat="server" CssClass="table">
                                                                        <table border="0" class="table">
                                                                            <tr>
                                                                                <td align="center" style="width: 80px">
                                                                                    <asp:Label ID="ctlLabelToTime" runat="server" Text="$Time$" />
                                                                                    <font color="red">
                                                                                        <asp:Label ID="ctlLabelStar4" runat="server" Text="*" />
                                                                                    </font>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" style="width: 80px">
                                                                                    <uc5:Time ID="ctlTimeToTime" runat="server" />
                                                                                    <ss:LabelExtender ID="ctlTimeToTimeLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlTimeToTime" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td>
                                                                    <table border="0" class="table">
                                                                        <tr>
                                                                            <td align="center" style="width: 100px">
                                                                                <asp:Label ID="ctlLabelAdjustedDay" runat="server" Text="$Adjusted Day$" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="width: 100px">
                                                                                <asp:TextBox ID="ctlTextBoxAdjustedDay" runat="server" MaxLength="3" OnKeyDown="disablePasteOption();"
                                                                                    OnKeyPress="isKeyInt()" OnKeyUp="disablePasteOption();" onpaste="event.returnValue=false"
                                                                                    SkinID="SkNumberTextBox" Style="text-align: right; width: 95%" />
                                                                                <ss:LabelExtender ID="ctlTextBoxAdjustedDayLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                    LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                    LinkControlID="ctlTextBoxAdjustedDay" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table border="0" class="table">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="ctlPanelHalfDay" runat="server" CssClass="table">
                                                                                    <table border="0" class="table">
                                                                                        <tr>
                                                                                            <td align="center" style="width: 80px">
                                                                                                <asp:Label ID="ctlLabelHalfDay" runat="server" Text="$Half Day$" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center" style="width: 80px">
                                                                                                <asp:TextBox ID="ctlTextBoxHalfDay" runat="server" MaxLength="3" OnKeyDown="disablePasteOption();"
                                                                                                    OnKeyPress="isKeyInt()" OnKeyUp="disablePasteOption();" onpaste="event.returnValue=false"
                                                                                                    SkinID="SkNumberTextBox" Style="text-align: right; width: 95%" />
                                                                                                <ss:LabelExtender ID="ctlTextBoxHalfDayLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                                    LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                                    LinkControlID="ctlTextBoxHalfDay" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Panel ID="ctlPanelCountry" runat="server" CssClass="table">
                                                                                    <table border="0" class="table">
                                                                                        <tr>
                                                                                            <td align="center" style="width: 150px">
                                                                                                <asp:Label ID="ctlLabelCountry" runat="server" Text="$Country$" />
                                                                                                <font color="red">
                                                                                                    <asp:Label ID="ctlLabelStar5" runat="server" Text="*" />
                                                                                                </font>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center" style="width: 150px">
                                                                                                <asp:DropDownList ID="ctlDropDownListCountry" runat="server" AutoPostBack="true"
                                                                                                    OnSelectedIndexChanged="ctlDropDownListCountry_SelectedIndexChanged" SkinID="SkGeneralDropdown"
                                                                                                    Width="95%">
                                                                                                </asp:DropDownList>
                                                                                                <ss:LabelExtender ID="ctlDropDownListCountryLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                                    LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                                    LinkControlID="ctlDropDownListCountry" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td>
                                                                                <table border="0" class="table">
                                                                                    <tr>
                                                                                        <td align="center" style="width: 200px">
                                                                                            <asp:Label ID="ctlLabelRemark" runat="server" Text="$Remark$" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center" style="width: 200px">
                                                                                            <asp:TextBox ID="ctlTextBoxRemark" runat="server" MaxLength="100" SkinID="SkGeneralTextBox"
                                                                                                Width="95%" />
                                                                                            <ss:LabelExtender ID="ctlTextBoxRemarkLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                                LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                                LinkControlID="ctlTextBoxRemark" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--<asp:Panel ID="ctlPanelHalfDay" CssClass="table" runat="server">
				                                        <table border="0" class="table">
			                                                <tr>				                              				                               				                                				                               				                               
				                                                <td align="center" style="width:80px">
				                                                   <asp:Label ID="ctlLabelHalfDay" runat="server" Text="$Half Day$"/>
				                                                </td>
				                                                <td align="center" style="width:150px">
				                                                   <asp:Label ID="ctlLabelCountry" runat="server" Text="$Country$"/>
				                                                   <font color="red"><asp:Label ID="ctlLabelStar5" runat="server" Text="*"/></font>			                           
				                                                </td>
				                                                <td align="center" style="width:200px">
				                                                   <asp:Label ID="ctlLabelRemark" runat="server" Text="$Remark$"/>				                           
				                                                </td>
			                                                </tr>
			                                                <tr>				                                				                               				                                				                               				                               
				                                                <td align="center" style="width:80px">
				                                                   <asp:TextBox ID="ctlTextBoxHalfDay" runat="server" SkinID="SkNumberTextBox" MaxLength="3" style="text-align :right; width :95%" OnKeyPress="isKeyInt()"
																					OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"/>
				                                                   <ss:LabelExtender ID="ctlTextBoxHalfDayLabelExtender" runat="server" LinkControlID="ctlTextBoxHalfDay" SkinID="SkGeneralLabel"
							                                        InitialFlag='<%# this.Mode %>'
							                                        LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
				                                                </td>
				                                                <td align="center" style="width:150px">
                                                                   <asp:DropDownList ID="ctlDropDownListCountry" SkinID="SkGeneralDropdown" runat="server" Width="95%" OnSelectedIndexChanged="ctlDropDownListCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                   <ss:LabelExtender ID="ctlDropDownListCountryLabelExtender" runat="server" LinkControlID="ctlDropDownListCountry" SkinID="SkGeneralLabel"
							                                        InitialFlag='<%# this.Mode %>'
							                                        LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
				                                                </td>
				                                                <td align="center" style="width:200px">
                                                                   <asp:TextBox ID="ctlTextBoxRemark" runat="server" SkinID="SkGeneralTextBox" MaxLength="100" Width="95%" />
                                                                   <ss:LabelExtender ID="ctlTextBoxRemarkLabelExtender" runat="server" LinkControlID="ctlTextBoxRemark" SkinID="SkGeneralLabel"
							                                        InitialFlag='<%# this.Mode %>'
							                                        LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
				                                                </td>
			                                                </tr>
		                                                 </table>
				                                    </asp:Panel>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:ImageButton ID="ctlAddItem" runat="server" CausesValidation="True" CommandName="AddItem"
                                                        OnClick="ctlAddItem_Click" SkinID="SkCtlFormNewRow" Text="Add" ToolTip="$Add$" />
                                                    <asp:ImageButton ID="ctlUpdateItem" runat="server" CausesValidation="True" CommandName="UpdateItem"
                                                        OnClick="ctlUpdateItem_Click" SkinID="SkCtlFormSave" Text="Update" ToolTip="$Update$" />
                                                    <asp:ImageButton ID="ctlCancelItem" runat="server" CausesValidation="False" CommandName="CancelItem"
                                                        OnClick="ctlCancelItem_Click" SkinID="SkCtlFormCancel" Text="Cancel" ToolTip="$Cancel$" />
                                                    <asp:HiddenField ID="ctlHiddenFieldPerdiemItemId" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <font color="red">
                                                        <spring:ValidationSummary ID="ctlValidationSummaryAddItem" runat="server" Provider="PerdiemItem.Error" />
                                                    </font>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ctlPerdiemGrid" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <center>
                                            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                                DynamicLayout="true" EnableViewState="true">
                                                <ProgressTemplate>
                                                    <uc6:SCGLoading ID="SCGLoading1" runat="server" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                            <ss:BaseGridView ID="ctlPerdiemGrid" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                                CssClass="table" DataKeyNames="PerdiemItemID" EnableInsert="False" HeaderStyle-CssClass="GridHeader"
                                                HeaderStyle-HorizontalAlign="Center" OnDataBound="ctlPerdiemGrid_DataBound" OnRowCommand="ctlPerdiemGrid_RowCommand"
                                                OnRowDataBound="ctlPerdiemGrid_RowDataBound" ReadOnly="true" ShowFooter="true"
                                                ShowMsgDataNotFound="false" Width="100%">
                                                <AlternatingRowStyle CssClass="GridItem" />
                                                <RowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="From Date">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelFromDateItem" runat="server" Mode="Encode" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("FromDate").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Time">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelFromTimeItem" runat="server" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "FromTime", "{0:HH:mm}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Date">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelToDateItem" runat="server" Mode="Encode" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ToDate").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Time">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelToTimeItem" runat="server" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "ToTime", "{0:HH:mm}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Day">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelTotalDayItem" runat="server" Mode="Encode" Text="" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Adjusted Day">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelAdjustedDayItem" runat="server" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "AdjustedDay", "{0:#,##0}") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="ctlLabelFooter" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Total") %>' />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Net Day">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelNetDayItem" runat="server" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "NetDay", "{0:#,##0.0}") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlLabelNetDayFooter" runat="server" Mode="Encode" Text="$TotalNetDay$" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount(USD)">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelMainAmountItem" runat="server" Mode="Encode" Text="" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlLabelMainAmountFooter" runat="server" Mode="Encode" Text="$TotalMainAmount$" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelAmountItem" runat="server" Mode="Encode" Text="" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlLabelAmountFooter" runat="server" Mode="Encode" Text="$TotalAmount$" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Full Day">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelFullDayItem" runat="server" Mode="Encode" Text='<%# Eval("FullDay") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlLabelFullDayFooter" runat="server" Mode="Encode" Text="$TotalFullDay$" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Half Day">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelHalfDayItem" runat="server" Mode="Encode" Text='<%# Eval("HalfDay") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlLabelHalfDayFooter" runat="server" Mode="Encode" Text="$TotalHalfDay$" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Country Zone">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlLabelCountryZoneIDItem" runat="server" Text="<%# DisplayZone(Container.DataItem) %>" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Country">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlLabelCountryIDItem" runat="server" Text="<%# DisplayCountry(Container.DataItem) %>" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLabelRemarkItem" runat="server" Mode="Encode" Text='<%# Eval("Remark") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ctlEdit" runat="server" CommandName="EditItem" SkinID="SkCtlGridEdit" />
                                                            <asp:ImageButton ID="ctlDelete" runat="server" CommandName="DeleteItem" SkinID="SkCtlGridDelete" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="180px" Wrap="false" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </ss:BaseGridView>
                                        </center>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ctlAddItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlUpdateItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlCancelItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlCommit" EventName="Click" />
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
                            <td align="left">
                                <asp:UpdatePanel ID="ctlUpdatePanelCalculate" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="ctlPanelCalculate" runat="server" CssClass="table">
                                            <table border="0" cellpadding="0" cellspacing="0" class="table" width="80%">
                                                <tr>
                                                    <td>
                                                        <div id="ctlPanelCalculateForeign" runat="server" style="background-color: #baddfb;">
                                                            <table align="center" border="0" cellpadding="0" cellspacing="0" class="table" width="95%">
                                                                <tr>
                                                                    <td colspan="8">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" width="12%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelCalculateName1" runat="server" Text="$เต็มวัน(วัน)$" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelCalculateName2" runat="server" Text="$อัตราต่อวัน(USD)$" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelCalculateName3" runat="server" Text="$รวม$" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelCalculateName4" runat="server" Text="$ครึ่งวัน(วัน)$" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelCalculateName5" runat="server" Text="$อัตราต่อวัน(USD)$" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelCalculateName6" runat="server" Text="$รวม$" />
                                                                    </td>
                                                                    <td align="center" width="20%">
                                                                        <asp:Label ID="ctlLabelCalculateName7" runat="server" Text="$รวมเบี้ยเลี้ยง$" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="12%">
                                                                        <asp:Label ID="ctlLabelCalculateName13" runat="server" Text="$เขตปกติ$" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiem" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #1E90FF; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelFullDayPerdiemRate" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #1E90FF; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiemAmount" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiem" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #4EEE94; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelHalfDayPerdiemRate" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #4EEE94; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiemAmount" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemTotalAmount" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="12%">
                                                                        <asp:Label ID="ctlLabelCalculateName14" runat="server" Text="$เขตสูง$" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiemHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelFullDayPerdiemRateHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiemAmountHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiemHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelHalfDayPerdiemRateHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiemAmountHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemTotalAmountHigh" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #9aa9af;
                                                                            vertical-align: middle;" Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="ctlThaiZoneRow" runat="server">
                                                                    <td align="left" width="12%">
                                                                        <asp:Label ID="ctlLabelThaiZone" runat="server" Text="$เขตไทย$" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiemThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelFullDayPerdiemRateThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalFullDayPerdiemAmountThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #1E90FF;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiemThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelHalfDayPerdiemRateThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="14%">
                                                                        <asp:Label ID="ctlLabelTotalHalfDayPerdiemAmountThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #4EEE94;
                                                                            vertical-align: middle;" Width="100%" />
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemTotalAmountThai" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #9aa9af;
                                                                            vertical-align: middle;" Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                    </td>
                                                                    <td align="right" colspan="5">
                                                                        <asp:Label ID="ctlLabelNamePerdiemAllowance" runat="server" Text="$PerdiemAllowance$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlPerdiemAllowanceLabel" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                    </td>
                                                                    <td align="right" colspan="5">
                                                                        <asp:Label ID="ctlLabelCalculateName8" runat="server" Text="$ส่วนตามราชการ$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemGovermentAmount" runat="server" BorderStyle="Solid"
                                                                            BorderWidth="1px" Height="20px" Style="text-align: right; background-color: #9aa9af;
                                                                            vertical-align: middle;" Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                    </td>
                                                                    <td align="right" colspan="5">
                                                                        <asp:Label ID="ctlLabelCalculateName10" runat="server" Text="$ส่วนเกิน$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelAllowance" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                    </td>
                                                                    <td align="right" colspan="5">
                                                                        <asp:Label ID="ctlLabelCalculateName11" runat="server" Text="$มีใบเสร็จ$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemPrivateAmount" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                    </td>
                                                                    <td align="right" colspan="6">
                                                                        <asp:Label ID="ctlLabelCalculateName12" runat="server" Text="$ยอดที่เสียภาษี$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlLabelPerdiemTaxAmount" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="8">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="ctlPanelCalculateDomestic" runat="server" style="background-color: #baddfb;"
                                                            width="70%">
                                                            <table align="center" border="0" cellpadding="0" cellspacing="0" class="table" width="70%">
                                                                <tr>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="12%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlLabelNetDayDM" runat="server" Text="$NetDay$" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlLabelRatePerDayDM" runat="server" Text="$Rate/Day$" />
                                                                    </td>
                                                                    <td align="center" width="20%">
                                                                        <asp:Label ID="ctlLabelTotalPerdiemAmountDM" runat="server" Text="$Total$" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="12%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="center" width="10%">
                                                                        <asp:Label ID="ctlNetDayDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #1E90FF; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="12%">
                                                                        <asp:Label ID="ctlRatePerDayDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #1E90FF; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                    <td align="center" width="20%">
                                                                        <asp:Label ID="ctlTotalPerdiemAmountDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #1E90FF; vertical-align: middle;"
                                                                            Width="100%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="3">
                                                                        <asp:Label ID="ctlLabelPerdiemAllowanceDM" runat="server" Text="$PerdiemAllowance$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlPerdiemAllowanceDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="ctlGovermentDMRow" runat="server">
                                                                    <td align="right" colspan="3">
                                                                        <asp:Label ID="ctlLabelGovermentAmountDM" runat="server" Text="$ส่วนตามราชการ$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlGovermentAmountDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="ctlExeedAllowanceRow" runat="server">
                                                                    <td align="right" colspan="3">
                                                                        <asp:Label ID="ctlLabelExceedAllowanceDM" runat="server" Text="$ส่วนเกิน$" />
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" width="20%">
                                                                        <asp:Label ID="ctlExceedAllowanceDM" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            Height="20px" Style="text-align: right; background-color: #9aa9af; vertical-align: middle;"
                                                                            Width="98%" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ctlAddItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlUpdateItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlCancelItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlPerdiemGrid" EventName="RowCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlCommit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlImageButtonPerdiemDetail" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlPerdiemDetailGrid" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="ctlPerdiemDetailZone" runat="server">
                            <td align="center">
                                <asp:Panel ID="ctlPanelPerdiemPersonalExpense" runat="server">
                                    <asp:Panel ID="ctlPanelPerdiemDetailHeader" runat="server" HorizontalAlign="left"
                                        Style="cursor: pointer; font-size: larger; color: Blue">
                                        <asp:ImageButton ID="imgToggle" runat="server" AlternateText="collapse" ImageUrl="~/App_Themes/Default/images/Slide/collapse.jpg"
                                            OnClientClick="return false;" />
                                        <b>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="searchBoxStatus" Text="$PersonalExpense$" />
                                        </b>
                                    </asp:Panel>
                                    <asp:Panel ID="ctlPanelPerdiemDetail" runat="server" CssClass="table" Width="80%">
                                        <br />
                                        <br />
                                        <table align="left" border="0" class="table" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="ctlPanelPerdiemInput" runat="server" CssClass="table" Width="100%">
                                                        <table align="center" border="0" class="table" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="Fieldset1" class="table" style="width: 95%">
                                                                        <table align="left" border="0" class="table" width="100%">
                                                                            <tr>
                                                                                <td align="center" style="width: 45%">
                                                                                    <asp:Label ID="ctlLabelPerdiemDetailDecription" runat="server" Text="$รายละเอียดค่าใช้จ่าย$" />
                                                                                </td>
                                                                                <td align="center" style="width: 15%">
                                                                                    <asp:Label ID="ctlLabelCurrency" runat="server" Text="$Currency$" />
                                                                                </td>
                                                                                <td align="center" style="width: 20%">
                                                                                    <asp:Label ID="ctlLabelExchangeRate" runat="server" Text="$Exchange Rate$" />
                                                                                </td>
                                                                                <td align="center" style="width: 20%">
                                                                                    <asp:Label ID="ctlLabelPerdiemDetailAmount" runat="server" Text="$Amount$" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" style="width: 45%">
                                                                                    <asp:TextBox ID="ctlTextBoxPerdiemDetailDecription" runat="server" MaxLength="100"
                                                                                        SkinID="SkGeneralTextBox" Width="95%" />
                                                                                    <ss:LabelExtender ID="ctlPerdiemDetailLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlTextBoxPerdiemDetailDecription" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                                <td align="center" style="width: 15%">
                                                                                    <%--<asp:DropDownList ID="ctlDropDownListCurrency" runat="server" 
                                                                            SkinID="SkGeneralDropdown" Width="95%">
                                                                        </asp:DropDownList>--%>
                                                                                    <uc7:CurrencyDropdown ID="ctlCurrency" runat="server" IsExpense="true" />
                                                                                    <ss:LabelExtender ID="ctlCurrencyLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlCurrency" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                                <td align="center" style="width: 20%">
                                                                                    <asp:TextBox ID="ctlTextBoxExchangeRate" runat="server" OnKeyDown="disablePasteOption();"
                                                                                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 8,5));" OnKeyUp="disablePasteOption();"
                                                                                        SkinID="SkNumberTextBox" Style="text-align: right; width: 95%" />
                                                                                    <ss:LabelExtender ID="ctlTextBoxExchangeRateLabelExtender" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlTextBoxExchangeRate" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                                <td align="center" style="width: 20%">
                                                                                    <asp:TextBox ID="ctlTextBoxPerdiemDetailAmount" runat="server" MaxLength="10" OnKeyDown="disablePasteOption();"
                                                                                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));" OnKeyUp="disablePasteOption();"
                                                                                        SkinID="SkNumberTextBox" Style="text-align: right; width: 95%" />
                                                                                    <ss:LabelExtender ID="ctlPerdiemDetailAmountLabelExtender9" runat="server" InitialFlag="<%# this.Mode %>"
                                                                                        LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>"
                                                                                        LinkControlID="ctlTextBoxPerdiemDetailAmount" SkinID="SkGeneralLabel"></ss:LabelExtender>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:ImageButton ID="ctlImageButtonPerdiemDetail" runat="server" CausesValidation="True"
                                                                        CommandName="AddItem" OnClick="ctlImageButtonPerdiemDetail_Click" SkinID="SkCtlFormNewRow"
                                                                        Text="Add" ToolTip="$Add$" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <font color="red">
                                                                        <spring:ValidationSummary ID="ValidationSummaryPerdiemDetail" runat="server" Provider="PerdiemDetail.Error" />
                                                                    </font>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="ctlUpdatePanelPerdiemDetailGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <center>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelPerdiemDetailGrid"
                                                                    DynamicLayout="true" EnableViewState="true">
                                                                    <ProgressTemplate>
                                                                        <uc6:SCGLoading ID="SCGLoading2" runat="server" />
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <ss:BaseGridView ID="ctlPerdiemDetailGrid" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                                                    CssClass="table" DataKeyNames="ExpensePerdiemDetailID" EnableInsert="False" HeaderStyle-CssClass="GridHeader"
                                                                    HeaderStyle-HorizontalAlign="Center" OnRowCommand="ctlPerdiemDetailGrid_RowCommand"
                                                                    OnRowDataBound="ctlPerdiemDetailGrid_RowDataBound" ReadOnly="true" ShowFooter="true"
                                                                    Width="97%">
                                                                    <AlternatingRowStyle CssClass="GridItem" />
                                                                    <RowStyle CssClass="GridAltItem" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="$Description$">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ctlLabelDescriptionPerdiemItem" runat="server" Mode="Encode" Text='<%# Bind("Description") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" Width="40%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="$Currency$">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ctlLabelCurrencyPerdiemDetailItem" runat="server" Text="<%# DisplayCurrency(Container.DataItem) %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="$Exchange Rate$" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ctlLabelExchangeRatePerdiemDetailItem" runat="server" Mode="Encode"
                                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.0000}") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="center" Width="0%" />
                                                                            <ItemStyle HorizontalAlign="Right" Width="0%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="$Amount$">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ctlLabelAmountPerdiemDetailItem" runat="server" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>' />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="ctlLabelAmountPerdiemDetailFooter" runat="server" SkinID="SkFieldCaptionLabel"
                                                                                    Text='<%# GetProgramMessage("Total") %>' />
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="$Amount(THB)$">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ctlLabelAmountTHBPerdiemDetailItem" runat="server" Mode="Encode"
                                                                                    Text="" />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Literal ID="ctlLabelAmountTHBPerdiemDetailFooter" runat="server" Mode="Encode"
                                                                                    Text="$TotalAmount(THB)$" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right" Width="20%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="20%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="$Action$">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ctlPerdiemDetailDelete" runat="server" CommandName="DeleteItem"
                                                                                    SkinID="SkCtlGridDelete" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </ss:BaseGridView>
                                                            </center>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ctlImageButtonPerdiemDetail" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <ajaxToolkit:CollapsiblePanelExtender ID="collapPanel1" runat="Server" CollapseControlID="ctlPanelPerdiemDetailHeader"
                                    Collapsed="true" CollapsedImage="~/App_Themes/Default/images/Slide/expand.jpg"
                                    CollapsedText="Show Personal Expense" ExpandControlID="ctlPanelPerdiemDetailHeader"
                                    ExpandedImage="~/App_Themes/Default/images/Slide/collapse.jpg" ExpandedText="Hide Personal Expense"
                                    ImageControlID="imgToggle" TargetControlID="ctlPanelPerdiemDetail" TextLabelID="lblStatus" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanelImageButtonCommit" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" border="0" cellpadding="0" cellspacing="0" class="table" width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:ImageButton ID="ctlCommit" runat="server" OnClick="ctlCommit_Click" SkinID="SkCtlFormSave" />
                                                    <asp:ImageButton ID="ctlPopupCancel" runat="server" OnClick="ctlPopupCancel_Click"
                                                        SkinID="SkCtlFormCancel" />
                                                    <asp:ImageButton ID="ctlViewClose" runat="server" OnClick="ctlViewClose_Click" SkinID="SkCtlFormCancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="ctlUpdatePanelBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <font color="red">
                                            <spring:ValidationSummary ID="ctlValidationSummaryCommit" runat="server" Provider="Perdiem.Error" />
                                            <spring:ValidationSummary ID="ctlValidationSummaryInvoice" runat="server" Provider="InvoiceItem.Error" />
                                        </font>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ctlAddItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlUpdateItem" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlPerdiemGrid" EventName="RowCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlCommit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlImageButtonPerdiemDetail" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ctlPerdiemDetailGrid" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
