<%@ Page Title="" StylesheetTheme="Default" Language="C#" MasterPageFile="~/ProgramsPages.Master"
    AutoEventWireup="true" CodeBehind="Country.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Country"
    meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/CountryEditor.ascx" TagName="CountryEditor" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>--%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 45%">
                <fieldset id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlCountryCodeLabel" runat="server" Text="$CountryCode$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlCountryCodeCri" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="ctlCommentLabel" runat="server" Text="$Comment$"></asp:Label>
                                :
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlCommentCri" MaxLength="100" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanelGridview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridview"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlCountryGrid" runat="server" AutoGenerateColumns="false" CssClass="table"
                AllowSorting="true" AllowPaging="true" DataKeyNames="CountryID" EnableInsert="False"
                ReadOnly="true" OnRowCommand="ctlCountryGrid_RowCommand" OnRowDataBound="ctlCountryGrid_RowDataBound"
                OnRequestCount="RequestCount" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                OnDataBound="ctlCountryGrid_DataBound" OnPageIndexChanged="ctlCountryGrid_PageIndexChanged">
                <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
                <Columns>
                    <%--  <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ctlSelectAllChk" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlSelectChk" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Country Code" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="CountryCode">
                        <ItemTemplate>
                            <asp:Label ID="ctlCountryCodeLabel" runat="server" Text='<%# Bind("CountryCode") %>'
                                CommandName="Select"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="40%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="DbCountry.Comment">
                        <ItemTemplate>
                            <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# Bind("Comment") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="50%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="DbCountry.Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlProgramActive" Checked='<%# Bind("Active") %>' runat="server"
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="UserEdit" />
                                 <asp:ImageButton runat="server" ID="ctlDelete" SkinID="SkCtlGridDelete"
                                 ToolTip='<%# GetProgramMessage("Delete") %>'
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="UserDelete" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                        runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="vertical-align: middle;">
                <table style="text-align: center;">
                    <tr>
                        <td>
                            <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:HiddenField ID="CountryId" runat="server" />
            <uc2:CountryEditor ID="CountryEditor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel></asp:Content>
