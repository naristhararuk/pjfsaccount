<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceDocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.AdvanceDocumentEditor"
    EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ActorData.ascx" TagName="ActorData"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/DocumentHeader.ascx" TagName="DocumentHeader"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/Initiator.ascx" TagName="Initiator"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/Attachment.ascx" TagName="Attachment"
    TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/TALookup.ascx" TagName="TALookup" TagPrefix="uc7" %>
<%@ Register Src="~/UserControls/CalendarOfDueDate.ascx" TagName="CalendarOfDueDate"
    TagPrefix="uc8" %>
<%@ Register Src="~/Usercontrols/DropdownList/SCG.DB/StatusDropdown.ascx" TagName="StatusDropdown"
    TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc10" %>
<%@ Register Src="../ViewPost/ViewPost.ascx" TagName="ViewPost" TagPrefix="uc11" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/History.ascx" TagName="History"
    TagPrefix="uc12" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc13" %>
<style type="text/css">
    .style2
    {
        width: 72%;
    }
    .style3
    {
        width: 73%;
    }
</style>
<ss:InlineScript ID="ctlInlineScript" runat="server">
    <script language="javascript" type="text/javascript">
        function paymentTypeChange() {
            if (document.getElementById('<%= ctlPaymentType.ClientID %>').value == "TR" ||
        document.getElementById('<%= ctlPaymentType.ClientID %>').value == "") {
                document.getElementById('<%= ctlDivCounterCashierDomesticText.ClientID %>').style.display = 'none';
                //            document.getElementById('<%= ctlCounterCashierText.ClientID %>').style.display = 'none';
                document.getElementById('<%= ctlCounterCashier.ClientID %>').style.display = 'none';
            }
            else {
                document.getElementById('<%= ctlDivCounterCashierDomesticText.ClientID %>').style.display = 'block';
                //            document.getElementById('<%= ctlCounterCashierText.ClientID %>').style.display = 'block';
                document.getElementById('<%= ctlCounterCashier.ClientID %>').style.display = 'block';
            }
        }
        function calExchangeRate(amtID, amtThbID, exchangeRateID) {
            var amountThb;
            var amount;
            if (amtThbID.value != "")
                amountThb = parseFloat(amtThbID.value.replace(/\,/g, '')).toFixed(2);
            else
                amountThb = 0;
            if (amtID != null || amtID.innerText != "" || amtID.innerText != 0) {
                amount = parseFloat(amtID.innerText.replace(/\,/g, '')).toFixed(2);
            }
            else if (amtID != null || amtID.value != "" || amtID.value != 0) {
                amount = parseFloat(amtID.value.replace(/\,/g, '')).toFixed(2);
            }
            else {
                amountThb = 0;
                amount = 1;
            }

        exchangeRateID.innerText = parseFloat(amountThb / amount).toFixed(5);

        }

        function calExchangeRateForRepOffice(amtID, amtMainCurrencyID, exchangeRateID, exchangeRateMainToTHB, amountThbID) {
            var numFormat = new NumberFormat(0);
            var amountMainCurrency;
            var amount;
            var exchangeRateToTHB = parseFloat(exchangeRateMainToTHB.innerText.replace(/\,/g, '')).toFixed(5);

            if (amtMainCurrencyID.value != "")
                amountMainCurrency = parseFloat(amtMainCurrencyID.value.replace(/\,/g, '')).toFixed(2);
            else
                amountMainCurrency = 0;
            if (amtID != null || amtID.innerText != "" || amtID.innerText != 0) {
                amount = parseFloat(amtID.innerText.replace(/\,/g, '')).toFixed(2);
            }
            else if (amtID != null || amtID.value != "" || amtID.value != 0) {
                amount = parseFloat(amtID.value.replace(/\,/g, '')).toFixed(2);
            }
            else {
                amountMainCurrency = 0;
                amount = 1;
            }

            numFormat.setNumber(parseFloat(amountMainCurrency / amount).toFixed(5));
            exchangeRateID.innerText = numFormat.toFormatted();

            numFormat.setNumber(parseFloat(amountMainCurrency * exchangeRateToTHB).toFixed(2));
            amountThbID.value = numFormat.toFormatted();

            var idTemplate = '<%= ctlAdvanceIntGrid.ClientID %>';
            var itemIndexer = 0;
            var totalMainAmt = 0;
            var totalThbAmt = 0;
            var advGridview = document.getElementById(idTemplate); //keep first seq for add total remittance amt
            var seq;
            if (advGridview != null)
                advGridview = advGridview.childNodes[itemIndexer];

            var maincurrencyAmount = 0;
            var thbAmount = 0;

            var i = 0;
            for (i = 1; i < advGridview.childNodes.length; i++) {
                seq = advGridview.childNodes[i];
                if (seq.childNodes[0].innerText != '') {
                    //totalRemitAmtThb += parseFloat('0' + amtThbObj.innerText.replace(/\,/g, '')).toFixed(2) * 1; //new amount
                    maincurrencyAmount = (parseFloat('0' + seq.childNodes[4].getElementsByTagName("input")[0].value.replace(/\,/g, '')).toFixed(2) * 1);
                    thbAmount = (parseFloat('0' + seq.childNodes[5].getElementsByTagName("input")[0].value.replace(/\,/g, '')).toFixed(2) * 1);

                    totalMainAmt += maincurrencyAmount;
                    totalThbAmt += thbAmount;
                }
            }

            var totalMainCurrencyAmtObj = document.getElementById('<%= ctlTotalAmountMainLabel.ClientID %>');
            var totalAmountThbObj = document.getElementById("<%= ctlTotalAmountTHBLabel.ClientID %>");

            numFormat.setNumber(totalMainAmt);
            totalMainCurrencyAmtObj.innerText = numFormat.toFormatted();

            numFormat.setNumber(totalThbAmt);
            totalAmountThbObj.innerText = numFormat.toFormatted();

        }
    </script>
