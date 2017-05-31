<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageSapInstance.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageSapInstance"
    EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register Src="~/UserControls/SapInstanceEditor.ascx" TagName="SapInstanceEditor"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 20%">
                <fieldset style="width: 350px" id="fdsSearch" class="table">
                    <table width="350px" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 125px">
                                <asp:Label ID="ctlAliasNameLabel" runat="server" Text="$Alias Name$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 225px">
                                <asp:TextBox ID="ctlAliasName" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="20"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 125px">
                                <asp:Label ID="ctlMsgHostServerLabel" runat="server" Text="$Msg Host Server$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 225px">
                                <asp:TextBox ID="ctlMsgServerHost" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="100"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlSearch_Click" />
            </td>
        </tr>
    </table>
    <uc1:SapInstanceEditor ID="ctlSapInstanceEditor" runat="server" ShowScrollBar="true"  />
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlSapGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="Code" SelectedRowStyle-BackColor="#6699FF"
                            OnRowCommand="ctlSapGrid_RowCommand" OnRequestCount="RequestCount" OnRequestData="RequestData"
                            Width="100%" HorizontalAlign="Left">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                             <asp:TemplateField HeaderText="Code" HeaderStyle-HorizontalAlign="Center" SortExpression="Code">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCodeLabel" runat="server" Text='<%# Bind("Code") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alias Name" HeaderStyle-HorizontalAlign="Center" SortExpression="AliasName">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlAliasNameLabel" runat="server" Text='<%# Bind("AliasName") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Msg Host Server" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="MsgHostServer">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlMsgHostServerLabel" runat="server" Text='<%# Bind("MsgServerHost") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Logon Group" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="LogonGroup">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlLogonGroupLabel" runat="server" Text='<%# Bind("LogonGroup") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client" HeaderStyle-HorizontalAlign="Center" SortExpression="Client">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlClientLabel" runat="server" Text='<%# Bind("Client") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center" SortExpression="Language">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlLanguageLabel" runat="server" Text='<%# Bind("Language") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="SapEdit" />
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="SapDelete" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkAddButton" OnClick="ctlAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="sapInstanceCode" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>