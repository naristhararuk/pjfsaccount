<%@ Page Language="C#" 
MasterPageFile="~/ProgramsPages.Master" 
AutoEventWireup="true" 
CodeBehind="UserLoginLog.aspx.cs" 
Inherits="SCG.eAccounting.Web.Forms.SCG.Log.Programs.UserLoginLog"
 Title="Untitled Page"
 EnableTheming="true"
 StylesheetTheme="Default" %>
 <%@ Register Src="~/UserControls/Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
  
   <asp:UpdatePanel ID="ctlUpdatePanelGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="ctlUpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelGridView"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1"  runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
 <table width="100%" class="table">
        <tr>
            <td align="left" style="width: 40%">
<fieldset id="fdsCritiria" style="text-align:left;border-color:Gray;border-style:solid;border-width:1px;font-family:Tahoma;font-size:small;width:90%;" runat="server"> 
                <table>
                   
                    <tr>
                        <td align="left">
                            <asp:Label ID="ctlFromDateLabel" runat="server" Text="$From Date$ :" ></asp:Label>
                        </td>
                        <td colspan="2" align="left">    
                            <uc1:calendar ID="ctlFromDate" runat="server"  />
                        </td>
                   </tr>
                    <tr>
                       <td align="left">
                            <asp:Label ID="ctlToDateLabel" runat="server" Text="$To Date$  :" Width = "50%"></asp:Label>
                        </td>
                        <td colspan="2" align="left">    
                            <uc1:calendar ID="ctlToDate" runat="server" />
                        </td>
                    </tr>
                   <tr>
                        <td align="left" >
                            <asp:Label ID="ctlUserIDLabel" runat="server" Text="$User ID$ :"></asp:Label>
                        </td>
                        <td align="left"> 
                            <asp:TextBox ID="ctlUserName" SkinID="SkCtlTextboxLeft" runat="server" Text='' MaxLength="20"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="50%">
                            <asp:Label ID="ctlStatusLabel" runat="server" Text="$Status$  :"></asp:Label>
                        </td>
                        <td align="left" width="60%"> 
                            <asp:DropDownList ID="ctlStatus" runat="server">
                                <asp:ListItem></asp:ListItem>                                
                                <asp:ListItem>Account Locked</asp:ListItem>                               
                                <asp:ListItem>Invalid UserName</asp:ListItem>
                                <asp:ListItem>Password Expired</asp:ListItem>
                                <asp:ListItem>Success</asp:ListItem>
                                <asp:ListItem>Wrong Password</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        
                        </td>
                    </tr>
                    </tr>
                </table>
            </fieldset>
            </td>
           
             <td valign="top" align="left">
              <asp:ImageButton ID="ctlSearch" SkinID="SkSearchButton" runat="server" 
              Text="Search" onclick="ctlSearch_Click"/>
             </td>
            </tr>
</table>
           
           <ss:BaseGridView ID="ctlUserLoginLogGrid" runat="server" 
                AutoGenerateColumns="False" Width="100%" 
                
                AllowPaging="True" AllowSorting="True"
                OnRequestData="RequestData"
                OnRequestCount="RequestCount" 
                CssClass="table" 
                HeaderStyle-CssClass="GridHeader"
                SelectedRowStyle-BackColor="#6699FF" ReadOnly="true">
                <Columns>
                    <asp:TemplateField HeaderText="User ID" SortExpression="UserName" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                              <asp:Label ID="ctlLblUserID" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                     </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date" SortExpression="SignInDate">
						<ItemTemplate>
							    <asp:Label ID="ctlLblLastDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("SignInDate").ToString()) %>'></asp:Label>
							
						</ItemTemplate>
						<ItemStyle HorizontalAlign="Center" Wrap="false" Width="30%" />
						<HeaderStyle HorizontalAlign="Center" />
					</asp:TemplateField>
                    <asp:TemplateField HeaderText="Status"   SortExpression="Status" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate >
                               <asp:Label ID="ctlLblStatus" runat="server"  Text='<%# Eval("Status")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left"  Width="30%"  />
                     </asp:TemplateField>
                     
                                      
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
           
            
            
            
</ContentTemplate>
</asp:UpdatePanel>
  
  
</asp:Content>
