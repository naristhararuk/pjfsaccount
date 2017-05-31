<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="VehicleMileageCriteria.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.VehicleMileageCriteria" 
EnableTheming="true"%>
<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" tagname="CompanyField" tagprefix="uc1" %>
<%@ Register Src="~/Usercontrols/LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup" TagPrefix="uc2" %>
<%@ Register src="~/Usercontrols/LOV/SCG.DB/TALookupField.ascx" tagname="TALookupField" tagprefix="uc3" %>
<asp:UpdatePanel ID="UpdatePanelVendor" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:60%" class="table">
    <legend id="legSearch" style="color:#4E9DDF" class="table"><asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
    </legend>
    <table class="table" width="600px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="ctlCompanyText" runat="server" Text="$Company$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <uc1:CompanyField ID="ctlCompanyField" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromRequesterIdText" runat="server" Text="$From Requester$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc2:UserAutoCompleteLookup id="ctlFromRequesterID" runat="server"></uc2:UserAutoCompleteLookup>        
            </td>
            <td>
                <asp:Label ID="ctlToRequesterIDText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                <uc2:UserAutoCompleteLookup id="ctlToRequesterID" runat="server"></uc2:UserAutoCompleteLookup>   
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromCarRegisText" runat="server" Text="$From Car Registration$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <asp:TextBox ID="ctlFromCarRegis" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="ctlToCarRegisText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <asp:TextBox ID="ctlToCarRegis" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlFromTaNoText" runat="server" Text="$From TA No.$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <uc3:TALookupField ID="ctlFromTaNo" runat="server"></uc3:TALookupField>
            </td>
            <td>
                <asp:Label ID="ctlToTaNoText" runat="server" Text="$To$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
             <td>
                 <uc3:TALookupField ID="ctlToTaNo" runat="server" ></uc3:TALookupField>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlDocumentStatusText" runat="server" Text="$Document Status$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="ctlDocumentStatus" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="ALL">ALL</asp:ListItem>
                    <asp:ListItem Value="Complete">Completed</asp:ListItem>
                    <asp:ListItem Value="Hold">Hold</asp:ListItem>
                    <asp:ListItem Value="Reject">Reject</asp:ListItem>
                    <asp:ListItem Value="WaitApprove">Wait for Approve</asp:ListItem>
                    <asp:ListItem Value="WaitApproveVerify">Wait for Approve Verify</asp:ListItem>
                    <asp:ListItem Value="WaitInitial">Wait for Initial</asp:ListItem>
                    <asp:ListItem Value="WaitPayment">Wait for Payment</asp:ListItem>
                    <asp:ListItem Value="WaitReceive">Wait for Receive Document</asp:ListItem>
                    <asp:ListItem Value="WaitRemittance">Wait for Remittance</asp:ListItem>
                    <asp:ListItem Value="WaitVerify">Wait for Verify</asp:ListItem>    
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
