<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserGroupEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.UserGroupEditor" %>
<%@ Register Src="LOV/SCG.DB/UserGroupLookup.ascx" TagName="UserGroupLookup" TagPrefix="uc1" %>
<asp:Panel ID="ctlUserGroupEditor" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlUpdatePanelGroup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 80%" id="ctlFieldsetGroup" runat="server" visible="False"
                enableviewstate="True">
                <legend id="ctlLegendGroup" runat="server" style="color: #4E9DDF" visible="True">
                    <asp:Label ID="ctlLabelGroup" runat="server" Text="$Display Group Setting$" SkinID="SkGeneralLabel" /></legend>
                <ss:BaseGridView ID="ctlGroupGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                    CssClass="Grid" Width="100%" OnDataBound="ctlGroupGrid_Databound" AllowSorting="false"
                    OnRequestData="RequestDataGroup">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Code" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlGroupCode" runat="server" Text='<%# Eval("Role.RoleCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Name" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="ctlGroup" runat="server" Text='<%# Eval("Role.RoleName") %>' SkinID="SkGeneralLabel"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <asp:ImageButton runat="server" ID="ctlAddGroup" ToolTip="Add" SkinID="SkCtlFormNewRow"
                    OnClick="ctlAddGroup_Click" />
                <asp:ImageButton runat="server" ID="ctlDeleteGroup" ToolTip="Delete" SkinID="SkCtlGridDelete"
                    CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                    OnClick="ctlDeleteGroup_Click" />
                <asp:ImageButton runat="server" ID="ctlCloseGroup" ToolTip="Close" SkinID="SkCtlFormCancel"
                    OnClick="ctlCloseGroup_Click" />
                <asp:HiddenField ID="user" runat="server" />
            </fieldset>
            <uc1:UserGroupLookup ID="ctlGroupLookup" runat="server" IsMultiple="true"/>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="User.Error">
                </spring:ValidationSummary>
            </font>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
