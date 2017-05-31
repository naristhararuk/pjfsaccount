<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupSupport.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.PopupSupport" %>
<style type="text/css">
@import url(<%= Page.ResolveUrl("~/Scripts/themes/jquery-ui-1.10.3.custom.css") %>);
</style>
<ss:InlineScript ID="InlineScript1" runat="server">
<script type="text/javascript">
//    document.windooManager = {
//    repository : {},
//    
//    createWindoo : function(name, callback, option) {
//        this.repository[name] = { 
//            windoo: new Windoo(option),
//            callback: callback
//        };
//        return this.repository[name].windoo;
//    },
//    
//    notifyPopupResult : function(name, action, value) {
//        var func = this.repository[name].callback;
//        func(action, value);
//    }
//    
//}
</script>
</ss:InlineScript>