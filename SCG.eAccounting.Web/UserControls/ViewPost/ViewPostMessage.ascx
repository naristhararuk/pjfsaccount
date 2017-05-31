<%@ 
    Control Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ViewPostMessage.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ViewPost.ViewPostMessage" 
    EnableTheming ="true"
%>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc2" %>

<asp:Panel ID="pnViewPostShow1" runat="server" Width="800px" BackColor="White" Style="display: none">

    <asp:Panel ID="pnViewPostShowHeader1" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;">
    
    <table width="100%">
    <tr>
        <td align="left" valign="top" width="100%">
            <div style="cursor:move;width:100%">
                <b>
                <asp:Label ID="lblSpace1" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
                <asp:Label ID="lblCapture1" runat="server" Text='Message' ></asp:Label>
                </b>
            </div>
        </td>
       <%-- <td align="right" valign="top" >
            <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif" OnClick="imgClose_Click" />
        </td>--%>
    </tr>
    </table>
    
	</asp:Panel>
	
	<asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	    
	    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelSearchAccount"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc2:SCGLoading ID="SCGLoading1"  runat="server" />               
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <table width="800px">
            <tr>
                <td align="left">
                    <b>
                    <asp:Label ID="Label1" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
                    <asp:Label ID="lblComCode" runat="server" ></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text='&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'></asp:Label>
			        <asp:Label ID="lblComName" runat="server" ></asp:Label>
			        </b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewShow" runat="server" AutoGenerateColumns="False" 
                        ShowHeader="true" Width="800px" Font-Size="Medium" 
                        SelectedRowStyle-BackColor="#6699FF" 
                        HeaderStyle-CssClass="GridHeader"
                        onrowdatabound="GridViewShow_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="DocSeq" DataField="DOC_SEQ">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ReturnType" DataField="RETURN_TYPE">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Message" DataField="MESSAGE">
                                <ItemStyle Width="600px" HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div runat="server" id="divHeadGridViewShowB2C">
                    <b>
                    <asp:Label ID="Label3" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
                    <asp:Label ID="lblComCodeB2C" runat="server" ></asp:Label>
                    <asp:Label ID="Label5" runat="server" Text='&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'></asp:Label>
			        <asp:Label ID="lblComNameB2C" runat="server" ></asp:Label>
			        </b>
			        </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divGridViewShowB2C">
                    <asp:GridView ID="GridViewShowB2C" runat="server" AutoGenerateColumns="False" 
                        ShowHeader="true" Width="800px" Font-Size="Medium" 
                        SelectedRowStyle-BackColor="#6699FF" 
                        HeaderStyle-CssClass="GridHeader"
                        onrowdatabound="GridViewShow_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="DocSeq" DataField="DOC_SEQ">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ReturnType" DataField="RETURN_TYPE">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Message" DataField="MESSAGE">
                                <ItemStyle Width="600px" HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnClose"       runat="server" Text="Close"     OnClick="btnClose_Click"    Width="100px"/>
                </td>
            </tr>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummy1" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="modalPopupMessage" runat="server" 
	TargetControlID="lnkDummy1"
	PopupControlID="pnViewPostShow1"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy1"
	DropShadow="true" 
	RepositionMode="None" 
	PopupDragHandleControlID="pnViewPostShowHeader1"  zIndex="10002" />