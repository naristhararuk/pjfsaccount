<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true"
    CodeBehind="TASearch.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.TASearch"
    EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" TagName="CompanyTextboxAutoComplete"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdateProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc5:SCGLoading ID="SCGLoading" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset style="width: 99%;" id="fdsSearch" class="table">
                <asp:Panel ID="pnSearchHeader" runat="server" HorizontalAlign="left" Style="cursor: pointer;
                    font-size: larger; color: Blue">
                    <asp:ImageButton ID="imgToggle" OnClientClick="return false;" runat="server" ImageUrl="~/App_Themes/Default/images/Slide/collapse.jpg"
                        AlternateText="collapse" />
                    <b>
                        <asp:Label ID="lblStatus" runat="server" CssClass="searchBoxStatus" Text="Hide Search Box"></asp:Label></b>
                </asp:Panel>
                <asp:Panel ID="pnSearchCriteria" runat="server" DefaultButton="ctlSearch">
                    <table width="98%" class="table" cellpadding="0" cellspacing="0" border="0">
                        <caption>
                            &nbsp;<tr>
                                <%--<td style="width: 20%" align="right">
                                <asp:Label ID="ctlRequestTypeLabel" runat="server" Text="Request Type" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ctlRequestType" runat="server" SkinID="SkGeneralDropdown" Width="145px">
                                </asp:DropDownList>
                            </td>--%>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="ctlCompanyLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="Company"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td>
                                    <uc4:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label10" runat="server" SkinID="SkGeneralLabel" Text="Subject"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox10" runat="server" MaxLength="15" 
                                        SkinID="SkGeneralTextBox"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label3" runat="server" SkinID="SkGeneralLabel" Text="Creator"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td align="left">
                                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupCreator" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ctlRequestNoLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="Request No"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="ctlRequestNo" runat="server" MaxLength="15" 
                                        SkinID="SkGeneralTextBox"></asp:TextBox>
                                    <%--<asp:Button ID="ctlReqNoOpenBtn" runat="server" SkinID="SkGeneralButton" Text="Open" Width="50" />--%>
                                </td>
                                <td align="right">
                                    <asp:Label ID="ctlTravellerNameTHLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="TravellerNameTH"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td align="left">
                                    <%--<uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupCreator" runat="server" />--%>
                                    <asp:TextBox ID="ctlTravellerNameTH" runat="server" MaxLength="50" 
                                        SkinID="SkGeneralTextBox" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="ctlTravellerNameENLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="TravellerNameEN"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td align="left">
                                    <%--<uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupRequester" runat="server" />--%>
                                    <asp:TextBox ID="ctlTravellerNameEN" runat="server" MaxLength="50" 
                                        SkinID="SkGeneralTextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ctlRequestDateFromLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="Request Date From"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td align="left">
                                    <uc1:Calendar ID="ctlRequestDateFrom" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="ctlRequestDateToLabel" runat="server" SkinID="SkGeneralLabel" 
                                        Text="To"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td colspan="3">
                                    <uc1:Calendar ID="ctlRequestDateTo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" SkinID="SkGeneralLabel" 
                                        Text="Approve Date From"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td align="left">
                                    <uc1:Calendar ID="ctlApproveDateFrom" runat="server" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" SkinID="SkGeneralLabel" Text="To"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td colspan="3">
                                    <uc1:Calendar ID="ctlApproveDateTo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label11" runat="server" SkinID="SkGeneralLabel" Text="Country"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="TextBox11" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label12" runat="server" SkinID="SkGeneralLabel" Text="Province"></asp:Label>
                                    &nbsp;:&nbsp;
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="TextBox12" runat="server" SkinID="SkGeneralTextBox"></asp:TextBox>
                                </td>
                            </tr>
                        </caption>
                    </table>
                    <table border="0">
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Button ID="ctlSearch" runat="server" SkinID="SkGeneralButton" Text="Search"
                                    OnClick="ctlSearch_Click" Width="100px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <ajaxToolkit:CollapsiblePanelExtender ID="collapPanel1" runat="Server" TargetControlID="pnSearchCriteria"
                ExpandControlID="pnSearchHeader" CollapseControlID="pnSearchHeader" Collapsed="false"
                CollapsedImage="~/App_Themes/Default/images/Slide/expand.jpg" ExpandedImage="~/App_Themes/Default/images/Slide/collapse.jpg"
                TextLabelID="lblStatus" ImageControlID="imgToggle" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="ctlUpdatePanelSearchResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <fieldset id="ctlFieldSearch" runat="server" class="table">
                            <legend id="ctlLegendSearchCriteria" style="color: #4E9DDF">
                                <asp:Label ID="ctlSearchCriteriaHeader" runat="server" Text='Data'></asp:Label>
                            </legend>
                            <center>
                                <ss:BaseGridView ID="ctlTASearchResultGrid" DataKeyNames="DocumentID,WorkflowID"
                                    runat="server" AutoGenerateColumns="False" ReadOnly="true" EnableInsert="False"
                                    CssClass="Grid" Width="100%" InsertRowCount="1" AllowPaging="true" AllowSorting="true"
                                    OnRequestCount="RequestCount" OnRequestData="RequestData" OnRowCommand="ctlTASearchResultGrid_RowCommand">
                                    <HeaderStyle CssClass="GridHeader" />
                                    <RowStyle CssClass="GridItem" HorizontalAlign="left" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Request No." HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="RequestNo" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ctlRequestNo" runat="server" SkinID="SkCtlLinkButton" Text='<%# Bind("RequestNo") %>'
                                                    CommandName="PopupDocument"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Center" SortExpression="Subject"
                                            HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlSubject" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                                    Text='<%# Bind("Subject")%>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creator" HeaderStyle-HorizontalAlign="Center" SortExpression="CreatorName"
                                            HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlCreatorName" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                                    Text='<%# Bind("CreatorName")%>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Requester" HeaderStyle-HorizontalAlign="Center" SortExpression="RequesterName"
                                            HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlRequestName" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                                    Text='<%# Bind("RequesterName")%>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Company" HeaderStyle-HorizontalAlign="Center" SortExpression="CompanyName"
                                            HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlCompany" Mode="Encode" runat="server" SkinID="SkGeneralLabel"
                                                    Text='<%# Bind("CompanyName") %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request Date" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="RequestDate" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlRequestDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel"
                                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("RequestDate")) %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve Date" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="ApproveDate" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlApproveDate" Mode="Encode" runat="server" SkinID="SkCalendarLabel"
                                                    Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("ApproveDate")) %>'></asp:Literal>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </ss:BaseGridView>
                                <br />
                            </center>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
