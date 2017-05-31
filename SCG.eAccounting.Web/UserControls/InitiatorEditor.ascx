<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InitiatorEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.InitiatorEditor" %>
<%@ Register Src="LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc1" %>
<asp:Panel ID="ctlInitiatorEditor" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlUpdatePanelInitiator" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 80%" id="ctlFieldsetInitiator" runat="server" visible="False"
                enableviewstate="True">
                <legend id="ctlLegendDetailGridViewInitiator" runat="server" style="color: #4E9DDF"
                    visible="True">
                    <asp:Label ID="ctrLabelGroup" runat="server" Text="$Display Initiator Setting$" SkinID="SkGeneralLabel"/></legend>
                <ss:BaseGridView ID="ctlInitiatorGrid" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="UserFavoriteActorID" CssClass="Grid" Width="100%" OnDataBound="ctlInitiatorGrid_Databound"
                    AllowSorting="false" OnRequestData="RequestDataInitiator">
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserID">
                            <ItemTemplate>
                                <asp:Label ID="ctlUserID" runat="server" Text='<%# Eval("ActorUserID.UserName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InitiatorName">
                            <ItemTemplate>
                                <asp:Label ID="ctlInitiatorName" runat="server" Text='<%# Eval("ActorUserID.EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>' Enabled ="false"   />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <asp:ImageButton runat="server" ID="ctlAddInitiator" ToolTip="Add" SkinID="SkCtlFormNewRow"
                    OnClick="ctlAddInitiator_Click" />
                <asp:ImageButton runat="server" ID="ctlDeleteInitiator" ToolTip="Delete" SkinID="SkCtlGridDelete"
                    CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                    OnClick="ctlDeleteInitiator_Click" />
                <asp:ImageButton runat="server" ID="ctlCloseInitiator" ToolTip="Close" SkinID="SkCtlFormCancel"
                    OnClick="ctlCloseInitiator_Click" />
                <asp:HiddenField ID="user" runat="server" />
                <table class="table">
                <font color="red">
                <asp:Label ID="ShowAdd" runat="server" Text=''></asp:Label>&nbsp;
                </font>
                
                </table>
                
                
            </fieldset>
            <uc1:UserProfileLookup ID="ctlInitiatorLookup" runat="server" isMultiple="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
