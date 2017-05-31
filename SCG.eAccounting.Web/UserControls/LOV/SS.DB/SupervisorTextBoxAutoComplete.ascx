<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupervisorTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.SupervisorTextBoxAutoComplete"
    EnableTheming="true" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />

<script language="javascript" type="text/javascript">
    function <%= ctlSupervisor.ClientID %>_OnSelected(source, eventArgs) {
        var selectedIO = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlSupervisor.ClientID %>').innerText = selectedIO.UserName;    
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedIO.UserID;
    }

    function <%= ctlSupervisor.ClientID %>_OnPopulated(source, eventArgs) {
        $get('<%= ctlSupervisor.ClientID %>').innerText = "";
    } 
    function <%= ctlSupervisor.ClientID %>_OnPopulating(source, eventArgs) {
        //alert("OnPopulating");


    }
    function <%= ctlSupervisor.ClientID %>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%= ctlSupervisor.ClientID %>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%= ctlSupervisor.ClientID %>_OnShowing(source, eventArgs) {
        //alert("OnShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%= ctlSupervisor.ClientID %>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%= ctlSupervisor.ClientID %>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }  
    function <%=ctlSupervisor.ClientID%>_onAutoCompleteViewList()
{
    var autoComplete = document.getElementById("<%=ctlSupervisor.ClientID%>").AutoCompleteBehavior;

    autoComplete._cache = {};
    autoComplete._currentPrefix = null;
    autoComplete._textBoxHasFocus = true;

    var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
    autoComplete.set_minimumPrefixLength(0);

    autoComplete._onTimerTick(null, null);
    
    autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
}
</script>

<table id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="ctlSupervisor" runat="server" Width="150px" MaxLength="20" OnTextChanged="ctlSupervisor_TextChanged" AutoPostBack="true" />
            <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                class="autocomplete_completionListElement">
            </div>
        </td>
        <td>
            <asp:Panel ID="ctlButtonAutoCompletePanel" runat="server">
                <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;"/>
                <input id="ctlBtnSupervisorAC" type="button" value="..." onclick="<%=ctlSupervisor.ClientID%>_onAutoCompleteViewList()" style="display:none;" />
            </asp:Panel>
            <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
        </td>
        <td>
            <asp:Label ID="ctlEmployeeName" runat="server" Style="display: none;" />
            <asp:Label ID="ctlUserID" runat="server" Style="display: none;" />
            <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="SupervisorAutoCompleteEx"
                ID="ctlSupervisorTextAutoComplete" TargetControlID="ctlSupervisor" ServicePath="~/WebService/SupervisorAutoComplete.asmx"
                ServiceMethod="GetUserList" MinimumPrefixLength="2" EnableCaching="true" SkinID="SkCtlAutoComplete"
                CompletionListElementID="ctlAutoCompleteSuggestionContainer" CompletionInterval="100"
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
                    var behavior = $find('SupervisorAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration=".2">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('SupervisorAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".2">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('SupervisorAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
                </Animations>
            </ajaxToolkit:AutoCompleteExtender>
        </td>
    </tr>
</table>
