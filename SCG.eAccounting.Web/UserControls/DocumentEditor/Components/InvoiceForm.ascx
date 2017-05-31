<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceForm.ascx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.InvoiceForm" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/IOAutoCompleteLookup.ascx" TagName="IOAutoCompleteLookup"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/AccountField.ascx" TagName="AccountField"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/WHTTypeDropdown.ascx" TagName="WHTTypeDropdown"
    TagPrefix="uc7" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/WHTRateDropdown.ascx" TagName="WHTRateDropdown"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/VendorLookUp.ascx" TagName="VendorLookUp"
    TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/VendorTextBoxAutoComplete.ascx" TagName="VendorTextBoxAutoComplete"
    TagPrefix="uc10" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc11" %>
<ss:inlinescript id="InlineScript1" runat="server">
    <script type="text/javascript" language="javascript">
        function OnRadioButtonChange() {
            var rdoVat = document.getElementById('<%= ctlVatRdo.ClientID %>');
            var rdoWht = document.getElementById('<%= ctlWhtRdo.ClientID %>');

            if (rdoVat == null) {
                rdoVat = document.getElementById('<%= ctlVatRdoLabelExtender.ClientID %>').children[0];
                rdoWht = document.getElementById('<%= ctlWhtRdoLabelExtender.ClientID %>').children[0];
            }
            var tblSummary = document.getElementById('<%= ctlSummary.ClientID %>');
            var fdsWht = document.getElementById('<%= ctlWhtFds.ClientID %>');
            var invDiv = document.getElementById('<%= ctlInvoiceDiv.ClientID %>');

            if (event.srcElement.value == 'ctlVatRdo') {
                document.getElementById('<%= ctlShowVatControl.ClientID %>').value = "showVat";
                document.getElementById('ctlDivSummary').style.display = "block";
                tblSummary.style.display = "block";
                invDiv.style.display = 'block';
            }
            else if (event.srcElement.value == 'ctlNonVatRdo') {
                document.getElementById('<%= ctlShowVatControl.ClientID %>').value = "hideVat";
                ClearVat();
                document.getElementById('ctlDivSummary').style.display = "none";
                tblSummary.style.display = "none";
                invDiv.style.display = 'none';
                if (rdoWht.checked == true) {
                    //alert("rdowht");
                    document.getElementById('ctlDivSummary').style.display = "block";
                    tblSummary.style.display = "block";
                    invDiv.style.display = 'block';
                }
                else {
                    ClearInvoice()
                }
            }

            if (event.srcElement.value == 'ctlWhtRdo') {
                document.getElementById('<%= ctlShowWHTControl.ClientID %>').value = "showWHT";
                document.getElementById('ctlDivSummary').style.display = "block";
                fdsWht.style.display = "block";
                tblSummary.style.display = "block";
                invDiv.style.display = 'block';
            }
            else if (event.srcElement.value == 'ctlNonWht') {
                document.getElementById('<%= ctlShowWHTControl.ClientID %>').value = "hideWHT";
                ClearWHT();
                document.getElementById('ctlDivSummary').style.display = "none";
                fdsWht.style.display = "none";
                tblSummary.style.display = "none";
                invDiv.style.display = 'none';
                if (rdoVat.checked == true) {
                    document.getElementById('ctlDivSummary').style.display = "block";
                    tblSummary.style.display = "block";
                    invDiv.style.display = 'block';
                }
                else {
                    ClearInvoice()
                }
            }
        }
        function ClearWHTAmt() {
            if (document.getElementById('<%= ctlBaseAmount1.ClientID %>').value == '') {
                document.getElementById('<%= ctlWhtAmount1.ClientID %>').value = ''
            }
            if (document.getElementById('<%= ctlBaseAmount2.ClientID %>').value == '') {
                document.getElementById('<%= ctlWhtAmount2.ClientID %>').value = ''
            }
        }

        function ClearInvoice() {
            if (document.getElementById('<%= ctlInvoiceNo.ClientID %>') != null && document.getElementById('<%= ctlInvoiceNo.ClientID %>').value != '')
                document.getElementById('<%= ctlInvoiceNo.ClientID %>').value = '';
            if (document.getElementById('<%= ctlInvoiceDate.FindControl("txtDate").ClientID %>') != null && document.getElementById('<%= ctlInvoiceDate.FindControl("txtDate").ClientID %>').value != '')
                document.getElementById('<%= ctlInvoiceDate.FindControl("txtDate").ClientID %>').value = '';
            if (document.getElementById('<%= ctlInvoiceDescription.ClientID %>') != null && document.getElementById('<%= ctlInvoiceDescription.ClientID %>').value != '')
                document.getElementById('<%= ctlInvoiceDescription.ClientID %>').value = '';
            if (document.getElementById('<%= ctlVendorTaxNo.ClientID %>') != null && document.getElementById('<%= ctlVendorTaxNo.ClientID %>').value != '')
                document.getElementById('<%= ctlVendorTaxNo.ClientID %>').value = '';
            if (document.getElementById('<%= ctlVendorCode.ClientID %>') != null && document.getElementById('<%= ctlVendorCode.ClientID %>').value != '')
                document.getElementById('<%= ctlVendorCode.ClientID %>').value = '';
            if (document.getElementById('<%= ctlVendorBranch.ClientID %>') != null && document.getElementById('<%= ctlVendorBranch.ClientID %>').value != '')
                document.getElementById('<%= ctlVendorBranch.ClientID %>').value = '';
            if (document.getElementById('<%= ctlVendorName.ClientID %>') != null && document.getElementById('<%= ctlVendorName.ClientID %>').value != '')
                document.getElementById('<%= ctlVendorName.ClientID %>').value = '';
            if (document.getElementById('<%= ctlStreet.ClientID %>') != null && document.getElementById('<%= ctlStreet.ClientID %>').value != '')
                document.getElementById('<%= ctlStreet.ClientID %>').value = '';
            if (document.getElementById('<%= ctlCity.ClientID %>') != null && document.getElementById('<%= ctlCity.ClientID %>').value != '')
                document.getElementById('<%= ctlCity.ClientID %>').value = '';
            if (document.getElementById('<%= ctlCountry.ClientID %>') != null && document.getElementById('<%= ctlCountry.ClientID %>').value != '')
                document.getElementById('<%= ctlCountry.ClientID %>').value = '';
            if (document.getElementById('<%= ctlPostalCode.ClientID %>') != null && document.getElementById('<%= ctlPostalCode.ClientID %>').value != '')
                document.getElementById('<%= ctlPostalCode.ClientID %>').value = '';
            if (document.getElementById('<%= ctlBranch.ClientID %>') != null && document.getElementById('<%= ctlBranch.ClientID %>').value != '')
                document.getElementById('<%= ctlBranch.ClientID %>').value = '';
        }
        function ClearVat() {
            //            alert(document.getElementById('<%= ctlBranch.ClientID %>'));
            if (document.getElementById('<%= ctlVatAmount.ClientID %>') != null && document.getElementById('<%= ctlVatAmount.ClientID %>').value != '')
                document.getElementById('<%= ctlVatAmount.ClientID %>').value = '';
            if (document.getElementById('<%= ctlNetAmount.ClientID %>') != null && document.getElementById('<%= ctlNetAmount.ClientID %>').value != '')
                document.getElementById('<%= ctlNetAmount.ClientID %>').value = '';
        }
        function ClearWHT() {
            if (document.getElementById('<%= ctlWhtRateDropDown.FindControl("ctlWHTRateDropdown").ClientID %>') != null && document.getElementById('<%= ctlWhtRateDropDown.FindControl("ctlWHTRateDropdown").ClientID %>').value != 1)
                document.getElementById('<%= ctlWhtRateDropDown.FindControl("ctlWHTRateDropdown").ClientID %>').value = 1;
            if (document.getElementById('<%= ctlWHTTypeDropdown.FindControl("ctlWHTTypeDropdown").ClientID %>') != null && document.getElementById('<%= ctlWHTTypeDropdown.FindControl("ctlWHTTypeDropdown").ClientID %>').value != 1)
                document.getElementById('<%= ctlWHTTypeDropdown.FindControl("ctlWHTTypeDropdown").ClientID %>').value = 1;
            if (document.getElementById('<%= ctlBaseAmount1.ClientID %>') != null && document.getElementById('<%= ctlBaseAmount1.ClientID %>').value != '')
                document.getElementById('<%= ctlBaseAmount1.ClientID %>').value = '';
            if (document.getElementById('<%= ctlWhtAmount1.ClientID %>') != null && document.getElementById('<%= ctlWhtAmount1.ClientID %>').value != '')
                document.getElementById('<%= ctlWhtAmount1.ClientID %>').value = '';
            if (document.getElementById('<%= ctlWhtRateDropDown2.FindControl("ctlWHTRateDropdown").ClientID %>') != null && document.getElementById('<%= ctlWhtRateDropDown2.FindControl("ctlWHTRateDropdown").ClientID %>').value != 1)
                document.getElementById('<%= ctlWhtRateDropDown2.FindControl("ctlWHTRateDropdown").ClientID %>').value = 1;
            if (document.getElementById('<%= ctlWHTTypeDropDown2.FindControl("ctlWHTTypeDropdown").ClientID %>') != null && document.getElementById('<%= ctlWHTTypeDropDown2.FindControl("ctlWHTTypeDropdown").ClientID %>').value != 1)
                document.getElementById('<%= ctlWHTTypeDropDown2.FindControl("ctlWHTTypeDropdown").ClientID %>').value = 1;
            if (document.getElementById('<%= ctlBaseAmount2.ClientID %>') != null && document.getElementById('<%= ctlBaseAmount2.ClientID %>').value != '')
                document.getElementById('<%= ctlBaseAmount2.ClientID %>').value = '';
            if (document.getElementById('<%= ctlWhtAmount2.ClientID %>') != null && document.getElementById('<%= ctlWhtAmount2.ClientID %>').value != '')
                document.getElementById('<%= ctlWhtAmount2.ClientID %>').value = '';
            if (document.getElementById('<%= ctlWHTAmount.ClientID %>').value != null && document.getElementById('<%= ctlWHTAmount.ClientID %>').value != '')
                document.getElementById('<%= ctlWHTAmount.ClientID %>').value = '';
            if (document.getElementById('<%= ctlNetAmount.ClientID %>').value != null && document.getElementById('<%= ctlNetAmount.ClientID %>').value != '')
                document.getElementById('<%= ctlNetAmount.ClientID %>').value = '';
        }
        function clearWHTAmount() {
            var baseAmount1 = document.getElementById('<%= ctlBaseAmount1.ClientID %>');
            var whtAmount1 = document.getElementById('<%= ctlWhtAmount1.ClientID %>');
            var baseAmount2 = document.getElementById('<%= ctlBaseAmount2.ClientID %>');
            var whtAmount2 = document.getElementById('<%= ctlWhtAmount2.ClientID %>');

            if ((whtAmount1 != null && whtAmount1.value != '') && (baseAmount1 != null && baseAmount1.value == ''))
                whtAmount1.value = '';
            if ((whtAmount2 != null && whtAmount2.value != '') && (baseAmount2 != null && baseAmount2.value == ''))
                whtAmount2.value = '';
        }

        function hideDiv(divID) {
            var divElement = document.getElementById(divID);
            if (divElement.style.display != "none") {
                divElement.style.display = "none";
            }
            else {
                divElement.style.display = "block";
            }
        }
    </script>
    <asp:HiddenField ID="ctlShowVatControl" runat="server" />
    <asp:HiddenField ID="ctlShowWHTControl" runat="server" />
