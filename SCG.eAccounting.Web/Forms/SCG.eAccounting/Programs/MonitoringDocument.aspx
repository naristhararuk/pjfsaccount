<%@ Page Title="Monitoring Document" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master"
    CodeBehind="MonitoringDocument.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.MonitoringDocument"
    StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table width="100%">
        <tr>
            <td>
                <ss:BaseGridView ID="ctlMonitoringDocGrid" runat="server" AutoGenerateColumns="False"
                    ReadOnly="true" EnableInsert="False" DataKeyNames="WorkflowID , Type , DocumentNo ,CacheAttachment ,CacheBoxID"
                    CssClass="Grid" Width="100%" InsertRowCount="1" AllowPaging="true" AllowSorting="true"
                    OnRequestData="ctlMonitoringDocGrid_RequestData" OnRowCommand="ctlMonitoringDocGrid_RowCommand"
                    OnRequestCount="ctlMonitoringDocGrid_RequestCount" OnRowDataBound="ctlMonitoringDocGrid_RowDataBound">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="Attachment" HeaderStyle-HorizontalAlign="Center" SortExpression="Attachment"
                            HeaderStyle-Width="3%" HeaderStyle-Height="80px">
                            <ItemTemplate>
                                <asp:Image ID="ctlAttach" runat="server" SkinID="SkAttachButton" Visible="false" />
                                <asp:Image ID="ctlFile" runat="server" SkinID="SkFileButton" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="เลขที่เอกสาร" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="DocumentNo" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="ctlRequestNo" runat="server" SkinID="SkCtlLinkButton" Text='<%# Bind("DocumentNo") %>'
                                    CommandName="PopupDocument"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="เลขที่ใบเบิก" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="ReferenceNo" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlReferenceNo" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# Bind("ReferenceNo") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="วันที่เอกสาร" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="DocumentDate" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlCreateDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel"
                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("DocumentDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="วันที่อนุมัติ" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="ApproveDate" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlApproveDate" Mode="Encode" runat="server" SkinID="SkCodeLabel"
                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ApproveDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center" SortExpression="CacheCurrentStateName"
                            HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlStatus" Mode="Encode" runat="server" SkinID="SkCalendarLabel"
                                    Text='<%# Bind("CacheCurrentStateName") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="เรื่อง" HeaderStyle-HorizontalAlign="Center" SortExpression="Subject"
                            HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlSubject" Mode="Encode" runat="server" SkinID="SkCalendarLabel"
                                    Text='<%# Bind("Subject") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ผู้ออกเอกสาร" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CacheCreatorName" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlCreator" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# Bind("CacheCreatorName")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ผู้ขอเบิก" HeaderStyle-HorizontalAlign="Center" SortExpression="CacheRequesterName"
                            HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlRequester" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# Bind("CacheRequesterName")%>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="จำนวนเงิน" HeaderStyle-HorizontalAlign="Center" SortExpression="CacheAmountLocalCurrency"
                            HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmountLocalCurrency" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "CacheAmountLocalCurrency", "{0:#,##0.00;(#,##0.00);}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="จำนวนเงิน(สกุลเงินหลัก)" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CacheAmountMainCurrency" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmountMainCurrency" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "CacheAmountMainCurrency", "{0:#,##0.00;(#,##0.00);}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="จำนวนเงิน(บาท)" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CacheAmountTHB" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlAmount" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "CacheAmountTHB", "{0:#,##0.00;(#,##0.00);}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="วันที่รับเอกสาร" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="ReceiveDocumentDate" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Literal ID="ctlReceiveDate" Mode="Encode" runat="server" SkinID="SkCodeLabel"
                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ReceiveDocumentDate")) %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VerifyWithImage" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="IsVerifyWithImage" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                    <asp:CheckBox ID="ctlIsVerifyWithImage" Checked='<%# Bind("IsVerifyWithImage") %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
            </td>
        </tr>
    </table>
</asp:Content>
