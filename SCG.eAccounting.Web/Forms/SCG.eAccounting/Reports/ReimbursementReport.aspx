<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="ReimbursementReport.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports.ReimbursementReport"
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/DropdownList/SCG.DB/PB.ascx" TagName="PBDropdown"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelMaintainRateGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelMaintainRateGrid"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading id="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset id="ctlFieldSetDetailGridView" style="width: 60%" class="table">
                <legend id="legSearch" style="color: #4E9DDF" class="table">
                    <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
                </legend>
                <table width="600">
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlPbLabel" runat="server" Text="PB"></asp:Label>
                        </td>
                        <td colspan="3" align="left">
                            <asp:DropDownList ID="ctlPbDropdownList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlRequestDateFrom" runat="server" Text="$RequestDateFrom$"></asp:Label>
                        </td>
                        <td align="left">
                            <uc2:Calendar ID="ctlRequestDateFromPicker" runat="server" width="100" />
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlRequestDateTo" runat="server" Text="$RequestDateTo$"></asp:Label>
                        </td>
                        <td align="left">
                            <uc2:Calendar ID="ctlRequestDateToPicker" runat="server" width="100" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlPaidDateFrom" runat="server" Text="$PaidDateFrom$"></asp:Label>
                        </td>
                        <td align="left">
                            <uc2:Calendar ID="ctlPaidDateFromPicker" runat="server" width="100" />
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlPaidDateTo" runat="server" Text="$PaidDateTo$"></asp:Label>
                        </td>
                        <td align="left">
                            <uc2:Calendar ID="ctlPaidDateToPicker" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlRequestNoFrom" runat="server" Text="$RequestNoFrom$"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="ctlRequestNoFromTextbox" runat="server"   SkinID="SkGeneralTextBox" MaxLength="15" ></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="ctlRequestNoTo" runat="server" Text="$RequestNoTo$"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="ctlRequestNoToTextbox" runat="server"  SkinID="SkGeneralTextBox" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:CheckBox ID="ctlUnmarkDocument" runat="server" Text="$IncludeUnMarkDocument$"
                                Checked="true" /><br />
                            <asp:CheckBox ID="ctlMarkedDocument" runat="server" Text="$IncludeMarkedDocument$" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:ImageButton ID="cltSearchBtn" runat="server" SkinID="SkSearchButton" OnClick="cltSearchBtn_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table width="100%" class="table">
                <tr>
                    <td>
                        <ss:BaseGridView ID="ctlReport" runat="server" AutoGenerateColumns="false" CssClass="table"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="Pbid" SelectedRowStyle-BackColor="#6699FF"
                            OnRequestCount="RequestCount" OnDataBound="ctlReport_DataBound" OnRowDataBound="ctlReport_RowDataBound"
                            OnRequestData="RequestData" Width="100%" HorizontalAlign="Left" PageSize="200">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="4%" SortExpression="Mark">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlMark" runat="server" Checked='<%# Bind("Mark") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="4%"/>
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Request No" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="10%" SortExpression="RequestNo">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlRequestNo" Mode="Encode" runat="server" Text='<%# Bind("RequestNo") %>'></asp:Literal>
                                        <asp:HiddenField ID="ctlDocumentID" runat="server" Value='<%# Bind("DocumentID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>

<%--                                <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlDateLabel" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDate")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" HorizontalAlign="Center" />
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="FI DOC" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" SortExpression="FI_DOC" >
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlFIDoc" Mode="Encode" runat="server" Text='<%# Bind("FI_DOC") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" SortExpression="Subject">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlSubject" Mode="Encode" runat="server" Text='<%# Bind("Subject") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" SortExpression="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlAmount" Mode="Encode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Amount","{0:#,##0.00;(#,##0.00); }") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" SortExpression="Currency">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlCurrency" Mode="Encode" runat="server" Text='<%# Bind("Currency") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Main Currency" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" SortExpression="AmountMainCurrency">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlMainCurrency" Mode="Encode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AmountMainCurrency","{0:#,##0.00;(#,##0.00); }") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(THB)" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" SortExpression="AmountTHB">
                                    <ItemTemplate>
                                        <asp:Label ID="ctlAmountTHB" Mode="Encode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AmountTHB","{0:#,##0.00;(#,##0.00); }") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" SortExpression="PaidDate">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlPaidDate" Mode="Encode" runat="server" Text='<%# Bind("PaidDate","{0:d/M/yyyy HH:mm:ss}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </ss:BaseGridView>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="ctlUpdateMarkBtn" runat="server" Text="Update Mark" Width="100" OnClick="ctlUpdateMarkBtn_Click" />
                        <asp:Button ID="ctlPrintAllBtn" runat="server" Text="Print Report" Width="100" OnClick="ctlPrintAllBtn_Click" />
                        <asp:HiddenField ID="ctlMarkDocList" runat="server" />
                        <asp:HiddenField ID="ctlUnMarkDocList" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
