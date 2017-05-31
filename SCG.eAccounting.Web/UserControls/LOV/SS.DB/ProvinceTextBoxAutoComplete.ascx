<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ProvinceTextBoxAutoComplete.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.ProvinceTextBoxAutoComplete" 
    EnableTheming="true" 
%>

<script language="javascript" type="text/javascript">
    function  OnProvinceSelected(source,eventArgs)
    {
        var selectedProvince = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        	
        $get('<%= lblProvinceId.ClientID %>').innerText = selectedProvince.ProvinceID;
   	    $get('<%= lblRegionId.ClientID %>').innerText = selectedProvince.RegionID;
   	    $get('<%= lblRegionName.ClientID %>').innerText = selectedProvince.RegionName;
    }
    
    function OnProvincePopulated(source,eventArgs)
    {
        $get('<%= lblProvinceId.ClientID %>').innerText = "";
        $get('<%= lblRegionId.ClientID %>').innerText = "";
   	    $get('<%= lblRegionName.ClientID %>').innerText = "";
    }
    
    function OnProvincePopulating(source,eventArgs)
    {
        
    }
    
    function OnProvinceShow(source,eventArgs)
    {
    
    }
    
    function OnProvinceShowing(source,eventArgs)
    {
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }

    function OnItemOut(source, eventArgs) 
    {
    }

    function OnHidden(source, eventArgs) 
    {
     
    }

    function OnHiding(source, eventArgs) 
    {
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }

   
</script>

<asp:TextBox ID="txtProvinceName"   runat="server" Width="148px" />
<asp:Label ID="lblProvinceId"       runat="server" style="display:none;" />
<asp:Label ID="lblRegionId"         runat="server" style="display:none;" />
<asp:Label ID="lblRegionName"       runat="server" style="display:none;" />
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" style="display:none;" />

<ajaxToolkit:AutoCompleteExtender runat="server" 
    BehaviorID="ProvinceAutoCompleteEx" 
    ID="ctlProvinceAutoComplete"
    TargetControlID="txtProvinceName" 
    ServicePath="~/WebService/ProvinceAutoComplete.asmx"
    ServiceMethod="GetProvinceList"
    MinimumPrefixLength="2"
    EnableCaching="true"
    SkinID="SkCtlAutoComplete"
    CompletionInterval="1000"
    CompletionSetCount="8"
    
    OnClientItemSelected="OnProvinceSelected"
    OnClientPopulating="OnProvincePopulating"
    OnClientPopulated="OnProvincePopulated"
    OnClientItemOut="OnItemOut"
    OnClientShowing="OnProvinceShowing"
    OnClientShown="OnProvinceShow"
    OnClientHidden="OnHidden"
    OnClientHiding="OnHiding"
    >
    
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
                    var behavior = $find('ProvinceAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                <Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('ProvinceAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            <Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('ProvinceAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
    
</ajaxToolkit:AutoCompleteExtender>

