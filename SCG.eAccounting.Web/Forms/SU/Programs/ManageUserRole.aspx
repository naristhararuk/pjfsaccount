<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUserRole.aspx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SU.Programs.ManageUserRole" MasterPageFile="~/ProgramsPages.Master"
    EnableTheming="true" StylesheetTheme="Default"%>

<%@ Register Src="../../../UserControls/UserRoleEditor.ascx" TagName="UserRoleEditor"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/PBInfo.ascx" TagName="PBInfo" TagPrefix="uc2" %>
<%@ Register Src="../../../UserControls/ServiceTeamInfo.ascx" TagName="ServiceTeamInfo"
    TagPrefix="uc3" %>
<%@ Register Src="../../../UserControls/ProgramInfo.ascx" TagName="ProgramInfo" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <div id="divCondition" align="left" style="width: 100%">
        <fieldset id="ctlFieldSetDetailGridView" style="width: 400px" id="fdsSearch" class="table">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table border="0" class="table" width="400px">
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="ctlLblGroupCode" SkinID="SkGeneralLabel" runat="server" Text='$Group Code$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlTxtGroupCode" SkinID="SkGeneralTextBox" Style="text-align: left"
                                        runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="ctlLblGroupName" SkinID="SkGeneralLabel" runat="server" Text='$Group Name$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlTxtGroupName" SkinID="SkGeneralTextBox" Style="text-align: left"
                                        runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left" valign="top">
                        <asp:ImageButton runat="server" ID="ctlSerchButton" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ss:BaseGridView ID="ctlGridRole" runat="server" Width="100%" AutoGenerateColumns="false"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="true" DataKeyNames="RoleID"
                OnRequestCount="RequestCount" OnRequestData="RequestData" OnRowCommand="ctlGridRole_RowCommand"
                CssClass="table" SelectedRowStyle-BackColor="#4E9DDF" HeaderStyle-CssClass="GridHeader"
                ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>
                    <asp:TemplateField HeaderText="Role Code" SortExpression="RoleCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblRoleCode" runat="server" Text='<%# Bind("RoleCode")%>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblRoleName" runat="server" Text='<%# Bind("RoleName")%>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-BackColor="Transparent" ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton Text="PB" ID="LinkButton1" runat="server" CommandName="PB" />
                            <asp:LinkButton Text="Service Team" ID="LinkButton2" runat="server" CommandName="Service" />
                            <asp:LinkButton Text="Program" ID="LinkButton3" runat="server" CommandName="Program" />
                            <%--<asp:LinkButton Text="Edit" ID="ctlEdit" runat="server" CausesValidation="False"
                                CommandName="RoleEdit" />
                            <asp:LinkButton Text="Delete" ID="ctlDelete" runat="server" CausesValidation="False"
                                CommandName="RoleDelete" OnClientClick="return confirm('Are you sure to delete this row?');" />--%>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" CommandName="RoleEdit"
                                CausesValidation="false" />
                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CommandName="RoleDelete"
                                OnClientClick="return confirm('Are you sure to delete this row?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <%--<div id="div1" align="left" style="width: 100%">--%>
                <asp:ImageButton runat="server" ID="ctlBtnAdd" SkinID="SkAddButton" Text="Add" OnClick="ctlBtnAdd_Click1" />
                <uc1:UserRoleEditor ID="CtlUserRoleEditor1" runat="server" />
                <uc2:PBInfo style="width: 100%" ID="ctlPBInfo1" runat="server" />
                <uc3:ServiceTeamInfo ID="ctlServiceTeamInfo1" style="width: 100%" runat="server" />
                <uc4:ProgramInfo ID="ctlProgramInfo1" runat="server" />
            <%--</div>--%>
            <font color="red">
                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Role.Error" />
            </font>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
