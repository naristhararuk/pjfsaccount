<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.SearchResult" %>

<asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <center>
        <ss:BaseGridView ID="ctlInboxGrid" runat="server" AutoGenerateColumns="False"
            ReadOnly="true" EnableInsert="False"
            CssClass="Grid" Width="100%" InsertRowCount="1" SaveButtonID="">
            <HeaderStyle CssClass="GridHeader"/> 
            <RowStyle CssClass="GridItem" HorizontalAlign="left"/>   
            <AlternatingRowStyle CssClass="GridAltItem" /> 
            <Columns>
                <asp:BoundField DataField="Seq" HeaderText="Seq" 
                    ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:HyperLinkField DataTextField="DocumentNo" HeaderText="Document No" 
                    NavigateUrl="~/Forms/SCG.eAccounting/Programs/DocumentView.aspx?WFID=1" />
                <asp:BoundField DataField="DocumentType" HeaderText="Document Type"/>
                <asp:BoundField DataField="Creator" HeaderText="Creator"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount" 
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,0.00}" 
                    HtmlEncode="False">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="DocumentStatus" HeaderText="Document Status" 
                    ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
				<asp:Literal ID="lblNodata" Mode="Encode" SkinID="SkCtlLabelNodata" runat="server" Text="No DataFound"></asp:Literal>
			</EmptyDataTemplate>
			<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
        </ss:BaseGridView>
        </center>
    </ContentTemplate>
</asp:UpdatePanel>