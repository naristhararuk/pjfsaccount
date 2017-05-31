<%@ Page Language="C#" Title="" AutoEventWireup="true" MasterPageFile="~/PopupMasterPage.Master" CodeBehind="RoleSearchPopup.aspx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.LOV.SS.DB.RoleSearchPopup"  EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>
<%@ Register Src="../../Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
    <div align="center">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
	    <asp:Panel ID="pnRoleSearch" runat="server" style="display:none;" Width="300px" BackColor="White">
		    <asp:Panel ID="pnRoleSearchHeader" runat="server" Style="cursor: move;border:solid 1px Gray;color:Black">
			    <div>
				    <p><asp:Label ID="lblCapture" runat="server" Text="Search Role"></asp:Label></p>
			    </div>
		    </asp:Panel>
		    <asp:UpdatePanel ID="UpdatePanelSearchRole" runat="server" UpdateMode="Conditional">
			    <ContentTemplate>
				    <fieldset style="width:80%" id="fdsSearch">
				    <legend id="legSearch" style="color:#4E9DDF">
					    <asp:Label ID="ctlSearchBox" runat="server"></asp:Label>
				    </legend>
				    <table width="100%" border="0" cellpadding="0" cellspacing="0">
					    <tr>
						    <td align="right" style="width:40%">
							    <asp:Label ID="ctlRoleNameText" runat="server"></asp:Label>&nbsp;:&nbsp;
						    </td>
						    <td align="left" style="width:60%">
							    <asp:TextBox ID="txtRoleName" runat="server" Width="132px"></asp:TextBox>
							    <asp:Label ID="txtUserId" runat="server" style="display:none;"></asp:Label>
							    <asp:Label ID="txtLanguageId" runat="server" style="display:none;"></asp:Label>
						    </td>
					    </tr>
					    <tr>
						    <td align="center" colspan="2">
							    <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
							    <%--<asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" Text="Search"></asp:LinkButton>--%>
							    <%--<asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click" Text="Close"></asp:LinkButton>--%>
						    </td>
					    </tr>
				    </table>
			    </fieldset>
			    </ContentTemplate>
		    </asp:UpdatePanel>
		    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
			    <ContentTemplate>
				    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
					    DynamicLayout="true" EnableViewState="true">
					    <ProgressTemplate>
						    <uc4:SCGLoading ID="SCGLoading1" runat="server" />
					    </ProgressTemplate>
				    </asp:UpdateProgress>
				    <ss:BaseGridView ID="ctlRoleGrid" runat="server" AutoGenerateColumns="False"
					    OnRequestData="ctlRoleGrid_RequestData" OnRequestCount="ctlRoleGrid_RequestCount" 
					    ReadOnly="true" EnableInsert="false" EnableViewState="true"
					    DataKeyNames="RoleId" CssClass="table" OnDataBound="ctlRoleGrid_DataBound" Width="80%">
					    <Columns>
						    <asp:TemplateField HeaderText="Select">
							    <HeaderTemplate>
								    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateUserControlCheckBox(this, '0');" />
							    </HeaderTemplate>
							    <ItemTemplate>
								    <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateUserControlCheckBox(this, '1');" />
							    </ItemTemplate>
							    <HeaderStyle Width="25px" HorizontalAlign="Center" />
							    <ItemStyle Width="25px" HorizontalAlign="Center" />
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="Role Name">
							    <ItemTemplate>
								    <asp:Label ID="ctlRoleName" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label>
							    </ItemTemplate>
							    <HeaderStyle HorizontalAlign="Center" />
							    <ItemStyle HorizontalAlign="Left" />
						    </asp:TemplateField>
					    </Columns>
					    <EmptyDataTemplate>
						    <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
					    </EmptyDataTemplate>
					    <EmptyDataRowStyle HorizontalAlign="Center" Width="80%" />
				    </ss:BaseGridView>
				    <div style="text-align:left;width:80%">
					    <table border="0">
						    <tr>
							    <span id="spanSaveButton" runat="server">
							    <td valign="middle"><asp:ImageButton runat="server" ID="ctlSubmit" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" /></td>
							    <td valign="middle"> | </td>
							    </span>
							    <td valign="middle"><asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" /></td>
						    </tr>
					    </table>
                    </div>
			    </ContentTemplate>
			    <Triggers>
				    <asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
			    </Triggers>
		    </asp:UpdatePanel>
	    </asp:Panel>
    </div>
    <uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
