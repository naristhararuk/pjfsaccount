<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencySetupEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CurrencySetupEditor" %>
<asp:Panel ID="ctlCurrencyEditor" runat="server" Style="display: none" CssClass="modalPopup" Width="750">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlCurrencySetupEditorFormHeader" CssClass="table" runat="server"
                    Style="cursor: move; border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditCurrency" runat="server" Text='$Header$' SkinID="SkFieldCaptionLabel"
                        Width="100%"></asp:Label></p>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlCurrencyUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <table width="100%">
                <tr>
                    <td align="left">
            <fieldset style="width: 90%" id="fdsCurrency">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                    <tr>
                        <td align="left" >
                            <asp:Label ID="ctrSymbolLabel" Text="$Symbol$:" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                        <font color="red">*</font>
                                
                            
                        </td>
                        
                        <td align="left" >
                            <asp:TextBox ID="ctlSymbol" SkinID="SkCtlTextboxLeft" MaxLength="20" runat="server"
                                Text="" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" >
                            <asp:Label ID="ctlCommentLabel" Text="$Comment$:" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>

                        </td>
                        
                        <td align="left" >
                            <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                Text="" Width="150px" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left" >
                            <asp:Label ID="ctlCurrencySymbolLabel" Text="$CurrencySymbol$:" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>

                        </td>
                        
                        <td align="left" >
                            <asp:TextBox ID="ctlCurrencySymbol" SkinID="SkCtlTextboxLeft" MaxLength="50" runat="server"
                                Text="" Width="150px" />
                        </td>
                    </tr>

                    <tr>
                        <td align="left" >
                            <asp:Label ID="ctlActiveLabel" Text="$Active$" SkinID="SkFieldCaptionLabel" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="ctlActive" runat="server" Checked="false" />
                        </td>
                    </tr>
                </table>
                 <br />
            </fieldset>
           </td>
                </tr>
                <tr>
                    <td align="left">
            <ss:BaseGridView ID="ctlCurrencyEditorGrid" runat="server" AutoGenerateColumns="false"
                CssClass="Grid" AllowSorting="true" DataKeyNames="LanguageID,CurrencyID" SelectedRowStyle-BackColor="#6699FF"
                OnDataBound="CurrencyEditor_DataBound" OnRowCommand="ctlCurrencySetup_RowCommand"
                OnRequestData="RequestData" Width="100%">
                <HeaderStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <RowStyle CssClass="GridItem" />
                <Columns>
                    <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center" SortExpression="l.LanguageID">
                        <ItemTemplate>
                            <asp:Label ID="ctlUserProfileCodeLabel" runat="server" Text='<%# Eval("LanguageName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width = "300px" 
                        SortExpression="l.Description">
                        <ItemTemplate>
                            <asp:TextBox ID="ctrDescription" SkinID="SkCtlTextboxLeft" MaxLength="100" runat="server"
                                Text='<%# Eval("Description")%>'/>
                        </ItemTemplate>
                        <ItemStyle Width= "300px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width = "300px" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:TextBox ID="ctrComment" SkinID="SkCtlTextboxLeft" MaxLength="500" runat="server"
                                Text='<%# Eval("Comment")%>'   />
                        </ItemTemplate>
                        <ItemStyle Width= "300px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MainUnit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width = "100px" SortExpression="MainUnit">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlMainUnit" SkinID="SkCtlTextboxLeft" Width="100" MaxLength="100" runat="server"
                                Text='<%# Eval("MainUnit")%>'   />
                        </ItemTemplate>
                        <ItemStyle Width= "50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SubUnit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width = "100px" SortExpression="SubUnit">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlSubUnit" SkinID="SkCtlTextboxLeft" Width="100"  MaxLength="100" runat="server"
                                Text='<%# Eval("SubUnit")%>'   />
                        </ItemTemplate>
                        <ItemStyle Width= "50px" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="LangActive">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActive" Checked='<%# Eval("LangActive") %>' runat="server" Enabled="true" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </ss:BaseGridView>
            </td>
                </tr>
                <tr>
                    <td align="left">
            <table width="100%" class="table">
                <tr>
                    <td align="left" style="width: 60%">
                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkSaveButton" OnClick="ctlAdd_Click" />
                        <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                            OnClick="ctlCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td  align="center">
                        <font color="red">
                            <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Currency.Error" />
                        </font>
                    </td>
                </tr>
            </table>
               </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="ctlCurrencyEditorModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlCurrencyEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlCurrencyEditor" />
