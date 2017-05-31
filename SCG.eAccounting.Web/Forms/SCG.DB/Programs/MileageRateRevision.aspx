<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" EnableTheming="true"
    StylesheetTheme="Default" AutoEventWireup="true" CodeBehind="MileageRateRevision.aspx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.MileageRateRevision" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/LOV/SCG.eAccounting/MileageRateRevisionLookUp.ascx"
    TagName="MileageRateRevisionLookUp" TagPrefix="uc5" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:sCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" class="table">
                <tr>
                    <td colspan="2">
                        <ss:BaseGridView ID="ctlMiRvsGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="Id,EffectiveFromDate,EffectiveToDate,ApprovedDate,StatusDesc"
                            EnableInsert="False" ReadOnly="true" OnRowCommand="ctlMileageRateRevisionGrid_RowCommand"
                            OnRequestCount="RequestCount" OnRequestData="RequestData" OnRowDataBound="ctlMileageRateRevisionGrid_RowDataBound"
                            SelectedRowStyle-BackColor="#6699FF" Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Approve Date" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlMiRvsApprovedDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("ApprovedDate")) %>'
                                            Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective From" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlMiRvsEffectiveFromDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveFromDate").ToString()) %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective To" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlMiRvsEffectiveToDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveToDate").ToString()) %>'
                                            Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlMiRvsStatusDesc" runat="server" Text='<%# Bind("StatusDesc") %>'
                                            Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="BtnView" runat="server" SkinID="SkCtlView" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("View") %>' CommandName="ViewItem" />
                                        <asp:ImageButton ID="BtnApproveDate" runat="server" SkinID="SkCtlApprove" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Approve") %>' CommandName="ApproveItem" />
                                        <asp:ImageButton ID="BtnImport" runat="server" SkinID="SkCtlImport" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Import") %>' CommandName="ImportItem" />
                                        <asp:ImageButton ID="BtnRemoveDate" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="RemoveItem" OnClientClick="return confirm('Are you sure delete this row');" />
                                        <asp:ImageButton ID="BtnCancel" runat="server" SkinID="SkCtlGridCancel" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="CancelItem" />
                                        <asp:ImageButton ID="BtnEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="EditMiRvsItem" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                    runat="server"></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton runat="server" ID="ctlAddItem" SkinID="SkCtlFormNewRow" OnClick="ctlAddItem_Click" />
                    </td>
                </tr>
            </table>
            <div style="text-align: left;" id="DivManageMileageRate" runat="server">
                <fieldset id="ctlFieldSetDetailGridView" style="width: 750px; padding: 5px; border-color: #000;"
                    class="table">
                    <table width="100%" class="table">
                        <tr valign="top">
                            <td align="left" style="width: 120px; vertical-align: middle">
                                <asp:Label ID="EffectFromLabel" runat="server">Effective From : </asp:Label>
                            </td>
                            <td align="left" style="width: 120px">
                                <uc4:Calendar ID="ctlEffectiveFrom" runat="server" />
                            </td>
                            <td align="left" style="width: 120px; vertical-align: middle">
                                <asp:Label ID="EffectToLabel" runat="server">Effective To : </asp:Label>
                            </td>
                            <td align="left" style="width: 120px">
                                <uc4:Calendar ID="ctlctlEffectiveTo" runat="server" />
                            </td>
                            <td align="left">
                                <asp:ImageButton runat="server" ID="AddButton" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                            </td>
                            <td align="left">
                                <asp:ImageButton runat="server" ID="UpdateMrrButton" SkinID="SkSaveButton" OnClick="UpdateMrrButton_Click" />
                            </td>
                            <td align="left">
                                <asp:ImageButton runat="server" ID="CancelMrrButton" SkinID="SkCancelButton" OnClick="CancelMrrButton_Click" />
                            </td>
                            <td align="left">
                                <font color="red">
                                    <spring:ValidationSummary ID="ctlValidationMiRvs" runat="server" Provider="MileageRateRivision.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <table width="100%" class="table">
                <tr valign="top">
                    <td colspan="2">
                        <ss:BaseGridView ID="ctlMiRvsDetail" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="Id,MileageProfileId,ProfileName,PersonalLevelGroupCode,CarRate,CarRate2,PickUpRate,PickUpRate2,MotocycleRate,MotocycleRate2"
                            EnableInsert="False" ReadOnly="true" OnRowCommand="ctlMiRvsItemGrid_RowCommand"
                            OnRequestCount="RequestCountMiRvsItem" OnRequestData="RequestDataMiRvsItem" OnRowDataBound="ctlMileageRateRevisionItemGrid_RowDataBound"
                            SelectedRowStyle-BackColor="#6699FF" Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Profile" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlProfile" runat="server" Text='<%# Bind("ProfileName") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Position Level" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPositionLevel" runat="server" Text='<%# Bind("PersonalLevelGroupCode") %>'
                                            Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Car" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <table width="100%" class="table" style="border-width: 0px;">
                                            <tr>
                                                <td colspan="3" style="text-align: center; border-width: 0px;">
                                                    <asp:Label ID="ctlCarHeader" runat="server" Text="Car"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlSubHeader" runat="server" Text="0-100"></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlSubHeader2" runat="server" Text=">100"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%" class="table">
                                            <tr style="border: 0">
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlCarRate" runat="server" Text='<%# Bind("CarRate") %>' Mode="Encode" />
                                                </td>
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlCarRate2" runat="server" Text='<%# Bind("CarRate2") %>' Mode="Encode" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PickUpRate" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <table width="100%" class="table">
                                            <tr>
                                                <td colspan="3" style="text-align: center; border-width: 0px;">
                                                    <asp:Label ID="ctlPickupHeaader" runat="server" Text="Pick Up"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlPickupSubHeader" runat="server" Text="0-100"></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlPickupSubHeader2" runat="server" Text=">100"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%" class="table">
                                            <tr>
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlPickUpRate" runat="server" Text='<%# Bind("PickUpRate") %>' Mode="Encode" />
                                                </td>
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlPickUpRate2" runat="server" Text='<%# Bind("PickUpRate2") %>'
                                                        Mode="Encode" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MotocycleRate" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <table width="100%" class="table">
                                            <tr>
                                                <td colspan="3" style="text-align: center; border-width: 0px;">
                                                    <asp:Label ID="ctlMotocycleRateHeader" runat="server" Text="MotocycleRate"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlMotocycleRateSubHeader" runat="server" Text="0-30"></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 33%; border-width: 0px;">
                                                    <asp:Label ID="ctlMotocycleRateSubHeader2" runat="server" Text=">30"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%" class="table">
                                            <tr>
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlMotocycleRate" runat="server" Text='<%# Bind("MotocycleRate") %>'
                                                        Mode="Encode" />
                                                </td>
                                                <td style="text-align: right; width: 33%; border-width: 0px;">
                                                    <asp:Literal ID="ctlMotocycleRate2" runat="server" Text='<%# Bind("MotocycleRate2") %>'
                                                        Mode="Encode" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="BtnEditMiRvsItem" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="EditMiRvsItem" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                    runat="server"></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>
                    </td>
                </tr>
            </table>
            <div style="text-align: left;" id="DivMileageIltem" runat="server">
                <fieldset id="Fieldset1" style="width: 750px; padding: 5px; border-color: #000;"
                    class="table">
                    <table width="100%" class="table">
                        <tr>
                            <td>
                                <asp:HiddenField ID="MiRvsItemId" runat="server" />
                                <asp:HiddenField ID="MiRvsId" runat="server" />
                                <asp:HiddenField ID="MiRvsProfileId" runat="server" />
                                <asp:HiddenField ID="MiRvsPositionLevel" runat="server" />
                                <asp:HiddenField ID="MiRvsStatus" runat="server" />
                            </td>
                            <td align="left" width="20%">
                                <table width="100%" class="table">
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Label ID="CarRate" runat="server" Text="Car"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelCarRate" runat="server" Text="0-100"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelCarRate2" runat="server" Text=">100"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputCarRate" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputCarRate2" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" width="20%">
                                <table width="100%" class="table">
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Label ID="PickUpRate" runat="server" Text="PickUp"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelPickUpRate" runat="server" Text="0-100"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelPickUpRate2" runat="server" Text=">100"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputPickUpRate" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputPickUpRate2" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" width="20%">
                                <table width="100%" class="table">
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Label ID="MotocycleRate" runat="server" Text="Motocycle"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelMotocycleRate" runat="server" Text="0-100"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="LabelMotocycleRate2" runat="server" Text=">100"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputMotocycleRate" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:TextBox ID="InputMotocycleRate2" runat="server" SkinID="SkGeneralTextBox" Width="50%"
                                                OnKeyPress="return(currencyFormat(this, ',', '.', event, 8));"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" width="40%">
                                <asp:ImageButton runat="server" ID="UpdateButton" SkinID="SkSaveButton" OnClick="ClickSave_MiRvsItem" />
                                <asp:ImageButton runat="server" ID="CancelButton" SkinID="SkCancelButton" OnClick="ClickCancel_MiRvsItem" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc5:MileageRateRevisionLookUp ID="MileageRateRevisionLookUp" runat="server" isMultiple="true" />
</asp:Content>
