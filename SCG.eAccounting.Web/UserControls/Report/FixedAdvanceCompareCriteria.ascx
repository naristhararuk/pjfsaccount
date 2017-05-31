<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="FixedAdvanceCompareCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.FixedAdvanceCompareCriteria" 
EnableTheming="true"%>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" tagname="CompanyField" tagprefix="uc1" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/LocationField.ascx" tagname="LocationField" tagprefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc3" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CostCenterField.ascx" TagName="CostCenterField" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/DropdownList/SCG.DB/ServiceTeam.ascx" TagName="ServiceTeam" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/DocumentEditor/Components/ActorData.ascx" TagName="ActorData" TagPrefix="uc7" %>
<asp:UpdatePanel ID="UpdatePanelVendor" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:60%" class="table">
    <legend id="legSearch" style="color:#4E9DDF" class="table"><asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
    </legend>
    <table class="table" width="600px" border="0" cellpadding="0" cellspacing="0">
        <tr align="left">
            <td>
                <asp:Label ID="ctlRequesterDataText" runat="server" Text ="$Requester$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan = "3" align="left">
                <uc4:UserAutoCompleteLookup ID="ctlRequesterData" runat="server"/>		
               <%-- <uc7:ActorData ID="ctlRequesterData" Legend='<%# GetProgramMessage("ctlRequesterData") %>'
                    ShowSMS="false" ShowVendorCode="true" ShowFavoriteButton="false" ShowSearchUser="true" runat="server"
                    Width="100%" ControlGroupID='<%# SCG.eAccounting.BLL.Implement.ExpenseFieldGroup.BuActor %>' />--%>
            </td>
        </tr>     
        <%--<tr>
            <td>
                <asp:Label ID="ctlApproverDataLabel" runat="server" Text="$Approver$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="ctlUpdatePanelApprover" runat="server" UpdateMode="Conditional">
                  <ContentTemplate>
                     <uc4:UserAutoCompleteLookup ID="ctlApproverData" runat="server"/>	
                  </ContentTemplate>
                </asp:UpdatePanel>	
            </td>
        </tr>--%>
        <tr>
            <td>
                <asp:Label ID="ctlDocumentNoText" runat="server" Text="$Document No.$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
               <asp:TextBox ID="ctlDocumentNo" runat="server" SkinID="SkGeneralTextBox" MaxLength="15"></asp:TextBox>               
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromDateText" runat="server" Text="$From (Date)$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <uc3:Calendar ID="ctlFromDateCalendar" runat="server"></uc3:Calendar>
            </td>
            <td>
                <asp:Label ID="ctlToDateText" runat="server" Text="$To (Date)$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <uc3:Calendar ID="ctlToDateCalendar" runat="server"></uc3:Calendar>
                  <asp:UpdatePanel ID="ctlValidateDate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="DateMaximumRange" SkinID="SkGeneralLabel" runat="server" Text="%DateMaximumRange%" 
                        ForeColor="Red" Visible="false" ></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlReportTypeText" runat="server" Text="$Report Type$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <asp:RadioButtonList ID="ctlReportType" runat="server" RepeatLayout="Flow">
                    <asp:ListItem Value="1" Selected="true">แสดงยอดรวมการเบิกและกราฟ</asp:ListItem>
                    <asp:ListItem Value="2">แสดงรายละเอียดการเบิกตามประเภทค่าใช้จ่าย</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>