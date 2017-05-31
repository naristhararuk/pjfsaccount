<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpVat.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ExpVat" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:Panel ID="pnAccountSearch" runat="server" Width="1000px" BackColor="White">
<asp:Panel ID="pnAccountSearchHeader" CssClass="table" runat="server" Style="cursor: move;border:solid 1px Gray;color:Black">
		<div>
			<p><asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
		</div>
</asp:Panel>
<asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
<table width="65%">
<tr>
<td style="width:20%"><asp:Label ID="ctlCostCenterLabel" runat="server" Text="$Cost Center$"></asp:Label><asp:Label ID="Label4" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td style="width:40%"><asp:TextBox ID="ctlCostCenter" runat="server" ></asp:TextBox>
<asp:ImageButton ID="ctlCostCenterImg" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlAccountCodeLabel" runat="server" Text="$Account Code$"></asp:Label><asp:Label ID="Label5" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlAccountCode" runat="server" ></asp:TextBox>
<asp:ImageButton ID="ImageButton1" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td><asp:TextBox ID="ctlAccountDetail" runat="server" Enabled="false" Width="98%"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="ctlInternalOrderLabel" runat="server" Text="$Internal Order$"></asp:Label><asp:Label ID="ctlCompanyReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlInternalOrder" runat="server" ></asp:TextBox>
<asp:ImageButton ID="ImageButton2" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label><asp:Label ID="Label1" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td colspan="2"><asp:TextBox ID="ctlDescription" runat="server"  Width="98%"></asp:TextBox></td>
</tr>
<tr>
<td colspan="3" >
<table><tr>
<td><asp:Label ID="ctlServiceFromDateLabel" runat="server" Text="$Service From Date$"></asp:Label>&nbsp;:</td>
<td><uc1:Calendar ID="ctlServiceFromDateCal" runat="server" /></td>
<td><asp:Label ID="ctlServiceToDateLabel" runat="server" Text="$Service To Date$"></asp:Label>&nbsp;:</td>
<td><uc1:Calendar ID="ctlServiceToDateCal" runat="server" /></td>
</tr></table>
</td>
</tr>
<tr>
<td><asp:Label ID="ctlAmountLabel" runat="server" Text="$Amount$"></asp:Label><asp:Label ID="Label2" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlAmount" runat="server" ></asp:TextBox></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlReferanceNoLabel" runat="server" Text="$Referance No.$"></asp:Label><asp:Label ID="Label6" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlReferanceNo" runat="server" ></asp:TextBox></td>
<td></td>
</tr>
<tr>
<td colspan="3" align="Left">
<asp:Button ID="ctlAdd" runat="server" Text="Add" Width="80px" 
        onclick="ctlAdd_Click" />
</td>
</tr>
</table>
<br /><center>
<asp:GridView ID="ctlExpenseGridview" Width="98%" runat="server" ShowFooter="true" AutoGenerateColumns="false">
<Columns>
<asp:BoundField HeaderText="$Cost Center$" DataField="CostCenter"/>
<asp:BoundField HeaderText="$Account Code$" DataField="AccountCode"/>
<asp:BoundField HeaderText="$Internal Order$" DataField="InternalOrder"/>
<asp:BoundField HeaderText="$Description$" DataField="Description"/>
<asp:TemplateField HeaderText="$Amount$">
<ItemTemplate>
<asp:Label ID="ctlAmountLabel" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="ctlTotalAmount" runat="server"></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="$Referance No.$" DataField="ReferanceNo"/>
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" />
<asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</center>
<br />
<div id="ctlChkDiv" runat="server" visible="false">
<asp:CheckBox ID="ctlVatChk" runat="server" Text="VAT" 
        oncheckedchanged="ctlVatChk_CheckedChanged" AutoPostBack="True" 
    />&nbsp;&nbsp;
<asp:CheckBox ID="ctlWhtChk" runat="server" Text="WHT" 
    oncheckedchanged="ctlWhtChk_CheckedChanged" AutoPostBack="True" />
    </div>
