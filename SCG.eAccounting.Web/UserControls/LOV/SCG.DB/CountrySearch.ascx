<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountrySearch.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.LOV.SCG.DB.CountrySearch" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<div align="center">
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
<asp:Panel ID="pnCountrySearch" runat="server" Width="600px" BackColor="White">
    <asp:Panel ID="pnCountrySearchHeader" CssClass="table" runat="server" Style="cursor: move;border:solid 1px Gray;color:Black">
		<div>
			<p><asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
		</div>
	</asp:Panel>
	<asp:UpdatePanel ID="UpdatePanelSearchCountry" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	        <fieldset id="ctlFieldSetDetailGridView" style="width:70%" id="fdsSearch" class="table">
			<legend id="legSearch" style="color:#4E9DDF" class="table">
                <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
			<table width="100%" border="0" class="table">
			    <tr>
					<td align="right" style="width:20%"><asp:Label ID="ctlCountryCode" runat="server" Text='$Country Code$'></asp:Label> : </td>
					<td align="left" style="width:30%">
						<asp:TextBox ID="txtCountryCode" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>

					</td>
				</tr>
				<tr>
					<td align="right" style="width:20%"><asp:Label ID="ctlCountryName" runat="server" Text='$Country Name$'></asp:Label> : </td>
					<td align="left" style="width:30%">
						<asp:TextBox ID="txtCountryName" SkinID="SkCtlTextboxLeft" runat="server"></asp:TextBox>
						<asp:TextBox ID="txtCountryId" runat="server" Visible="false"></asp:TextBox>
						<asp:TextBox ID="txtLanguageId" runat="server" Visible="false"></asp:TextBox>
					</td>
				</tr>
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
                <ss:BaseGridView ID="ctlCountryGrid" runat="server" AutoGenerateColumns="False" OnRequestCount="RequestCount"
                    OnRequestData="RequestData" ReadOnly="true" EnableInsert="false" EnableViewState="true"
                    DataKeyNames="CountryID" CssClass="table" OnDataBound="ctlCountryGrid_DataBound" Width="99%">
                    <Columns>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                        </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Country Code" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                             <asp:Label ID="ctlCountryCode" runat="server" Text='<%# Eval("CountryCode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Country Name" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                             <asp:Label ID="ctlCountryName" runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
						<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
					</EmptyDataTemplate>
					<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                </ss:BaseGridView>
                <div style="text-align:left;width:98%">
					<table border="0">
						<tr>
							<td valign="middle"><asp:ImageButton runat="server" ID="ctlSubmit" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" /></td>
							<td valign="middle"> | </td>
							<td valign="middle"><asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" /></td>
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
</asp:Panel>
    

	
</div>

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
	TargetControlID="lnkDummy"
	PopupControlID="pnCountrySearch"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy"
	DropShadow="true" 
	RepositionMode="None"
	PopupDragHandleControlID="pnCountrySearchHeader" />