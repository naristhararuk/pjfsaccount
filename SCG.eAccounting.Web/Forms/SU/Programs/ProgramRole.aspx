<%@ Page Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
CodeBehind="ProgramRole.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SU.Programs.ProgramRole" 
EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register Src="~/UserControls/LOV/SS.DB/ProgramSearch.ascx" TagName="ProgramSearch" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/AlertMessageFadeOut.ascx" tagname="AlertMessageFadeOut" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelGridView"
                DynamicLayout="true" EnableViewState="true">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlRoleGrid" runat="server" AutoGenerateColumns="False"
                OnRequestData="RequestData" OnRequestCount="RequestCount" ReadOnly="true" EnableInsert="false"
                InsertRowCount="1" DataKeyNames="Roleid" CssClass="table" AllowPaging="true" AllowSorting="true"
                OnRowCommand="ctlRoleGrid_RowCommand" EnableViewState="true" Width="100%" SelectedRowStyle-BackColor="#6699FF">
                <Columns>
                    <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName">
                    <HeaderStyle Width="30%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="ctlRole" runat="server" Text='<%# Eval("RoleName") %>'
                                CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" HeaderStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Update Date" SortExpression="UpdDate">
                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                    <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDate(Eval("UpdDate").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75px" HeaderText="$Active$" SortExpression="Active" />
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="ctlUpdatePanelProgramRoleGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:AlertMessageFadeOut ID="ctlMessage" runat="server"/>
        <fieldset id="ctlFieldSetDetailGridView" style="vertical-align:middle;font-family:Tahoma;font-size:small;width:100%;" runat="server"> 
            <legend id="ctlLegendDetailGridView" style="color:#4E9DDF" class="table">
                <asp:Label ID="ctlDetailGridView" runat="server" Text=""></asp:Label></legend>
            <center>
            <br />
            <ss:BaseGridView ID="ctlProgramRoleGrid" runat="server" AutoGenerateColumns="false" Width="98%" AllowSorting="true"
                OnRequestData="RequestProgramRoleData" OnRequestCount="RequestProgramRoleCount" CssClass="table" ReadOnly="true" DataKeyNames="ID,RoleId,ProgramId" OnDataBound="ctlProgramRoleGrid_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                    <HeaderStyle HorizontalAlign="Center" Width="25px" /> 
                    <ItemStyle HorizontalAlign="Center" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="ctlHeader" runat="server" onclick="javascript:validateCheckBox(this, '0');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlSelect" runat="server" onclick="javascript:validateCheckBox(this, '1');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Program" SortExpression="pl.ProgramsName"> 
                        <ItemTemplate>
                            <asp:Label ID="ctlProgram"  runat="server" Text='<%# Eval("ProgramsName") %>'/>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Add State" SortExpression="pr.AddState">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlAddState" runat="server" Checked='<%# Eval("AddState") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit State" SortExpression="pr.EditState">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlEditState" runat="server" Checked='<%# Eval("EditState") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete State" SortExpression="pr.DeleteState">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlDeleteState" runat="server" Checked='<%# Eval("DeleteState") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Display State" SortExpression="pr.DisplayState">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlDisplayState" runat="server" Checked='<%# Eval("DisplayState") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="pr.Comment">
                        <ItemTemplate>
                            <asp:TextBox ID="ctlCommentProgramRole" SkinID="SkCtlLongTexboxLeft" runat="server" MaxLength="500" Text='<%# Eval("Comment") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="400px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="pr.Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="ctlActiveProgramRole" runat="server" Checked='<%# Eval("Active") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="75px" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<asp:Label ID="lblNodata" SkinID="SkCtlLabelNodata" runat="server" Text='<%# GetMessage("NoDataFound") %>'></asp:Label>
				</EmptyDataTemplate>
				<EmptyDataRowStyle HorizontalAlign="Center" Width="100%" />
            </ss:BaseGridView>
            <div id="divButton" runat="server" align="left" style="width:98%;">
            <table border="0"><tr align="left">
				<td valign="middle">
                    <asp:ImageButton runat="server" ID="ctlAddNew" SkinID="SkCtlFormNewRow" OnClick="ctlAddNew_Click" />
				</td>
				<td valign="middle"> | </td>
				<td valign="middle">
                    <asp:ImageButton ID="ctlDelete" runat="server" SkinID="SkCtlGridDelete" OnClick="ctlDelete_Click" />
				</td>
				<td valign="middle"> | </td>
				<td valign="middle">
                    <asp:ImageButton runat="server" ID="ctlSave" SkinID="SkCtlFormSave" OnClick="ctlSave_Click" />
				</td>
				<td valign="middle"> | </td>
				<td valign="middle">
                    <asp:ImageButton ID="ctlCancel" runat="server" SkinID="SkCtlFormCancel" OnClick="ctlCancel_Click" />
				</td>
				</tr>
			</table>
            </div>
            </center>
        </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr><td><uc1:ProgramSearch ID="ProgramSearch1" runat="server" zIndex="10002" /></td></tr>
    </table>
    
</asp:Content>
