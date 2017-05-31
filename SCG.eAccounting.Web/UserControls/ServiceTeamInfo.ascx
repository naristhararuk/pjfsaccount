<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceTeamInfo.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DropdownList.ServiceTeamInfo" %>

<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

<%@ Register Src="DropdownList/SCG.DB/ServiceTeam.ascx" TagName="ServiceTeam" TagPrefix="uc2" %>
<asp:Panel ID="ctlPanelPBInfo" runat="server" Visible="false">
    <br />
    <asp:Label runat="server" ID="ctlLabelHead" SkinID="SkGeneralLabel" Text="$Service's Information :$"></asp:Label>
    <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ss:BaseGridView ID="ctlService" runat="server" AllowPaging="false" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="table" DataKeyNames="RoleServiceID" EnableInsert="False"
                HeaderStyle-CssClass="GridHeader" OnDataBound="Grid_DataBound" OnRequestData="RequestData"
                ReadOnly="true" SelectedRowStyle-BackColor="#6699FF" ShowHeaderWhenEmpty="true"
                Width="100%">
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
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Service Code"
                        SortExpression="ServiceCode">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblServiceCode" runat="server" Text='<%# Bind("ServiceCode")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Service Name"
                        SortExpression="ServiceName">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblPBName" runat="server" Text='<%# Bind("ServiceName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" runat="server" SkinID="SkCtlLabelNodata"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <br />
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr align="left">
                    <td style="width: 20%">
                        <asp:Label ID="ctlLabelServiceName" SkinID="SkCtlLabel" Text="$Service Name :$ "
                            runat="server"></asp:Label>
                    </td>
                    <td>
                        <uc2:ServiceTeam ID="ctlServiceTeam1" runat="server" />
                    </td>
                    <td>
                        <asp:ImageButton ID="ctlButtonAdd" SkinID="SkAddButton" Text="$Add$" runat="server"
                            OnClick="Add_Click" />
                        <asp:ImageButton ID="ctlButtonDelete" SkinID="SkDeleteButton" Text="$Delete$" runat="server"
                            OnClick="Delete_Click" OnClientClick="return confirm('Are you sure to delete this row?');" />
                        <asp:ImageButton ID="ctlButtonClose" OnClick="ctlButtonClose_Click" SkinID="SkCancelButton"
                            Text="$Close$" runat="server" />
                    </td>
                </tr>
            </table>
            <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="RoleService.Error">
                </spring:ValidationSummary>
            </font>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
