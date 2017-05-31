<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="WithHoldingTax.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.WithHoldingTax"
    Title="Untitled Page" EnableTheming="true" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/AlertMessageFadeOut.ascx" TagName="AlertMessageFadeOut"
    TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <%-- ------------ --%>
    <%-- Search       --%>
    <%-- ------------ --%>
    <asp:UpdatePanel ID="updPanelSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="updProgressSearch" runat="server" AssociatedUpdatePanelID="updPanelSearch"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="divCondition" align="left" style="width: 100%">
                <fieldset id="ctlFieldSetDetailGridView" style="width: 400px" class="table">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" class="table" width="400px">
                                    <tr>
                                        <td align="left" style="width: 20%">
                                            <asp:Label ID="ctlLblWHTCode" runat="server" Text='$WHT Code$'></asp:Label>
                                            :
                                        </td>
                                        <td align="left" style="width: 30%">
                                            <asp:TextBox ID="ctlTxtWHTCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"
                                                Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 20%">
                                            <asp:Label ID="ctlLblWHTTypeName" runat="server" Text='$WHT Name$'></asp:Label>
                                            :
                                        </td>
                                        <td align="left" style="width: 30%">
                                            <asp:TextBox ID="ctlTxtWHTName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="left" valign="top">
                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- ------------ --%>
    <%-- Main Program --%>
    <%-- ------------ --%>
    <asp:UpdatePanel ID="updPanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server" />
            <asp:UpdateProgress ID="updProgressGridView" runat="server" AssociatedUpdatePanelID="updPanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlGridWithholdingTax" runat="server" Width="100%" AutoGenerateColumns="False"
                EnableInsert="False" ReadOnly="true" AllowSorting="true" AllowPaging="true" DataKeyNames="WHTID"
                OnRequestCount="RequestCount" OnRequestData="RequestData" OnRowCommand="ctlGridWithholdingTax_RowCommand"
                CssClass="table" SelectedRowStyle-BackColor="#6699FF" HeaderStyle-CssClass="GridHeader">
                <AlternatingRowStyle CssClass="GridItem" />
                <RowStyle CssClass="GridAltItem" />
                <Columns>
                    <asp:TemplateField HeaderText="WHT Code" SortExpression="WHTCode" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblWhtCode" runat="server" Text='<%# Bind("WHTCode")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="WHT Name" SortExpression="WHTName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblWhtName" runat="server" Text='<%# Bind("WHTName")%>' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblRate" runat="server" Text='<%#SCG.eAccounting.Web.Helper.UIHelper.BindDecimal(Eval("Rate").ToString())+ " บาท " %> '></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="Active" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlChkActive" runat="server" Checked='<%# Bind("Active")%>' Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seq" SortExpression="Seq" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="ctlLblSeq" runat="server" Text='<%# Bind("Seq") %> ' Mode="Encode"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                CommandName="WHTEdit" ToolTip='<%# GetProgramMessage("EditWHT") %>' />
                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                CommandName="WHTDelete" ToolTip='<%# GetProgramMessage("DeleteWHT") %>' OnClientClick="return confirm('Are you sure delete this row');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" Width="20%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" runat="server" Text='<%# GetMessage("DataNotFound") %>'></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left">
                <asp:ImageButton ID="ctlBtnAddWHT" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlBtnAddWHT_Click" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ctlBtnAddWHT" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- ------------ --%>
    <%-- Grid View    --%>
    <%-- ------------ --%>
    <asp:Panel ID="panelFormView" runat="server" Style="display: none" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlPanelFormViewHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlLblFormViewHeader" SkinID="SkFieldCaptionLabel" runat="server" Text="$Add / Edit With Holding Tax$"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="updPanelFormView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div align="center" style="display: block;">
                    <asp:UpdateProgress ID="updProgressFormView" runat="server" AssociatedUpdatePanelID="updPanelFormView"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table border="0" cellpadding="0" cellspacing="0" class="TableInFormView">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlFormViewWHT" runat="server" DataKeyNames="WHTID" OnDataBound="ctlFormViewWHT_DataBound"
                                    OnItemCommand="ctlFormViewWHT_ItemCommand" OnItemInserting="ctlFormViewWHT_ItemInserting"
                                    OnItemUpdating="ctlFormViewWHT_ItemUpdating" OnModeChanging="ctlFormViewWHT_ModeChanging">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblWHTCode" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("WHTCode") %>'></asp:Label>
                                                    <asp:Label ID="lblWHTReq" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    
                                                    <asp:TextBox ID="ctlTxtWHTCode" runat="server" Text='<%# Bind("WHTCode") %>' MaxLength="20" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblWHTName" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("WHTName") %>'></asp:Label>
                                                    <asp:Label ID="Label3" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTxtWHTName" runat="server" Text='<%# Bind("WHTName") %>' MaxLength="100" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblRate" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Rate") %>'></asp:Label>
                                                    <asp:Label ID="Label1" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTxtRate" runat="server" Text='<%# Bind("Rate") %>' OnKeyPress="return(currencyFormat(this, ',', '.', event, 6));"
								                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();"  />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblChkActive" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                                    
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblSeq" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Seq") %>'></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSeq" runat="server" Text='<%# Bind("Seq") %>' />
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ctlSeqFilteredTextBoxExtender" runat="server"
                                                    TargetControlID="ctlSeq" FilterType="Numbers" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        CommandName="Update" Text="Update" ToolTip='<%# GetProgramMessage("Update") %>'>
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'>
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblWHTCode" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("WHTCode") %>'></asp:Label>
                                                    <asp:Label ID="lblWHTReq" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTxtWHTCode" runat="server" MaxLength="20" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblWHTName" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Description") %>'></asp:Label>
                                                    <asp:Label ID="lblWHTTYPEReq" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTxtWHTName" runat="server" MaxLength="100" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblRate" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Rate") %>'></asp:Label>
                                                    <asp:Label ID="Label2" runat="server" Text="*" Style="color: Red;"></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlTxtRate" runat="server" OnKeyPress="return(currencyFormat(this, ',', '.', event, 6));"
								                    OnKeyDown="disablePasteOption();" OnKeyUp="disablePasteOption();" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="chkActiveLabel" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Active") %>'></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ctlLblSeq" SkinID="SkFieldCaptionLabel" runat="server" Text='<%# GetProgramMessage("Seq") %>'></asp:Label>&nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSeq" runat="server"/>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ctlSeqFilteredTextBoxExtender" runat="server"
                                                    TargetControlID="ctlSeq" FilterType="Numbers" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        CommandName="Insert" Text="Insert" ToolTip='<%# GetProgramMessage("Insert") %>'>
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        CommandName="Cancel" Text="Cancel" ToolTip='<%# GetProgramMessage("Cancel") %>'>
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:FormView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <font color="red">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="WHT.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlModalPopupWHT" runat="server" TargetControlID="lnkDummy"
        PopupControlID="panelFormView" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
        DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlLblFormViewHeader" />
</asp:Content>
