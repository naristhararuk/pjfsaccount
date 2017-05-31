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
        <asp:ListItem Text="���Ҥ�" Value="1"></asp:ListItem>
        <asp:ListItem Text="����Ҿѹ��" Value="2"></asp:ListItem>
        <asp:ListItem Text="�չҤ�" Value="3"></asp:ListItem>
        <asp:ListItem Text="����¹" Value="4"></asp:ListItem>
        <asp:ListItem Text="����Ҥ�" Value="5"></asp:ListItem>
        <asp:ListItem Text="�Զع�¹" Value="6"></asp:ListItem>
        <asp:ListItem Text="�á�Ҥ�" Value="7"></asp:ListItem>
        <asp:ListItem Text="�ԧ�Ҥ�" Value="8"></asp:ListItem>
        <asp:ListItem Text="�ѹ��¹" Value="9"></asp:ListItem>
        <asp:ListItem Text="���Ҥ�" Value="10"></asp:ListItem>
        <asp:ListItem Text="��Ȩԡ�¹" Value="11"></asp:ListItem>
        <asp:ListItem Text="�ѹ�Ҥ�" Value="12"></asp:ListItem>
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
        <asp:ListItem Value="0" Text="�дѺ �3 ����"></asp:ListItem>
        <asp:ListItem Value="1" Text="�дѺ��ӡ��� �3"></asp:ListItem>
        <asp:ListItem Value="2" Text="������"></asp:ListItem>
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

