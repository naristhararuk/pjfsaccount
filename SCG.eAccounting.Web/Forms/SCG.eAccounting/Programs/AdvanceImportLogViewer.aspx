<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="AdvanceImportLogViewer.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.AdvanceImportLogViewer"
    EnableTheming="true" StylesheetTheme="Default" %>

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
            <div style="text-align:left;">
            <table width="40%" style="text-align:left;">
            <tr><td align="left">
            <fieldset id="fdsCritiria" style="text-align: left; border-color: Gray; border-style: solid;
                border-width: 1px; font-family: Tahoma; font-size: small; width: 100%;" runat="server">
                <table width="100%" class="table">
                    <tr>
                        <td align="left" style="width: 35%">
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlEaccountRequestIDLabel" SkinID="SkGeneralLabel" runat="server" Text="$E-Account Request ID$"></asp:Label>&nbsp; :
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:TextBox ID="ctlEaccountRequestID" SkinID="SkGeneralTextBox" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctleExpenseRequestIDLabel" SkinID="SkGeneralLabel" runat="server" Text="$E-Expense Request ID$"></asp:Label>&nbsp; :
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:TextBox ID="ctleExpenseRequestID" SkinID="SkGeneralTextBox" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlMessageLabel" SkinID="SkGeneralLabel" runat="server" Text="$Message$"></asp:Label>&nbsp; :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlMessage" SkinID="SkGeneralTextBox" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="50%">
                                        <asp:Label ID="ctlStatusLabel" SkinID="SkGeneralLabel" runat="server" Text="$Status$"></asp:Label>&nbsp; :
                                    </td>
                                    <td align="left" width="60%">
                                        <asp:DropDownList ID="ctlStatus" runat="server" SkinID="SkGeneralDropdown">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Fail</asp:ListItem>
                                            <asp:ListItem>Success</asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <asp:ImageButton ID="ctlSearch" SkinID="SkSearchButton" runat="server" Text="Search"
                                OnClick="ctlSearch_Click" />
                            
                        </td>
                    </tr>
                </table>
            </fieldset>
            </td></tr>
            </table>
            </div>
            <br />
            <center>
            <ss:BaseGridView ID="ctlAdvanceImportLogViewerGrid" runat="server" AutoGenerateColumns="False"
                Width="98%" AllowPaging="True" AllowSorting="true" OnRequestData="RequestData"
                OnRequestCount="RequestCount" CssClass="table" HeaderStyle-CssClass="GridHeader" ReadOnly="true">
                <Columns>
                    <asp:TemplateField HeaderText="Date" SortExpression="CreDate" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlDateLabel" runat="server" SkinID="SkGeneralLabel" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("CreDate").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="15%"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="E-Account Request ID" SortExpression="EACRequestNo">
                        <ItemTemplate>
                            <asp:Label ID="ctleAccountRequestIDLabel" SkinID="SkGeneralLabel" runat="server" Text='<%# Eval("EACRequestNo")  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" Width="20%"  />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="E-eExpense Request ID" SortExpression="EXPRequestNo" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctleExpenseRequestID" SkinID="SkGeneralLabel" runat="server" Text='<%# Eval("EXPRequestNo")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"  Width="20%"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlStatusLabel" SkinID="SkGeneralLabel" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"  Width="10%"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Message" SortExpression="Message" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlMessageLabel" SkinID="SkGeneralLabel" runat="server" Text='<%# Eval("Message")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            </center>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
