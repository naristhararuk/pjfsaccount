<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeSummary.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.HomeSummary" EnableTheming="true" %>

<asp:UpdatePanel ID="ctlUpdatePanelSearchResult" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset id="ctlHomeSummaryFieldSet" runat="server" style="width:623px;vertical-align:top;">
            <div id="ctlDivEmployee" runat="server">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table" >
                    <tr>
                        <td align="left" colspan="4" style="background-color:#51dc64 ;height:25px">
                            <asp:Label ID="ctlEmployeeHeader" Text="$Employee$" SkinID="SkGeneralLabel" runat="server" ForeColor="White"></asp:Label>
                        </td>                
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlTotalInboxEmp" Text="$Total Inbox$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlTotalInboxEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("TotalInboxEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItems1" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlRejectWithdrawEmp" Text="$Reject/Withdraw$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlRejectWithdrawEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("RejectWithdrawEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem2" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforAgreeEmp" Text="$Wait for Agree$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforAgreeEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("WaitforAgreeEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem3" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforInitialEmp" Text="$Wait for Initial$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforInitialEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("WaitforInitialEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem4" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforApproveEmp" Text="$Wait for Approve$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforApproveEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("WaitforApproveEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem5" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlHoldEmp" Text="$Hold$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlHoldEmpSummary" runat="server" OnClick="InboxEmployee_Click" Text='<%#Eval("HoldEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem6" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlDraftEmp" Text="$Draft$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlDraftEmpSummary" runat="server" OnClick="InboxDraft_Click" Text='<%#Eval("DraftEmp") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem7" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlTotalUnclearAdvanceEmp" Text="$Total Outstanding Advance$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlTotalUnclearAdvanceEmpSummary" runat="server" Text='<%#Eval("TotalUnclearAdvanceEmp") %>'></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem8" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlTotalUnclearAdvanceOverdueEmp" Text="$Total Advance Overdue$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:Label ID="ctlTotalUnclearAdvanceOverdueEmpSummary" runat="server" Text='<%#Eval("TotalUnclearAdvanceOverdueEmp") %>'></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem9" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="ctlDivAccountant" runat="server">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table">
                    <tr>
                        <td align="left" colspan="4" style="background-color:#51dc64 ;height:25px">
                            <asp:Label ID="ctlAccountantHeader" Text="$Accountant$" SkinID="SkGeneralLabel" runat="server" ForeColor="White"></asp:Label>
                        </td>                
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlTotalInboxAcc" Text="$Total Inbox$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlTotalInboxAccSummary" runat="server" OnClick="InboxAccountant_Click" Text='<%#Eval("TotalInboxAcc") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem10" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforReceiveAcc" Text="$Wait for Receive$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforReceiveAccSummary" runat="server" OnClick="InboxAccountantWaitforReceive_Click" Text='<%#Eval("WaitforReceiveAcc") %>' CommandArgument="Wait for Document"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem11" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforVerifyAcc" Text="$Wait for Verify$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforVerifyAccSummary" runat="server" OnClick="InboxAccountantWaitforVerify_Click" Text='<%#Eval("WaitforVerifyAcc") %>' CommandArgument="Wait for Verify"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem12" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforApproveVerifyAcc" Text="$Wait for Approve Verify$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforApproveVerifyAccSummary" runat="server" OnClick="InboxAccountantWaitforApproveVerify_Click" Text='<%#Eval("WaitforApproveVerifyAcc") %>' CommandArgument="Wait for Approve Verify" ></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem13" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                    <td style="width:10%">&nbsp;</td>
                    <td align="left" style="width:30%">
                        <asp:Label ID="ctlHoldAcc" Text="$Hold$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                    </td>
                    <td align="center" style="width:10%">
                        <asp:LinkButton ID="ctlHoldAccSummary" runat="server" OnClick="InboxAccountantHold_Click" Text='<%#Eval("HoldAcc") %>' CommandArgument="Hold"></asp:LinkButton>
                    </td>
                    <td align="left">
                        <asp:Label ID="ctlItem14" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                    </td>
                </tr>
                </table>
            </div>
            <div id="ctlDivPayment" runat="server">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table">
                    <tr>
                        <td align="left" colspan="4" style="background-color:#51dc64 ;height:25px">
                            <asp:Label ID="ctlPaymentHeader" Text="$Payment$" SkinID="SkGeneralLabel" runat="server" ForeColor="White"></asp:Label>
                        </td>                
                    </tr>
                    <tr>
                        <td align="left" style="width:30%" colspan="2">
                            <asp:Label ID="ctlTotalInboxPay" Text="$Total Inbox$" SkinID="SkGeneralLabel" runat="server"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlTotalInboxPaySummary" runat="server" OnClick="InboxPayment_Click" Text='<%#Eval("TotalInboxPay") %>'></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem15" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforVerifyPay" Text="$Wait for Verify$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforVerifyPaySummary" runat="server" OnClick="InboxPaymentWaitforVerify_Click" Text='<%#Eval("WaitforVerifyPay") %>' CommandArgument="Wait for Verify"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem16" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforApproveVerifyPay" Text="$Wait for Approve Verify$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforApproveVerifyPaySummary" runat="server" OnClick="InboxPaymentWaitforApproveVerify_Click" Text='<%#Eval("WaitforApproveVerifyPay") %>' CommandArgument="Wait for Approve Verify"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem17" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforPaymentPay" Text="$Wait for Payment$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforPaymentPaySummary" runat="server" OnClick="InboxPaymentWaitforPayment_Click" Text='<%#Eval("WaitforPaymentPay") %>' CommandArgument="Wait for Payment"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem18" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforRemittancePay" Text="$Wait for Remittance$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforRemittancePaySummary" runat="server" OnClick="InboxPaymentWaitforRemittance_Click" Text='<%#Eval("WaitforRemittancePay") %>' CommandArgument="Wait for Remittance"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem19" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">&nbsp;</td>
                        <td align="left" style="width:30%">
                            <asp:Label ID="ctlWaitforApproveRemittancePay" Text="$Wait for Approve Remittance$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                        <td align="center" style="width:10%">
                            <asp:LinkButton ID="ctlWaitforApproveRemittancePaySummary" runat="server" OnClick="InboxPaymentWaitforApproveRemittance_Click" Text='<%#Eval("WaitforApproveRemittancePay") %>' CommandArgument="Wait for Approve Remittance"></asp:LinkButton>
                        </td>
                        <td align="left">
                            <asp:Label ID="ctlItem20" Text="$items$" runat="server" SkinID="SkGeneralLabel"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>