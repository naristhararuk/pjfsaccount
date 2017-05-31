<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
CodeBehind="MaintainPersonalLevel.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.MaintainPersonalLevel" 
EnableTheming="true" StylesheetTheme="Default" %>
<%@ Register Src="~/UserControls/MaintainPersonalLevelEditor.ascx" TagName="MaintainPersonalLevelEditor"
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
                                <asp:Label ID="ctlPersonalLevelLabel" runat="server" Text="$Personal Level$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 225px">
                                <asp:TextBox ID="ctlPersonalLevel" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="20"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 125px">
                                <asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 225px">
                                <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="100"
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
    <uc1:MaintainPersonalLevelEditor ID="ctlMaintainPersonalLevelEditor" runat="server" ShowScrollBar="true"  />
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlMaintainPersonalLevelGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="PersonalLevel" SelectedRowStyle-BackColor="#6699FF"
                            OnRowCommand="ctlMaintainPersonalLevelGrid_RowCommand" OnRequestCount="RequestCount" OnRequestData="RequestData"
                            Width="100%" HorizontalAlign="Left">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                             <asp:TemplateField HeaderText="PersonalLevel" HeaderStyle-HorizontalAlign="Center" SortExpression="PersonalLevel">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPersonalLevelLabel" runat="server" Text='<%# Bind("PersonalLevel") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" SortExpression="Description">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ordinal" HeaderStyle-HorizontalAlign="Center" SortExpression="Ordinal">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlOrdinalLabel" runat="server" Text='<%# Bind("Ordinal") %>'
                                            SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="MaintainPersonalLevelEdit" />
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="MaintainPersonalLevelDelete" />
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
            <asp:HiddenField ID="personalLevelCode" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
