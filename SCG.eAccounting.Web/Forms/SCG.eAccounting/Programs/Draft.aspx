<%@ Page Title="Draft" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master" CodeBehind="Draft.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.Draft" StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register src="~/UserControls/InboxSearchResult/InboxEmployeeSearchResult.ascx" tagname="InboxEmployeeSearchResult" tagprefix="uc1" %>

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
                                    <uc1:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResult" runat="server" StateName="Draft"/>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>                         
    </table>
</asp:Content>
        