<%@ Page 
    Language="C#" 
    MasterPageFile="~/LoginPage.Master" 
    AutoEventWireup="true" 
    CodeBehind="GetLog.aspx.cs" 
    Inherits="SCG.eAccounting.Web.GetLog" 
    Title="Untitled Page" 
    StylesheetTheme="Default" 
    EnableEventValidation="false" 
    EnableTheming="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWelcomeMsg" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderService" runat="server">

<asp:UpdatePanel ID="UpdatePanelShow" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div id="divLogin" align="center" visible="true" runat="server">

    <table>
        <tr>
            <td valign="center">
                <fieldset id="fieldLogin" runat="server" style="width:300px" title="Welcome...">
                <table  width="100%">
                <tr>
                    <td colspan=2 align="left">
                        <b><asp:Label ID="lblShow" runat="server" Text="Welcome..." ForeColor="Blue" Font-Size="Medium"></asp:Label></b>
                    </td>
                </tr>
                <tr>
                    <td align="right">UserName : </td>
                    <td align="left">
                        <asp:TextBox ID="txtUserName" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Password : </td>
                    <td align="left">
                        <asp:TextBox ID="txtPassword" runat="server" Width="150px" MaxLength="20" TextMode="Password" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnOK" runat="server" Text="LOGIN" onclick="btnOK_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" 
                            onclick="btnCancel_Click" />
                    </td>
                </tr>
                </table>
                </fieldset>
            </td>
            <td align="right">
                <asp:Image ID="imgReach" ImageUrl="~/App_Themes/Default/images/Slide/REACH.gif" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div id="divShow" align="left" visible="false" runat="server">
    <p>
        Get Log Error Form Server</p>
    <p>
        <asp:GridView ID="ctlGrdDetail" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" GridLines="Horizontal" Width="823px" 
            onrowdatabound="ctlGrdDetail_RowDataBound">
            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
            <Columns>
                <asp:BoundField HeaderText="ลำดับ" DataField="SEQ">
                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ชื่อไฟล์" DataField="NAME" >
                    <HeaderStyle HorizontalAlign="Center" Width="300px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="ขนาด" DataField="SIZE" >
                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField HeaderText="วันที่สร้าง" DataField="CREDATE" >
                    <HeaderStyle HorizontalAlign="Center" Width="170px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="วันที่แก้ไข" DataField="MODDATE" >
                    <HeaderStyle HorizontalAlign="Center" Width="170px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Download">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ctlImgDownload" runat="server" 
                            ImageUrl="~/App_Themes/Default/images/icon/isv.gif" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
            <AlternatingRowStyle BackColor="#F7F7F7" />
        </asp:GridView>
    </p>
    <br />
</div>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
