<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CADocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.CADocumentEditor"
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
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc8" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField"
    TagPrefix="uc9" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/IOAutoCompleteLookup.ascx" TagName="IOAutoCompleteLookup"
    TagPrefix="uc10" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/AccountField.ascx" TagName="AccountField"
    TagPrefix="uc11" %>
<%@ Register Src="~/Usercontrols/DropdownList/SCG.DB/StatusDropdown.ascx" TagName="StatusDropdown"
    TagPrefix="uc12" %>
<%@ Register Src="~/Usercontrols/Time.ascx" TagName="Time" TagPrefix="uc13" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/History.ascx" TagName="History"
    TagPrefix="uc14" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc15" %>
<style>
    .setDateStyle table table
    {
        display: inline-table;
        vertical-align: middle;
    }
</style>
<script language="javascript" type="text/javascript">
    function DisableRadioButton() {
        if (document.getElementById('<%= ctlWorkInArea.ClientID %>').checked) {
            document.getElementById('<%= ctlWorkOutOfAreatxt.ClientID %>').disabled = true;
            document.getElementById('<%= ctlWorkOutOfAreatxt.ClientID %>').value = '';
        } else {
            document.getElementById('<%= ctlWorkOutOfAreatxt.ClientID %>').disabled = false;
            //            document.getElementById('<%= ctlStartDate.ClientID %>').value = '';
            //            document.getElementById('<%= ctlEndDate.ClientID %>').disabled = false;
            //            document.getElementById('<%= ctlEndDate.ClientID %>').value = '';  
        }

    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" class="table">
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="ctlUpdatePanelHeader" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="ctlUpdatePanelHeaderProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHeader"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc15:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" colspan="2">
                                <uc3:DocumentHeader ID="ctlCAFormHeader" HeaderForm='<%# GetProgramMessage("Car Authorization Form") %>'
                                    runat="server" labelNo="$No$" />
                                <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 12%">
                                <asp:Label ID="ctlCompany" runat="server" Text="$Company$" SkinID="SkDocumentHeader2Label"></asp:Label>
                                <asp:Label ID="ctlCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td>
                                <uc6:CompanyField ID="ctlCompanyField" runat="server" />
                                <ss:LabelExtender ID="ctlCompanyFieldExtender" runat="server" LinkControlID="ctlCompanyField"
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.Company %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 12%">
                                <asp:Label ID="ctlSubjectLabel" runat="server" Text="$Subject$" SkinID="SkDocumentHeader2Label"></asp:Label>
                                <asp:Label ID="ctlSubjectReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="ctlSubject" runat="server" Width="42%" SkinID="SkGeneralTextBox"
                                    MaxLength="200"></asp:TextBox>
                                <ss:LabelExtender ID="ctlSubjectExtender" runat="server" LinkControlID="ctlSubject"
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlCreatorData" Legend='<%# GetProgramMessage("ctlCreatorData") %>'
                                                ShowSMS="false" ShowVendorCode="false" ShowFavoriteButton="false" ShowSearchUser="false"
                                                runat="server" Width="100%" />
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true"
                                                runat="server" Width="100%" />
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
            <asp:UpdatePanel ID="ctlUpdatePanelTab" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ajaxToolkit:TabContainer ID="ctlTabContainer" runat="server" ActiveTabIndex="0">
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabGeneral" HeaderText="General">
                            <HeaderTemplate>
                                <asp:Label ID="ctlGeneralLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$General$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelGeneral" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressGeneral" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGeneral"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading1" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <center>
                                            <fieldset id="ctlEffectiveDateFds" runat="server" style="width: 100%;">
                                                <legend id="ctlLegendDetailEffectiveDate" style="color: #4E9DDF">
                                                    <asp:Label ID="ctlEffectiveDateHeader" runat="server" Text="$EffectiveDate$"></asp:Label></legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="ctlCarLicenseNoLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Car License No$"></asp:Label>&nbsp;:&nbsp;
                                                            <asp:TextBox ID="ctlCarLicenseNo" runat="server" MaxLength="200" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                            <ss:LabelExtender ID="CarLicenseNoExtender" runat="server" LinkControlID="ctlCarLicenseNo"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlBrandLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Brand$"></asp:Label>&nbsp;:&nbsp;
                                                            <asp:TextBox ID="ctlBrand" runat="server" MaxLength="200" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                            <ss:LabelExtender ID="BrandExtender" runat="server" LinkControlID="ctlBrand" SkinID="SkCalendarLabel"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlModelLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Model$"></asp:Label>&nbsp;:&nbsp;
                                                            <asp:TextBox ID="ctlModel" runat="server" MaxLength="200" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                            <ss:LabelExtender ID="ModelExtender" runat="server" LinkControlID="ctlModel" SkinID="SkCalendarLabel"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="ctlRegular" GroupName="Performance" SkinID="SkGeneralRadioButton"
                                                                Text="$Regular$" runat="server" AutoPostBack="true"
                                                                oncheckedchanged="ctlRegular_SelectedIndexChanged" />
                                                            <ss:LabelExtender ID="RegularExtender" runat="server" LinkControlID="ctlRegular"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td class="setDateStyle" align="left" colspan="3">
                                                        <asp:Panel id="tableRegular" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="ctlStartDateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Start Date$"></asp:Label>
                                                                        <asp:Label ID="ctlStartDateRequired" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                                        &nbsp;:&nbsp;
                                                                        <uc1:Calendar ID="ctlStartDate" runat="server" />
                                                                        <ss:LabelExtender ID="LabelExtender1" runat="server" InitialFlag="<%# this.InitialFlag %>"
                                                                            LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>" LinkControlID="ctlStartDate"
                                                                            SkinID="SkCalendarLabel">
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ctlEndDateLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$End Date$"></asp:Label>
                                                                        <asp:Label ID="ctlEndDateRequire" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                                        &nbsp;:&nbsp;
                                                                        <uc1:Calendar ID="ctlEndDate" runat="server" />
                                                                        <ss:LabelExtender ID="LabelExtender2" runat="server" InitialFlag="<%# this.InitialFlag %>"
                                                                            LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>" LinkControlID="ctlEndDate"
                                                                            SkinID="SkCalendarLabel">
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="ctlSomeTime" GroupName="Performance" SkinID="SkGeneralRadioButton"
                                                                Text="$Sometime$" runat="server" AutoPostBack="true"
                                                                oncheckedchanged="ctlSomeTime_SelectedIndexChanged" />
                                                            <ss:LabelExtender ID="SomeTimeExtender" runat="server" LinkControlID="ctlSomeTime"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        
                                                        <td align="left" class="setDateStyle" colspan="3">
                                                        <asp:Panel id="sometimeRegular" runat="server">
                                                            <table >
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="ctlStartDateSumtimeLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                                            Text="$Start Date$"></asp:Label>
                                                                        <asp:Label ID="ctlStartDateSumtimeRequire" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                                        &nbsp;:&nbsp;
                                                                        <uc1:Calendar ID="ctlStartDateSumtime" runat="server" />
                                                                        <ss:LabelExtender ID="LabelExtender3" runat="server" InitialFlag="<%# this.InitialFlag %>"
                                                                            LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>" LinkControlID="ctlStartDateSumtime"
                                                                            SkinID="SkCalendarLabel">
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ctlEndDateSumtimeLabel" runat="server" SkinID="SkFieldCaptionLabel"
                                                                            Text="$End Date$"></asp:Label>
                                                                        <asp:Label ID="ctlEndDateSumtimeRequire" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                                                        &nbsp;:&nbsp;
                                                                        <uc1:Calendar ID="ctlEndDateSumtime" runat="server" />
                                                                        <ss:LabelExtender ID="LabelExtender4" runat="server" InitialFlag="<%# this.InitialFlag %>"
                                                                            LinkControlGroupID="<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>" LinkControlID="ctlEndDateSumtime"
                                                                            SkinID="SkCalendarLabel">
                                                                        </ss:LabelExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>   
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="ctlWorkInArea" GroupName="WorkArea" SkinID="SkGeneralRadioButton"
                                                                Text="$WorkInArea$" runat="server" onclick="DisableRadioButton();" OnDataBinding="Page_Load" />
                                                                 <ss:LabelExtender ID="WorkInAreaExtender" runat="server" LinkControlID="ctlWorkInArea"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td colspan="2" align="left">
                                                            <asp:RadioButton ID="ctlWorkOutOfArea" GroupName="WorkArea" SkinID="SkGeneralRadioButton"
                                                                Text="$WorkOutOfArea$" runat="server" onclick="DisableRadioButton();" />
                                                                <ss:LabelExtender ID="WorkOutOfAreaExtender" runat="server" LinkControlID="ctlWorkOutOfArea"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:TextBox ID="ctlWorkOutOfAreatxt" runat="server" MaxLength="200" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                            <ss:LabelExtender ID="WorkOutOfAreatxtExtender" runat="server" LinkControlID="ctlWorkOutOfAreatxt"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="ctlCategoryLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Category$"></asp:Label>
                                                            <asp:DropDownList ID="ctlDropDownListCategory" runat="server" SkinID="SkGeneralDropdown"
                                                                OnSelectedIndexChanged="ctlDropDownListCategory_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <ss:LabelExtender ID="DropDownListCategoryExtender" runat="server" LinkControlID="ctlDropDownListCategory"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="ctlTypeLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$Type$"></asp:Label>
                                                            <asp:DropDownList ID="ctlDropDownListType" runat="server" SkinID="SkGeneralDropdown">
                                                            </asp:DropDownList>
                                                            <ss:LabelExtender ID="ctlDropDownListTypeExtender" runat="server" LinkControlID="ctlDropDownListType"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </center>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabInitial" HeaderText="Initial">
                            <HeaderTemplate>
                                <asp:Label ID="InitialLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Initial$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelInitial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressInitial" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelInitial"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading3" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc4:Initiator ID="ctlInitiator" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAttachment" HeaderText="Attachment">
                            <HeaderTemplate>
                                <asp:Label ID="AttachmentLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Attachment$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelAttachment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressAttachment" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading4" runat="server" />
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
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabMemo" HeaderText="Memo">
                            <HeaderTemplate>
                                <asp:Label ID="MemoLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Memo$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelMemo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressMemo" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMemo"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading5" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="ctlMemo" runat="server" TextMode="MultiLine" Height="300px" Width="800px"
                                                        onkeypress="return IsMaxLength(this, 1000);" onkeyup="return IsMaxLength(this, 1000);"
                                                        SkinID="SkGeneralTextBox" Wrap="true"></asp:TextBox>
                                                    <ss:LabelExtender ID="ctlMemoExtender" runat="server" LinkControlID="ctlMemo" Width="800px"
                                                        SkinID="SkMultiLineLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.CAFieldGroup.All %>'>
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
                                <asp:Label ID="HistoryLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$History$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelHistory" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="ctlUpdateProgressHistory" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHistory"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading6" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <uc14:History ID="ctlHistory" runat="server" />
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
                    <uc2:ActorData ID="ctlApproverData" Legend='<%# GetProgramMessage("ctlApproverData") %>'
                        ShowSMS="true" IsApprover="true" ShowVendorCode="false" ShowFavoriteButton="true"
                        ShowSearchUser="true" runat="server" Width="100%" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:UpdatePanel ID="ctlUpdatePanelValidationSummary" runat="server" UpdateMode="Conditional"
                ChildrenAsTriggers="true">
                <ContentTemplate>
                    <table border="0" width="400px">
                        <tr>
                            <td align="left">
                                <font color="red" style="text-align: left">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlPopUpWarningRequireAttachmentUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
