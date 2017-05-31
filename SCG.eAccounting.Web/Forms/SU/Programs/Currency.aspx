<%@ Page Title="" Language="C#" StylesheetTheme="Default" MasterPageFile="~/ProgramsPages.Master" EnableTheming="true"
    AutoEventWireup="true" CodeBehind="Currency.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.Currency" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/JClock.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 35%">
                <fieldset style="width: 90%" id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="right" style="width: 40%">
                                <asp:Label ID="ctlSymbolLabel" runat="server" Text="$Symbol$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlSymbol" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">
                                <asp:Label ID="ctrDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctrDescription" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left">
                <asp:ImageButton runat="server" ID="ctlCurrencySearch" ToolTip="Search" SkinID="SkCtlQuery"
                   OnClick="ctlCurrencySearch_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlCurrencyUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlCurrencyUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:HiddenField ID="ctlCurrencyIdHidden" runat="server" />
            <ss:BaseGridView ID="ctlCurrencyGrid" runat="server" AutoGenerateColumns="false"
                CssClass="table" Width="100%" AllowPaging="true" DataKeyNames="CurrencyID" EnableInsert="False"
                AllowSorting="true" ReadOnly="true" OnRequestCount="RequestCount" OnRequestData="RequestData"
                SelectedRowStyle-BackColor="#6699FF" OnDataBound="ctlCurrencyGrid_Databound"
                OnPageIndexChanged="ctlCurrencyGrid_PageIndexChanged" OnRowCommand="ctlCurrencyGrid_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlSelectAllChk" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelectChk" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Symbol" HeaderStyle-HorizontalAlign="Center" SortExpression="Symbol">
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlSymbolLabel" runat="server" Text='<%# Bind("Symbol") %>' CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="31%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:Label ID="ctlCommentLabel" runat="server" Text='<%# Bind("Description") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlProgramActive" Checked='<%# Bind("Active") %>' runat="server"
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="UserEdit" />
                        </ItemTemplate>
                        <ItemStyle Width="50px" HorizontalAlign="Center" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="UserDelete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                        runat="server"></asp:Label>
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left">
                <table style="text-align: center;">
                    <tr>
                        <td>
                            <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                        </td>
                        <td>
                            <span class="spanSeparator">| </span>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="ctlExchangeRateUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelExchangeRateFormProgress" runat="server" AssociatedUpdatePanelID="ctlExchangeRateUpdatePanel"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 100%; text-align: Center" id="ctlExchangeFds" runat="server"
                visible="false">
                <legend id="ctlLegendDetailGridView" style="color: #4E9DDF">
                    <asp:Label ID="ctlExchangeGridHeader" runat="server" Text="Manage ExchangeRate Data"></asp:Label></legend>
                <ss:BaseGridView ID="ctlExchangeGrid" runat="server" AutoGenerateColumns="false"
                    Width="98%" OnRowCommand="ctlExchangeGrid_RowCommand" EnableInsert="false" AllowSorting="true"
                    CssClass="table" DataKeyNames="ExchangeRateID" OnDataBound="ctlExchangeGrid_Databound"
                    ReadOnly="true" EmptyDataRowStyle-ForeColor="red" EmptyDataRowStyle-HorizontalAlign="Center"
                    EmptyDataRowStyle-BorderWidth="0" OnRequestCount="RequestExchangeCount" OnRequestData="RequestExchangeData"
                    AllowPaging="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlSelectAllChk" runat="server" onclick="javascript:validateCheckBox2(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelectChk" runat="server" onclick="javascript:validateCheckBox2(this, '1');" />
                            </ItemTemplate>
                            <ItemStyle Width="25px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Date" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="FromDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlFromDateText" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("FromDate").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="13%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ToDateText" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="ToDate">
                            <ItemTemplate>
                                <asp:Label ID="ctlTodate" runat="server" Width="95%" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ToDate").ToString()) %>' />
                            </ItemTemplate>
                            <ItemStyle Width="13%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Buy Rate" HeaderStyle-HorizontalAlign="Center" SortExpression="BuyRate">
                            <ItemTemplate>
                                <asp:Label ID="ctlBuyRate" runat="server" Width="95%" Text='<%# Eval("BuyRate") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="13%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sell Rate" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="SellRate">
                            <ItemTemplate>
                                <asp:Label ID="ctlSellRate" runat="server" Width="95%" Text='<%# Eval("SellRate") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="13%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center" SortExpression="Comment">
                            <ItemTemplate>
                                <asp:Label ID="ctlComment" runat="server" Width="95%" Text='<%# Eval("Comment") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Enabled="false" Checked='<%# Eval("Active") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                    ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="UserEdit" />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                            runat="server"></asp:Label>
                    </EmptyDataTemplate>
                    <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                </ss:BaseGridView>
                <div id="ctlExchangeButton" runat="server" align="left" visible="False">
                    <table style="text-align: center;">
                        <tr>
                            <td>
                                <asp:ImageButton runat="server" ID="ctlExchangeInsert" SkinID="SkCtlFormNewRow" OnClick="ctlExchangeAddNew_Click"
                                    ToolTip="Add" />
                            </td>
                            <td>
                                <span class="spanSeparator">| </span>
                            </td>
                            <td>
                                <asp:ImageButton ID="ctlExchangeDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlExchangeDelete_Click"
                                    ToolTip="Delete" />
                            </td>
                            <td>
                                <span class="spanSeparator">| </span>
                            </td>
                            <td>
                                <asp:ImageButton ID="ctlExchangeCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlExchangeCancel_Click"
                                    ToolTip="Cancel" />
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="ctlCurrencyFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="ctlCurrencyFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text="Manage Currency Data" Width="160px"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelCurrencyForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="UpdatePanelCurrencyFormProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelCurrencyForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlCurrencyForm" runat="server" DataKeyNames="CurrencyID" OnItemCommand="ctlCurrencyForm_ItemCommand"
                                    OnItemInserting="ctlCurrencyForm_ItemInserting" OnItemUpdating="ctlCurrencyForm_ItemUpdating"
                                    OnModeChanging="ctlCurrencyForm_ModeChanging" OnDataBound="ctlCurrencyForm_DataBound">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Symbol") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSymbol" runat="server" MaxLength="100" SkinID="SkCtlTextboxLeft"
                                                        Text='<%# Bind("Symbol") %>' Width="250px" />
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine" Height="50px" SkinID="SkCtlTextboxMultiLine"
                                                        onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" Text='<%# Bind("Comment") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Bind("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlCurrencyUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ToolTip='<%# GetProgramMessage("Update") %>' ValidationGroup="ValidateFormView"
                                                        CommandName="Update" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCurrencyCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Symbol") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSymbol" MaxLength="50" runat="server" Width="250px" SkinID="SkCtlTextboxLeft" />
                                                    <font color="red">
                                                        <asp:Label ID="ctlRequired" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine" Height="50px" SkinID="SkCtlTextboxLeft"
                                                        onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlCurrencyUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                        CommandName="Insert" Text="Insert"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlCurrencyCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
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
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Currency.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="ctlExchangeFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
        Width="500px">
        <asp:Panel ID="CtlExchangeFormPanelHerder" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="ctlExchangeFormHeader" runat="server" Text="Manage ExchangeRate Data"
                        Width="160px"></asp:Label>
                </p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelExchangeForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: block;" align="center">
                    <asp:UpdateProgress ID="ctlExchangeUpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelExchangeForm"
                        DynamicLayout="true" EnableViewState="False">
                        <ProgressTemplate>
                            <uc3:SCGLoading ID="SCGLoading4"  runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <asp:FormView ID="ctlExchangeForm" runat="server" DataKeyNames="ExchangeRateID" OnItemCommand="ctlExchangeForm_ItemCommand"
                                    OnItemInserting="ctlExchangeForm_ItemInserting" OnItemUpdating="ctlExchangeForm_ItemUpdating"
                                    OnModeChanging="ctlExchangeForm_ModeChanging" OnDataBound="ctlExchangeForm_DataBound">
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("FromDate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="Calendar1" runat="server" SkinID="SkCtlCalendar" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("FromDate").ToString()) %>' />
                                                    <font color="red">
                                                        <asp:Label ID="Label2" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("ToDate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="Calendar2" runat="server" SkinID="SkCtlCalendar" DateValue='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ToDate").ToString()) %>' />
                                                    <font color="red">
                                                        <asp:Label ID="Label3" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("BuyRate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlBuyRate" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();"
                                                        runat="server" Text='<%# Bind("BuyRate") %>' Width="132px" />
                                                    <font color="red">
                                                        <asp:Label ID="Label4" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("SellRate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSellRate" SkinID="SkCtlTextboxLeft" onkeypress="return isKeyInt();"
                                                        runat="server" Text='<%# Bind("SellRate") %>' Width="132px" />
                                                    <font color="red">
                                                        <asp:Label ID="Label5" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" SkinID="SkCtlTextboxMultiLine" runat="server" TextMode="MultiLine"
                                                        Height="50px" onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" Text='<%# Bind("Comment") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlExchangeUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ToolTip='<%# GetProgramMessage("Update") %>' ValidationGroup="ValidateFormView"
                                                        CommandName="Update" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlExchangeCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("FromDate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="Calendar1" runat="server" SkinID="SkCtlCalendar" />
                                                    <font color="red">
                                                        <asp:Label ID="Label6" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("ToDate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <uc1:Calendar ID="Calendar2" runat="server" SkinID="SkCtlCalendar" />
                                                    <font color="red">
                                                        <asp:Label ID="Label3" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("BuyRate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlBuyRate" onkeypress="return isKeyInt();" runat="server" Width="132px"
                                                        SkinID="SkCtlTextboxLeft" />
                                                    <font color="red">
                                                        <asp:Label ID="Label7" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("SellRate") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlSellRate" onkeypress="return isKeyInt();" runat="server" Width="132px"
                                                        SkinID="SkCtlTextboxLeft" />
                                                    <font color="red">
                                                        <asp:Label ID="Label8" runat="server" Text="*"></asp:Label></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Comment") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="ctlComment" runat="server" TextMode="MultiLine" Height="50px" SkinID="SkCtlTextboxMultiLine"
                                                        onkeypress="return IsMaxLength(this, 500);" onkeyup="return IsMaxLength(this, 500);"
                                                        Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%# GetProgramMessage("Active") %>
                                                    &nbsp;:
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ctlActiveChk" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ImageButton ID="ctlExchangeUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                        ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                        CommandName="Insert" Text="Update"></asp:ImageButton>
                                                    <asp:ImageButton ID="ctlExchangeCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
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
                                    <spring:ValidationSummary ID="ValidationSummary1" runat="server" Provider="Currency.Error" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlCurrencyModalPopupExtender" runat="server"
        TargetControlID="lnkDummy" PopupControlID="ctlCurrencyFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlCurrencyFormPanelHeader" />
    <asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
    <ajaxToolkit:ModalPopupExtender ID="ctlExchangeModalPopupExtender" runat="server"
        TargetControlID="lnkDummy2" PopupControlID="ctlExchangeFormPanel" BackgroundCssClass="modalBackground"
        CancelControlID="lnkDummy2" DropShadow="true" RepositionMode="None" PopupDragHandleControlID="ctlExchangeFormPanelHeader" />
</asp:Content>
