<%@ Page Title="Inbox Employee" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master" CodeBehind="EmployeeInbox.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.EmployeeInbox" StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register src="~/UserControls/InboxSearchResult/InboxEmployeeSearchCriteria.ascx" tagname="InboxEmployeeSearchCriteria" tagprefix="uc1" %>
<%@ Register src="~/UserControls/InboxSearchResult/InboxEmployeeSearchResult.ascx" tagname="InboxEmployeeSearchResult" tagprefix="uc2" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <table  width="100%">
        <tr>
            <td align="left">  
                <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanel"
                            DynamicLayout="true" EnableViewState="true">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Panel ID="ctlPanelEmployeeInbox" runat="server" DefaultButton="ctlSearch">
                        <table width="100%" class="table" border="0">  
                            <tr>
                                <td>
                                    <uc1:InboxEmployeeSearchCriteria ID="ctlInboxEmployeeSearchCriteria" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ctlSearch" Width="80px" runat="server" Text="$Search$" SkinID="SkGeneralButton" OnClick="ctlSearchCriteria_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc2:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResultDraft" runat="server" StateName="Draft" />
                                </td>
                            </tr> 
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ctlSendDraft" Width="80px" runat="server" Text="$Send$" SkinID="SkGeneralButton" OnClick="ctlSendDraft_Click" />
                                </td>
                            </tr>    
                            <tr>
                                <td>
                                    <uc2:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResultWaitInitial" runat="server" StateName="WaitInitial" />
                                </td>
                            </tr> 
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ctlAcceptWaitInitial" Width="80px" runat="server" Text="$Accept$" SkinID="SkGeneralButton" OnClick="ctlAcceptWaitInitial_Click" />
                                </td>
                            </tr>  
                            <tr>
                                <td>
                                    <uc2:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResultWaitApprove" runat="server" StateName="WaitApprove"/>
                                </td>
                            </tr> 
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ctlApproveWaitApprove" Width="80px" runat="server" Text="$Approve$" SkinID="SkGeneralButton" OnClick="ctlApproveWaitApprove_Click" />
                                </td>
                            </tr>  
                            <tr>
                                <td>
                                    <uc2:InboxEmployeeSearchResult ID="ctlInboxemployeeSearchResultHold" runat="server" StateName="Hold"/>
                                </td>
                            </tr> 
                            <tr>
                                <td>
                                    <uc2:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResultWaitAgree" runat="server" StateName="WaitAR" />
                                </td>
                            </tr> 
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ctlApproveAgree" Width="80px" runat="server" Text="$Agree$" SkinID="SkGeneralButton" OnClick="ctlApproveWaitAgree_Click" />
                                </td>
                            </tr>         
                        </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="ctlUpdatePanelValidation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table border="0" width="400px">
                            <tr>
                                <td align="left" style="color: Red;">
                                    <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="Provider.Error" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>