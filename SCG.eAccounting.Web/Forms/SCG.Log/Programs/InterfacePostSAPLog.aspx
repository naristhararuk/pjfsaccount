<%@ Page Title="" Language="C#" MasterPageFile="~/ProgramsPages.Master" AutoEventWireup="true" 
    CodeBehind="InterfacePostSAPLog.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.Log.InterfacePostSAPLog" 
    EnableTheming="true"
    StylesheetTheme="Default"
    meta:resourcekey="PageResource1"
%>

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
            
            <div align="left">
            <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                <fieldset id="fdsCritiria" style="width:400px" class="table" runat="server">
                    <table>
                        <tr>
                            <td align="right" width="40%">
                                <asp:Label ID="ctlRequestNoLabel" runat="server" Text="Document No :"></asp:Label>
                            </td>
                            <td align="left" width="60%"> 
                                <asp:TextBox ID="ctlRequestNo" SkinID="SkCtlTextboxLeft" runat="server" Text='' MaxLength="20"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ctlDateLabel" runat="server" Text="Date :"></asp:Label>
                            </td>
                            <td align="left">    
                                <uc1:Calendar ID="ctlDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ctlStatusLabel" runat="server" Text="Status :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ctlStatus" SkinID="SkCtlDropDownList" runat="server">
                                    <asp:ListItem Text="All"        Value="A" Selected="True" />
                                    <asp:ListItem Text="Success"    Value="S" />
                                    <asp:ListItem Text="Error"      Value="E" />
                                    <asp:ListItem Text="Warning"    Value="W" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </td>
                <td>
                    &nbsp;&nbsp;
                </td>
                <td valign="top">
                    <asp:ImageButton ID="ctlSearch" SkinID="SkSearchButton" runat="server" Text="Search" onclick="ctlSearch_Click" />
                </td>
            </tr>
            </table>
            </div>
            
            <br />
            <ss:BaseGridView ID="ctlPostSAPLogGrid" runat="server" 
                AutoGenerateColumns="False" Width="100%" 
                OnRequestData="RequestData"
                OnRequestCount="RequestCount" 
                AllowPaging="True" 
                AllowSorting="True" 
                CssClass="table" 
                HeaderStyle-CssClass="GridHeader"
                SelectedRowStyle-BackColor="#6699FF"
                ReadOnly="true" >
            <AlternatingRowStyle CssClass="GridItem" />
            <RowStyle CssClass="GridAltItem" />
            
                <Columns>
                    <asp:BoundField HeaderText="Posting Date"   DataField="PostingDate"  SortExpression="PostingDate"    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="7%"/>
                    <asp:BoundField HeaderText="Requester"      DataField="EmployeeCode" SortExpression="EmployeeCode"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"      ItemStyle-Width="7%"/>
                    <asp:BoundField HeaderText="Documant No"    DataField="DocNo"        SortExpression="DocNo"          HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="9%"/>
                    <asp:BoundField HeaderText="Doc Seq"        DataField="DocSeq"       SortExpression="DocSeq"         HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="6%"/>
                    <asp:BoundField HeaderText="Posting Type"   DataField="PostingType"  SortExpression="PostingType"    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="8%"/>
                    <asp:BoundField HeaderText="DOC YEAR"       DataField="DocYear"      SortExpression="DocYear"        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="5%"/>
                    <asp:BoundField HeaderText="Company"        DataField="CompanyCode"  SortExpression="CompanyCode"    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"      ItemStyle-Width="7%"/>
                    <asp:BoundField HeaderText="FI_DOC"         DataField="FiDoc"        SortExpression="FiDoc"          HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="7%"/>
                    <asp:BoundField HeaderText="Type"           DataField="Type"         SortExpression="Type"           HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"    ItemStyle-Width="6%"/>
                    <asp:BoundField HeaderText="Message"        DataField="Message"      SortExpression="Message"        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"      ItemStyle-Width="38%"/>
                    
                    <%--<asp:BoundField HeaderText="ครั้งที่ Post" DataField="PostNo" SortExpression="PostNo" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField HeaderText="ลำดับที่ของ Document ใน Request" DataField="DocumentSeqOnRequest" SortExpression="DocumentSeqOnRequest" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField HeaderText="Invoice" DataField="InvoiceNo" SortExpression="InvoiceNo"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Year" DataField="Year" SortExpression="Year" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Company Code" DataField="CompanyCode" SortExpression="CompanyCode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Fi Doc" DataField="FiDocument" SortExpression="FiDocument"/>
                    <asp:BoundField HeaderText="Flag" DataField="Flag" SortExpression="Flag"/>
                    <asp:BoundField HeaderText="Message" DataField="Message" SortExpression="Message"/>--%>
                </Columns>
                <SelectedRowStyle BackColor="#6699FF" />
            </ss:BaseGridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
