<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RejectReasonTextBoxAutoComplete.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.RejectReasonTextBoxAutoComplete" EnableTheming="true"%>
<script language="javascript" type="text/javascript">
    function  OnReasonSelected(source,eventArgs) {
//        alert('onselected');
        var selectedReason = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
//        alert(selectedReason.ReasonID);
        $get('<%= ctlReasonId.ClientID %>').innerText = selectedReason.ReasonID;
       
    }
    
    function OnReasonPopulated(source,eventArgs) {
        //alert('OnReasonPopulated');
        $get('<%= ctlReasonId.ClientID %>').innerText = "";
      
    }
    function OnReasonPopulating(source, eventArgs) {
        //alert("OnDivisionPopulating");


    }
    function OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function OnReasonShowing(source, eventArgs) {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function OnReasonShown(source, eventArgs) {
        //alert("OnDivisionShown");
    }
    function OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }
    
</script>
<asp:TextBox ID="txtReasonDetail" runat="server" Width="148px" />
<asp:Label ID="ctlReasonId" runat="server" style="display:none;" />
<asp:Label ID="ctlDocumentTypeCode" runat="server" style="display:none;" />
<asp:Label ID="ctlLanguageId" runat="server" style="display:none;" />
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" style="display:none;" />
<ajaxToolkit:AutoCompleteExtender runat="server" 
    BehaviorID="ReasonAutoCompleteEx" 
    ID="ctlReasonAutoComplete"
    TargetControlID="txtReasonDetail" 
    ServicePath="~/WebService/RejectReasonAutoComplete.asmx"
    ServiceMethod="GetReasonList"
    MinimumPrefixLength="2"
    EnableCaching="true"
    OnClientItemSelected="OnReasonSelected"
    OnClientPopulating="OnReasonPopulating"
    OnClientPopulated="OnReasonPopulated"
    OnClientItemOut="OnItemOut"
    OnClientShowing="OnReasonShowing"
    OnClientShown="OnReasonShown"
    OnClientHidden="OnHidden"
    OnClientHiding="OnHiding"
    SkinID="SkCtlAutoComplete"
    CompletionInterval="1000"
    CompletionSetCount="8">
    <Animations>
        <OnShow>
            <Sequence>
                <%-- Make the completion list transparent and then show it --%>
                <OpacityAction Opacity="0" />
                <HideAction Visible="true" />
                
                <%--Cache the original size of the completion list the first time
                    the animation is played and then set it to zero --%>
                
                
                
                <ScriptAction Script="
                    // Cache the size and setup the initial size
                    var behavior = $find('ReasonAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('ReasonAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('ReasonAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>

