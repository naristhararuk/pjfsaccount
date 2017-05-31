<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InboxEmployeeSearchCriteria.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.InboxSearchResult.InboxEmployeeSearchCriteria" EnableTheming="true" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" tagname="UserAutoCompleteLookup" tagprefix="uc2" %>

<asp:UpdatePanel ID="ctlUpdatePanelSearchResult" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%" class="table" >
            <tr>
                <td>
                    <fieldset id="ctlFieldSearch" runat="server" class="table">
                        <legend id="ctlLegendSearchCriteria" style="color: #4E9DDF">
                            <asp:Label ID="ctlSearchCriteriaHeader" runat="server" Text="$Filter$"></asp:Label>
                        </legend>
                        <table width="100%" class="table" border="0">
                            <tr>
                                <td style="width:20%" align="left">
                                    <asp:Label ID="ctlRequestNoLabel" runat="server" SkinID="SkGeneralLabel" Text="$Request No$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td  style="width:20%"   align="left">
                                    <asp:TextBox ID="ctlRequestNo" runat="server" SkinID="SkGeneralTextBox" Width="175px" MaxLength="15"></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td style="width:20%" align="left">
                                    <asp:Label ID="ctlRequestDateFrom" runat="server" SkinID="SkGeneralLabel" Text="$Request Date From$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td style="width:20%" align="left">
                                    <uc1:Calendar ID="ctlRequestDateFromCal" runat="server" />                  
                                </td>
                                <td style="width:10%" align="right">
                                    <asp:Label ID="ctlRequestDateTo" runat="server" SkinID="SkGeneralLabel" Text="$To$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td>
                                    <uc1:Calendar ID="ctlRequestDateToCal" runat="server" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%" align="left">
                                    <asp:Label ID="ctlCreator" runat="server" SkinID="SkGeneralLabel" Text="$Creator$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td  style="width:3%;text-align:left;" colspan="3">
                                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupCreator" runat="server" DisplayCaption="true" />
                                </td>
                                <td colspan="2" >
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%" align="left">
                                    <asp:Label ID="ctlRequester" runat="server" SkinID="SkGeneralLabel" Text="$Requester$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td  style="width:3%;text-align:left;" colspan="3">
                                    <uc2:UserAutoCompleteLookup ID="ctlUserAutoCompleteLookupRequester" runat="server" DisplayCaption="true" />
                                </td>
                                <td colspan="2" >
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%" align="left">
                                    <asp:Label ID="ctlRequestTypeLabel" runat="server" SkinID="SkGeneralLabel" Text="$Request Type$"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td colspan="3" style="width:20%" align="left">
                                    <asp:DropDownList ID="ctlRequestType" runat="server" SkinID="SkGeneralDropdown" Width="175px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>    
                   
            