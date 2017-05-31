<%@ Page Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" 
CodeBehind="IOLookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.IOLookUp"
     EnableTheming="true" StylesheetTheme="Default" %>
    
<%@ Register Src="IOLookup.ascx" TagName="IOLookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table id="ctlContainer" runat="server">
        <tr align="center">
            <td>
                <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

                <asp:Panel ID="pnIOSearch" runat="server" Width="600px" BackColor="White">
                    <asp:Panel ID="pnIOSearchHeader" CssClass="table" runat="server" Style="cursor: move;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                                    <table width="100%" border="0" class="table">
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlMode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCompanyCode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCompanyId" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCostCenterCode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlCostCenterId" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlIO" runat="server" Text='$Internal Order$' SkinID="SkGeneralLabel"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlIONumber" SkinID="SkGeneralTextBox" Style="text-align: center;"
                                                    runat="server" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlDescription" runat="server" Text='$Description$' SkinID="SkGeneralLabel"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlIODescription" SkinID="SkGeneralTextBox" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="ctlIODiv" style="height: 300; overflow-y: auto;">
                        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <center>
                                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                        DynamicLayout="true" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlSearchResultGrid" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="true" OnRequestData="RequestData" ReadOnly="true" EnableInsert="false"
                                        AllowSorting="true" EnableViewState="true" DataKeyNames="IOID" CssClass="Grid"
                                        OnDataBound="ctlSearchResultGrid_DataBound" Width="99%" OnRowCommand="ctlSearchResultGrid_RowCommand"
                                        OnRowDataBound="ctlSearchResultGrid_RowDataBound" OnRequestCount="ctlSearchResultGrid_RequestCount">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="25px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ctlIOSelect" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                                        CommandName="SelectIO" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Internal Order" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="io.IONumber">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlInternalOrder" runat="server" Text='<%# Eval("IONumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="io.IOText">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlDescription" runat="server" Text='<%# Eval("IOText") %>'></asp:Label>
                                                </ItemTemplate>
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
                                                <td>
                                                    <div id="ctlSubmitButton" runat="server" visible="false">
                                                        <table>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <asp:ImageButton runat="server" ToolTip="Save" ID="ctlSubmit" SkinID="SkCtlFormSave"
                                                                        OnClick="ctlSubmit_Click" />
                                                                </td>
                                                                <td valign="middle">
                                                                    |
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
