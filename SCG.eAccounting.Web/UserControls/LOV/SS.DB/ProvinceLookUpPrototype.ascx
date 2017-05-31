<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    EnableTheming = "true"
    CodeBehind="ProvinceLookUpPrototype.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.ProvinceLookUpPrototype" 
%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<div align="center">

    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
    <asp:Panel ID="pnProvinceSearch" runat="server" Width="600px" BackColor="White">
        
        <asp:Panel ID="pnProvinceSearchHeader" CssClass="table" runat="server" Style="cursor: move;border:solid 1px Gray;color:Black">
		    <div>
			    <p><asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
		    </div>
	    </asp:Panel>
    
        <asp:UpdatePanel ID="UpdatePanelSearchProvince" runat="server" UpdateMode="Conditional">
	        <ContentTemplate>
	            <fieldset id="ctlFieldSetDetailGridView" style="width:70%" id ="fdsSearch" class="table">
			        <legend id="legSearch" style="color:#4E9DDF" class="table">
                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
                    </legend>
			        <table width="100%" border="0" class="table">
			            <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblProvinceNo" runat="server" Text='$Province No$'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
						        <asp:TextBox ID="ctlTxtProvinceNo" SkinID="SkCtlTextboxLeft" runat="server" Width="200px"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblProvinceName" runat="server" Text='$Province Name$'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
						        <asp:TextBox ID="ctlTxtProvinceName" SkinID="SkCtlTextboxLeft" runat="server" Width="200px"></asp:TextBox>
					        </td>
				        </tr>
				        <tr>
					        <td align="right" style="width:20%">
					            <asp:Label ID="ctlLblRegionCode" runat="server" Text='$Region Name$'></asp:Label> : 
					        </td>
					        <td align="left" style="width:30%">
					            <asp:DropDownList ID="ctlCmbRegionCode" SkinID="SkCtlDropDownList" runat="server" Width="203px"></asp:DropDownList>
					        </td>
				        </tr>
				        <tr>
					        <td align="center" colspan="2">
					            <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
					        </td>
				        </tr>
			        </table>
		        </fieldset>
	        </ContentTemplate>
	    </asp:UpdatePanel>
	
	    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <center>
                <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
                <ss:BaseGridView 
                    runat="server"          Width="100%"    AutoGenerateColumns="False" EnableViewState="true"      SelectedRowStyle-BackColor="#6699FF"
                    EnableInsert="false"    ReadOnly="true" AllowSorting="False"         AllowPaging="False"          CssClass="table" 
                    ID                  = "ctlGridProvince" 
                    DataKeyNames        ="ProvinceId" 
                    OnRowCommand        = "ctlGridProvince_RowCommand" 
                    OnDataBound         = "ctlGridProvince_DataBound" >
                
                    <Columns>
                    
                    <asp:TemplateField HeaderText="$Select$" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlSelectHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="$ProvinceId$" SortExpression="DbProvince.ProvinceId" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLinkProvinceId" runat="server" Text='<%# Eval("ProvinceId")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                                        
                    <asp:TemplateField HeaderText="$ProvinceName$" SortExpression="DbProvinceLang.ProvinceName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblProvinceName" runat="server" Text='<%# Eval("ProvinceName")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="$RegionName$" SortExpression="DbRegionLang.RegionName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ctlLblRegionName" runat="server" Text='<%# Eval("RegionName")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="false">
                        <ItemTemplate>
                             <asp:ImageButton ID="ctlProvinceSelect" runat="server" SkinID="SkCtlGridSelect"  CausesValidation="False" CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    </Columns>
                    
                    <EmptyDataTemplate>
						<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
					</EmptyDataTemplate>
					<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
					
                </ss:BaseGridView>
                
                <div id="divButton" runat="server" align="left">
				    <table border="0">
					    <tr>
						    <td valign="middle">
							    <asp:ImageButton ID="ctbSelect" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlSelect_Click"/>
						    </td>
						    <td valign="middle">
						        <asp:Label ID="ctlLblLine" runat="server" Text="|"></asp:Label>
						    </td>
						    <td valign="middle">
							    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click"/>
						    </td>
					    </tr>
				    </table>
                </div>
            </center>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ctbSelect" EventName="Click" />
			    <asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
			</Triggers>
        </asp:UpdatePanel>
    
    </asp:Panel>  
    
</div>
	
<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
	TargetControlID="lnkDummy"
	PopupControlID="pnProvinceSearch"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="pnProvinceSearchHeader" />
