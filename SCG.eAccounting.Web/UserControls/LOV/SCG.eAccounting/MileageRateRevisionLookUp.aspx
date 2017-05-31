<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
    CodeBehind="MileageRateRevisionLookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting.MileageRateRevisionLookUp"
    StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/PopupCallback.ascx" TagName="PopupCallback"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <table id="ctlContainerTable" runat="server">
        <tr>
            <td>
                <div align="center">
                    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
                    <asp:Panel ID="pnAdvanceSearch" runat="server" Width="100%" BackColor="White">
                        <asp:Panel ID="pnAdvanceSearchHeader" CssClass="table" runat="server" Style="cursor: move;
                            border: solid 1px Gray; color: Black">
                            <div>
                                <p>
                                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="100%"></asp:Label>
                                </p>
                            </div>
                        </asp:Panel>
                        <table width="100%">
                            <tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ctlFileUploadText" runat="server" SkinID="SkFieldCaptionLabel" Text="Attach File"></asp:Label>
                                        &nbsp;:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="ctlFileUpload" runat="server" Width="250px" />
                                    </td>
                                    <td align="center" style="width: 180px">
                                        <asp:ImageButton runat="server" ID="ctlAttach" SkinID="SkAddButton" ToolTip="$Attach$"
                                            OnClick="ctlAttach_OnClick" />
                                        <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
                                    </td>
                                </tr>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <font color="red">
                                        <spring:ValidationSummary ID="ctlValidationImPortExcel" runat="server" Provider="ImPortExcel.Error" />
                                    </font>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
