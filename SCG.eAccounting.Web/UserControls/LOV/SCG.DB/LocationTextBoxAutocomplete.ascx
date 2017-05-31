<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationTextBoxAutocomplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.LocationTextBoxAutocomplete" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />

<script language="javascript" type="text/javascript">
    function <%= ctlLocation.ClientID %>_OnSelected(source, eventArgs) {
        var selectedIO = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlLocation.ClientID %>').innerText = selectedIO.LocationCode;    
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedIO.LocationID;
    }

    function <%= ctlLocation.ClientID %>_OnPopulated(source, eventArgs) {
        $get('<%= ctlLocation.ClientID %>').innerText = "";
    } 
    function <%= ctlLocation.ClientID %>_OnPopulating(source, eventArgs) {
        //alert("OnPopulating");


    }
    function <%= ctlLocation.ClientID %>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%= ctlLocation.ClientID %>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%= ctlLocation.ClientID %>_OnShowing(source, eventArgs) {
        //alert("OnShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%= ctlLocation.ClientID %>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%= ctlLocation.ClientID %>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }  
    function <%=ctlLocation.ClientID%>_onAutoCompleteViewList()
{
    var autoComplete = document.getElementById("<%=ctlLocation.ClientID%>").AutoCompleteBehavior;

    autoComplete._cache = {};
    autoComplete._currentPrefix = null;
    autoComplete._textBoxHasFocus = true;

    var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
    autoComplete.set_minimumPrefixLength(0);

    autoComplete._onTimerTick(null, null);
    
    autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
}
</script>

<asp:UpdatePanel ID="ctlLocationTexboxAutoCompleteUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:TextBox ID="ctlLocation" runat="server" Width="150px" MaxLength="20"  SkinID="SkGeneralTextBox" style="text-align:center;" OnTextChanged="ctlLocation_TextChanged" AutoPostBack="true"/>
                    <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                        class="autocomplete_completionListElement">
                    </div>
                </td>
                <td>
                    <asp:Panel ID="ctlButtonLocationPanel" runat="server">
                    <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;"/>
                        <input id="ctlBtnLocationAC" type="button" value="..." onclick="<%=ctlLocation.ClientID%>_onAutoCompleteViewList()" style="display:none;" />
                    </asp:Panel>
                </td>
                <td>
                    <asp:Label ID="ctlLocationName" runat="server" Style="display: none;" />
                    <asp:Label ID="ctlLocationID" runat="server" Style="display: none;" />
                    <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="LocationAutoCompleteEx"
                        ID="ctlLocationTextAutoComplete" TargetControlID="ctlLocation" ServicePath="~/WebService/LocationAutoComplete.asmx"
                        ServiceMethod="GetLocationList" MinimumPrefixLength="2" EnableCaching="true"
                        SkinID="SkCtlAutoComplete" CompletionListElementID="ctlAutoCompleteSuggestionContainer"
                        CompletionInterval="100" CompletionSetCount="8">
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
                    var behavior = $find('LocationAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration=".2">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('LocationAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".2">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('LocationAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
                        </Animations>
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
