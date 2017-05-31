<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryTextBoxAutoComplete.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CountryTextBoxAutoComplete" EnableTheming="true"%>
<script language="javascript" type="text/javascript">
    function  OnCountrySelected(source,eventArgs) {
       // alert('onselected');
        var selectedCountry = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlCountryId.ClientID %>').innerText = selectedCountry.CountryID;
       
    }
    
    function OnCountryPopulated(source,eventArgs) {
        //alert('OnCountryPopulated');
        $get('<%= ctlCountryId.ClientID %>').innerText = "";
      
    }
    function OnCountryPopulating(source, eventArgs) {
        //alert("OnDivisionPopulating");


    }
    function OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function OnCountryShowing(source, eventArgs) {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function OnCountryShown(source, eventArgs) {
    	//alert("OnDivisionShown");
    	var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
    		behavior.style.width = 200;
    }
    function OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }
    
</script>
<asp:TextBox ID="txtCountryName" runat="server" Width="148px" />
<div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display:none;" class="autocomplete_completionListElement"></div>
<asp:Label ID="ctlCountryId" runat="server" style="display:none;" />
<asp:Label ID="ctlLanguageId" runat="server" style="display:none;" />
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" style="display:none;" />
<ajaxToolkit:AutoCompleteExtender runat="server" 
    BehaviorID="CountryAutoCompleteEx" 
    ID="ctlCountryAutoComplete"
    TargetControlID="txtCountryName" 
    ServicePath="~/WebService/CountryAutoComplete.asmx"
    ServiceMethod="GetCountryList"
    MinimumPrefixLength="2"
    EnableCaching="true"
    OnClientItemSelected="OnCountrySelected"
    OnClientPopulating="OnCountryPopulating"
    OnClientPopulated="OnCountryPopulated"
    OnClientItemOut="OnItemOut"
    OnClientShowing="OnCountryShowing"
    OnClientShown="OnCountryShown"
    OnClientHidden="OnHidden"
    OnClientHiding="OnHiding"
    SkinID="SkCtlAutoComplete"
    CompletionListElementID="ctlAutoCompleteSuggestionContainer"
    CompletionInterval="100"
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
                    var behavior = $find('CountryAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration="0">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('CountryAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration="0">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('CountryAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>

