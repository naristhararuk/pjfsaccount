<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SapInstanceEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.SapInstanceEditor" %>
<style type="text/css">
    .style4
    {
        width: 200px;
    }
    .style7
    {
        width: 100px;
    }
    .style8
    {
        width: 120px;
    }
</style>
<asp:Panel ID="ctlSapEditor" runat="server" Style="display: block" CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="ctlSAPFormHeader" CssClass="table" runat="server" Style="cursor: move;
                    border: solid 1px Gray; color: Black" Width="100%">
                    <asp:Label ID="ctlSAPHeader" runat="server" SkinID="SkFieldCaptionLabel" Text='$SAPHeader$'
                        Width="100%"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="ctlPanelScroll" runat="server" HorizontalAlign="Center">
                <div style="display: block;" align="center">
                    <table cellpadding="0" cellspacing="0" border="0" width="90%">
                        <tr>
                            <td align="center">
                                <table class="table" width="100%">
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlSapInstanceCodeLabel" SkinID="SkFieldCaptionLabel" Text='$Code$'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="ctlNumberReq" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlSapInstanceCode" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlAliasNameLabel" SkinID="SkFieldCaptionLabel" Text='$Alias Name$'
                                                runat="server"></asp:Label><asp:Label ID="Label1" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlAliasName" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" Text='<%# Bind("AliasName")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlSystemIDLabel" SkinID="SkFieldCaptionLabel" Text='$System ID$'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="Label2" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlSystemID" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" Text='<%# Bind("SystemID")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlClientLabel" SkinID="SkFieldCaptionLabel" Text='$Client$' runat="server"></asp:Label>
                                            <asp:Label ID="Label3" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlClient" SkinID="SkCtlTextboxLeft" runat="server" Width="150px"
                                                Text='<%# Bind("Client")%>' MaxLength="5"/>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="ctlClientFilteredTextBoxExtender" runat="server"
                                                    TargetControlID="ctlClient" FilterType="Numbers" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlUserLabel" SkinID="SkFieldCaptionLabel" Text='$User$' runat="server"></asp:Label>
                                            <asp:Label ID="Label4" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlUser" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" Text='<%# Bind("User")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlPasswordLabel" SkinID="SkFieldCaptionLabel" Text='$Password$' runat="server"></asp:Label>
                                            <asp:Label ID="Label5" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlPassword" SkinID="SkCtlTextboxLeft" MaxLength="50" runat="server" Width="150px"
                                                Text='<%# Bind("Password")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlLanguageLabel" SkinID="SkFieldCaptionLabel" Text='$Language$' runat="server"></asp:Label>
                                            <asp:Label ID="Label6" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlLanguage" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="5"
                                                Width="150px" Text='<%# Bind("Language")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlSystemNumberLabel" SkinID="SkFieldCaptionLabel" Text='$System Number$'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="Label7" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlSystemNumber" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="5" Width="150px"
                                                Text='<%# Bind("SystemNumber")%>' />
                                                <ajaxToolkit:FilteredTextBoxExtender ID="ctlSystemNumberFilteredExtender" runat="server"
                                                    TargetControlID="ctlSystemNumber" FilterType="Numbers" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlMsgServerHostLabel" SkinID="SkFieldCaptionLabel" Text='$Msg Server Host$'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="Label8" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlMsgServerHost" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="200"
                                                Width="300px" Text='<%# Bind("MsgServerHost")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlLogonGroupLabel" SkinID="SkFieldCaptionLabel" Text='$Logon Group$'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="Label9" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlLogonGroup" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" Text='<%# Bind("LogonGroup")%>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style8">
                                            <asp:Label ID="ctlUserCPICLabel" SkinID="SkFieldCaptionLabel" Text='$UserCPIC$' runat="server"></asp:Label>
                                            <asp:Label ID="Label10" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="ctlUserCPIC" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="50"
                                                Width="150px" Text='<%# Bind("UserCPIC")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <fieldset class="table">
                                    <legend><b>Doc Type for Posting:</b></legend>
                                    <table class="table" width="100%">
                                        <tr>
                                            <td>
                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend1" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlExpenseDomesticLabel" runat="server" Text="$Expense (Domestic)$"
                                                                        SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeExpPostingDMLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label11" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeExpPostingDM" SkinID="SkCtlTextboxLeft" runat="server"
                                                                                MaxLength="2" Width="150px" Text='<%# Bind("DocTypeExpPostingDM")%>' />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeExpRmtPostingDMLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Remittance Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label12" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeExpRmtPostingDM" SkinID="SkCtlTextboxLeft" MaxLength="2"
                                                                                runat="server" Width="150px" Text='<%# Bind("DocTypeExpRmtPostingDM")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend2" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlExpenseForeignLabel" runat="server" Text="$Expense (Foreign)$"
                                                                        SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeExpPostingFRLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label13" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeExpPostingFR" SkinID="SkCtlTextboxLeft" runat="server"
                                                                                MaxLength="2" Width="150px" Text='<%# Bind("DocTypeExpPostingFR")%>' />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeExpRmtPostingFRLabel" SkinID="SkFieldCaptionLabel" Text='$Expense Remittance Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label14" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeExpRmtPostingFR" SkinID="SkCtlTextboxLeft" MaxLength="2"
                                                                                runat="server" Width="150px" Text='<%# Bind("DocTypeExpRmtPostingFR")%>' />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeExpICPostingFRLabel" SkinID="SkFieldCaptionLabel" Text='$Expense IC Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label15" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeExpICPostingFR" SkinID="SkCtlTextboxLeft" MaxLength="2"
                                                                                runat="server" Width="150px" Text='<%# Bind("DocTypeExpICPostingFR")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend3" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlAdvanceDomesticLabel" runat="server" Text="$Advance (Domestic)$"
                                                                        SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeAdvancePostingDMLabel" SkinID="SkFieldCaptionLabel" Text='$Advance (DM) Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label16" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeAdvancePostingDM" SkinID="SkCtlTextboxLeft" runat="server"
                                                                                MaxLength="2" Width="150px" Text='<%# Bind("DocTypeAdvancePostingDM")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend4" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlAdvanceForeignLabel" runat="server" Text="$Advance (Foreign)$"
                                                                        SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeAdvancePostingFRLabel" SkinID="SkFieldCaptionLabel" Text='$Advance (FR) Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label17" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeAdvancePostingFR" SkinID="SkCtlTextboxLeft" runat="server"
                                                                                MaxLength="2" Width="150px" Text='<%# Bind("DocTypeAdvancePostingFR")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend5" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlRemittanceLabel" runat="server" Text="$Remittance$" SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlDocTypeRmtPostingLabel" SkinID="SkFieldCaptionLabel" Text='$Remittance  Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label18" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeRmtPosting" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="2"
                                                                                Width="150px" Text='<%# Bind("DocTypeRmtPosting")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--FixedAdvance-->

                                                <table class="table" width="80%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="table">
                                                                <legend id="Legend6" runat="server" style="color: #4E9DDF" visible="true">
                                                                    <asp:Label ID="ctlFixedAdvance" runat="server" Text="$ctlFixedAdvance$" SkinID="SkFieldCaptionLabel" />
                                                                </legend>
                                                                <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlFixedAdvancePosting" SkinID="SkFieldCaptionLabel" Text='$FixedAdvance Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label21" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeFixedAdvancePosting" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="2"
                                                                                Width="150px" Text='<%# Bind("DocTypeFixedAdvancePosting")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                 <table width="100%" border="0" class="table">
                                                                    <tr>
                                                                        <td align="left" class="style4">
                                                                            <asp:Label ID="ctlFixedAdvanceReturnPosting" SkinID="SkFieldCaptionLabel" Text='$FixedAdvance Return Posting$'
                                                                                runat="server"></asp:Label>
                                                                            <asp:Label ID="Label20" SkinID="SkRequiredLabel" runat="server"></asp:Label>&nbsp:&nbsp
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="ctlDocTypeFixedAdvanceReturnPosting" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="2"
                                                                                Width="150px" Text='<%# Bind("DocTypeFixedAdvanceReturnPosting")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>


                                                <!--FixedAdvance-->
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="table" width="100%">
                                    <tr>
                                        <td align="center">
                                            <font color="red">
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="SAPInstance.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <table class="table" width="100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton runat="server" ID="ctlAdd" ToolTip="Add" SkinID="SkSaveButton" OnClick="Add_Click"
                            ImageAlign="Left" />
                        <asp:ImageButton runat="server" ID="ctlCencel" ToolTip="Cencel" SkinID="SkCancelButton"
                            OnClick="Cencel_Click" ImageAlign="Left" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
<ajaxToolkit:ModalPopupExtender ID="ctlSAPModalPopupExtender" runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlSapEditor" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    RepositionMode="None" PopupDragHandleControlID="ctlSAPFormHeader" />
