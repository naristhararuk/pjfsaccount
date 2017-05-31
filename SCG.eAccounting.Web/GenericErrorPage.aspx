<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericErrorPage.aspx.cs"
    Inherits="SCG.eAccounting.Web.GenericErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link rel="Stylesheet" type="text/css" href="~/App_Themes/Default/default.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr align="center" style="vertical-align: top; height: 100%; width: 100%">
            <td>
                <table id="Table2" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="imgHead1" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif"
                                CssClass="bannerLogIn" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center" style="vertical-align: center; height: 100%; width: 100%">
            <td>
                <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