<br />
<fieldset id="ctlInvoicefds" runat="server" visible="false">
<legend id="ctlInvoicefdsLegend" runat="server"><asp:Label ID="ctlLegendText" runat="server" Text="Invoice"></asp:Label>&nbsp;:&nbsp;</legend>
<table><tr>
<td style="width:60%">
<table width="100%">
<tr>
<td style="width:20%"><asp:Label ID="ctlInvoiceNoLabel" runat="server" Text="$Invoice No.$"></asp:Label><asp:Label ID="Label7" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td style="width:30%"><asp:TextBox ID="ctlInvoiceNo" runat="server" Text="15435"></asp:TextBox>&nbsp;&nbsp;</td>
<td style="width:20%"><asp:Label ID="ctlInvoiceDateLabel" runat="server" Text="$Invoice Date$"></asp:Label>&nbsp;:&nbsp;</td>
<td><uc1:Calendar ID="Calendar1" runat="server" /></td>
</tr>
<tr>
<td ><asp:Label ID="ctlVendorCodeLabel" runat="server" Text="$Vendor Code$"></asp:Label><asp:Label ID="Label9" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td ><asp:TextBox ID="ctlVendorCode" runat="server" Text="00009999999"></asp:TextBox>
<asp:ImageButton ID="ImageButton3" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td ></td>
<td></td>
</tr>
<tr>
<td ><asp:Label ID="ctlVendorNameLabel" runat="server" Text="$Vendor Name$"></asp:Label><asp:Label ID="Label11" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td colspan="2"><asp:TextBox ID="ctlVendorName" runat="server" Text="บริษัท ฟิชชั่นซอฟต์ จำกัด" Width="98%"></asp:TextBox></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlVendorTaxIdLabel" runat="server" Text="$Vendor Tax ID$"></asp:Label><asp:Label ID="Label13" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlVendorTaxId" runat="server" Text="32450000897"></asp:TextBox></td>
<td></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlAddressNoLabel" runat="server" Text="$Address No.$"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlAddressNo" runat="server"></asp:TextBox></td>
<td><asp:Label ID="ctlAddressLabel" runat="server" Text="$Address$"></asp:Label>&nbsp;:&nbsp;
</td>
<td><asp:TextBox ID="ctlAddress" runat="server"></asp:TextBox>
</td>
</tr>
<tr>
<td><asp:Label ID="ctlCityLabel" runat="server" Text="$City$"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlCity" runat="server"></asp:TextBox></td>
<td><asp:Label ID="ctlProviceLabel" runat="server" Text="$Provice$"></asp:Label>&nbsp;:&nbsp;
</td>
<td><asp:TextBox ID="ctlProvice" runat="server"></asp:TextBox>
</td>
</tr>
<tr>
<td><asp:Label ID="ctlCountryLabel" runat="server" Text="$Country$"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlCountry" runat="server"></asp:TextBox></td>
<td><asp:Label ID="ctlPostCodeLabel" runat="server" Text="$Post Code$"></asp:Label>&nbsp;:&nbsp;
</td>
<td><asp:TextBox ID="ctlPostCode" runat="server"></asp:TextBox></td>
</tr>
<tr>
<td colspan="4" align="Left">

</td>
</tr>
</table>
</td>
<td valign="top" align="right">
<table id="ctlVAT" runat="server" visible="false" width="40%">
<tr>
<td><asp:Label ID="ctlBaseAmountLabel" runat="server" Text="$Base Amount$"></asp:Label><asp:Label ID="Label8" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:Label ID="ctlVatAountLabel" runat="server" Text="$VAT Amount$"></asp:Label><asp:Label ID="Label12" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:Label ID="ctlNetAmountLabel" runat="server" Text="$Net Amount$"></asp:Label><asp:Label ID="Label15" runat="server" Text="*" style="color:Red;"></asp:Label></td>
</tr>
<tr>
<td><asp:TextBox ID="ctlBaseAmount" runat="server" Text="2,336.45"></asp:TextBox></td>
<td><asp:TextBox ID="ctlVatAount" runat="server" Text="163.55"></asp:TextBox></td>
<td><asp:TextBox ID="ctlNetAmount" runat="server" Text="2500.00"></asp:TextBox></td>
</tr>
</table>
<br />
<table id="ctlwht" runat="server" visible="false">
<tr>
<td><asp:Label ID="ctlWhtRateLabel" runat="server" Text="$WHT Rate$"></asp:Label><asp:Label ID="Label10" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:DropDownList ID="ctlWhtRateDropDown" runat="server">
<asp:ListItem Text="1"></asp:ListItem>
<asp:ListItem Text="2"></asp:ListItem>
<asp:ListItem Text="3"></asp:ListItem>
<asp:ListItem Text="4"></asp:ListItem>
<asp:ListItem Text="5"></asp:ListItem>
<asp:ListItem Text="6"></asp:ListItem>
<asp:ListItem Text="7"></asp:ListItem>
</asp:DropDownList></td>
<td><asp:Label ID="ctlBaseAmountLabel1" runat="server" Text="$Base Amount$"></asp:Label><asp:Label ID="Label16" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:TextBox ID="ctlBaseAmount1" runat="server" Text="3500.00"></asp:TextBox></td>
</tr>
<tr>
<td></td>
<td></td>
<td><asp:Label ID="ctlWhtAmountLabel" runat="server" Text="$WHT Amount$"></asp:Label><asp:Label ID="Label18" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:TextBox ID="ctlWhtAmount" runat="server" Text="35.00"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="ctlWhtRate2" runat="server" Text="$WHT Rate$"></asp:Label><asp:Label ID="Label20" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:DropDownList ID="ctlWhtRateDropDown2" runat="server">
<asp:ListItem Text="1"></asp:ListItem>
<asp:ListItem Text="2"></asp:ListItem>
<asp:ListItem Text="3"></asp:ListItem>
<asp:ListItem Text="4"></asp:ListItem>
<asp:ListItem Text="5"></asp:ListItem>
<asp:ListItem Text="6"></asp:ListItem>
<asp:ListItem Text="7"></asp:ListItem>
</asp:DropDownList></td>
<td><asp:Label ID="ctlBaseAmountLabel2" runat="server" Text="$Base Amount$"></asp:Label><asp:Label ID="Label22" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:TextBox ID="ctlBaseAmount2" runat="server" ></asp:TextBox></td>
</tr>
<tr>
<td></td>
<td></td>
<td><asp:Label ID="ctlWhtAountLabel2" runat="server" Text="$WHT Amount$"></asp:Label><asp:Label ID="Label24" runat="server" Text="*" style="color:Red;"></asp:Label></td>
<td><asp:TextBox ID="ctlWhtAount2" runat="server"></asp:TextBox></td>
</tr>
</table>
</td>
</tr>
</table>

</fieldset>
<br />
<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlGridSelect" onclick="ctlSubmit_Click" />
<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlGridCancel" onclick="ctlCancel_Click"/> 
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
	TargetControlID="lnkDummy"
	PopupControlID="pnAccountSearch"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="pnAccountSearchHeader" />