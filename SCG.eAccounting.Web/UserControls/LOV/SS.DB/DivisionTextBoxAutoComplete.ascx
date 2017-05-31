<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DivisionTextBoxAutoComplete.ascx.cs" 
	Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.DivisionTextBoxAutoComplete" EnableTheming="true" %>

<script language="javascript" type="text/javascript">
//    function pageLoad(sender, args)
//    {
//        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
//    }
    function  OnDivisionSelected(source,eventArgs)
    {
        //alert("OnDivisionSelected");
        //alert(eventArgs.get_value());

        // eventArgs.get_value() = JSON string
        // Use Sys.Serialization.JavaScriptSerializer.deserialize to deserialize JSON string to JSON object
        var selectedDivision = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        //alert(results.ID);
        
        // $get same as document.all.getElementByID
        $get('<%= ctlDivisionId.ClientID %>').innerText = selectedDivision.DivisionId;
        /*
            Can get other properties of the ValueObject instance
            selectedDivision.LanguageId ,
            selectedDivision.LanguageName ,
            selectedDivision.DivisionLangId ,
            selectedDivision.DivisionName , ...
        */
    }
    
    function OnDivisionPopulated(source,eventArgs)
    {
        //alert("OnDivisionPopulated");
        //Clear Selected Id
        $get('<%= ctlDivisionId.ClientID %>').innerText = "";
    }
    
    function OnDivisionPopulating(source,eventArgs)
    {
        //alert("OnDivisionPopulating");
        
        
    }
    function OnDivisionShown(source,eventArgs)
    {
        //alert("OnDivisionShown");
    }
    function OnDivisionShowing(source,eventArgs)
    {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }

    function OnItemOut(source, eventArgs) 
    {
        //alert("OnItemOut");
    }

    function OnHidden(source, eventArgs) 
    {
        //alert("OnHidden");
     
    }

    function OnHiding(source, eventArgs) 
    {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
   
</script>
<asp:TextBox ID="ctlDivisionName" runat="server" Width="148px" />
<asp:Label ID="ctlDivisionId" runat="server" style="display:none;" />
<asp:Label ID="ctlOrganizationId" runat="server" style="display:none;" />
<asp:Label ID="ctlLanguageId" runat="server" style="display:none;" />
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" style="display:none;" />
<ajaxToolkit:AutoCompleteExtender runat="server" 
    BehaviorID="DivisionAutoCompleteEx" 
    ID="ctlDivisionAutoComplete"
    TargetControlID="ctlDivisionName" 
    ServicePath="~/WebService/DivisionAutoComplete.asmx"
    ServiceMethod="GetDivisionList"
    MinimumPrefixLength="2"
    EnableCaching="true"
    OnClientItemSelected="OnDivisionSelected"
    OnClientPopulating="OnDivisionPopulating"
    OnClientPopulated="OnDivisionPopulated"
    OnClientItemOut="OnItemOut"
    OnClientShowing="OnDivisionShowing"
    OnClientShown="OnDivisionShown"
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
                    var behavior = $find('DivisionAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                <Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('DivisionAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            <Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('DivisionAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>