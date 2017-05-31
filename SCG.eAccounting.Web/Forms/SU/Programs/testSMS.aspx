<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master" EnableTheming="true" StylesheetTheme="Default" CodeBehind="testSMS.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.testSMS" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="A">

                                
                                            

   
    <table width="450xp">
    
        <tr>
            <td> <asp:Label ID="Label1" runat="server" Text="TO : "></asp:Label></td>
            <td><asp:TextBox ID="TO" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td> <asp:Label ID="Label2" runat="server" Text="CONTENT : "></asp:Label></td>
            <td><asp:TextBox ID="CONTENT" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
            <asp:Button ID="Send" runat="server" Text="Button" onclick="Send_Click" />
            </td>
            
        </tr>
    </table>
    
    

        <br />
    <asp:Button ID="btnSendMail" runat="server" Text="testSendMail" />                  
                                            

    <asp:Button ID="btnReporting" runat="server" Text="test Reporting" 
        onclick="btnReporting_Click" />
                                            

</asp:Content>
