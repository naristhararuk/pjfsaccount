<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyTextboxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CompanyTextboxAutoComplete" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />
<asp:HiddenField ID="ctlFlagUseEccOnly" runat="server" />
<asp:HiddenField ID="ctlFlagUseExpOnly" runat="server" />
<asp:HiddenField ID="ctlFlagActive" runat="server" />
<script language="javascript" type="text/javascript">
    function <%= ctlCompanyCode.ClientID %>_OnSelected(source, eventArgs) {
       // alert('onselected');
        var selectedCompany = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlCompanyCode.ClientID %>').innerText = selectedCompany.CompanyCode;
//        $get('<%= ctlCompanyID.ClientID %>').innerText = selectedCompany.CompanyID;       
//        $get('<%= ctlCompanyName.ClientID %>').innerText = selectedCompany.CompanyName;  
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        var objCompanyId = $get('<%= ctlCompanyID.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedCompany.CompanyID;
        objCompanyId.innerText = selectedCompany.CompanyID;
    }

    function <%= ctlCompanyCode.ClientID %>_OnPopulated(source, eventArgs) {
        //alert('OnCountryPopulated');
        $get('<%= ctlCompanyID.ClientID %>').innerText = "";
      
    }
    function <%= ctlCompanyCode.ClientID %>_OnPopulating(source, eventArgs) {
        //alert("OnDivisionPopulating");


    }
    function <%= ctlCompanyCode.ClientID %>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%= ctlCompanyCode.ClientID %>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%= ctlCompanyCode.ClientID %>_OnShowing(source, eventArgs) {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%= ctlCompanyCode.ClientID %>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%= ctlCompanyCode.ClientID %>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }
    function <%=ctlCompanyCode.ClientID%>_onAutoCompleteViewList()
    {
        var autoComplete = document.getElementById("<%=ctlCompanyCode.ClientID%>").AutoCompleteBehavior;

        autoComplete._cache = {};
        autoComplete._currentPrefix = null;
        autoComplete._textBoxHasFocus = true;

        var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
        autoComplete.set_minimumPrefixLength(0);

        autoComplete._onTimerTick(null, null);
        
        autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
      
    }
    
</script>

<asp:UpdatePanel ID="ctlCompanyAutoCompleteUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<asp:Label ID="ctlCompanyID" runat="server" SkinID="SkGeneralLabel" Style="display: none;" />
<asp:Label ID="ctlCompanyName" runat="server" SkinID="SkGeneralLabel" Style="display: none;" />
<table id="ctlContainerTable" runat="server" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="ctlCompanyCode" runat="server" Width="60px" MaxLength="4" Style="text-align: center;"
                SkinID="SkGeneralTextBox" OnTextChanged="ctlCompanyCode_TextChanged" AutoPostBack="true" />
            <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                class="autocomplete_completionListElement">
            </div>
        </td>
        <td>
            <asp:Panel ID="ctlButtonAutoCompletePanel" runat="server">
                <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;"/>
                <input id="ctlBtnCompanyAC" type="button" value="..." onclick="<%=ctlCompanyCode.ClientID%>_onAutoCompleteViewList()" style="display:none;" />
            </asp:Panel>
            <asp:Image ID="ctlLoading" runat="server" SkinID="SkAutoCompleteLoading" Style="display: none;" />
        </td>
        <td>
            <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="CompanyAutoCompleteEx"
                ID="ctlCompanyAutoComplete" TargetControlID="ctlCompanyCode" ServicePath="~/WebService/CompanyAutoComplete.asmx"
                ServiceMethod="GetCompanyList" MinimumPrefixLength="2" EnableCaching="true" SkinID="SkAutoCompleteItem"
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
                    var behavior = $find('CompanyAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration="0">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('CompanyAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            <Parallel Duration="0">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('CompanyAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
                </Animations>
            </ajaxToolkit:AutoCompleteExtender>
        </td>
    </tr>
</table>
    </ContentTemplate>
</asp:UpdatePanel>
