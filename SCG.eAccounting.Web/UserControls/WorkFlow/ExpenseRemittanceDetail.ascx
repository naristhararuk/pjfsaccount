<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseRemittanceDetail.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.WorkFlow.ExpenseRemittanceDetail"
    EnableTheming="true" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>

<asp:UpdatePanel ID="UpdatePanelOverRole" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table">
            <tr>
                <td>
                    <fieldset id="ctlRemittanceFieldset" runat="server" style="width: 99%;">
                        <legend id="ctlRemittanceLegend" style="color: #4E9DDF;">
                            <asp:Label ID="ctlRemittanceDetailLabel" runat="server" Text="$Pay-In Detail$"></asp:Label>
                        </legend>
                        <table style="width: 55%">
                            <tr>
                                <td style="width: 30%">
                                    <asp:Label ID="ctlPayInMethodLabel" runat="server" Text="$Pay-In Method$" SkinID="SkFieldCaptionLabel" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="ctlCashierMethod" runat="server" AutoPostBack="True" GroupName="Method"
                                        Text="$Cashier$"/>
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="ctlBankMethod" runat="server" AutoPostBack="True" GroupName="Method"
                                        Text="$Bank$" />
                                </td>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="ctlKBankDetailPanel" runat="server" Visible="false">
                                            <table style="width: 54%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="ctlGLAccountLabel" runat="server" SkinID="SkFieldCaptionLabel" 
                                                            Text="$GLAcc$"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="ctlGLAccount" runat="server" SkinID="SkGeneralTextBox" MaxLength="10"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="ctlValueDateLabel" runat="server" SkinID="SkFieldCaptionLabel" 
                                                            Text="$ValueDate$"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc1:Calendar ID="ctlValueDate" runat="server" SkinID="SkGeneralTextBox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                    </fieldset>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
