<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachment.ascx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Attachment" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<div align="center">
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
<ss:InlineScript ID="ctlInlineScript" runat="server">
    <script type="text/javascript">
        function uploadfile() {
            var button = document.getElementById('<%= ctlUploaded.ClientID %>');
            if (button != null)
                button.click();
        }
    </script>
</ss:InlineScript>
    <asp:Panel ID="pnAttachment" runat="server" Width="100%" BackColor="White">
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdatePanelAttachment" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                    DynamicLayout="true" EnableViewState="true">
                                    <ProgressTemplate>
                                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <div id="ctlUploadFile" runat="server">
                                    <table width="60%">
                                        <tr>
                                            <td colspan="2">
                                                <iframe id="ctlUpload" runat="server" width="100%" scrolling="no" frameborder="0"
                                                    height="60"></iframe>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="ctlUpdateAttachmentGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <center>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelAttachment"
                                    DynamicLayout="true" EnableViewState="true">
                                    <ProgressTemplate>
                                        <uc4:SCGLoading ID="SCGLoading2" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="60%">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <font color="red">
                                                <asp:Label ID="ctlDescription" SkinID="SkGeneralLabel" runat="server"></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button runat="server" ID="ctlUploaded" OnClick="ctlUploaded_OnClick" UseSubmitBehavior="false"
                                                Style="display: none" />
                                            <ss:BaseGridView ID="ctlAttachmentGrid" runat="server" AutoGenerateColumns="False"
                                                CssClass="Grid" ReadOnly="true" EnableViewState="true" DataKeyNames="AttachmentID"
                                                Width="100%" OnRowDataBound="ctlAttachmentGrid_RowDataBound" ShowMsgDataNotFound="false"
                                                AllowPaging="false" OnDataBound="ctlAttachmentGrid_DataBound">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <RowStyle CssClass="GridItem" />
                                                <FooterStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Seq">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ctlSequence" runat="server" SkinID="SkOrderLabel"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="ctlAttachFileName" runat="server" Text='<%# Eval("AttachFileName") %>'
                                                                Font-Names="Tahoma" Font-Size="8"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCtlAttachmentGridCheckBox(this, '0');" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCtlAttachmentGridCheckBox(this, '1');" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblNodata" SkinID="SkNodataLabel" runat="server" Text='     '></asp:Label>
                                                </EmptyDataTemplate>
                                            </ss:BaseGridView>
                                        </td>
                                        <td align="center" valign="top" style="width: 10%">
                                            <asp:ImageButton runat="server" ID="ctlRemove" SkinID="SkDeleteButton" ToolTip="$Remove$"
                                                OnClick="ctlRemove_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ctlUploaded" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
