<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="FixedAdvanceOverDueReport.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.FixedAdvanceOverDueReport" EnableTheming="true" StylesheetTheme="Default"%>

<%@ Register Src="~/UserControls/Report/FixedAdvanceOverDueCriteria.ascx" TagName="FixedAdvanceOverDueCriteria"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Report/SendFollowUpEmail.ascx" TagName="SendFollowUpEmail"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Report/EmailLog.ascx" TagName="EmailLog" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelCriteria" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlFixedAdvanceOverdueProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCriteria"
                DynamicLayout="false" EnableViewState="true">
                <ProgressTemplate>
                    <uc4:SCGLoading ID="SCGLoading" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%">
                <tr>
                    <td>
                        <uc1:FixedAdvanceOverDueCriteria ID="ctlFixedAdvanceOverDueCriteria" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkCtlQuery"
                            OnClick="ctlSearch_Click" /><br />
                        <asp:Label ID="showException" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <%--<asp:ImageButton runat="server" ID="ctlPrint" ToolTip="Print" SkinID="SkCtlPrint"/>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="ctlUpdatePanelFixedAdvanceDocument" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="ctlFieldSetDetailGridView" style="width: 99%" class="table">
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelFixedAdvanceDocument"
                    DynamicLayout="true" EnableViewState="False">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <ss:BaseGridView ID="ctlFixedAdvanceReportGrid" runat="server" AutoGenerateColumns="false"
                    CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="DocumentID,RequesterID,FixedAdvanceID,DocumentNo"
                    EnableInsert="False" OnRequestCount="RequestCount" OnRequestData="RequestData"
                    SelectedRowStyle-BackColor="#6699FF" Width="100%" OnRowDataBound="ctlFixedAdvanceReportGrid_RowDataBound"
                    OnRowCommand="ctlFixedAdvanceReportGrid_RowCommand" OnDataBound="ctlFixedAdvanceReportGrid_DataBound">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlEmailButton" runat="server" Text="E-Mail" CommandName="ClickEmail"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlNoText" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DocumentNo" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlDocumentNo" runat="server" Text='<%# Bind("DocumentNo") %>'
                                    CommandName="ClickFixedAdvanceNo"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DocumentDate" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CreDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlCreDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("CreDate")) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EffectiveFromDate" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="EffectiveFromDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlEffectiveFromDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveFromDate")) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EffectiveToDate" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="EffectiveToDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlEffectiveToDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveToDate")) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RequesterName" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="RequesterName">
                            <ItemTemplate>
                                <asp:Label ID="ctlRequesterName" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" SortExpression="Subject">
                            <ItemTemplate>
                                <asp:Label ID="ctlSubject" runat="server" Text='<%# Bind("Subject") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Objective" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="ctlObjective" runat="server" Text='<%# Bind("Objective") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="ctlAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Amount", "{0:#,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NetAmount" HeaderStyle-HorizontalAlign="Center" SortExpression="NetAmount">
                            <ItemTemplate>
                                <asp:Label ID="ctlNetAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NetAmount", "{0:#,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
<%--                        <asp:TemplateField HeaderText="Ref FixedAdvance" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="RefFixedAdvanceNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlRefFixedAdvanceNo" runat="server" Text='<%# Bind("RefFixedAdvanceNo") %>'
                                    CommandName="ClickRefFixedAdvanceNo"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="FixedAdvanceStatus" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="FixedAdvanceStatus">
                            <ItemTemplate>
                                <asp:Label ID="ctlFixedAdvanceStatus" runat="server" Text='<%# Bind("FixedAdvanceStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DueDate" HeaderStyle-HorizontalAlign="Center" SortExpression="DueDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DueDate")) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OverDueDay" HeaderStyle-HorizontalAlign="Center" SortExpression="OverdueDay">
                            <ItemTemplate>
                                <asp:Label ID="ctlOverDueDay" runat="server" Text='<%# Bind("OverDueDay") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SendTime" HeaderStyle-HorizontalAlign="Center" SortExpression="Sendtime">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlSendTime" runat="server" Text='<%# Bind("SendTime") %>' CommandName="ClickSendtime"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%# GetMessage("NoDataFound") %>'
                            runat="server"></asp:Label>
                    </EmptyDataTemplate>
                    <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                </ss:BaseGridView>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:SendFollowUpEmail ID="ctlSendFollowUpEmail" runat="server" EmailType="EM15" />
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <asp:Panel ID="ctlEmailPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Width="850px">
        <div style="overflow: auto; height: 400px">
            <uc3:EmailLog ID="ctlEmailLog" runat="server" />
        </div>
        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClientClick="return false;" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlEmailPanel" BackgroundCssClass="modalBackground" CancelControlID="ctlCancel"
        DropShadow="true" RepositionMode="None" />
</asp:Content>
