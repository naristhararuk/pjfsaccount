<%@ Page Language="C#" Title="" AutoEventWireup="true" MasterPageFile="~/PopupMasterPage.Master"  
CodeBehind="ProgramSearchPopup.aspx.cs" EnableTheming="true" StylesheetTheme="Default"
Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.ProgramSearchPopup" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
<div align="center">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>

    <asp:Panel ID="pnProgramSearch" runat="server" Width="600px" BackColor="White">
        <asp:Panel ID="pnProgramSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchProgram" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlProgramName" runat="server" Text='$Program Name$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 30%">
                                <asp:TextBox ID="txtCompanyName" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtRoleId" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtLanguageId" runat="server" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
                    <ss:BaseGridView ID="ctlProgramGrid" runat="server" AutoGenerateColumns="False" OnRequestData="RequestData"
                        OnRequestCount="RequestCount" ReadOnly="true" EnableInsert="false" EnableViewState="true"
                        DataKeyNames="ProgramId,ProgramLangId" AllowPaging="true" CssClass="Grid" OnDataBound="ctlProgramGrid_DataBound"
                        Width="99%">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ShowHeader="false" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Program Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlProgram" runat="server" Text='<%# Eval("ProgramName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                                </ItemTemplate>
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
                                    <asp:ImageButton runat="server" ID="ctlSubmit" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" />
                                </td>
                                <td valign="middle">
                                    |
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
</div>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>