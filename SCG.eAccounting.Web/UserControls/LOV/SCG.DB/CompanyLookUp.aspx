<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
    CodeBehind="CompanyLookup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CompanyLookup"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table id="ctlContainer" runat="server">
        <tr align="center">
            <td>
                <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
                <asp:Panel ID="pnCompanySearch" runat="server" Width="600px" BackColor="White">
                    <asp:Panel ID="pnCompanySearchHeader" CssClass="table" runat="server" Style="cursor: move;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblTitle" runat="server" Text="Company" Width="210px"></asp:Label></p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelSearchCompany" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <fieldset id="Fieldset1" style="width: 70%" id="fdsSearch" class="table">
                                    <legend id="Legend1" style="color: #4E9DDF" class="table">
                                        <asp:Label ID="Label2" runat="server" Text='Search Box'></asp:Label></legend>
                                    <table width="100%" border="0" class="table">
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlCompanyNameLabel" runat="server" Text='Company Code' SkinID="SkGeneralLabel"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlCompanyCodeCri" SkinID="SkGeneralTextBox" MaxLength="20" Style="text-align: center;"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:HiddenField ID="ctlFlagActive" runat="server" />
                                                <asp:HiddenField ID="ctlFlagUseEccOnly" runat="server" />
                                                <asp:HiddenField ID="ctlFlagUseExpOnly" runat="server" />
                                                <asp:Label ID="ctlIsMultiple" runat="server" Text="false" Style="display: none;"></asp:Label>
                                                <asp:Label ID="ctlCompanyCodeLabel" runat="server" Text='Company Name' SkinID="SkGeneralLabel"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlCompanyNameCri" runat="server" MaxLength="100" SkinID="SkGeneralTextBox"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                                                    OnClick="ctlSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="ctlCompanyDiv" style="height: 300; overflow-y: auto;">
                        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <center>
                                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                                        DynamicLayout="true" EnableViewState="true">
                                        <ProgressTemplate>
                                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlCompanyGrid" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                        AllowSorting="true" ReadOnly="true" EnableInsert="false" EnableViewState="true"
                                        OnRowCommand="ctlCompanyGrid_RowCommand" DataKeyNames="CompanyID" CssClass="Grid"
                                        OnDataBound="ctlCompanyGrid_DataBound" Width="99%" OnRequestCount="RequestCount"
                                        OnRequestData="RequestData">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                                </ItemTemplate>
                                                <ItemStyle Width="25px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ctlSelectCompany" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                                        CommandName="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company Code" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="CompanyCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCompanyCode" runat="server" Text='<%# Eval("CompanyCode") %>' SkinID="SkGeneralLabel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company Name" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="CompanyName">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCompanyName" runat="server" Text='<%# Eval("CompanyName") %>' SkinID="SkGeneralLabel"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='No DataFound'></asp:Label>
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                    </ss:BaseGridView>
                                    <div style="text-align: left; width: 98%">
                                        <table border="0">
                                            <tr>
                                                <td valign="middle">
                                                    <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkSubmitButton" OnClick="ctlSubmit_Click"
                                                        Text="Submit" Visible="false" />&nbsp;
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCancelButton" OnClick="ctlCancel_Click" />
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
