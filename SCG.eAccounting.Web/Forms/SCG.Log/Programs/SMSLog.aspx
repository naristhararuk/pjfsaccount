<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" CodeBehind="SMSLog.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.Log.Programs.SMSLog"  EnableTheming="true" StylesheetTheme="Default"%>
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
                            <asp:Label ID="ctlFromDateLabel" runat="server" Text="$From Date :$"></asp:Label>
                        </td>
                        <td align="left" width="50%"> 
                            <uc1:Calendar ID="ctlFromDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlToDateLabel" runat="server" Text="$To Date :$"></asp:Label>
                        </td>
                        <td align="left">    
                            <uc1:Calendar ID="ctlToDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ctlPhoneNoLabel" runat="server" Text="$Phone No :$"></asp:Label>
                        </td>
                        <td align="left">    
                            <asp:TextBox ID="ctlPhoneNo" SkinID="SkCtlTextboxLeft" runat="server" Text='' MaxLength="20"/>
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
            <ss:BaseGridView ID="ctlSmsLogGrid" runat="server" 
                AutoGenerateColumns="False" Width="100%" OnRequestData="RequestData"
                OnRequestCount="RequestCount" AllowPaging="True" AllowSorting="True" 
                CssClass="table" HeaderStyle-CssClass="GridHeader" 
                SelectedRowStyle-BackColor="#6699FF" ReadOnly="true">
                <Columns>
                    <asp:TemplateField HeaderText="Date" SortExpression="Date">
						<ItemTemplate>
							<asp:Label ID="lblDate" runat="server" Text='<%#  SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("Date").ToString()) %>'></asp:Label>
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="15%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Phone No." DataField="PhoneNo" SortExpression="PhoneNo"/>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="SendOrReceive" DataField="SendOrReceive" SortExpression="SendOrReceive"/>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Message" DataField="Message" SortExpression="Message"/>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="TRANID" DataField="TRANID" SortExpression="TRANID"/>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="SMID" DataField="SendMsgSMID" SortExpression="SendMsgSMID"/>
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

