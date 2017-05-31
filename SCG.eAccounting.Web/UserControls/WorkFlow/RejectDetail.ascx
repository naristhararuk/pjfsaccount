<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="RejectDetail.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.RejectDetail" 
    EnableViewState ="true"
%>
<asp:Panel ID="pnApproveRejection" runat="server" Width="100%" BackColor="White">
    <asp:UpdatePanel ID="UpdatePanelSearchProgram" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	        <center>
	        <table  class="table" width="100%">
                <tr>
                    <td>
                        <fieldset id="PReject" style="background-color:#edeff5">
                            <legend id="Legend1" class="table" style="color: #4E9DDF">
                                <asp:Label ID="lblPReject" runat="server" SkinID="SkGeneralLabel" Text="Reject"></asp:Label>
                            </legend>
                            <center>
                            <table style="height: 107px; width: 98%" style="text-align: left">
                                <tr>
                                    <td style="width:10%">
                                        <asp:Label ID="lblReason" SkinID="SkCtlLabel" runat="server" Text="Reason :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ctlReasonDdl"  SkinID="SkGeneralDropdown" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRemark" SkinID="SkCtlLabel" runat="server" Text="Remark :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ctlRemark" runat="server" SkinID="SkCtlTextboxMultiLine" TextMode="MultiLine" onkeypress="return IsMaxLength(this, 200);"
                                            onkeyup="return IsMaxLength(this, 200);"></asp:TextBox>
                                    </td>
                                 </tr>
                            </Table>
                            </center>
                        </fieldset>
                    </td>
                </tr>
             </table>
	        </center>    
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
