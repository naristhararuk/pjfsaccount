<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseDocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.ExpenseDocumentEditor"
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
<%@ Register Src="~/UserControls/DocumentEditor/Components/PaymentDetail.ascx" TagName="PaymentDetail"
    TagPrefix="uc7" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ExpenseGeneral.ascx" TagName="ExpenseGeneral"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ExpensesViewByAccount.ascx"
    TagName="ExpensesViewByAccount" TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ClearingAdvance.ascx"
    TagName="ClearingAdvance" TagPrefix="uc10" %>
<%@ Register Src="Components/InvoiceForm.ascx" TagName="InvoiceForm" TagPrefix="uc11" %>
<%@ Register Src="../ViewPost/ViewPost.ascx" TagName="ViewPost" TagPrefix="uc12" %>
<%@ Register Src="Components/History.ascx" TagName="History" TagPrefix="uc13" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc14" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ExpensesMPA.ascx" TagName="ExpensesMPA"
    TagPrefix="uc15" %>
<script type="text/javascript">
    function CheckboxesCheckFixedAdvance(objChk, gvClientID) {
        var ctrlParent = document.getElementById(gvClientID);
        if (objChk.checked) {
            var ctrlChild = "ctlFixedAdvanceSelect";
            var Inputs = ctrlParent.getElementsByTagName("input");

            for (var i = 0; i < Inputs.length; ++i) {
                if (Inputs[i].type == 'checkbox' && Inputs[i].id.indexOf(ctrlChild, 0) >= 0) {
                    if (objChk.id != Inputs[i].id) {
                        Inputs[i].checked = false;
                    } 
                    else 
                    {
                        Inputs[i].checked = true;
                    }
                }
            }
        }
    }
