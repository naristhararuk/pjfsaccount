<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserTextBoxAutoComplete.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.UserTextBoxAutoComplete" EnableTheming="true"%>
<script language="javascript" type="text/javascript">
    function OnUserSelected(source, eventArgs) {
        //alert('onselected');
        var selectedUser = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlUserName.ClientID %>').innerText = selectedUser.UserName;
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }

    function OnUserPopulated(source, eventArgs) {
       // alert('OnUserPopulated');
        $get('<%= ctlUserName.ClientID %>').innerText = "";

    }
    function OnUserPopulating(source, eventArgs) {
       // alert("OnDivisionPopulating");


    }
    function OnHiding(source, eventArgs) {
       // alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function OnHidden(source, eventArgs) {
       // alert("OnHidden");

    }
    function OnUserShowing(source, eventArgs) {
      //  alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function OnUserShown(source, eventArgs) {
      //  alert("OnDivisionShown");
    }
    function OnItemOut(source, eventArgs) {
     //   alert("OnItemOut");
    }
    
</script>
<asp:TextBox ID="txtNameLastName" runat="server" Width="148px" />
<asp:Label ID="ctlUserName" runat="server"  style="display:none;"/>
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading"  style="display:none;" />
<ajaxToolkit:AutoCompleteExtender runat="server" 
    BehaviorID="UserAutoCompleteEx" 
    ID="ctlUserAutoComplete"
    TargetControlID="txtNameLastName" 
    ServicePath="~/WebService/UserAutoComplete.asmx"
    ServiceMethod="GetUserList"
    MinimumPrefixLength="2"
    EnableCaching="true"
    OnClientItemSelected="OnUserSelected"
    OnClientPopulating="OnUserPopulating"
    OnClientPopulated="OnUserPopulated"
    OnClientItemOut="OnItemOut"
    OnClientShowing="OnUserShowing"
    OnClientShown="OnUserShown"
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
                    var behavior = $find('UserAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('UserAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('UserAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>

