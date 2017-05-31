<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" EnableTheming="true"
    StylesheetTheme="Default" AutoEventWireup="true" CodeBehind="Tax.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Tax"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/CompanyTaxInfo.ascx" TagName="CompanyTaxInfo" TagPrefix="uc2" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:sCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <table width="100%" class="table">
                <tr align="left" valign="top">
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <fieldset>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="left" style="width: 20%">
                                                    <asp:Label ID="ctlTaxCodeText" runat="server" Text="$Tax Code$"></asp:Label>&nbsp:&nbsp
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTaxCode" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%">
                                                    <asp:Label ID="ctlDescriptionText" runat="server" Text="$Description$"></asp:Label>&nbsp:&nbsp
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlDescription" runat="server" SkinID="SkGeneralTextBox" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td valign="top">
                                    <asp:ImageButton ID="ctlSearch" runat="server" SkinID="SkSearchButton" ToolTip="Search"
                                        OnClick="ctlSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                                        DynamicLayout="true" EnableViewState="False">
                                        <ProgressTemplate>
                                            <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ss:BaseGridView ID="ctlTaxGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                        AllowSorting="true" AllowPaging="true" DataKeyNames="TaxID" EnableInsert="False"
                                        ReadOnly="true" OnRowCommand="ctlTaxGrid_RowCommand" OnRowDataBound="ctlTaxGrid_RowDataBound"
                                        OnRequestCount="RequestCount" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                                        Width="100%">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="TaxCode" HeaderStyle-HorizontalAlign="Center" SortExpression="TaxCode">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ctlTaxCode" runat="server" Text='<%# Bind("TaxCode") %>' Mode="Encode"></asp:Literal></ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TaxName" HeaderStyle-HorizontalAlign="Center" SortExpression="TaxName">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ctlTaxName" runat="server" Text='<%# Bind("TaxName") %>' Mode="Encode"></asp:Literal></ItemTemplate>
                                                <ItemStyle Width="35%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GL" HeaderStyle-HorizontalAlign="Center" SortExpression="GL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ctlGl" runat="server" Text='<%# Bind("GL") %>' Mode="Encode"/>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate" HeaderStyle-HorizontalAlign="Center" SortExpression="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rate", "{0:#,##0.0000}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate Non-Deduct" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="RateNonDeduct">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlRateNonDeduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RateNonDeduct", "{0:#,##0.0000}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server"
                                                        Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ApplyAllCompany" HeaderStyle-HorizontalAlign="Center" SortExpression="ApplyAllCompany">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlApplyAllCompany" Checked='<%# Bind("ApplyAllCompany") %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton Text="Company" ID="LinkButton1" runat="server" CommandName="Company" />
                                                    <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="UserEdit" />
                                                    <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="UserDelete" OnClientClick="return confirm('Are you sure delete this row');" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                                runat="server"></asp:Label></EmptyDataTemplate>
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                    </ss:BaseGridView>
                                    <div id="divButton" runat="server" style="vertical-align: middle;">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                                                </td>
                                                <td>
                                                    <span class="spanSeparator" style="vertical-align: top;"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <uc2:CompanyTaxInfo style="width: 100%" ID="ctlCompanyTaxInfo1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <asp:Panel ID="ctlTaxFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlTaxFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture" runat="server" SkinID="SkFieldCaptionLabel" Text="Manage Program Data"></asp:Label></p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelTaxForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelTaxFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelTaxForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlTaxForm" runat="server" DataKeyNames="TaxID" OnItemCommand="ctlTaxForm_ItemCommand"
                                                OnItemInserting="ctlTaxForm_ItemInserting" OnItemUpdating="ctlTaxForm_ItemUpdating"
                                                OnModeChanging="ctlTaxForm_ModeChanging" OnDataBound="ctlTaxForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditTaxCodeText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxCode")%>'></asp:Label><asp:Label
                                                                    ID="ctlTaxCodeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlEditTaxCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="20"
                                                                    Text='<%# Bind("TaxCode") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="ctlEditTaxNameText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxName")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlEditTaxName" runat="server" SkinID="SkGeneralTextBox" MaxLength="100"
                                                                    Text='<%# Bind("TaxName") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditGLText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("GL")%>'></asp:Label><asp:Label
                                                                    ID="ctlGLReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlEditGL" runat="server" SkinID="SkNumberTextBox" MaxLength="10"
                                                                    Text='<%# Bind("GL") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditRateText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Rate")%>'></asp:Label><asp:Label
                                                                    ID="ctlRateReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlEditRate" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"
                                                                    Text='<%# Bind("Rate") %>' Width="250px" />
                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                    TargetControlID="ctlEditRate" ValidChars="." FilterType="Numbers,Custom" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditRateNonDeductText" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("Rate Non-Deduct")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlEditRateNonDeduct" runat="server" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" SkinID="SkNumberTextBox"
                                                                    Text='<%# Bind("RateNonDeduct") %>' Width="250px" />
                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                    TargetControlID="ctlEditRateNonDeduct" ValidChars="." FilterType="Numbers,Custom" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditApplyAllCompanyText" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("ApplyAllCompany")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlApplyAllCompany" runat="server" Checked='<%# Eval("ApplyAllCompany") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlEditActiveText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Active")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Save") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Update" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlTaxCodeText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxCode")%>'></asp:Label><asp:Label
                                                                    ID="ctlNewTaxCodeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlTaxCode" SkinID="SkCtlTextboxCenter" MaxLength="20" runat="server"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="ctlTaxNameText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxName")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlTaxName" runat="server" SkinID="SkCtlTextboxLeft" MaxLength="100"
                                                                    Text='<%# Bind("TaxName") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlGLText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("GL")%>'></asp:Label><asp:Label
                                                                    ID="ctlGLReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlGL" runat="server" SkinID="SkCtlTextboxLeft" MaxLength="10" Text='<%# Bind("GL") %>'
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlRateText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Rate")%>'></asp:Label><asp:Label
                                                                    ID="ctlRateReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlRate" runat="server" SkinID="SkNumberTextBox"  OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" Text='<%# Bind("Rate") %>'
                                                                    Width="250px" />
                                                                <%--<ajaxToolkit:FilteredTextBoxExtender
	                                                            ID="FilteredTextBoxExtender3"
	                                                            runat="server"
	                                                            TargetControlID="ctlRate"
	                                                            ValidChars ="."
	                                                            FilterType="Numbers,Custom" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="ctlNewRateNonDeductText" runat="server" SkinID="SkFieldCaptionLabel"
                                                                    Text='<%# GetProgramMessage("Rate Non-Deduct")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlRateNonDeduct" runat="server" SkinID="SkNumberTextBox" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6, 4));"
                                                                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"
                                                                    Text='<%# Bind("RateNonDeduct") %>' Width="250px" />
                                                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                    TargetControlID="ctlRateNonDeduct" ValidChars="." FilterType="Numbers,Custom" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlApplyAllCompanyText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("ApplyAllCompany")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlApplyAllCompany" runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ctlNewActiveText" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Active")%>'></asp:Label>&nbsp:&nbsp
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Insert"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </InsertItemTemplate>
                                            </asp:FormView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:ValidationSummary ID="vsSummary" runat="server" Style="text-align: left" Width="250px"
                                            ValidationGroup="ValidateFormView" />--%>
                                            <font color="red">
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Tax.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlTaxModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlTaxFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlTaxFormPanelHeader" />
</asp:Content>
