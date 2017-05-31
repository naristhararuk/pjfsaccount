<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="History.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.History" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:UpdatePanel ID="UpdatePanelHistory" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdatePanelHistoryProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelHistory"
            DynamicLayout="true" EnableViewState="true">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <ss:BaseGridView ID="ctlHistoryGridView" runat="server" OnRowCommand="ctlHistoryGridView_RowCommand"
            OnRowDataBound="ctlHistoryGridView_RowDataBound" AutoGenerateColumns="False"
            AllowPaging="true" OnDataBound="ctlHistoryGridView_DataBound"
            DataKeyNames="" OnRequestData="RequestData" 
            OnRequestCount="RequestCount" EnableInsert="False"
            ShowMsgDataNotFound = "false"
            InsertRowCount="1" SaveButtonID="" Width="100%" CssClass="Grid">
            <HeaderStyle CssClass="GridHeader" />
            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:HiddenField ID="ctlWorkFlowStateEventID" runat="server" Value='<%# Eval("WorkFlowStateEventID") %>' />
                        <asp:Label ID="ctlDateLabel" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("ResponseDate")) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                    <ItemTemplate>
                        <asp:Label ID="ctlNameLabel" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Response">
                    <ItemTemplate>
                        <asp:Label ID="ctlResponseLabel" runat="server" Text='<%# Bind("Response") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Response Method">
                    <ItemTemplate>
                        <asp:Label ID="ctlResponseMethodLabel" runat="server" Text='<%# Bind("ResponseMethod") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                </asp:TemplateField>
            </Columns>
            
            <EmptyDataTemplate>
            <asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server" Text=' '></asp:Label>
            </EmptyDataTemplate>
        </ss:BaseGridView>
    </ContentTemplate>
</asp:UpdatePanel>
