<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupCallback.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.PopupCallback" %>
<ss:InlineScript ID="InlineScript1" runat="server">
<script type="text/javascript">
    function notifyPopupResult(action, value) {
        <% if(!String.IsNullOrEmpty(Request["_pid"])) { %>
            jq.fsPopup.close(action, value);
        <% } %>
//<% if(!String.IsNullOrEmpty(Request["_pid"])) { %>
//    parent.document.windooManager.notifyPopupResult('<%= Request["_pid"] %>', action, value);
//<% } else { %>
//    alert("[action] :" + action + ", [value] :" + value);
//<% } %>
}

</script>
</ss:InlineScript>
