<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemittanceDocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.RemittanceDocumentEditor" %>
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
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/StatusDropdown.ascx" TagName="StatusDropdown"
    TagPrefix="uc10" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/TALookup.ascx" TagName="TALookup" TagPrefix="uc11" %>
<%@ Register Src="~/UserControls/LOV/AV/AdvanceLookup.ascx" TagName="AdvanceLookup"
    TagPrefix="uc13" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc7" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/CounterCashier.ascx" TagName="CounterCashier"
    TagPrefix="uc12" %>
<%@ Register Src="~/UserControls/ViewPost/ViewPost.ascx" TagName="ViewPost" TagPrefix="uc13" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/History.ascx" TagName="History"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc14" %>

<script type="text/jscript">
    function calAmountTHB(excRateObj, currAmtObj, amtThbObj, divAmtObj, advCurrAmtObj) {
        // ================= Calculate Amount THB on the record =================
        var numFormat = new NumberFormat(0);
        var excRate = 0; var currAmt = 0;
        var advAmtThb = 0; var currAdvAmt = 0;
        var amtThbObj2;

        excRateObj = document.getElementById(excRateObj);
        var excRateObj2 = excRateObj.getElementsByTagName('TD');


        currAmtObj = document.getElementById(currAmtObj);
        amtThbObj = document.getElementById(amtThbObj);
        var childItem = amtThbObj.getElementsByTagName('TD');
        var amtThbObj2 = childItem[8];



        var divAmt = document.getElementById(divAmtObj);
        advCurrAmtObj = document.getElementById(advCurrAmtObj).childNodes[3];
        if (excRateObj != null && excRateObj.value != null)
            excRate = parseFloat('0' + excRateObj.value.replace(/\,/g, '')).toFixed(5); 
        else if (excRateObj != null && excRateObj.innerText != null)
            excRate = parseFloat('0' + excRateObj2[6].innerText.replace(/\,/g, '')).toFixed(5);  
        if (currAmtObj != null && currAmtObj.value != '')
            currAmt = parseFloat('0' + currAmtObj.value.replace(/\,/g, '')).toFixed(2);
        if (advCurrAmtObj != null && advCurrAmtObj.innerText != '')
            currAdvAmt = parseFloat('0' + advCurrAmtObj.innerText.replace(/\,/g, '')).toFixed(2);

        numFormat.setNumber(parseFloat(excRate * currAmt));
        amtThbObj2.innerText = numFormat.toFormatted();
        if (currAmt > currAdvAmt) {
            numFormat.setNumber(parseFloat(currAmt - currAdvAmt));
        } else {
            numFormat.setNumber(parseFloat(currAdvAmt - currAmt));
        }
        divAmt.value = numFormat.toFormatted();
        // ================= Calculate Total Amount THB =================
        var idTemplate = '<%= ctlRemittanceItemGrid.ClientID %>_ctl0{0}'; //_ctlAmountTHB';
        var itemIndexer = 2;
        var totalRemitAmtThb = 0;
        var totalDivAmt = 0;
        var seq = document.getElementById(idTemplate.replace('{0}', itemIndexer + '')); //keep first seq for add total remittance amt
        var advAmount = 0;
        var rmtAmount = 0;
        amtThbObj = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
        amtThbObj = amtThbObj.getElementsByTagName('TD');
        if(amtThbObj != null)
            amtThbObj = amtThbObj[8];
        while (amtThbObj != null) {
            if (seq.childNodes[0].innerText != '') {
                totalRemitAmtThb += parseFloat('0' + amtThbObj.innerText.replace(/\,/g, '')).toFixed(2) * 1; //new amount
                advAmount = (parseFloat('0' + seq.childNodes[3].innerText.replace(/\,/g, '')).toFixed(2) * 1);
                rmtAmount = (parseFloat('0' + seq.childNodes[5].getElementsByTagName("input")[0].value.replace(/\,/g, '')).toFixed(2) * 1);

                if (advAmount >= rmtAmount)
                    totalDivAmt += advAmount - rmtAmount;
                else
                    totalDivAmt += rmtAmount - advAmount;
            }
            itemIndexer += 1;
            amtThbObj = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
            seq = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
            if (amtThbObj != null) {
                amtThbObj = amtThbObj.getElementsByTagName('TD');
                amtThbObj = amtThbObj[8];
            }
        }

        // ================= Check condition for hide checkbox ctlFullReturnCashChk =================
        var totalRemitAmtThbObj = document.getElementById('<%= ctlTotalRemittanceAmountLabel.ClientID %>');
        var totalDivAmtObj = document.getElementById("<%= ctlTotalDivAmount.ClientID %>");
        numFormat.setNumber(totalRemitAmtThb);
        totalRemitAmtThbObj.innerText = numFormat.toFormatted();

        numFormat.setNumber(totalDivAmt);
        totalDivAmtObj.innerText = numFormat.toFormatted();
        
        var totalAdvAmtObj = document.getElementById("<%= ctlTotalAdvanceAmountLabel.ClientID %>");
        var chkFullReturn = document.getElementById("<%= ctlFullReturnCashChk.ClientID %>");

        if ((totalRemitAmtThb == parseFloat(totalAdvAmtObj.innerText.replace(/\,/g, ''))) || (totalDivAmt == 0)) {
            chkFullReturn.parentNode.style.display = 'inline';
            chkFullReturn.style.display = 'inline';
        }
        else {
            chkFullReturn.parentNode.style.display = 'none';
            chkFullReturn.style.display = 'none';
            chkFullReturn.checked = false;            
        }
    }

    function calAmountTHBForRepOffice(excRateObj, currAmtObj, amtThbObj, divAmtObj, advCurrAmtObj, excRatetoTHB,amtMainObj,divMainAmtObj) {
        // ================= Calculate Amount THB on the record =================
        var numFormat = new NumberFormat(0);
        var excRate = 0; var currAmt = 0;
        var excRateTHB = 0;
        var advAmtThb = 0; var currAdvAmt = 0;
        //excRateObj = document.getElementById(excRateObj).childNodes[4];
        excRateObj = document.getElementById(excRateObj);
        excRatetoTHB = document.getElementById(excRatetoTHB);
        currAmtObj = document.getElementById(currAmtObj);
        //amtThbObj = document.getElementById(amtThbObj).childNodes[7];
        amtMainObj = document.getElementById(amtMainObj).childNodes[6];
        var divAmt = document.getElementById(divAmtObj);
        advCurrAmtObj = document.getElementById(advCurrAmtObj).childNodes[3];
        if (excRateObj != null && excRateObj.value != null)
            excRate = parseFloat('0' + excRateObj.value.replace(/\,/g, '')).toFixed(5);
        else if (excRateObj != null && excRateObj.innerText != null)
            excRate = parseFloat('0' + excRateObj.childNodes[4].innerText.replace(/\,/g, '')).toFixed(5);
        if (currAmtObj != null && currAmtObj.value != '')
            currAmt = parseFloat('0' + currAmtObj.value.replace(/\,/g, '')).toFixed(2);
        if (advCurrAmtObj != null && advCurrAmtObj.innerText != '')
            currAdvAmt = parseFloat('0' + advCurrAmtObj.innerText.replace(/\,/g, '')).toFixed(2);
        excRateTHB = parseFloat(excRatetoTHB.innerHTML);
//        if (excRatetoTHB != null && excRatetoTHB.value != null)
//            excRateTHB = parseFloat('0' + excRatetoTHB.value.replace(/\,/g, '')).toFixed(4);
//        else if (excRatetoTHB != null && excRatetoTHB.innerText != null)
//            excRateTHB = parseFloat('0' + excRatetoTHB.childNodes[4].innerText.replace(/\,/g, '')).toFixed(4);
        numFormat.setNumber(parseFloat(excRate * currAmt));
        amtMainObj.innerText = numFormat.toFormatted();
//        numFormat.setNumber(parseFloat((excRate * currAmt)*excRateTHB));
//        amtThbObj.innerText = numFormat.toFormatted();
        if (currAmt > currAdvAmt) {
            numFormat.setNumber(parseFloat(currAmt - currAdvAmt));
        } else {
            numFormat.setNumber(parseFloat(currAdvAmt - currAmt));
        }
        divAmt.value = numFormat.toFormatted();
        // ================= Calculate Total Amount THB =================
        var idTemplate = '<%= ctlRemittanceItemGrid.ClientID %>_ctl0{0}'; //_ctlAmountTHB';
        var itemIndexer = 2;
        var totalRemitAmtThb = 0;
        var totalDivAmt = 0;
        var seq = document.getElementById(idTemplate.replace('{0}', itemIndexer + '')); //keep first seq for add total remittance amt
        var advAmount = 0;
        var rmtAmount = 0;
        amtMainObj = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
        if (amtMainObj != null)
            amtMainObj = amtMainObj.childNodes[6];
        while (amtMainObj != null) {
            if (seq.childNodes[0].innerText != '') {
                totalRemitAmtThb += parseFloat('0' + amtMainObj.innerText.replace(/\,/g, '')).toFixed(2) * 1; //new amount
                advAmount = (parseFloat('0' + seq.childNodes[3].innerText.replace(/\,/g, '')).toFixed(2) * 1);
                rmtAmount = (parseFloat('0' + seq.childNodes[5].getElementsByTagName("input")[0].value.replace(/\,/g, '')).toFixed(2) * 1);
                //alert(advAmount + " : " + rmtAmount + " : " + totalDivAmt);
                if (advAmount >= rmtAmount)
                    totalDivAmt += advAmount - rmtAmount;
                else
                    totalDivAmt += rmtAmount - advAmount;
            }
            itemIndexer += 1;
            amtMainObj = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
            seq = document.getElementById(idTemplate.replace('{0}', itemIndexer + ''));
            if (amtMainObj != null)
                amtMainObj = amtMainObj.childNodes[6];
        }

        // ================= Check condition for hide checkbox ctlFullReturnCashChk =================
        var totalRemitAmtThbObj = document.getElementById('<%= ctlTotalRemittanceAmountLabel.ClientID %>');
        var totalDivAmtObj = document.getElementById("<%= ctlTotalDivAmount.ClientID %>");
        numFormat.setNumber(totalRemitAmtThb);
        totalRemitAmtThbObj.innerText = numFormat.toFormatted();

        numFormat.setNumber(totalDivAmt);
        totalDivAmtObj.innerText = numFormat.toFormatted();

        var totalAdvAmtObj = document.getElementById("<%= ctlTotalAdvanceAmountLabel.ClientID %>");
        var chkFullReturn = document.getElementById("<%= ctlFullReturnCashChk.ClientID %>");

        if ((totalRemitAmtThb == parseFloat(totalAdvAmtObj.innerText.replace(/\,/g, ''))) || (totalDivAmt == 0)) {
            chkFullReturn.parentNode.style.display = 'inline';
            chkFullReturn.style.display = 'inline';
        }
        else {
            chkFullReturn.parentNode.style.display = 'none';
            chkFullReturn.style.display = 'none';
            chkFullReturn.checked = false;
        }
    }
