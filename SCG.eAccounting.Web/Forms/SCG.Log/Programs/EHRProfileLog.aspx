<%@ Page 
Language="C#" 
MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true"
    CodeBehind="EHRProfileLog.aspx.cs" 
    EnableTheming="true"
StylesheetTheme="Default"
meta:resourcekey="PageResource1"
    Inherits="SCG.eAccounting.Web.Forms.SCG.Log.Programs.EHRProfileLog" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="A">
        <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ss:BaseGridView ID="ctlEHRProfileLogGrid"
                 runat="server" 
                 AutoGenerateColumns="False"
                    Width="100%" 
                    OnRequestData="RequestData" 
                    OnRequestCount="RequestCount" 
                    AllowPaging="True"
                    AllowSorting="True"
                    CssClass="table"
                    HeaderStyle-CssClass="GridHeader"
                    SelectedRowStyle-BackColor="#6699FF"
                    ReadOnly="true"
                    DataKeyNames="EHrProfileLogID"
                    ShowHeaderWhenEmpty ="true">
                   
                    <Columns>
                        <asp:TemplateField HeaderText="People ID" SortExpression="peopleID" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblPeopleID" runat="server" Text='<%# Bind("PeopleID")%>'></asp:Label>
                            
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User ID" SortExpression="userName" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblUserName" runat="server" Text='<%# Bind("UserName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Message" SortExpression="message" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblMessage" runat="server" Text='<%# Bind("Message")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                     <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server" ></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                
                </ss:BaseGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>