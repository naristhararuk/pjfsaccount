<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReceiveDocument.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.ReceiveDocument" EnableTheming="true" %>
<asp:UpdatePanel ID="UpdatePanelOverRole" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table">
            <tr>
                <td>
                    <fieldset id="ctlReceiveFieldset" runat="server" style="width: 99%;">
                        <legend id="ctlReceiveLegend" style="color: #4E9DDF;">
                            <asp:Label ID="ctlReceiveDetailLabel" runat="server" Text="$Receive Detail$"></asp:Label>
                        </legend>
                        <table style="width:50%">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="ctlBoxIDLabel" runat="server" Text="$Box ID$ :" SkinID="SkFieldCaptionLabel"></asp:Label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="ctlBoxID" runat="server" MaxLength="15" SkinID="SkGeneralTextBox" OnKeyPress="keyAlphaNumeric();" 
                                    OnPaste="return false" OnDrop="return false"/>
                                </td>
                                <td align="left">
                                    <asp:Label ID="ctlBoxIdRemark" runat="server" Text="(15 characters)" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
