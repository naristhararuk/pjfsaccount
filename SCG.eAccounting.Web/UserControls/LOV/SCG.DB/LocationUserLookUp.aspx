<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="LocationUserLookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationUserLookUp" 
EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="LocationUserLookup.ascx" TagName="LocationUserLookup" TagPrefix="uc1" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <asp:Panel ID="pnLocationSearch" runat="server" Width="600px" BackColor="White">
        <asp:Panel ID="pnLocationSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <fieldset id="ctlFieldSetDetailGridView" style="width: 85%" id="fdsSearch" class="table">
                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="right" style="width: 20%">
                            <asp:HiddenField ID="ctlCompanyID" runat="server" />
                                <asp:Label ID="ctlCompanyLabel" runat="server" Text='$Company$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:DropDownList ID="ctlCompany" runat="server" Width="325px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlLocationCodeLabel" runat="server" Text='$Location Code$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:TextBox ID="ctlLocationCode" runat="server" Text='<%# Bind("LocationCode") %>'
                                    Width="320px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlLocationNameLabel" runat="server" Text='$Location Name$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:TextBox ID="ctlLocationName" runat="server" Text='<%# Bind("LocationName") %>'
                                    Width="320px" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                        DynamicLayout="true" EnableViewState="true">
                        <ProgressTemplate>
                            <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <ss:BaseGridView ID="ctlLocationGridView" runat="server" AutoGenerateColumns="False"
                        OnRequestData="RequestData" ReadOnly="True" EnableInsert="False" OnRequestCount="RequestCount"
                        AllowPaging="True" AllowSorting="true" DataKeyNames="LocationID" CssClass="table"
                        OnDataBound="ctlLocationGrid_DataBound" Width="99%" OnRowCommand="ctlLocationGrid_RowCommand"
                        InsertRowCount="1" SaveButtonID="" HeaderStyle-CssClass="GridHeader">
                        <AlternatingRowStyle CssClass="GridItem" />
                        <RowStyle CssClass="GridAltItem" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ctlLocationSelect" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="LocationCode"
                                HeaderText="Location Code">
                                <ItemTemplate>
                                    <asp:Label ID="ctlLocationCode" runat="server" Text='<%# Bind("LocationCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="DbLocation.Description"
                                HeaderText="Location Name">
                                <ItemTemplate>
                                    <asp:Label ID="ctlLocationName" runat="server" Text='<%# Bind("LocationName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="CompanyName"
                                HeaderText="Company">
                                <ItemTemplate>
                                    <asp:HiddenField ID="ctlHiddenFieldCompanyId" runat="server" Value='<%# Bind("CompanyID") %>' />
                                    <asp:Label ID="ctlCompanyName" runat="server" Text='<%# Bind("CompanyName") %>'   ></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                    </ss:BaseGridView>
                    <div style="text-align: left; width: 98%">
                        <table border="0">
                            <tr>
                                <td valign="middle">
                                    <asp:ImageButton ID="ctlSelect" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlSelect_Click" />
                                </td>
                                <td valign="middle">
                                    <asp:Label ID="ctlLine" runat="server" Text="|"></asp:Label>
                                </td>
                                <td valign="middle">
                                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctlSelect" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>

    <uc2:Popupcallback ID="PopupCallback1" runat="server" />    
</asp:Content>
