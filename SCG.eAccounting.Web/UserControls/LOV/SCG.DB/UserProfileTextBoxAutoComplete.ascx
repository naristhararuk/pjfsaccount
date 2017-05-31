<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="UserProfileTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.UserProfileTextBoxAutoComplete" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />

<script language="javascript" type="text/javascript">
    function <%=ctlUserID.ClientID%>_OnSelected(source, eventArgs) {
        var selected = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());

        $get('<%= ctlUserID.ClientID %>').innerText = selected.UserName;
        
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selected.UserID;
    }

    function <%=ctlUserID.ClientID%>_OnPopulated(source, eventArgs) {
        $get('<%= ctlUserID.ClientID %>').innerText = "";
    } 
    function <%=ctlUserID.ClientID%>_OnPopulating(source, eventArgs) {
        //alert("OnPopulating");

    }
    function <%=ctlUserID.ClientID%>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%=ctlUserID.ClientID%>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%=ctlUserID.ClientID%>_OnShowing(source, eventArgs) {
        //alert("OnShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%=ctlUserID.ClientID%>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%=ctlUserID.ClientID%>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }  
    function <%=ctlUserID.ClientID%>_onAutoCompleteViewList()
{
    var autoComplete = document.getElementById("<%=ctlUserID.ClientID%>").AutoCompleteBehavior;

    autoComplete._cache = {};
    autoComplete._currentPrefix = null;
    autoComplete._textBoxHasFocus = true;

    var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
    autoComplete.set_minimumPrefixLength(0);

    autoComplete._onTimerTick(null, null);
    
    autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
}

</script>

<asp:UpdatePanel ID="ctlUpdatePanelUser" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="ctlEmployeeID" runat="server" Style="display: none;" />
        <asp:Label ID="ctlEmployeeName" runat="server" Style="display: none;" />
<%--<asp:Label ID="ctlCompanyCode" runat="server" Style="display: none;" />
<asp:Label ID="ctlCompanyId" runat="server" Style="display: none;" />
<asp:Label ID="ctlCostCenterCode" runat="server" Style="display: none;" />
<asp:Label ID="ctlCostCenterId" runat="server" Style="display: none;" />--%>
        <table border="0" cellpadding="0" cellspacing="0" class="table">
            <tr>
                <td>
                    <asp:TextBox ID="ctlUserID" runat="server" SkinID="SkGeneralTextBox" MaxLength="20" OnTextChanged="ctlUserID_TextChanged" AutoPostBack="true" />
                    <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                        class="autocomplete_completionListElement">
                    </div>
                </td>
                <td>
                    <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;"/>
                    <input id="ctlBtnUserAC" type="button" value="..." onclick="<%=ctlUserID.ClientID%>_onAutoCompleteViewList()" style="display:none;"/>
                    <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
                </td>
                <td>
                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="" ID="ctlUserTextAutoComplete"
                        TargetControlID="ctlUserID" ServicePath="~/WebService/UserProfileAutoComplete.asmx"
                        ServiceMethod="GetUserList" MinimumPrefixLength="2" EnableCaching="true" SkinID="SkCtlAutoComplete"
                        CompletionListElementID="ctlAutoCompleteSuggestionContainer" CompletionInterval="100"
                        CompletionSetCount="20">
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
                
                
                
                <Parallel Duration=".2">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('UserAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".2">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('UserAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
                        </Animations>
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
