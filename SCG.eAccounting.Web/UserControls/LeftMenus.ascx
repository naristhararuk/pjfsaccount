<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenus.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LeftMenus"
    EnableViewState="true" %>
<table border="0" cellpadding="0" cellspacing="0" class="backGroundMenu">
    <tr>
        <td style="width: 20px">
            &nbsp;&nbsp;&nbsp;
        </td>
        <td align="left" valign="top">
            <br />
            <ajaxToolkit:Accordion ID="MenuAccordion" runat="Server" SelectedIndex="-1" Height="300px" Width = "250px"
                AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true" EnableViewState="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="RequestPane" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
                        runat="server">
                        <Header>
                            <asp:Panel ID="RequestHeader" runat="server" SkinID="AccordionHeader">
                             <asp:Label ID="RequestHeaderLabel" runat="server" Text="$Request Form$"  />
                            </asp:Panel>
                        </Header>
                        <Content>
                            <asp:Panel ID="RequestLink" runat="server" SkinID="AccordionChild">
                              
                             &nbsp;<asp:HyperLink ID="RequestTA" runat="server" SkinID="SkHyperLinkLeftMenu"
                                    NavigateUrl="~/Forms/SCG.eAccounting/Programs/TAForm.aspx" Text="$TA$"></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="RequestAdvanceDomestic" SkinID="SkHyperLinkLeftMenu"
                                    runat="server" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AdvanceDomesticForm.aspx" Text= "$Advance(Domestic)$"></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="RequestAdvanceForeign" SkinID="SkHyperLinkLeftMenu"
                                    runat="server" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AdvanceForeignForm.aspx" Text = "$Advance(Foreign)$"></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="RequestExpenseDomestic" SkinID="SkHyperLinkLeftMenu"
                                    runat="server" NavigateUrl="~/Forms/SCG.eAccounting/Programs/ExpenseFormDM.aspx" Text = "$Expense(Domestic)$" ></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="RequestExpenseForeign" SkinID="SkHyperLinkLeftMenu"
                                    runat="server" NavigateUrl="~/Forms/SCG.eAccounting/Programs/ExpenseFormFR.aspx" Text = "$Expense(Foreign)$"></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="FixedAdvance" SkinID="SkHyperLinkLeftMenu"
                                    runat="server" NavigateUrl="~/Forms/SCG.eAccounting/Programs/FixedAdvanceDomesticForm.aspx" Text ="$FixedAdvance$"></asp:HyperLink><br />
                                &nbsp;<asp:HyperLink ID="RequestRemittance" runat="server" SkinID="SkHyperLinkLeftMenu"
                                    NavigateUrl="~/Forms/SCG.eAccounting/Programs/RemittanceForm.aspx" Text = "$Remittance$"></asp:HyperLink><br />
                                <div id="MobilePhone" runat="server">&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="MobilePhoneAuthorization" runat="server" SkinID="SkHyperLinkLeftMenu"
                                    NavigateUrl="~/Forms/SCG.eAccounting/Programs/MPAForm.aspx" Text = "$MobilePhoneAuthorization$"></asp:HyperLink><br /></div>
                                <div id="Car" runat="server">&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="CarAuthorization" runat="server" SkinID="SkHyperLinkLeftMenu"
                                    NavigateUrl="~/Forms/SCG.eAccounting/Programs/CAForm.aspx" Text = "$CarAuthorization$"></asp:HyperLink></div>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="EmployeePane" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
                        runat="server">
                        <Header>
                            <asp:Panel ID="EmployeeHeader" runat="server" SkinID="AccordionHeader">
                                <asp:Label ID="EmployeeHeaderLabel" runat="server" Text="$Employee$"  /></asp:Panel>
                                
                        </Header>
                        <Content>
                             <asp:Panel ID="EmployeeLink" runat="server" SkinID="AccordionChild">
                                <div runat="server" id="ctlDivEmployeeDraftLink">
                                &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="EmployeeRequest" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/Draft.aspx" Text= "$Draft$"></asp:HyperLink><br />
                                </div>
                                <div runat="server" id="ctlDivEmployeeInboxLink">
                                &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="EmployeeInbox" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/EmployeeInbox.aspx" Text = "$Inbox$"></asp:HyperLink><br />
                                </div>
                                <div runat="server" id="ctlDivEmployeeHistoryLink">
                                &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="EmployeeSearch" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/EmployeeHistory.aspx" Text = "$History$"></asp:HyperLink>
                                </div>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="AccountantPane" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
                        runat="server">
                        <Header>
                            <asp:Panel ID="AccountantHeader" runat="server" SkinID="AccordionHeader">
                                <asp:Label ID="AccountantHeaderLabel" runat="server" Text="$Accountant$"  /></asp:Panel>
                        </Header>
                        <Content>
                            <asp:Panel ID="AccountantLink" runat="server" SkinID="AccordionChild">
                             <div runat="server" id="ctlDivAccountantInbox">
                             &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="AccountantInbox" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AccountantPaymentInbox.aspx" Text = "$Inbox$"></asp:HyperLink><br />
                             </div>
                             <div runat="server" id="ctlDivAccountantHistory">
                             &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="AccountantSearch" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AccountantHistory.aspx" Text = "$History$"></asp:HyperLink><br />
                             </div>
                             <div runat="server" id="ctlDivAccountantSearch">
                             &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="AccountantSearch1" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AccountantSearch.aspx" Text = "$Search$"></asp:HyperLink><br />
                             </div>
                             <div runat="server" id="ctlDivAccountantMonitoring">
                             &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="AccountantMonitoring" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/AccountantMonitoring.aspx" Text = "$AccountantMonitoring$"></asp:HyperLink><br />
                             </div>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="PaymentPane" runat="server">
                        <Header>
                            <asp:Panel ID="PaymentHeader" runat="server" SkinID="AccordionHeader">
                                <asp:Label ID="PaymentHeaderLabel" runat="server" Text="$Payment$"  /></asp:Panel>
                        </Header>
                        <Content>
                            <asp:Panel ID="PaymentLink" runat="server" SkinID="AccordionChild">
                            <div runat="server" id="ctlDivPaymentInbox">
                            &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="PaymentInbox" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/PaymentInbox.aspx" Text = "$Inbox$"></asp:HyperLink><br />
                            </div>
                            <div runat="server" id="ctlDivPaymentHistory">
                            &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="PaymentSearch" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/PaymentHistory.aspx" Text = "$History$"></asp:HyperLink><br />
                            </div>
                            <div runat="server" id="ctlDivPaymentSearch">
                            &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="PaymentSearch1" runat="server" SkinID="SkHyperLinkLeftMenu" NavigateUrl="~/Forms/SCG.eAccounting/Programs/PaymentSearch.aspx" Text = "$Search$"></asp:HyperLink><br />
                            </div>
                            </asp:Panel>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
            <%--<asp:LinkButton ID="Search" runat="server" SkinID="SkLinkHeaderMenu" PostBackUrl="~/Forms/SCG.eAccounting/Programs/Search.aspx" Text ="$Search$"></asp:LinkButton>--%>
            <div runat="server" id="ctlDivSearchAll">
            <asp:HyperLink ID="Search" runat="server" SkinID="SkHyperLinkLeftMenuHeader" NavigateUrl="~/Forms/SCG.eAccounting/Programs/Search.aspx" Text ="$Search$"></asp:HyperLink><br />
            </div>
            <div runat="server" id="DivTASearch">
            <asp:HyperLink ID="ctlTASearchLink" runat="server" SkinID="SkHyperLinkLeftMenuHeader" NavigateUrl="~/Forms/SCG.eAccounting/Programs/TASearch.aspx" Text ="TA Search"></asp:HyperLink><br />
            </div>
            <br />
            <div runat="server" id="ctlDivArchiveLink">
            <asp:LinkButton ID="ctlArchiveButton" runat="server" OnClick="ctlArchiveButton_Click" Text="Archive" SkinID="SkLinkLeftMenuHeaderArchive"></asp:LinkButton>
            </div>
            <div runat="server" id="ctlDivEXpenseLink">
            <asp:LinkButton ID="ctlBackToExpenseButton" runat="server" OnClick="ctlBackButton_Click" Text="Back To e-Xpense" SkinID="SkLinkLeftMenuHeaderArchive"></asp:LinkButton>
            </div>
            <div runat="server" id="ctlDivGoeXpenseLink">
            <asp:LinkButton ID="ctlGoeXpenseButton" runat="server" OnClick="ctlGoeXpenseButton_Click" Text="e-Xpense 4.7" SkinID="SkLinkLeftMenuHeaderArchive"></asp:LinkButton>
            </div>
        </td>
    </tr>
</table>
