<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="Company.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Company"
    StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/DropdownList/SCG.DB/PaymentMethod.ascx" TagName="PaymentMethodDropdown"
    TagPrefix="uc1" %>

<%@ Register Src="~/UserControls/LocationEditor.ascx" TagName="LocationEditor" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 45%">
                <fieldset id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlCompanyCodeLabel" runat="server" Text="$CompanyCode$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlCompanyCodeCri" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="ctlCompanyNameLabel" runat="server" Text="$ctlCompanyNameLabel$"></asp:Label>
                                :
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlCompanyNameCri" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlSearch_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelCompanyGridview" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCompanyGridview"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading1" runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <ss:BaseGridView ID="ctlCompanyGrid" runat="server" AutoGenerateColumns="false" CssClass="table"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="CompanyID" EnableInsert="False"
                            ReadOnly="true" OnRowCommand="ctlCompanyGrid_RowCommand" OnRowDataBound="ctlCompanyGrid_RowDataBound"
                            OnRequestCount="RequestCount" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                            OnDataBound="ctlCompanyGrid_DataBound" OnPageIndexChanged="ctlCompanyGrid_PageIndexChanged"
                            Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Company Code" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="CompanyCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCompanyCodeLabel" runat="server" Text='<%# Bind("CompanyCode") %>' Mode="Encode"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="CompanyName">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCompanyNameLabel" runat="server" Text='<%# Bind("CompanyName") %>' Mode="Encode"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Type" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentType">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPaymentTypeLabel" runat="server" Text='<%# Bind("StatusDesc") %>' Mode="Encode"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentMethod">
                                    <HeaderTemplate>
                                        <table width="100%" class="table" border="1">
                                            <tr>
                                                <td colspan="3" style="text-align: center;">
                                                    <asp:Label ID="ctlPaymentMethodHeader" runat="server" Text='<%# GetProgramMessage("PaymentMethod") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Label ID="ctlPettyHeader" runat="server" Text='<%# GetProgramMessage("Petty") %>'></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Label ID="ctlTransferHeader" runat="server" Text='<%# GetProgramMessage("Transfer") %>'></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Label ID="ctlCheque" runat="server" Text='<%# GetProgramMessage("Cheque") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%" class="table">
                                            <tr>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Literal ID="ctlPettyLabel" runat="server" Text='<%# Bind("PettyName") %>' Mode="Encode"/>
                                                </td>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Literal ID="ctlTransferLabel" runat="server" Text='<%# Bind("TransferName") %>' Mode="Encode"/>
                                                </td>
                                                <td style="text-align: center; width: 33%">
                                                    <asp:Literal ID="ctlChequeLabel" runat="server" Text='<%# Bind("ChequeName") %>' Mode="Encode"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DefaultTaxCode" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="DefaultTaxCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDefaultTaxCodeLabel" runat="server" Text='<%# Bind("DefaultTaxCode") %>' Mode="Encode"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="ctlPaymentMethodLink" runat="server" Text="Payment Method" CommandName="PaymentMethodEdit" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="ctlLocationLink" runat="server" Text="Location" CommandName="LocationEdit" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="CompanyEdit" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ctlDeleteCompany" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Delete") %>' OnClientClick="return confirm('Are you sure delete this row');"
                                                        CommandName="CompanyDelete" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                        <div id="divButton" runat="server" style="vertical-align: middle;">
                            <table style="text-align: center;">
                                <tr>
                                    <td>
                                        <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="ctlCompanyFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlCompanyFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture" runat="server" SkinID="SkCtlLabel" Text="Manage Company Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ctlUpdatePanelCompanyForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelCompanyFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCompanyForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc3:SCGLoading ID="SCGLoading2" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlCompanyForm" runat="server" DataKeyNames="CompanyID" OnItemCommand="ctlCompanyForm_ItemCommand"
                                                OnItemInserting="ctlCompanyForm_ItemInserting" OnItemUpdating="ctlCompanyForm_ItemUpdating"
                                                OnModeChanging="ctlCompanyForm_ModeChanging" OnDataBound="ctlCompanyForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left" style="width: 40%">
                                                                <%# GetProgramMessage("CompanyCode")%>
                                                                <font color="red">
                                                                    <asp:Label ID="ctlCompanyCodeRequired" runat="server" Text="*"></asp:Label></font>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlCompanyCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="4"
                                                                    Text='<%# Bind("CompanyCode")%>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("CompanyName")%>
                                                                <font color="red">
                                                                    <asp:Label ID="ctlCompanyNameRequired" runat="server" Text="*"></asp:Label></font>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlCompanyName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                                                    Text='<%# Bind("CompanyName") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentType")%>
                                                                <font color="red">
                                                                    <asp:Label ID="ctlPaymentTypeRequired" runat="server" Text="*"></asp:Label></font>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlPaymentTypeDropdown" SkinID="SkCtlDropDownList" runat="server"
                                                                    Width="252px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodPetty")%>
                                                                <font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodPettyDropdown" runat="server" Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodTransfer")%><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodTransferDropdown" runat="server"
                                                                    Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodCheque")%><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodChequeDropdown" runat="server" Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Default Tax Code<font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlTaxCodeDropDown" runat="server" SkinID="SkGeneralDropdown"
                                                                    Width="100px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                               <%# GetProgramMessage("PerdiemRateProfile")%> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlPerdiemRateProfileDropdown" runat="server" SkinID="SkGeneralDropdown">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("AllowImportUserFromEHr")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlAllowImportUserFromEHrChk" runat="server" Checked='<%# Eval("AllowImportUserFromEHr") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ExpenseRequireAttachment")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlExpenseRequireAttachment" runat="server" Checked='<%# Eval("ExpenseRequireAttachment") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("BU") %><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropdownList ID="ctlBUDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                                                                </asp:DropdownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 40%">
                                                                <asp:Label ID="ctlBusinessAreaLebel" runat="server" Text='<%# GetProgramMessage("BusinessArea") %>'></asp:Label>
                                                                :
                                                            </td>
                                                            <td align="left" style="width: 60%">
                                                                <asp:TextBox ID="ctlBusinessArea" MaxLength="4" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Require Business Area")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlRequireBusinessArea" runat="server" Checked='<%# Eval("RequireBusinessArea") %>' />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                 <%# GetProgramMessage("IsVerifyHardCopyOnly")%>
                                                                 :
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="ctlIsVerifyHardCopyOnly" runat="server" Checked='<%# Eval("IsVerifyHardCopyOnly") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>Bank Detail</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left" style="width: 40%">
                                                                                <asp:Label ID="ctlBankNameLabel" runat="server" Text='<%# GetProgramMessage("BankName") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:TextBox ID="ctlBankName" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlBankBranchLabel" runat="server" Text='<%# GetProgramMessage("BankBranch") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlBankBranch" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlAccountNoLabel" runat="server" Text='<%# GetProgramMessage("AccountNo") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlAccountNo" MaxLength="15" SkinID="SkCtlTextboxCenter" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlAccountTypeLabel" runat="server" Text='<%# GetProgramMessage("AccountType") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlAccountType" MaxLength="50" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>ECC 6.0</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <%# GetProgramMessage("Use ECC 6.0")%>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:CheckBox ID="ctlUseEcc" runat="server" Checked='<%# Eval("UseECC") %>' OnCheckedChanged="ctlUseEcc_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                       
                                                                    </table>
                                                                       <asp:Panel ID="ctlSAPInstancePanel" runat="server" Visible="false">
                                                                    <table>
                                                                     <tr>
                                                                            <td align="left">
                                                                                <%# GetProgramMessage("SAP Instance") %><font color="red">*</font> :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ctlSAPInstanceDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>Use Special Pay-In Slip</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left" style="width: 40%">
                                                                                <%# GetProgramMessage("UseSpecialPayIn")%>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:CheckBox ID="ctlUseSpecialPayInChk" runat="server" OnCheckedChanged="ctlUseSpecialPayInChk_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Panel ID="ctlSpecialPayInPanel" runat="server" Visible="false">
                                                                        <table width="100%" border="0" class="table">
                                                                            <tr>
                                                                                <td align="left" style="width: 40%">
                                                                                    <asp:Label ID="ctlTaxIdLabel" runat="server" Text='<%# GetProgramMessage("TaxId") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td align="left" style="width: 60%">
                                                                                    <asp:TextBox ID="ctlTaxId" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="ctlKBankCodeLabel" runat="server" Text='<%# GetProgramMessage("KBankCode") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="ctlKBankCode" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="ctlGLAccountLabel" runat="server" Text='<%# GetProgramMessage("DefaultGLAccount") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="ctlDefaultGLAccount" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Update") %>' ValidationGroup="ValidateFormView"
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
                                                            <td align="left">
                                                                <%# GetProgramMessage("CompanyCode")%><font color="red"><asp:Label ID="ctlCompanyCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlCompanyCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="4"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("CompanyName")%><font color="red"><asp:Label ID="ctlCompanyNameRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlCompanyName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentType")%><font color="red"><asp:Label ID="ctlPaymentTypeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlPaymentTypeDropdown" SkinID="SkCtlDropDownList" runat="server"
                                                                    Width="252px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodPetty")%><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodPettyDropdown" runat="server" Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodTransfer")%><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodTransferDropdown" runat="server"
                                                                    Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PaymentMedthodCheque")%><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <uc1:PaymentMethodDropdown ID="ctlPaymentMedthodChequeDropdown" runat="server" Width="252px">
                                                                </uc1:PaymentMethodDropdown>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Default Tax Code<font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlTaxCodeDropDown" runat="server" SkinID="SkGeneralDropdown"
                                                                    Width="100px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("PerdiemRateProfile")%> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlPerdiemRateProfileDropdown" runat="server" SkinID="SkGeneralDropdown">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("AllowImportUserFromEHr")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlAllowImportUserFromEHrChk" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ExpenseRequireAttachment")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlExpenseRequireAttachment" runat="server" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("BU") %><font color="red">*</font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropdownList ID="ctlBUDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                                                                </asp:DropdownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 40%">
                                                                 <%# GetProgramMessage("Business Area")%>
                                                                :
                                                            </td>
                                                            <td align="left" style="width: 60%">
                                                                <asp:TextBox ID="ctlBusinessArea" MaxLength="4" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Require Business Area")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlRequireBusinessArea" runat="server"  />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left">
                                                                 <%# GetProgramMessage("IsVerifyHardCopyOnly")%>
                                                                 :
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="ctlIsVerifyHardCopyOnly" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>Bank Detail</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left" style="width: 40%">
                                                                                <asp:Label ID="ctlBankNameLabel" runat="server" Text='<%# GetProgramMessage("BankName") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:TextBox ID="ctlBankName" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlBankBranchLabel" runat="server" Text='<%# GetProgramMessage("BankBranch") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlBankBranch" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlAccountNoLabel" runat="server" Text='<%# GetProgramMessage("AccountNo") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlAccountNo" MaxLength="15" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="ctlAccountTypeLabel" runat="server" Text='<%# GetProgramMessage("AccountType") %>'></asp:Label>
                                                                                :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="ctlAccountType" MaxLength="50" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>ECC 6.0</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <%# GetProgramMessage("Use ECC 6.0")%>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:CheckBox ID="ctlUseEcc" runat="server" OnCheckedChanged="ctlUseEcc_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                       
                                                                    </table>
                                                                       <asp:Panel ID="ctlSAPInstancePanel" runat="server" Visible="false">
                                                                    <table>
                                                                     <tr>
                                                                            <td align="left">
                                                                                <%# GetProgramMessage("SAP Instance") %><font color="red">*</font> :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ctlSAPInstanceDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <fieldset class="table">
                                                                    <legend><b>Use Special Pay-In Slip</b></legend>
                                                                    <table width="100%" border="0" class="table">
                                                                        <tr>
                                                                            <td align="left" style="width: 40%">
                                                                                <%# GetProgramMessage("UseSpecialPayIn")%>
                                                                                :
                                                                            </td>
                                                                            <td align="left" style="width: 60%">
                                                                                <asp:CheckBox ID="ctlUseSpecialPayInChk" runat="server" OnCheckedChanged="ctlUseSpecialPayInChk_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Panel ID="ctlSpecialPayInPanel" runat="server" Visible="false">
                                                                        <table width="100%" border="0" class="table">
                                                                            <tr>
                                                                                <td align="left" style="width: 40%">
                                                                                    <asp:Label ID="ctlTaxIdLabel" runat="server" Text='<%# GetProgramMessage("TaxId") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td align="left" style="width: 60%">
                                                                                    <asp:TextBox ID="ctlTaxId" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="ctlKBankCodeLabel" runat="server" Text='<%# GetProgramMessage("KBankCode") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="ctlKBankCode" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="ctlGLAccountLabel" runat="server" Text='<%# GetProgramMessage("DefaultGLAccount") %>'></asp:Label>
                                                                                    :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="ctlDefaultGLAccount" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
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
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Company.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
                <ajaxtoolkit:modalpopupextender ID="ctlCompanyModalPopupExtender" runat="server"
                    TargetControlID="lnkDummy" PopupControlID="ctlCompanyFormPanel" BackgroundCssClass="modalBackground"
                    CancelControlID="lnkDummy" RepositionMode="None" 
                    PopupDragHandleControlID="ctlCompanyFormPanelHeader" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelPaymentMethodGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="ctlCompanyIDHidden" runat="server" />
                        <asp:UpdateProgress ID="ctlUpdateProgressPaymentMethodGrid" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelPaymentMethodGrid"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading3" runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <ss:BaseGridView ID="ctlPaymentMethodGrid" Width="60%" Visible="false" runat="server"
                            AutoGenerateColumns="false" CssClass="table" DataKeyNames="CompanyID,PaymentMethodID,CompanyPaymentMethodID"
                            ReadOnly="true" OnRowCommand="ctlPaymentMethodGrid_RowCommand" SelectedRowStyle-BackColor="#6699FF"
                            OnDataBound="ctlPaymentMethodGrid_DataBound" OnPageIndexChanged="ctlPaymentMethodGrid_PageIndexChanged">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PaymentMethod" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentMethodCode">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlPaymentMethod" runat="server" Text='<%# Bind("PaymentMethodCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="PaymentMethodName">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("PaymentMethodName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                        <div id="div1" runat="server" style="vertical-align: middle;">
                            <table style="text-align: center;">
                                <tr>
                                    <td>
                                        <div id="ctlPaymentMethodTools" runat="server" visible="false">
                                            <table border="0">
                                                <tr>
                                                    <td valign="middle">
                                                        <asp:DropDownList ID="ctlPaymentMethodDropdown" Visible="false" SkinID="SkCtlDropDownList"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:ImageButton runat="server" Visible="false" ID="ctlAddPaymentMethod" OnClick="ctlAddPaymentMethod_Click"
                                                            SkinID="SkCtlFormNewRow" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:ImageButton runat="server" Visible="false" ID="ctlUpdatePaymentMethod" OnClick="ctlUpdatePaymentMethod_Click"
                                                            SkinID="SkCtlFormSave" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:ImageButton runat="server" Visible="false" SkinID="SkCtlGridDelete" ID="ctlDeletePaymentMethod"
                                                            OnClick="ctlDeletePaymentMethod_Click" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:ImageButton runat="server" Visible="false" SkinID="SkCtlGridCancel" ID="ctlClosePaymentMethod"
                                                            OnClick="ctlClosePaymentMethod_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <font color="red">
                                                            <spring:ValidationSummary ID="ctlPaymentMethodValidation" runat="server" Provider="CompanyPaymentMethod.Error" />
                                                        </font>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <font color="red">
                                            <spring:ValidationSummary ID="ctlPaymentMethodValidationSummary" runat="server" Provider="PaymentMethod.Error" />
                                        </font>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelLocationGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left" style="width: 45%">
                                    <div id="ctlLocationCriteria" runat="server" visible="false">
                                        <fieldset id="ctlLocationFds" class="table">
                                            <table width="100%" border="0" class="table">
                                                <tr>
                                                    <td align="left" style="width: 40%">
                                                        <asp:Label ID="ctlLocationCodeLabel" runat="server" Text="$LocationCode$"></asp:Label>
                                                        :
                                                    </td>
                                                    <td align="left" style="width: 60%">
                                                        <asp:TextBox ID="ctlLocationCode" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="ctlDescription" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                                <td valign="top" align="left">
                                    <asp:ImageButton runat="server" ID="ctlSearchLocation" OnClick="ctlSearchLocation_Click"
                                        ToolTip="Search" SkinID="SkSearchButton" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="ctlCompanyIDHidden2" runat="server" />
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelLocationGrid"
                                        DynamicLayout="true" EnableViewState="False">
                                        <ProgressTemplate>
                                            <uc3:SCGLoading ID="SCGLoading4" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <%-- edit location--%>
                                    <ss:BaseGridView ID="ctlLocationGrid" Width="60%" Visible="false" runat="server"
                                        AutoGenerateColumns="false" CssClass="table" DataKeyNames="LocationID" ReadOnly="true"
                                        OnRowCommand="ctlLocationGrid_RowCommand" SelectedRowStyle-BackColor="#6699FF"
                                        OnDataBound="ctlLocationGrid_DataBound" OnPageIndexChanged="ctlLocationGrid_PageIndexChanged">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Center" SortExpression="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlLocation" runat="server" Text='<%# Bind("LocationCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="35%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="PaymentMethodName">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("LocationName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlActive" runat="server" Enabled="false" Checked='<%# Bind("Active")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="ctlEditLocation" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="LocationEdit" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ctlDeleteLocation" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="LocationDelete" OnClientClick="return confirm('Are you sure delete this row');" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                    <div id="ctlLocationTools" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton runat="server" ID="ctlAddLocation" OnClick="ctlAddLocation_Click"
                                                        SkinID="SkCtlFormNewRow" />
                                                </td>
                                                <td valign="middle">
                                                    <asp:ImageButton runat="server" SkinID="SkCtlGridCancel" ID="ctlCloseLocation" OnClick="ctlCloseLocation_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <uc2:LocationEditor ID="ctlLocationEditor" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="ctlLocationFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlLocationFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="Label1" runat="server" SkinID="SkFieldCaptionLabel" Text="Manage Location Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ctlUpdatePanelLocationForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="ctlLocationIDHidden" runat="server" />
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelLocationForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc3:SCGLoading ID="SCGLoading5" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlLocationForm" runat="server" DataKeyNames="LocationID" OnItemInserting="ctlLocationForm_ItemInserting"
                                                OnItemUpdating="ctlLocationForm_ItemUpdating" OnItemCommand="ctlLocationForm_ItemCommand"
                                                OnModeChanging="ctlLocationForm_ModeChanging" OnDataBound="ctlLocationForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("LocationCode")%><font color="red"><asp:Label ID="ctlLocationCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlLocationCode" Text='<%#Bind("LocationCode") %>' SkinID="SkCtlTextboxCenter"
                                                                    runat="server" MaxLength="4" Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" Checked='<%#Bind("Active") %>' runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <ss:BaseGridView ID="ctlLocationLangGrid" runat="server" AutoGenerateColumns="false"
                                                                    CssClass="table" DataKeyNames="LanguageID" EnableInsert="False" ReadOnly="true"
                                                                    SelectedRowStyle-BackColor="#6699FF">
                                                                    <HeaderStyle CssClass="GridHeader" />
                                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                                    <RowStyle CssClass="GridItem" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ctlLanguageNameLabel" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="ctlDescription" runat="server" Text='<%# Bind("LocationName") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="ctlComment" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </ss:BaseGridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Update") %>' ValidationGroup="ValidateFormView"
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
                                                            <td align="left">
                                                                <%# GetProgramMessage("LocationCode")%><font color="red"><asp:Label ID="ctlLocationCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlLocationCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="20"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <ss:BaseGridView ID="ctlLocationLangGrid" runat="server" AutoGenerateColumns="false"
                                                                    CssClass="table" DataKeyNames="LanguageID" EnableInsert="False" ReadOnly="true"
                                                                    SelectedRowStyle-BackColor="#6699FF">
                                                                    <HeaderStyle CssClass="GridHeader" />
                                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                                    <RowStyle CssClass="GridItem" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ctlLanguageNameLabel" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="ctlDescription" runat="server" Text='<%# Bind("LocationName") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="ctlComment" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </ss:BaseGridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
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
                                            <font color="red">
                                                <spring:ValidationSummary ID="ValidationSummary1" runat="server" Provider="Location.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
                <ajaxtoolkit:modalpopupextender ID="ctlLocationFormModalPopupExtender" runat="server"
                    TargetControlID="lnkDummy2" PopupControlID="ctlLocationFormPanel" BackgroundCssClass="modalBackground"
                    CancelControlID="lnkDummy2" RepositionMode="None" 
                    PopupDragHandleControlID="ctlLocationFormPanelHeader" />
            </td>
        </tr>
    </table>
</asp:Content>
