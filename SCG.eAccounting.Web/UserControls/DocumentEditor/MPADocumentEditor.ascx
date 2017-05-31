<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPADocumentEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.MPADocumentEditor"
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

<script language="javascript" type="text/javascript">
    function KeyInt(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
            if (unicode < 48 || unicode > 57) { //if not a number
                if (unicode == 46) {
                    return true
                }
                return false
            }  //disable key press   
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
                                <uc3:DocumentHeader ID="ctlMPAFormHeader" HeaderForm='<%# GetProgramMessage("Mobile Phone Authorization Form") %>'
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
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.Company %>'>
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
                                    SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
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
                                                        <td align="left" style="width: 20%">
                                                            <asp:Label ID="ctlStartDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$Start Date$"></asp:Label>
                                                            <asp:Label ID="ctlStartDateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left" style="width: 13%">
                                                            <uc1:Calendar ID="ctlStartDateCal" runat="server" />
                                                            <ss:LabelExtender ID="ctlStartDateCalExtender" runat="server" LinkControlID="ctlStartDateCal"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                        <td align="left" style="width: 10%">
                                                            <asp:Label ID="ctlEndDateLabel" SkinID="SkFieldCaptionLabel" runat="server" Text="$End Date$"></asp:Label>
                                                            <asp:Label ID="ctlEndDateReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <uc1:Calendar ID="ctlEndDateCal" runat="server" />
                                                            <ss:LabelExtender ID="ctlToDateCalExtender" runat="server" LinkControlID="ctlEndDateCal"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:Label ID="ctlRequesterInfoLabel" SkinID="SkFieldCaptionLabel" runat="server"
                                                                Text="$Information$"></asp:Label>&nbsp;:&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <ss:BaseGridView ID="ctlRequesterInfoGrid" DataKeyNames="MPAItemID" OnRowDataBound="ctlRequesterInfoGrid_RowDataBound"
                                                                OnDataBound="ctlRequesterInfoGrid_DataBound" OnRowCommand="ctlRequesterInfoGrid_RowCommand"
                                                                AutoGenerateColumns="false" runat="server" Width="100%" CssClass="grid" 
                                                                HeaderStyle-CssClass="GridHeader">
                                                                <AlternatingRowStyle CssClass="GridItem" />
                                                                <RowStyle CssClass="GridAltItem" />
                                                                <Columns>
                                                                 <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$Number$">
                                                                        <ItemTemplate>
                                                                          <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$Full Name (THA)$">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ctlEmployeeName" runat="server" Text='<%# DisplayEmployeeName(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$User ID$">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="ctlUserId" runat="server" Value='<%#Eval("UserID") %>' />
                                                                            <asp:Label ID="ctlUserName" runat="server" Text='<%# DisplayUserName(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$Degree$">
                                                                        <ItemTemplate>
                                                                             <asp:Label ID="ctlPersonalLevel" runat="server" Text='<%# DisplayPersonalLevel(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$Department/Part/Group$">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="ctlSectionName" runat="server" Text='<%# DisplaySectionName(Container.DataItem) %>'
                                                                                Width="96%"></asp:Label>
                                                                        </ItemTemplate>
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$ActualAmount$" ControlStyle-Width="100">
                                                                        <ItemTemplate>
                                                                          <asp:TextBox ID="ctlActualAmount" SkinID="SkGeneralTextBox" Text='<%#Eval("ActualAmount") %>'
                                                                                runat="server" Width="90%" MaxLength="20" onkeypress="return KeyInt(event)" style="text-align:right"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlActualAmountExtender" runat="server" LinkControlID="ctlActualAmount"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$ActualAmountNotExceed$" ControlStyle-Width="120">
                                                                        <ItemTemplate>
                                                                               <asp:TextBox ID="ctlActualAmountNotExceed" SkinID="SkGeneralTextBox"  Text='<%#Eval("ActualAmountNotExceed") %>'
                                                                                runat="server" Width="90%" MaxLength="20" onkeypress="return KeyInt(event)" style="text-align:right"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlActualAmountNotExceedExtender" runat="server" LinkControlID="ctlActualAmountNotExceed"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate> 
                                                                        <ItemStyle Width="250px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$AmountCompanyPackage$" ControlStyle-Width="120">
                                                                        <ItemTemplate>
                                                                          <asp:TextBox ID="ctlAmountCompanyPackage" SkinID="SkGeneralTextBox" Text='<%#Eval("AmountCompanyPackage") %>'
                                                                                runat="server" Width="90%" MaxLength="20" onkeypress="return KeyInt(event)" style="text-align:right"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlAmountCompanyPackageExtender" runat="server" LinkControlID="ctlAmountCompanyPackage"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="250px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$TotalAmount$" ControlStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ctlTotalAmount" SkinID="SkGeneralTextBox" Text='<%#Eval("TotalAmount") %>'
                                                                                runat="server" Width="90%" MaxLength="20" onkeypress="return KeyInt(event)" style="text-align:right"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlTotalAmountExtender" runat="server" LinkControlID="ctlTotalAmount"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$MobilePhoneNo$" ControlStyle-Width="100">
                                                                        <ItemTemplate>
                                                                          <asp:TextBox ID="ctlMobilePhoneNo" SkinID="SkGeneralTextBox" Text='<%#Eval("MobilePhoneNo") %>'
                                                                                runat="server" Width="90%" MaxLength="20"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlMobilePhoneNoExtender" runat="server" LinkControlID="ctlMobilePhoneNo"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$MobileBrand$" ControlStyle-Width="100">
                                                                        <ItemTemplate>
                                                                          <asp:TextBox ID="ctlMobileBrand" SkinID="SkGeneralTextBox" Text='<%#Eval("MobileBrand") %>'
                                                                                runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlMobileBrandExtender" runat="server" LinkControlID="ctlMobileBrand"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="$MobileModel$" ControlStyle-Width="100">
                                                                        <ItemTemplate>
                                                                          <asp:TextBox ID="ctlMobileModel" SkinID="SkGeneralTextBox" Text='<%#Eval("MobileModel") %>'
                                                                                runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                                                                                <ss:LabelExtender ID="ctlMobileModelExtender" runat="server" LinkControlID="ctlMobileModel"
                                                                SkinID="SkCalendarLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                                        
                                                                   </asp:TemplateField>
                                                                    <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                        HeaderText="Full Name (ENG)">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ctlEmployeeNameEng" SkinID="SkGeneralTextBox" Text='<%#Eval("EmployeeNameEng") %>'
                                                                                runat="server" Width="98%" MaxLength="100" />
                                                                            <ss:LabelExtender ID="ctlEmployeeNameEngExtender" runat="server" LinkControlID="ctlEmployeeNameEng"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    <%--<asp:TemplateField HeaderText="$Airline Member (if any)" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ctlAirlineMember" SkinID="SkGeneralTextBox" Text='<%#Eval("AirlineMember") %>'
                                                                                runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                                            <ss:LabelExtender ID="ctlAirlineMemberExtender" runat="server" LinkControlID="ctlAirlineMember"
                                                                                Width="98%" SkinID="SkGeneralLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
                                                                            </ss:LabelExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="$Action$" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                                    <div id="ctlAddRequester" runat="server">
                                                        <tr>
                                                            <td align="left" valign="middle" colspan="4">
                                                                <asp:ImageButton ID="ctlUserSave" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlUserProfileLookup_Click" />
                                                                <uc8:UserProfileLookup ID="ctlUserProfileLookup" runat="server" isMultiple="true" />
                                                            </td>
                                                        </tr>
                                                    </div>
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
                                                        SkinID="SkMultiLineLabel" InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# SCG.eAccounting.BLL.Implement.MPAFieldGroup.All %>'>
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
