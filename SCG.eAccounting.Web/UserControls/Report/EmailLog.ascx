<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailLog.ascx.cs" 
Inherits="SCG.eAccounting.Web.UserControls.Report.EmailLog" EnableTheming="true"%>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc3" %>

<asp:UpdatePanel ID="ctlUpdatePanelEmailLog" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetDetailGridView" style="width:40%" class="table">
        <table class="table" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:25%">
                <asp:Label ID="ctlEmailTypeText" runat="server" Text="$Email Type$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ctlEmailType" runat="server" SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="EM01">EM01 (Approve/Reject)</asp:ListItem>
                    <asp:ListItem Value="EM02">EM02 (Status change)</asp:ListItem>
                    <asp:ListItem Value="EM03">EM03 (Over role)</asp:ListItem>
                    <asp:ListItem Value="EM04">EM04 (Rejection)</asp:ListItem>
                    <asp:ListItem Value="EM05">EM05 (Payment ready (Cash & Cheque))</asp:ListItem>
                    <asp:ListItem Value="EM06">EM06 (Payment ready (Transfer))</asp:ListItem>
                    <asp:ListItem Value="EM07">EM07 (Password expire notification)</asp:ListItem>
                    <asp:ListItem Value="EM08">EM08 (New user)</asp:ListItem>
                    <asp:ListItem Value="EM09">EM09 (Submission notification)</asp:ListItem>
                    <asp:ListItem Value="EM10">EM10 (Clearing notification)</asp:ListItem>
                    <asp:ListItem Value="EM11">EM11 (Money return)</asp:ListItem>
                    <asp:ListItem Value="EM12">EM12 (Reset password)</asp:ListItem>
                    <asp:ListItem Value="EM13">EM13 (Confirm Remittance Advance)</asp:ListItem>
                    <asp:ListItem Value="EM14">EM14 (Remind Document)</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="ctlEmailTypeLabel" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlDateText" runat="server" Text="$Date$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                <uc1:Calendar id="ctlDate" runat="server"></uc1:Calendar>        
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ctlRequestNoText" runat="server" Text="$Request No$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td>
                 <asp:TextBox ID="ctlRequestNo" runat="server" MaxLength="15"></asp:TextBox>
            </td>
        </tr>
        <tr align="left">
            <td>
                <asp:Label ID="ctlStatusText" runat="server" Text="$Status$" SkinID="SkFieldCaptionLabel"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ctlStatus" runat="server"  SkinID="SkGeneralDropdown">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Success</asp:ListItem>
                    <asp:ListItem Value="2">Fail</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" 
                    SkinID="SkCtlQuery" onclick="ctlSearch_Click"/>
            </td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ctlUpdatePanelEmailGrid" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <fieldset id="ctlFieldSetGrid" style="width:98%" class="table">
        <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="ctlUpdatePanelEmailGrid"
                DynamicLayout="true" EnableViewState="False">
                <ProgressTemplate>
                    <uc3:SCGLoading ID="SCGLoading1" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <ss:BaseGridView ID="ctlEmailLogGrid" runat="server"  AutoGenerateColumns="false" CssClass="Grid" AllowSorting="true"
            AllowPaging="true" DataKeyNames="EmailLogID" EnableInsert="False" OnRequestCount="RequestCount" OnRequestData="RequestData"
            SelectedRowStyle-BackColor="#6699FF" Width="100%" OnRowDataBound="ctlEmailLogGrid_RowDataBound">
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <RowStyle CssClass="GridItem" />
            <Columns>
                <asp:TemplateField HeaderText="RequestNo" HeaderStyle-HorizontalAlign="Center" SortExpression="RequestNo">
                    <ItemTemplate>
                        <asp:Label ID="ctlRequestNo" runat="server" Text='<%# Bind("RequestNo") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="115" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EmailType" HeaderStyle-HorizontalAlign="Center" SortExpression="EmailType">
                    <ItemTemplate>
                        <asp:Label ID="ctlEmailType" runat="server" Text='<%# Bind("EmailType") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SendDate" HeaderStyle-HorizontalAlign="Center" SortExpression="SendDate">
                    <ItemTemplate>
                        <asp:Label ID="ctlSendDate" runat="server" Text='<%# SCG.eAccounting.Web.Helper.UIHelper.BindDateTime(Eval("SendDate").ToString())%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="130" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To" HeaderStyle-HorizontalAlign="Center" SortExpression="ToEmail">
                    <ItemTemplate>
                        <asp:Label ID="ctlTo" runat="server" Text='<%# Bind("ToEmail") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" Wrap="true" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CC" HeaderStyle-HorizontalAlign="Center" SortExpression="CCEmail">
                    <ItemTemplate>
                        <asp:Label ID="ctlCC" runat="server" Text='<%# Bind("CCEmail") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" Wrap="true"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" SortExpression="Status">
                    <ItemTemplate>
                        <asp:Label ID="ctlStatus" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" Width="55" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" SortExpression="Remark">
                    <ItemTemplate>
                        <asp:Label ID="ctlRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" />
                </asp:TemplateField>
            </Columns>
        </ss:BaseGridView>
                  
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>