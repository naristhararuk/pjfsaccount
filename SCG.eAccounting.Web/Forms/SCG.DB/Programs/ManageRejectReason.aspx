<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageRejectReason.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ManageRejectReason"
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/FormEditor/SCG.DB/DbRejectReasonEditor.ascx" TagName="DbRejectResonEditor"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" style="text-align: left;">
                <tr>
                    <td style="width: 40%; text-align: center;">
                        <fieldset style="width: 90%; text-align: left;" id="fdsSearch">
                            <table border="0" width="100%">
                                <tr>
                                    <td align="left" style="width: 40%">
                                        <asp:Label ID="ctlReasonCodeLabel" runat="server" Style="text-align: center;" Text="$Reason Code$"
                                            SkinID="SkGeneralLabel" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlReasonCodeTxt" MaxLength="20" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 40%">
                                        <asp:Label ID="ctlRequestTypeLabel" runat="server" Text="$Request Type$" SkinID="SkGeneralLabel" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlRequestTypeDropdown" SkinID="SkGeneralDropdown" runat="server" OnSelectedIndexChanged="ctlRequestTypeDropdown_OnSelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 40%">
                                        <asp:Label ID="ctlStateEventIDLabel" runat="server" Text="$State ID$" SkinID="SkGeneralLabel" />
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlStateEventIDDropdown" SkinID="SkGeneralDropdown" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td align="left">
                        <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearchButton_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <ss:BaseGridView ID="ctlRejectReasonGridView" runat="server" Width="98%" ReadOnly="true"
                AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="ReasonID"
                CssClass="Grid" OnRequestData="RequestData" OnRequestCount="RequestCount" OnDataBound="ctlRejectReasonGridView_DataBound"
                OnRowCommand="ctlRejectReasonGridView_RowCommand" HeaderStyle-HorizontalAlign="Center"
                SelectedRowStyle-BackColor="#6699FF">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                <InsertRowStyle CssClass="GridItem" HorizontalAlign="left" />
                <FooterStyle CssClass="GridItem" HorizontalAlign="left" />
                <Columns>
                    <asp:TemplateField HeaderText="ReasonCode" SortExpression="ReasonCode">
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="ctlReasonCode" runat="server" Text='<%# Bind("ReasonCode") %>'
                                CommandName="Select" />--%>
                            <asp:Label ID="ctlReasonCode" runat="server" Text='<%# Bind("ReasonCode")%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Type" SortExpression="DocumentTypeCode">
                        <ItemTemplate>
                            <asp:Literal ID="ctlRequestTypeCode" runat="server" Text='<%# Bind("DocumentTypeCode") %>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="StateEvent ID" SortExpression="wfsl.displayname">
                        <ItemTemplate>
                            <asp:Literal ID="ctlStateID" runat="server" Text='<%# Bind("StateEventID") %>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%">
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <%# GetProgramMessage("Require")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%;">
                                        <%# GetProgramMessage("Comment")%>
                                    </td>
                                    <td>
                                        <%# GetProgramMessage("ConfirmRejection")%>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="width: 50%;">
                                        <asp:CheckBox ID="ctlRequireCommentChk" Enabled="false" runat="server" Checked='<%# Bind("RequireComment") %>' />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ctlRequireConfirmReject" runat="server" Enabled="false" Checked='<%# Bind("RequireConfirmReject") %>' />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkEditButton" CausesValidation="False"
                                CommandName="ReasonEdit" ToolTip='<%# GetProgramMessage("EditReason") %>' />
                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkDeleteButton" CausesValidation="False"
                                CommandName="ReasonDelete" OnClientClick="return confirm('Are you sure delete this row');"
                                ToolTip='<%# GetProgramMessage("DeleteReason") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" style="text-align: left;" visible="false">
                <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkAddButton" OnClick="ctlAddNew_Click" />
            </div>
            <uc2:DbRejectResonEditor ID="ctlDbRejectReasonEditor" runat="server"></uc2:DbRejectResonEditor>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server" />
</asp:Content>
