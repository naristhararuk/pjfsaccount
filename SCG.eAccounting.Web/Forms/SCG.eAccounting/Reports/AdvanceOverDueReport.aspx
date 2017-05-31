<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
    CodeBehind="AdvanceOverDueReport.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.AdvanceOverDueReport" 
    EnableTheming="true" StylesheetTheme="Default" %>  
<%@ Register src="~/UserControls/Report/AdvanceOverDueCriteria.ascx" tagname="AdvanceOverDueCriteria" tagprefix="uc1"%>
<%@ Register src="~/UserControls/Report/SendFollowUpEmail.ascx"tagname="SendFollowUpEmail" tagprefix="uc2"%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register src="~/UserControls/Report/EmailLog.ascx" tagname="EmailLog" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<asp:UpdatePanel ID="ctlUpdatePanelCriteria" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlAdvanceOverdueProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCriteria"
             DynamicLayout="false" EnableViewState="true">
             <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading"  runat="server" />
             </ProgressTemplate>
        </asp:UpdateProgress>
        <table width="100%">
            <tr>
                <td>
                    <uc1:AdvanceOverDueCriteria ID="ctlAdvanceOverDueCriteria" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" 
                        SkinID="SkCtlQuery" onclick="ctlSearch_Click"/><br />
                    <asp:Label ID="showException" runat="server" Text="" ForeColor="Red"></asp:Label>
                     <%--<asp:ImageButton runat="server" ID="ctlPrint" ToolTip="Print" SkinID="SkCtlPrint"/>--%>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlUpdatePanelAdvanceDocument" runat="server" UpdateMode="Conditional"> 
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:99%" class="table">
        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAdvanceDocument"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <ss:BaseGridView ID="ctlAdvanceReportGrid" runat="server"  AutoGenerateColumns="false" CssClass="Grid" AllowSorting="true"
            AllowPaging="true" DataKeyNames="DocumentID,RequesterID,AdvanceID,ExpenseID,DocumentNo" EnableInsert="False" OnRequestCount="RequestCount" OnRequestData="RequestData"
            SelectedRowStyle-BackColor="#6699FF" Width="100%" OnRowDataBound="ctlAdvanceReportGrid_RowDataBound" OnRowCommand="ctlAdvanceReportGrid_RowCommand" OnDataBound="ctlAdvanceReportGrid_DataBound">
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <RowStyle CssClass="GridItem" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlEmailButton" runat="server" Text="E-Mail" CommandName="ClickEmail"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="5%"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:Label ID="ctlNoText" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocumentNo" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlDocumentNo" runat="server" Text='<%# Bind("DocumentNo") %>' CommandName="ClickAdvanceNo"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocumentDate" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentDate">
                    <ItemTemplate>
                        <asp:Label ID="ctlDocumentDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RequesterName" HeaderStyle-HorizontalAlign="Center" SortExpression="RequesterName">
                    <ItemTemplate>
                        <asp:Label ID="ctlRequesterName" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" SortExpression="Description">
                    <ItemTemplate>
                        <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AdvanceAmt" HeaderStyle-HorizontalAlign="Center" SortExpression="AdvanceAmt">
                    <ItemTemplate>
                        <asp:Label ID="ctlAdvanceAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AdvanceAmt", "{0:#,##0.00}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ExpenseAmt" HeaderStyle-HorizontalAlign="Center" SortExpression="ExpenseAmt">
                    <ItemTemplate>
                        <asp:Label ID="ctlExpenseAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ExpenseAmt", "{0:#,##0.00}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RemittanceAmt" HeaderStyle-HorizontalAlign="Center" SortExpression="RemittanceAmt">
                    <ItemTemplate>
                        <asp:Label ID="ctlRemittanceAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RemittanceAmt", "{0:#,##0.00}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OutstandingAmt" HeaderStyle-HorizontalAlign="Center" SortExpression="OutstandingAmt">
                    <ItemTemplate>
                        <asp:Label ID="ctlOutstandingAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OutstandingAmt", "{0:#,##0.00}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ExpenseNo" HeaderStyle-HorizontalAlign="Center" SortExpression="ExpenseNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlExpenseNo" runat="server" Text='<%# Bind("ExpenseNo") %>' CommandName="ClickExpenseNo"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ExpenseStatus" HeaderStyle-HorizontalAlign="Center" SortExpression="ExpenseStatus">
                    <ItemTemplate>
                        <asp:Label ID="ctlExpenseStatus" runat="server" Text='<%# Bind("ExpenseStatus") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DueDate" HeaderStyle-HorizontalAlign="Center" SortExpression="DueDate">
                    <ItemTemplate>
                        <asp:Label ID="ctlDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DueDate")) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OverDueDay" HeaderStyle-HorizontalAlign="Center" SortExpression="OverdueDays">
                    <ItemTemplate>
                        <asp:Label ID="ctlRequestDateOfRemittance" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDateofRemittance")) %>' style="display:none"></asp:Label>
                        <asp:Label ID="ctlOverDueDay" runat="server" Text='<%# Bind("OverdueDays") %>'></asp:Label>
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
		        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%# GetMessage("NoDataFound") %>' runat="server"></asp:Label>
	        </EmptyDataTemplate>
	        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
        </ss:BaseGridView>
    </fieldset>
    </ContentTemplate> 
</asp:UpdatePanel> 
<uc2:SendFollowUpEmail ID="ctlSendFollowUpEmail" runat="server" EmailType="EM10" />
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<asp:Panel ID="ctlEmailPanel" runat="server" Style="display: none" CssClass="modalPopup" Width="850px">
    <div style="overflow:auto;height:400px">
        <uc3:EmailLog ID="ctlEmailLog" runat="server" />
    </div>
    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClientClick="return false;" />
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlEmailPanel" BackgroundCssClass="modalBackground" CancelControlID="ctlCancel"
        DropShadow="true" RepositionMode="None" />
</asp:Content>