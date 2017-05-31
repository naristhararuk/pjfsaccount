<%@ Page Language="C#" Title="" MasterPageFile="~/PopupMasterPage.Master" 
AutoEventWireup="true" CodeBehind="UserGroupLookUp.aspx.cs" EnableTheming="true" StylesheetTheme="Default" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserGroupLookUp" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <div align="center">

        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

        <asp:Panel ID="pnUserGroupSearch" runat="server" Width="600px" BackColor="White">
            <asp:Panel ID="pnUserGroupSearchHeader" CssClass="table" runat="server" Style="cursor: move;
                border: solid 1px Gray; color: Black">
                <div>
                    <p>
                        <asp:Label ID="lblTitle" runat="server" Text="$User Group$" Width="210px"></asp:Label></p>
                </div>
            </asp:Panel>
            <asp:UpdatePanel ID="UpdatePanelSearchUserGroup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="Fieldset1" style="width: 70%" id="fdsSearch" class="table">
                        <legend id="Legend1" style="color: #4E9DDF" class="table">
                            <asp:Label ID="Label2" runat="server" Text='Search Box'></asp:Label></legend>
                        <table width="100%" border="0" class="table">
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlUserGroupCodeLabel" runat="server" Text="$User Group Code$"></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlUserGroupCode" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlIsMultiple" runat="server" Text="false" Style="display: none;"></asp:Label>
                                    <asp:Label ID="ctlUserGroupNameLabel" runat="server" Text="$User Group Name$"></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlUserGroupName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkCtlQuery"
                                        OnClick="ctlSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <center>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                            DynamicLayout="true" EnableViewState="true">
                            <ProgressTemplate>
                                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <ss:BaseGridView ID="ctlUserGroupGrid" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" AllowSorting="true" ReadOnly="true" EnableInsert="false" EnableViewState="true"
                            OnRowCommand="ctlUserGroupGrid_RowCommand" DataKeyNames="RoleID" CssClass="Grid"
                            Width="99%" OnDataBound="ctlUserGroupGrid_DataBound" OnRequestCount="RequestCount"
                            OnRequestData="RequestData">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ctlSelectUserGroup" runat="server" SkinID="SkCtlGridSelect"
                                            CausesValidation="False" CommandName="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Group Code" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="RoleCode">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlUserGroupCode" runat="server" Text='<%# Eval("RoleCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Group Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="RoleName">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlUserGroupName" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='No DataFound'></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>
                        <div style="text-align: left; width: 98%">
                            <table border="0">
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click"
                                            Text="Submit" Visible="false" />&nbsp;
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" ToolTip="Cancel"
                                            OnClick="ctlCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
