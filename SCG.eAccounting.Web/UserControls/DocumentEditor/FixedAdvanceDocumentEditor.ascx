<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FixedAdvanceDocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.FixedAdvanceDocumentEditor"
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
<%@ Register Src="~/UserControls/CalendarForFixedAdvance.ascx" TagName="CalendarForFixedAdvance"
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
                document.getElementById('<%= ctlCounterCashier.ClientID %>').style.display = 'none';
            }
            else {
                document.getElementById('<%= ctlDivCounterCashierDomesticText.ClientID %>').style.display = 'block';
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

    </script>
</ss:InlineScript>
<table width="100%" cellpadding="0" style="text-align: left" class="table" cellspacing="0">
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelHeader" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="ctlUpdatePanelHeaderProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHeader">
                        <ProgressTemplate>
                            <uc13:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table width="100%" border="0">
                        <tr>
                            <td align="left">
                                <uc3:DocumentHeader ID="ctlFixedAdvanceFormHeader" runat="server" labelNo="Advance No" />
                                <asp:Label ID="ctlFixedAdvanceType" runat="server" Style="display: none;"></asp:Label>
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
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Company %>'></ss:LabelExtender>
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
                                            <asp:Label ID="ctlSubjectText" runat="server" Text="$Subject$" SkinID="SkDocumentHeader2Label"
                                                meta:resourcekey="ctlSubjectTextResource1"></asp:Label>&nbsp;<asp:Label ID="ctlSubjectReq"
                                                    runat="server" SkinID="SkRequiredLabel" meta:resourcekey="ctlSubjectReqResource1"></asp:Label>&nbsp;:
                                        </td>
                                        <td style="width: 88%">
                                            <asp:TextBox ID="ctlSubject" runat="server" SkinID="SkGeneralTextBox" MaxLength="200"
                                                Width="400px" meta:resourcekey="ctlSubjectResource1"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlSubjectExtender" runat="server" LinkControlID="ctlSubject"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Subject %>'
                                                meta:resourcekey="ctlSubjectExtenderResource1"></ss:LabelExtender>
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
                                                ShowSMS="false" ShowFavoriteButton="false" ShowSearchUser="false" runat="server"
                                                width="100%" />
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                width="100%" />
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlReceiverData" Legend='<%# GetProgramMessage("ctlReceiverData") %>'
                                                ShowSMS="false" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                width="100%" />
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
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabGeneral" HeaderText="$General$" SkinID="SkFieldCaptionLabel"
                            meta:resourcekey="ctlTabGeneralResource1">
                            <HeaderTemplate>
                                <asp:Label ID="ctlGeneralLabel" runat="server" Text="$General$" SkinID="SkFieldCaptionLabel"
                                    meta:resourcekey="ctlGeneralLabelResource1"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelGeneral" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGeneral"
                                            EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading1" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:Label ID="ctlFixedAdvanceTypeLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                        Text="Fixed Advance Type"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:RadioButton ID="New" GroupName="FixedType" Text="New" runat="server" OnCheckedChanged="New_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <ss:LabelExtender ID="LabelExtender1" runat="server" LinkControlID="New" InitialFlag='<%# this.InitialFlag %>'
                                                        SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    <asp:RadioButton ID="Adjust" GroupName="FixedType" Text="Adjust" runat="server" meta:resourcekey="AdjustResource1"
                                                        OnCheckedChanged="Adjust_CheckedChanged" AutoPostBack="true" />
                                                    <ss:LabelExtender ID="LabelExtender2" runat="server" LinkControlID="Adjust" InitialFlag='<%# this.InitialFlag %>'
                                                        SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                                <div id="DivRefFixedAdvance" runat="server">
                                                    <td align="left" valign="middle" style="width: 30%">
                                                        <asp:Label ID="ctlReferredFixedAdvanceText" SkinID="SkFieldCaptionLabel" runat="server"
                                                            Text="Referred Fixed Advance"></asp:Label>
                                                        <asp:Label ID="refRequre" runat="server" SkinID="SkGeneralLabel" Style="color: Red;"
                                                            Text="*"></asp:Label>&nbsp;:&nbsp;
                                                        <asp:DropDownList ID="ctlReferredFixedAdvance" SkinID="SkGeneralDropdown" Width="198px"
                                                            runat="server" DataTextField="DocNo" DataValueField="FixedAdvanceID" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ctlReferredFixedAdvance_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="LabelExtender4" runat="server" LinkControlID="ctlReferredFixedAdvance"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    </td>
                                                </div>
                                            </tr>
                                            <div id="ctlDivDomesticAmount" runat="server">
                                                <tr>
                                                    <td align="left" style="width: 25%">
                                                        <asp:Label ID="ctlEffectiveFromText" SkinID="SkFieldCaptionLabel" runat="server"
                                                            Text="Effective Date From"></asp:Label>
                                                        <asp:Label ID="ctlEffectiveFromReq" SkinID="SkGeneralLabel" runat="server" Text="*"
                                                            Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" style="width: 25%">
                                                        <uc8:CalendarForFixedAdvance ID="ctlEffectiveDateFrom" runat="server" />
                                                        <asp:TextBox ID="ctlEffectiveDateFromDummy" SkinID="SkFieldCaptionLabel" runat="server"
                                                            ReadOnly="true" Style="background-color: #F5F5F5; width: 70px"></asp:TextBox>
                                                        <ss:LabelExtender ID="LabelExtender5" runat="server" LinkControlID="ctlEffectiveDateFrom"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    </td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="ctlEffectiveToText" runat="server" SkinID="SkFieldCaptionLabel" Text="Effective Date To"></asp:Label>
                                                                    <asp:Label ID="ctlEffectiveToReq" runat="server" SkinID="SkGeneralLabel" Style="color: Red;"
                                                                        Text="*"></asp:Label>
                                                                    &nbsp;:&nbsp;
                                                                </td>
                                                                <td>
                                                                    <uc1:Calendar ID="ctlEffectiveDateTo" runat="server" />
                                                                    <asp:TextBox ID="ctlEffectiveDateToDummy" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        ReadOnly="true" Style="background-color: #F5F5F5; width: 70px"></asp:TextBox>
                                                                    <ss:LabelExtender ID="LabelExtender6" runat="server" LinkControlID="ctlEffectiveDateTo"
                                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </div>
                                            <tr>
                                                <div id="DivPrevionsAmount" runat="server">
                                                    <td>
                                                        <asp:Label ID="ctlPrevionsAmount" SkinID="SkFieldCaptionLabel" runat="server" Text="Previons Amount(THB)"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="ctlPrevionsAmountValue" SkinID="SkFieldCaptionLabel" runat="server"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <ss:LabelExtender ID="LabelExtender10" runat="server" LinkControlID="ctlPrevionsAmountValue"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    </td>
                                                </div>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="ctlAmount" SkinID="SkFieldCaptionLabel" runat="server" Text="Amount(THB)"
                                                        meta:resourcekey="ctlAmountResource1"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="ctlAmountValue" runat="server" meta:resourcekey="ctlAmountValueResource1"
                                                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 10));" OnTextChanged="ctlAmountValue_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <ss:LabelExtender ID="LabelExtender7" runat="server" LinkControlID="ctlAmountValue"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    <asp:TextBox ID="ctlAmountGetValue" runat="server" meta:resourcekey="ctlAmountValueResource1"
                                                        Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <div id="DivCtlNetReceive" runat="server">
                                                    <td>
                                                        <asp:Label ID="ctlNetReceive" SkinID="SkFieldCaptionLabel" runat="server" Text="Net Receive/Return Amount(THB)"></asp:Label>&nbsp;:&nbsp;</br>
                                                        <asp:Label ID="ctlPayBackCompany" SkinID="SkFieldCaptionLabel" runat="server" Text="(Pay back)"
                                                            Style="color: Red;"></asp:Label>
                                                        <asp:Label ID="ctlReceiveCashFromCompany" SkinID="SkFieldCaptionLabel" runat="server"
                                                            Text="(Get more)" Style="color: Blue;"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="ctlNetReceiveValue" SkinID="SkFieldCaptionLabel" runat="server"
                                                            ReadOnly="true"></asp:TextBox>
                                                        <ss:LabelExtender ID="LabelExtender11" runat="server" LinkControlID="ctlNetReceiveValue"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                    </td>
                                                </div>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="ctrObjective" SkinID="SkFieldCaptionLabel" runat="server" Text="Objective"></asp:Label>
                                                    <asp:Label ID="Label1" runat="server" SkinID="SkGeneralLabel" Style="color: Red;"
                                                        Text="*"></asp:Label>
                                                    &nbsp;:&nbsp;
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="ctlObjectiveValue" runat="server" SkinID="SkGeneralTextBox" Width="300px"
                                                        Height="50px" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 500);"
                                                        Style="margin-left: 1px" onkeyup="return IsMaxLength(this, 500);"></asp:TextBox>
                                                    <ss:LabelExtender ID="LabelExtender8" runat="server" LinkControlID="ctlObjectiveValue"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="ctlRequestDateText" SkinID="SkFieldCaptionLabel" runat="server" Text="RequestDate"></asp:Label>
                                                    <asp:Label ID="ctlRequestDateReq" runat="server" SkinID="SkGeneralLabel" Style="color: Red;"
                                                        Text="*"></asp:Label>&nbsp;:&nbsp;
                                                </td>
                                                <td colspan="2">
                                                    <uc1:Calendar ID="ctlRequestDate" runat="server" />
                                                    <ss:LabelExtender ID="LabelExtender9" runat="server" LinkControlID="ctlRequestDate"
                                                        InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.RequestDate %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <div id="ctlDivDomesticPaymentType" runat="server">
                                                    <td align="left" valign="middle" style="width: 25%">
                                                        <asp:Label ID="ctlPaymentTypeText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Payment Type$"></asp:Label>
                                                        <asp:Label ID="ctlPaymentTypeReq" SkinID="SkGeneralLabel" runat="server" Text="*"
                                                            Style="color: Red;"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left" valign="middle" style="width: 30%">
                                                        <asp:DropDownList ID="ctlPaymentType" SkinID="SkGeneralDropdown" Width="198px" runat="server"
                                                            OnSelectedIndexChanged="ctlLblPaymentType_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlPaymentTypeExtender" runat="server" LinkControlID="ctlPaymentType"
                                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.PaymentType %>'></ss:LabelExtender>
                                                    </td>
                                                </div>
                                                <td align="left" valign="middle" style="width: 25%">
                                                    <div id="ctlDivCounterCashierDomesticText" runat="server">
                                                        <table>
                                                            <td>
                                                                <asp:Label ID="ctlCounterCashierInterText" SkinID="SkFieldCaptionLabel" runat="server"
                                                                    Text="$Counter / Cashier$"></asp:Label>
                                                                &nbsp;:&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ctlCounterCashier" SkinID="SkGeneralDropdown" Width="198px"
                                                                    runat="server" DataTextField="Text" DataValueField="ID" AutoPostBack="True" meta:resourcekey="ctlCounterCashierResource1">
                                                                </asp:DropDownList>
                                                                <ss:LabelExtender ID="ctlCounterCashierExtender" runat="server" LinkControlID="ctlCounterCashier"
                                                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.CounterCashier %>'
                                                                    meta:resourcekey="ctlCounterCashierExtenderResource1"></ss:LabelExtender>
                                                            </td>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <div id="ctlDivServiceTeam" runat="server">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="ctlServiceText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Service Team$"></asp:Label>
                                                        <asp:Label ID="ctlServiceReq" SkinID="SkRequiredLabel" Text="*" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ctlServiceTeam" SkinID="SkGeneralDropdown" Width="198px" runat="server"
                                                            DataTextField="Text" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <ss:LabelExtender ID="ctlServiceTeamExtender" runat="server" LinkControlID="ctlServiceTeam"
                                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.ServiceTeam %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                            </div>
                                            <div id="divreturnMedthod" runat="server">
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Label ID="ReturnMethodLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="ReturnMethod"></asp:Label>
                                                    </td>
                                                </tr>
                                                <!--Return Type -->
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="ReturnRequestDate" SkinID="SkFieldCaptionLabel" runat="server" Text="RequestDate"></asp:Label>
                                                        <asp:Label ID="ReturnRequestDateReq" runat="server" SkinID="SkGeneralLabel" Style="color: Red;"
                                                            Text="*"></asp:Label>
                                                        <ss:LabelExtender ID="LabelExtender12" runat="server" LinkControlID="ReturnRequestDateReq"
                                                            InitialFlag='<%# this.InitialFlag %>' Style="color: Red;" SkinID="SkGeneralLabel"
                                                            LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'></ss:LabelExtender>
                                                        &nbsp;:&nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <uc1:Calendar ID="ctlReturnRequestDate" runat="server" />
                                                        <ss:LabelExtender ID="LabelExtender3" runat="server" LinkControlID="ctlReturnRequestDate"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'></ss:LabelExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="ReturnPaymentDiv" runat="server">
                                                        <td align="left" valign="middle" style="width: 25%">
                                                            <asp:Label ID="ReturnPaymentText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Return Payment Type$"
                                                                meta:resourcekey="ctlPaymentTypeTextResource1"></asp:Label>
                                                            <asp:Label ID="ReturnPaymentReq" SkinID="SkGeneralLabel" runat="server" Text="*"
                                                                Style="color: Red;" meta:resourcekey="ctlPaymentTypeReqResource1"></asp:Label>
                                                            <ss:LabelExtender ID="LabelExtender13" runat="server" LinkControlID="ReturnPaymentReq"
                                                                SkinID="SkGeneralLabel" Style="color: Red;" InitialFlag='<%# this.InitialFlag %>'
                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'
                                                                meta:resourcekey="ctlPaymentTypeExtenderResource1"></ss:LabelExtender>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" valign="middle" style="width: 30%">
                                                            <asp:DropDownList ID="ctlReturnPaymentType" SkinID="SkGeneralDropdown" Width="198px"
                                                                runat="server" OnSelectedIndexChanged="ctlLblReturnPaymentType_SelectedIndexChanged"
                                                                AutoPostBack="True" meta:resourcekey="ctlPaymentTypeResource1">
                                                            </asp:DropDownList>
                                                            <ss:LabelExtender ID="ReturnPaymentLabel" runat="server" LinkControlID="ctlReturnPaymentType"
                                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'
                                                                meta:resourcekey="ctlPaymentTypeExtenderResource1"></ss:LabelExtender>
                                                        </td>
                                                    </div>
                                                    <td align="left" valign="middle" style="width: 25%">
                                                        <div id="ReturnCounterCashierDiv" runat="server">
                                                            <table>
                                                                <td>
                                                                    <asp:Label ID="ReturnCounterCashierLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                        Text="$Counter / Cashier$"></asp:Label>&nbsp;:&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ctlReturnCounterCashier" SkinID="SkGeneralDropdown" Width="198px"
                                                                        runat="server" DataTextField="Text" DataValueField="ID" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                    <ss:LabelExtender ID="ctlReturnCounterCashierExtender" runat="server" LinkControlID="ctlReturnCounterCashier"
                                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'></ss:LabelExtender>
                                                                </td>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <div id="ReturnServiceTeamDiv" runat="server" visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ReturnServiceTeamLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Service Team$" meta:resourcekey="ctlServiceTextResource1"></asp:Label>
                                                            <asp:Label ID="ReturnServiceTeamReq" SkinID="SkRequiredLabel" Text="*" runat="server"></asp:Label>
                                                            <ss:LabelExtender ID="LabelExtender14" runat="server" LinkControlID="ReturnServiceTeamReq"
                                                                SkinID="SkRequiredLabel" Style="color: Red;" InitialFlag='<%# this.InitialFlag %>'
                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'
                                                                meta:resourcekey="ctlServiceTeamExtenderResource1"></ss:LabelExtender>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ctlReturnServiceTeam" SkinID="SkGeneralDropdown" Width="198px"
                                                                runat="server" DataTextField="Text" DataValueField="ID" meta:resourcekey="ctlServiceTeamResource1">
                                                            </asp:DropDownList>
                                                            <ss:LabelExtender ID="LabelExtender33" runat="server" LinkControlID="ctlReturnServiceTeam"
                                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Return %>'></ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                </div>
                                                <!--Return Type -->
                                            </div>
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
                                        <ss:BaseGridView ID="ctlOutstandingGrid" runat="server" AutoGenerateColumns="False"
                                            CssClass="Grid" AllowSorting="True" OnRowDataBound="ctlOutstandingGrid_RowDataBound"
                                            OnRowCommand="ctlOutstandingGrid_RowCommand" AllowPaging="True" DataKeyNames="avDocumentID,expenseDocumentID"
                                            Width="100%" OnRequestCount="RequestCountOutstanding" OnRequestData="RequestDataOutstanding"
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
                                                <asp:TemplateField HeaderText="Advance No" SortExpression="b.documentno" meta:resourcekey="TemplateFieldResource2">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlAdvanceNo" runat="server" Text='<%# Bind("AdvanceNo") %>'
                                                            CommandName="ClickAdvanceNo" SkinID="SkCodeLabel" meta:resourcekey="ctlAdvanceNoResource1"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" SortExpression="b.subject" meta:resourcekey="TemplateFieldResource3">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlDescription" Mode="Encode" runat="server" Text='<%# Bind("Description") %>'
                                                            SkinID="SkGeneralLabel"></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="29%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AdvanceStatus" SortExpression="e.displayName" meta:resourcekey="TemplateFieldResource4">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAdvanceStatus" Mode="Encode" runat="server" Text='<%# Bind("AdvanceStatus") %>'
                                                            SkinID="SkGeneralLabel" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExpenseNo" SortExpression="h.documentNo" meta:resourcekey="TemplateFieldResource5">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ctlExpenseNo" runat="server" Text='<%# Bind("ExpenseNo") %>'
                                                            CommandName="ClickExpenseNo" SkinID="SkCodeLabel" meta:resourcekey="ctlExpenseNoResource1" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="13%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExpenseStatus" SortExpression="k.displayName" meta:resourcekey="TemplateFieldResource6">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlExpenseStatus" Mode="Encode" runat="server" Text='<%# Bind("ExpenseStatus") %>'
                                                            SkinID="SkGeneralLabel" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DueDate" SortExpression="a.duedateofremittance" meta:resourcekey="TemplateFieldResource7">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlDueDate" Mode="Encode" SkinID="SkCalendarLabel" runat="server"
                                                            Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DueDate")) %>'></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" SortExpression="a.amount" meta:resourcekey="TemplateFieldResource8">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkNodataLabel" Text='<%# GetMessage("NoDataFound") %>'
                                                    runat="server"></asp:Literal>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                            <SelectedRowStyle BackColor="#6699FF" />
                                        </ss:BaseGridView>
                                        <ss:BaseGridView ID="FixedAdvanceOutstandingGrid" runat="server" AutoGenerateColumns="False"
                                            CssClass="Grid" AllowSorting="True" OnRowDataBound="ctlFixedAdvanceOutstandingGrid_RowDataBound"
                                            OnRowCommand="ctlFixedAdvanceOutstandingGrid_RowCommand" AllowPaging="True" DataKeyNames="DocumentID"
                                            Width="100%" OnRequestCount="RequestCountFixedAdvanceOutstanding" OnRequestData="RequestDataFixedAdvanceOutstanding"
                                            ClearSortExpression="False" CustomPageIndex="0" EnableModelValidation="True"
                                            SaveButtonID="">
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
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabInitial" HeaderText="$Initial$" SkinID="SkFieldCaptionLabel"
                            meta:resourcekey="ctlTabInitialResource1">
                            <HeaderTemplate>
                                <asp:Label ID="ctlTabInitialText" runat="server" Text="$Initial$" SkinID="SkFieldCaptionLabel"
                                    meta:resourcekey="Label1Resource1"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelInitial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressInitial" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelInitial"
                                            EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading2" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc4:Initiator ID="ctlInitiator" runat="server" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Initiator %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAttachment" HeaderText="$Attachment$"
                            SkinID="SkFieldCaptionLabel" meta:resourcekey="ctlTabAttachmentResource1">
                            <HeaderTemplate>
                                <asp:Label ID="ctlTabAttachmentText" runat="server" Text="$Attachment$" SkinID="SkFieldCaptionLabel"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelAttachment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressAttachment" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                            EnableViewState="False">
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
                                <asp:Label ID="ctlTabMemoText" runat="server" Text="$Memo$" SkinID="SkFieldCaptionLabel"
                                    meta:resourcekey="Label3Resource1"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelMemo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressMemo" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMemo"
                                            EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc13:SCGLoading ID="SCGLoading4" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" cellpadding="10" cellspacing="10" width="100%">
                                            <tr>
                                                <td align="Center">
                                                    <asp:TextBox ID="ctlMemo" runat="server" TextMode="MultiLine" Height="300px" Width="900px"
                                                        SkinID="SkGeneralTextBox" onkeypress="return IsMaxLength(this, 1000);" onkeyup="return IsMaxLength(this, 1000);"
                                                        meta:resourcekey="ctlMemoResource1"></asp:TextBox>
                                                    <ss:LabelExtender ID="ctlMemoExtender" runat="server" LinkControlID="ctlMemo" SkinID="SkMultiLineLabel"
                                                        Width="800px" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.Memo %>'
                                                        meta:resourcekey="ctlMemoExtenderResource1"></ss:LabelExtender>
                                                </td>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabHistory" HeaderText="$History$" SkinID="SkFieldCaptionLabel"
                            meta:resourcekey="ctlTabHistoryResource1">
                            <HeaderTemplate>
                                <asp:Label ID="ctlTabHistoryText" runat="server" Text="$History$" SkinID="SkFieldCaptionLabel"
                                    meta:resourcekey="ctlTabHistoryTextResource1"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelHistory" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressHistory" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHistory"
                                            EnableViewState="False">
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
            <asp:UpdatePanel ID="ctlUpdatePanelApprover" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:ActorData ID="ctlApproverData" Legend='<%# GetProgramMessage("ctlApproverData") %>'
                        ShowSMS="true" ShowFavoriteButton="true" ShowSearchUser="true" IsApprover="true"
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
        <asp:UpdateProgress ID="ctlUpdatePanelViewPostProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewPost">
            <ProgressTemplate>
                <uc13:SCGLoading ID="SCGLoading6" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div align="left">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="ctlDivViewDetailDomestic" runat="server" style="display: inline;">
                            <fieldset id="ctlFieldSetVerifyDetailDomestic" runat="server" style="width: 99%;">
                                <legend id="ctlLegendVerifyDetailDomesticView" style="color: #4E9DDF;">
                                    <asp:Label ID="ctlVerifyDetailDomesticHeader" runat="server" Text="$Verify Detail$"
                                        SkinID="SkGeneralLabel"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlBranchTextDomestic" runat="server" Text="$Branch$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlBranchTextDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>&nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox ID="ctlBranchDomestic" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"
                                                meta:resourcekey="ctlBranchDomesticResource1"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBranchDomesticExtender" runat="server" LinkControlID="ctlBranchDomestic"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.VerifyDetail %>'
                                                meta:resourcekey="ctlBranchDomesticExtenderResource1"></ss:LabelExtender>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlPaymentMethodDomesticText" runat="server" Text="$Payment Method$"
                                                SkinID="SkFieldCaptionLabel" meta:resourcekey="ctlPaymentMethodDomesticTextResource1"></asp:Label>
                                            <%--<asp:Label ID="ctlPaymentMethodDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>&nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ctlPaymentMethodDomestic" SkinID="SkGeneralDropdown" Width="170px"
                                                runat="server" meta:resourcekey="ctlPaymentMethodDomesticResource1">
                                            </asp:DropDownList>
                                            <ss:LabelExtender ID="ctlPaymentMethodDomesticExtender" runat="server" LinkControlID="ctlPaymentMethodDomestic"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.VerifyDetail %>'
                                                meta:resourcekey="ctlPaymentMethodDomesticExtenderResource1"></ss:LabelExtender>
                                        </td>
                                        <td style="width: 40%">
                                            <div id="ctlDivSupplementFixedAdvance" runat="server">
                                                <table>
                                                    <td>
                                                        <asp:Label ID="ctlSupplementaryDomesticText" runat="server" Text="Supplementary:"
                                                            SkinID="SkFieldCaptionLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="ctlSupplementary" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                        <ss:LabelExtender ID="ctlSupplementaryLabelExtender" runat="server" LinkControlID="ctlSupplementary"
                                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.VerifyDetail %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                </table>
                                            </div>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Button ID="ctlViewPostButtonDomestic" runat="server" Text="View Post" OnClick="ctlViewPostButtonDomestic_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlPostingDateDomestic" runat="server" Text="$Posting Date$" SkinID="SkFieldCaptionLabel"
                                                meta:resourcekey="ctlPostingDateDomesticResource1"></asp:Label>
                                            <%--<asp:Label ID="ctlPostingDateDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>&nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <uc1:Calendar ID="ctlPostingDateCalendarDomestic" runat="server" />
                                            <ss:LabelExtender ID="ctlPostingDateCalendarDomesticExtender" runat="server" LinkControlID="ctlPostingDateCalendarDomestic"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.VerifyDetail %>'
                                                meta:resourcekey="ctlPostingDateCalendarDomesticExtenderResource1"></ss:LabelExtender>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlBaselineDateDomesticText" runat="server" Text="$Baseline Date$"
                                                SkinID="SkFieldCaptionLabel" meta:resourcekey="ctlBaselineDateDomesticTextResource1"></asp:Label>
                                            :
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlBaselineDate" runat="server" SkinID="SkGeneralLabel" meta:resourcekey="ctlBaselineDateResource1"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlPostingStatusDomesticText" runat="server" Text="$Posting Status$"
                                                SkinID="SkGeneralLabel" meta:resourcekey="ctlPostingStatusDomesticTextResource1"></asp:Label>:
                                            <asp:Label ID="ctlPostingStatusDomestic" runat="server" SkinID="SkGeneralLabel" meta:resourcekey="ctlPostingStatusDomesticResource1"></asp:Label>
                                        </td>
                                        <div id="DivBankAccountGL" runat="server">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Label ID="BankAccountGLText" runat="server" Text="Bank Account/GL Account" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td style="width: 20%">
                                                    <asp:TextBox ID="BankAccountGL" runat="server" SkinID="SkGeneralTextBox" MaxLength="6"></asp:TextBox>
                                                    <ss:LabelExtender ID="LabelExtender18" runat="server" LinkControlID="BankAccountGL"
                                                        SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                        </div>
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
<asp:UpdatePanel ID="ctlUpdatePanelViewPostReturn" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewPost">
            <ProgressTemplate>
                <uc13:SCGLoading ID="SCGLoadingReturn" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div align="left">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="ctlDivViewDetailDomesticReturn" runat="server" style="display: inline;">
                            <fieldset id="Fieldset1" runat="server" style="width: 99%;">
                                <legend id="Legend1" style="color: #4E9DDF;">
                                    <asp:Label ID="VerifyReturnDetailReturn" runat="server" Text="$VerifyReturn Detail$"
                                        SkinID="SkGeneralLabel"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="BranchReturnText" runat="server" Text="$Branch$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox ID="ctlBranchRetunr" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"></asp:TextBox>
                                            <ss:LabelExtender ID="LabelExtender15" runat="server" LinkControlID="ctlBranchRetunr"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.ClearingReturn %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="PaymentMethodReturnText" runat="server" Text="$Payment Method$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ctlPaymentMethodReturnPost" SkinID="SkGeneralDropdown" Width="170px"
                                                runat="server">
                                            </asp:DropDownList>
                                            <ss:LabelExtender ID="LabelExtender16" runat="server" LinkControlID="ctlPaymentMethodReturnPost"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.ClearingReturn %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Button ID="PostReturn" runat="server" Text="View Post" OnClick="ctlViewPostButtonDomesticReturn_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="PostingDateReturnText" runat="server" Text="$Posting Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            <%--<asp:Label ID="ctlPostingDateDomesticReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>--%>&nbsp;
                                        </td>
                                        <td style="width: 20%">
                                            <uc1:Calendar ID="ctlPostingDateReturn" runat="server" />
                                            <ss:LabelExtender ID="LabelExtender17" runat="server" LinkControlID="ctlPostingDateReturn"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.ClearingReturn %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="BaselineDateReturnText" runat="server" Text="$Baseline Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                            :
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="ctlBaselineDateReturn" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="PostingStatusReturn" runat="server" Text="$Posting Status$" SkinID="SkGeneralLabel"></asp:Label>:
                                            <asp:Label ID="ctlPostingStatusReturn" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <div id="DivBankAccountText" runat="server">
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Label ID="BankAccountText" runat="server" Text="Bank Account/GL Account" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td style="width: 20%">
                                                <asp:TextBox ID="ctlBankAccount" runat="server" SkinID="SkGeneralTextBox" MaxLength="6"></asp:TextBox>
                                                <ss:LabelExtender ID="LabelExtender55" runat="server" LinkControlID="ctlBankAccount"
                                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.FixedAdvanceFieldGroup.ClearingReturn %>'></ss:LabelExtender>
                                            </td>
                                        </tr>
                                    </div>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <uc11:ViewPost ID="ctlViewPostReturn" runat="server" />
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
