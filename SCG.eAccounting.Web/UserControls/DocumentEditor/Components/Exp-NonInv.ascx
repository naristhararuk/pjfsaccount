<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Exp-NonInv.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Exp_NonInv" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:Panel ID="pnAccountSearch" runat="server" Width="900px" BackColor="White">
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
<table width="100%">
<tr>
<td style="width:20%"><asp:Label ID="ctlCostCenterLabel" runat="server" Text="$Cost Center$"></asp:Label><asp:Label ID="Label4" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td style="width:30%"><asp:TextBox ID="ctlCostCenter" runat="server" Text="0110-94300"></asp:TextBox>
<asp:ImageButton ID="ctlCostCenterImg" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td style="width:20%"></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlAccountCodeLabel" runat="server" Text="$Account Code$"></asp:Label><asp:Label ID="Label5" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td colspan="2"><asp:TextBox ID="ctlAccountCode" runat="server" Text="0311022922"></asp:TextBox>
<asp:ImageButton ID="ImageButton1" runat="server" SkinID="SkCtlGridSelect" />
<asp:TextBox ID="ctlAccountDetail" runat="server" Text="ค่าทางด่วน" ></asp:TextBox></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlInternalOrderLabel" runat="server" Text="$Internal Order$"></asp:Label><asp:Label ID="ctlCompanyReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlInternalOrder" runat="server" Text="64031"></asp:TextBox>
<asp:ImageButton ID="ImageButton2" runat="server" SkinID="SkCtlGridSelect" />
</td>
<td></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label><asp:Label ID="Label1" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td colspan="3"><asp:TextBox ID="ctlDescription" runat="server" Text="เบิกค่าทางด่วน" Width="98%"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="ctlServiceFromDateLabel" runat="server" Text="$Service From Date$"></asp:Label>&nbsp;:&nbsp;</td>
<td><uc1:Calendar ID="ctlServiceFromDateCal" runat="server" />
</td>
<td><asp:Label ID="ctlServiceToDateLabel" runat="server" Text="$Service To Date$"></asp:Label>&nbsp;:&nbsp;</td>
<td><uc1:Calendar ID="ctlServiceToDateCal" runat="server" /></td>
</tr>
<tr>
<td><asp:Label ID="ctlAmountLabel" runat="server" Text="$Amount$"></asp:Label><asp:Label ID="Label2" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlAmount" runat="server" Text="90.00"></asp:TextBox></td>
<td></td>
<td></td>
</tr>
<tr>
<td><asp:Label ID="ctlReferanceNoLabel" runat="server" Text="$Referance No.$"></asp:Label><asp:Label ID="Label6" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;</td>
<td><asp:TextBox ID="ctlReferanceNo" runat="server" ></asp:TextBox></td>
<td></td>
<td></td>
</tr>
<tr>
<td colspan="4" align="center" style="text-align: left">
<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlGridSelect" onclick="ctlSubmit_Click"/> 
<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlGridCancel" onclick="ctlCancel_Click"/>
</td>
</tr>
</table>

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