</script>

<table width="100%" cellpadding="0" style="text-align: left">
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelHeader" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="ctlUpdatePanelHeaderProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHeader"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc14:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <uc3:DocumentHeader ID="ctlRemittanceFormHeader" runat="server" />
                                <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                                <asp:Label ID="ctlAdvanceType" runat="server" Style="display: none;"></asp:Label>
                                <asp:Label ID="ctlTempTADocumentID" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 9%">
                                            <asp:Label ID="ctlCompanyLabel" SkinID="SkDocumentHeader2Label" runat="server" Text="Company"></asp:Label>
                                            <asp:Label ID="Label5" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td>
                                            <uc6:CompanyField ID="ctlCompaymyField" runat="server" />
                                            <ss:LabelExtender ID="ctlCompaymyFieldExtender" runat="server" LinkControlID="ctlCompaymyField"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.Company %>'>
                                            </ss:LabelExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 9%">
                                            <asp:Label ID="ctlSubjectLabel" SkinID="SkDocumentHeader2Label" runat="server" Text="Subject"></asp:Label>
                                            <asp:Label ID="ctlSubjecyRequired" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ctlSubject" runat="server" Width="55%" MaxLength="200" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlSubjectExtender" runat="server" LinkControlID="ctlSubject"
                                                InitialFlag='<%# this.InitialFlag %>' Width="98%" SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                            </ss:LabelExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlCreatorData" Legend='<%# GetProgramMessage("ctlCreatorData") %>'
                                                ShowSMS="false" ShowVendorCode="false" ShowFavoriteButton="false" ShowSearchUser="false" runat="server"
                                                Width="100%" />
                                            <%--<ss:LabelExtender ID="ctlCreatorDatatExtender" runat="server" LinkControlID="ctlCreatorData"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                            </ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                Width="100%" />
                                            <%--<ss:LabelExtender ID="ctlRequesterDataExtender" runat="server" LinkControlID="ctlRequesterData"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                            </ss:LabelExtender>--%>
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
            
            <asp:UpdatePanel ID="UpdatePanelRemittanceTab" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            
            <ajaxToolkit:TabContainer ID="ctlAdvanceTabContainer" runat="server" ActiveTabIndex="0">
                <ajaxToolkit:TabPanel runat="server" ID="ctlTabGeneral" HeaderText="General">
                    <HeaderTemplate>
                        <asp:Label ID="ctlGeneralLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$General$"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="ctlUpdatePanelGeneral" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGeneral"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc14:SCGLoading ID="SCGLoading2" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table border="0" width="100%" style="text-align: left;">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlAdvanceInformationLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                Text="$Advance's Information$"></asp:Label>
                                            <%--<asp:Button ID="ctlAddAddvance" runat="server" Text="Add Advance" SkinID="SkGeneralButton"
                                                OnClick="ctlAdvanceLookup_Click" />--%>
                                            <asp:ImageButton ID="ctlAddAddvance" runat="server" SkinID="SkAddButton" OnClick="ctlAdvanceLookup_Click" />
                                            <uc13:AdvanceLookup ID="ctlAdvanceLookup" runat="server" isMultiple="true" />
                                            <ss:LabelExtender ID="ctlAdvanceLookupExtender" runat="server" LinkControlID="ctlAdvanceLookup"
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                            </ss:LabelExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <ss:BaseGridView ID="ctlAdvanceGridView" runat="server" OnRowCommand="ctlAdvanceGridview_RowCommand"
                                                OnRowDataBound="ctlAdvanceGridview_RowDataBound" AutoGenerateColumns="False"
                                                OnDataBound="ctlAdvanceGridview_DataBound" DataKeyNames="AdvanceID,RemittanceID"
                                                EnableInsert="False" ShowMsgDataNotFound="False" InsertRowCount="1" SaveButtonID="" Width="100%" CssClass="Grid">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="No.">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlNoLabel" Mode="Encode" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Advance No.">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="ctlLbtnAdvanceNo" runat="server" CommandName="AdvanceLink" Text='<%# Bind("DocumentNo") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctllblDescription" Mode="Encode" runat="server" Text='<%# Bind("Description") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Requester">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLblRequester" Mode="Encode" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Receiver">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="cltLblReceiver" Mode="Encode" runat="server" Text='<%# Bind("ReceiverName") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Date">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="cltLblDueDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDateOfRemittance")) %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount(USD)">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLblMainCurrencyAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                            <asp:HiddenField ID="ctlExchangeRateMainToTHBCurrency" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ExchangeRateMainToTHBCurrency", "{0:#,##0.00000}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount(THB)">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ctlLblAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CommandName="DeleteAdvance" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </ss:BaseGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <div id="ctlTotalAdvanceDiv" runat="server" visible="false">
                                                <asp:Label ID="ctlTotalAdvanceAmount" SkinID="SkGeneralLabel" Text="Total" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                <asp:Label ID="ctlTotalAdvanceAmountLabel" SkinID="SkNumberLabel" runat="server"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 40%;">
                                                        <asp:Label ID="ctlTANoText" SkinID="SkFieldCaptionLabel" runat="server" Text="$TA No.$"></asp:Label>&nbsp;:&nbsp;
                                                        <%--<asp:Label ID="ctlTANoLabel" SkinID="SkGeneralLabel" runat="server"></asp:Label>--%>
                                                        <asp:LinkButton ID="ctlTANo" SkinID="SkCtlLinkButton" runat="server" OnClick="ctlTANo_Click"
                                                            Visible="false"></asp:LinkButton>
                                                        <asp:Label ID="ctlTANoLbl" SkinID="SkGeneralLabel" runat="server" Text="N/A"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%;">
                                                        <asp:ImageButton ID="ctlTANoLookup" runat="server" SkinID="SkSearchButton" OnClick="ctlTANoLookup_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ctlDeleteTA" runat="server" SkinID="SkDeleteButton" OnClick="ctlDeleteTA_Click" />
                                                        <uc11:TALookup ID="ctlTALookup" runat="server" isQueryForRemittance="true" isMultiple="false" />
                                                        <ss:LabelExtender ID="ctlTALookupExtender" runat="server" LinkControlID="ctlTALookup"
                                                            InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                        </ss:LabelExtender>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="ctlExchangeRateLbl" SkinID="SkGeneralLabel" runat="server" Text="$ExchangeRateToTHB$" Visible="true"/>&nbsp;&nbsp;
                                                        <asp:Label ID="ctlExchangeRateToTHB" SkinID="SkGeneralLabel" runat="server" Text="0.00" Visible="true"/>&nbsp;&nbsp;
                                                        <asp:Label ID="ctlExchangeRateToTHBLbl" SkinID="SkGeneralLabel" runat="server" Text="$ExchangeRateCurrency$" Visible="true"/>
                                                        <asp:HiddenField ID="ctlExchangeRateToTHBHField" runat="server"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="ctlRemittanceInformation" SkinID="SkFieldCaptionLabel" runat="server"
                                                Text="$Remittance's Information$"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <ss:BaseGridView ID="ctlRemittanceItemGrid" runat="server" AutoGenerateColumns="false"
                                                Width="100%" CssClass="Grid" ReadOnly="false" DataKeyNames="RemittanceItemID"
                                                EnableViewState="true" ShowFooter='<%# this.isShowFooter %>' ShowHeader="true"
                                                 ShowMsgDataNotFound="False" OnRowCommand="ctlRemittanceItemGrid_RowCommand" OnRowDataBound="ctlRemittanceItemGrid_RowDataBound"
                                                OnDataBound="ctlRemittanceItemGrid_DataBound">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <RowStyle CssClass="GridItem" HorizontalAlign="center" />
                                                <InsertRowStyle CssClass="GridItem" HorizontalAlign="center" />
                                                <FooterStyle CssClass="GridItem" HorizontalAlign="center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="No." HeaderStyle-HorizontalAlign="Center">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:Literal ID="ctlNo" Mode="Encode" runat="server"></asp:Literal>
                                                            </center>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlNo" runat="server"></asp:Literal>
                                                        </FooterTemplate>
                                                        <FooterStyle Width="5px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Type" HeaderStyle-HorizontalAlign="Center">
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="ctlTempPaymentMethodValue" runat="server" Value='<%# Bind("PaymentType") %>' />                                                             
                                                            <uc10:StatusDropdown ID="ctlPaymentTypeDropdown" runat="server" />
                                                            <ss:LabelExtender ID="ctlPaymentTypeDropdownExtender" runat="server" LinkControlID="ctlPaymentTypeDropdown"
                                                                InitialFlag='<%# this.InitialFlag %>' Width="98%" SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <uc10:StatusDropdown ID="ctlPaymentTypeDropdown" runat="server" />
                                                            <ss:LabelExtender ID="ctlPaymentTypeDropdownExtender" runat="server" LinkControlID="ctlPaymentTypeDropdown"
                                                                InitialFlag='<%# this.InitialFlag %>' Width="98%" SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center">
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="ctlTempCurrencyValue" runat="server" Value='<%# Bind("CurrencyID") %>' />
                                                            <uc7:CurrencyDropdown ID="ctlCurrencyDropdown" runat="server" />
                                                            <ss:LabelExtender ID="ctlCurrencyDropdownExtender" runat="server" LinkControlID="ctlCurrencyDropdown"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <itemstyle horizontalalign="Center" />
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <uc7:CurrencyDropdown ID="ctlCurrencyDropdown" runat="server" />
                                                            <ss:LabelExtender ID="ctlCurrencyDropdownExtender" runat="server" LinkControlID="ctlCurrencyDropdown"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Foreign Currency Advanced" HeaderStyle-HorizontalAlign="Center">
                                                        <EditItemTemplate>
                                                            <asp:Literal ID="ctlFCurrencyAdv" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "ForeignCurrencyAdvanced", "{0:#,##0.00}") %>'></asp:Literal>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlFCurrencyAdv" Mode="Encode" runat="server" SkinID="SkNumberLabel"></asp:Literal>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exchange Rate">
                                                        <EditItemTemplate>
                                                            <asp:Literal ID="ctlExchangRateLabel" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "ExchangeRate", "{0:#,##0.00000}") %>'></asp:Literal>
                                                            <asp:HiddenField ID="ctlAmountTHBAdvance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ForeignAmountTHBAdvanced", "{0:#,##0.00}") %>' />
                                                            <asp:HiddenField ID="ctlAmountMainCurrencyAdvance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ForeignAmountMainCurrencyAdvanced", "{0:#,##0.00}") %>' />
                                                            <asp:HiddenField ID="ctlExchangeRateTHB" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ExchangeRateTHB", "{0:#,##0.00000}") %>' />
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="ctlExchangeRate" runat="server" SkinID="SkNumberTextBox" Style="text-align: right;"
                                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 12, 5));" OnKeyDown="disablePasteOption();"
                                                                OnKeyUp="disablePasteOption();" Width="90%" MaxLength="21"></asp:TextBox>
                                                            <asp:HiddenField ID="ctlAmountMainCurrencyAdvance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ForeignAmountMainCurrencyAdvanced", "{0:#,##0.00}") %>'  />
                                                            <asp:HiddenField ID="ctlExchangeRateTHB" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ExchangeRateTHB", "{0:#,##0.00000}") %>' />
                                                            <ss:LabelExtender ID="ctlExchangeRateExtender" runat="server" LinkControlID="ctlExchangeRate"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Foreign Currency Remitted" HeaderStyle-HorizontalAlign="Center">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="ctlFCurrencyRem" runat="server" SkinID="SkNumberTextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ForeignCurrencyRemitted", "{0:#,##0}") %>'
                                                                Style="text-align: right;" OnKeyPress="return isKeyInt();"
                                                                OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Width="90%"
                                                                MaxLength="21"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlFCurrencyRemExtender" runat="server" LinkControlID="ctlFCurrencyRem"
                                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:HiddenField ID="ctlDivAmount" runat="server" />
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="ctlFCurrencyRem" runat="server"  SkinID="SkNumberTextBox" Style="text-align: right;"
                                                                OnKeyPress="return isKeyInt();" OnKeyDown="disablePasteOption();"
                                                                OnKeyUp="disablePasteOption();" Width="90%" MaxLength="21"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlFCurrencyRemExtender" runat="server" LinkControlID="ctlFCurrencyRem"
                                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:HiddenField ID="ctlDivAmount" runat="server" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount(USD)">
                                                        <EditItemTemplate>
                                                            <asp:Literal ID="ctlMainCurrencyAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "MainCurrencyAmount", "{0:#,##0.00}") %>'></asp:Literal>
                                                            
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlMainCurrencyAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel"></asp:Literal></EditItemTemplate>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount(THB)">
                                                        <EditItemTemplate>
                                                            <asp:Literal ID="ctlAmountTHB" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "AmountTHB", "{0:#,##0.00}") %>'></asp:Literal>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ctlAmountTHB" Mode="Encode" runat="server" SkinID="SkNumberLabel"></asp:Literal></EditItemTemplate>
                                                        </FooterTemplate>
                                                        <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CommandName="DeleteRemitanceItem" /></EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton ID="ctlAdd" runat="server" SkinID="SkAddButton" CommandName="AddRemittanceItem" /></FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </ss:BaseGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <div id="ctlTotalRemittanceDiv" runat="server" visible="false">
                                                <asp:Label ID="ctlTotalRemittanceAmount" Text="Total" SkinID="SkGeneralLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                <asp:Label ID="ctlTotalRemittanceAmountLabel" SkinID="SkNumberLabel" runat="server"></asp:Label>
                                                <asp:HiddenField ID="ctlTotalDivAmount" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="ctlFullReturnCashChk" Style="display: none;" runat="server" Text="คืนเต็มจำนวนโดยไม่เบิกค่าใช้จ่าย" OnCheckedChanged="ctlFullReturnCashChk_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ctlFullReturnCashChk"  />
                            </Triggers>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="ctlTabAttachment" HeaderText="$Attachment$">
                    <HeaderTemplate>
                        <asp:Label ID="Label8" SkinID="SkFieldCaptionLabel" runat="server" Text="$Attachment$"></asp:Label></HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="ctlUpdatePanelAttechment" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttechment"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc14:SCGLoading ID="SCGLoading3" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <uc5:Attachment ID="ctlAttachment" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="ctlTabMemo" HeaderText="$Memo$">
                    <HeaderTemplate>
                        <asp:Label ID="Label7" SkinID="SkFieldCaptionLabel" runat="server" Text="$Memo$"></asp:Label></HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="ctlUpdatePanelMemo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="ctlUpdateProgressMemo" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMemo"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc14:SCGLoading ID="SCGLoading4" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table border="0" cellpadding="10" cellspacing="10" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="ctlMemo" runat="server" TextMode="MultiLine" Height="300px" Width="90%" onkeypress="return IsMaxLength(this, 1000);" onkeyup="return IsMaxLength(this, 1000);"
                                                SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlMemoLabelExtender" runat="server" LinkControlID="ctlMemo" 
                                                InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                            </ss:LabelExtender>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="ctlTabHistory" HeaderText="History">
                    <HeaderTemplate>
                        <asp:Label ID="Label6" SkinID="SkFieldCaptionLabel" runat="server" Text="$History$"></asp:Label></HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="ctlUpdatePanelHistory" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHistory"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc14:SCGLoading ID="SCGLoading5" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <uc8:History ID="ctlHistory" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
            
            </ContentTemplate>
            </asp:UpdatePanel>
            
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlCounterCasheirUpdate" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td width="16%">
                                <asp:Label ID="ctlCounterChier" SkinID="SkFieldCaptionLabel" runat="server" Text="$Counter Cashier$"></asp:Label>
                                <asp:Label ID="ctlCounterChierRequired" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ctlCounterCashierDropdown" runat="server" OnSelectedIndexChanged="ctlCounterCashierDropdown_SelectedIndexChanged" AutoPostBack="true"/>
                                <ss:LabelExtender ID="ctlCounterCashierDropdownExtender" runat="server" LinkControlID="ctlCounterCashierDropdown"
                                    InitialFlag='<%# this.InitialFlag %>' SkinID="SkGeneralLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.All %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="ctlFooterCommentLebel" SkinID="SkFieldCaptionLabel" runat="server"
                            Text="หมายเหตุ"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label4" SkinID="SkGeneralLabel" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="ctlFooterLabel1" SkinID="SkFieldCaptionLabel" runat="server" Text="เงินสกุล MYR (มาเลเซีย) ธนาคารจะรับคืนเฉพาะธนบัตรฉบับละ 50, 100 (ทุกสกุลเงินรับเฉพาะธนบัตรเท่านั้น)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label3" SkinID="SkGeneralLabel" runat="server" Text="**" ForeColor="Red"></asp:Label>
                        <asp:Label ID="ctlFooterLabel2" SkinID="SkFieldCaptionLabel" runat="server" Text="พนักงานต้องนำเงินที่เหลือคืนให้หน่วยงานการเงินภายใน 7 วันทำการ ภายหลังการเดินทางกลับ"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label2" SkinID="SkGeneralLabel" runat="server" Text="***" ForeColor="Red"></asp:Label>
                        <asp:Label ID="ctlFooterLabel3" SkinID="SkFieldCaptionLabel" runat="server" Text="กรณีคืนเงินเต็มจำนวนต้องจัดทำบันทึกขออนุมัติลงนามโดยผู้บังคับบัญชาของผู้อนุมัติยืมเงินทดรองจ่าย"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlUpdatePanelViewPost" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdatePanelViewPostProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewPost"
            DynamicLayout="true" EnableViewState="true">
            <ProgressTemplate>
                <uc14:SCGLoading ID="SCGLoading6" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div align="left">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="ctlDivViewDetailForeign" runat="server" style="display:inline;">
                            <fieldset id="ctlFieldSetVerifyDetailForeign" runat="server" style="width: 99%;">
                                <legend id="Legend1" style="color: #4E9DDF;">
                                    <asp:Label ID="ctlViewDetailForeign" runat="server" SkinID="SkGeneralLabel" Text="$Verify Detail$"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlBranchForeignText" runat="server" Text="Branch" SkinID="SkGeneralLabel"></asp:Label>
                                            <asp:Label ID="ctlBranchForeignTextRequired" runat="server" SkinID="SkRequiredLabel"></asp:Label>:
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="ctlBranchForeign" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBranchForeignExtender" runat="server" LinkControlID="ctlBranchForeign"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 15%">
                                        <asp:Label ID="ctlBusinessAreaForeignText" runat="server" Text="Business Area" SkinID="SkGeneralLabel"></asp:Label>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="ctlBusinessAreaForeign" runat="server" Width="85px" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            <ss:LabelExtender ID="ctlBusinessAreaForeignLabelExtender" runat="server" LinkControlID="ctlBusinessAreaForeign"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Button ID="ctlViewPostForeign" runat="server" Text="View Post" SkinID="SkGeneralButton"
                                                OnClick="ctlViewPostForeign_Click"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlPostingDateForeignText" runat="server" SkinID="SkGeneralLabel"
                                                Text="Posting Date"></asp:Label>
                                            <asp:Label ID="Label12" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td style="width: 15%">
                                            <uc1:Calendar ID="ctlPostingDateForeign" runat="server" />
                                            <ss:LabelExtender ID="ctlPostingDateForeignExtender" runat="server" LinkControlID="ctlPostingDateForeign"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlValueDateForeignText" runat="server" Text="Baseline Date" SkinID="SkGeneralLabel"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td style="width: 15%">
                                            <uc1:Calendar ID="ctlReceiveDateForeign" runat="server" />
                                            <ss:LabelExtender ID="ctlReceiveDateForeignExtender" runat="server" LinkControlID="ctlReceiveDateForeign"
                                                SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.RemittanceFieldGroup.VerifyDetail %>'></ss:LabelExtender>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="ctlPostingStatusForeignText" runat="server" Text="Posting Status"
                                                SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                                            <asp:Label ID="ctlPostingStatusForeign" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <uc13:ViewPost ID="ctlViewPost" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlUpdatePanelValidate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table border="0" width="400px">
            <tr>
                <font color="red" style="text-align: left" class="table">
                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                    </spring:ValidationSummary>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
