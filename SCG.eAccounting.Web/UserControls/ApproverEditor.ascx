<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApproverEditorEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.ApproverEditor" %>
<%@ Register Src="LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc1" %>
<asp:Panel ID="ctlApproverEditor" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlApprroverUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 80%" id="ctlFieldSetApproverEditorInfo" runat="server" visible="False"
                enableviewstate="True">
                <legend id="ctlLegendDetailGridViewApproverEditor" runat="server" style="color: #4E9DDF"
                    visible="True">
                    <asp:Label ID="ctlDetailHeaderApproverEditor" runat="server" Text="$Display Approver Editor Setting$" SkinID="SkGeneralLabel" /></legend>
                <ss:BaseGridView ID="ctlApproverEditorGrid" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="UserFavoriteActorID" CssClass="Grid" Width="100%" OnDataBound="ctlApproverEditorGrid_Databound"
                    AllowSorting="false" AllowPaging="true" OnRequestData="RequestDataApproverEditor" >
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
                        <asp:TemplateField HeaderText="ApproverEditorName">
                            <ItemTemplate>
                                <asp:Label ID="ctlApproverEditorName" runat="server" Text='<%# Eval("ActorUserID.EmployeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlActive" runat="server" Checked='<%# Eval("Active") %>'  Enabled ="false"   />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ss:BaseGridView>
                <asp:ImageButton runat="server" ID="ctlAddApproverEditor" ToolTip="Add" SkinID="SkCtlFormNewRow"
                    OnClick="ctlAddApproverEditor_Click" />
                <asp:ImageButton runat="server" ID="ctlDeleteApproverEditor" ToolTip="Delete" SkinID="SkCtlGridDelete"
                    CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                    OnClick="ctlDeleteApproverEditor_Click" />
                <asp:ImageButton runat="server" ID="ctlApproverEditorClose" ToolTip="Delete" SkinID="SkCtlFormCancel"
                    OnClick="ctlCloseApproverEditor_Click" />
                <asp:HiddenField ID="user" runat="server" />
            </fieldset>
            <uc1:UserProfileLookup ID="ctlUserProfileLookUp" runat="server" isMultiple="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
