<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="CADocumentLookup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting.CADocumentLookup" StylesheetTheme="Default"  %>
<%@ Register Src="CADocumentLookup.ascx" TagName="CALookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table id="ctlContainerTable" runat="server">
        <tr>
            <td>
                <div align="center">

                    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

                    <asp:Panel ID="pnAdvanceSearch" runat="server" Width="600px" BackColor="White" >
                        <asp:Panel ID="pnAdvanceSearchHeader" CssClass="table" runat="server" Style="cursor: move;
                            border: solid 1px Gray; color: Black">
                            <div>
                                <p>
                                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
                            </div>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <center>
                                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                        DynamicLayout="true" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlCALookupGrid" EnableViewState="true" Width="99%" runat="server"
                                        DataKeyNames="CADocumentID" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                                        OnRowCommand="ctlCALookupGrid_RowCommand" OnDataBound="ctlCALookupGrid_DataBound"
                                        OnRequestData="RequestData" OnRequestCount="RequestCount" CssClass="Grid" ReadOnly="true"
                                        HeaderStyle-CssClass="GridHeader">
                                        <AlternatingRowStyle CssClass="GridItem" />
                                        <RowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ctlAdvanceSelect" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                                        CommandName="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MPA No." HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="DocumentNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridDocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridDescription" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Request Date" HeaderStyle-HorizontalAlign="Center" SortExpression="DueDateOfRemittance">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                    </ss:BaseGridView>
                                    <div style="text-align: left; width: 98%">
                                        <table border="0">
                                            <tr>
                                                <td valign="middle">
                                                    <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlSubmit_Click" />
                                                </td>
                                                <td valign="middle">
                                                    <asp:Label ID="ctlLblLine" runat="server" Text="|"></asp:Label>
                                                </td>
                                                <td valign="middle">
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>