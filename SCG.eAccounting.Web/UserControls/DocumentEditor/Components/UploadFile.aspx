<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFile.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.UploadFile"
    MasterPageFile="~/PopupMasterPage.Master" EnableTheming="true" StylesheetTheme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
      function Validate() {
          debugger;
          var bIsValid = true;
          var uploadedFile = document.getElementById("<%=ctlFileUpload.ClientID %>");

          if (uploadedFile.files[0].size > 1024000) 
          {
              bIsValid = false;
          }

          if (!bIsValid) {
              document.getElementById('<%=ValidationFileSize.ClientID %>').style.display = "block";
              return false;
          } else {
              document.getElementById('<%=ValidationFileSize.ClientID %>').style.display = "none";
              return true;
          }
          //On Success
             
      }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table width="100%">
        <tr>
            <td align="right">
                <asp:Label ID="ctlFileUploadText" runat="server" SkinID="SkFieldCaptionLabel" Text="Attach File"></asp:Label>
                &nbsp;:&nbsp;
            </td>
            <td align="left">
                <asp:FileUpload ID="ctlFileUpload" runat="server" Width="360px" />
                <asp:HiddenField ID="ctlDocumentId" runat="server" />
                <asp:HiddenField ID="ctlPath" runat="server" />
            </td>
            <td align="center" style="width: 10%">
                <asp:ImageButton runat="server" ID="ctlAttach" SkinID="SkAddButton" ToolTip="$Attach$"
                   OnClientClick="Javascript: return Validate()" OnClick="ctlAttach_OnClick" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table border="0" cellpadding="0" cellspacing="0" width="300px">
                    <tr>
                        <td align="left" style="color: Red; font-family: Tahoma; font-size: 8pt;">
                            <asp:Label style="font-weight:normal" ID="ValidationFileSize" runat="server" SkinID="SkFieldCaptionLabel" Text="AttachmentFileSizeNotAllow"></asp:Label>
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ValidationError">
                            </spring:ValidationSummary>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
