<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="InterfaceImageToSAPLog.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.Log.Programs.InterfaceImageToSAPLog" EnableTheming="true" StylesheetTheme="Default"%>
<%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    
    <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <fieldset id="fdsCritiria" style="text-align:left;border-color:Gray;border-style:solid;border-width:1px;font-family:Tahoma;font-size:small;width:35%;" runat="server"> 
                <table>
                    <tr>
                        <td align="right" width="50%">
                            <asp:Label ID="ctlRequestNoLabel" runat="server" Text="$Request No :$"></asp:Label>
                        </td>
                        <td align="left" width="50%"> 
                            <asp:TextBox ID="ctlRequestNo" SkinID="SkCtlTextboxLeft" runat="server" Text='' MaxLength="20"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlSubmitDateLabel" runat="server" Text="$Submit Date :$"></asp:Label>
                        </td>
                        <td align="left">    
                            <uc1:Calendar ID="ctlSubmitDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlStatusLabel" runat="server" Text="$Status :$"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ctlStatus" SkinID="SkCtlDropDownList" runat="server">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem>Error</asp:ListItem>
                                <asp:ListItem>File</asp:ListItem>
                                <asp:ListItem>New</asp:ListItem>
                                <asp:ListItem>Success</asp:ListItem>                                
                            </asp:DropDownList>
                        </td>
                    </tr
                    <tr>
                        <td colspan="2" align="center">
                            <asp:ImageButton ID="ctlSearch" SkinID="SkCtlQuery" runat="server" 
                                Text="Search" onclick="ctlSearch_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <ss:BaseGridView ID="ctlImageToSAPLogGrid" runat="server" 
                AutoGenerateColumns="False" Width="100%" OnRequestData="RequestData"
                OnRequestCount="RequestCount" AllowPaging="True" AllowSorting="True" 
                CssClass="table" HeaderStyle-CssClass="GridHeader"
                SelectedRowStyle-BackColor="#6699FF" ReadOnly="true">
                <Columns>
                    <asp:BoundField HeaderText="Request No" DataField="RequestNo" SortExpression="RequestNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                    <asp:TemplateField HeaderText="Submit Date" SortExpression="SubmitDate">
						<ItemTemplate>
							<asp:Label ID="lblSubmitDate" runat="server" style="text-align:center" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("SubmitDate").ToString()) %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false"/>
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
                    <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status"/>
                    <asp:BoundField HeaderText="Message" DataField="Message" SortExpression="Message"/>
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

