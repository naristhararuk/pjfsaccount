<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="UserProfileLookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserProfileLookUp"
    EnableTheming="true" StylesheetTheme="Default" EnableEventValidation="false"  %>

<%@ Register Src="UserProfileLookup.ascx" TagName="UserProfileLookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table id="ctlContainer" runat="server">
        <tr align="center">
            <td>

                <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

                <asp:Panel ID="ctlUserSearchPanel" runat="server" Width="600px" BackColor="White"
                    Style="display: block">
                    <asp:Panel ID="ctlUserSearchHeader" CssClass="table" runat="server" Style="cursor: move;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture" runat="server" Text="Header" Width="210px"></asp:Label></p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelSearchUser" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <fieldset id="fdsSearch" style="width: 70%" class="table">
                                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                                        <asp:Label ID="ctlSearchbox" runat="server" Text="Search Box"></asp:Label></legend>
                                    <table width="100%" border="0" class="table">
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlMode" runat="server" Style="display: none;" />
                                                <asp:Label ID="ctlUserIDLabel" runat="server" Text="User ID"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlUserId" runat="server" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlNameLabel" runat="server" Text="User Name"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlName" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 20%">
                                                <asp:Label ID="ctlEmployeeCodeLabel" runat="server" Text="Employee Code"></asp:Label>
                                                :
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:TextBox ID="ctlEmployeeCode" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <div id="divSearchFavorite" style="display: none;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ctlSearchFromFavorite" runat="server" Text="$Search From My Favorite Only$" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                &nbsp;&nbsp;<asp:CheckBox ID="ctlSearchFavoriteApprover" runat="server" Text="$Approver$" />
                                                                <br />
                                                                &nbsp;&nbsp;<asp:CheckBox ID="ctlSearchFavoriteInitiator" runat="server" Text="$Initiator$" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ctlSearchFromAllUser" runat="server" Text="$Search From All User$" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                &nbsp;&nbsp;<asp:CheckBox ID="ctlApprovalFlag" runat="server" Text="$ApprovalFlag$" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                                                    OnClick="ctlSearch_Click" />
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
                                <%--<div id="divUserSearchResultGrid" style="height: 300px; overflow-y: auto;">--%>
                                    <ss:BaseGridView ID="ctlUserSearchResultGrid" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="true" OnRequestData="RequestData" ReadOnly="true" EnableInsert="false"
                                        AllowSorting="true" EnableViewState="true" DataKeyNames="Userid" CssClass="Grid"
                                        OnRequestCount="RequestCount" OnDataBound="ctlUserSearchResultGrid_DataBound"
                                        OnRowDataBound="ctlUserSearchResultGrid_RowDataBound" Width="99%" OnRowCommand="ctlUserSearchResultGrid_RowCommand"
                                        ShowHeader="true">
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
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ctlSelect1" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                                        CommandName="SelectUser" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UserID" HeaderStyle-HorizontalAlign="Center" SortExpression="u.UserID">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Name" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="u.EmployeeName">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlEmpName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EmployeeCode" SortExpression="u.EmployeeCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlEmpCode" runat="server" Text='<%# Eval("EmployeeCode") %>' SortExpression="u.EmployeeCode"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CompanyCode" SortExpression="u.CompanyCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlComCode" runat="server" Text='<%# Eval("CompanyCode") %>' SortExpression="u.CompanyCode"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                                    </ss:BaseGridView>
                                <%--</div>--%>
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
                                                <asp:ImageButton ID="ctlCancel" runat="server" ToolTip="Cancel" SkinID="SkCtlFormCancel"
                                                    OnClick="ctlCancel_Click" />
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
            </td>
        </tr>
    </table>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
