<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageExpenseGroup.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageExpenseGroup"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/ExpenseGroupEditor.ascx" TagName="ExpenseGroupEditor"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ExpenseInfoEditor.ascx" TagName="ExpenseInfoEditor"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 35%">
                <fieldset style="width: 90%" id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlExpenseGroupSearchLabel" runat="server"
                                    Text="$Expense Group$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlExpenseGroupSearch" Width="150" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlDescriptionSearchLabel" runat="server"
                                    Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlDescriptionSearch" Width="200" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlExpenseGroupSearchButton" ToolTip="Search"
                    SkinID="SkSearchButton" OnClick="ExpenseGroupSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlExpenseGroupUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlExpenseGroupGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="table" AllowSorting="true" AllowPaging="true" DataKeyNames="ExpenseGroupID"
                             OnRowCommand="ExpenseGroup_RowCommand" OnRequestCount="RequestCount"
                            OnPageIndexChanged="ExpenseGroupGrid_PageIndexChanged" 
                            SelectedRowStyle-BackColor="#4E9DDF" OnRequestData="RequestData"
                            OnDataBound="ExpenseGroup_DataBound" Width="100%" HorizontalAlign="Left">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Expense Group" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="15%" SortExpression="DbExpenseGroup.ExpenseGroupCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlExpenseGroupLabel" runat="server" Text='<%# Bind("ExpenseGroupCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50%" SortExpression="DbExpenseGroupLang.Description">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"
                                    SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <table border="0">
                                            <tr>
                                                <td align="center" valign="middle">
                                                    <asp:LinkButton ID="ctlExpense" Text="Expense" CommandName="ExpenseGroup" runat="server" />
                                                </td>
                                                <td align="center" valign="middle">
                                                    <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                                        CausesValidation="False" CommandName="ExpenseGroupEdit" />
                                                </td>
                                                <td align="center" valign="middle">
                                                    <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                                        CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                                        CommandName="ExpenseGroupDelete" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctrAdd" ToolTip="Add" SkinID="SkAddButton" OnClick="ctlAdd_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <uc2:ExpenseInfoEditor ID="ctlExpenseInfoEditor" runat="server" />
            <asp:HiddenField ID="expense" runat="server" />
            <uc1:ExpenseGroupEditor ID="ctlExpenseGroupEditor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