</ss:inlinescript>
<asp:Panel ID="ctlInvoiceFormPanel" runat="server" Width="800px" BackColor="White"
    Style="display: block;">
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Panel ID="ctlInvoiceFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black">
                    <div>
                        <p>
                            <asp:Label ID="lblCapture" runat="server" Text='$Header$'></asp:Label></p>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divInvoiceItemGrid" style="height: 600; overflow-y: auto;">
                    <asp:Panel ID="ctlContent" runat="server">
                        <asp:UpdatePanel ID="ctlUpdatePanelInvoiceForm" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelInvoiceForm"
                                    DynamicLayout="true" EnableViewState="true">
                                    <ProgressTemplate>
                                        <uc11:SCGLoading ID="SCGLoading1" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Label ID="ctlInvoiceID" runat="server" Style="display: none;" />
                                <asp:Label ID="ctlMode" runat="server" Style="display: none;" />
                                <asp:Label ID="ctlFormType" runat="server" Style="display: none;" />
                                <asp:HiddenField ID="ctlRenderFlag" runat="server" />
                                <asp:HiddenField ID="ctlHideTotalAmountTHB" runat="server" />
                                <asp:HiddenField ID="ctlHideTotalAmountLocalCurrency" runat="server" />
                                <asp:HiddenField ID="ctlHideTotalAmountMainCurrency" runat="server" />
                                <fieldset id="ctlInvoiceItemFormFds" runat="server" style="width: 600px;">
                                    <table id="ctlTableInForm" runat="server" class="table" width="100%">
                                        <tr>
                                            <td align="left" style="width: 150px;">
                                                <asp:Label ID="ctlCostCenterLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Cost Center$"></asp:Label>
                                                <asp:Label ID="ctlCostCenterReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <uc2:CostCenterField ID="ctlCostCenterField" runat="server" OnOnObjectLookUpReturn="ctlCostCenterField_OnObjectLookUpReturn" />
                                                <ss:LabelExtender ID="ctlCostCenterFieldExtender" runat="server" LinkControlID="ctlCostCenterField"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.CostCenter %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlAccountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Expense Code$"></asp:Label>
                                                <asp:Label ID="ctlAccountReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <uc3:AccountField ID="ctlAccountField" runat="server" />
                                                <ss:LabelExtender ID="ctlAccountFieldLabelExtender1" runat="server" LinkControlID="ctlAccountField"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.AccountCode %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlInternalOrderLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                    Text="$Internal Order$"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <uc4:IOAutoCompleteLookup ID="ctlIOAutoCompleteLookup" runat="server" />
                                                <ss:LabelExtender ID="ctlIOAutoCompleteLookupLabelExtender2" runat="server" LinkControlID="ctlIOAutoCompleteLookup"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InternalOrder %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label onclick="hideDiv('ctl00_X_ctlInvoiceForm_ctlSales');">
                                                    [+]</label>
                                            </td>
                                        </tr>
                                        <tr id="ctlSales" style="display: none;">
                                            <td align="left">
                                                <asp:Label ID="ctlSaleOrderLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Sale Order$"></asp:Label>&nbsp;:&nbsp;
                                                <br />
                                                <asp:Label ID="ctlSaleItemLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Sale Item$"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlSaleOrder" runat="server" SkinID="SkGeneralTextBox" MaxLength="10"></asp:TextBox>
                                                <ss:LabelExtender ID="ctlSaleOrderLabelExtender" runat="server" LinkControlID="ctlSaleOrder"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>' />
                                                <br />
                                                <asp:TextBox ID="ctlSaleItem" runat="server" SkinID="SkGeneralTextBox" MaxLength="6"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="ctlSaleItemFilteredExtender" runat="server"
                                                    TargetControlID="ctlSaleItem" FilterType="Numbers" />
                                                <ss:LabelExtender ID="ctlSaleItemLabelExtender" runat="server" LinkControlID="ctlSaleItem"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlDescLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Description$"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlDescription" runat="server" SkinID="SkGeneralTextBox" MaxLength="50"
                                                    Width="250px"></asp:TextBox>
                                                <ss:LabelExtender ID="ctlDescriptionLabelExtender" runat="server" LinkControlID="ctlDescription"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlCurrencyLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Currency$"></asp:Label>
                                                <asp:Label ID="ctlRequiredCurrency" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <%--<asp:DropDownList ID="ctlCurrencyDrowdown" runat="server" SkinID="SkGeneralDropdown"
                                        DataTextField="Symbol" DataValueField="CurrencyID" AutoPostBack="true" OnSelectedIndexChanged="ctlCurrencyDropdown_OnSelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                                <uc5:CurrencyDropdown ID="ctlCurrency" runat="server" IsExpense="true" OnNotifyPopupResult="ctlCurrency_NotifyPopupResult" />
                                                <ss:LabelExtender ID="ctlCurrencyLabelExtender" runat="server" LinkControlID="ctlCurrency"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Amount$"></asp:Label>
                                                <asp:Label ID="Label2" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="ctlAmount" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 18, 2, true));"
                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                TargetControlID="ctlAmount" FilterType="Custom, Numbers" ValidChars=".,-" />
                                                            <ss:LabelExtender ID="ctlAmountLabelExtender" runat="server" LinkControlID="ctlAmount"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="ctlNoteComment" runat="server" Text="$Note$" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlAVExchangeRateLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                    Text="$Advance Exchange Rate$"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlAdvanceExchangeRate" runat="server" SkinID="SkNumberTextBox"
                                                    ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                    TargetControlID="ctlAdvanceExchangeRate" FilterType="Custom, Numbers" ValidChars=".," />
                                                <ss:LabelExtender ID="ctlAdvanceExchangeRateLabelExtender" runat="server" LinkControlID="ctlAdvanceExchangeRate"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlExchangeRateLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                    Text="$Exchange Rate$"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlExchangeRate" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 5));"
                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                    TargetControlID="ctlExchangeRate" FilterType="Custom, Numbers" ValidChars=".," />
                                                <ss:LabelExtender ID="ctlExchangeRateLabelExtender" runat="server" LinkControlID="ctlExchangeRate"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                                </ss:LabelExtender>
                                                  <asp:Label ID="ctlRemarkExchangeRate" runat="server" SkinID="SkFieldCaptionLabel"
                                                    Text="$Remark Exchange Rate$" style="color:Red" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="ctlReferanceNoLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Referance No.$"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="ctlReferanceNo" runat="server" SkinID="SkGeneralTextBox" MaxLength="30"
                                                    Width="250px"></asp:TextBox>
                                                <ss:LabelExtender ID="ctlReferanceNoLabelExtender" runat="server" LinkControlID="ctlReferanceNo"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.ReferenceNo %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                            <div id="ctlVendorAP" runat="server" style="display: none;">
                                                <table>
                                                    <tr>
                                                        <td align="left" style="width: 150px;">
                                                            <asp:Label ID="ctlVendorCodeAPLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="Vendor Code" />&nbsp;:&nbsp;        
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ctlVendorCodeAP" runat="server" SkinID="SkGeneralTextBox" MaxLength="20" Width="100px" />
                                                            <asp:ImageButton runat="server" ID="ctlVendorCodeAPSearch" SkinID="SkLookupButton" OnClick="ctlVendorCodeAPSearch_Click" OnOnObjectLookUpReturn="ctlVendorCodeAPSearch_OnObjectLookUpReturn"/>
                                                            <ss:LabelExtender ID="ctlVendorCodeAPExtender" runat="server" LinkControlID="ctlVendorAP" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'></ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <div id="ctlAddItemButton" runat="server" style="display: block;">
                                                    <asp:ImageButton runat="server" ID="ctlAdd" SkinID="SkAddButton" OnClick="ctlAdd_Click" />
                                                </div>
                                                <div id="ctlEditItemButton" runat="server" style="display: none;">
                                                    <asp:ImageButton ID="ctlUpdateItem" runat="server" SkinID="SkSaveButton" OnClick="ctlUpdateItem_Click" />
                                                    <asp:ImageButton ID="ctlCancelEditItem" runat="server" SkinID="SkCancelButton" OnClick="ctlCancelEditItem_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <font color="red">
                                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="InvoiceItem.Error" />
                                                </font>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <br />
                                <center>
                                    <ss:BaseGridView ID="ctlInvoiceItemGridview" Width="98%" runat="server" ShowFooter="true"
                                        AutoGenerateColumns="false" OnRowCommand="ctlInvoiceItemGridview_RowCommand"
                                        CssClass="table" OnDataBound="ctlInvoiceItemGridview_DataBound" DataKeyNames="InvoiceItemID"
                                        OnRowDataBound="ctlInvoiceItemGridview_RowDataBound" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-CssClass="GridHeader">
                                        <AlternatingRowStyle CssClass="GridItem" />
                                        <RowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cost Center" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCostCenterLabel" runat="server" SkinID="SkCodeLabel" Text='<%# DisplayCostCenter(Container.DataItem) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expense Code" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlAccountCodeLabel" runat="server" SkinID="SkCodeLabel" Text='<%# DisplayAccount(Container.DataItem) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Internal Order" SortExpression="">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="ctlIOLabel" runat="server" Text='<%# Eval("IOID") %>'></asp:Label>--%>
                                                    <asp:Label ID="ctlIOLabel" runat="server" SkinID="SkCodeLabel" Text='<%# DisplayIO(Container.DataItem) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlDescriptionLabel" runat="server" SkinID="SkGeneralLabel"
                                                        Text='<%# Eval("Description") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="ctlTotalAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Total") %>'></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCurrencyLabel1" runat="server" SkinID="SkCodeLabel" Text='<%# DisplayCurrency(Container.DataItem) %>'></asp:Label>
                                                </ItemTemplate>
                                                <%--<FooterTemplate>
                                <asp:Label ID="ctlTotalAmountTHBLabel" runat="server" Text="Total"></asp:Label>
                            </FooterTemplate>--%>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountLabel1" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTotalAmount" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Exchange Rate">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlExchangeRateLabel1" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <%--                            <FooterTemplate>
                                <asp:Label ID="ctlTotalAmountTHBLabel" runat="server" Text="Total"></asp:Label>
                            </FooterTemplate>--%>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <%--<FooterStyle HorizontalAlign="Right" />--%>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount(Local)">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountLocalLabel1" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "LocalCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTotalAmountLocalCurrency" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount(MainCurrency)">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountMainLabel1" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTotalAmountMainCurrency" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount(THB)">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlAmountTHBLabel1" runat="server" SkinID="SkNumberLabel"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlTotalAmountTHB" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference No." SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Literal Mode="Encode" ID="ctlReferenceNo" runat="server" SkinID="SkGeneralLabel"
                                                        Text='<%# Eval("ReferenceNo") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" CommandName="EditItem" />
                                                    <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CommandName="DeleteItem" />
                                                    <asp:ImageButton ID="ctlView" runat="server" SkinID="SkQueryButton" CommandName="ViewItem" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="180px" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                </center>
                                <br />
                                <div id="ctlRadioDiv" runat="server" style="display: none;">
                                    <table class="table">
                                        <tr align="left">
                                            <td style="width: 150px;">
                                                <asp:Label ID="ctlVatLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$VAT$"></asp:Label>&nbsp;<asp:Label
                                                    ID="ctlRequiredVAT" runat="server" SkinID="SkRequiredLabel"></asp:Label>:&nbsp;
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="ctlVatRdo" runat="server" GroupName="VAT" Text="$Vat$" Width="100px"
                                                    onClick="OnRadioButtonChange();" />
                                                <ss:LabelExtender ID="ctlVatRdoLabelExtender" runat="server" LinkControlID="ctlVatRdo"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                </ss:LabelExtender>
                                                <asp:RadioButton ID="ctlNonVatRdo" runat="server" GroupName="VAT" Text="$Non-Vat$"
                                                    Width="100px" onClick="OnRadioButtonChange();" />
                                                <ss:LabelExtender ID="ctlNonVatRdoLabelExtender" runat="server" LinkControlID="ctlNonVatRdo"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:Label ID="ctlWhtLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$With Holding Tax$"></asp:Label>&nbsp;<asp:Label
                                                    ID="ctlRequiredWHTax" runat="server" SkinID="SkRequiredLabel"></asp:Label>:&nbsp;
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="ctlWhtRdo" runat="server" GroupName="WHT" Text="$WHT$" Width="100px"
                                                    onClick="OnRadioButtonChange();" />
                                                <ss:LabelExtender ID="ctlWhtRdoLabelExtender" runat="server" LinkControlID="ctlWhtRdo"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                </ss:LabelExtender>
                                                <asp:RadioButton ID="ctlNonWht" runat="server" GroupName="WHT" Text="$Non-WHT$" Width="100px"
                                                    onClick="OnRadioButtonChange();" />
                                                <ss:LabelExtender ID="ctlNonWhtLabelExtender" runat="server" LinkControlID="ctlNonWht"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <asp:UpdatePanel ID="ctlUpdateInvoicePanel" runat="server" UpdateMode="Conditional"
                                    ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <div id="ctlInvoiceDiv" runat="server" style="display: none;">
                                            <fieldset id="ctlInvoicefds" runat="server" style="width: 95%; display: none;">
                                                <legend id="ctlInvoicefdsLegend" runat="server">
                                                    <asp:Label ID="ctlLegendText" runat="server" SkinID="SkFieldCaptionLabel" Text="$Invoice$"></asp:Label>
                                                </legend>
                                                <table width="100%" class="table">
                                                    <tr>
                                                        <td align="left" style="width: 20%;">
                                                            <asp:Label ID="ctlInvoiceNoLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Invoice No.$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" style="width: 35%;">
                                                            <asp:TextBox ID="ctlInvoiceNo" runat="server" SkinID="SkGeneralTextBox" MaxLength="16"></asp:TextBox>&nbsp;&nbsp;
                                                            <ss:LabelExtender ID="ctlInvoiceNoLabelExtender" runat="server" LinkControlID="ctlInvoiceNo"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                            <asp:Label ID="ctlBookNoLabel" runat="server" Text="$BookNo$" />
                                                        </td>
                                                        <td style="width: 20%;">
                                                        </td>
                                                        <td align="right" style="width: 15%;">
                                                            <asp:Label ID="ctlBranchLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Branch$"></asp:Label>&nbsp;&nbsp;
                                                            <ss:LabelExtender ID="ctlBranchExtenderLabel" runat="server" LinkControlID="ctlBranchLabel"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlBranch" runat="server" SkinID="SkGeneralTextBox" MaxLength="20" Width="80"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlBranchLabelExtender" runat="server" LinkControlID="ctlBranch"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="ctlInvoiceDateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Invoice Date$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <uc1:Calendar ID="ctlInvoiceDate" runat="server" />
                                                            <ss:LabelExtender ID="ctlInvoiceDateLabelExtender" runat="server" LinkControlID="ctlInvoiceDate"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlFLDocumentLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Fl Document$"></asp:Label>&nbsp;&nbsp;
                                                            <ss:LabelExtender ID="ctlFLDocLabelExtender3" runat="server" LinkControlID="ctlFLDocumentLabel"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlFLDocument" runat="server" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                            <ss:LabelExtender ID="ctlFLDocumentLabelExtender" runat="server" LinkControlID="ctlFLDocument"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="ctlInvoiceDescriptionLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                                Text="$Description$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="ctlInvoiceDescription" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlInvoiceDescriptionLabelExtender" runat="server" LinkControlID="ctlInvoiceDescription"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlVendorTaxNoLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Vendor Tax No$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="ctlVendorTaxNo" runat="server" OnTextChanged="ctlVendorTaxNo_TextChange"
                                                                            AutoPostBack="true" SkinID="SkGeneralTextBox" MaxLength="13" onkeypress="return isKeyFloat();" />
                                                                        <ss:LabelExtender ID="ctlVendorTaxNoLabelExtender" runat="server" LinkControlID="ctlVendorTaxNo"
                                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxVendorTaxNo" runat="server"
                                                                            TargetControlID="ctlVendorTaxNo" FilterType="Numbers" />
                                                                        <asp:HiddenField ID="ctlVendorId" runat="server" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:ImageButton runat="server" ID="ctlVendorSearch" SkinID="SkLookupButton" OnClick="ctlVendorSearch_Click" /><asp:ImageButton
                                                                            runat="server" ID="ctlClearVendor" SkinID="SkEditButton" OnClick="ctlClearVendor_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" style="width: 10%">
                                                            <asp:Label ID="ctlVendorBranchLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                                Text="$Vendor Branch$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlVendorBranch" runat="server" SkinID="SkGeneralTextBox" MaxLength="5"
                                                                OnTextChanged="ctlVendorBranch_TextChange" AutoPostBack="true"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                TargetControlID="ctlVendorBranch" FilterType="Numbers" />
                                                            <ss:LabelExtender ID="ctlVendorBranchLabelExtender" runat="server" LinkControlID="ctlVendorBranch"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 10%">
                                                            <asp:Label ID="ctlVendorCodeLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Vendor Code$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlVendorCode" runat="server" SkinID="SkGeneralTextBox" MaxLength="10"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlVendorCodeLabelExtender" runat="server" LinkControlID="ctlVendorCode"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlVendorNameLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Vendor Name$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <asp:TextBox ID="ctlVendorName" runat="server" SkinID="SkGeneralTextBox" MaxLength="100"
                                                                Width="250px"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlVendorNameLabelExtender" runat="server" LinkControlID="ctlVendorName"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlStreetLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Street$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlStreet" runat="server" SkinID="SkGeneralTextBox" MaxLength="35"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlStreetLabelExtender" runat="server" LinkControlID="ctlStreet"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 10%">
                                                            <asp:Label ID="ctlCityLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$City$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlCity" runat="server" SkinID="SkGeneralTextBox" MaxLength="35"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlCityLabelExtender" runat="server" LinkControlID="ctlCity"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlCountryLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Country$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlCountry" runat="server" SkinID="SkGeneralTextBox" MaxLength="3"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlCountryLabelExtender" runat="server" LinkControlID="ctlCountry"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlPostalCodeLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Postal Code$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlPostalCode" runat="server" SkinID="SkGeneralTextBox" MaxLength="5"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlPostalCodeLabelExtender" runat="server" LinkControlID="ctlPostalCode"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'></ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <uc9:VendorLookUp ID="ctlVendorLookup" runat="server" OnOnObjectLookUpReturn="ctlVendorLookup_OnObjectLookUpReturn" />
                                 <uc9:VendorLookUp ID="ctlVendorAPLookup" runat="server" OnOnObjectLookUpReturn="ctlVendorAPLookup_OnObjectLookUpReturn" />
                                <asp:UpdatePanel ID="ctlUpdatePanelWHTSummary" runat="server" UpdateMode="Conditional"
                                    ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <div id="ctlDivWhtfds" style="display: block;">
                                            <fieldset id="ctlWhtFds" runat="server" style="width: 95%; display: none;">
                                                <legend id="ctlWhtLeg" runat="server">
                                                    <asp:Label ID="ctlWhtTitle" runat="server" SkinID="SkFieldCaptionLabel" Text="$Witholding Tax$"></asp:Label>
                                                </legend>
                                                <table border="0" width="100%" class="table">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="ctlWhtRateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Rate$"></asp:Label>
                                                            <asp:Label ID="ctlWhtRateReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td style="width: 45%">
                                                            <uc8:WHTRateDropdown ID="ctlWhtRateDropDown" runat="server" />
                                                            <ss:LabelExtender ID="ctlWhtRateDropDownLabelExtender" runat="server" LinkControlID="ctlWhtRateDropDown"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <asp:Label ID="ctlWHTTypeLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Type$"></asp:Label>&nbsp;&nbsp;
                                                            <ss:LabelExtender ID="ctlWHTTypeLabelExtender" runat="server" LinkControlID="ctlWHTTypeLabel"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td>
                                                            <uc7:WHTTypeDropdown ID="ctlWHTTypeDropdown" runat="server" />
                                                            <ss:LabelExtender ID="ctlWHTTypeDropdownLabelExtender" runat="server" LinkControlID="ctlWHTTypeDropdown"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlBaseAmountLabel1" runat="server" SkinID="SkFieldCaptionLabel" Text="$Base Amount$"></asp:Label>
                                                            <asp:Label ID="ctlBaseAmountReq1" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlBaseAmount1" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" onblur='clearWHTAmount();'></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlBaseAmountLabelExtender1" runat="server" LinkControlID="ctlBaseAmount1"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlWhtAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Amount$"></asp:Label>
                                                            <asp:Label ID="ctlWhtAmountReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <ss:ReadOnlyTextBox ID="ctlWhtAmount1" runat="server" SkinID="SkNumberTextBox" IsReadOnly="true"
                                                                Width="41%" Style="text-align: right"></ss:ReadOnlyTextBox>
                                                            <ss:LabelExtender ID="ctlWhtAmountLabelExtender" runat="server" LinkControlID="ctlWhtAmount1"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlWhtRate2" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Rate$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <uc8:WHTRateDropdown ID="ctlWhtRateDropDown2" runat="server" />
                                                            <ss:LabelExtender ID="ctlWhtRateDropDownLabelExtender2" runat="server" LinkControlID="ctlWhtRateDropDown2"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlWHTTypeLabel2" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Type$"></asp:Label>&nbsp;&nbsp;
                                                            <ss:LabelExtender ID="ctlWHTTypeLabelExtender2" runat="server" LinkControlID="ctlWHTTypeLabel2"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <uc7:WHTTypeDropdown ID="ctlWHTTypeDropDown2" runat="server" />
                                                            <ss:LabelExtender ID="ctlWHTTypeDropDownLabelExtender2" runat="server" LinkControlID="ctlWHTTypeDropDown2"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlBaseAmountLabel2" runat="server" SkinID="SkFieldCaptionLabel" Text="$Base Amount$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlBaseAmount2" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));"
                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" onblur="clearWHTAmount();"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlBaseAmountLabelExtender2" runat="server" LinkControlID="ctlBaseAmount2"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlWhtAmountLabel2" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT Amount$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <ss:ReadOnlyTextBox ID="ctlWhtAmount2" runat="server" SkinID="SkNumberTextBox" IsReadOnly="true"
                                                                Width="41%" Style="text-align: right"></ss:ReadOnlyTextBox>
                                                            <ss:LabelExtender ID="ctlWhtAmountLabelExtender2" runat="server" LinkControlID="ctlWhtAmount2"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="ctlUpdatePanelSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="ctlDivSummary" style="">
                                            <table id="ctlSummary" runat="server" width="90%" style="text-align: center; border-width: 1px;
                                                border-color: black; display: none;" class="table">
                                                <tr>
                                                    <td align="center" style="width: 18%;">
                                                        <asp:Label ID="ctlBaseAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Base Amount$"></asp:Label>
                                                        <asp:Label ID="ctlBaseAmountReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 17%;">
                                                        <asp:Label ID="ctlVATLabel2" runat="server" SkinID="SkFieldCaptionLabel" Text="$VAT$"></asp:Label>
                                                        <asp:Label ID="ctlVATReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 17%;">
                                                        <asp:Label ID="ctlWHTLabel2" runat="server" SkinID="SkFieldCaptionLabel" Text="$WHT$"></asp:Label>
                                                        <asp:Label ID="ctlWHTReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 18%;">
                                                        <asp:Label ID="ctlNetAmountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Net Amount$"></asp:Label>
                                                        <asp:Label ID="ctlNetAmountReq" runat="server" SkinID="SkRequiredTextBox"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:TextBox ID="ctlBaseAmount" runat="server" SkinID="SkNumberTextBox" ReadOnly="true"></asp:TextBox>
                                                        <asp:HiddenField ID="ctlNonDeductAmount" runat="server" />
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ctlBaseAmountFilterExtender" runat="server"
                                                            TargetControlID="ctlBaseAmount" FilterType="Custom, Numbers" ValidChars=".,-" />
                                                        <ss:LabelExtender ID="ctlBaseAmountLabelExtender" runat="server" LinkControlID="ctlBaseAmount"
                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="ctlVatAmount" runat="server" SkinID="SkNumberTextBox" ReadOnly="true"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ctlVatAmountFilterExtender" runat="server"
                                                            TargetControlID="ctlVatAmount" FilterType="Custom, Numbers" ValidChars=".,-" />
                                                        <ss:LabelExtender ID="ctlVatAmountLabelExtender" runat="server" LinkControlID="ctlVatAmount"
                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="ctlWHTAmount" runat="server" SkinID="SkNumberTextBox" ReadOnly="true"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ctlWHTAmountFilterExtender" runat="server"
                                                            TargetControlID="ctlWHTAmount" FilterType="Custom, Numbers" ValidChars=".,-" />
                                                        <ss:LabelExtender ID="ctlWHTAmountExtender" runat="server" LinkControlID="ctlWHTAmount"
                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.WHTax %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="ctlNetAmount" runat="server" SkinID="SkNumberTextBox" ReadOnly="true"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="ctlNetAmountFilterExtender" runat="server"
                                                            TargetControlID="ctlNetAmount" FilterType="Custom, Numbers" ValidChars=".,-" />
                                                        <ss:LabelExtender ID="ctlNetAmountLabelExtender" runat="server" LinkControlID="ctlNetAmount"
                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VAT %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Label ID="ctlTaxCodeLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Tax Code$"></asp:Label>&nbsp;&nbsp;
                                                        <ss:LabelExtender ID="ctlTaxCodeLabelExtender" runat="server" LinkControlID="ctlTaxCodeLabel"
                                                            Style="font-weight: bold;" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ctlTaxCodeDropDown" AutoPostBack="true" OnSelectedIndexChanged="ctlTaxCodeDropDown_SelectedIndexChanged"
                                                            runat="server" SkinID="SkGeneralDropdown" Width="100px">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlTaxCodeDropDownLabelExtender" runat="server" LinkControlID="ctlTaxCodeDropDown"
                                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.InvoiceVerifier %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                <center>
                                    <table width="80%" class="table">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="ctlCalculate" runat="server" SkinID="SkCalculateButton" Text="$Calculate$"
                                                    OnClick="ctlCalculate_Click" />
                                                <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkSaveButton" OnClick="ctlSubmit_Click" />
                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlGridCancel" OnClick="ctlCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" width="50%" class="table">
                                        <tr align="center">
                                            <td style="color: Red;" align="center">
                                                <spring:ValidationSummary ID="ctlServiveVS" runat="server" Provider="ValidationError">
                                                </spring:ValidationSummary>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>
