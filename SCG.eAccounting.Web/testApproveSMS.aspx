<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testApproveSMS.aspx.cs" Inherits="SCG.eAccounting.Web.testApproveSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 480px;
        }
        .style2
        {
            width: 338px;
        }
    </style>
</head>
<script>

    function showRespone(obj) {
        alert(obj);
    }
</script>
<body>
    <form id="form1" runat="server">
    <div>
            <center>
               
              
                        <asp:Label ID="Label6" runat="server" 
    Text="Test Approval via SMS" Font-Bold="True"></asp:Label>
    <br />
     <table style="width:50%; height: 293px;" align="center">
         <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label5" runat="server" Text="TRANID"></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="TRANSID" runat="server" Width="287px" Text="000000"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label1" runat="server" Text="FROM" ></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="FROM" runat="server" Width="287px" Text="66865709511"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label2" runat="server" Text="TO"></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="TO" runat="server" Text="scgaccount" Width="287px"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label3" runat="server" Text="Token ID"></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="TOKENID" runat="server" Width="287px"></asp:TextBox>
                </td>
               
            </tr>
             <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label4" runat="server" Text="Approval"></asp:Label>
                </td>
                <td align="left" class="style1">
                    <asp:RadioButton ID="apprvoe" runat="server" GroupName="approval" 
                        Text="Approve" AutoPostBack="True" />
                    <asp:RadioButton ID="reject" runat="server" GroupName="approval" 
                        Text="Reject" AutoPostBack="True" />
                </td>
                
            </tr>
             <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSend" runat="server" Text="Send" onclick="btnSend_Click" />
                </td>
                
            </tr>
             <tr>
                <td colspan="2" align="center">
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                    <br />
                    
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="419px" 
                        Height="124px"></asp:TextBox>
                </td>
                
            </tr>
        </table>
    
            </center>
       
    </div>
    
    <div>
            <center>
              
              
                        <asp:Label ID="Label8" runat="server" 
    Text="Test DLVRREP  via AIS SMS Server" Font-Bold="True"></asp:Label>
    <br />
     <table style="width:50%;" align="center">
         <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label9" runat="server" Text="FROM"></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="txtFROM" runat="server" Width="287px" Text="000000"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label10" runat="server" Text="SMID" ></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="txtSMID" runat="server" Width="287px" Text="66865709511"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td align="right" class="style2">
                    <asp:Label ID="Label12" runat="server" Text="DETAIL"></asp:Label>
                </td>
                <td  align="left" class="style1">
                    <asp:TextBox ID="txtDetail" runat="server" Width="287px"></asp:TextBox>
                </td>
               
            </tr>
             <tr>
                <td align="right" class="style2">
                    Status</td>
                <td align="left" class="style1">
                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="approvalavl" 
                        Text="OK" AutoPostBack="True" />
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="approvalavl" 
                        Text="ERR" AutoPostBack="True" />
                </td>
                
            </tr>
             <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="Button1" runat="server" Text="Send" onclick="Button1_Click"  />
                </td>
                
            </tr>
             <tr>
                <td colspan="2" align="center">
                    <br />
                    <asp:Label ID="Label14" runat="server" Text="Label"></asp:Label>
                    <br />
                    
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="400px" 
                        Height="147px"></asp:TextBox>
                </td>
                
            </tr>
        </table>
    
                
            </center>
       
    </div>
    </form>
</body>
</html>
