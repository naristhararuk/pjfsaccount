<%@ Page Title="AdvanceDomesticForm" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master" CodeBehind="AdvanceDomesticForm.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.AdvanceDomesticForm" StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register src="~/Forms/SCG.eAccounting/Programs/AdvanceForm.ascx" tagname="AdvanceForm" tagprefix="uc1" %>
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
                                    <uc1:AdvanceForm ID="ctlAdvanceForm" runat="server" AdvanceTypeForm="1"/>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>                         
    </table>
</asp:Content>
        