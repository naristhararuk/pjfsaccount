<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PBInfo.ascx.cs" EnableTheming="true"
    Inherits="SCG.eAccounting.Web.UserControls.PBInfo" %>
<%@ Register Src="DropdownList/SCG.DB/PB.ascx" TagName="PB" TagPrefix="uc1" %>
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
<asp:Panel ID="ctlPanelPBInfo" runat="server" Visible="false">
<br />
    <asp:Label runat="server" ID="ctlLabelHead"  SkinID="SkGeneralLabel" Text="$PB's Information :$" ></asp:Label>
    <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ss:BaseGridView ID="ctlGridRole" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="false"
                DataKeyNames="RolePbID" OnRequestData="RequestData" CssClass="table" HeaderStyle-CssClass="GridHeader"
                OnDataBound="Grid_DataBound" 
                SelectedRowStyle-BackColor="#6699FF" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PB Code" SortExpression="PBCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblPBCode" runat="server" Text='<%# Bind("PBCode")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PB Name" SortExpression="PBName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblPBName" runat="server" Text='<%# Bind("PBName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" SortExpression="Company" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblPBCompany" runat="server" Text='<%# Bind("Company")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <br />
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr align="left">
                    <td style="width:20%">
                        <asp:Label ID="ctlLabelPB" SkinID="SkCtlLabel" Text="$PB :$ " runat="server"></asp:Label>
                    </td>
                    <td>
                        <uc1:PB ID="ctlDropDownListPB" runat="server" />
                    </td>
                    <td>
                        <asp:ImageButton ID="ctlButtonAdd" SkinID="SkAddButton" Text="$Add$" runat="server" OnClick="Add_Click" />
                        <asp:ImageButton ID="ctlButtonDelete" SkinID="SkDeleteButton" Text="$Delete$" runat="server" OnClick="Delete_Click"  OnClientClick="return confirm('Are you sure to delete this row?');" />
                        <asp:ImageButton ID="ctlButtonClose" OnClick="ctlButtonClose_Click" SkinID="SkCancelButton"
                            Text="$Close$" runat="server" />
                    </td>
                </tr>
            </table>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="SuRolepb.Error">
                </spring:ValidationSummary>
            </font>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
