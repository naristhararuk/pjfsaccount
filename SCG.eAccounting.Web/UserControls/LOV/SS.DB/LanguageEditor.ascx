<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageEditor.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.LanguageEditor" %>
<table>
    <tr>
        <td align="right"><asp:Label ID="ctlLblName" runat="server" Text="$Language$"></asp:Label> :</td>
        <td align="left">
            <asp:TextBox ID="ctlLanguage" runat="server" MaxLength="200" Text='<%# Bind("LanguageName") %>' Width="163px" />
            <asp:Label ID="ctlTxtNameReq" runat="server" Text="*" style="color:Red;"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="ctlLblLanguageCode" runat="server" Text="$Language Code$"></asp:Label> :
        </td>
        <td align="left">
            <asp:TextBox ID="ctlLanguageCode" runat="server" MaxLength="50" Text='<%# Bind("LanguageCode") %>' Width="163px" />
            <asp:Label ID="lblTxtCodeReq" style="color:Red;" Text="*" runat="server"></asp:Label>
        </td>
    </tr>
    <div runat="server" id="ctlImageArea">
    <tr>
		<td align="right"><asp:Label ID="ctlLblImage" runat="server" Text="$Image$"></asp:Label> :</td>
		<td align="left">
			<asp:Image ID="ctlImage" runat="server" /><br />
		</td>
	</tr>
	</div>
	<tr>
		<td align="right"><asp:Label ID="ctlLblNewImage" runat="server" Text="$New Image$"></asp:Label> :</td>
		<td align="left">
			<asp:FileUpload ID="ctlImageFile" Width="250px" runat="server" />
		</td>
	</tr>
    <tr>
        <td align="right">
            <asp:Label ID="ctlLblComment" runat="server" Text="$Comment$"></asp:Label> :
        </td>
        <td align="left">
            <asp:TextBox ID="ctlComment" MaxLength="500" runat="server" Text='<%# Bind("Comment") %>' Width="163px" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="ctlLblActive" runat="server" Text="$Active$"></asp:Label> :
        </td>
        <td align="left">
            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                ValidationGroup="ValidateFormView" CommandName="Update" Text="Update"></asp:ImageButton>
             <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                ValidationGroup="ValidateFormView" CommandName="Insert" Text="Insert"></asp:ImageButton>
            <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                CommandName="Cancel" Text="Cancel"></asp:ImageButton>
        </td>
    </tr>
</table>