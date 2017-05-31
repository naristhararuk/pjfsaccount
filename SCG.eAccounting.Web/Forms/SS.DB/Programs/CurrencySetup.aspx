<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="CurrencySetup.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SS.DB.Programs.CurrencySetup"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/CurrencySetupEditor.ascx" TagName="CurrencySetupEditor"
    TagPrefix="uc1" %>
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
                                <asp:Label ID="ctlSymbolLabel"  runat="server" Text="$Symbol$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlSymbol" SkinID="SkCtlTextboxLeft" Width="150" MaxLength="20" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctrDescriptionLabel" runat="server" MaxLength="100" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctrDescription" Width="200" MaxLength="500" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlCurrencySearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlCurrencySearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlCurrencySetupUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlCurrencySetupGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="CurrencyID"
                            SelectedRowStyle-BackColor="#6699FF" OnRowCommand="ctlCurrency_RowCommand" OnRequestCount="RequestCount"
                            OnRequestData="RequestData" Width="100%" HorizontalAlign="Left">
                           <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
                          
                            <Columns>
                                <asp:TemplateField HeaderText="Symbol" HeaderStyle-HorizontalAlign="Center" SortExpression="DbCurrency.Symbol">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlSymbolLabel"  runat="server" Text='<%# Bind("Symbol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="DbCurrencyLang.Description">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlDescriptionLabel"  runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="55%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lock" HeaderStyle-HorizontalAlign="Center" SortExpression="DbCurrency.Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="CurrencyEdit" />               
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="CurrencyDelete" />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctrAdd" ToolTip="Add" SkinID="SkCtlFormNewRow"
                            OnClick="ctlAdd_Click" />
                    </td>
                </tr>
            </table>
            <uc1:CurrencySetupEditor ID="ctlCurrencyEditor" runat="server" />
            <asp:HiddenField ID="currency" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
