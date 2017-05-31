<%@ Page Title="Accountant Payment Inbox" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master" CodeBehind="AccountantPaymentInbox.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.AccountantPaymentInbox" StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register src="~/UserControls/InboxSearchResult/InboxAccountantPaymentSearchCriteria.ascx" tagname="InboxAccountantPaymentSearchCriteria" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table  width="100%">
        <tr>
            <td align="left">  
                <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <table width="100%" class="table" border="0">  
                            <tr>
                                <td>
                                    <uc1:InboxAccountantPaymentSearchCriteria ID="ctlInboxAccountantPaymentSearchCriteria" runat="server" FlagSearch="Accountant" FlagJoin="Inbox" SearchType="2" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>                         
    </table>
</asp:Content>