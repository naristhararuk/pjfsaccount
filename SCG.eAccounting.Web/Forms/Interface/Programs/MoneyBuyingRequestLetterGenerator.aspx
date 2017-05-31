<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoneyBuyingRequestLetterGenerator.aspx.cs"
    EnableTheming="true" Inherits="SCG.eAccounting.Web.Forms.Interface.Programs.MoneyBuyingRequestLetterGenerator"
    MasterPageFile="~/ProgramsPages.Master" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx"
    TagName="CompanyTextboxAutoComplete" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc2" %>
<asp:Content ID="ctlContent" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelRequestCriteria" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <table style="text-align: left" class="table">
                    <tr>
                        <td>
                            <asp:Label Text="Request Date of Advance " ID="ctlReqDate" runat="server" SkinID="SkCtlLabel"></asp:Label>
                            <asp:Label ID="ctlLabelReqDate" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                            :
                        </td>
                        <td>
                            <uc2:Calendar Id="ctlCalendar" runat="server">
                            </uc2:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Company" ID="ctlLabelCompany" runat="server" SkinID="SkCtlLabel"></asp:Label>
                            <asp:Label ID="ctlLableCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label>
                            :
                        </td>
                        <td>
                            <uc1:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnAddCompany" runat="server" Text="Add" SkinID="SkCtlButton" OnClick="btnAddCompany_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="250" Height="80" MaxLength="1000" ID="txtCompanyList"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Letter ID :" ID="ctlLetterIdLabel" runat="server" SkinID="SkCtlLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="ctlLetterId" MaxLength="20" SkinID="SkCtlTextboxCenter"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="ctlChkGeneratedLetter" />
                            Include document with Generated Letter
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button runat="server" ID="ctlSearchBtn" Text="Search" OnClick="ctlSearchBtn_Click" />
                  <font color="red">
                    <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="Export.Error">
                    </spring:ValidationSummary>
                </font>
            </center>
            <br />
            <div id="divReqGrid" runat="server">
                <ss:BaseGridView runat="server" ID="ctlReqGrid" AllowSorting="true" AllowPaging="true"
                    AutoGenerateColumns="false" ReadOnly="true" OnDataBound="ctlReqGrid_DataBound"
                    OnRowCommand="ctlReqGrid_RowCommand" OnRowDataBound="ctlReqGrid_RowDataBound"
                    CssClass="table" DataKeyNames="LetterNo" EnableInsert="False" OnRequestCount="RequestCount"
                    OnRequestData="RequestData" Width="98%">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <FooterStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                            </ItemTemplate>
                            <ItemStyle Width="25px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "150" HeaderText="Request No." HeaderStyle-HorizontalAlign="Center"
                            SortExpression="DocumentNo">
                            <ItemTemplate>
                                <asp:Label ID="ctlRequestNo" runat="server" Text='<%# Eval("DocumentNo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requester" HeaderStyle-HorizontalAlign="Center" SortExpression="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="ctlRequester" runat="server" Text='<%# Eval("EmployeeName") %>' />
                            </ItemTemplate>
                            <ItemStyle  HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "100" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" SortExpression="Amount">
                            <ItemTemplate>
                                <asp:Label ID="ctlAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Amount","{0:#,##0.00}") %>' />
                            </ItemTemplate>
                            <ItemStyle  HorizontalAlign="Right"/>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "200" HeaderText="Letter ID" HeaderStyle-HorizontalAlign="Center" SortExpression="LetterNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlLetterID" runat="server" Text='<%# Eval("LetterNo") %>' CommandName="ShowLetter" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ctlDocumentID" runat="server" Text='<%# Eval("DocumentID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <br />
                <div id="divGenerateButton" runat="server" style="display:none">
                <asp:Button runat = "server" ID="ctlGenerateBtn" Text="Generate" OnClick="ctlGenerateBtn_Click" />

                <font color="red">
                    <spring:ValidationSummary runat="server" ID="ValidationSummary1" Provider="Generate.Error">
                    </spring:ValidationSummary>
                </font>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
