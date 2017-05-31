<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoneySellingRequestLetterGenerator.aspx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.Forms.Interface.Programs.MoneySellingRequestLetterGenerator"
    MasterPageFile="~/ProgramsPages.Master" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx"
    TagName="CompanyTextboxAutoComplete" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc2" %>
<asp:Content ID="ctlContentRm" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelRequestCriteriaRm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <table style="text-align: left" class="table" style="width: 35%">
                    <tr>
                        <td>
                            <asp:Label Text="Company" ID="ctlLabelCompanyRm" runat="server" SkinID="SkCtlLabel"></asp:Label>
                            <asp:Label ID="ctlLableCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                            :
                        </td>
                        <td>
                            <uc1:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoCompleteRm" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnAddCompanyRm" runat="server" Text="Add" SkinID="addButton" OnClick="btnAddCompany_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="250" Height="80" MaxLength="1000" ID="txtComListRm"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Letter ID :" ID="ctlLetterIdLabelRm" runat="server" SkinID="SkCtlLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="ctlLetterIdRm" MaxLength="20" SkinID="SkCtlTextboxCenter"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="ctlChkGeneratedLetterRm" />
                            Include document with Generated Letter
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button runat="server" ID="ctlSearchBtnRm" Text="Search" OnClick="ctlSearchBtn_Click" />
                <font color="red">
                    <spring:ValidationSummary runat="server" ID="ctlvalidationSummaryRm" Provider="Export.Error">
                    </spring:ValidationSummary>
                </font>
            </center>
            <br />
            <div id="divReqGridRm" runat="server">
                <ss:BaseGridView AllowSorting="true" DataKeyNames="LetterNo" runat="server" ID="ctlGridRm"
                    AllowPaging="true" AutoGenerateColumns="false" ReadOnly="true" OnDataBound="ctlReqGrid_DataBound"
                    CssClass="table" OnRowCommand="ctlReqGrid_RowCommand" OnRowDataBound="ctlReqGrid_RowDataBound"
                    EnableInsert="False" OnRequestCount="RequestCount" OnRequestData="RequestData"
                    Width="98%">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <FooterStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeaderRm" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelectRm" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                            </ItemTemplate>
                            <ItemStyle Width="25px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="150" HeaderText="Request No." HeaderStyle-HorizontalAlign="Center"
                            SortExpression="DocumentNo">
                            <ItemTemplate>
                                <asp:Label ID="ctlRequestNoRm" runat="server" Text='<%# Eval("DocumentNo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requester" HeaderStyle-HorizontalAlign="Center" SortExpression="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="ctlRequesterRm" runat="server" Text='<%# Eval("RequestName") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="ctlAmountRm" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Amount", "{0:#,##0.00}") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="200" HeaderText="Letter ID" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="LetterNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlLetterIDRm" runat="server" Text='<%# Eval("LetterNo") %>'
                                    CommandName="Select" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ctlDocumentIDRm" runat="server" Text='<%# Eval("DocumentID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
            </div>
            <br />
            <div id="divGenerateButton" runat="server" style="display:none">
            <table class="table">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Date :"></asp:Label>
                        <asp:Label ID="Label1" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                    </td>
                    <td>
                        <uc2:Calendar ID="ctlDate" runat="server" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="ctlGenerateBtnRm" Text="Generate" OnClick="ctlGenerateBtn_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <font color="red">
                            <spring:ValidationSummary runat="server" ID="ValidationSummary1" Provider="Generate.Error">
                            </spring:ValidationSummary>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