</script>
<table width="100%" cellpadding="0" class="table">
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
                    <table width="100%" border="0">
                        <tr>
                            <td align="left" colspan="2">
                                <%--<uc3:DocumentHeader ID="ctlFormHeader" HeaderForm='<%# GetProgramMessage("Domestic Expense Form") %>' runat="server" />--%>
                                <uc3:DocumentHeader ID="ctlFormHeader" runat="server" />
                                <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                                <asp:Label ID="ctlType" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Label ID="ctlCompanyLabel" runat="server" Text="$Company$" SkinID="SkDocumentHeader2Label" />
                                <asp:Label ID="ctlCompanyFieldReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                            </td>
                            <td align="left">
                                <uc6:CompanyField ID="ctlCompanyField" runat="server" />
                                <ss:LabelExtender ID="ctlCompanyFieldExtender" runat="server" LinkControlID="ctlCompanyField"
                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Company %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%">
                                <asp:Label ID="ctlDescriptionHeaderText" runat="server" Text="$Subject$" SkinID="SkDocumentHeader2Label"></asp:Label>
                                <asp:Label ID="ctlDescriptionHeaderReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlDescriptionHeader" runat="server" Width="300px" SkinID="SkGeneralTextBox"
                                    MaxLength="200"></asp:TextBox>
                                <ss:LabelExtender ID="ctlDescriptionHeaderLabelExtender" runat="server" LinkControlID="ctlDescriptionHeader"
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Subject %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="ctlCurrencyDropdownDiv" runat="server" style="display: none;">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="ctlCurrencyLabel" runat="server" Text="Currency" SkinID="SkDocumentHeader2Label"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ctlCurrencyDropdown" runat="server" Width="150px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ctlCurrencyDropdown_SelectedIndexChanged" />
                                                <ss:LabelExtender ID="ctlCurrencyLabelExtender1" runat="server" LinkControlID="ctlCurrencyDropdown"
                                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.LocalCurrency %>'>
                                                </ss:LabelExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlCreatorData" Legend='<%# GetProgramMessage("ctlCreatorData") %>'
                                                ShowSMS="false" ShowVendorCode="false" ShowFavoriteButton="false" ShowSearchUser="false" runat="server"
                                                Width="100%" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor %>' />
                                            <%--<ss:LabelExtender ID="ctlCreatorDataLabelExtender" runat="server" LinkControlID="ctlCreatorData"
                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                Width="100%" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor %>' />
                                            <%--<ss:LabelExtender ID="ctlRequesterDataLabelExtender" runat="server" LinkControlID="ctlRequesterData"
                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                            </ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 30%" valign="top">
                                            <uc2:ActorData ID="ctlReceiverData" Legend='<%# GetProgramMessage("ctlReceiverData") %>'
                                                ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server" Width="100%"
                                                ShowSMS="false" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor %>' />
                                            <%--<ss:LabelExtender ID="ctlReceiverDataLabelExtender" runat="server" LinkControlID="ctlReceiverData"
                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
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
            <asp:UpdatePanel ID="UpdateExpenseTab" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ajaxToolkit:TabContainer ID="X" runat="server" ActiveTabIndex="0">
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabGeneral" HeaderText="$Expense$" SkinID="SkTab">
                            <ContentTemplate>
                                <uc8:ExpenseGeneral ID="ctlExpenseGeneral" runat="server" Width="100%" />
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAdvanceRemittance" HeaderText="$Clearing Advance$"
                            SkinID="SkTab">
                            <ContentTemplate>
                                <uc10:ClearingAdvance ID="ctlClearingAdvance" Width="100%" runat="server" />
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <!---------------------------------------FixedAdvance------------------------------------------->
                        <ajaxToolkit:TabPanel runat="server" ID="ctlFixedAdvance" HeaderText="Fixed Advance"
                            SkinID="SkTab">
                            <ContentTemplate>
                                <%--<asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                        DynamicLayout="true" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc14:SCGLoading ID="SCGLoading200" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>--%>
                                <ss:BaseGridView ID="ctlFixedAdvanceGrid" EnableViewState="true" Width="100%" runat="server"
                                    OnDataBound="ctlFixedAdvanceGrid_DataBound" DataKeyNames="DocumentID" AllowPaging="true"
                                    AllowSorting="true" AutoGenerateColumns="false" OnRowDataBound="FixedAdvanceOutstandingGrid_RowDataBound"
                                    OnRowCommand="FixedAdvanceGrid_RowCommand" OnRequestData="RequestDataFixedAdvanceOutstanding"
                                    OnRequestCount="RequestCountFixedAdvanceOutstanding" CssClass="Grid" ReadOnly="true"
                                    HeaderStyle-CssClass="GridHeader">
                                    <AlternatingRowStyle CssClass="GridItem" />
                                    <RowStyle CssClass="GridAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText=" ">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ctlFixedAdvanceSelect" runat="server" SkinID="SkCtlGridSelect"
                                                    onclick="javascript:validateCheckBox(this);" />
                                                <ss:LabelExtender ID="ctlFixedAdvanceSelectExtender" runat="server" LinkControlID="ctlFixedAdvanceSelect"
                                                    InitialFlag='<%# this.InitialFlag %>' SkinID="SkMultiLineLabel" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fixed Advance No." HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="b.documentno">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="ctlGridDocumentId" runat="server" Value='<%#Eval("fixedAdvanceDocumentID") %>' />
                                                <asp:Literal ID="ctlGridDocumentNo" runat="server" Text='<%# Bind("fixedAdvanceNo") %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" SortExpression="b.subject">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlGridDescription" runat="server" Text='<%# Bind("subject") %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FixedAdvanceStatus" SortExpression="b.CacheCurrentStateName">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlAdvanceStatus" Mode="Encode" runat="server" Text='<%# Bind("fixedAdvanceStatus") %>'
                                                    SkinID="SkGeneralLabel" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date From" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="a.EffectiveFromDate">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlGridEffectiveFrom" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("effecttiveDateFrom")) %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date To" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="a.EffectiveToDate">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlGridEffectiveTo" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("effecttiveDateTo")) %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" SortExpression="a.NetAmount">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:#,##0.00}") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                </ss:BaseGridView>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <!---------------------------------------FixedAdvance----------------------------------------->
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabInitial" HeaderText="$Initial$" SkinID="SkTab">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelInitial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressInitial" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelInitial"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc14:SCGLoading ID="SCGLoading2" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc4:Initiator ID="ctlInitiator" runat="server" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Initiator %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAttachment" HeaderText="$Attachment$"
                            SkinID="SkTab">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelAttachment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressAttachment" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc14:SCGLoading ID="SCGLoading3" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc5:Attachment ID="ctlAttachment" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <uc15:ExpensesMPA ID="ctlExpensesMPA" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabViewByAccount" HeaderText="$View By Expense$"
                            SkinID="SkTab">
                            <ContentTemplate>
                                <uc9:ExpensesViewByAccount ID="ctlExpenseViewByAccount" runat="server" />
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabMemo" HeaderText="$Memo$" SkinID="SkTab">
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
                                                    <asp:TextBox ID="ctlMemo" runat="server" TextMode="MultiLine" Height="300px" Width="900px"
                                                        onkeypress="return IsMaxLength(this, 1000);" onkeyup="return IsMaxLength(this, 1000);"></asp:TextBox>
                                                    <ss:LabelExtender ID="ctlMemoExtender" runat="server" LinkControlID="ctlMemo" InitialFlag='<%# this.InitialFlag %>'
                                                        SkinID="SkMultiLineLabel" Width="800px" LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'></ss:LabelExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabHistory" HeaderText="$History$" SkinID="SkTab">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelHistory" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateHistoryProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHistory"
                                            DynamicLayout="true" EnableViewState="true">
                                            <ProgressTemplate>
                                                <uc14:SCGLoading ID="SCGLoading5" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc13:History ID="ctlHistory" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
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
            <asp:UpdatePanel ID="ctlUpdatePanelApprover" runat="server" UpdateMode="Conditional"
                ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:UpdateProgress ID="ctlUpdateApproverProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelApprover"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc14:SCGLoading ID="SCGLoading6" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table border="0" width="100%" class="table">
                        <tr>
                            <td align="left" style="width: 40%" valign="top">
                                <uc2:ActorData ID="ctlApproverData" Legend='<%# GetProgramMessage("ctlApproverData")%>'
                                    runat="server" ShowSMS="true" ShowVendorCode="false" ShowFavoriteButton="true" ShowSearchUser="true"
                                    Width="100%" IsApprover="true" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor %>' />
                                    <asp:Label ID="AleartMessageFixedAdvance" SkinID="SkGeneralLabel" runat="server" Text="%AleartMessageFixedAdvance%" ForeColor="Red" visible="false"></asp:Label>
                                <%--<ss:LabelExtender ID="ctlApproverDataLabelExtender" runat="server" LinkControlID="ctlApproverData"
                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.Other %>'>
                                </ss:LabelExtender>--%>
                            </td>
                            <td align="left" style="width: 50%" valign="top">
                                <uc7:PaymentDetail ID="ctlPaymentDetail" runat="server" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlUpdatePanelViewPost" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdatePanelViewPostProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelViewPost"
            DynamicLayout="true" EnableViewState="true">
            <ProgressTemplate>
                <uc14:SCGLoading ID="SCGLoading7" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table class="table" width="100%">
            <tr>
                <td align="left">
                    <div id="ctlDivVerifyDetail" runat="server" style="display:inline;">
                        <fieldset id="ctlFieldSetVerifyDetail" runat="server" style="width: 99%;">
                            <legend id="ctlLegendVerifyDetailView" style="color: #4E9DDF;">
                                <asp:Label ID="ctlVerifyDetailHeader" runat="server" Text="$Verify Detail$"></asp:Label>
                            </legend>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlBranchText" runat="server" Text="$Branch$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="Label6" runat="server" SkinID="SkCtlLabel" Text="*" ForeColor="Red"></asp:Label>:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="ctlBranch" runat="server" SkinID="SkGeneralTextBox" MaxLength="4"></asp:TextBox>
                                        <ss:LabelExtender ID="ctlBranchLabelExtender" runat="server" LinkControlID="ctlBranch"
                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlPaymentMethodText" runat="server" Text="$Payment Method$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ctlPaymentMethod" SkinID="SkCtlDropDown" Width="180px" runat="server"
                                            DataTextField="PaymentMethodName" DataValueField="PaymentMethodID">
                                        </asp:DropDownList>
                                        <ss:LabelExtender ID="ctlPaymentMethodLabelExtender" runat="server" LinkControlID="ctlPaymentMethod"
                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlSupplementaryDomesticText" runat="server" Text="$Supplementary$ :"
                                            SkinID="SkFieldCaptionLabel"></asp:Label>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="ctlSupplementary" runat="server" Width="85px" SkinID="SkGeneralTextBox"></asp:TextBox>
                                        <ss:LabelExtender ID="ctlSupplementaryLabelExtender" runat="server" LinkControlID="ctlSupplementary"
                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="ctlViewPostButton" runat="server" Text="$View Post$" OnClick="ctlViewPostButton_Click">
                                        </asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="ctlPostRemittanceButton" runat="server" Text="$Post Remittance$"
                                            OnClick="ctlPostRemittanceButton_Click"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="ctlPostingDate" runat="server" Text="$Posting Date$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="Label8" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <uc1:Calendar ID="ctlPostingDateCalendar" runat="server" />
                                        <ss:LabelExtender ID="ctlPostingDateCalendarLabelExtender" runat="server" LinkControlID="ctlPostingDateCalendar"
                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlBaselineDateText" runat="server" Text="$Baseline Date$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <uc1:Calendar ID="ctlBaselineDate" runat="server" />
                                        <ss:LabelExtender ID="ctlBaselineDateLabelExtender" runat="server" LinkControlID="ctlBaselineDate"
                                            InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlBusinessAreaDomesticText" runat="server" Text="$BusinessArea$ :"
                                            SkinID="SkFieldCaptionLabel"></asp:Label>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:TextBox ID="ctlBusinessArea" runat="server" Width="85px" SkinID="SkGeneralTextBox"></asp:TextBox>
                                        <ss:LabelExtender ID="ctlBusinessAreaLabelExtender" runat="server" LinkControlID="ctlBusinessArea"
                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlPostingStatusText" runat="server" Text="$Posting Status$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="ctlPostingStatus" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlRemittedPostingStatusText" runat="server" Text="$RemittancePosting Status$ :"
                                            SkinID="SkFieldCaptionLabel"></asp:Label>
                                        <asp:Label ID="ctlRemittedPostingStatus" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ctlVerifyImage" runat="server" Text="$Verify on Image$" SkinID="SkGeneralCheckBox" />
                                        <ss:LabelExtender ID="ctlVerifyImageLabelExtender" runat="server" LinkControlID="ctlVerifyImage"
                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.VerifyDetail %>'>
                                        </ss:LabelExtender>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="ctlDivReceiveDetail" runat="server" style="display:inline;">
                        <fieldset id="ctlReceiveFieldset" runat="server" style="width: 99%;">
                            <legend id="ctlReceiveLegend" style="color: #4E9DDF;">
                                <asp:Label ID="ctlReceiveDetailLabel" runat="server" Text="$Receive Detail$"></asp:Label>
                            </legend>
                            <table>
                                <tr>
                                    <td style="width: 20%">
                                        <asp:Label ID="ctlBoxIDLabel" runat="server" Text="$Box ID$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlBoxID" runat="server" SkinID="SkGeneralLabel" />
                                        <ss:LabelExtender ID="ctlBoxIDLabelExtender" runat="server" LinkControlID="ctlBoxID"
                                            SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BoxID %>'>
                                        </ss:LabelExtender>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="ctlDivExpRemittanceDetail" runat="server" style="display:inline;">
                        <fieldset id="ctlRemittanceFieldset" runat="server" style="width: 99%;">
                            <legend id="ctlRemittanceLegend" style="color: #4E9DDF;">
                                <asp:Label ID="ctlRemittanceDetailLabel" runat="server" Text="$PayInDetail$"></asp:Label>
                            </legend>
                            <table>
                                <tr>
                                    <td style="width: 45%">
                                        <asp:Label ID="ctlPayInMethodLabel" runat="server" Text="$PayInMethod$" SkinID="SkFieldCaptionLabel" />
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlPayInMethod" runat="server" SkinID="SkGeneralLabel" />
                                    </td>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="ctlKBankDetailPanel" runat="server" Visible="false">
                                                <table style="width: 99%">
                                                    <tr>
                                                        <td style="width: 45%">
                                                            <asp:Label ID="ctlGLAccountLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$GLAccount$"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="ctlGLAccount" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="ctlValueDateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$ValueDate$"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="ctlValueDate" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
        </table>
        <uc12:ViewPost ID="ctlViewPost" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<center>
    <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table">
                <tr>
                    <td align="left" style="color: Red;">
                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                        </spring:ValidationSummary>
                        <spring:ValidationSummary ID="ctlValidationSummary2" runat="server" Provider="InvoiceItem.Error">
                        </spring:ValidationSummary>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</center>
<asp:UpdatePanel ID="ctlPopUpWarningRequireAttachmentUpdatePanel" runat="server"
    UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
