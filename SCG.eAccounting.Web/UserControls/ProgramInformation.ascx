<%@ Control Language="C#" 
AutoEventWireup="true" 
CodeBehind="ProgramInformation.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.ProgramInformation" %>



<%@ Register src="LOV/SS.DB/RoleSearch.ascx" tagname="RoleSearch" tagprefix="uc1" %>



<%@ Register src="LOV/SS.DB/ProgramSearch.ascx" tagname="ProgramSearch" tagprefix="uc2" %>

<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>">
</script>

<asp:Panel ID="ctlPanelProgramInfo" runat="server" Visible="false">
<br />
    <asp:Label runat="server" ID="ctlLabelHead" SkinID="SkCtlLabel" Text="$Program's Information :$" ></asp:Label>
<asp:UpdatePanel ID="ctlUpdPanelGridView" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<ss:BaseGridView ID="ctlProgramGridView" 
    runat="server" 
    AllowPaging="false" 
    AllowSorting="true" 
    AutoGenerateColumns="False" 
    CssClass="table" 
    DataKeyNames="ProgramRoleID" 
    EnableInsert="False" 
    HeaderStyle-CssClass="GridHeader" 
    OnDataBound="Grid_DataBound"
     OnRequestData="RequestData" 
     ReadOnly="true" 
    SelectedRowStyle-BackColor="#6699FF" ShowHeaderWhenEmpty="true" Width="100%">
    <AlternatingRowStyle CssClass="GridItem" />
    <RowStyle CssClass="GridAltItem" />
    <Columns>
        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
            <HeaderTemplate>
                <asp:CheckBox ID="ctlHeader" runat="server" 
                    onclick="javascript:validateCheckBoxControl(this, '0');" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="ctlSelect" runat="server" 
                    onclick="javascript:validateCheckBoxControl(this, '1');" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Program Code" 
            SortExpression="ProgramCode">
            <ItemTemplate>
                <asp:Label ID="ctlLblProgramCode" runat="server" Text='<%# Bind("ProgramCode")%>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Program Name" 
            SortExpression="ProgramName">
            <ItemTemplate>
                <asp:Label ID="ctlLblPBName" runat="server" Text='<%# Bind("ProgramsName")%>'></asp:Label>
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
                    <td>
                        <asp:Button ID="ctlButtonAdd" SkinID="SkCtlButton" Text="$Add$" runat="server" OnClick="Add_Click" />
                        <asp:Button ID="ctlButtonDelete" SkinID="SkCtlButton" Text="$Delete$" runat="server" OnClick="Delete_Click"  OnClientClick="return confirm('Are you sure delete this row');" />
                        <asp:Button ID="ctlButtonClose" OnClick="ctlButtonClose_Click" SkinID="SkCtlButton"
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










