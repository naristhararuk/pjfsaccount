<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" EnableTheming="true"
    StylesheetTheme="Default" AutoEventWireup="true" CodeBehind="ProfileList.aspx.cs"
    Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ProfileList" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:sCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" class="table">
                <tr>
                    <td colspan="2">
                        <ss:BaseGridView ID="ctlProfileListGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="Id,ProfileName"
                            EnableInsert="False" ReadOnly="true" OnRowCommand="ctlProfileListGrid_RowCommand"
                            OnRowDataBound="ctlProfileListGrid_RowDataBound" OnRequestCount="RequestCount"
                            OnDataBound="ctlProfileListGrid_DataBound" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                            Width="50%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Profile Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="TaxCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlProfileName" runat="server" Text='<%# Bind("ProfileName") %>'
                                            Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="EditItem" />
                                        <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                            ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="DeleteItem" OnClientClick="return confirm('Are you sure delete this row');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                    runat="server"></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton runat="server" ID="ctlAddItem" SkinID="SkCtlFormNewRow" OnClick="ctlAddItem_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">
                            <spring:ValidationSummary ID="ctlValidationProfileError" runat="server" Provider="ProfileError.Error" />
                        </font>
                    </td>
                    <tr>
            </table>
            <div style="text-align: left;" id="DivManageDataField" runat="server">
                <fieldset id="ctlFieldSetDetailGridView" style="width: 400px; padding: 5px; border-color: #000;"
                    class="table">
                    <table width="100%" class="table">
                        <tr>
                            <td align="left" width="30%">
                                <asp:Label ID="PrifileNameLabel" Text="Profile name : " runat="server"></asp:Label>
                                <asp:TextBox ID="ctlInputProfileName" runat="server" SkinID="SkGeneralTextBox" Width="180px"></asp:TextBox>
                                <asp:HiddenField ID="ctlHiddenProfileListId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:ImageButton runat="server" ID="AddButton" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                                <asp:ImageButton runat="server" ID="UpdateButton" SkinID="SkSaveButton" OnClick="ctlUpdate_Click" />
                                <asp:ImageButton runat="server" ID="CancelButton" SkinID="SkCancelButton" OnClick="ctlCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
