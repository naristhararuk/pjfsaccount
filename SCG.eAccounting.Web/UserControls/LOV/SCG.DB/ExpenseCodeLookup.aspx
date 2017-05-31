<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
    CodeBehind="ExpenseCodeLookup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.ExpenseCodeLookup"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <asp:Panel ID="pnAccountSearch" runat="server" Width="600px" BackColor="White" Style="display: block">
        <asp:Panel ID="pnAccountSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px" SkinID="SkGeneralLabel"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                        <legend id="legSearch" style="color: #4E9DDF" class="table">
                            <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$' SkinID="SkGeneralLabel"></asp:Label></legend>
                        <table width="100%" border="0" class="table">
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlExpenseGroupLabel" runat="server" Text='$Expense Group$' SkinID="SkGeneralLabel"></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:DropDownList ID="ctlExpenseGroup" runat="server" Width="203px" SkinID="SkGeneralDropdown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlAccountCodeLabel" runat="server" Text='$Expense Code$' SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlAccountCode" runat="server" Width="200px" MaxLength="20" SkinID="SkGeneralTextBox"
                                        Style="text-align: center;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlDescriptionLabel" runat="server" Text='$Description$' SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlDescription" runat="server" Width="200px" MaxLength="100" SkinID="SkGeneralTextBox"
                                        Style="text-align: center;"></asp:TextBox>
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
        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <ss:BaseGridView ID="ctlAccountGridView" runat="server" AutoGenerateColumns="False"
                        OnRequestData="RequestData" ReadOnly="True" EnableInsert="False" OnRequestCount="RequestCount"
                        AllowPaging="True" AllowSorting="true" DataKeyNames="AccountID" CssClass="table"
                        OnDataBound="ctlAccountGrid_DataBound" Width="99%" OnRowCommand="ctlAccountGrid_RowCommand"
                        InsertRowCount="1" SaveButtonID="" HeaderStyle-CssClass="GridHeader">
                        <AlternatingRowStyle CssClass="GridItem" />
                        <RowStyle CssClass="GridAltItem" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15px"/>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ctlAccountSelect" runat="server" SkinID="SkSelectButton" CausesValidation="False"
                                        CommandName="Select" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="AccountCode"
                                HeaderText="Expense Code">
                                <ItemTemplate>
                                    <asp:Label ID="ctlAccountCode" runat="server" Text='<%# Eval("AccountCode") %>' SkinID="SkGeneralLabel"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="Description"
                                HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="ctlDescription" runat="server" Text='<%# Eval("AccountName") %>' SkinID="SkGeneralLabel"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
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
                                    <asp:ImageButton ID="ctlSelect" runat="server" SkinID="SkSelectButton" OnClick="ctlSelect_Click" />
                                </td>
                                <td valign="middle">
                                    <asp:Label ID="ctlLine" runat="server" Text="|"></asp:Label>
                                </td>
                                <td valign="middle">
                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCancelButton" OnClick="ctlCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ctlSelect" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <uc1:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
