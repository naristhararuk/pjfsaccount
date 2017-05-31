<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.UserProfileEditor" %>
<%@ Register Src="LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc5" %>
<%@ Register Src="LOV/SS.DB/SupervisorUserField.ascx" TagName="SupervisorUserField"
    TagPrefix="uc6" %>
<%@ Register Src="LOV/SCG.DB/LocationUserField.ascx" TagName="LocationUserField"
    TagPrefix="uc4" %>
<%@ Register Src="LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>

<%--<asp:Panel ID="ctlUserProfileEditor" runat="server" Style="display: none" CssClass="modalPopup">
    <asp:Panel ID="ctlUserProfileEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
        border: solid 1px Gray; color: Black">
        <asp:Label ID="lblCapture" runat="server" Text='$Header$'></asp:Label></p>
    </asp:Panel>--%>
<asp:Panel ID="ctlContent" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlUpdatePanelUserProfileForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelUserProfileProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelUserProfileForm"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:HiddenField ID="ctlChangeMode" runat="server" />
            <asp:HiddenField ID="ctlIdEdit" runat="server" />
            <asp:Panel ID="ctlPanelScroll" runat="server" HorizontalAlign="Center">
                <fieldset style="width: 90%" id="FieldsetSystemInfo" runat="server" enableviewstate="True">
                    <legend id="Legend1" runat="server" style="color: #4E9DDF" visible="True">
                        <asp:Label ID="ctlLblSystemInfo" runat="server" Text="$System Info$" SkinID="SkFieldCaptionLabel" />
                    </legend>
                    
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 300px;">
                                <asp:Label ID="FromEHR" Text="$From e-HR$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="ctlUserProfileFromERH" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text="No"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 300px;">
                                <asp:Label ID="ctlADUserLabel" Text="AD User" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlIsAdUser" runat="server" OnCheckedChanged="ctlIsAdUser_CheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfileIdLabel" Text="$User ID$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlUserIDRequired" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonUser" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileId" SkinID="SkGeneralTextBox" MaxLength="20" runat="server"
                                    Text='<%# Bind("UserName")%>' Width="200px" />
                            </td>
                        </tr>
                        <%--##############--%>
                        <%--INPUT PASSWORD--%>
                        <%--##############--%>
                        <div id="divInputPassword" runat="server">
                            <tr>
                                <td align="left" style="width: 30%;">
                                    <asp:Label ID="ctlUserPasswordText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Password$"></asp:Label>&nbsp;:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="ctlUserPassword" SkinID="SkCtlTextboxLeft" runat="server" TextMode="Password"
                                        Width="148px" MaxLength="50"></asp:TextBox>
                                    <asp:Label ID="ctlUserPasswordReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                    <ajaxToolkit:PasswordStrength ID="ctlPasswordStrength" runat="server" TargetControlID="ctlUserPassword"
                                        DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="8"
                                        PrefixText="Strength : " StrengthStyles="PasswordStrengthPolicyStrength1; PasswordStrengthPolicyStrength2; PasswordStrengthPolicyStrength3; PasswordStrengthPolicyStrength4; PasswordStrengthPolicyStrength5"
                                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" MinimumLowerCaseCharacters="1"
                                        MinimumUpperCaseCharacters="1" RequiresUpperAndLowerCaseCharacters="false" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 30%;">
                                    <asp:Label ID="ctlConfirmUserPasswordText" SkinID="SkFieldCaptionLabel" runat="server"
                                        Text="$Confirm Password$"></asp:Label>&nbsp;:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="ctlConfirmUserPassword" SkinID="SkCtlTextboxLeft" runat="server"
                                        TextMode="Password" Width="148px" MaxLength="50"></asp:TextBox>
                                    <asp:Label ID="ctlConfirmUserPasswordReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                </td>
                            </tr>
                        </div>
                        <%--##############--%>
                        <%--RESET PASSWORD--%>
                        <%--##############--%>
                        <tr>
                            <td align="left" style="width: 30%;">
                            </td>
                            <td align="left">
                                <asp:Button runat="server" ID="ctlResetPassword" Text="$ResetPassword$" OnClick="ctlResetPassword_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlLblLanguge" SkinID="SkFieldCaptionLabel" runat="server" Text="$Language$"></asp:Label>&nbsp;:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ctlCmbLanguage" SkinID="SkGeneralDropdown" Width="151px" runat="server" ></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlSetFailTimeText" SkinID="SkFieldCaptionLabel" runat="server" Text="$Set Fail Time$"></asp:Label>&nbsp;:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlSetFailTime" SkinID="SkCtlTextboxLeft" Width="148px" runat="server"
                                    onkeypress="return isKeyInt();" Style="text-align: right;" MaxLength="5" Text='<%# Bind("SetFailTime") %>'></asp:TextBox>
                                <asp:HiddenField ID="ctlOriginFailTime" runat="server" Value='<%# Bind("SetFailTime") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlChangePassword" SkinID="SkGeneralCheckBox" runat="server" Text="$Change Password$"
                                    Checked='<%# Eval("ChangePassword") %>' />&nbsp;&nbsp;
                                <asp:CheckBox ID="ctlLockUser" SkinID="SkGeneralCheckBox" runat="server" Text="$Lock User(Wrong password)$" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                
                <fieldset style="width: 90%" id="FieldsetGeneralInfo" runat="server" enableviewstate="True">
                    <legend id="LegendGeneralInfo" runat="server" style="color: #4E9DDF" visible="True">
                        <asp:Label ID="Label1" runat="server" Text="$General Info$" SkinID="SkFieldCaptionLabel" /></legend>
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePeopleIdLabel" Text="$People ID$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlPeopleIDRequired" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonPeopleID" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePeopleId" SkinID="SkGeneralTextBox" MaxLength="20"
                                    runat="server" Text='<%# Bind("PeopleID") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfileEmployeeCodeLabel" Text="$Employee Code$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlEmployeeCodeRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonEmployeeCode" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileEmployeeCode" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="20" Text='<%# Bind("EmployeeCode") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfileEmployeeNameLabel" Text="$Employee Name$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlEomployeeNameRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonEmployeeName" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileEmployeeName" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="100" Text='<%# Bind("EmployeeName") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlCompany" Text="$Company$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                <asp:Label ID="ctlCompanyRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonCompany" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <uc1:CompanyField ID="ctlCompanyField" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlCostCenterCode" Text="$Cost Center Code$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlCostCenterRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonCostCenter" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <uc5:CostCenterField ID="ctlCostCenterField" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlLocationLabel" Text="$Location$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                <asp:Label ID="ctlLocationRequire" SkinID="SkRequiredLabel" runat="server" Text="*"></asp:Label>
                                <asp:Label ID="ctlColonLocation" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <uc4:LocationUserField ID="ctlLocationField" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlSuppervisorLabel" Text="$Supervisor$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonSupervisor" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <uc6:SupervisorUserField ID="ctlSupervisor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlVendorCodeLabel" Text="$Vendor Code$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="Label3" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlVendorCode" runat="server" MaxLength="10" Width="200px"/>
                                <asp:Label SkinID="SkFieldCaptionLabel" runat="server" Text=" ตัวอย่าง 005XXXXXXX "></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfileSectionNameLabel" Text="$Section Name$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonSection" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileSectionName" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="150" Text='<%# Bind("SectionName") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePersonalLevelLabel" Text="$Personal Level$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonPersonalLevel" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePersonalLevel" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="10" Text='<%# Bind("PersonalLevel") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePersonalLevelDescriptionLabel" SkinID="SkFieldCaptionLabel"
                                    Text="$Personal Level Description$" runat="server"></asp:Label>
                                <asp:Label ID="ctlColonDescription" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePersonalLevelDescription" SkinID="SkGeneralTextBox"
                                    runat="server" MaxLength="100" Text='<%# Bind("PersonalDescription") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePersonalGroupLabel" Text="$Personal Group$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonGroup" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePersonalGroup" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="10" Text='<%# Bind("PersonalGroup") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePersonalGroupDescriptionLabel" SkinID="SkFieldCaptionLabel"
                                    Text="$Personal Group Description$" runat="server"></asp:Label>
                                <asp:Label ID="ctlColonPersonalGroupDescription" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePersonalGroupDescription" SkinID="SkGeneralTextBox"
                                    runat="server" MaxLength="100" Text='<%# Bind("PersonalLevelGroupDescription") %>'
                                    Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePositionNameLabel" Text="$Position Name$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonPosition" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePositionName" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="150" Text='<%# Bind("PositionName") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfilePhoneNoLabel" Text="$Phone No$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonPhoneNo" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfilePhoneNo" SkinID="SkGeneralTextBox" runat="server"
                                    MaxLength="20" Text='<%# Bind("PhoneNo") %>' Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlUserProfileEmailLabel" Text="$E-Mail$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonEmail" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileEmail" runat="server" MaxLength="50" Width="200" SkinID="SkGeneralTextBox"
                                    Text='<%# Bind("Email") %>' />
                                <asp:RegularExpressionValidator ID="ctlEmailRegex" runat="server" ControlToValidate="ctlUserProfileEmail"
                                    ValidationExpression="(([;]\s\w)*(([a-zA-Z0-9]+([-_.a-zA-Z0-9]+)*[-_a-zA-Z0-9]+)|[a-zA-Z0-9])@[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*\.[-_a-zA-Z0-9]+([.][-_a-zA-Z0-9]+)*)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlApprovalFlagLabel" Text="$Approval Flag$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonApproval" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlUserProfileApprovalFlag" SkinID="SkGeneralCheckBox" runat="server"
                                    Checked='<%# Eval("ApprovalFlag") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlActiveLabel" Text="$Active$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                <asp:Label ID="ctlColonActive" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlUserProfileActive" SkinID="SkGeneralCheckBox" runat="server"
                                    Checked='<%# Eval("Active") %>' />
                            </td>
                        </tr>
                         <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="Label2" Text="$EmailActive$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                                <asp:Label ID="Label4" SkinID="SkFieldCaptionLabel" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlEmailActive" SkinID="SkGeneralCheckBox" runat="server"
                                    Checked='<%# Eval("EmailActive") %>' />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset style="width: 90%" id="ctlFieldsetSmsOption" runat="server" enableviewstate="True">
                    <legend id="ctlLegendSmsOptions" runat="server" style="color: #4E9DDF" visible="True">
                        <asp:Label ID="ctlLabelSms" runat="server" Text="$Sms Options$" SkinID="SkFieldCaptionLabel" /></legend>
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 30%;">
                                <asp:Label ID="ctlMobilePhoneNoLabel" Text="$Mobile Phone No$" SkinID="SkFieldCaptionLabel"
                                    runat="server"></asp:Label>
                                <asp:Label ID="ctlColonMobilePhoneNo" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlUserProfileMobilePhoneNo" runat="server" 
                                    Text='<%# Bind("MobilePhoneNo") %>'
								    SkinID="SkGeneralTextBox" 
								    Width="200px"
								    onkeypress="return isKeyInt();"
								    MaxLength="10" >
						        </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            <asp:Label ID="ctlExMoblie" SkinID="SkFieldCaptionLabel" runat="server"
                                    Text="( Ex. 081975xxxx )"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlApproveRecject" SkinID="SkGeneralCheckBox" runat="server" Text="$Approve/Initial$"
                                    Checked='<%# Eval("SMSApproveOrReject") %>' TextAlign="Right" />
                                <asp:CheckBox ID="ctlToReceive" SkinID="SkGeneralCheckBox" runat="server" Text="$Payment Notification$"
                                    Checked='<%# Eval("SMSReadyToReceive") %>' TextAlign="Right" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td align="center" colspan="2">
                        <asp:ImageButton ID="ctlInsert" runat="server" CommandName="Insert" OnClick="ctlInsert_Click1"
                            SkinID="SkSaveButton" Text="Update" />
                        <asp:ImageButton ID="ctlCancel" runat="server" CommandName="Cancel" OnClick="ctlCancel_Click"
                            SkinID="SkCancelButton" Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <font color="red">
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="User.Error" />
                        </font>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%--</asp:Panel>--%>
<%--<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlUserProfileModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlUserProfileEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlUserProfileEditor" />--%>