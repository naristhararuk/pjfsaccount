<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageIO.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageIO"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register src="~/UserControls/IOEditor.ascx" tagname="IOEditor" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 35%">
                <fieldset style="width: 90%" id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" >
                                <asp:Label ID="ctlIONumberLabelSearch" runat="server" Text="$IONumber$"  ></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="ctlIONumber" SkinID="SkCtlTextboxLeft"  Width="100" MaxLength="20" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlIOTextLabelSearch" runat="server" Text="$IOText$" ></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlIOText" SkinID="SkCtlTextboxLeft" Width="200"  MaxLength="100" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlIOSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlIOSearch_Click" />
            </td>
        </tr>
    </table>
    <uc1:IOEditor ID="ctlIOEditor" runat="server" />
    <asp:UpdatePanel ID="ctlIOUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlIOGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="IOID" SelectedRowStyle-BackColor="#6699FF"
                            OnRowCommand="ctlCurrency_RowCommand" OnRequestCount="RequestCount" OnRequestData="RequestData"
                            Width="100%" HorizontalAlign="Left">
                                <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="IONumber" HeaderStyle-HorizontalAlign="Center" SortExpression="IONumber">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlIONumberLabel" runat="server" Text='<%# Bind("IONumber") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IOType" HeaderStyle-HorizontalAlign="Center" SortExpression="IOType">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlIOTypeLabel" runat="server" Text='<%# Bind("IOType") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IOText" HeaderStyle-HorizontalAlign="Center" SortExpression="IOText">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlIOTextLabel" runat="server" Text='<%# Bind("IOText") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CompanyCode" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="CompanyCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCompanyCodeLabel" runat="server" Text='<%# Bind("CompanyCode") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CostCenterCode" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="CostCenterCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCostCenterLabel" runat="server" Text='<%# Bind("CostCenterCode") %>' SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ValidDate" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="EffectiveDate">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlValidDateLabel" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("EffectiveDate")) %>' SkinID="SkGeneralLabel" ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ExpireDate" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="ExpireDate">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlExpireDateLabel" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ExpireDate")) %>' SkinID="SkGeneralLabel" ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="db.Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="IOEdit" />
                                    
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="IODelete" />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" HorizontalAlign="Center" />
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
          <asp:HiddenField ID="internalOrderId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
