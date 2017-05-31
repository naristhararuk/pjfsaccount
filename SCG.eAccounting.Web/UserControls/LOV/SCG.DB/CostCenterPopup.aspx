<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMasterPage.Master" AutoEventWireup="true"
	CodeBehind="CostCenterPopup.aspx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CostCenterPopup"
	EnableTheming="true" StylesheetTheme="Default" %>

<%@ Register Src="CostCenterLookUp.ascx" TagName="CostCenterLookup" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Shared/PopupCallback.ascx" TagName="PopupCallback" TagPrefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="X" runat="server">
	<asp:Panel ID="pnCostCenterSearch" runat="server" Width="600px" BackColor="White">
		<asp:Panel ID="pnCostCenterSearchHeader" CssClass="table" runat="server" Style="cursor: move; border: solid 1px Gray; color: Black">
			<div><p><asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p></div>
		</asp:Panel>
		<asp:UpdatePanel ID="UpdatePanelSearchCostCenter" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<center>
					<fieldset id="ctlFieldSetDetailGridView" style="width: 70%" id="fdsSearch" class="table">
						<legend id="legSearch" style="color: #4E9DDF" class="table">
							<asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label>
						</legend>
						<table width="100%" border="0" class="table">
							<tr>
								<asp:HiddenField ID="ctlCompanyID" runat="server" />
								<td align="right" style="width: 20%">
									<asp:Label ID="lblCostCenterCode" runat="server" Text='$CostCenter Code$'></asp:Label> :
								</td>
								<td align="left" style="width: 30%">
									<asp:TextBox ID="ctlCostCenterCode" SkinID="SkCtlTextboxCenter" runat="server" MaxLength="20"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td align="right" style="width: 20%">
									<asp:Label ID="lblDescription" runat="server" Text='$CostCenterName$'></asp:Label>
									:
								</td>
								<td align="left" style="width: 30%">
									<asp:TextBox ID="ctlDescription" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td align="center" colspan="2">
									<asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkSearchButton" OnClick="ctlSearch_Click" />
								</td>
							</tr>
						</table>
					</fieldset>
				</center>
			</ContentTemplate>
		</asp:UpdatePanel>
		<div id="ctlCostCenterDiv" style="height: 300; overflow-y: auto;">
			<asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<center>
						<asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
							DynamicLayout="true" EnableViewState="true">
							<ProgressTemplate>
								<uc4:SCGLoading ID="SCGLoading1" runat="server" />
							</ProgressTemplate>
						</asp:UpdateProgress>
						<ss:BaseGridView ID="ctlCostCenterGrid" runat="server" AutoGenerateColumns="False"
							OnRequestCount="RequestCount" OnRequestData="RequestData" ReadOnly="true" EnableInsert="false"
							EnableViewState="true" DataKeyNames="CostCenterID" CssClass="Grid" Width="99%"
							AllowPaging="true" AllowSorting="true" OnRowCommand="ctlCostCenterGrid_RowCommand"
							OnDataBound="ctlCostCenterGrid_DataBound" HeaderStyle-CssClass="GridHeader">
							<alternatingrowstyle cssclass="GridItem" />
							<rowstyle cssclass="GridAltItem" />
							<columns>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="25px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="ctlCostCenterSelect" runat="server" SkinID="SkCtlGridSelect"
                                    CausesValidation="False" CommandName="Select" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CostCenter Code" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CostCenterCode">
                            <ItemTemplate>
                                <asp:Label ID="ctlGridCostCenterCode" runat="server" Text='<%# Eval("CostCenterCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CostCenterName" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="ctlGridDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </columns>
							<emptydatatemplate>
                        <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
                    </emptydatatemplate>
							<emptydatarowstyle horizontalalign="Center" width="100%" />
						</ss:BaseGridView>
						<div style="text-align: left; width: 98%">
							<table border="0">
								<tr>
									<td valign="middle">
										<asp:ImageButton ID="ctlSubmit" runat="server" SkinID="SkCtlFormNewRow" OnClick="ctlSubmit_Click" />
									</td>
									<td valign="middle">
										<asp:Label ID="ctlLblLine" runat="server" Text="|"></asp:Label>
									</td>
									<td valign="middle">
										<asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
									</td>
								</tr>
							</table>
						</div>
					</center>
				</ContentTemplate>
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="ctlSearch" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="ctlCancel" EventName="Click" />
				</Triggers>
			</asp:UpdatePanel>
		</div>
	</asp:Panel>
	<uc2:PopupCallback ID="PopupCallback1" runat="server" />
</asp:Content>
