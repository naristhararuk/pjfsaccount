<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Holiday.ascx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.Holiday"  EnableTheming="true" %>

<%@ Register Src="~/UserControls/HolidayEditor.ascx" TagName="HolidayEditor" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc2" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">--%>
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 45%">
                <fieldset id="fdsSearch" class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 40%">
                                <asp:Label ID="ctlYearLabel" runat="server" Text="$Year$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlYear" MaxLength="20" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
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
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelHolidayProfileGridview" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHolidayProfileGridview"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc2:scgloading id="SCGLoading1" runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <ss:BaseGridView ID="ctlHolidayProfileGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="table" AllowSorting="true" AllowPaging="true" DataKeyNames="Id" EnableInsert="False"
                            ReadOnly="true" OnRowCommand="ctlHolidayProfileGrid_RowCommand" OnRowDataBound="ctlHolidayProfileGrid_RowDataBound"
                            OnRequestCount="RequestCount" OnRequestData="RequestData" SelectedRowStyle-BackColor="#6699FF"
                            OnDataBound="ctlHolidayProfileGrid_DataBound" OnPageIndexChanged="ctlHolidayProfileGrid_PageIndexChanged"
                            Width="55%">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                       <%--         <asp:TemplateField HeaderText="Id" HeaderStyle-HorizontalAlign="Center" SortExpression="Year"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlIdLabel" runat="server" Text='<%# Bind("Id") %>' Mode="Encode" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Year" HeaderStyle-HorizontalAlign="Center" SortExpression="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlYearLabel" runat="server" Text='<%# Bind("Year") %>' Mode="Encode" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approve" HeaderStyle-HorizontalAlign="Center" SortExpression="IsApprove">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlIsApprove" Checked='<%# Bind("IsApprove") %>' runat="server"
                                            Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="ctlDetailLink" runat="server" Text="Detail" CommandName="HolidayProfileDetail" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ctlEdit" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="HolidayProfileEdit" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                        ToolTip='<%# GetProgramMessage("Delete") %>' OnClientClick="return confirm('Are you sure delete this row');"
                                                        CommandName="HolidayProfileDelete" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                     <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" Width="250px"/>
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                        <div id="divButton" runat="server" style="vertical-align: middle;">
                            <table style="text-align: center;">
                                <tr>
                                    <td>
                                        <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="ctlCopy" SkinID="SkCopyButton" OnClick="ctlCopy_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="ctlHolidayProfileFormPanel" runat="server" Style="display: block"
                    CssClass="modalPopup" Width="500px">
                    <asp:Panel ID="ctlHolidayProfileFormPanelHeader" runat="server" Style="cursor: move;
                        background-color: #DDDDDD; border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture1" runat="server" SkinID="SkCtlLabel" Text="Manage Holiday Profile Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ctlUpdatePanelHolidayProfileForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelHolidayProfileFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHolidayProfileForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc2:SCGLoading ID="SCGLoading2" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlHolidayProfileForm" runat="server" DataKeyNames="Id" OnItemCommand="ctlHolidayProfileForm_ItemCommand"
                                                OnItemInserting="ctlHolidayProfileForm_ItemInserting" OnItemUpdating="ctlHolidayProfileForm_ItemUpdating"
                                                OnModeChanging="ctlHolidayProfileForm_ModeChanging" OnDataBound="ctlHolidayProfileForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left" style="width: 40%">
                                                                <%# GetProgramMessage("Year")%>
                                                                <font color="red">
                                                                    <asp:Label ID="ctlYearRequired" runat="server" Text="*"></asp:Label></font>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="ctlYear" SkinID="SkFieldCaptionLabel" runat="server" Width="252px">
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Approve")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlApprove" runat="server" Checked='<%# Bind("IsApprove") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Update") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Update" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Cancel") %>' CommandName="Cancel" Text="Cancel">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Year")%><font color="red"><asp:Label ID="ctlYearRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlYear" SkinID="SkCtlDropDownList" runat="server" Width="252px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Approve")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlApprove" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
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
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="HolidayProfile.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="ctlCopyFormPanel" runat="server" Style="display: block" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlCopyFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCapture2" runat="server" SkinID="SkCtlLabel" Text="Manage Copy Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    <asp:UpdatePanel ID="ctlUpdatePanelCopyForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelCopyFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCopyForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc2:SCGLoading ID="SCGLoading3" runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlCopyForm" runat="server" DataKeyNames="Id" OnItemCommand="ctlCopyForm_ItemCommand"
                                                OnItemInserting="ctlCopyForm_ItemInserting" OnModeChanging="ctlCopyForm_ModeChanging" OnDataBound="ctlCopyForm_DataBound">
                                                <InsertItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("YearFrom")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                               <asp:DropDownList ID="ctlYearFrom" SkinID="SkCtlDropDownList" runat="server" Width="252px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("YearTo")%>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ctlYearTo" SkinID="SkCtlDropDownList" runat="server" Width="252px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("Insert") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
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
                                                <spring:ValidationSummary ID="ValidationSummary2" runat="server" Provider="Copy.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:LinkButton ID="lnkDummy1" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkDummy1"
                    PopupControlID="ctlHolidayProfileFormPanel" BackgroundCssClass="modalBackground"
                    CancelControlID="lnkDummy1" RepositionMode="None" PopupDragHandleControlID="ctlHolodayProfileFormPanelHeader" />
                <asp:LinkButton ID="lnkDummy2" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource2" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkDummy2"
                    PopupControlID="ctlCopyFormPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy2"
                    RepositionMode="None" PopupDragHandleControlID="ctlCopyFormPanelHeader" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelHolidayGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="ctlHolidayProfileIDHidden" runat="server" />
                                    <asp:HiddenField ID="ctlHolidayProfileYearHidden" runat="server" />
                                    <asp:UpdateProgress ID="ctlUpdateProgressHolidayGrid" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelHolidayGrid"
                                        DynamicLayout="true" EnableViewState="False">
                                        <ProgressTemplate>
                                            <uc2:SCGLoading ID="SCGLoading4" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlHolidayGrid" Width="55%" Visible="false" runat="server" AutoGenerateColumns="false"
                                        CssClass="table" DataKeyNames="Id" ReadOnly="true" AllowSorting="true" AllowPaging="true"
                                        OnRowDataBound="ctlHolidayGrid_RowDataBound" OnRequestCount="RequestHolidayCount" OnRequestData="RequestHolidayData" OnRowCommand="ctlHolidayGrid_RowCommand"
                                        SelectedRowStyle-BackColor="#6699FF" OnDataBound="ctlHolidayGrid_DataBound" OnPageIndexChanged="ctlHolidayGrid_PageIndexChanged">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" SortExpression="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlDateLabel" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("Date")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="35%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="ctlHolidayDelete" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Delete") %>' OnClientClick="return confirm('Are you sure delete this row');"
                                                                    CommandName="HolidayDelete" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                    <div id="ctlHolidayTools" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton runat="server" ID="ctlAddHoliday" OnClick="ctlAddHoliday_Click"
                                                        SkinID="SkCtlFormNewRow" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <uc1:HolidayEditor ID="ctlHolidayEditor" runat="server" />
            </td>
        </tr>
    </table>
<%--</asp:Content>--%>
