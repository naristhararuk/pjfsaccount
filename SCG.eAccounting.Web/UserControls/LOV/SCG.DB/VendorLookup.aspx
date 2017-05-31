<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
    CodeBehind="VendorLookup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.VendorLookup"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <ss:InlineScript ID="InlineScript1" runat="server">
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
    </ss:InlineScript>
    <table>
        <tr align="center">
            <td>
                <asp:Panel ID="pnVendorSearch" runat="server" Width="800" BackColor="White" Style="display: block;">
        <asp:Panel ID="pnVendorSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchVendor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                        <legend id="legSearch" style="color: #4E9DDF" class="table">
                            <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                        <table width="100%" border="0" class="table">
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="lblTaxId" runat="server" Text='$TaxID$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlTaxId" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="13"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="lblBranch" runat="server" Text='$Branch$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlBranch" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="lblVendorCode" runat="server" Text='$Vendor Code$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlVendorCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlCountryName" runat="server" Text='$Vendor Name$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlVendorName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"></asp:TextBox>
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
                        <ss:BaseGridView ID="ctlVendorGrid" runat="server" AutoGenerateColumns="False" OnRequestCount="RequestCount"
                            OnRequestData="RequestData" ReadOnly="true" EnableViewState="true" DataKeyNames="VendorID"
                            CssClass="Grid" Width="99%" OnDataBound="ctlVendorGrid_DataBound" AllowPaging="true"
                            AllowSorting="true" OnRowCommand="ctlVendorGrid_RowCommand" HeaderStyle-CssClass="GridHeader">
                            <AlternatingRowStyle CssClass="GridItem" />
                            <RowStyle CssClass="GridAltItem" />
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
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ctlTaxSelect" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False"
                                            CommandName="Select" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Code" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="VendorCode">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlGridVendorCode" runat="server" Text='<%# Eval("VendorCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="VendorName">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlGridVendorName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="TaxID1" HeaderStyle-HorizontalAlign="Center" SortExpression="VendorTaxCode">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlGridTaxID" runat="server" Text='<%# Eval("VendorTaxCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlStreet" runat="server" Text='<%# Eval("Street") %>' />
                                        <asp:Label ID="ctlCountry" runat="server" Text='<%# Eval("Country") %>' />
                                        <asp:Label ID="ctlCity" runat="server" Text='<%# Eval("City") %>' />
                                        <asp:Label ID="ctlPostalCode" runat="server" Text='<%# Eval("PostalCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch" HeaderStyle-HorizontalAlign="Center" SortExpression="VendorBranch">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlGridBranch" runat="server" Text='<%# Eval("VendorBranch") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
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
                                        <asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlSubmit_Click" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="ctlLblLine" runat="server" Text="|"></asp:Label>
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
                    <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
    </asp:Panel>
            </td>
        </tr>
    </table>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
