<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
    CodeBehind="ServiceTeam.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.DB.Programs.ServiceTeam"
    Title="" StylesheetTheme="Default" meta:resourcekey="PageResource1" %>
    
<%@ Register src="~/UserControls/LOV/SCG.DB/LocationLookup.ascx" tagname="LocationLookup" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>
<%@ Register src="~/UserControls/LOV/SCG.DB/LocationUserLookup.ascx" tagname="LocationUserLookup" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
   <table width="100%" class="table">
        <tr>
            <td align="left" width="30%">
                 <fieldset id="fdsSearch"  class="table">
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="left" style="width: 30%">
                                <asp:Label ID="ctlServiceTeamCodeLabel" runat="server" Text="$ServiceTeamCode$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="ctlServiceTeamCodeCri" MaxLength="20" Width="200" SkinID="SkGeneralTextBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 30%">
                                <asp:Label ID="ctlDescriptionLabel" runat="server" Text="$Description$"></asp:Label>
                                :
                            </td>
                            <td align="left" style="width:60%">
                                <asp:TextBox ID="ctlDescriptionCri" MaxLength="100" Width="200" SkinID="SkGeneralTextBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td valign="top" align="left" width="60%">
                 <asp:ImageButton runat="server" ID="ctlServiceTeamSearch" ToolTip="Search" SkinID="SkSearchButton"
                  OnClick="ctlServiceTeamSearch_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                 <asp:UpdatePanel ID="ctlUpdatePanelServiceTeamGridview" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelServiceTeamGridview"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        
                        <ss:BaseGridView ID="ctlServiceTeamGrid" runat="server" AutoGenerateColumns="false" CssClass="table"
                            AllowSorting="true" AllowPaging="true" DataKeyNames="ServiceTeamID" EnableInsert="False"
                            ReadOnly="true" SelectedRowStyle-BackColor="#4E9DDF" Width="100%"
                            OnRowCommand="ctlServiceTeamGrid_RowCommand" 
                            OnRequestCount="RequestCount" 
                            OnRequestData="RequestData">
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <RowStyle CssClass="GridItem" />
                                    
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:TemplateField HeaderText="ServiceTeam Code" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="ServiceTeamCode">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlServiceTeamCodeLabel" runat="server" Text='<%# Bind("ServiceTeamCode") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    SortExpression="Description">
                                    <ItemTemplate>
                                        <asp:Literal ID="ctlDescriptionLabel" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-HorizontalAlign="Center" SortExpression="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ctlActive" Checked='<%# Bind("Active") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <table width="100%">
                                          <tr>
                                              <td>
                                                  <asp:LinkButton ID="ctlLocationLink" runat="server" Text="Location" CommandName="LocationEdit"/>
                                              </td>
                                              <td>
                                                  <asp:ImageButton ID="ctlEditServiceTeam" runat="server" SkinID="SkCtlGridEdit" CausesValidation="False"
                                                    ToolTip='<%# GetProgramMessage("EditServiceTeam") %>' CommandName="ServiceTeamEdit" />
                                              </td>
                                              <td>
                                                  <asp:ImageButton ID="ctlDeleteServiceTeam" runat="server" SkinID="SkCtlGridDelete" CausesValidation="False"
                                                    ToolTip='<%# GetProgramMessage("DeleteServiceTeam") %>' OnClientClick="return confirm('Are you sure delete this row');" CommandName="ServiceTeamDelete" />     
                                              </td>
                                          </tr>
                                       </table>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                    runat="server"></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                        </ss:BaseGridView>                       
                        <div id="divButton" runat="server" style="vertical-align: middle;">
                            <table style="text-align: center;">
                                <tr>
                                    <td>
                                        <asp:ImageButton runat="server" ID="ctlServiceTeamAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlServiceTeamAddNew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                
                 <asp:Panel ID="ctlServiceTeamFormPanel" runat="server" Style="display: none" CssClass="modalPopup"
                    Width="500px">
                    <asp:Panel ID="ctlServiceTeamFormPanelHeader" runat="server" Style="cursor: move; background-color: #DDDDDD;
                        border: solid 1px Gray; color: Black">
                        <div>
                            <p>
                                <asp:Label ID="lblCaptureServiceTeam" runat="server" SkinID="SkFieldCaptionLabel" Text="Manage Service Team Data"></asp:Label>
                            </p>
                        </div>
                    </asp:Panel>
                    
                    <asp:UpdatePanel ID="ctlUpdatePanelServiceTeamForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: block;" align="center">
                                <asp:UpdateProgress ID="UpdatePanelServiceTeamFormProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelServiceTeamForm"
                                    DynamicLayout="true" EnableViewState="False">
                                    <ProgressTemplate>
                                        <uc3:SCGLoading ID="SCGLoading2"  runat="server" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="center">
                                            <asp:FormView ID="ctlServiceTeamForm" runat="server" DataKeyNames="ServiceTeamID" 
                                                OnItemCommand="ctlServiceTeamForm_ItemCommand"
                                                OnItemInserting="ctlServiceTeamForm_ItemInserting" 
                                                OnItemUpdating="ctlServiceTeamForm_ItemUpdating"
                                                OnModeChanging="ctlServiceTeamForm_ModeChanging" 
                                                OnDataBound="ctlServiceTeamForm_DataBound">
                                                <EditItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ServiceTeamCode")%> <font color="red"><asp:Label ID="ctlServiceTeamCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlServiceTeamCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="20"
                                                                    Text='<%# Bind("ServiceTeamCode")%>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ServiceTeamDescription")%><font color="red"><asp:Label ID="ctlServiceTeamDescriptionRequired"
                                                                    runat="server"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                                                    Text='<%# Bind("Description") %>' Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active") %>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" Checked='<%# Eval("Active") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlUpdate" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("UpdateServiceTeam") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Update" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("CancelServiceTeam") %>' CommandName="Cancel" Text="Cancel">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <table class="table">
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ServiceTeamCode")%><font color="red"><asp:Label ID="ctlServiceTeamCodeRequired"
                                                                    runat="server" Text="*"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlServiceTeamCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="20"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("ServiceTeamDescription")%><font color="red"><asp:Label ID="ctlServiceTeamDescriptionRequired"
                                                                    runat="server"></asp:Label></font> :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server" MaxLength="100"
                                                                    Width="250px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <%# GetProgramMessage("Active") %>
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="ctlActiveChk" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:ImageButton ID="ctlInsert" runat="server" SkinID="SkCtlFormSave" CausesValidation="True"
                                                                    ToolTip='<%# GetProgramMessage("InsertServiceTeam") %>' ValidationGroup="ValidateFormView"
                                                                    CommandName="Insert" Text="Update"></asp:ImageButton>
                                                                <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" CausesValidation="False"
                                                                    ToolTip='<%# GetProgramMessage("CancelServiceTeam") %>' CommandName="Cancel" Text="Cancel">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </InsertItemTemplate>
                                            </asp:FormView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <font color="red">
                                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ServiceTeam.Error" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                
                <asp:LinkButton ID="lnkDummy" runat="server" Style="display: none" meta:resourcekey="lnkDummyResource1" />
                <ajaxToolkit:ModalPopupExtender ID="ctlServiceTeamModalPopupExtender" runat="server"
                    TargetControlID="lnkDummy" PopupControlID="ctlServiceTeamFormPanel" BackgroundCssClass="modalBackground"
                    CancelControlID="lnkDummy" RepositionMode="None" PopupDragHandleControlID="ctlServiceTeamFormPanelHeader" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                   <asp:UpdatePanel ID="ctlUpdatePanelLocationGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    
                        <asp:HiddenField ID="ctlServiceTeamIDHidden" runat="server" />
                        
                        <asp:UpdateProgress ID="ctlUpdateProgressLocationGrid" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelLocationGrid"
                            DynamicLayout="true" EnableViewState="False">
                            <ProgressTemplate>
                                <uc3:SCGLoading ID="SCGLoading3"  runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <table width="100%" class="table">
                            <tr>
                                <td>
                                   &nbsp;<asp:Label ID="lblLocationHeader" Visible="false" runat="server" Text="$Location's Information$" ForeColor="#0066CC" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ss:BaseGridView ID="ctlLocationGrid" Width="60%" Visible="false" runat="server"
                                    AutoGenerateColumns="false" CssClass="table" DataKeyNames="ServiceTeamLocationID,ServiceTeamID,LocationID"
                                    ReadOnly="true" AllowSorting="true" AllowPaging="false"  EnableInsert="False"
                                    SelectedRowStyle-BackColor="#6699FF" ShowPageSizeDropDownList="false" 
                                    OnDataBound="ctlLocationGrid_DataBound"
                                    OnRequestData="RequestData_ctlLocationGrid"
                                    OnRequestCount="RequestCount_ctlLocationGrid" >
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <RowStyle CssClass="GridItem" />
                                    
                                    <Columns>
                                    
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Company Code" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="CompanyCode">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlCompanyCode" runat="server" Text='<%# Bind("CompanyCode") %>' Mode="Encode"></asp:Literal>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Location Code" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="LocationCode">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlLocationCode" runat="server" Text='<%# Bind("LocationCode") %>' Mode="Encode"></asp:Literal>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="Description">
                                            <ItemTemplate>
                                                <asp:Literal ID="ctlDescription" runat="server" Text='<%# Bind("Description") %>' Mode="Encode"></asp:Literal>
                                            </ItemTemplate>

                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />

                                            <HeaderStyle Width="50%" HorizontalAlign="Center" />
                                            <ItemStyle Width="50%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                    
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" Text='<%#GetMessage("NoDataFound") %>'
                                            runat="server"></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" Width="60%" />
                                </ss:BaseGridView>
                                
                                <div id="divLocationButton" runat="server" style="vertical-align: middle;">
                                    <table style="text-align: center;" align="left">
                                        <tr>
                                            <td align ="left">
                                                <div id="ctlLocationTools" runat="server" visible="false">
                                                    <table border="0">
                                                        <tr>
                                                            <td valign="middle">
                                                                <asp:ImageButton runat="server" Visible="false" ID="ctlAddLocation" SkinID="SkCtlFormNewRow"
                                                                    OnClick="ctlAddLocation_Click"/>
                                                              <%--  <uc1:LocationLookup ID="ctlLocationLookup" runat="server" />--%>
                                                            <uc2:LocationUserLookup ID="ctlLocationLookup" runat="server" isMultiple="true" />
                                                            </td>
                                                            <td valign="middle">
                                                                <asp:ImageButton runat="server" Visible="false" ID="ctlDeleteLocation" SkinID="SkCtlGridDelete" 
                                                                    OnClick="ctlDeleteLocation_Click" />
                                                            </td>
                                                            <td valign="middle">
                                                                <asp:ImageButton runat="server" Visible="false" ID="ctlCloseLocation" SkinID="SkCtlGridCancel" 
                                                                    OnClick="ctlCloseLocation_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200px">
                                                <font color="red">
                                                       <spring:ValidationSummary ID="ctlServiceTeamLocationValidationSummary" runat="server" Provider="ServiceTeamLocation.Error" />
                                                </font>             
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                </td>
                            </tr>
                        </table>    
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                   
            </td>
        </tr>
        
    </table>
</asp:Content>
