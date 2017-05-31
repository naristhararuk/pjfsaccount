<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramInfo.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ProgramInfo" %>
<%@ Register Src="LOV/SS.DB/RoleSearch.ascx" TagName="RoleSearch" TagPrefix="uc1" %>
<%@ Register Src="LOV/SS.DB/ProgramSearch.ascx" TagName="ProgramSearch" TagPrefix="uc2" %>

<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>">
</script>

<asp:Panel ID="ctlPanelProgramInfo" runat="server" Visible="false">
    <br />
    <asp:Label runat="server" ID="ctlLabelHead" SkinID="SkGeneralLabel" Text="$Program's Information :$"></asp:Label>
    <asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ss:BaseGridView ID="ctlProgramGridView" runat="server" AllowPaging="false" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="table" DataKeyNames="ProgramRoleID" EnableInsert="False"
                HeaderStyle-CssClass="GridHeader" OnDataBound="Grid_DataBound" OnRequestData="RequestData"
                ReadOnly="true" SelectedRowStyle-BackColor="#6699FF" ShowHeaderWhenEmpty="true"
                Width="100%">
                <alternatingrowstyle cssclass="GridItem" />
                <rowstyle cssclass="GridAltItem" />
                <columns>
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
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Program Code"
                        SortExpression="ProgramCode">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblProgramCode" runat="server" Text='<%# Bind("ProgramCode")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Program Name"
                        SortExpression="ProgramName">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblPBName" runat="server" Text='<%# Bind("ProgramsName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                </columns>
                <emptydatatemplate>
                    <asp:Label ID="lblNodata" runat="server" SkinID="SkNodataLabel"></asp:Label>
                </emptydatatemplate>
                <emptydatarowstyle horizontalalign="Center" width="100%" />
            </ss:BaseGridView>
            <br />
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr align="left">
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
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="ProgramRole.Error">
                </spring:ValidationSummary>
            </font>
            <uc2:ProgramSearch ID="ctlProgramSearch1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
