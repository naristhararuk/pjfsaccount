<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.AccountTextBoxAutoComplete"
    EnableTheming="true" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />
<ss:InlineScript ID="InlineScript1" runat="server">

    <script language="javascript" type="text/javascript">
    function <%=ctlAccountCode.ClientID%>_OnSelected(source, eventArgs) {
        var selected = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());

        $get('<%=ctlAccountCode.ClientID%>').innerText = selected.AccountCode;
        
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selected.AccountID;
    }

    function <%=ctlAccountCode.ClientID%>_OnPopulated(source, eventArgs) {
        $get('<%=ctlAccountCode.ClientID%>').innerText = "";
    } 
    function <%=ctlAccountCode.ClientID%>_OnPopulating(source, eventArgs) {
        //alert("OnPopulating");


    }
    function <%=ctlAccountCode.ClientID%>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%=ctlAccountCode.ClientID%>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%=ctlAccountCode.ClientID%>_OnShowing(source, eventArgs) {
        //alert("OnShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%=ctlAccountCode.ClientID%>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%=ctlAccountCode.ClientID%>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }  
    function <%=ctlAccountCode.ClientID%>_onAutoCompleteViewList()
{
    var autoComplete = document.getElementById("<%=ctlAccountCode.ClientID%>").AutoCompleteBehavior;

    autoComplete._cache = {};
    autoComplete._currentPrefix = null;
    autoComplete._textBoxHasFocus = true;

    var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
    autoComplete.set_minimumPrefixLength(0);

    autoComplete._onTimerTick(null, null);
    
    autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
}
    </script>

</ss:InlineScript>

<asp:UpdatePanel ID="ctlAccountAutoCompleteUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:TextBox ID="ctlAccountCode" runat="server" Width="100px" SkinID="SkGeneralTextBox" MaxLength="20"
                    Style="text-align: center;" OnTextChanged="ctlAccountCode_TextChanged" AutoPostBack="true"/>
                <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                    class="autocomplete_completionListElement">
                </div>
            </td>
            <td>
                <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;"/>
                <input id="ctlBtnAccountAC" type="button" value="..." onclick="<%=ctlAccountCode.ClientID%>_onAutoCompleteViewList();" style="display:none;" />
                <asp:Label ID="ctlAccountID" runat="server" Style="display: none;" />
                <asp:Label ID="ctlAccountText" runat="server" Style="display: none;" />
            </td>
        </tr>
        </table>
        <%--<asp:Label ID="ctlCompanyCode" runat="server" Style="display: none;" />

<asp:Label ID="ctlCompanyId" runat="server" Style="display: none;" />
<asp:Label ID="ctlCostCenterCode" runat="server" Style="display: none;" />
<asp:Label ID="ctlCostCenterId" runat="server" Style="display: none;" />--%>
        <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AccountAutoCompleteEx"
            ID="ctlAccountTextAutoComplete" TargetControlID="ctlAccountCode" ServicePath="~/WebService/AccountAutoComplete.asmx"
            ServiceMethod="GetAccountList" MinimumPrefixLength="2" EnableCaching="true" SkinID="SkAutoCompleteItem"
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
                    var behavior = $find('AccountAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration="0">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AccountAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration="0">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('AccountAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
            </Animations>
        </ajaxToolkit:AutoCompleteExtender>
    </ContentTemplate>
</asp:UpdatePanel>
