<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TADocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.TADocumentEditor" EnableTheming="true" %>
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
<script language="javascript" type="text/javascript">
    function DisableCheckbox() {
        if (document.getElementById('<%= ctlOtherChk.ClientID %>').checked) {
            document.getElementById('<%= ctlOther.ClientID %>').disabled = false;
        }
        else {
            document.getElementById('<%= ctlOther.ClientID %>').disabled = true;
            document.getElementById('<%= ctlOther.ClientID %>').value = '';
        }
    }

    function DisableRadioButton() {
        if (document.getElementById('<%= ctlDomesticProvinceChk.ClientID %>').checked) {
            document.getElementById('<%= ctlProvince.ClientID %>').disabled = false;
            document.getElementById('<%= ctlCountry.ClientID %>').disabled = true;
            document.getElementById('<%= ctlCountry.ClientID %>').value = '';
        }
        else {
            document.getElementById('<%= ctlProvince.ClientID %>').disabled = true
            document.getElementById('<%= ctlCountry.ClientID %>').disabled = false;
            document.getElementById('<%= ctlProvince.ClientID %>').value = '';
        }
    }


</script>
<style type="text/css">
.hiddenColumn {display:none;}
</style>
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
                                <uc3:DocumentHeader ID="ctlTAFormHeader" HeaderForm='<%# GetProgramMessage("Travelling Authorization Form") %>'
                                    runat="server" labelNo="$TA No$" />
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
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.Company %>'>
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
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                </ss:LabelExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <table border="0" width="100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlCreatorData" Legend='<%# GetProgramMessage("ctlCreatorData") %>'
                                                ShowSMS="false" ShowVendorCode="false" ShowFavoriteButton="false" ShowSearchUser="false" runat="server"
                                                Width="100%" />
                                            <%--<ss:LabelExtender ID="ctlCreatorDataExtender" runat="server" LinkControlID="ctlCreatorData" SkinID="SkGeneralLabel"
                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                            </ss:LabelExtender>--%>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <uc2:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                                                ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                                                Width="100%" />
                                            <%--<ss:LabelExtender ID="ctlRequesterDataExtender" runat="server" LinkControlID="ctlRequesterData" SkinID="SkGeneralLabel"
                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
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
                                            <fieldset id="ctlTravellingFds" runat="server" style="width: 100%;">
                                                <legend id="ctlLegendDetailTravelling" style="color: #4E9DDF">
                                                    <asp:Label ID="ctlTravellingHeader" runat="server" Text="$Travelling$"></asp:Label></legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" style="width: 20%">
                                                            <asp:Label ID="ctlFromDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$From Date$"></asp:Label>
                                                            <asp:Label ID="ctlFromDateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" style="width: 13%">
                                                            <uc1:Calendar ID="ctlFromDateCal" runat="server" />
                                                            <ss:LabelExtender ID="ctlFromDateCalExtender" runat="server" LinkControlID="ctlFromDateCal"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 10%">
                                                            <asp:Label ID="ctlToDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$To Date$"></asp:Label>
                                                            <asp:Label ID="ctlToDateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <uc1:Calendar ID="ctlToDateCal" runat="server" />
                                                            <ss:LabelExtender ID="ctlToDateCalExtender" runat="server" LinkControlID="ctlToDateCal"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlPurposeLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Purpose$"></asp:Label>
                                                            <asp:Label ID="ctlPurposeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" colspan="3" style="width: 83%">
                                                            <asp:CheckBox ID="ctlBusinessChk" Text="$Business$" SkinID="SkGeneralCheckBox" runat="server" />
                                                            <ss:LabelExtender ID="ctlBusinessChkExtender" runat="server" LinkControlID="ctlBusinessChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:CheckBox ID="ctlTrainingChk" Text="$Training$" SkinID="SkGeneralCheckBox" runat="server" />
                                                            <ss:LabelExtender ID="ctlTrainingChkExtender" runat="server" LinkControlID="ctlTrainingChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:CheckBox ID="ctlOtherChk" Text="$Other$" SkinID="SkGeneralCheckBox" runat="server"
                                                                onclick="DisableCheckbox();" />&nbsp;
                                                            <ss:LabelExtender ID="ctlOtherChkExtender" runat="server" LinkControlID="ctlOtherChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:TextBox ID="ctlOther" SkinID="SkGeneralTextBox" runat="server" Width="300px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlOtherExtender" runat="server" LinkControlID="ctlOther" InitialFlag='<%# this.InitialFlag %>'
                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlTravelBy" runat="server" SkinID="SkFieldCaptionLabel" Text="$Travel by$"></asp:Label>
                                                            <asp:Label ID="ctlTravelByReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" style="width: 11%">
                                                            <asp:RadioButton ID="ctlDomesticProvinceChk" GroupName="Travelby" SkinID="SkGeneralRadioButton"
                                                                Text="$Domestic$" runat="server" OnCheckedChanged="ctlDomesticProvinceChk_OnCheckedChanged"
                                                                AutoPostBack="true" />
                                                            <ss:LabelExtender ID="ctlDomesticProvinceChkExtender" runat="server" LinkControlID="ctlDomesticProvinceChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 11%">
                                                            <asp:Label ID="ctlProvinceLabel" runat="server" Text="$Province$"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlProvince" SkinID="SkGeneralTextBox" runat="server" MaxLength="100"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlProvinceExtender" runat="server" LinkControlID="ctlProvince"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 11%">
                                                            <asp:RadioButton ID="ctlAbroadCountryChk" GroupName="Travelby" SkinID="SkGeneralRadioButton"
                                                                Text="$Abroad$" runat="server" OnCheckedChanged="ctlAbroadCountryChk_OnCheckedChanged"
                                                                AutoPostBack="true" />
                                                            <ss:LabelExtender ID="ctlAbroadCountryChkExtender" runat="server" LinkControlID="ctlAbroadCountryChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 11%">
                                                            <asp:Label ID="ctlCountryLabel" runat="server" Text="$Country$"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ctlCountry" SkinID="SkGeneralTextBox" runat="server" MaxLength="100"></asp:TextBox>
                                                            <ss:LabelExtender ID="ctlCountryExtender" runat="server" LinkControlID="ctlCountry"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:Label ID="ctlCountryNote" runat="server" Text="สำหรับประเทศจีน กรุณาระบุมณฑลด้วย" ForeColor="Red"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Label ID="ctlTicketingLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Ticketing$"></asp:Label>
                                                            <asp:Label ID="ctlTicktingReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" colspan="3" style="width: 83%">
                                                            <asp:RadioButton ID="ctlByTravellingChk" GroupName="Ticketing" SkinID="SkGeneralRadioButton"
                                                                Text="$By Travelling Service Section$" runat="server" AutoPostBack="true" 
                                                                oncheckedchanged="ctlByTravellingChk_CheckedChanged" />
                                                            <ss:LabelExtender ID="ctlByTravellingChkExtender" runat="server" LinkControlID="ctlByTravellingChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                            <asp:RadioButton ID="ctlByEmployeeChk" GroupName="Ticketing" SkinID="SkCtlCheckBox"
                                                                Text="$By Employee's Section$" runat="server" AutoPostBack="true" oncheckedchanged="ctlByTravellingChk_CheckedChanged"/>
                                                            <ss:LabelExtender ID="ctlByEmployeeChkExtender" runat="server" LinkControlID="ctlByEmployeeChk"
                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:Label ID="ctlTravellingInfoLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Traveller's Information$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <ss:BaseGridView ID="ctlTravellingInfoGrid" DataKeyNames="TravellerID" OnRowDataBound="ctlTravellingInfoGrid_RowDataBound"
                                                                OnDataBound="ctlTravellingInfoGrid_DataBound" OnRowCommand="ctlTravellingInfoGrid_RowCommand"
                                                                AutoGenerateColumns="false" runat="server" Width="100%" CssClass="grid" HeaderStyle-CssClass="GridHeader">
                                                                <AlternatingRowStyle CssClass="GridItem" />
                                                                <RowStyle CssClass="GridAltItem" />
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="User ID">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="ctlUserId" runat="server" Value='<%#Eval("UserID") %>' />
                                                                            <asp:Label ID="ctlUserName" runat="server" Text='<%# DisplayUserName(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Full Name (THA)" ControlStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ctlEmployeeName" runat="server" Text='<%# DisplayEmployeeName(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Full Name (ENG)" ControlStyle-Width="180">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ctlEmployeeNameEng" SkinID="SkGeneralTextBox" Text='<%#Eval("EmployeeNameEng") %>'
                                                                                runat="server" Width="98%" MaxLength="100" />
                                                                            <ss:LabelExtender ID="ctlEmployeeNameEngExtender" runat="server" LinkControlID="ctlEmployeeNameEng"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="$Airline Member (if any)" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="180">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ctlAirlineMember" SkinID="SkGeneralTextBox" Text='<%#Eval("AirlineMember") %>'
                                                                                runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlAirlineMemberExtender" runat="server" LinkControlID="ctlAirlineMember"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Costcenter" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <uc9:CostCenterField ID="ctlCostCenter" runat="server" />
                                                                            <ss:LabelExtender ID="ctlCostCenterFieldExtender" runat="server" LinkControlID="ctlCostCenter"
                                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="310px" HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Account Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <uc11:AccountField ID="ctlAccount" runat="server" />
                                                                            <ss:LabelExtender ID="ctlAccountFieldExtender" runat="server" LinkControlID="ctlAccount"
                                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="300px" HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="IO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <uc10:IOAutoCompleteLookup ID="ctlIO" runat="server" />
                                                                            <ss:LabelExtender ID="IOAutoCompleteLookupExtender" runat="server" LinkControlID="ctlIO"
                                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="160px" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ctlDelete" runat="server" Text="Delete" CommandName="DeleteTravellerInformation"
                                                                                SkinID="SkCtlGridDelete"></asp:ImageButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server" Text=' '></asp:Label>
                                                                </EmptyDataTemplate>
                                                            </ss:BaseGridView>
                                                        </td>
                                                    </tr>
                                                    <div id="ctlAddTraveller" runat="server">
                                                        <tr>
                                                            <td align="left" valign="middle" colspan="4">
                                                                <asp:ImageButton ID="ctlUserSave" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlUserProfileLookup_Click" />
                                                                <uc8:UserProfileLookup ID="ctlUserProfileLookup" runat="server" isMultiple="true" />
                                                            </td>
                                                        </tr>
                                                    </div>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:Label ID="ctlTravellingScheduleLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Travelling Schedule$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <ss:BaseGridView ID="ctlTravellingScheduleGrid" DataKeyNames="ScheduleID" OnDataBound="ctlTravellingScheduleGrid_DataBound"
                                                                OnRowDataBound="ctlTravellingScheduleGrid_RowDataBound" OnRowCommand="ctlTravellingSchedule_RowCommand"
                                                                ShowFooter='<%# isShowFooter %>' AutoGenerateColumns="false" runat="server" Width="100%"
                                                                CssClass="grid" ShowMsgDataNotFound="false" ShowFooterWhenEmpty="true" HeaderStyle-CssClass="GridHeader">
                                                                <AlternatingRowStyle CssClass="GridItem" />
                                                                <RowStyle CssClass="GridAltItem" />
                                                                <FooterStyle CssClass="GridItem" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Date">
                                                                        <%--<HeaderTemplate>
                                                                <asp:Label ID="ctlLblHeaderDate" runat="server" Text='<%# GetProgramMessage("ctlLblHeaderDate") %>'></asp:Label>
                                                                <asp:Label ID="ctlLblHeaderDateReq" runat="server"  SkinID="SkRequiredLabel"></asp:Label>
                                                            </HeaderTemplate>--%>
                                                                        <EditItemTemplate>
                                                                            <uc1:Calendar ID="ctlDateCal" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("Date")) %>'
                                                                                runat="server" />
                                                                            <ss:LabelExtender ID="ctlDateCalExtender" runat="server" LinkControlID="ctlDateCal"
                                                                                Width="98%" SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <uc1:Calendar ID="ctlDateCal" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("Date")) %>'
                                                                                runat="server" />
                                                                            <ss:LabelExtender ID="ctlDateCalExtender" runat="server" LinkControlID="ctlDateCal"
                                                                                Width="98%" SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Departure from (Location/Province/Country)$">
                                                                        <%--<HeaderTemplate>
                                                                <asp:Label ID="ctlLblHeaderDeparture" runat="server" Text='<%#GetProgramMessage("ctlLblHeaderDeparture")%>'></asp:Label>
                                                                <asp:Label ID="ctlLblHeaderDepartureReq" runat="server"  SkinID="SkRequiredLabel"></asp:Label>
                                                            </HeaderTemplate>--%>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="ctlDeparturefrom" SkinID="SkGeneralTextBox" Text='<%#Eval("DepartureFrom") %>'
                                                                                runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlDeparturefromExtender" runat="server" LinkControlID="ctlDeparturefrom"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="ctlDeparturefrom" SkinID="SkGeneralTextBox" Text='<%#Eval("DepartureFrom") %>'
                                                                                runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlDeparturefromExtender" runat="server" LinkControlID="ctlDeparturefrom"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrival at (Location/Province/Country)">
                                                                        <%--<HeaderTemplate>
                                                                <asp:Label ID="ctlLblHeaderArrival" runat="server" Text='<%#GetProgramMessage("ctlLblHeaderArrival")%>'></asp:Label>
                                                                <asp:Label ID="ctlLblHeaderArrivalReq" runat="server"  SkinID="SkRequiredLabel"></asp:Label>
                                                            </HeaderTemplate>--%>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="ctlArrival" Text='<%#Eval("ArrivalAt") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="98%" MaxLength="100" />
                                                                            <ss:LabelExtender ID="ctlArrivalExtender" runat="server" LinkControlID="ctlArrival"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="ctlArrival" Text='<%#Eval("ArrivalAt") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="98%" MaxLength="100" />
                                                                            <ss:LabelExtender ID="ctlArrivalExtender" runat="server" LinkControlID="ctlArrival"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="Travel By">
                                                                        <%--<HeaderTemplate>
                                                                <asp:Label ID="ctlLblHeaderTravel" runat="server" Text='<%#GetProgramMessage("ctlLblHeaderTravel")%>'></asp:Label>
                                                                <asp:Label ID="ctlLblHeaderTravelReq" runat="server"  SkinID="SkRequiredLabel"></asp:Label>
                                                            </HeaderTemplate>--%>
                                                                        <EditItemTemplate>
                                                                            <asp:HiddenField ID="ctlTravelBy" Value='<%#Eval("TravelBy") %>' runat="server" />
                                                                            <uc12:StatusDropdown ID="ctlStatusDropdown" runat="server">
                                                                            </uc12:StatusDropdown>
                                                                            <ss:LabelExtender ID="ctlStatusDropdownExtender" runat="server" LinkControlID="ctlStatusDropdown"
                                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <uc12:StatusDropdown ID="ctlStatusDropdown" runat="server">
                                                                            </uc12:StatusDropdown>
                                                                            <ss:LabelExtender ID="ctlStatusDropdownExtender" runat="server" LinkControlID="ctlStatusDropdown"
                                                                                InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Time" ItemStyle-HorizontalAlign="Center">
                                                                        <EditItemTemplate>
                                                                            <uc13:Time ID="ctlTime" runat="server" TimeValue='<%#Eval("Time") %>'>
                                                                            </uc13:Time>
                                                                            <ss:LabelExtender ID="ctlTimeExtender" runat="server" LinkControlID="ctlTime" InitialFlag='<%# this.InitialFlag %>'
                                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <uc13:Time ID="ctlTime" runat="server">
                                                                            </uc13:Time>
                                                                            <ss:LabelExtender ID="ctlTimeExtender" runat="server" LinkControlID="ctlTime" InitialFlag='<%# this.InitialFlag %>'
                                                                                LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Flight No. (If any)" ItemStyle-HorizontalAlign="Center">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="ctlFlightNo" Text='<%#Eval("FlightNo") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="96%" MaxLength="20"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlFlightNoExtender" runat="server" LinkControlID="ctlFlightNo"
                                                                                Width="96%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="ctlFlightNo" Text='<%#Eval("FlightNo") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="96%" MaxLength="20"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlFlightNoExtender" runat="server" LinkControlID="ctlFlightNo"
                                                                                Width="96%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="13%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Travelling Detail" ItemStyle-HorizontalAlign="Center">
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="ctlTravellingDetail" Text='<%#Eval("TravellingDetail") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="96%" MaxLength="200"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlTravellingDetailExtender" runat="server" LinkControlID="ctlTravellingDetail"
                                                                                Width="96%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="ctlTravellingDetail" Text='<%#Eval("TravellingDetail") %>' SkinID="SkGeneralTextBox"
                                                                                runat="server" Width="96%" MaxLength="200"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlTravellingDetailExtender" runat="server" LinkControlID="ctlTravellingDetail"
                                                                                Width="96%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </FooterTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <EditItemTemplate>
                                                                            <asp:ImageButton ID="ctlDelete" runat="server" Text="Delete" CommandName="DeleteTravellingSchedule"
                                                                                SkinID="SkCtlGridDelete"></asp:ImageButton>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:ImageButton ID="ctlAdd" runat="server" Text="Add" CommandName="AddTravellingSchedule"
                                                                                SkinID="SkOKButton"></asp:ImageButton>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </ss:BaseGridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                               <%-- <div id="ctlDivAddSchedule" runat="server">
                                                    <table id="ctlSchedule" runat="server" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" style="width: 15%">
                                                                <asp:Label ID="ctlCostCenter" runat="server" Text="$Cost Center$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                <asp:Label ID="ctlCostCenterReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                            </td>
                                                            <td colspan="4" align="left" valign="middle">
                                                                <uc9:CostCenterField ID="ctlCostCenterField" runat="server" />
                                                                <ss:LabelExtender ID="ctlCostCenterFieldExtender" runat="server" LinkControlID="ctlCostCenterField"
                                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                </ss:LabelExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 15%">
                                                                <asp:Label ID="ctlExpenseCode" runat="server" Text="$Expense Code$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                <asp:Label ID="ctlExpenseCodeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                                            </td>
                                                            <td colspan="4" align="left" valign="middle">
                                                                <uc11:AccountField ID="ctlAccountField" runat="server" />
                                                                <ss:LabelExtender ID="ctlAccountFieldExtender" runat="server" LinkControlID="ctlAccountField"
                                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                </ss:LabelExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 15%">
                                                                <asp:Label ID="ctlInternalCode" runat="server" Text="$Internal Order$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                                &nbsp;:&nbsp;
                                                            </td>
                                                            <td colspan="4" align="left" valign="middle">
                                                                <uc10:IOAutoCompleteLookup ID="ctlIOAutoCompleteLookup" runat="server" />
                                                                <ss:LabelExtender ID="IOAutoCompleteLookupExtender" runat="server" LinkControlID="ctlIOAutoCompleteLookup"
                                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                                </ss:LabelExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>
                                            </fieldset>
                                        </center>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabAdvance" HeaderText="Advance">
                            <HeaderTemplate>
                                <asp:Label ID="Label2" SkinID="SkFieldCaptionLabel" runat="server" Text="$Advance$"></asp:Label></HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="ctlUpdatePanelAdvance" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAdvance"
                                            DynamicLayout="true" EnableViewState="False">
                                            <ProgressTemplate>
                                                <uc15:SCGLoading ID="SCGLoading2" runat="server" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <center>
                                            <asp:UpdatePanel ID="ctlUpdatePanelAdvanceTab" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="ctlTraveller" runat="server" style="width: 100%;">
                                                        <legend id="Legend1" style="color: #4E9DDF">
                                                            <asp:Label ID="Label6" runat="server" Text="$Traveller$"></asp:Label>
                                                        </legend>
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td align="left" style="width: 10%">
                                                                    <asp:Label ID="ctlTravellerLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Traveller$"></asp:Label>&nbsp;:&nbsp;
                                                                </td>
                                                                <td align="left" style="width: 10%">
                                                                    <asp:DropDownList ID="ctlTravellerAdvanceDropdown" SkinID="SkGeneralDropdown" runat="server">
                                                                    </asp:DropDownList>
                                                                    <%--<ss:LabelExtender ID="ctlTravellerAdvanceDropdownExtender" runat="server" LinkControlID="ctlTravellerAdvanceDropdown"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                                                </ss:LabelExtender>--%>
                                                                </td>
                                                                <td align="left" style="width: 10%">
                                                                    <asp:Button ID="ctlCreateAdvance" runat="server" Text="Create Advance" SkinID="SkGeneralButton"
                                                                        OnClick="ctlCreateAdvance_Click" />
                                                                </td>
                                                                <td align="left" style="width: 5%">
                                                                    <asp:Button ID="ctlRefresh" runat="server" Text="Refresh" SkinID="SkGeneralButton"
                                                                        OnClick="ctlRefreshAdvance_Click" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="ctlMessage" runat="server" SkinID="SkGeneralLabel" ForeColor="Red"
                                                                        Text="$Text$"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="5">
                                                                    <ss:BaseGridView ID="ctlTravellerAdvanceGrid" DataKeyNames="TADocumentID,AdvanceID,DocumentID"
                                                                        OnRowCommand="ctlTravellerAdvanceGrid_RowCommand" OnRowDataBound="ctlTravellerAdvanceGrid_RowDataBound"
                                                                        OnDataBound="ctlTravellerAdvanceGrid_DataBound"
                                                                        AutoGenerateColumns="false" ShowMsgDataNotFound="false" runat="server" Width="100%"
                                                                        CssClass="grid" HeaderStyle-CssClass="GridHeader">
                                                                        <AlternatingRowStyle CssClass="GridItem" />
                                                                        <RowStyle CssClass="GridAltItem" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlNo" Mode="Encode" runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Advance No.">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="ctlAdvanceNoLink" runat="server" CommandName="LinkTravellerAdvance"
                                                                                        Text='<%#Eval("AdvanceNo") %>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Advance Status">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlAdvanceStatus" Mode="Encode" Text='<%#Eval("AdvanceStatus") %>'
                                                                                        runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlDescription" Mode="Encode" Text='<%#Eval("Description") %>' runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Requester">
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField ID="ctlRequesterID" runat="server" Value='<%#Eval("RequesterID") %>' />
                                                                                    <asp:Literal ID="ctlRequester" Mode="Encode" Text='<%#Eval("RequesterName") %>' runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Receiver">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlReceiver" Mode="Encode" Text='<%#Eval("ReceiverName") %>' runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Due Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlDueDate" Mode="Encode" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DueDate")) %>'
                                                                                        runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Amount (MainCurrency)">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlAmountMainCurrency" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "AdvanceMainCurrencyAmount", "{0:#,##0.00}") %>'
                                                                                        runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Amount (THB)">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ctlAmount" Mode="Encode" Text='<%# DataBinder.Eval(Container.DataItem, "AmountTHB", "{0:#,##0.00}") %>'
                                                                                        runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ctlDelete" runat="server" Text="Delete" SkinID="SkCtlGridDelete"
                                                                                        CommandName="DeleteTravellerAdvance"></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            <asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkNodataLabel" runat="server" Text='    '></asp:Literal>
                                                                        </EmptyDataTemplate>
                                                                    </ss:BaseGridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </center>
                                        <table border="0" width="100%" cellpadding="10" cellspacing="10">
                                            <tr>
                                                <td align="center">
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel runat="server" ID="ctlTabInitial" HeaderText="Initial">
                            <HeaderTemplate>
                                <asp:Label ID="Label1" SkinID="SkFieldCaptionLabel" runat="server" Text="$Initial$"></asp:Label></HeaderTemplate>
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
                                <asp:Label ID="Label3" SkinID="SkFieldCaptionLabel" runat="server" Text="$Attachment$"></asp:Label></HeaderTemplate>
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
                                <asp:Label ID="Label4" SkinID="SkFieldCaptionLabel" runat="server" Text="$Memo$"></asp:Label></HeaderTemplate>
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
                                                        SkinID="SkMultiLineLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
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
                                <asp:Label ID="Label5" SkinID="SkFieldCaptionLabel" runat="server" Text="$History$"></asp:Label></HeaderTemplate>
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
                        ShowSMS="true" IsApprover="true" ShowVendorCode="false" ShowFavoriteButton="true" ShowSearchUser="true"
                        runat="server" Width="100%" />
                    <%-- <ss:LabelExtender ID="ctlApproverDataExtender" runat="server" LinkControlID="ctlApproverData" SkinID="SkGeneralLabel"
                        InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.TAFieldGroup.All %>'>
                    </ss:LabelExtender>--%>
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
