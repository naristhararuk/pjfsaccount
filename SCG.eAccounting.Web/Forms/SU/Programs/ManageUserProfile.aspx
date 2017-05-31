<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ManageUserProfile.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.ManageUserProfile"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="../../../UserControls/UserProfileEditor.ascx" TagName="UserProfileEditor"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../../UserControls/LOV/SCG.DB/UserGroupLookup.ascx" TagName="UserGroupLookup"
    TagPrefix="uc3" %>
<%@ Register Src="../../../UserControls/ApproverEditor.ascx" TagName="Approver" TagPrefix="uc4" %>
<%@ Register Src="../../../UserControls/InitiatorEditor.ascx" TagName="InitiatorEditor"
    TagPrefix="uc5" %>
<%@ Register Src="../../../UserControls/UserGroupEditor.ascx" TagName="UserGroupEditor"
    TagPrefix="uc6" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 45%">
                <fieldset style="width: 90%" id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlUserIdLabel" runat="server" Text="$UserId$"></asp:Label>
                                <asp:Label ID="ctlColonUserId" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlUserID" SkinID="SkGeneralTextBox" Width="150" MaxLength="20"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="ctlUserGroupLabel" runat="server" Text="$GroupName$"></asp:Label>
                                <asp:Label ID="ctlColonUserGroup" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ctlUserGroup" SkinID="SkGeneralDropdown" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctrEmployeeNameLabel" runat="server"
                                    Text="$EmployeeName$"></asp:Label>
                                <asp:Label ID="ctlColonEmployeeName" runat="server"
                                    Text=":"></asp:Label>
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctrEmployeeName" SkinID="SkGeneralTextBox" Width="250" MaxLength="100"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlUserProfileSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlUserProfileSearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUserGridUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUserGridUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc7:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <ss:BaseGridView ID="ctlUserProfileGrid" runat="server" AutoGenerateColumns="false"
                CssClass="table" AllowSorting="true" AllowPaging="true" DataKeyNames="UserId"
                OnRowCommand="ctlUserProfile_RowCommand" ReadOnly="true" OnRequestCount="RequestCount"
                OnRequestData="RequestData" SelectedRowStyle-BackColor="#4E9DDF" Width="100%">
                <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
                <Columns>
                    <asp:TemplateField HeaderText="User Id" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="u.UserName">
                        <ItemTemplate>
                            <asp:Literal ID="ctlUserProfileCodeLabel" SkinID="SkGeneralLabel" runat="server" Text='<%# Bind("UserName") %>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="u.EmployeeName">
                        <ItemTemplate>
                            <asp:Literal ID="ctlUserProfileNameLabel" SkinID="SkGeneralLabel" runat="server" Text='<%# Bind("EmployeeName") %>' Mode="Encode"/>
                        </ItemTemplate>
                        <ItemStyle Width="25%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="u.CompanyCode">
                        <ItemTemplate>
                            <asp:Literal ID="ctlUserProfileCompanyLabel" SkinID="SkGeneralLabel" runat="server" Mode="Encode" Text='<%# Bind("CompanyCode") %>'/>
                        </ItemTemplate>
                        <ItemStyle Width="20%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActive" SkinID="SkGeneralCheckBox" Checked='<%# Bind("Active") %>'
                                runat="server" Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Email Active" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlEmailActive" SkinID="SkGeneralCheckBox" Checked='<%# Bind("EmailActive") %>'
                                runat="server" Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <table border="0">
                                <tr>
                                    <td align="center" valign="middle">
                                        <asp:LinkButton ID="ctlApprover" Text="Approver" CommandName="Approver" runat="server" />
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:LinkButton ID="ctlInitiator" Text="Initiator" CommandName="Initiator" runat="server" />
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:LinkButton ID="ctlGroup" Text="Group" CommandName="Group" runat="server" />
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                            CausesValidation="False" CommandName="UserProfileMethodEdit" />
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="UserProfileMethodDelete" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 60%">
                <asp:ImageButton runat="server" ID="ctrAdd" ToolTip="Add" SkinID="SkCtlFormNewRow"
                    OnClick="ctlAdd_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanelInformation" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="div1" align="center" style="width: 100%">
                <%--approver--%>
                <uc4:Approver ID="Approver" runat="server" isMultiple="true" />
                <%--initiator--%>
                <uc5:InitiatorEditor ID="Initiator" runat="server" />
                <%--group--%>
                <uc6:UserGroupEditor ID="UserGroup" runat="server" />
                <uc3:UserGroupLookup ID="ctlGroupLookup" runat="server" />
            </div>
            <asp:HiddenField ID="user" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlUserProfileEditorPanel" runat="server" Style="display: none" CssClass="modalPopup"
        Height="550px">
        <asp:Panel ID="ctlUserProfileEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <asp:Label ID="lblCapture" runat="server" SkinID="SkFieldCaptionLabel" Text='$Header$'></asp:Label>
        </asp:Panel>
        <uc1:UserProfileEditor ID="ctlAddEditPopup" runat="server" IsAdmin="true" ShowScrollBar="true" />
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlUserProfileModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlUserProfileEditorPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" RepositionMode="None" zIndex="2000" />
</asp:Content>
