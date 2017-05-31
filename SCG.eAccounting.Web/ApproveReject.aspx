<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveReject.aspx.cs"
    Inherits="SCG.eAccounting.Web.ApproveReject" %>

<%@ Register Src="UserControls/MenuBottom.ascx" TagName="MenuBottom" TagPrefix="uc7" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link rel="Stylesheet" type="text/css" href="~/App_Themes/Default/default.css" />
    <title></title>
</head>
<%--<body>
    <form id="form1" runat="server">
    <font color="red">
        <spring:ValidationSummary ID="ValidationSummary2" runat="server" Provider="General.Error"></spring:ValidationSummary>
        <spring:ValidationSummary ID="ValidationSummary1" runat="server" Provider="WorkFlow.Error"></spring:ValidationSummary>
    </font>
    <div>
        <asp:Label ID="ctlResult" runat="server"></asp:Label>
    </div>
    </form>
</body>--%>
<body>
    <form id="FormContent" runat="server" align="center">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"
        CombineScripts="true" EnablePageMethods="true" EnablePartialRendering="true"
        AsyncPostBackTimeout="300" />
    <table id="MAIN_CENTER" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr align="center" style="vertical-align: top; height: 100%; width: 100%">
            <!-- <td style="width:30px;background-image:url(<%= ResolveClientUrl("~/App_Themes/Default/images/MasterPage/BorderLeft.png") %>);"> -->
            <td style="width: 30px; background-color: White">
            </td>
            <td>
                <!-- MAIN_CENTER -->
                <table id="MAIN_TABLE" border="0" cellpadding="0" cellspacing="0" style="vertical-align: top;
                    height: 100%; width: 100%;">
                    <!-- ROW_HEAD -->
                    <tr valign="top" style="height: 100%; width: 100%">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 100%; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Image ID="imgHead1" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif"
                                            CssClass="logoSCG" />
                                    </td>
                                    <td class="backgroundHeaderCenter">
                                    </td>
                                    <td>
                                        <asp:Image ID="imgHead2" runat="server" ImageUrl="~/App_Themes/Default/images/empty.gif"
                                            CssClass="bannerExpense" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- DETAIL -->
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" class="backgroundLeftMenu" style="height: 500px">
                                <tr valign="top">
                                    <td rowspan="2" class="backgroundContent">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                                            <tr>
                                                <td>
                                                    <font color="red">
                                                        <spring:ValidationSummary ID="ValidationSummary2" runat="server" Provider="General.Error">
                                                        </spring:ValidationSummary>
                                                        <spring:ValidationSummary ID="ValidationSummary1" runat="server" Provider="WorkFlow.Error">
                                                        </spring:ValidationSummary>
                                                    </font>
                                                    <div>
                                                        <asp:Label ID="ctlResult" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- BUTTOM -->
                    <tr>
                        <td>
                            <uc7:menubottom id="MenuBottom" runat="server" />
                        </td>
                    </tr>
                </table>
                <!-- MAIN_CENTER -->
            </td>
            <!-- <td style="width:30px;background-image:url(<%= ResolveClientUrl("~/App_Themes/Default/images/MasterPage/BorderRight.png") %>);"> -->
            <td style="width: 30px; background-color: White">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
