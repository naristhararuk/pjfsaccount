<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InboxAccountantPaymentSearchCriteria.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.InboxAccountantPaymentSearchCriteria"
    EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/InboxSearchResult/InboxEmployeeSearchResult.ascx"
    TagName="InboxEmployeeSearchResult" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" TagName="CompanyTextboxAutoComplete"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/PB.ascx" TagName="PB" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/ServiceTeam.ascx" TagName="ServiceTeam"
    TagPrefix="uc7" %>
<asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
    <contenttemplate> 
        <asp:UpdateProgress ID="ctlUpdateProgressSearch" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc5:SCGLoading ID="SCGLoading" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <fieldset style="width:99%;" id="fdsSearch" >
            <asp:Panel ID="pnSearchHeader" runat="server" HorizontalAlign="left" style="cursor:pointer;font-size:larger;color:Blue">
	        <asp:ImageButton ID="imgToggle" OnClientClick="return false;" runat="server" ImageUrl="~/App_Themes/Default/images/Slide/collapse.jpg" AlternateText="collapse" />
	        <b><asp:Label ID="lblStatus" runat="server" CssClass="searchBoxStatus" Text="Hide Search Box"></asp:Label></b>
    </asp:Panel>
            <asp:Panel ID="pnSearchCriteria" runat="server" DefaultButton="ctlSearch">
        <table width="98%" class="table" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    &nbsp;&nbsp;
                </td>
                <td style="width: 40%" align="right">
                    <asp:Label ID="ctlRequestTypeLabel" runat="server" Text="$Request Type$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ctlRequestType" runat="server" SkinID="SkGeneralDropdown" Width="145px" OnSelectedIndexChanged="ctlRequestType_OnSelectedIndexChanged" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
                <td style="width: 35%" align="right">
                    <asp:Label ID="ctlStatusLabel" runat="server" Text="$Status$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td  align="left">
                    <asp:DropDownList ID="ctlStatus" runat="server" SkinID="SkGeneralDropdown" Width="145px" OnSelectedIndexChanged="ctlStatus_OnSelectedIndexChanged" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
                <td style="width: 35%" align="right">
                    <asp:Label ID="ctlRoleLabel" runat="server" Text="$Role$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ctlRole" runat="server" SkinID="SkGeneralDropdown" Width="145px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Label ID="ctlCompanyLabel" runat="server" Text="$Company$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td>
                    <uc4:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete" runat="server"/>
                </td>
                <td>
                </td>
                <td>
                    <asp:CheckBox runat="server" SkinID="SkCtlCheckBox" ID="ctlCheckboxMultipleApprove" Text="Search For Multiple Approve" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Label ID="ctlRequestNoLabel" runat="server" Text="$Request No$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td colspan="5">
                    <asp:TextBox ID="ctlRequestNo" runat="server" SkinID="SkGeneralTextBox" MaxLength="15"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:Button ID="ctlReqNoOpenBtn" runat="server" SkinID="SkGeneralButton" Text="Open" OnClick="ctlReqNoOpenBtn_Click" Width="50" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td align="right">
                    <asp:Label ID="ctlCreatorLabel" runat="server" Text="$Creator$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupCreator" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="ctlRequesterLabel" runat="server" Text="$Requester$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupRequester" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="ctlReceiverLabel" runat="server" Text="$Receiver$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupReceiver" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td align="right">
                    <asp:Label ID="ctlRequestDateFromLabel" runat="server" SkinID="SkGeneralLabel" Text="$Request Date From$"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <uc1:Calendar ID="ctlRequestDateFrom" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="ctlRequestDateToLabel" runat="server" SkinID="SkGeneralLabel" Text="$To$"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td colspan="3">
                    <uc1:Calendar ID="ctlRequestDateTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td align="right">
                    <asp:Label ID="ctlSubjectLabel" runat="server" Text="$Subject$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td colspan="5">
                    <asp:TextBox ID="ctlSubject" runat="server" SkinID="SkGeneralTextBox" MaxLength="50" Width="140px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
                <td align="right">
                    <asp:Label ID="ctlAmountFromLabel" runat="server" SkinID="SkGeneralLabel" Text="$Amount From$"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="ctlAmountFrom" runat="server" SkinID="SkNumberTextBox" Width="140px"
                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));" OnKeyDown="disablePasteOption();"
                        OnKeyUp="disablePasteOption();"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Label ID="ctlAmountToLabel" runat="server" SkinID="SkGeneralLabel" Text="$To$"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td colspan="3">
                    <asp:TextBox ID="ctlAmountTo" runat="server" SkinID="SkNumberTextBox" Width="140px"
                        OnKeyPress="return(currencyFormat(this, ',', '.', event, 18));" OnKeyDown="disablePasteOption();"
                        OnKeyUp="disablePasteOption();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Label ID="ctlReferenceNoLabel" runat="server" Text="$Reference No$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="ctlRefenenceNo" runat="server" SkinID="SkGeneralTextBox" MaxLength="15" Width="140px"></asp:TextBox>
                    
                    &nbsp;&nbsp;
                    <asp:Button ID="ctlRefNoOpen" runat="server" SkinID="SkGeneralButton" 
                        Text="Open" Width="50px" onclick="ctlRefNoOpen_Click" />
                </td>
                <td align="right">
                    <div id="ctlImageOptionDiv" runat="server">
                        <asp:Label ID="ctlImageOptionLabel" runat="server" Text="Image Option" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                    </div>
                </td>
                <td colspan="3" rowspan="3">
                    <asp:CheckBox ID="ImageCheckBox" text = "Image" runat="server" /><br />
                    <asp:CheckBox ID="HardCopyCheckBox" Text = "Hard Copy" runat="server" /><br />
                    <asp:CheckBox ID="ImageHardCopy" text = "Image & Hard Copy" runat="server" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Label ID="ctlServiceTeamLabel" runat="server" Text="$Service Team$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td>
                    <uc7:ServiceTeam ID="ctlServiceTeam" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Label ID="ctlPBLabel" runat="server" Text="$PB$" SkinID="SkGeneralLabel"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td> 
                    <uc6:PB ID="ctlPB" runat="server"  />
                </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td colspan="5">
                    <asp:CheckBox ID="selecteHrFrom" Text = "  Select eHr From" runat="server" /> 
                </td>
            </tr>
        </table>
        <table class="table" border="0" >
            <tr>
                <td>&nbsp;
                    <asp:Button ID="ctlSearch" runat="server" SkinID="SkGeneralButton" Text="$Search$" OnClick="ctlSearch_Click" Width="100px" />
                </td> 
            </tr>
        </table>
    </asp:Panel>
        </fieldset>
        
        <ajaxToolkit:CollapsiblePanelExtender ID="collapPanel1" runat="Server"
	    TargetControlID="pnSearchCriteria"
	    ExpandControlID="pnSearchHeader"
	    CollapseControlID="pnSearchHeader"
	    Collapsed="false"
	    CollapsedImage="~/App_Themes/Default/images/Slide/expand.jpg"
	    ExpandedImage="~/App_Themes/Default/images/Slide/collapse.jpg"
	    TextLabelID="lblStatus"
	    ImageControlID="imgToggle" />
    </contenttemplate>
</asp:UpdatePanel>
<br />
<br />
<asp:UpdatePanel ID="ctlUpdatePanelSearchResult" runat="server" UpdateMode="Conditional">
    <contenttemplate>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <uc3:InboxEmployeeSearchResult ID="ctlInboxEmployeeSearchResult" runat="server"/>
                </td>
            </tr>
        </table>
    </contenttemplate>
</asp:UpdatePanel>
