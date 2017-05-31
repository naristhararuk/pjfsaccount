<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="DocumentFollowupReport.aspx.cs" 
	Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.DocumentFollowupReport" EnableTheming="true" StylesheetTheme="Default" EnableEventValidation="false"   %>
<%@ Register src="~/UserControls/Report/DocumentFollowupCriteria.ascx" tagname="DocumentFollowupCriteria" tagprefix="uc1"%>
<%@ Register src="~/UserControls/Report/SendFollowUpEmail.ascx"tagname="SendFollowUpEmail" tagprefix="uc2"%>
<%@ Register src="~/UserControls/Report/EmailLog.ascx" tagname="EmailLog" tagprefix="uc3" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
<asp:UpdatePanel ID="ctlUpdatePanelCriteria" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlDocumentFollowUpProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCriteria"
             DynamicLayout="false" EnableViewState="true">
             <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading"  runat="server" />
             </ProgressTemplate>
        </asp:UpdateProgress>
        <table width="100%" border=0>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="panelControl">
                        <uc1:DocumentFollowupCriteria ID="ctlDocumentFollowupCriteria" runat="server"  />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" 
                        SkinID="SkCtlQuery" onclick="ctlSearch_Click" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<%--<table width="100%">--%>
<asp:UpdatePanel ID="ctlUpdatePanelDocument" runat="server" UpdateMode="Conditional"> 
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:98%" class="table">
        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelDocument"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <ss:BaseGridView ID="ctlDocumentGrid" runat="server"  AutoGenerateColumns="false" CssClass="Grid" AllowSorting="true"
            AllowPaging="true" DataKeyNames="DocumentID,RequesterID,CreatorID" EnableInsert="False" OnRequestCount="RequestCount" OnRequestData="RequestData"
            SelectedRowStyle-BackColor="#6699FF" Width="100%" OnRowDataBound="ctlDocumentGrid_RowDataBound" OnRowCommand="ctlDocumentGrid_RowCommand">
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <RowStyle CssClass="GridItem" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlEmailButton" runat="server" Text="E-Mail" CommandName="ClickEmail"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="4%"/>
                </asp:TemplateField>
                <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:Label ID="ctlNoText" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocumentDate" HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentDate">
                    <ItemTemplate>
                        <asp:Label ID="ctlDocumentDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" SortExpression="Status">
                    <ItemTemplate>
                        <asp:Label ID="ctlDocumentStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RequestNo" HeaderStyle-HorizontalAlign="Center" SortExpression="RequestNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlDocumentNo" runat="server" Text='<%# Bind("RequestNo") %>' CommandName="ClickAdvanceNo"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" Width="115" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CreatorName" HeaderStyle-HorizontalAlign="Center" SortExpression="CreatorName">
                    <ItemTemplate>
                        <asp:Label ID="ctlCreator" runat="server" Text='<%# Bind("CreatorName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RequesterName" HeaderStyle-HorizontalAlign="Center" SortExpression="RequesterName">
                    <ItemTemplate>
                        <asp:Label ID="ctlRequester" runat="server" Text='<%# Bind("RequesterName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TelNo" HeaderStyle-HorizontalAlign="Center" SortExpression="TelNo">
                    <ItemTemplate>
                        <asp:Label ID="ctlTelNo" runat="server" Text='<%# Bind("TelNo") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="110"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Center" SortExpression="Email">
                    <ItemTemplate>
                        <asp:Label ID="ctlEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="150"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" SortExpression="Description">
                    <ItemTemplate>
                        <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount">
                    <ItemTemplate>
                        <asp:Label ID="ctlAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Amount", "{0:#,##0.00}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days" HeaderStyle-HorizontalAlign="Center" SortExpression="CountDays">
                    <ItemTemplate>
                        <asp:Label ID="ctlCountDayApprove" runat="server" Text='<%# Bind("CountDays") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SendTime" HeaderStyle-HorizontalAlign="Center" SortExpression="SendTime">
                    <ItemTemplate>
                        <asp:LinkButton ID="ctlSendTime" runat="server" Text='<%# Bind("SendTime") %>' CommandName="ClickSendtime"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
		        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>' runat="server"></asp:Label>
	        </EmptyDataTemplate>
	        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
        </ss:BaseGridView>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
    <uc2:SendFollowUpEmail ID="ctlSendFollowUpEmail1" runat="server" EmailType="EM09"/>
<%--</table>--%>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<asp:Panel ID="ctlEmailPanel" runat="server" Style="display: none" CssClass="modalPopup" Width="900px">
    <div style="overflow:auto;height:400px">
        <uc3:EmailLog ID="ctlEmailLog" runat="server" />
    </div>
    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClientClick="return false;" />
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtender" runat="server" TargetControlID="lnkDummy"
        PopupControlID="ctlEmailPanel" BackgroundCssClass="modalBackground" CancelControlID="ctlCancel"
        DropShadow="true" RepositionMode="None" />
</asp:Content>