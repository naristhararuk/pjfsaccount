<%@ 
Page Language="C#" 
AutoEventWireup="true" 
CodeBehind="ExportPayroll.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.Interface.Programs.ExportPayroll" 
MasterPageFile="~/ProgramsPages.Master" 
EnableTheming="true"
StylesheetTheme="Default"
%>

<%@ Register src="~/UserControls/LOV/SCG.DB/CompanyTextboxAutoComplete.ascx" tagname="CompanyTextboxAutoComplete" tagprefix="uc1" %>

<asp:Content ID="ctlContent" ContentPlaceHolderID="A" runat="server">
<asp:UpdatePanel ID="UpdatePanelPayrollCriteria" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<center>
<table style="text-align:left" class="table">
<tr>
<td><asp:Label Text="$Month :$ " ID="ctlLabelMonth" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
<td> <asp:DropDownList runat="server" SkinID="SkGeneralDropdown"  ID= "ctlDropDownMonth">
        <asp:ListItem Text="มกราคม" Value="1"></asp:ListItem>
        <asp:ListItem Text="กุมภาพันธ์" Value="2"></asp:ListItem>
        <asp:ListItem Text="มีนาคม" Value="3"></asp:ListItem>
        <asp:ListItem Text="เมษายน" Value="4"></asp:ListItem>
        <asp:ListItem Text="พฤษภาคม" Value="5"></asp:ListItem>
        <asp:ListItem Text="มิถุนายน" Value="6"></asp:ListItem>
        <asp:ListItem Text="กรกฎาคม" Value="7"></asp:ListItem>
        <asp:ListItem Text="สิงหาคม" Value="8"></asp:ListItem>
        <asp:ListItem Text="กันยายน" Value="9"></asp:ListItem>
        <asp:ListItem Text="ตุลาคม" Value="10"></asp:ListItem>
        <asp:ListItem Text="พฤศจิกายน" Value="11"></asp:ListItem>
        <asp:ListItem Text="ธันวาคม" Value="12"></asp:ListItem>
    </asp:DropDownList></td>
</tr>
<tr>
<td><asp:Label Text="$Year :$ " ID="ctlLabelYear" runat="server" SkinID="SkCtlLabel"></asp:Label></td>
<td><asp:TextBox runat="server" SkinID="SkCtlTextbox" ID="ctlTextboxYear" MaxLength="4"></asp:TextBox>
</td>
</tr>
<tr>
<td><asp:Label Text="$Company :$ " ID="ctlLabelCompany" runat="server" SkinID="SkCtlLabel"></asp:Label>
    <asp:Label id="ctlLableCompanyReq" runat="server" SkinID="SkRequiredLabel"></asp:Label></td>
<td><uc1:CompanyTextboxAutoComplete ID="ctlCompanyTextboxAutoComplete1" runat="server"/></td>
</tr>
<tr><td></td><td><asp:Button ID="btnAddCompany" runat="server" Text="Add" 
        SkinID ="SkCtlButton" onclick="btnAddCompany_Click" /></td>
</tr>
<tr>
<td></td>
<td>
    <asp:TextBox runat="server" Width="250" Height="80" MaxLength="1000" ID="txtCompanyList" ></asp:TextBox>
</td>
</tr>
<tr>
<td valign="top">
    <asp:Label runat="server" SkinID="SkCtlLabel" ID="ctlRadioLevel">Personal Level :</asp:Label>
</td>
<td>
    <asp:RadioButtonList runat="server" ID="rblPersonalLevel">
        <asp:ListItem Value="0" Text="ระดับ จ3 ขึ้นไป"></asp:ListItem>
        <asp:ListItem Value="1" Text="ระดับต่ำกว่า จ3"></asp:ListItem>
        <asp:ListItem Value="2" Text="ทั้งหมด"></asp:ListItem>
    </asp:RadioButtonList>
</td>
</tr>
</table>  
        <br />
            <asp:Button runat="server" Text="$Export$" SkinID ="SkCtlButton" 
        ID="ctlButtonExport" onclick="ctlButtonExport_Click"/>
        
           <asp:Button runat="server" Text="$Print$" SkinID ="SkCtlButton" 
        ID="ctlButtonPrint" onclick="ctlPrint_Click"/>
                &nbsp;<ajaxToolkit:FilteredTextBoxExtender
        ID="FilteredTextBoxExtender1"
        runat="server"
        TargetControlID="ctlTextboxYear"
        FilterType="Numbers" />
&nbsp;
         <font color="red">
                <spring:ValidationSummary runat="server" ID="ctlvalidationSummary" Provider="Export.Error">
                </spring:ValidationSummary>

            </font>
</center>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

