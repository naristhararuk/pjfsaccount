<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="PBManage.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.PBManage"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register src="~/UserControls/PBEditor.ascx" tagname="PBEditor" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 35%">
                <fieldset style="width: 90%" id="pbSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlPBCodeLabel"   runat="server" Text="$PB Code$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlPBCode" Width="200" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctrDescriptionLabel"   runat="server" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctrDescription" Width="200" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                
                <asp:ImageButton runat="server" ID="ctlPBSearch" ToolTip="$Search$" SkinID="SkSearchButton"
                    OnClick="ctlPBSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlPBUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlPBGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="Pbid"
                            SelectedRowStyle-BackColor="#6699FF" OnRowCommand="ctlPB_RowCommand" OnRequestCount="RequestCount"
                            OnDataBound="PB_DataBound"  OnRequestData="RequestData" Width="100%" HorizontalAlign="Left" >
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="PB Code" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width ="10%" SortExpression="DbPB.PBCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPBCodeLabel" runat="server" Text='<%# Bind("PbCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Code" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width ="10%"
                                    SortExpression="DbPB.CompanyCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCompanyCodeLabel" runat="server" Text='<%# Bind("CompanyCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="40%" 
                                    SortExpression="DbPBLang.Description">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                    <asp:HiddenField ID="ctlHidePettyCashLimitLabel" runat="server" Value='<%# Bind("PettyCashLimit") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="PettyCashLimit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width ="10%"  
                                    SortExpression="DbPB.PettyCashLimit">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPettyCashLimitLabel" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" SortExpression="DbPB.Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="PBEdit" />
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="PBDelete" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100%"  />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctrAdd" ToolTip="Add" SkinID="SkAddButton"
                            OnClick="ctlAdd_Click" />
                    </td>
                </tr>
            </table>
    <asp:HiddenField ID="pb" runat="server" />
    <uc1:PBEditor ID="PBEditor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>