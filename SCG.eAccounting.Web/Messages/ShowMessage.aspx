<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowMessage.aspx.cs" Inherits="SCG.eAccounting.Web.Messages.ShowMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <script  type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/Ajax.js") %>"></script>

</head>
<body onload="getDialogMessage(<%=MsgID%>)">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
