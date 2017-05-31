<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupCaller.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.PopupCaller" EnableTheming="true" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" OnValueChanged="ctlReturnAction_ValueChanged" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />
<%--<a href="#" onclick="<%= ClientID %>_popup();return false;">Popup</a>--%>
<asp:Button ID="ctlPopup" runat="server" OnClick="ctlPopup_Click" Style="display:none"/>
<ss:InlineScript ID="InlineScript1" runat="server">
    <script type="text/javascript">
//var <%= ClientID %>_win;

function <%= ClientID %>_popup(url) 
{
    if(url == null)url = '<%= ProcessedURL %>';
    jq.fsPopup.popup(url, <%= ClientID %>_notifyPopupResult,{width:'<%= Width %>',height:'<%= Height %>',modal:true});
}


function <%= ClientID %>_notifyPopupResult(action, value)
{
    var ctlReturnAction = jq('#<%= ctlReturnAction.ClientID %>');
    var ctlReturnValue = jq('#<%= ctlReturnValue.ClientID %>');
    //var win = <%= ClientID %>_win;
    ctlReturnAction.val(action);
    ctlReturnValue.val(value);  
    //win.close();
    <%= Page.GetPostBackEventReference(ctlReturnAction) %>;
}
    </script>
</ss:InlineScript>