<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master"
    CodeBehind="ForeignPerdiemRateProfile.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.ForeignPerdiemRateProfile"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/ForeignPerdiemRateProfileEditor.ascx" TagName="ForeignPerdiemRateProfileEditor"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ForeignPerdiemRateProfileDetailEditor.ascx" TagName="ForeignPerdiemRateProfileDetailEditor"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ForeignPerdiemRateProfileCountryEditor.ascx" TagName="ForeignPerdiemRateProfileCountryEditor"
    TagPrefix="uc4" %>    
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 30%">
                <fieldset style="width: 365px; float: left" id="fdsSearch" class="table">
                    <table width="400px" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 125px">
                                <asp:Label ID="ctlPerdiemProfileNameLabel" runat="server" Text="$Perdiem Profile Name$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 200px">
                                <asp:TextBox ID="ctlPerdiemProfileName" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="20"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 125px">
                                <asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 200px">
                                <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" Width="200" MaxLength="100"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                    OnClick="ctlSearch_Click" Style="display: inline" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc1:ForeignPerdiemRateProfileEditor ID="ctlForeignPerdiemRateProfileEditor" runat="server"
                    ShowScrollBar="true" />
                <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <br />
                        <table width="100%" class="table">
                            <tr>
                                <td>
                                    <ss:BaseGridView ID="ctlForeignPerdiemRateProfileGrid" runat="server" AutoGenerateColumns="false"
                                        CssClass="Grid" AllowSorting="true" AllowPaging="true" DataKeyNames="PerdiemProfileID"
                                        SelectedRowStyle-BackColor="#6699FF" OnRowCommand="ctlForeignPerdiemRateProfileGrid_RowCommand"
                                        OnRequestCount="RequestCount" OnRequestData="RequestData" Width="100%" HorizontalAlign="Left">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="PerdiemProfileName" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="PerdiemProfileName">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ctlPerdiemProfileName" runat="server" Text='<%# Bind("PerdiemProfileName") %>'
                                                        SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>'
                                                        SkinID="SkGeneralLabel" Mode="Encode"></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ctlDetailLink" runat="server" Text="Perdiem Rate" CommandName="Detail" /> &nbsp;&nbsp;
                                                    <asp:LinkButton ID="ctlCountryLink" runat="server" Text="Country" CommandName="Country" />&nbsp;
                                                    <asp:ImageButton runat="server" ID="ctlEdit" ToolTip="Edit" SkinID="SkCtlGridEdit"
                                                        CausesValidation="False" CommandName="ForeignPerdiemRateProfileEdit" />
                                                    <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkCtlGridDelete"
                                                        OnClientClick="return confirm('Are you sure delete this row');" CausesValidation="False"
                                                        CommandName="ForeignPerdiemRateProfileDelete" />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 60%">
                                    <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkAddButton" OnClick="ctlAdd_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="foreignPerdiemRateProfileCode" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelDetailGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="foreignPerdiemRateProfileCode2" runat="server" />
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelDetailGrid"
                                        DynamicLayout="true" EnableViewState="False">
                                        <ProgressTemplate>
                                            <uc3:SCGLoading id="SCGLoading4" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlDetailGrid" Width="100%" Visible="false" runat="server" AutoGenerateColumns="false"
                                        CssClass="table" DataKeyNames="PerdiemRateID" ReadOnly="true" OnRowCommand="ctlDetailGrid_RowCommand"
                                        SelectedRowStyle-BackColor="#6699FF" OnDataBound="ctlDetailGrid_DataBound" OnPageIndexChanged="ctlDetailGrid_PageIndexChanged">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Personal Level" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="PersonalLevel">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlPersonalLevel" runat="server" Text='<%# Bind("PersonalLevel") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Zone" HeaderStyle-HorizontalAlign="Center" SortExpression="Zone">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlZone" runat="server" Text='<%# Bind("ZoneName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Official Rate" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="OfficialRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlOfficialRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OfficialPerdiemRate", "{0:#,##0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra Rate" HeaderStyle-HorizontalAlign="Center" SortExpression="ExtraRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlExtraRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExtraPerdiemRate", "{0:#,##0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="International Staff Rate" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="InternationalStaffRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlInternationalStaffRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InternationalStaffPerdiemRate", "{0:#,##0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SCG Staff Rate" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="SCGStaffRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlSCGStaffRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SCGStaffPerdiemRate", "{0:#,##0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctlActive2" runat="server" Enabled="false" Checked='<%# Bind("Active")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="ctlEditDetail" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Edit") %>' CommandName="DetailEdit" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ctlDeleteDetail" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("Delete") %>' CommandName="DetailDelete" OnClientClick="return confirm('Are you sure delete this row');" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                    <div id="ctlDetailTools" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton runat="server" ID="ctlAddDetail" OnClick="ctlAddDetail_Click" SkinID="SkCtlFormNewRow" />
                                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                        OnClick="ctlCancel_Click" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <uc2:ForeignPerdiemRateProfileDetailEditor id="ctlForeignPerdiemRateProfileDetailEditor"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="ctlUpdatePanelCountryGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="foreignPerdiemRateProfileCode3" runat="server" />
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelCountryGrid"
                                        DynamicLayout="true" EnableViewState="False">
                                        <ProgressTemplate>
                                            <uc3:SCGLoading id="SCGLoading5" runat="server" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <ss:BaseGridView ID="ctlCountryGrid" Width="60%" Visible="false" runat="server"
                                        AutoGenerateColumns="false" CssClass="table" DataKeyNames="ID" ReadOnly="true"
                                        OnRowCommand="ctlCountryGrid_RowCommand" SelectedRowStyle-BackColor="#6699FF"
                                        OnDataBound="ctlCountryGrid_DataBound" OnPageIndexChanged="ctlCountryGrid_PageIndexChanged">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                              <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Zone" HeaderStyle-HorizontalAlign="Center" SortExpression="ZoneID">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlZoneName" runat="server" Text='<%# Bind("ZoneName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country Name" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="CountryName">
                                                <ItemTemplate>
                                                    <asp:Label ID="ctlCountryName" runat="server" Text='<%# Bind("CountryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </ss:BaseGridView>
                                    <div id="ctlCountryTools" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton runat="server" ID="ctlAddCountry" OnClick="ctlAddCountry_Click"
                                                        SkinID="SkCtlFormNewRow" />
                                                    <asp:ImageButton ID="ctlDeleteCountry" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                        OnClick="ctlDeleteCountry_Click" Text="Cancel" OnClientClick="return confirm('Are you sure delete this row');"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <uc4:ForeignPerdiemRateProfileCountryEditor id="ctlForeignPerdiemRateProfileCountryEditor"
                    runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
