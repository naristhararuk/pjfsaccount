<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InboxEmployeeSearchResult.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.InboxEmployeeSearchResult"
    EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc1" %>

<%@ Register src="../ApproveResultSummary.ascx" tagname="ApproveResultSummary" tagprefix="uc2" %>

<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
<asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdateProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc1:scgloading id="SCGLoading" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <fieldset id="ctlFieldSearch" runat="server" class="table">
            <legend id="ctlLegendSearchCriteria" style="color: #4E9DDF">
                <asp:Label ID="ctlSearchCriteriaHeader" runat="server" Text='Data'></asp:Label>
            </legend>
            <center>
                <ss:basegridview id="ctlInboxEmployeeGrid" datakeynames="DocumentID,WorkFlowStateID,WorkflowID"
                    runat="server" autogeneratecolumns="False" readonly="true" enableinsert="False"
                    cssclass="Grid" width="100%" insertrowcount="1" allowpaging="true" allowsorting="true"
                    showmsgdatanotfound='<%# isShowMsg %>' ondatabound="ctlInboxEmployeeGrid_DataBound"
                    onrequestcount="ctlInboxEmployeeGrid_RequestCount" onrequestdata="ctlInboxEmployeeGrid_RequestData"
                    onrowdatabound="ctlInboxEmployeeGrid_RowDataBound" onrowcommand="ctlInboxEmployeeGrid_RowCommand">
                    <HeaderStyle CssClass="GridHeader"/> 
                    <RowStyle CssClass="GridItem" HorizontalAlign="left"/>   
                    <AlternatingRowStyle CssClass="GridAltItem" /> 
                    <Columns>
                       
                        <asp:TemplateField HeaderText="Attachment" HeaderStyle-HorizontalAlign="Center" SortExpression ="Attachment" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:Image ID="ctlAttach" runat="server" SkinID="SkAttachButton" Visible="false"/>
                                <asp:Image ID="ctlFile" runat="server" SkinID="SkFileButton" Visible="false"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="3%"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                  
                        <asp:TemplateField HeaderText="Request No." HeaderStyle-HorizontalAlign="Center" SortExpression="RequestNo" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlRequestNo" runat="server" SkinID="SkCtlLinkButton" Text='<%# Bind("RequestNo") %>' CommandName="PopupDocument"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Document Type" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentTypeName" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlDocumentType" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Bind("DocumentTypeName") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Create Date" HeaderStyle-HorizontalAlign="Center" SortExpression="CreateDate" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlCreateDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("CreateDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center" SortExpression="ReferenceNo" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlReferenceNo" Mode="Encode" runat="server" SkinID="SkCodeLabel" Text='<%# Bind("ReferenceNo")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Request Date" HeaderStyle-HorizontalAlign="Center" SortExpression="RequestDate" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlRequestDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approve Date" HeaderStyle-HorizontalAlign="Center" SortExpression="ApproveDate" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlApproveDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ApproveDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentStatus" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlStatus" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Bind("DocumentStatus")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" SortExpression="Subject" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlSubject" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Bind("Subject")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="Creator" HeaderStyle-HorizontalAlign="Center" SortExpression="CreatorName" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlCreatorName" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Bind("CreatorName")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="Requester" HeaderStyle-HorizontalAlign="Center" SortExpression="RequesterName" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlRequestName" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# Bind("RequesterName")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="AmountLocalCurrency" HeaderStyle-HorizontalAlign="Center" SortExpression="AmountLocalCurrency" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmountLocalCurrency" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# DataBinder.Eval(Container.DataItem, "AmountLocalCurrency", "{0:#,##0.00;(#,##0.00);}") %>' ></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                       <asp:TemplateField HeaderText="AmountMainCurrency" HeaderStyle-HorizontalAlign="Center" SortExpression="AmountMainCurrency" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmountMainCurrency" Mode="Encode" runat="server" SkinID="SkGeneralLabel" Text='<%# DataBinder.Eval(Container.DataItem, "AmountMainCurrency", "{0:#,##0.00;(#,##0.00);}") %>' ></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00;(#,##0.00);}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Receive Date" HeaderStyle-HorizontalAlign="Center" SortExpression="ReceiveDocumentDate" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlReceive" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ReceiveDocumentDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <%--<EmptyDataTemplate>
				        <asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkCtlLabelNodata" runat="server" Text="No DataFound"></asp:Literal>
			        </EmptyDataTemplate>
			        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />--%>
                </ss:basegridview>
                <br />
                <asp:Button runat="server" SkinID="SkGeneralButton" ID="ctlApprove" Text="Approve"
                    OnClick="Approve_Click" />
            </center>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="ctlApproveResultSummary" runat="server" Style="display: none" CssClass="modalPopup" Width="850">
    <div style="overflow:auto;height:400px">
        <uc2:ApproveResultSummary ID="ctlApproveStatusSummary" runat="server" />
    </div>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlApproveResultSummaryModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlApproveResultSummary" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" />