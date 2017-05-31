<%@ Page 
    Language="C#" 
    MasterPageFile="~/ProgramsPages.Master" 
    AutoEventWireup="true" CodeBehind="TestPostingData.aspx.cs" 
    Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.TestPostingData" 
    Title="Test Posting Data By Kookkla...." 
    EnableTheming = "true"
    StylesheetTheme="Default" meta:resourcekey="PageResource1"    
%>


<%@ Register src="~/UserControls/ViewPost/ViewPostTest.ascx" tagname="ViewPostTest" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 138px;
            height: 39px;
        }
        .style4
        {
            height: 39px;
        }
        .style5
        {
            width: 138px;
            height: 30px;
        }
        .style6
        {
            height: 30px;
        }
        .style7
        {
            height: 36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">

<asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        
        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>

        <table border="1">
            <tr>
                <td class="style7"></td>
                <td class="style7" style="text-align: left">
                    <asp:DropDownList ID="ddlType" runat="server" Height="25px" Width="145px">
                        <asp:ListItem Value="1">Advance</asp:ListItem>
                        <asp:ListItem Value="2">Remittance</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">Expense</asp:ListItem>
                        <asp:ListItem Value="4" >ExpenseRemitance</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style5">Document No.</td>
                <td class="style6" style="text-align: left"><asp:TextBox ID="txtDocNo" runat="server" Width="405px" 
                        BackColor="#FFFF99" Height="23px" Text="425"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Button ID="btnCreatePosting" runat="server" BackColor="#00CCFF" Text="CreatePosting" 
                        Width="100px" onclick="btnCreatePosting_Click" />
                    <asp:Button ID="btnViewPost" runat="server" BackColor="#FF6600" Text="ViewPost" 
                        Width="100px" onclick="btnViewPost_Click" />
                </td>
                <td class="style4">&nbsp;<asp:Button ID="btnSimulate" runat="server" BackColor="#FF99CC" 
                        Text="Simulate" Width="100px" onclick="btnSimulate_Click" />
                    &nbsp;<asp:Button ID="btnPosting" runat="server" BackColor="#FF66CC" Text="Posting" 
                        Width="100px" onclick="btnPosting_Click" />
                    &nbsp;<asp:Button ID="btnReverse" runat="server" BackColor="#FF33CC" Text="Reverse" 
                        Width="100px" onclick="btnReverse_Click" />
                    &nbsp;<asp:Button ID="btnApprove" runat="server" BackColor="#CC0099" Text="Approve" 
                        Width="100px" onclick="btnApprove_Click" />
                </td>
            </tr>           
        </table>
        <uc1:ViewPostTest ID="ViewPostTest1" runat="server" />
        <br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        &nbsp;
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnViewPost" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCreatePosting" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnSimulate" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPosting" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnReverse" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
    
    
</asp:Content>
