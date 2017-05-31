<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetType.aspx.cs" Inherits="SCG.eAccounting.Web.GetType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td colspan="2" style="background-color: #00FFFF">
                    Get Control</td>
            </tr>
            <tr>
                <td class="style2">
                    Path Form</td>
                <td>
    
        <asp:TextBox ID="txtPath" runat="server" Width="780px">E:\[WORK\[SoftSquare\STANDARD\1.0\main\program\NHibernate\SCG.eAccounting.Web\Forms\SU\Programs\</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="2">
                    File In Folder<br />
                    <asp:ListBox ID="listFile" runat="server" Height="240px" Width="867px">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="2">
                    <asp:Button ID="btnGetFile" runat="server" onclick="btnGetFile_Click" 
                        Text="Get File" Width="141px" />
&nbsp;<asp:Button ID="btnGo" runat="server" onclick="btnGo_Click" Text="Go Go..." Width="149px" />
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click1" 
                        Text="Button" />
                </td>
            </tr>
        </table>
        <br />
    
    </div>
    </form>
</body>
</html>
