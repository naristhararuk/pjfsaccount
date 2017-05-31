<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LeftMenu" %>
<script type="text/javascript" language="javascript">
    var imgRightArrowPath = '<%= ResolveUrl("~/App_Themes/Default/images/icon/anired05_next.gif") %>';
    var imgLeftArrowPath = '<%= ResolveUrl("~/App_Themes/Default/images/icon/anired05_back.gif") %>';
    function MenuControl()
    {
            if (event.srcElement.value == '<')
            {
                event.srcElement.value = '>';
                document.getElementById('<%= btnHideMenu.ClientID %>').click();
                document.getElementById('ctlClose').src = imgRightArrowPath
                // Call function on global.js for set cookie
                SetCookie("leftMenuDisplay", ">");
            }
            else
            {
                event.srcElement.value = '<';
                document.getElementById('<%= btnDisplayMenu.ClientID %>').click();
                document.getElementById('ctlClose').src = imgLeftArrowPath
                // Call function on global.js for set cookie
                SetCookie("leftMenuDisplay", "<");
            }
    }

    var ad;
    function BeginInitAccordion()
    {
        try
        {
            // Call function on global.js for get cookie
            if (GetCookie("leftMenuDisplay") == '>') {
                document.getElementById('<%= MenuZone.ClientID %>').style.width = 0;
                document.getElementById('<%= btnMenuController.ClientID %>').value = '>';
                document.getElementById('ctlClose').src = imgRightArrowPath
            }
            InitAccordion();
        }
        catch (exception) { }
        if (ad == null)
        {
            // Loop until Accordion has been finish loading.
            setTimeout('BeginInitAccordion();', 500);
        }
    }

    // Call function after finish loading to complete set cookie to control Accordian
    window.onload = BeginInitAccordion;

    function InitAccordion() {

        ad = $find("<%= MenuAccordion.ClientID %>_AccordionExtender");

        //Get the value from Cookie
        getIndexFromCookie();

        ad.add_selectedIndexChanged(function() {
            //Save the SelectedIndex in the Cookie
            saveIndexInCookie();
        });
    }

</script>
<center>
    <table style="height:100%;vertical-align:top;">
        <tr>
            <td style="width:10px">&nbsp;</td>
            <td style="height:30px;" align="right" valign="bottom">
                <img id="ctlClose" alt="" style="cursor:hand;" onclick="document.getElementById('<%= btnMenuController.ClientID %>').click();" src="<%= ResolveUrl("~/App_Themes/Default/images/icon/anired05_back.gif") %>"/>
            </td>
        </tr>
        <tr>
            <td></td>
            <td valign="top" align="center">
                <div id="MenuZone" runat="server" style="text-align:center;height:300px;width:140px;overflow:hidden;">
                    <ajaxToolkit:Accordion
                        ID="MenuAccordion"
                        runat="Server"
                        SelectedIndex="-1"
                        Height="300px"
                        AutoSize="None"
                        FadeTransitions="true"
                        TransitionDuration="250"
                        FramesPerSecond="40"
                        RequireOpenedPane="false"
                        SuppressHeaderPostbacks="true"
                        EnableViewState="true">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="RequestPane" runat="server">
                                <Header>
                                    <asp:Panel id="RequestHeader" runat="server" SkinID="AccordionHeader">Request</asp:Panel>
                                </Header>
                                <Content>
                                    <asp:Panel id="RequestLink" runat="server" SkinID="AccordionChild">
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="RequestTA" runat="server" PostBackUrl="~/Forms/SCG.eAccounting/Programs/TAForm.aspx">TA</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="RequestAdvance" runat="server" PostBackUrl="~/Forms/SCG.eAccounting/Programs/AdvanceForm.aspx">Advance</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="RequestExpense" runat="server" PostBackUrl="~/Forms/SCG.eAccounting/Programs/ExpenseForm.aspx" >Expense</asp:LinkButton>
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="EmployeePane"
                                HeaderCssClass="accordionHeader"
                                ContentCssClass="accordionContent" runat="server">
                                <Header>
                                    <asp:Panel id="EmployeeHeader" runat="server" SkinID="AccordionHeader">Employee</asp:Panel>
                                </Header>
                                <Content>
                                    <asp:Panel id="EmployeeLink" runat="server" SkinID="AccordionChild">
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="EmployeeRequest" runat="server">My Request</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="EmployeeInbox" runat="server" PostBackUrl="~/Inbox.aspx">Inbox</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="EmployeeSearch" runat="server" PostBackUrl="~/Search.aspx">Search</asp:LinkButton>
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccountantPane" runat="server" >
                                <Header>
                                    <asp:Panel id="AccountantHeader" runat="server" SkinID="AccordionHeader">Accountant</asp:Panel>
                                </Header>
                                <Content>
                                    <asp:Panel id="AccountantLink" runat="server" SkinID="AccordionChild">
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="AccountantInbox" runat="server" PostBackUrl="~/Inbox.aspx">Inbox</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="AccountantSearch" runat="server" PostBackUrl="~/Search.aspx">Search</asp:LinkButton>
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="PaymentPane" runat="server">
                                <Header>
                                    <asp:Panel id="PaymentHeader" runat="server" SkinID="AccordionHeader">Payment</asp:Panel>
                                </Header>
                                <Content>
                                    <asp:Panel id="PaymentLink" runat="server" SkinID="AccordionChild">
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="PaymentInbox" runat="server" PostBackUrl="~/Inbox.aspx">Inbox</asp:LinkButton><br />
                                        &nbsp;&nbsp;&nbsp;<asp:LinkButton id="PaymentSearch" runat="server" PostBackUrl="~/Search.aspx">Search</asp:LinkButton>
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                        </Panes>            
                    </ajaxToolkit:Accordion>
                </div>
            </td>
            <td style="height:100%">
                <input id="btnMenuController" type="button" runat="server" style="height:0px;width:0px;" value="<" onclick="MenuControl();" />
                <input id="btnDisplayMenu" type="button" runat="server" style="height:0px;width:0px;"/>
                <input id="btnHideMenu" type="button" runat="server" style="height:0px;width:0px;"/>
            </td>
        </tr>
    </table>
</center>
<ajaxToolkit:AnimationExtender id="AnimateExtender" runat="server" TargetControlID="btnDisplayMenu">
    <Animations>
        <OnClick>
            <Sequence>
                <Parallel duration="0.3" Fps="30">
                    <Resize AnimationTarget="MenuZone" WidthScript="140" />
                </Parallel>
            </Sequence>
        </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>
<ajaxToolkit:AnimationExtender id="AnimationExtender1" runat="server" TargetControlID="btnHideMenu">
    <Animations>
        <OnClick>
            <Sequence>
                <Parallel duration="0.3" Fps="30">
                    <Resize AnimationTarget="MenuZone" WidthScript="0" />
                </Parallel>
            </Sequence>
        </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>