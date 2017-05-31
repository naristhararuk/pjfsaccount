<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorField.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.VendorField" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/VendorLookUp.ascx" TagName="Vendor" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/VendorTextBoxAutoComplete.ascx" TagName="Auto" TagPrefix="uc2" %>

<asp:UpdatePanel ID="ctlUpdatePanelVendor" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
<table class="table">
    <tr>
		<td align="left" valign="middle" style="width:100px;">
			<asp:Label ID="ctlMode" runat="server" style="display:none;"></asp:Label>
			<asp:Label ID="lblVendorTaxNo" runat="server" Text="$VendorTaxNo$" SkinID="SkCtlLabel"></asp:Label>
        </td>
		<td valign="middle" align="left">
		    <uc2:Auto id = "ctlTaxNo" runat="server" OnNotifyPopupResult="ctlTaxNo_NotifyPopupResult"/>
		</td>
		<td valign="middle" align="left" style="width:120px;">
			<asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" onclick="ctlSearch_Click" />
            <asp:Label ID="lblVendorCode" runat="server" Text="$VendorCode$" SkinID="SkCtlLabel"></asp:Label>
		</td>
		<td valign="middle" align="left">
			<asp:TextBox ID="ctlVendorCode" SkinID="SkCtlLongTexboxCenter" ReadOnly="true" runat="server"></asp:TextBox>
		</td>
	</tr>   
	 <tr>
		<td align="left" valign="middle" style="width:100px;">
			<asp:Label ID="lblVendorName" runat="server" Text="$VendorName$" SkinID="SkCtlLabel"></asp:Label>
        </td>
		<td valign="middle" align="left" colspan="3">
			<asp:TextBox ID="ctlVendorName" SkinID="SkCtlLongTexboxLeft" runat="server" ReadOnly="true"></asp:TextBox>
		</td>
	</tr>
	 <tr>
		<td align="left" valign="middle" style="width:100px;">
			<asp:Label ID="lblStreet" runat="server" Text="$Street$" SkinID="SkCtlLabel"></asp:Label>
        </td>
		<td valign="middle" align="left">
			<asp:TextBox ID="ctlStreet" SkinID="SkCtlShortTextboxLeft" runat="server" ReadOnly="true"></asp:TextBox>
		</td>
		<td valign="middle" align="left" style="width:120px;">
            <asp:Label ID="lblCity" runat="server" Text="$City$" SkinID="SkCtlLabel"></asp:Label>
		</td>
		<td valign="middle" align="left">
			<asp:TextBox ID="ctlCity" SkinID="SkCtlShortTextboxLeft" ReadOnly="true" runat="server"></asp:TextBox>
		</td>
	</tr> 
	<tr>
		<td align="left" valign="middle" style="width:100px;">
			<asp:Label ID="lblCountry" runat="server" Text="$Country$" SkinID="SkCtlLabel"></asp:Label>
        </td>
		<td valign="middle" align="left">
			<asp:TextBox ID="ctlCountry" SkinID="SkCtlShortTextboxLeft" ReadOnly="true" runat="server"></asp:TextBox>
		</td>
		<td valign="middle" align="left" style="width:120px;">
            <asp:Label ID="lblPostalCode" runat="server" Text="$PostalCode$" SkinID="SkCtlLabel"></asp:Label>
		</td>
		<td valign="middle" align="left">
			<asp:TextBox ID="ctlPostalCode" SkinID="SkCtlShortTextboxLeft" ReadOnly="true" runat="server"></asp:TextBox>
		</td>
	</tr>          
</table>
<table>
        <tr><td>&nbsp;</td></tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
<uc1:Vendor ID="Vendor1" runat="server" isMultiple="false"/>
