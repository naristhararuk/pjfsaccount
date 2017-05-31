<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterGrid.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.MasterGrid" EnableTheming="true" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <ss:BaseGridView ID="ctlMasterGridView" runat="server" Width="95%" AllowPaging="true"
            InsertRowCount="1" AllowSorting="false" OnDataBound="ctlMasterGridView_DataBound"
            OnRequestData="RequestData" EnableInsert="false" OnRequestCount="RequestCount"
            OnRowCommand="ctlMasterGridView_RowCommand" AutoGenerateColumns="false" ReadOnly="true"
            CssClass="table" DataKeyNames="DivisionId" OnRowEditing="ctlMasterGridView_RowEditing"
            OnRowCancelingEdit="ctlMasterGridView_RowCancelingEdit" OnRowUpdating="ctlMasterGridView_RowUpdating"
            HeaderStyle-HorizontalAlign="Center" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <HeaderTemplate>
                        <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                    </ItemTemplate>
                    <HeaderStyle Width="25px" />
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Code" SortExpression="Code">
                    <EditItemTemplate>
                        <asp:TextBox ID="ctlCode" MaxLength="200" runat="server" Text="" Width="250px"/>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="ctlCode" runat="server" Text="" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="ctlCode" MaxLength="200" runat="server" Width="250px" />
                    </FooterTemplate>
                    <ItemStyle Width="250px" HorizontalAlign="Center" />
                    <FooterStyle Width="250px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                    <EditItemTemplate>
                        <asp:TextBox ID="ctlComment" MaxLength="500" Width="350px" runat="server" Text='<%# Eval("Comment") %>'/>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="ctlComment" MaxLength="500" runat="server" Width="350px" />
                    </FooterTemplate>
                    <ItemStyle Width="350px"/>
                    <FooterStyle Width="350px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Active" SortExpression="Active">
                    <EditItemTemplate>
                        <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="ctlActive" runat="server" />
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                    <HeaderStyle Width="75px" HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Center" Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CommandName="Update"
                            Text="Update"></asp:ImageButton>
                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CommandName="Cancel"
                            Text="Cancel"></asp:ImageButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                            CommandName="Edit" ToolTip="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CommandName="Insert"
                            Text="Insert" ToolTip="Insert"></asp:ImageButton>
                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CommandName="CancelInsert"
                            Text="Cancel"></asp:ImageButton>
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" Width="10%" Wrap="false"/>
                    <ItemStyle Width="10%" HorizontalAlign="Center" Wrap="False" />
                </asp:TemplateField>
            </Columns>
        </ss:BaseGridView>
        <div id="div1" runat="server">
            <table border="0">
                <tr>
                    <td valign="middle">
                        <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click"
                            ToolTip="Add" />
                    </td>
                    <td valign="middle">
                        |
                    </td>
                    <td valign="middle">
                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click"
                            ToolTip="Delete" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
