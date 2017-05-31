<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CountryEditor" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Panel ID="ctlCountryEditor" runat="server" Style="display: none" CssClass="modalPopup"
    Width="600px">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlCountryEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditCountry" runat="server" SkinID="SkFieldCaptionLabel" Text='$Manage Country Data$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanelCountryForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="table">
                <tr>
                    <td align="right">
                        <asp:Label ID="ctlCountryCodeLabel" Text="$Country Code$" runat="server"></asp:Label>&nbsp;
                        :
                        <asp:Label ID="ctlCountryCodeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="ctlCountryCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="50"
                            Text='<%# Bind("CountryCode") %>' Width="250px" />
                        <font color="red">
                            <asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        <asp:Label ID="ctlCommentLabel" Text="$Comment$" runat="server"></asp:Label>&nbsp;
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine" SkinID="SkCtlTextboxMultiLine"
                            Height="50px" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                            Text='<%# Bind("Comment") %>' Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="ctlActiveLabel" Text="$Active$" runat="server"></asp:Label>&nbsp;
                        :
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                            <ss:BaseGridView ID="ctlCountryLangGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" DataKeyNames="LanguageID,CID" SelectedRowStyle-BackColor="#6699FF"
                             OnRequestData="RequestData" OnDataBound="ctlCountryLangGrid_DataBound"
                            Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Language Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="LanguageName">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlLanguageName" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Country Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="CountryName">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctlCountryName" SkinID="SkCtlTextboxLeft" runat="server" Width="95%"
                                            MaxLength="200" Text='<%# Bind("CountryName") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="31%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxLeft" runat="server" Width="95%"
                                            MaxLength="500" Text='<%# Bind("Comment") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Bind("Active") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="75px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Save" SkinID="SkSaveButton"
                            OnClick="ctlAdd_Click" />
                        <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                            OnClick="ctlCancel_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlCountryModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlCountryEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlCountryEditor" />
