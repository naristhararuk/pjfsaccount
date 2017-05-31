<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true" CodeBehind="TALookUp.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.TALookUp" 
EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="TALookup.ascx" TagName="TALookup" TagPrefix="uc1" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <asp:Panel ID="pnTADocumentSearch" runat="server" Width="600px" BackColor="White">
        <asp:Panel ID="pnTADocumentSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p><asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <center>
                    <fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
                        <legend id="legSearch" style="color: #4E9DDF" class="table">
                            <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                        <table width="100%" border="0" class="table">
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlTANoLabel" runat="server" Text='$TA No.$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlDocumentNo" runat="server" Text='<%# Bind("DocumentNo") %>' Width="200px"
                                        MaxLength="19"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
					    <td align="right" style="width:20%">
					        <asp:Label ID="ctlAdvanceReferenceLabel" runat="server" Text='$Advance Reference$'></asp:Label> : </td>
					    <td align="left" style="width:30%">
                            <asp:DropDownList ID="ctlAdvanceReference" runat="server" Width="203px"></asp:DropDownList>
					    </td>
				    </tr>--%>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlDescriptionLabel" runat="server" Text='$Description$'></asp:Label>
                                    :
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:TextBox ID="ctlDescription" runat="server" Text='<%# Bind("Description") %>'
                                        Width="200px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <%-- For display "without document(AV, RM, EXP)" radio button --%>
                                <td align="center" colspan="2">
                                    <div id="divWithoutAdvance" runat="server">
                                        <asp:RadioButton runat="server" ID="ctlRadioWithoutAdvance" GroupName="WithoutDocumentRadioGroup"
                                            Text="Search only TA without Advance" /><br />
                                    </div>
                                    <div id="divWithoutRemittance" runat="server">
                                        <asp:RadioButton runat="server" ID="ctlRadioWithoutRemittance" GroupName="WithoutDocumentRadioGroup"
                                            Text="Search only TA without Remittance" /><br />
                                    </div>
                                    <div id="divWithoutExpense" runat="server">
                                        <asp:RadioButton runat="server" ID="ctlRadioWithoutExpense" GroupName="WithoutDocumentRadioGroup"
                                            Text="Search only TA without Expense" /><br />
                                    </div>
                                    <asp:RadioButton runat="server" ID="ctlRadioAllTA" GroupName="WithoutDocumentRadioGroup"
                                        Text="Search All Travelling Document" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:TextBox ID="ctlLanguageId" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="ctlCompanyID" runat="server" />
                                    <asp:HiddenField ID="ctlRequesterID" runat="server" />
                                    <asp:HiddenField ID="ctlTravelBy" runat="server" />
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
                    <ss:BaseGridView ID="ctlTADocumentGridView" runat="server" AutoGenerateColumns="False"
                        OnRequestData="RequestData" ReadOnly="True" EnableInsert="False" OnRequestCount="RequestCount"
                        AllowPaging="True" AllowSorting="True" DataKeyNames="DocumentID" CssClass="table"
                        OnDataBound="ctlTADocumentGrid_DataBound" Width="99%" OnRowCommand="ctlTADocumentGrid_RowCommand"
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
                                    <asp:ImageButton ID="ctlTADocumentSelect" runat="server" SkinID="SkCtlGridSelect"
                                        CausesValidation="False" CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="DocumentNo"
                                HeaderText="TA No.">
                                <ItemTemplate>
                                    <asp:Label ID="ctlDocumentNo" runat="server" Text='<%# Bind("DocumentNo") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Wrap="false" Width="17%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="Subject"
                                HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="ctlSubject" runat="server" Text='<%# Bind("Subject") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Width="47%" Wrap="true" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="FromDate"
                                HeaderText="From Date">
                                <ItemTemplate>
                                    <asp:Label ID="ctlFromDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("FromDate")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="18%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" SortExpression="ToDate" HeaderText="To Date">
                                <ItemTemplate>
                                    <asp:Label ID="ctlToDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ToDate")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="18%" />
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
         <asp:UpdatePanel ID="ctlUpdatePanelValidate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table border="0" width = "600px">
            <tr>
                <font color="red" style="text-align: left" class="table">
                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error">
                    </spring:ValidationSummary>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
    </asp:Panel>

    <uc2:Popupcallback ID="PopupCallback1" runat="server" />  
   
</asp:Content>



