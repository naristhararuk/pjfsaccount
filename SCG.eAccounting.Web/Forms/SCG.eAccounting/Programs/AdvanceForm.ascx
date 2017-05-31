    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceForm.ascx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.AdvanceForm" EnableTheming="true" %>

<%@ Register src="~/UserControls/DocumentEditor/AdvanceDocumentEditor.ascx" tagname="AdvanceDocumentEditor" tagprefix="uc7" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">--%>
<script language="javascript" type="text/javascript">
    function confirm_save()
    {
        var hiddenContirm = document.getElementById('<%= ctlHiddenConfirm.ClientID %>');
        var hiddenTaDocument = document.getElementById('<%= ctlHiddenTaDocument.ClientID %>');
  
        if (hiddenTaDocument.value != "TADocument") 
        {
            if (confirm('คุณต้องการ submit เอกสารนี้หรือไม่') == true) {
                hiddenContirm.value = "ok"
                return true;
            }
            else {
                hiddenContirm.value = "cancel";
                return false;
            }
        }
    }

</script>
<table width="100%">
    <tr>
        <td>
            <uc7:AdvanceDocumentEditor ID="ctlAdvanceDocumentEditor" runat="server" />
            <asp:HiddenField id="ctlHiddenConfirm" runat="server"/>
            <asp:HiddenField ID="ctlHiddenTaDocument" runat="server" />
        </td>
    </tr>
    <tr>
		<td align="left">
			<asp:UpdatePanel ID="ctlUpdatePanelReadwriteButton" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<div id="ctlDivReadwriteButton" runat="server">
						<table width="100%"><tr>
							<td valign="middle"><asp:ImageButton ID="ctlSave" runat="server" SkinID="SkCtlGridSave" ToolTip="Save" onclick="ctlSave_Click" /></td>
							<td valign="middle"> | </td>
							<td valign="middle"><asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlGridCancel" ToolTip="Cancel" onclick="ctlCancel_Click" /></td>
							<td valign="middle" style="width:100%"></td>
						</tr></table>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table border="0" width="400px;">
						<tr>
							<td align="left" style="color: Red; font-family: Tahoma; font-size: 10pt;">
								<spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error" />
							</td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
    </tr>
</table>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>

<%--</asp:Content>--%>