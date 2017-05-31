<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActorData.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.ActorData" %>
<%@ Register Src="~/Usercontrols/LOV/SS.DB/UserLookup.ascx" TagName="UserLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<asp:UpdatePanel ID="ctlUpdatePanelActorData" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="ctlUpdateProgressActorData" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelActorData"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc4:SCGLoading ID="SCGLoading1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table id="ctlTableFieldSet" runat="server">
            <tr>
                <td valign="top">
                    <fieldset id="ctlFieldSetActorData" runat="server" class="table">
                        <legend id="legSearch" style="color: #4E9DDF" class="table">
                            <asp:Label ID="ctlActorDataLegend" SkinID="SkCtlLabel" runat="server" Text="Data"></asp:Label>
                        </legend>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" runat="server" id="ctlSearchUser">
                                        <tr>
                                            <td>
                                                <asp:Label ID="ctlUserIDLabel" runat="server" Text="$User ID$" SkinID="SkFieldCaptionLabel"></asp:Label>
                                                <asp:Label ID="ctlUserIDReq" runat="server" Text="*" SkinID="SkRequiredLabel"></asp:Label>&nbsp;:&nbsp;
                                            </td>
                                            <td>
                                                <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookup" runat="server" />
                                                <ss:LabelExtender ID="ctlUserAutoCompleteLookupExtender" runat="server" LinkControlID="ctlUserAutoCompleteLookup"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# this.ControlGroupID %>'
                                                    SkinID="SkGeneralLabel"></ss:LabelExtender>
                                            </td>
                                            <td>
                                                &nbsp;<asp:ImageButton runat="server" ID="ctlFavorite" ToolTip="Search" SkinID="SkCtlFavorite"
                                                    OnClick="ctlFavorite_Click" />
                                            </td>
                                            <td>
                                                &nbsp;<asp:CheckBox ID="ctlSMSCheckBox" runat="server" Enabled="false" Text="SMS"
                                                    SkinID="SkGeneralCheckBox" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" runat="server" id="ctlUserDiv"
                                        visible="false">
                                        <tr>
                                            <td>
                                                <asp:Label ID="ctlUserLabel" runat="server" SkinID="SkFieldCaptionLabel" Text="$User ID$ :" />&nbsp;:&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="ctlUserLoginName" SkinID="SkGenetalLable" runat="server" />
                                                <ss:LabelExtender ID="ctlUserLoginNameLabelExtender" runat="server" LinkControlID="ctlUserLoginName"
                                                    InitialFlag='<%# this.InitialFlag %>' LinkControlGroupID='<%# this.ControlGroupID %>'
                                                    SkinID="SkGeneralLabel"></ss:LabelExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ctlName" runat="server" SkinID="SkGeneralLabel" />&nbsp;
                                    <asp:Label ID="ctlEmployeeCode" runat="server" SkinID="SkGeneralLabel" />&nbsp;/
                                    <div id="ctlVendorCodeDiv" runat="server" style="display:inline">
                                        <asp:Label ID="ctlVendorCode" runat="server" SkinID="SkGeneralLabel" />&nbsp;/
                                    </div>
                                    <asp:Label ID="ctlPositionName" runat="server" SkinID="SkGeneralLabel" /><br />
                                    <asp:Label ID="ctlOrganization" runat="server" SkinID="SkGeneralLabel" />&nbsp;/
                                    <asp:Label ID="ctlDivision" runat="server" SkinID="SkGeneralLabel" /><br />
                                    <asp:Label ID="ctlEMailAddress" runat="server" SkinID="SkGeneralLabel" />&nbsp;/
                                    <asp:Label ID="ctlPhoneNumber" runat="server" SkinID="SkGeneralLabel" />&nbsp;/
                                    <asp:Label ID="ctlCostCenterCode" runat="server" SkinID="SkGeneralLabel" />
                                    <uc3:UserProfileLookup ID="ctlUserFavoriteLookup" runat="server" isMultiple="false"
                                        SearchFavoriteApprover="true" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
            <asp:Label ID="ctlCompanyIDLabel" SkinID="SkCtlLabel" runat="server" Style="display: none;" />
            <asp:Label ID="ctlCompanyCodeLabel" SkinID="SkCtlLabel" runat="server" Style="display: none;" />
            <asp:Label ID="ctlUserID" SkinID="SkCtlLabel" runat="server" Style="display: none;" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
