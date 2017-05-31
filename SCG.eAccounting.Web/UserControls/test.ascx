<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="test.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.test" %>
<style type="text/css">
    .style1
    {
        width: 168px;
    }
</style>
<asp:Panel ID="ctltest" runat="server" Style="display: block" CssClass="modalPopup">
    <asp:Panel ID="ctlCurrencySetupEditorFormHeader" CssClass="table" runat="server"
        Style="cursor: move; border: solid 1px Gray; color: Black" Width="100%">
        <div>
            <p>
                <asp:Label ID="ctlAddEditCurrency" runat="server" Text='$Header$' Width="100%"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="ctlCurrencySetupForm2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 90%" id="fdsCurrency" class="table">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <table class="table" width="100%">
                        <tr>
                            <td align="left" class="style1">
                                $Symbol$ :<font color="red"><asp:Label ID="ctrSymbolLabel" runat="server"></asp:Label></font>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="ctlSymbol" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                    Text="" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style1">
                                $Active$
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="ctlActive" runat="server" Checked="false" />
                            </td>
                        </tr>
                    </table>
                </table>
                   </fieldset>
             <ss:BaseGridView ID="ctltestGrid" runat="server" AutoGenerateColumns="false"
                    CssClass="table" AllowSorting="true"  DataKeyNames="CurrencyID"
                    SelectedRowStyle-BackColor="#6699FF" OnRowCommand="ctlCurrencySetup_RowCommand"
                    OnRequestData="RequestData" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="$Language$" HeaderStyle-HorizontalAlign="Center" SortExpression="l.LanguageID">
                            <ItemTemplate>
                                <asp:Label ID="ctlUserProfileCodeLabel" runat="server" Text='<%# Bind("LanguageName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="$Description$" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="l.Description">
                            <ItemTemplate>
                            <asp:TextBox ID="ctrDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Text='<%# Bind("Description")%>' Width="250px" />
                            </ItemTemplate>
                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="$Comment$" HeaderStyle-HorizontalAlign="Center" SortExpression="l.">
                            <ItemTemplate>
                          
                               <asp:TextBox ID="ctrComment" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Text='<%# Bind("Comment")%>' Width="250px" /> 
                            </ItemTemplate>
                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="$Active$" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                           
                    </Columns>
                </ss:BaseGridView>
                <table width="100%" class="table">
                    <tr>
                        <td align="left" style="width: 60%">
                            <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkCtlFormNewRow"
                                OnClick="ctlAdd_Click" />
                        </td>
                         <td align="left" style="width: 60%">
                            <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Add" SkinID="SkCtlFormNewRow"
                                OnClick="ctlCancel_Click" />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctltestModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctltest" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    RepositionMode="None" PopupDragHandleControlID="ctltest" />
