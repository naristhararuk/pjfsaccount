<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCostCenter.aspx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageCostCenter" MasterPageFile="~/ProgramsPages.Master"
    EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/CostCenterEditor.ascx" TagName="CostCenterEditor"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <div id="divCondition" align="left" style="width: 100%">
        <fieldset id="ctlFieldSetDetailGridView" style="width: 400px" id="fdsSearch" class="table">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table border="0" class="table" width="400px">
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="ctlLblCostCenterCode" SkinID="SkGeneralLabel" runat="server" Text='$Cost Center$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlTxtCostCenterCode" SkinID="SkGeneralTextBox" Style="text-align: left"
                                        runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="ctlLblCostCenterDescription" SkinID="SkGeneralLabel" runat="server"
                                        Text='$Decription$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlTxtCostCenterDescription" SkinID="SkGeneralTextBox" Style="text-align: left"
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
                   <asp:UpdateProgress ID="updProgressSearch" 
       runat="server" AssociatedUpdatePanelID="ctlUpdPanelGridView" 
       DynamicLayout="true" EnableViewState="False">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    
            <ss:BaseGridView ID="ctlGridView" runat="server" Width="100%" AutoGenerateColumns="false"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="true" DataKeyNames="CostCenterID"
                OnRequestCount="RequestCount" OnRequestData="RequestData" OnRowCommand="ctlGridView_RowCommand"
                CssClass="table" SelectedRowStyle-BackColor="#4E9DDF"  HeaderStyle-CssClass="GridHeader"
                ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>
                    <asp:TemplateField HeaderText="Cost Center" SortExpression="CostCenterCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblCostCenterCode" runat="server" Text='<%# Bind("CostCentercode")%>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblDescription" runat="server" Text='<%# Bind("Description")%>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" SortExpression="cmpn.CompanyCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblCompany" runat="server" Text='<%# Bind("CompanyCode")%>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valid Date" SortExpression="Valid" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblValid" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("Valid")) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expire Date" SortExpression="Expire" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblExpire" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("Expire")) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lock" SortExpression="ActualPrimaryCosts" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActualPrimaryCost" runat="server" Checked='<%# Bind("ActualPrimaryCosts")%>'
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active"   >
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" SortExpression="" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="EditCostCenter" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ctlDeleteCompany" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Delete") %>' OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="DeleteCostCenter" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div align="left">
            <asp:ImageButton runat="server" ID="ImageButtonSave" SkinID="SkAddButton" OnClick="Add_Click" />
            </div>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="CostCenter.Error">
                </spring:ValidationSummary>
            </font>
            <uc1:CostCenterEditor ID="ctlCostCenterEditor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
