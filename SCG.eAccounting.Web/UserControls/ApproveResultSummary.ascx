<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApproveResultSummary.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ApproveResultSummary" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<asp:UpdatePanel ID="ctlPopUpUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:UpdateProgress ID="ctlPopUpUpdatePanelProgress" runat="server" AssociatedUpdatePanelID="ctlPopUpUpdatePanel"
             DynamicLayout="false" EnableViewState="true">
             <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading"  runat="server" />
             </ProgressTemplate>
        </asp:UpdateProgress>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Panel ID="ctlApproveResultSummaryFormHeader" runat="server" Style="cursor: move;
                    background-color: #DDDDDD; border: solid 1px Gray; color: Black" >
                    <p>
                        <asp:Label ID="ctlApproveResultSummaryHeader" runat="server" SkinID="SkFieldCaptionLabel"
                            Text="ApproveResultSummary"></asp:Label>
                    </p>
                </asp:Panel>
                <br />
                <ss:BaseGridView ID="ctlApproveSummaryGrid" DataKeyNames="DocumentID" runat="server"
                    AutoGenerateColumns="False" ReadOnly="true" EnableInsert="False" CssClass="Grid"
                    Width="100%" InsertRowCount="1" AllowPaging="true" AllowSorting="true" OnRequestData="ctlApproveSummaryGrid_RequestData" OnRequestCount="ctlApproveSummaryGrid_RequestCount" >
                    <HeaderStyle CssClass="GridHeader"/>
                    <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="Request No." HeaderStyle-HorizontalAlign="Center"
                            SortExpression="DocumentNo">
                            <ItemTemplate>
                                <asp:Literal ID="ctlRequestNo" runat="server" SkinID="SkCodeLabel" Text='<%# Bind("DocumentNo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="115"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" SortExpression="Subject"
                            HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlSubject" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# Bind("Subject") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount"
                            HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkNumberLabel" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" SortExpression="Status"
                            HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlStatus" Mode="Encode" runat="server" SkinID="SkCodeLabel"
                                    Text='<%# Bind("Status") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason" HeaderStyle-HorizontalAlign="Center" SortExpression="Reason">
                            <ItemTemplate>
                               <%-- <asp:Literal ID="ctlReason" Mode="Encode" runat="server" SkinID="SkCodeLabel" Text='<%# Bind("Reason")%>'></asp:Literal>--%>
                               <%--<spring:ValidationSummary ID="ctlDocumentValidationSummary" runat="server" Provider='<%# Bind("DocumentNo") %>'/>--%>
                               <asp:Repeater ID="ctlRepeaterReason" runat="server" DataSource='<%# Eval("Reason") %>'>
                                <ItemTemplate>
                                    <asp:Literal ID="ctlReason" Mode="Encode" SkinID="SkGeneralLabel" runat="server" Text='<%# Container.DataItem %>' />
                                    <br />
                                </ItemTemplate>
                               </asp:Repeater> 
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="ctlClose" runat="server" OnClick="ctlClose_Click" Text="Close" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
</asp:UpdatePanel>

