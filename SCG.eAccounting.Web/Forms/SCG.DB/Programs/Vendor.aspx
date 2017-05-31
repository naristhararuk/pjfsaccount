<%@ Page 
    Language="C#" 
    MasterPageFile="~/ProgramsPages.Master" 
    AutoEventWireup="true" 
    CodeBehind="Vendor.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Vendor" 
    Title="Untitled Page" 
    EnableTheming = "true"
    StylesheetTheme="Default"
    meta:resourcekey="PageResource1"
%>

<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<%-- ------------ --%>
<%-- Search       --%>
<%-- ------------ --%>
<asp:UpdatePanel ID="updPanelSearch" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelSearch" DynamicLayout="true" EnableViewState="False">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
        
    <div id="divCondition" align="left" style="width:100%">
        <fieldset id="ctlFieldSetDetailGridView"  style="width:400px" id ="fdsSearch" class="table">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table border="0" class="table" width="400px">
                        <tr>
	                        <td align="left" style="width:20%">
	                            <asp:Label ID="ctlLblVendorCode" runat="server" Text='$Vendor Code$'></asp:Label> : 
	                        </td>
	                        <td align="left" style="width:30%">
		                        <asp:TextBox ID="ctlTxtVendorCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"  Width="100px"></asp:TextBox>
	                        </td>
                        </tr>
                        <tr>
	                        <td align="left" style="width:20%">
	                            <asp:Label ID="ctlLblVendorName" runat="server" Text='$Vendor Name$'></asp:Label> : 
	                        </td>
	                        <td align="left" style="width:30%">
		                        <asp:TextBox ID="ctlTxtVendorName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
	                        </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td align="left" valign="top">
                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click"/>
                </td>
            </tr>
        </table>
        
        </fieldset>
    </div>
    
    </ContentTemplate>
    
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<%-- ------------ --%>
<%-- Main Program --%>
<%-- ------------ --%>
<asp:UpdatePanel ID="updPanelGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
  
    <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
    
        <asp:UpdateProgress ID="updProgressGridView" runat="server" AssociatedUpdatePanelID="updPanelGridView" DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <ss:BaseGridView ID="ctlGridVendor" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="true"
                DataKeyNames        = "VendorID" 
                OnRequestCount      = "RequestCount" 
                OnRequestData       = "RequestData"
                OnRowCommand        = "ctlGridVendor_RowCommand" 
                CssClass="table"
                SelectedRowStyle-BackColor="#6699FF" 
                HeaderStyle-CssClass="GridHeader"
                ShowHeaderWhenEmpty ="true"
                >

                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />

                <Columns>
                    
                    <asp:TemplateField HeaderText="Vendor Code" SortExpression="VendorCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblVendorCode" runat="server" Text='<%# Bind("VendorCode")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Vendor Title" SortExpression="VendorTitle" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblVendorTitle" runat="server" Text='<%# Bind("VendorTitle")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Vendor Name1" SortExpression="VendorName1" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblVendorName1" runat="server" Text='<%# Bind("VendorName1")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Street" SortExpression="Street" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblStreet" runat="server" Text='<%# Bind("Street")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Tax No. 1" SortExpression="TaxNo1" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblTaxNo1" runat="server" Text='<%# Bind("TaxNo1")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Tax No. 2" SortExpression="TaxNo2" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblTaxNo2" runat="server" Text='<%# Bind("TaxNo2")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax No. 3" SortExpression="TaxNo3" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblTaxNo3" runat="server" Text='<%# Bind("TaxNo3")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax No. 4" SortExpression="TaxNo4" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblTaxNo4" runat="server" Text='<%# Bind("TaxNo4")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="False" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False" CommandName="VendorEdit" ToolTip='<%# GetProgramMessage("EditVendor") %>' />
                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False" CommandName="VendorDelete" ToolTip='<%# GetProgramMessage("DeleteVendor") %>' OnClientClick="return confirm('Are you sure delete this row');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                    </asp:TemplateField>
                </Columns>
                
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" ></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
				
            </ss:BaseGridView>
            
        <div id="divButton" runat="server" align="left">
            <asp:ImageButton ID="ctlBtnAddVendor" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlBtnAddVendor_Click"/>
        </div>        
            
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ctlBtnAddVendor" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<%-- ------------ --%>
<%-- Grid View    --%>
<%-- ------------ --%>
<asp:Panel ID="panelFormView" runat="server" Style="display: none" CssClass="modalPopup" Width="500px">

    <asp:Panel ID="ctlPanelFormViewHeader" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p>
                <asp:Label ID="ctlLblFormViewHeader" runat="server" SkinID="SkFieldCaptionLabel" Text="$Add / Edit With Vendor$"></asp:Label>
            </p>
        </div>
    </asp:Panel>
            
    <asp:UpdatePanel ID="updPanelFormView" runat="server" UpdateMode="Conditional">

        <ContentTemplate> 
        
        <div align="center" style="display: block;">
            
            <asp:UpdateProgress ID="updProgressFormView" runat="server" AssociatedUpdatePanelID="updPanelFormView" DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
                        
            <table border="0" cellpadding="0" cellspacing="0" class="TableInFormView">
                <tr>
                    <td align="center">
                        <asp:FormView ID="ctlFormViewVendor" runat="server" 
                            DataKeyNames="VendorID" 
                            OnDataBound     = "ctlFormViewVendor_DataBound"
                            OnItemCommand   = "ctlFormViewVendor_ItemCommand" 
                            OnItemInserting = "ctlFormViewVendor_ItemInserting"
                            OnItemUpdating  = "ctlFormViewVendor_ItemUpdating" 
                            OnModeChanging   = "ctlFormViewVendor_ModeChanging">
                            
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorCode" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorCode") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorCode" runat="server" Text='<%# Bind("VendorCode") %>' MaxLength="20" />
                                            <asp:Label ID="lblVendorCodeReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorTitle" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorTitle") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorTitle" runat="server" Text='<%# Bind("VendorTitle") %>' MaxLength="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorName1" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorName1") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorName1" runat="server" Text='<%# Bind("VendorName1") %>' MaxLength="100" />
                                            <asp:Label ID="Label4" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorName2" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorName2") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorName2" runat="server" Text='<%# Bind("VendorName2") %>' MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblStreet" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Street") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtStreet" runat="server" Text='<%# Bind("Street") %>' MaxLength="100" />
                                            <asp:Label ID="Label2" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblCity" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("City") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtCity" runat="server" Text='<%# Bind("City") %>' MaxLength="100" />
                                            <asp:Label ID="Label3" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblDistrict" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("District") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtDistrict" runat="server" Text='<%# Bind("District") %>' MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblCountry" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Country") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtCountry" runat="server" Text='<%# Bind("Country") %>' MaxLength="100" />
                                            <asp:Label ID="Label5" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblPostalCode" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("PostalCode") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtPostalCode" runat="server" Text='<%# Bind("PostalCode") %>' MaxLength="20" />
                                            <asp:Label ID="Label6" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo1" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo1") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo1" runat="server" Text='<%# Bind("TaxNo1") %>' MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo2" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo2") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo2" runat="server" Text='<%# Bind("TaxNo2") %>' MaxLength="20" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo3" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo3") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo3" runat="server" Text='<%# Bind("TaxNo3") %>' MaxLength="13" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo4" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo4") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo4" runat="server" Text='<%# Bind("TaxNo4") %>' MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblBlock" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Block") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkBlockDelete" runat="server"  SkinID="SkGeneralCheckBox" Checked='<%# Bind("BlockDelete") %>' Text='<%# GetProgramMessage("BlockDelete") %>' />
                                            <asp:CheckBox ID="chkBlockPost" runat="server"  SkinID="SkGeneralCheckBox" Checked='<%# Bind("BlockPost") %>' Text='<%# GetProgramMessage("BlockPost") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblChkActive" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("UpdateVendor") %>' ></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelVendor") %>' ></asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                                        
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorCode" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorCode") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorCode" runat="server" MaxLength="20" />
                                            <asp:Label ID="lblVendorCodeReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorTitle" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorTitle") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorTitle" runat="server" MaxLength="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorName1" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorName1") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorName1" runat="server" MaxLength="100" />
                                            <asp:Label ID="Label4" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblVendorName2" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("VendorName2") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtVendorName2" runat="server" MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblStreet" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Street") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtStreet" runat="server" MaxLength="100" />
                                            <asp:Label ID="Label8" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblCity" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("City") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtCity" runat="server" MaxLength="100" />
                                            <asp:Label ID="Label9" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblDistrict" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("District") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtDistrict" runat="server" MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblCountry" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Country") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtCountry" runat="server" MaxLength="100" />
                                            <asp:Label ID="Label10" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblPostalCode" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("PostalCode") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtPostalCode" runat="server" MaxLength="20" />
                                            <asp:Label ID="Label11" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo1" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo1") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo1" runat="server" MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo2" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo2") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo2" runat="server" MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo3" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo3") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo3" runat="server" MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblTaxNo4" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("TaxNo4") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlTxtTaxNo4" runat="server" MaxLength="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ctlLblBlock" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Block") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkBlockDelete" runat="server" Checked="false" SkinID="SkGeneralCheckBox" Text='<%# GetProgramMessage("BlockDelete") %>' />
                                            <asp:CheckBox ID="chkBlockPost" runat="server" Checked="false" SkinID="SkGeneralCheckBox" Text='<%# GetProgramMessage("BlockPost") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblChkActive" runat="server" SkinID="SkFieldCaptionLabel" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                CommandName="Insert" Text="Update" ToolTip='<%# GetProgramMessage("InsertVendor") %>' ></asp:ImageButton>
                                            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("CancelVendor") %>' ></asp:ImageButton>
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
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Vendor.Error"  />
                        </font>
                    </td>
                </tr>
            </table>
                        
        </div>
        </ContentTemplate>
    
    </asp:UpdatePanel>

</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupVendor" runat="server" TargetControlID="lnkDummy"
        PopupControlID="panelFormView" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlLblFormViewHeader" />

</asp:Content>
