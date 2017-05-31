<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ChangePassword.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ChangePassword"
    EnableTheming="true" 
    
%>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Panel ID="pnChangePassword" runat="server" Width="700px" style="display:none;" BackColor="White">

    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <asp:Panel ID="pnChangePasswordHeader" CssClass="table" runat="server" Style="border:solid 1px Gray;color:Black;background:#33CCFF;cursor:move" >
                <asp:Label ID="ctlChangePasswordHeaderLabel" runat="server" Text='$ChangePassword$' />
            </asp:Panel>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif"
                    OnClick="imgClose_Click" />
        </td>
    </tr>
    </table>
    	    
    <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
    <asp:UpdateProgress ID="UpdatePanelGridViewProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelMessage"
        DynamicLayout="true" EnableViewState="true">
        <ProgressTemplate>
            <uc3:SCGLoading ID="SCGLoading1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress> 
    
    <div align="center">
       <fieldset style="width: 90%" id="fdsSearch" class="table">  
        <table width="100%">
        <tr>
            <td colspan="2" align="left">
            <%--<b>นโยบาย SCG IT Policy</b> --%>
            <b><asp:Label ID="ctlPolicyHeader" runat="server"></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <%--1. มาตรฐานการตั้งรหัสผ่าน หัวข้อ ข้อกำหนด คำอธิบาย<br />
            Maximum Password Age 90 วัน ระยะเวลาที่หมดอายุของ Password<br />
            Minimum Password Age 2 วัน ระยะเวลาที่อนุญาตให้เปลี่ยน Password ได้<br />
            Minimum Password Length 8 ตัว จำนวนตัวอักษรของ Password ต้องไม่น้อยกว่าจำนวนที่กำหนด <br />
            Password History 4 ครั้ง จำนวนครั้งที่สามารถกลับมาตั้ง Password ซ้ำเดิม<br />
            2. ห้ามตั้งรหัสผ่าน เหมือนชื่อ User และห้ามตั้งรหัสผ่านเป็น = “Password” เนื่องจากสามารถคาดเดาได้ง่าย<br />
            3. เปลี่ยนรหัสผ่านทันที เมื่อได้รับแจ้งรหัสผ่านในครั้งแรก<br />--%>
            <asp:Label ID="ctlPolicyDetails" runat="server" ></asp:Label>
            <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <%--<b>ข้อแนะนำ</b>--%>
            <b><asp:Label ID="ctlGuideHeader" runat="server" ></asp:Label></b>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <%--• ไม่ควรตั้งรหัสผ่าน ให้เป็นคำที่มีความหมายตาม Dictionary เพราะจะทำให้มีการเดาได้จากโปรแกรมสืบค้นข้อมูล ควรตั้งให้มีตัวอักษรที่เป็นสัญลักษณ์ เช่น %?! # @ ปะปนแทรกอยู่ภายใน<br />
            • ไม่ควรตั้งรหัสผ่าน ตามตัวเลขที่มีส่วนเกี่ยวข้องกับผู้ตั้ง เช่น วันเดือนปีเกิด, เลขที่บ้าน, ทะเบียนรถ เป็นต้น<br />--%>
            <asp:Label ID="ctlGuideDetails" runat="server"></asp:Label>
            </td>
        </tr>
        </table>
        </fieldset>
                   
        <br /> 
        <fieldset style="width: 90%" id="Fieldset1" class="table">  
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label ID="ctlOldPasswordText" runat="server" Text="Old Password"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="ctlOldPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="ctlOldPasswordReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="ctlNewPasswordText" runat="server" Text="New Password"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="ctlNewPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="ctlNewPasswordReq" runat="server" Text="*" Style="color: Red;"></asp:Label>
                    <AjaxToolkit:PasswordStrength ID="PasswordStrength1" runat="server" TargetControlID="ctlNewPasswordTextbox"
                        DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="10"
                        PrefixText="Strength : " StrengthStyles="PasswordStrengthPolicyStrength1; PasswordStrengthPolicyStrength2; PasswordStrengthPolicyStrength3; PasswordStrengthPolicyStrength4; PasswordStrengthPolicyStrength5"
                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" RequiresUpperAndLowerCaseCharacters="false"
                        TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="ctlConfirmPasswordText" runat="server" Text="Confirm New Password"></asp:Label>&nbsp;:&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="ctlConfirmPasswordTextbox" runat="server" Width="242px" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="ctlConfirmPasswordRequired" runat="server" Text="*" Style="color: Red;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="ctlUpdate" runat="server" Text="Save" OnClick="ctlUpdate_Click" Width="100px" />
                    <asp:Button ID="ctlCancelUpdate" runat="server" Text="Cancel" OnClick="ctlCancelUpdate_Click" Width="100px"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="color: Red;">
                    <spring:ValidationSummary ID="vsSummary" runat="server" Provider="ChangePassword.Error">
                    </spring:ValidationSummary>
                    <%--<asp:Label ID="ctlErrorValidationLabel" runat="server" Text="" ForeColor="Red"></asp:Label>--%>
                </td>
            </tr>
        </table>       
        </fieldset>
    </div>  
     
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ctlUpdate" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<AjaxToolkit:ModalPopupExtender ID="ctlPanelModalPopupExtender" runat="server" 
    RepositionMode="None"
    DropShadow="true" 
    BackgroundCssClass="modalBackground"
    TargetControlID="lnkDummy" 
	CancelControlID="lnkDummy"
    PopupControlID="pnChangePassword"
    PopupDragHandleControlID="pnChangePasswordHeader" />