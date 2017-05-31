<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PBEditor.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.PBEditor" %>
<%@ Register Src="LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DropdownList/SS.DB/CurrencyDropdown.ascx" TagName="CurrencyDropdown"
    TagPrefix="uc10" %>
<asp:Panel ID="ctlPBEditor" runat="server" Style="display: none" CssClass="modalPopup"
    Width="600px">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlPBEditorFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlAddEditPB" runat="server" SkinID="SkFieldCaptionLabel" Text='$Manage PB Data$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlPBUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="left">
                        <fieldset style="width: 98%" id="fdsPB">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlPBCodeLabel" Text="$PB Code$" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="ctlPBCodeReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlPBCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                            Text="" Width="148px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlCompanyFieldLabel" Text="$Company$" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="ctlCompanyFieldReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2" style="margin-left: 50px">
                                        <uc1:CompanyField ID="ctlCompanyField" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlPettyCashLimitLabel" Text="$Petty Cash Limit$" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="ctlPettyCashLimitReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlPettyCashLimit" SkinID="SkCtlTextboxLeft" runat="server" Text="" style="text-align:right" 
                                            Width="148px" OnKeyPress="return(currencyFormat(this, ',', '.', event, 12));"
                                            OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                    </td>
                                </tr>
                                  <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlSupplementaryLabel" Text="$Supplementary$" runat="server"></asp:Label>&nbsp:&nbsp
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="ctlSupplementary" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                            Text="" Width="148px" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="left">
                                        <asp:Label ID="ctlBlockPostLabel" Text="$Block Post$"
                                            runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlBlockPost" runat="server" Checked="false" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlActiveLabel" Text="$Active$" runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlActive" runat="server" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlRepOfficeLabel" Text="Rep Office" runat="server"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ctlRepOffice" runat="server" OnCheckedChanged="ctlRepOffice_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlMainCurrencyLabel" runat="server" Text="Main Currency"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ctlMainCurrencyDropdown" runat="server" MaxLength="50" Width="235px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="ctlErrorMainCurrencyLabel" runat="server" ForeColor="Red" Font-Bold =true Visible=false  ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ctlLocalCurrencyLabel" runat="server" Text="Local Currency"></asp:Label>
                                    </td>
                                    
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <uc10:CurrencyDropdown ID="ctlLocalCurrencyDropdown" runat="server" IsExpense="true"/>
                                                </td>
                                                <td>
                                                    <asp:ImageButton runat="server" ID="ctlAddLocalCurrencyButton" SkinID="SkAddButton" OnClick="ctlAddLocal_Click"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                  
                                   
                                </tr>
                                <tr>
                                 <td align="left">
                                    <asp:Label ID="ctlErrorLocalCurrencyLabel" runat="server" Text="test" ForeColor="Red" Font-Bold =true Visible=false ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        <ss:BaseGridView ID="ctlPBLocalCurrencyGridview" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grid" AllowSorting="true" DataKeyNames="PBID, CurrencyID" SelectedRowStyle-BackColor="#6699FF"
                                            OnRowDataBound="ctlPBLocalCurrencyGridview_RowDataBound" Width="100%" ShowHeaderWhenEmpty="true" OnRowCommand="ctlPBLocalCurrencyGridview_RowCommand">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctlLocalCurrency" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                    SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ctrLocalDescription" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ctlDeleteLocalCurrency" text="Delete" runat="server" SkinID="SkCtlGridDelete" CommandName="DeleteLocalCurrency" OnClientClick="return confirm('Are you sure delete this row');" />

                                                       <%-- <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                            CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                                            CommandName="PBDelete" />--%>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </ss:BaseGridView>
                                    
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <ss:BaseGridView ID="ctlPBEditorGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="Grid" AllowSorting="true" DataKeyNames="LanguageID,PBID" SelectedRowStyle-BackColor="#6699FF"
                            OnRowCommand="ctlPB_RowCommand" OnRequestData="RequestData" OnDataBound="PBEditor_DataBound"
                            Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Language" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlPBLanguageLabel" runat="server" Text='<%# Eval("LanguageName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctrDescription" runat="server" MaxLength="100" Text='<%# Eval("Description")%>'
                                            Width="200px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="200px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ctrComment" runat="server" MaxLength="500" Text='<%# Eval("Comment")%>'
                                            Width="200px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="200px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Eval("Active") %>' runat="server" Enabled="true" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
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
                                    <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Save" SkinID="SkSaveButton"
                                        OnClick="ctlAdd_Click" />
                                    <asp:ImageButton runat="server" ID="ctlCancel" ToolTip="Cancel" SkinID="SkCancelButton"
                                        OnClick="ctlCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    <font color="red">
                                        <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="PB.Error" />
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
<ajaxToolkit:ModalPopupExtender ID="ctlPBEditorModalPopupExtender" runat="server"
    TargetControlID="lnkDummy" PopupControlID="ctlPBEditor" BackgroundCssClass="modalBackground"
    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlPBEditor" />