</ss:InlineScript>
<table width="100%" cellpadding="0" style="text-align: left" class="table" cellspacing="0">
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelHeader" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="ctlUpdatePanelHeaderProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHeader"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc13:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left">
                                <%--<uc3:DocumentHeader ID="ctlAdvanceFormHeader" HeaderForm='<%# GetProgramMessage("Domestic Advance Form") %>' runat="server" labelNo="Advance No"/>--%>
                                <uc3:DocumentHeader ID="ctlAdvanceFormHeader" runat="server" labelNo="Advance No" />
                                <asp:Label ID="ctlAdvanceType" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 12%">
                                            <asp:Label ID="ctlCompanyText" runat="server" Text="$Company$" SkinID="SkDocumentHeader2Label"></asp:Label>&nbsp;<asp:Label
                                                ID="ctlCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:
                                        </td>
                                        <td style="width: 88%">
                                            <uc6:CompanyField ID="ctlCompanyField" runat="server" />
                                            <ss:LabelExtender ID="ctlCompanyFieldExtender" runat="server" LinkControlID="ctlCompanyField"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Company %>'></ss:LabelExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 12%">
                                            <asp:Label ID="ctlSubjectText" runat="server" Text="$Subject$" SkinID="SkDocumentHeader2Label"></asp:Label>&nbsp;<asp:Label
                                                ID="ctlSubjectReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:
                                        </td>
                                        <td style="width: 88%">
                                            <asp:TextBox ID="ctlSubject" runat="server" SkinID="SkGeneralTextBox" MaxLength="200"
                                                Width="400px"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlSubjectExtender" runat="server" LinkControlID="ctlSubject"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Subject %>'></ss:LabelExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlCreatorData" Legend='<%# GetProgramMessage("ctlCreatorData") %>'
                                                ShowSMS="false" ShowVendorCode="false" ShowFavoriteButton="false" ShowSearchUser="false" runat="server"
                                                width="100%" />
                                            <%--<ss:LabelExtender ID="ctlCreatorDataExtender" runat="server" LinkControlID="ctlCreatorData" 
							                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
							                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                width="100%" />
                                            <%--<ss:LabelExtender ID="ctlRequesterDataExtender" runat="server" LinkControlID="ctlRequesterData" 
							                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
							                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.AdvanceReferTA %>'></ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlReceiverData" Legend='<%# GetProgramMessage("ctlReceiverData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                width="100%" />
                                            <%--<ss:LabelExtender ID="ctlReceiverDataExtender" runat="server" LinkControlID="ctlReceiverData" 
							                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
							                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="UpdatePanelAdvanceTab" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ajaxToolkit:TabContainer ID="ctlAdvanceTabContainer" runat="server" ActiveTabIndex="0">
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabGeneral" HeaderText="$General$" SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="ctlGeneralLabel" runat="server" Text="$General$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelGeneral" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGeneral"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading1" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:Label ID="ctlTANoText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Travelling Authorization$"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td align="left" valign="middle" colspan="3">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" valign="middle">
                                                                <asp:Label ID="ctlNATaNo" SkinID="SkCtlLabelData" runat="server" Text="N/A"></asp:Label>
                                                                <asp:LinkButton ID="ctlTANo" SkinID="SkCtlLinkButton" runat="server" OnClick="ctlTANo_Click"></asp:LinkButton>
                                                            </td>
                                                            <td align="left" valign="middle">
                                                                <asp:ImageButton ID="ctlTANoLookup" OnClick="ctlTANoLookup_Click" runat="server"
                                                                    SkinID="SkQueryButton" ToolTip="Search" />
                                                                <asp:ImageButton ID="ctlDeleteTA" runat="server" SkinID="SkDeleteButton" OnClick="ctlDeleteTA_Click"
                                                                    ToolTip="Delete" />
                                                                <uc7:TALookup ID="ctlTALookup" runat="server" isQueryForAdvance="true" isMultiple="false" />
                                                                <%--<ss:LabelExtender ID="ctlTALookupExtender" runat="server" LinkControlID="ctlTALookup" 
							                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
							                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.AdvanceReferTA %>'></ss:LabelExtender>--%>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="ctlTAIdLookup" runat="server" Style="display: none" Width="0px" Height="0px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <div id="ctlDivDomesticAmount" runat="server">
                                                <tr>
                                                    <td align="left" style="width: 25%">
                                                        <asp:Label ID="ctlAmountText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Amount (VND)$"></asp:Label>
                                                        <asp:Label ID="ctlAmountReq" SkinID="SkGeneralLabel" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" style="width: 25%">
                                                        <asp:TextBox ID="ctlAmount" runat="server" SkinID="SkNumberTextBox" Width="160px"
                                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 14));" OnKeyDown="disablePasteOption();"
                                                            OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                        <ss:LabelExtender ID="ctlAmountExtender" runat="server" LinkControlID="ctlAmount"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.DomesticAmountTHB %>'></ss:LabelExtender>
                                                    </td>
                                                    <td colspan="2">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td style="width: 40%">
                                                                    <asp:Label ID="ctlAmounTHBLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="Amount (THB)"></asp:Label>
                                                                </td>
                                                                <td style="width: 30%">
                                                                    <asp:Label ID="ctlAmountTotalTHBLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="0.00"></asp:Label>
                                                                    <asp:Label ID="ctlAmountTotalMainCurrency" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="0.00" Style="display: none" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="ctlAmountTHBCurrencyLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text=" Baht"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </div>
                                            <div id="ctlDivDomesticPaymentType" runat="server">
                                                <tr>
                                                    <td align="left" valign="middle" style="width: 25%">
                                                        <asp:Label ID="ctlPaymentTypeText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Payment Type$"></asp:Label>
                                                        <asp:Label ID="ctlPaymentTypeReq" SkinID="SkGeneralLabel" runat="server" Text="*"
                                                            Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" valign="middle" style="width: 25%">
                                                        <asp:DropDownList ID="ctlPaymentType" SkinID="SkGeneralDropdown" Width="198px" runat="server"
                                                            OnSelectedIndexChanged="ctlLblPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlPaymentTypeExtender" runat="server" LinkControlID="ctlPaymentType"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.PaymentType %>'></ss:LabelExtender>
                                                    </td>
                                                    <td align="left" valign="middle" style="width: 20%">
                                                        <div id="ctlDivCounterCashierDomesticText" runat="server" style="display: none">
                                                            <asp:Label ID="ctlCounterCashierText" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Counter/Cashier$"></asp:Label>
                                                            <asp:Label ID="ctlCounterCashierDomesReq" runat="server" SkinID="SkGeneralLabel"
                                                                Text="*" Style="color: Red;"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td align="left" valign="middle" style="width: 30%">
                                                        <asp:DropDownList ID="ctlCounterCashier" SkinID="SkGeneralDropdown" Width="198px"
                                                            runat="server" DataTextField="Text" DataValueField="ID" OnSelectedIndexChanged="ctlCounterCashierDomestic_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlCounterCashierExtender" runat="server" LinkControlID="ctlCounterCashier"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.CounterCashier %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </div>
                                            <div id="ctlDivInternationalGrid" runat="server">
                                                <tr>
                                                    <td colspan="4" align="center">
                                                        <ss:BaseGridView ID="ctlAdvanceIntGrid" runat="server" AutoGenerateColumns="false"
                                                            CssClass="Grid" OnRowCommand="ctlAdvanceIntGrid_RowCommand" DataKeyNames="AdvanceItemID"
                                                            Width="100%" ShowFooter='<%# this.isShowFooter %>' OnDataBound="ctlAdvanceIntGrid_DataBound"
                                                            SelectedRowStyle-BackColor="#6699FF" OnRowDataBound="ctlAdvanceIntGrid_RowDataBound"
                                                            ShowMsgDataNotFound="false" ShowFooterWhenEmpty="true">
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                            <RowStyle CssClass="GridItem" />
                                                            <FooterStyle CssClass="GridItem" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="PaymentType" ItemStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlPaymentTypeHeader" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("PaymentType") %>'></asp:Label>
															<asp:Label ID="ctlPaymentTypeReq" SkinID="SkGeneralLabel" runat="server" Text="*" style="color:Red;"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <%--<asp:Literal ID="ctlPaymentTypeLabel" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Eval("PaymentType") %>'></asp:Literal>--%>
                                                                        <asp:HiddenField ID="ctlPaymentTypeID" runat="server" Value='<%# Eval("PaymentType") %>' />
                                                                        <uc9:StatusDropdown ID="ctlPaymentTypeDDL" runat="server" />
                                                                        <ss:LabelExtender ID="ctlPaymentTypeDDLExtender" runat="server" LinkControlID="ctlPaymentTypeDDL"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.PaymentTypeFR %>'></ss:LabelExtender>
                                                                        <%--<ss:LabelExtender ID="ctlPaymentTypeDropdownExtender" runat="server" LinkControlID="ctlPaymentTypeDropdown" 
                                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>--%>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc9:StatusDropdown ID="ctlPaymentTypeDropdown" runat="server" />
                                                                        <ss:LabelExtender ID="ctlPaymentTypeDropdownExtender" runat="server" LinkControlID="ctlPaymentTypeDropdown"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlCurrencyHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Currency") %>'></asp:Label>
															<asp:Label ID="ctlCurrencyReq" runat="server" Text="*" style="color:Red;" SkinID="SkGeneralLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <asp:Literal ID="ctlCurrencyLabel" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                                                            Text='<%# Eval("CurrencyID") %>' />
                                                                        <asp:HiddenField ID="ctlCurrencyID" runat="server" Value='<%# Eval("CurrencyID") %>'>
                                                                        </asp:HiddenField>
                                                                        <%--<ss:LabelExtender ID="ctlCurrencyDropdownExtender" runat="server" LinkControlID="ctlCurrencyDropdown" 
                                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel"
                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>--%>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc10:CurrencyDropdown ID="ctlCurrencyDropdown" runat="server" IsAdvanceFR="true" />
                                                                        <ss:LabelExtender ID="ctlCurrencyDropdownExtender" runat="server" LinkControlID="ctlCurrencyDropdown"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlAmountHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Amount") %>'></asp:Label>
															<asp:Label ID="ctlAmountReq" runat="server" Text="*" style="color:Red;" SkinID="SkGeneralLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <%--<asp:TextBox ID="ctlAmount" runat="server" SkinID="SkCtlTextboxRight" style="text-align:right;" 
															onkeypress="return isKeyFloat();" Text='<%# Eval("Amount") %>' Width="90%" MaxLength="21"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="ctlAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'
                                                                            SkinID="SkNumberTextBox" Width="160px" OnKeyPress="return(currencyFormat(this, ',', '.', event, 14));"
                                                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlCurrencyAmountExtender" runat="server" LinkControlID="ctlAmount"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.CurrencyAmount %>'></ss:LabelExtender>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="ctlAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'
                                                                            SkinID="SkNumberTextBox" Width="160px" OnKeyPress="return(currencyFormat(this, ',', '.', event, 14));"
                                                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlAmountExtender" runat="server" LinkControlID="ctlAmount"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exchange Rate" HeaderStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlExchangeRateHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("ExchangeRate") %>'></asp:Label>
															<asp:Label ID="ctlExchangeRateReq" runat="server" Text="*" style="color:Red;" SkinID="SkGeneralLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <asp:Label ID="ctlExchangeRate" runat="server" IsReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'
                                                                            SkinID="SkNumberTextBox" Width="90%" Style="text-align: right" />
                                                                        <ss:LabelExtender ID="ctlExchangeRateExtender" runat="server" LinkControlID="ctlExchangeRate"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="ctlExchangeRate" runat="server" IsReadOnly="true" SkinID="SkNumberTextBox"
                                                                            Width="70%" Style="text-align: right" />
                                                                        <ss:LabelExtender ID="ctlExchangeRateExtender" runat="server" LinkControlID="ctlExchangeRate"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="20%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount(USD)" HeaderStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlExchangeRateHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("ExchangeRate") %>'></asp:Label>
															<asp:Label ID="ctlExchangeRateReq" runat="server" Text="*" style="color:Red;" SkinID="SkGeneralLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="ctlAmountMain" runat="server" SkinID="SkNumberTextBox" Width="160px"
                                                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));" OnKeyDown="disablePasteOption();"
                                                                            OnKeyUp="disablePasteOption();" Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'> </asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlAmountUSDExtender" runat="server" LinkControlID="ctlAmountMain"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <ss:ReadOnlyTextBox ID="ctlAmountMain" runat="server" IsReadOnly="true" OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));"
                                                                            SkinID="SkNumberTextBox" Width="90%" Style="text-align: right" />
                                                                        <ss:LabelExtender ID="ctlAmountUSDExtender" runat="server" LinkControlID="ctlAmountMain"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount(THB)" HeaderStyle-HorizontalAlign="Center">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlAmountTHBHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("AmountTHB") %>'></asp:Label>
															<asp:Label ID="ctlAmountTHBReq" runat="server" Text="*" style="color:Red;" SkinID="SkGeneralLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="ctlAmountTHB" runat="server" SkinID="SkNumberTextBox" Width="160px"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "AmountTHB", "{0:#,##0.00}") %>'
                                                                            OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));" OnKeyDown="disablePasteOption();"
                                                                            OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlAmountTHBExtender" runat="server" LinkControlID="ctlAmountTHB"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="ctlAmountTHB" runat="server" SkinID="SkNumberTextBox" Width="160px"
                                                                            Text='<%# Eval("AmountTHB") %>' OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));"
                                                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                                        <ss:LabelExtender ID="ctlAmountTHBExtender" runat="server" LinkControlID="ctlAmountTHB"
                                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" Width="98%" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <%--<HeaderTemplate>
															<asp:Label ID="ctlActionHeader" runat="server" Text='$Action$' Width="5%" SkinID="SkFieldCaptionLabel"></asp:Label>
														</HeaderTemplate>--%>
                                                                    <EditItemTemplate>
                                                                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" ToolTip="Delete"
                                                                            CommandName="DeleteAdvance" />
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:ImageButton ID="ctlAddItem" runat="server" SkinID="SkAddButton" ToolTip="Save"
                                                                            CommandName="AddAdvance" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                                    <FooterStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </ss:BaseGridView>
                                                        <table class="table" width="100%">
                                                            <tr>
                                                                <td style="width: 45%">
                                                                </td>
                                                                <td align="right" style="width: 20%">
                                                                    <asp:Label ID="ctlTotalLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="Total" />
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <asp:Label ID="ctlTotalAmountMainLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="0.00" />
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <asp:Label ID="ctlTotalAmountTHBLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="0.00" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td align="right" style="width: 20%">
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <asp:Label ID="ctlExchangeRateTextLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="ExchangeRate :" />
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlExchangeRateForeign" runat="server" SkinID="SkFieldCaptionLabel"
                                                                                    Text="0.00" />
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="ctlExchangeRateCurrencyLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                                                    Text="THB/USD" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td align="right" style="width: 20%">
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <asp:Label ID="ctlExchangeRateTextLabel2" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="ExchangeRate :" />
                                                                </td>
                                                                <td align="right" style="width: 15%">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlExchangeRateForeign2" runat="server" SkinID="SkFieldCaptionLabel"
                                                                                    Text="0.00" />
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="ctlExchangeRateCurrencyLabel2" runat="server" SkinID="SkFieldCaptionLabel"
                                                                                    Text="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                        <table>
                                            <tr>
                                                <td style="width:70%"></td>

                                            </tr>
                                        </table>
                                    </tr>--%>
                                            </div>
                                            <div id="ctlDivInterCounterCashier" runat="server">
                                                <tr>
                                                    <td align="left" valign="middle" style="width: 25%">
                                                        <asp:Label ID="ctlCounterCashierInterText" SkinID="SkFieldCaptionLabel" runat="server"
                                                            Text="$Counter / Cashier$"></asp:Label>
                                                        <asp:Label ID="ctlCounterCashierInterReq" runat="server" SkinID="SkRequiredLabel" />&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" valign="middle" colspan="3">
                                                        <asp:DropDownList ID="ctlCounterCashierInter" SkinID="SkGeneralDropdown" Width="198px"
                                                            runat="server" DataTextField="Text" DataValueField="ID" OnSelectedIndexChanged="ctlCounterCashierForeign_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlCounterCashierInterExtender" runat="server" LinkControlID="ctlCounterCashierInter"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.CounterCashier %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </div>
                                            <div id="ctlDivServiceTeam" runat="server">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="ctlServiceText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Service Team$"></asp:Label>
                                                        <asp:Label ID="ctlServiceReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ctlServiceTeam" SkinID="SkGeneralDropdown" Width="198px" runat="server"
                                                            DataTextField="Text" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlServiceTeamExtender" runat="server" LinkControlID="ctlServiceTeam"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ServiceTeam %>'></ss:LabelExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="ctlCurrencyLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="Currency"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ctlCurrencyDropdown" SkinID="SkGeneralDropdown" Width="198px"
                                                            runat="server" DataTextField="Text" DataValueField="ID" Visible="false" OnSelectedIndexChanged="ctlCurrencyDropdown_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlCurrencyDropdownExtender" runat="server" LinkControlID="ctlCurrencyDropdown"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.CurrencyRepOffice %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </div>
                                            <tr>
                                                <td align="left" valign="middle" style="width: 25%">
                                                    <asp:Label ID="ctlRequestDateOfAdvanceText" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Request Date of Advance$"></asp:Label>
                                                    <asp:Label ID="ctlRequestDateOfAdvanceReq" SkinID="SkGeneralLabel" runat="server"
                                                        Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td align="left" valign="middle" style="width: 25%">
                                                    <uc8:CalendarOfDueDate ID="ctlRequestDateOfAdvance" DueDateControl="ctlDueDateOfRemittance"
                                                        RequestRemitControl="ctlRequestDateOfRemittance" runat="server" />
                                                    <ss:LabelExtender ID="ctlRequestDateOfAdvanceExtender" runat="server" LinkControlID="ctlRequestDateOfAdvance"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.RequestDateOfAdvance %>'></ss:LabelExtender>
                                                    <uc1:Calendar ID="ctlRequestDateOfAdvanceForeign" runat="server" />
                                                    <ss:LabelExtender ID="ctlRequestDateOfAdvanceForeignExtender" runat="server" LinkControlID="ctlRequestDateOfAdvanceForeign"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.RequestDateOfAdvance %>'></ss:LabelExtender>
                                                </td>
                                                <td align="left" valign="middle" style="width: 20%">
                                                    <asp:Label ID="ctlDueDateOfRemittanceText" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Due Date of Remittance$"></asp:Label>
                                                    <asp:Label ID="ctlDueDateOfRemittanceReq" SkinID="SkGeneralLabel" runat="server"
                                                        Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td align="left" valign="middle" style="width: 30%">
                                                    <asp:Label ID="ctlDueDateOfRemittance" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                                    <ss:LabelExtender ID="ctlDueDateOfRemittanceExtender" runat="server" LinkControlID="ctlDueDateOfRemittanceExtender"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <div id="ctlDivInternationalArrivalDate" runat="server">
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <asp:Label ID="ctlArrivalDateText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Arrival Date$"></asp:Label>
                                                        <asp:Label ID="ctlArrivalDateReq" SkinID="SkGeneralLabel" runat="server" Text="*"
                                                            Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" valign="middle" colspan="3">
                                                        <uc8:CalendarOfDueDate ID="ctlArrivalDate" runat="server" DueDateControl="ctlDueDateOfRemittance"
                                                            RequestRemitControl="ctlRequestDateOfRemittance" />
                                                        <ss:LabelExtender ID="ctlArrivalDateExtender" runat="server" LinkControlID="ctlArrivalDate"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ArrivalDate %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </div>
                                            <tr>
                                                <td align="left" colspan="4">
                                                    <asp:Label ID="ctlRemarkDomestic" SkinID="SkGeneralLabel" runat="server" Text="***Delay clearing more than 14 days from the date of advance(limiting to 30 days) must be identified the expected date for remittance,<br /> the reason and approved by, at least, the division director or any person in the same level."></asp:Label>
                                                    <asp:Label ID="ctlRemarkForeign" SkinID="SkGeneralLabel" runat="server" Text="***Delay clearing more than 14 days from the date of arrival and remittance with in 7 days(limiting to 30 days) must be identified the expected date for<br /> remittance, the reasons and approved by, at least, the division director or any person in the same level."></asp:Label>
                                                    <asp:Label ID="ctlBenefit" SkinID="SkGeneralLabel" runat="server" Text="<BR>***เพื่อประโยชน์สูงสุดในการขอคืนภาษีให้กับบริษัท  โปรดออกใบเสร็จ/ใบกำกับภาษี,ใบเสร็จรับเงิน เป็นชื่อและที่อยู่ของบรัษัทที่ตนสังกัด"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:Label ID="ctlRequestDateOfRemittanceText" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="$Request Date of Remittance$"></asp:Label>
                                                    <asp:Label ID="ctlRequestDateOfRemittanceReq" SkinID="SkGeneralLabel" runat="server"
                                                        Text="*" Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td align="left" valign="middle">
                                                    <uc1:Calendar ID="ctlRequestDateOfRemittance" runat="server" />
                                                    <ss:LabelExtender ID="ctlRequestDateOfRemittanceExtender" runat="server" LinkControlID="ctlRequestDateOfRemittance"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.RequestDateOfRemittance %>'></ss:LabelExtender>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 15%">
                                                                <asp:Label ID="ctlReasonText" runat="server" SkinID="SkFieldCaptionLabel" Text="$Reason$"></asp:Label>&nbsp;:&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ctlReason" runat="server" SkinID="SkGeneralTextBox" Width="300px"
                                                                    Height="50px" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 100);"
                                                                    onkeyup="return IsMaxLength(this, 100);"></asp:TextBox>
                                                                <ss:LabelExtender ID="ctlReasonExtender" runat="server" LinkControlID="ctlReason"
                                                                    InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" Width="300px" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Reason %>'
                                                                    ForeColor="Red"></ss:LabelExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%--<td align="left" rowspan="2" valign="top">
											
										</td>--%>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <div id="ctlDivExchangeRate1" runat="server">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <asp:Label ID="ctlExchangeRateLabel1" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="ExchangeRate : "></asp:Label>
                                                                </td>
                                                                <td align="right" width="30%">
                                                                    <asp:Label ID="ctlExchangeRate1" SkinID="SkFieldCaptionLabel" runat="server" Text="0.00"></asp:Label>
                                                                </td>
                                                                <td width="2%">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="ctlCurrencyExchangeRateLabel1" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="VND/USD"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <div id="ctlDivExchangeRate2" runat="server">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <asp:Label ID="ctlExchangeRateLabel2" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="ExchangeRate : "></asp:Label>
                                                                </td>
                                                                <td align="right" width="30%">
                                                                    <asp:Label ID="ctlExchangeRate2" SkinID="SkFieldCaptionLabel" runat="server" Text="0.00"></asp:Label>
                                                                </td>
                                                                <td width="2%">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="ctlCurrencyExchangeRateLabel2" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="THB/USD"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <div id="ctlDivInternationalExchangeRateForPerDiem" runat="server">
                                                        <asp:Label ID="ctlExchangeRateForPerDiemText" SkinID="SkFieldCaptionLabel" runat="server"
                                                            Text="Exchange Rate for per diem calculation (THB)"></asp:Label>
                                                        <asp:Label ID="Label4" SkinID="SkGeneralLabel" runat="server" Text="*" ForeColor="Red"></asp:Label>&nbsp;:&nbsp;
                                                        <asp:TextBox ID="ctlExchangeRateForPerDiem" runat="server" SkinID="SkNumberTextBox"
                                                            MaxLength="21" OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 4));"
                                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"></asp:TextBox>
                                                        <%-- <ajaxToolkit:FilteredTextBoxExtender
	                                                            ID="FilteredTextBoxExtender3"
	                                                            runat="server"
	                                                            TargetControlID="ctlExchangeRateForPerDiem"
	                                                            ValidChars ="."
	                                                            FilterType="Numbers,Custom" />--%>
                                                        <ss:LabelExtender ID="ctlExchangeRateForPerDiemExtender" runat="server" LinkControlID="ctlExchangeRateForPerDiem"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkNumberLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.ExchangeRateForPerDiemCalculation %>'></ss:LabelExtender>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabOutstanding" HeaderText="$OutStanding Advance$"
                            SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="ctlOutstandingText" runat="server" Text="$OutStanding Advance$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelOutstanding" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <ss:BaseGridView ID="ctlOutstandingGrid" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grid" AllowSorting="true" OnRowDataBound="ctlOutstandingGrid_RowDataBound"
                                            OnRowCommand="ctlOutstandingGrid_RowCommand" AllowPaging="true" DataKeyNames="avDocumentID,expenseDocumentID"
                                            Width="100%" ReadOnly="true" OnRequestCount="RequestCountOutstanding" OnRequestData="RequestDataOutstanding"
                                            SelectedRowStyle-BackColor="#6699FF">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="No" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlNoText" Mode="Encode" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Advance No" HeaderStyle-HorizontalAlign="Center" SortExpression="b.documentno">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlAdvanceNo" runat="server" Text='<%# Bind("AdvanceNo") %>'
                                                            CommandName="ClickAdvanceNo" SkinID="SkCodeLabel"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                    SortExpression="b.subject">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlDescription" Mode="Encode" runat="server" Text='<%# Bind("Description") %>'
                                                            SkinID="SkGeneralLabel"></asp:Literal>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="29%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AdvanceStatus" HeaderStyle-HorizontalAlign="Center"
                                                    SortExpression="e.displayName">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAdvanceStatus" Mode="Encode" runat="server" Text='<%# Bind("AdvanceStatus") %>'
                                                            SkinID="SkGeneralLabel" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExpenseNo" HeaderStyle-HorizontalAlign="Center" SortExpression="h.documentNo">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlExpenseNo" runat="server" Text='<%# Bind("ExpenseNo") %>'
                                                            CommandName="ClickExpenseNo" SkinID="SkCodeLabel" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExpenseStatus" HeaderStyle-HorizontalAlign="Center"
                                                    SortExpression="k.displayName">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlExpenseStatus" Mode="Encode" runat="server" Text='<%# Bind("ExpenseStatus") %>'
                                                            SkinID="SkGeneralLabel" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DueDate" HeaderStyle-HorizontalAlign="Center" SortExpression="a.duedateofremittance">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlDueDate" Mode="Encode" SkinID="SkCalendarLabel" runat="server"
                                                            Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DueDate")) %>'></asp:Literal>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="a.amount">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkNodataLabel" Text='<%#GetMessage("NoDataFound") %>'
                                                    runat="server"></asp:Literal>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                        </ss:BaseGridView>
                                        <ss:BaseGridView ID="FixedAdvanceOutstandingGrid" runat="server" AutoGenerateColumns="False"
                                            CssClass="Grid" AllowSorting="True" OnRowDataBound="ctlFixedAdvanceOutstandingGrid_RowDataBound"
                                            OnRowCommand="ctlFixedAdvanceOutstandingGrid_RowCommand" AllowPaging="True" DataKeyNames="DocumentID"
                                            Width="100%" OnRequestCount="RequestCountFixedAdvanceOutstanding" OnRequestData="RequestDataFixedAdvanceOutstanding"
                                            ClearSortExpression="False" CustomPageIndex="0" EnableModelValidation="True"
                                            meta:resourcekey="ctlOutstandingGridResource1" SaveButtonID="">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="No" meta:resourcekey="TemplateFieldResource1">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlNoText" Mode="Encode" runat="server" SkinID="SkNumberLabel" meta:resourcekey="ctlNoTextResource1"></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FixedAdvance No" SortExpression="b.documentno">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlFixedAdvanceNo" runat="server" Text='<%# Bind("fixedAdvanceNo") %>'
                                                            CommandName="ClickFixedAdvanceAdvanceNo" SkinID="SkCodeLabel" meta:resourcekey="ctlAdvanceNoResource1"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" SortExpression="b.subject">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlDescription" Mode="Encode" runat="server" Text='<%# Bind("subject") %>'
                                                            SkinID="SkGeneralLabel"></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="29%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FixedAdvanceStatus" SortExpression="b.CacheCurrentStateName">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAdvanceStatus" Mode="Encode" runat="server" Text='<%# Bind("fixedAdvanceStatus") %>'
                                                            SkinID="SkGeneralLabel" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Effective Date From" SortExpression="a.EffectiveFromDate">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlEffectiveFrom" Mode="Encode" SkinID="SkCalendarLabel" runat="server"
                                                            Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("effecttiveDateFrom")) %>'></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Effective Date To" SortExpression="a.EffectiveToDate">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlEffectiveTo" Mode="Encode" SkinID="SkCalendarLabel" runat="server"
                                                            Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("effecttiveDateTo")) %>'></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" SortExpression="a.NetAmount">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:#,##0.00}") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Objective" SortExpression="a.Objective">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlObjective" Mode="Encode" runat="server" Text='<%# Bind("objective") %>'
                                                            SkinID="SkGeneralLabel"></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="29%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkNodataLabel" Text='<%# GetMessage("NoDataFound") %>'
                                                    runat="server"></asp:Literal>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                            <SelectedRowStyle BackColor="#6699FF" />
                                        </ss:BaseGridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabInitial" HeaderText="$Initial$" SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="$Initial$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelInitial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressInitial" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelInitial"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading2" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc4:Initiator ID="ctlInitiator" runat="server" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Initiator %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAttachment" HeaderText="$Attachment$"
                            SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" Text="$Attachment$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelAttachment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressAttachment" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading3" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc5:Attachment ID="ctlAttachment" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabMemo" HeaderText="$Memo$" SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="Label3" runat="server" Text="$Memo$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelMemo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressMemo" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMemo"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading4" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" cellpadding="10" cellspacing="10" width="100%">
                                            <tr>
                                                <td align="Center">
                                                    <asp:TextBox ID="ctlMemo" runat="server" TextMode="MultiLine" Height="300px" Width="900px"
                                                        SkinID="SkGeneralTextBox" onkeypress="return IsMaxLength(this, 1000);" onkeyup="return IsMaxLength(this, 1000);"></asp:TextBox>
                                                    <ss:LabelExtender ID="ctlMemoExtender" runat="server" LinkControlID="ctlMemo" InitialFlag='<%# this.InitialFlag %>'
                                                        SkinID="SkMultiLineLabel" Width="800px" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.Memo %>'></ss:LabelExtender>
                                                </td>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabHistory" HeaderText="$History$" SkinID="SkFieldCaptionLabel">
                            <HeaderTemplate>
                                <asp:Label ID="ctlTabHistoryText" runat="server" Text="$History$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelHistory" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressHistory" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHistory"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading5" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <uc12:History ID="ctlHistory" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="ctlUpdatePanelApprover" runat="server" UpdateMode="Conditional"
                ChildrenAsTriggers="true">
                <ContentTemplate>
                    <uc2:ActorData ID="ctlApproverData" Legend='<%# GetProgramMessage("ctlApproverData")%>'
                        ShowSMS="true" ShowVendorCode="false" ShowFavoriteButton="true" ShowSearchUser="true" IsApprover="true"
                        runat="server" Width="100%" />
                    <asp:Label ID="AleartMessageFixedAdvance" SkinID="SkGeneralLabel" runat="server" Text="%AleartMessageFixedAdvance%" ForeColor="Red" Visible="false" ></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td align="left">
            &nbsp;
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlUpdatePanelViewPost" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdatePanelViewPostProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewPost"
            DynamicLayout="true" EnableViewState="true">
            <ProgressTemplate>
                <uc13:SCGLoading ID="SCGLoading6" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div align="left">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="ctlDivViewDetailDomestic" runat="server" style="display:inline;">
                            <fieldset id="ctlFieldSetVerifyDetailDomestic" runat="server" style="width: 99%;">
                                <legend id="ctlLegendVerifyDetailDomesticView" style="color: #4E9DDF;">
                                    <asp:Label ID="ctlVerifyDetailDomesticHeader" runat="server" Text="$Verify Detail$"
                                        SkinID="SkGeneralLabel"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlBranchTextDomestic" runat="server" Text="$Branch$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlBranchTextDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>:
                                        </td>
                                        <td style="width: 10%">
                                            <asp:TextBox ID="ctlBranchDomestic" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBranchDomesticExtender" runat="server" LinkControlID="ctlBranchDomestic"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlPaymentMethodDomesticText" runat="server" Text="$Payment Method$"
                                                SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlPaymentMethodDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ctlPaymentMethodDomestic" SkinID="SkGeneralDropdown" Width="180px"
                                                runat="server">
                                            </asp:DropDownList>
                                            <ss:LabelExtender ID="ctlPaymentMethodDomesticExtender" runat="server" LinkControlID="ctlPaymentMethodDomestic"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlSupplementaryDomesticText" runat="server" Text="$Supplementary$"
                                                SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:TextBox ID="ctlSupplementaryDomestic" Width="85px" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlSupplementaryDomesticLabelExtender" runat="server" LinkControlID="ctlSupplementaryDomestic"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Button ID="ctlViewPostButtonDomestic" runat="server" Text="View Post" OnClick="ctlViewPostButtonDomestic_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlPostingDateDomestic" runat="server" Text="$Posting Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlPostingDateDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>:
                                        </td>
                                        <td style="width: 15%">
                                            <uc1:Calendar ID="ctlPostingDateCalendarDomestic" runat="server" />
                                            <ss:LabelExtender ID="ctlPostingDateCalendarDomesticExtender" runat="server" LinkControlID="ctlPostingDateCalendarDomestic"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlBaselineDateDomesticText" runat="server" Text="$Baseline Date$"
                                                SkinID="SkFieldCaptionLabel"></asp:Label>
                                            :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlBaselineDate" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="ctlBusinessAreaDomesticText" runat="server" Text="$BusinessArea$"
                                                SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ctlBusinessAreaDomestic" Width="85px" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBusinessAreaDomesticLabelExtender" runat="server" LinkControlID="ctlBusinessAreaDomestic" Width="200"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td  style="width: 30%">
                                            <asp:Label ID="ctlPostingStatusDomesticText" runat="server" Text="$Posting Status$"
                                                SkinID="SkGeneralLabel"></asp:Label>:
                                            <asp:Label ID="ctlPostingStatusDomestic" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="ctlDivViewDetailForeign" runat="server" style="display:inline;">
                            <fieldset id="ctlFieldSetVerifyDetailForeign" runat="server" style="width: 99%;">
                                <legend id="Legend1" style="color: #4E9DDF;">
                                    <asp:Label ID="ctlViewDetailForeign" runat="server" Text="$Verify Detail$" SkinID="SkGeneralLabel"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlBranchForeignText" runat="server" Text="$Branch$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlBranchForeignReq" runat="server" SkinID="SkRequiredLabel" ></asp:Label>--%>:
                                        </td>
                                        <td style="width: 10%">
                                            <asp:TextBox ID="ctlBranchForeign" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBranchForeignExtender" runat="server" LinkControlID="ctlBranchForeign"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 10%">
                                            <div id="divBankAccountLabel" runat="server">
                                                <asp:Label ID="ctlBankAccountForeignText" runat="server" Text="$Bank Account$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                <asp:Label ID="ctlBankAccountForeignText2" runat="server" SkinID="SkFieldCaptionLabel"
                                                    Text=":"></asp:Label>
                                            </div>
                                        </td>
                                        <td style="width: 15%">
                                            <div id="divBankAccountTextBox" runat="server">
                                                <asp:TextBox ID="ctlBankAccountForeign" runat="server" SkinID="SkGeneralTextBox"
                                                    MaxLength="6"></asp:TextBox>
                                                <ss:LabelExtender ID="ctlBankAccountForeignExtender" runat="server" LinkControlID="ctlBankAccountForeign"
                                                    InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="ctlBusinessAreaForeignText" runat="server" Text="$BusinessArea$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ctlBusinessAreaForeign" Width="85px" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBusinessAreaForeignLabelExtender" runat="server" LinkControlID="ctlBusinessAreaForeign" Width="200"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="ctlViewPostForeign" runat="server" OnClick="ctlViewPostForeign_Click"
                                                Text="$View Post$" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlPostingDateForeignText" runat="server" Text="$Posting Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlPostingDateForeignReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>:
                                        </td>
                                        <td style="width: 15%">
                                            <uc1:Calendar ID="ctlPostingDateForeign" runat="server" />
                                            <ss:LabelExtender ID="ctlPostingDateForeignExtender" runat="server" LinkControlID="ctlPostingDateForeign"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.AdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="ctlValueDateForeignText" runat="server" Text="$Value Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlValueDateForeign" runat="server" SkinID="SkCalendarLabel"></asp:Label>
                                        </td>
                                        <td/>
                                        <td/>
                                        <td>
                                            <asp:Label ID="ctlPostingStatusForeignText" runat="server" Text="$Posting Status$"
                                                SkinID="SkFieldCaptionLabel"></asp:Label>:
                                            <asp:Label ID="ctlPostingStatusForeign" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <uc11:ViewPost ID="ctlViewPost" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<center>
    <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table">
                <tr>
                    <td align="left" style="color: Red;">
                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider2.Error">
                        </spring:ValidationSummary>
                        <spring:ValidationSummary ID="ctlValidationSummary1" runat="server" Provider="Provider.Error">
                        </spring:ValidationSummary>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</center>
