<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorTextBoxAutoComplete.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.VendorTextBoxAutoComplete"
    EnableTheming="true" %>
<asp:HiddenField ID="ctlReturnAction" runat="server" OnValueChanged="ctlReturnAction_ValueChanged" />
<asp:HiddenField ID="ctlVendorChange" runat="server" OnValueChanged="ctlVendorChange_OnVendorChanged" />
<asp:HiddenField ID="ctlReturnValue" runat="server" />

<script language="javascript" type="text/javascript">
    function  <%=txtTaxNo.ClientID%>_OnSelected(source,eventArgs) {
        var selectedVendor = Sys.Serialization.JavaScriptSerializer.deserialize(eventArgs.get_value());

        $get('<%= txtTaxNo.ClientID %>').innerText = selectedVendor.VendorTaxCode;
        $get('<%= txtVendorName.ClientID %>').innerText = selectedVendor.VendorName;
        $get('<%= ctlVendorCode.ClientID %>').innerText = selectedVendor.VendorCode;
        $get('<%= ctlStreet.ClientID %>').innerText = selectedVendor.Street;
        $get('<%= ctlCity.ClientID %>').innerText = selectedVendor.City;
        $get('<%= ctlCountry.ClientID %>').innerText = selectedVendor.Country;
        $get('<%= ctlPostal.ClientID %>').innerText = selectedVendor.PostalCode;
        $get('<%= ctlVendorID.ClientID %>').innerText = selectedVendor.VendorID;

        var actionField = $get('<%= ctlReturnAction.ClientID %>');
        var valueField = $get('<%= ctlReturnValue.ClientID %>');
        actionField.value = "select";
        valueField.value = selectedVendor.VendorID;

        <%= Page.GetPostBackEventReference(ctlReturnAction) %>;
    }
    
    function <%=txtTaxNo.ClientID%>_OnPopulated(source,eventArgs) {
        $get('<%= ctlVendorCode.ClientID %>').innerText = "";
      
    }
    function <%=txtTaxNo.ClientID%>_OnPopulating(source, eventArgs) {
        //alert("OnDivisionPopulating");


    }
    function <%=txtTaxNo.ClientID%>_OnHiding(source, eventArgs) {
        //alert("OnHiding");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "none";
    }
    function <%=txtTaxNo.ClientID%>_OnHidden(source, eventArgs) {
        //alert("OnHidden");

    }
    function <%=txtTaxNo.ClientID%>_OnShowing(source, eventArgs) {
        //alert("OnDivisionShowing");
        $get('<%= ctlLoading.ClientID %>').style["display"] = "inline";
    }
    function <%=txtTaxNo.ClientID%>_OnShown(source, eventArgs) {
        //alert("OnDivisionShown");
    }
    function <%=txtTaxNo.ClientID%>_OnItemOut(source, eventArgs) {
        //alert("OnItemOut");
    }
    function <%=txtTaxNo.ClientID%>_onAutoCompleteViewList()
    {
        var autoComplete = document.getElementById("<%=txtTaxNo.ClientID%>").AutoCompleteBehavior;

        autoComplete._cache = {};
        autoComplete._currentPrefix = null;
        autoComplete._textBoxHasFocus = true;

        var oldMinimumPrefixLength = autoComplete.get_minimumPrefixLength();
        autoComplete.set_minimumPrefixLength(0);

        autoComplete._onTimerTick(null, null);
        
        autoComplete.set_minimumPrefixLength(oldMinimumPrefixLength);
    }
    
    function vendorTaxChange() 
    {
        var actionVendor = $get('<%= ctlVendorChange.ClientID %>');
        actionVendor.value = "vendorChange";
        <%= Page.GetPostBackEventReference(ctlVendorChange) %>;
    }
</script>

<asp:TextBox ID="txtTaxNo" runat="server" Width="100px" />
<%--<input type="button" value="..." onclick="<%=txtTaxNo.ClientID%>_onAutoCompleteViewList()" />--%>
<asp:Label ID="txtVendorName" runat="server" Style="display: none;" />
<asp:Label ID="ctlVendorCode" runat="server" Style="display: none;" />
<asp:Label ID="ctlVendorID" runat="server" Style="display: none;" />
<asp:Label ID="ctlStreet" runat="server" Style="display: none;" />
<asp:Label ID="ctlCity" runat="server" Style="display: none;" />
<asp:Label ID="ctlCountry" runat="server" Style="display: none;" />
<asp:Label ID="ctlPostal" runat="server" Style="display: none;" />
<asp:Image ID="ctlLoading" runat="server" SkinID="ctlLoading" Style="display: none;" />
<%--<ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="VendorAutoCompleteEx"
    ID="ctlVendorAutoComplete" TargetControlID="txtTaxNo" ServicePath="~/WebService/VendorAutoComplete.asmx"
    ServiceMethod="GetVendorList" MinimumPrefixLength="2" EnableCaching="true" SkinID="SkCtlAutoComplete"
    CompletionInterval="1000" CompletionSetCount="8">
    <Animations>
        <OnShow>
            <Sequence>--%>
                <%-- Make the completion list transparent and then show it --%>
                <%--<OpacityAction Opacity="0" />
                <HideAction Visible="true" />--%>
                
                <%--Cache the original size of the completion list the first time
                    the animation is played and then set it to zero --%>
                
                
                
                <%--<ScriptAction Script="
                    // Cache the size and setup the initial size
                    var behavior = $find('VendorAutoCompleteEx');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />--%>
                
                <%-- Expand from 0px to the appropriate size while fading in --%>
                
                
                
                <%--<Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('VendorAutoCompleteEx')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>--%>
            <%-- Collapse down to 0px and fade out --%>
            
            
            <%--<Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('VendorAutoCompleteEx')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>--%>
