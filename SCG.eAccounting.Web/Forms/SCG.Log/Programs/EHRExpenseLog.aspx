<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="EHRExpenseLog.aspx.cs" EnableTheming="true" Inherits="SCG.eAccounting.Web.Forms.SCG.Log.Programs.EHRExpenseLog"
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" class="table">
                <tr>
                    <td align="left" style="width: 35%">
                        <fieldset id="fdsCritiria" style="text-align: left; border-color: Gray; border-style: solid;
                            border-width: 1px; font-family: Tahoma; font-size: small; width: auto;" runat="server">
                            <table>
                                <tr>
                                    <td align="left" style="width: 50%">
                                        <asp:Label ID="ctlStatusLabel" runat="server" Text=" $Status$:"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlStatus" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Fail</asp:ListItem>
                                            <asp:ListItem>Success</asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlDateLabel" runat="server" Text="$Date$ :"></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                        <uc1:Calendar ID="ctlDate" runat="server" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlExpenseRequestNoLabel" runat="server" Text="$e-HR Form NO.$ :"></asp:Label>
                                    </td>
                                    <td colspan="2" align="left">
                                       <asp:TextBox ID="ctlExpenseRequestNo" SkinID="SkCtlTextboxLeft" runat="server" Text='' MaxLength="20"/>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td valign="top" align="left">
                        <asp:ImageButton ID="ctlSearch" SkinID="SkSearchButton" runat="server" ext="Search" OnClick="ctlSearch_Click" />
                    </td>
                </tr>
            </table>
            <ss:BaseGridView ID="ctlEHRExpenseLogGrid" runat="server" AutoGenerateColumns="False"
                Width="100%" OnRequestData="RequestData" OnRequestCount="RequestCount" AllowPaging="True"
                AllowSorting="True" CssClass="table" HeaderStyle-CssClass="GridHeader" SelectedRowStyle-BackColor="#6699FF"
                ReadOnly="true">
                <Columns>
                    <asp:TemplateField HeaderText="Last Date" SortExpression="LastDate">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblLastDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("LastDate").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="e-HR Expense ID" SortExpression="EHrExpenseLogID"
                        HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblEHrExpenseLogID" runat="server" Text='<%# Bind("ExpenseRequestNo")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expense ID" SortExpression="EHrExpenseID" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblEHrExpenseID" runat="server" Text='<%# Bind("EHrExpenseID")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Replace ID" SortExpression="ReplaceID" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblReplaceID" runat="server" Text='<%# Bind("ReplaceID")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblStatus" runat="server" Text='<%# Bind("Status")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Message" SortExpression="Message" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblMessage" runat="server" Text='<%# Bind("Message")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expense Date" SortExpression="ExpenseDate" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblExpenseDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("ExpenseDate").ToString())%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
