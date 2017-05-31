<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PopupMasterPage.Master"
    CodeBehind="AdvanceLookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.AV.AdvanceLookUp"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="AdvanceLookup.ascx" TagName="AdvanceLookup" TagPrefix="uc1" %>
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
                        <asp:UpdatePanel ID="UpdatePanelSearchAdvance" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                                    <table width="100%" border="0" class="table">
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="lblAdvanceNo" runat="server" Text='$Advance No.$'></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlAdvanceNo" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="lblDescription" runat="server" Text='$Subject$'></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <center>
                                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                        DynamicLayout="true" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlAdvanceLookupGrid" EnableViewState="true" Width="99%" runat="server"
                                        DataKeyNames="AdvanceID" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                                        OnRowCommand="ctlAdvanceLookupGrid_RowCommand" OnDataBound="ctlAdvanceLookupGrid_DataBound"
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
                                            <asp:TemplateField HeaderText="Advance No." HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="DocumentNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridAdvanceNo" runat="server" Text='<%# Eval("DocumentNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="Center" SortExpression="DueDateOfRemittance">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridDueDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDateOfRemittance")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlGridAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:#,##0.00}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
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
                                <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
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
