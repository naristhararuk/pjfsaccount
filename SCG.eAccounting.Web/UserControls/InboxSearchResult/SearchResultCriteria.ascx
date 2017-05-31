<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResultCriteria.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.SearchResultCriteria" EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/InboxSearchResult/SearchResult.ascx" tagname="SearchResult" tagprefix="uc2" %>
<%@ Register src="~/Usercontrols/DocumentEditor/Components/CostCenterLookupPrototype.ascx" tagname="CostCenterLookupPrototype" tagprefix="uc3" %>
<asp:UpdatePanel ID="ctlUpdatePanelSearchResult" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table">
            <tr><td>
            <fieldset id="ctlFieldSearch" runat="server" class="table">
                <table width="100%" class="table">
                    <tr>
                        <td><asp:Label ID="ctlYear" runat="server" Text="Budget Year" SkinID="SkCtlLabel"></asp:Label></td>
                        <td colspan="3"><asp:TextBox ID="ctlInputYear" SkinID="SkCtlShortTextboxCenter" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="cltDocTypeFrom" runat="server" Text="Document Type From" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputDocTypeFrom" runat="server"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ctlSearchDocTypeFrom" SkinID="SkCtlQuery" 
                                onclick="ctlSearchDocType_Click"/></td>
                        <td><asp:Label ID="cltDocTypeTo" runat="server" Text="To" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputDocTypeTo" runat="server"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ctlSearchDocTypeTo" SkinID="SkCtlQuery" 
                                onclick="ctlSearchDocType_Click"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="ctlCostCenterFrom" runat="server" Text="Cost Center From" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputCostCenterFrom" runat="server"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ctlSearchCostCenterFrom" 
                                SkinID="SkCtlQuery" onclick="ctlSearchCostCenter_Click"/></td>
                        <td><asp:Label ID="ctlCostCenterTo" runat="server" Text="To" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputCostCenterTo" runat="server"></asp:TextBox><asp:ImageButton runat="server" ID="ctlSearchCostCenterTo" SkinID="SkCtlQuery" onclick="ctlSearchCostCenter_Click" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="ctlCreateDocFrom" runat="server" Text="Create Date From" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><uc1:Calendar ID="ctlCalCreateDocFrom" runat="server" zIndex="10002" /></td>
                        <td><asp:Label ID="ctlCreateDocTo" runat="server" Text="To" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><uc1:Calendar ID="ctlCalCreateDocTo" runat="server" zIndex="10002" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="ctlDocNoFrom" runat="server" Text="Document No From" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputDocNoFrom" runat="server"></asp:TextBox></td>
                        <td><asp:Label ID="ctlDocNoTo" runat="server" Text="To" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputDocNoTo" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="ctlAmountFrom" runat="server" Text="Amount From" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputAmountFrom" runat="server"></asp:TextBox></td>
                        <td><asp:Label ID="ctlAmountTo" runat="server" Text="To" SkinID="SkCtlLabel"></asp:Label></td>
                        <td><asp:TextBox ID="ctlInputAmountTo" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="ctlDocStatus" runat="server" Text="Document Status" SkinID="SkCtlLabel"></asp:Label></td>
                        <td colspan="3">
                            <asp:DropDownList ID="ctlSelectedDocStatus" runat="server">
                            <asp:ListItem Text="Draft"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center"><asp:Button ID="ctlButtonSearch" runat="server" 
                                Text="Search" Width="100px" OnClick="Search_OnClick" /></td>
                    </tr>
                </table>
            </fieldset>
            </td></tr>
        </table>
        <uc2:SearchResult ID="SearchResult1" runat="server" zIndex="10002" />
    
    </ContentTemplate>
</asp:UpdatePanel>
<uc3:CostCenterLookupPrototype ID="CostCenterLookupPrototype1" runat="server" />
