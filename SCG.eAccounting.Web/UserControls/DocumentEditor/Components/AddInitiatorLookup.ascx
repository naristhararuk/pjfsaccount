<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddInitiatorLookup.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.AddInitiatorLookup" %>

<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<div align="center">
    <%if (!this.DesignMode)
      { %>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/global.js")%>"></script>
    <%} %>

    <asp:Panel ID="ctlUserSearchPanel"  runat="server" Width="600px" BackColor="White">
        <asp:Panel ID="ctlUserSearchHeader" CssClass="table" runat="server" Style="cursor: move;
            border: solid 1px Gray; color: Black">
            <div>
                <p>
                    <asp:Label ID="lblCapture" runat="server" Text='$Header$' Width="210px"></asp:Label></p>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelSearchUser" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsSearch" style="width: 70%" class="table">
                    <legend id="legSearch" style="color: #4E9DDF" class="table">
                        <asp:Label ID="ctlSearchbox" runat="server" Text='$Search Box$'></asp:Label></legend>
                    <table width="100%" border="0" class="table">
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlActorDataCheck" runat="server" Text='$User ID$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 30%">
                                <asp:TextBox ID="ctlUserId" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlNameLabel" runat="server" Text='$Name$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 30%">
                                <asp:TextBox ID="ctlName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlEmaolLabel" runat="server" Text='$Email$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 30%">
                                <asp:TextBox ID="ctlEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="ctlCompanyNameLabel" runat="server" Text='$CompanyName$'></asp:Label>
                                :
                            </td>
                            <td align="left" style="width: 30%">
                                <asp:TextBox ID="ctlCompanyName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="center">
                                <asp:ImageButton runat="server" ID="ctlSearch" SkinID="SkCtlQuery" OnClick="ctlSearch_Click" />
                                            <asp:TextBox ID="ctlUserIdNotIn" runat="server" style="visibility:hidden"></asp:TextBox>

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
                    <ss:BaseGridView ID="ctlUserSearchResultGrid" runat="server" AutoGenerateColumns="False"
                        OnRequestData="RequestData"  EnableViewState="true"
                        DataKeyNames="UserID, DivisionID, OrganizationID" CssClass="Grid" OnRequestCount="RequestCount"
                        OnDataBound="ctlUserSearchResultGrid_DataBound" Width="99%" OnRowCommand="ctlUserSearchResultGrid_RowCommand">
                        <HeaderStyle CssClass="GridHeader"/> 
                        <RowStyle CssClass="GridItem" HorizontalAlign="left"/>   
                        <AlternatingRowStyle CssClass="GridAltItem" /> 
                        <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ctlSelect1" runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UserID" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlUserId2" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlFullName" runat="server" Text=''></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CompanyName" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="ctlCompanyName" runat="server" Text='<%# Eval("OrganizationName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text=''></asp:Label>
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
                    </ss:BaseGridView>
                    <div style="text-align: left; width: 98%">
                        <table border="0">
                            <tr>
                                <td>
                                    <div id="ctlDivSubmitButton" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td valign="middle">
                                                    <asp:ImageButton runat="server" ID="ctlSubmit" SkinID="SkCtlFormSave" OnClick="ctlSubmit_Click" />
                                                </td>
                                                <td valign="middle">
                                                    |
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
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
    </asp:Panel>
    
</div>
<asp:LinkButton ID="lnkDummy" runat="server" Style="visibility: hidden" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1"  runat="server" TargetControlID="lnkDummy"
    PopupControlID="ctlUserSearchPanel" BackgroundCssClass="modalBackground" CancelControlID="lnkDummy"
    DropShadow="true" Drag="false" RepositionMode="None" PopupDragHandleControlID="ctlUserSearchHeader" />
 
