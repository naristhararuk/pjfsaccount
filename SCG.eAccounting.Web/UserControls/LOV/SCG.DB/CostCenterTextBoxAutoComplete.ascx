<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostCenterTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CostCenterTextBoxAutoComplete"
    EnableTheming="true" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />
<asp:HiddenField ID="ctlCallCompanyID" runat="server" />
<ss:InlineScript ID="InlineScript1" runat="server">
    <script language="javascript" type="text/javascript">
    function  <%= ctlCostCenter.ClientID %>_OnSelected(source,eventArgs) {
        //alert('onselected');
        
        var selectedCostCenter = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());
        $get('<%= ctlCostCenter.ClientID %>').innerText = selectedCostCenter.Code;
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedCostCenter.ID;

    }
    
    function <%= ctlCostCenter.ClientID %>_OnPopulated(source,eventArgs) {
        //alert('OnCostCenterPopulated');
        $get('<%= ctlCostCenter.ClientID %>').innerText = "";
      
    }
    function <%= ctlCostCenter.ClientID %>_OnPopulating(source, eventArgs) {
        //alert("OnDivisionPopulating");


    }
    function <%= ctlCostCenter.ClientID %>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%= ctlCostCenter.ClientID %>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%= ctlCostCenter.ClientID %>_OnShowing(source, eventArgs) {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%= ctlCostCenter.ClientID %>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
       
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%= ctlCostCenter.ClientID %>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }
    function <%=ctlCostCenter.ClientID%>_onAutoCompleteViewList()
    {
        var autoComplete = document.getElementById("<%=ctlCostCenter.ClientID%>").AutoCompleteBehavior;
        
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
<asp:UpdatePanel ID="ctlCostCenterTexboxAutoCompleteUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:TextBox ID="ctlCostCenter" runat="server" Width="100px" MaxLength="20" SkinID="SkGeneralTextBox"
                        Style="text-align: center;" OnTextChanged="ctlCostCenter_TextChanged" AutoPostBack="true" />
                    <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                        class="autocomplete_completionListElement">
                    </div>
                </td>
                <td>
                    <asp:Panel ID="ctlButtonAutoCompletePanel" runat="server">
                        <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;" />
                        <input id="ctlBtnCostCenterAC" type="button" value="..." onclick="<%=ctlCostCenter.ClientID%>_onAutoCompleteViewList()"
                            style="display: none;" />
                    </asp:Panel>
                    <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
                </td>
                <td>
                    <asp:Label ID="ctlCostCenterID" runat="server" Style="display: none;" />
                    <asp:HiddenField ID="ctlCompanyID" runat="server" />
                </td>
            </tr>
        </table>
        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="CostCenterAutoCompleteEx"
            ID="ctlCostCenterAutoComplete" TargetControlID="ctlCostCenter" ServicePath="~/WebService/CostCenterAutoComplete.asmx"
            ServiceMethod="GetCostCenterList" MinimumPrefixLength="2" EnableCaching="true"
            SkinID="SkAutoCompleteItem" CompletionListElementID="ctlAutoCompleteSuggestionContainer"
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
                                var behavior = $find('CostCenterAutoCompleteEx');
                                if (!behavior._height) {
                                    var target = behavior.get_completionList();
                                    behavior._height = target.offsetHeight - 2;
                                    target.style.height = '0px';
                                }" />
                            
                            <%-- Expand from 0px to the appropriate size while fading in --%>
                            
                            
                            
                            <Parallel Duration="0">
                                <FadeIn />
                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('CostCenterAutoCompleteEx')._height" />
                            </Parallel>
                        </Sequence>
                    </OnShow>
                    <OnHide>
                        <%-- Collapse down to 0px and fade out --%>
                        
                        
			            <Parallel Duration="0">
                            <FadeOut />
                            <Length PropertyKey="height" StartValueScript="$find('CostCenterAutoCompleteEx')._height" EndValue="0" />
                        </Parallel>
                    </OnHide>
            </Animations>
        </ajaxToolkit:AutoCompleteExtender>
    </ContentTemplate>
</asp:UpdatePanel>
