<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TALookup.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.TALookup"%>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<div align="center">

    <asp:Panel ID="ctlPnTASearch" runat="server" Width="600px" BackColor="White" Style="display:none" >        
        <asp:Panel ID="ctlPnTASearchHeader" CssClass="table" runat="server" Style="cursor: move;border:solid 1px Gray;color:Black">
		    <div>
			    <p><asp:Label ID="ctlLblCapture" runat="server" Text='Header' Width="210px"></asp:Label></p>
		    </div>
	    </asp:Panel>
        <asp:UpdatePanel ID="cltUpdatePanelTADocumentSearch" runat="server" UpdateMode="Conditional">
	        <ContentTemplate>
	            <fieldset id="ctlFieldSetDetailGridView" style="width:70%" class="table">
			        <legend id="ctlLegSearch" style="color:#4E9DDF" class="table">
                        <asp:Label ID="ctlLblSearchbox" runat="server" Text='Search Box'></asp:Label>
                    </legend>
			        <table width="100%" border="0" class="table">
			            <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblDocumentNo" runat="server" Text='Document No'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
						        <asp:TextBox ID="ctlTxtDocumentNo" SkinID="SkCtlTextboxLeft" runat="server" Width="200px"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblCreator" runat="server" Text='Creator'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
					            <asp:TextBox ID="ctlTxtCreator" SkinID="SkCtlTextboxLeft" runat="server" Width="200px"></asp:TextBox>
					           
					        </td>
				        </tr>
				        <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblCreateDate" runat="server" Text='Create Date'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
					            <uc1:Calendar ID="ctlCalendar" runat="server" />
					        </td>
				        </tr>				        
				        <tr>
					        <td align="center" colspan="2">
					            <asp:ImageButton runat="server" ID="ctlImgSearch" SkinID="SkCtlQuery" OnClick="ctlImgSearch_Click" />
					        </td>
				        </tr>
			        </table>
		        </fieldset>
	        </ContentTemplate>
	    </asp:UpdatePanel>   
	    
	    <asp:UpdatePanel ID="ctlUpdatePanelTADocumentGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <center>
                <asp:UpdateProgress ID="ctlUpdatePanelTADocumentGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelTADocumentGridView"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>

				<ss:BaseGridView 
                    runat="server"          Width="100%"    AutoGenerateColumns="False" EnableViewState="true"      SelectedRowStyle-BackColor="#6699FF"
                    EnableInsert="false"    ReadOnly="true" AllowSorting="true"         AllowPaging="False"       
                    ID                  = "ctlGridTADocument" 
                    DataKeyNames        = "DocumentId" 
                    OnRowCommand        = "ctlGridTADocument_RowCommand" 
                    OnDataBound         = "ctlGridTADocument_DataBound" CssClass="Grid">  
                    <HeaderStyle CssClass="GridHeader"/> 
                    <RowStyle CssClass="GridItem" HorizontalAlign="left"/>   
                    <AlternatingRowStyle CssClass="GridAltItem" /> 
                    <Columns>                    
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Document No" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblDocumentNo" runat="server" Text='<%# Bind("DocumentNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                                            
                        <asp:TemplateField HeaderText="Creator" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblCreator" runat="server" Text='<%# Bind("Creator")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Create Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="ctlLblCreateDate" runat="server" Text='<%# Bind("CreateDate")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                 <asp:ImageButton ID="ctlImgTASelect" runat="server" SkinID="SkCtlGridSelect" CausesValidation="False" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                    </Columns>
                    
                    <EmptyDataTemplate>
						<asp:Label ID="ctlLblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
					</EmptyDataTemplate>
					<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
					
                </ss:BaseGridView>
                
                <div id="divButton" runat="server" align="left">
				    <table border="0">
					    <tr>
						    <td valign="middle">
							    <asp:ImageButton ID="ctlImgSelect" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlImgSelect_Click"/>
						    </td>
						    <td valign="middle">
						        <asp:Label ID="ctlLblLine" runat="server" Text="|"></asp:Label>
						    </td>
						    <td valign="middle">
							    <asp:ImageButton ID="ctlImgCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlImgCancel_Click"/>
						    </td>
					    </tr>
				    </table>
                </div>
            </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ctlImgSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctlImgSelect" EventName="Click" />
			    <asp:AsyncPostBackTrigger ControlID="ctlImgCancel" EventName="Click" />
			</Triggers>
        </asp:UpdatePanel>
         
    </asp:Panel>     
    
   
</div> 

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
    <ajaxToolkit:ModalPopupExtender ID="ctlModalPopupExtenderTADocument" runat="server" 
	    TargetControlID="lnkDummy"
	    PopupControlID="ctlPnTASearch"
	    BackgroundCssClass="modalBackground"
	    CancelControlID="lnkDummy"
	    DropShadow="true" 
	    RepositionMode="None"
	    PopupDragHandleControlID="ctlPnTASearchHeader" />