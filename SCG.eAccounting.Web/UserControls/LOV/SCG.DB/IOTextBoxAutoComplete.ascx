<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IOTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.IOTextBoxAutoComplete"
    EnableTheming="true" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<asp:HiddenField ID="ctlReturnAction" runat="server" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />
<ss:InlineScript ID="InlineScript1" runat="server">

    <script language="javascript" type="text/javascript">
    function <%= ctlIO.ClientID %>_OnSelected(source, eventArgs) {
        var selectedIO = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());

        $get('<%= ctlIO.ClientID %>').innerText = selectedIO.IONumber;
        
        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedIO.IOID;
    }

    function <%= ctlIO.ClientID %>_OnPopulated(source, eventArgs) {
        $get('<%= ctlIOID.ClientID %>').innerText = "";
    } 
    function <%= ctlIO.ClientID %>_OnPopulating(source, eventArgs) {
        //alert("OnPopulating");


    }
    function <%= ctlIO.ClientID %>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%= ctlIO.ClientID %>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%= ctlIO.ClientID %>_OnShowing(source, eventArgs) {
        //alert("OnShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%= ctlIO.ClientID %>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
        var behavior = $get('<%= ctlAutoCompleteSuggestionContainer.ClientID %>');

    	if (behavior != null)
			behavior.style.width = 200;
    }
    function <%= ctlIO.ClientID %>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }  
    function <%=ctlIO.ClientID%>_onAutoCompleteViewList()
{
    var autoComplete = document.getElementById("<%=ctlIO.ClientID%>").AutoCompleteBehavior;

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

<asp:UpdatePanel ID="UpdatePanelIO" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<table id="ctlContainer" runat="server" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
        
            
            <asp:TextBox ID="ctlIO" runat="server" Width="100px" MaxLength="20" SkinID="SkGeneralTextBox"
                Style="text-align: center;" OnTextChanged="ctlIO_TextChanged" AutoPostBack="true" />         
            
            <div id="ctlAutoCompleteSuggestionContainer" runat="server" style="display: none;"
                class="autocomplete_completionListElement">
            </div>
        </td>
        <td>
            <asp:ImageButton ID="ctlACBtn" runat="server" SkinID="SkAutoCompleteButton" OnClientClick="this.parentElement.children[1].click();return false;" />
            <input id="ctlBtnIOAC" type="button" value="..." onclick="<%=ctlIO.ClientID%>_onAutoCompleteViewList()"
                style="display: none;" />
        </td>
        <td>
            <asp:Label ID="ctlIOID" runat="server" Style="display: none;" />
            <asp:Label ID="ctlIOText" runat="server" Style="display: none;" />
            <asp:Label ID="ctlCompanyCode" runat="server" Style="display: none;" />
            <asp:Label ID="ctlCompanyId" runat="server" Style="display: none;" />
            <asp:Label ID="ctlCostCenterCode" runat="server" Style="display: none;" />
            <asp:Label ID="ctlCostCenterId" runat="server" Style="display: none;" />
            <asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
            <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="IOAutoCompleteEx" ID="ctlIOTextAutoComplete"
                TargetControlID="ctlIO" ServicePath="~/WebService/IOAutoComplete.asmx" ServiceMethod="GetIOList"
                MinimumPrefixLength="2" EnableCaching="true" SkinID="SkCtlAutoComplete" CompletionListElementID="ctlAutoCompleteSuggestionContainer"
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
                    var behavior = $find('IOAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <Parallel Duration=".2">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('IOAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <Parallel Duration=".2">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('IOAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
                </Animations>
            </ajaxToolkit:AutoCompleteExtender>
        </td>
    </tr>
</table>

</ContentTemplate>
</asp:UpdatePanel> 

<asp:Panel ID="pnViewPostShow1" runat="server" Width="400px" BackColor="White" Style="display: none">
    
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnViewPostShowHeader1" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;cursor:move;">
            <asp:Label ID="lblSpace1" runat="server" Text='&nbsp;&nbsp;LOG IN ...'></asp:Label>
            </asp:Panel>
        </td>
        <td align="right" valign="top" >
            <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="imgClose_Click" />
        </td>
    </tr>
    </table>
    
	<asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	    
	    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelSearchAccount"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <table width="400px">
            <tr align="center">
                <td>
                   <asp:Label ID="ctlErrorValidationLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnOK"     runat="server" Text="OK"      OnClick="btnOK_Click"    Width="100px"/>
                    <asp:Button ID="btnClose"  runat="server" Text="Close"   OnClick="btnClose_Click" Width="100px"/>
                </td>
            </tr>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummy1" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1ShowMessage" runat="server" 
	TargetControlID="lnkDummy1"
	PopupControlID="pnViewPostShow1"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy1"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="pnViewPostShowHeader1"
	/>