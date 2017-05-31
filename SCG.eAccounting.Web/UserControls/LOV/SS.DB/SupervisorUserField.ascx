<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupervisorUserField.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.SupervisorUserField" %>

<%@ Register src="SupervisorTextBoxAutoComplete.ascx" tagname="SupervisorTextBoxAutoComplete" tagprefix="uc1" %>

<asp:UpdatePanel ID="ctlUpdatePanelUser" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
		<table class="table" id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td align="left">

					<uc1:SupervisorTextBoxAutoComplete ID="ctlUserTextBoxAutoComplete" OnNotifyPopupResult="ctlAutoUser_NotifyPopupResult"  
					UserID="<%# this.UserID %>"  runat="server" />
					
					<asp:Label ID="ctlMode" runat="server" Style="display: none"></asp:Label>
			
					<asp:HiddenField ID="ctlUserName" runat="server" />
					<asp:HiddenField ID="ctlUserID" runat="server" />
				</td>
				<td align="left">
					<asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
				</td>
				<td>
				  <asp:Label ID="ctlDescription" runat="server" SkinID="SkGeneralLabel"></asp:Label>
				</td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>












