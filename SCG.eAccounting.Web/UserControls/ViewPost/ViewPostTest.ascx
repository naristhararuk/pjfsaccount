<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ViewPostTest.ascx.cs" 
    Inherits="SCG.eAccounting.Web.UserControls.ViewPost.ViewPostTest" 
    EnableTheming ="true"
%>

<%@ Register src="ViewPostMessage.ascx" tagname="ViewPostMessage" tagprefix="uc1" %>
<%@ Register src="ViewPostEditDate.ascx" tagname="ViewPostEditDate" tagprefix="uc3" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc2" %>

<asp:Panel ID="pnViewPostShow" runat="server" Width="1000px" BackColor="White" Style="display: none">

    <asp:UpdatePanel ID="UpdatePanelSearchAccount" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	    
        <asp:Panel ID="pnViewPostShowHeader" CssClass="table" runat="server" Style="border:solid 2px Gray;color:Black;background:#33CCFF;">
        
        <table width="100%">
        <tr>
            <td align="left" valign="top" width="100%">
                <div style="cursor:move;width:100%">
                    <b>
	                <asp:Label ID="lblSpace1" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
		            <asp:Label ID="lblCapture1" runat="server" Text='View Post : ' ></asp:Label>
		            <asp:Label ID="lblDocumentNo" runat="server" Text='EXP-0001' ForeColor="Blue"></asp:Label>
		            <br />
		            <asp:Label ID="lblSpace2" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
		            <asp:Label ID="lblCapture2" runat="server" Text='Status : ' ></asp:Label>
		            <asp:Label ID="lblStatus" runat="server" Text='New' ForeColor="Red"></asp:Label>
		            </b>
                </div>
            </td>
            <td align="right" valign="top" >
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/images/icon/BtDelete.gif"
                    OnClick="ctlCloseImageButton_Click" />
            </td>
        </tr>
        </table>
	        
	    </asp:Panel>
	    
	    <asp:UpdateProgress ID="UpdatePanelGridViewProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelSearchAccount"
            DynamicLayout="true" EnableViewState="False">
            <ProgressTemplate>
                <uc2:SCGLoading ID="SCGLoading1"  runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        
	    <table width="100%">
	        <tr>
	            <td align="left">
                    <asp:Button ID="btnSimulate"    runat="server" Text="Simulate"  OnClick="btnSimulate_Click" Width="100px"/>
                    <asp:Button ID="btnPost"        runat="server" Text="Post"      OnClick="btnPost_Click"     Width="100px"/>
                    <asp:Button ID="btnApprove"     runat="server" Text="Approve"   OnClick="btnApprove_Click"  Width="100px"/>
                    <asp:Button ID="btnReverse"     runat="server" Text="Reverse"   OnClick="btnReverse_Click"  Width="100px"/>
                    <asp:Button ID="btnClose"       runat="server" Text="Close"     OnClick="btnClose_Click"    Width="100px"/>
	            </td>
	        </tr>
	    </table>
	    <div style="overflow:scroll;height:400px;width:1000px">
            <table width="2140px">
                <tr>
                    <td style="width:140px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:120px"><hr /></td>
                    <td style="width:140px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:105px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:140px"><hr /></td>
                    <td style="width:140px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:100px"><hr /></td>
                    <td style="width:200"><hr /></td>
                    <td style="width:220"><hr /></td>
                </tr>
                <tr style="background-color:#CCFFFF">
                    <td>Doc.No.</td>
                    <td>Branch</td>
                    <td>HEADER TXT<br />PK</td>
                    <td>Reference<br />Account</td>
                    <td>Doc.Date<br />Amount</td>
                    <td>Exchange Rate</td>
                    <td>Amount THB</td>
                    <td>Post.Date<br />Tx</td>
                    <td>Doc.Type<br />Base.Date</td>
                    <td>Com.Code<br />Term</td>
                    <td>Period<br />Mth</td>
                    <td>Year<br />B</td>
                    <td>Currency<br />CostCenter</td>
                    <td><br />Inter.Order</td>
                    <td><br />W/H Code1</td>
                    <td><br />W/H Base1</td>
                    <td><br />W/H Code2</td>
                    <td><br />W/H Base2</td>
                    <td><br />Assignment</td>
                    <td><br />Text</td>
                </tr>
                <tr>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                    <td><hr /></td>
                </tr>
            </table>
            <div align="left">
            <b>
            <asp:Label ID="Label1" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
            <asp:Label ID="lblComCode" runat="server" ></asp:Label>
            <asp:Label ID="Label2" runat="server" Text='&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'></asp:Label>
			<asp:Label ID="lblComName" runat="server" ></asp:Label>
			</b>
			</div>
			<asp:GridView ID="GridViewShow" runat="server" AutoGenerateColumns="False" 
                ShowHeader="false" Width="2140px" Font-Size="Medium" 
                onrowdatabound="GridViewShow_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="FI_DOC" DataField="FIELD01">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Branch" DataField="FIELD08">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Header TXT" DataField="HEADERTXT">
                        <ItemStyle Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Reference" DataField="FIELD10">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="DocDate" DataField="FIELD02">
                        <ItemStyle Width="100px"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ExRate" DataField="ExRate">
                        <ItemStyle Width="100px"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="AmountTHB" DataField="AmountTHB">
                        <ItemStyle Width="100px"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="PostDate" DataField="FIELD03">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="DocType" DataField="FIELD04">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="CompCode" DataField="FIELD05">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Period" DataField="FIELD06">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Year" DataField="FIELD07">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Currency" DataField="FIELD09">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="InterOrder" DataField="FIELD11">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTCode1" DataField="FIELD12">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTBase1" DataField="FIELD13">
                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTCode2" DataField="FIELD14">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTBase2" DataField="FIELD15">
                        <ItemStyle Width="100px" HorizontalAlign="Right"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Assignment" DataField="FIELD16">
                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Text" DataField="FIELD17">
                        <ItemStyle Width="220px" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            
            <div runat=server id="B2C">
            
            <div align="left">
            <b>
            <asp:Label ID="Label3" runat="server" Text='&nbsp;&nbsp;'></asp:Label>
            <asp:Label ID="lblComCodeB2C" runat="server" ></asp:Label>
            <asp:Label ID="Label5" runat="server" Text='&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'></asp:Label>
			<asp:Label ID="lblComNameB2C" runat="server" ></asp:Label>
			</b>
			</div>
			<asp:GridView ID="GridViewShowB2C" runat="server" AutoGenerateColumns="False" 
                ShowHeader="false" Width="2140px" Font-Size="Medium" 
                onrowdatabound="GridViewShow_RowDataBound">
                <Columns>
                       <asp:BoundField HeaderText="FI_DOC" DataField="FIELD01">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Branch" DataField="FIELD08">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Header TXT" DataField="HEADERTXT">
                        <ItemStyle Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Reference" DataField="FIELD10">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="DocDate" DataField="FIELD02">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ExRate" DataField="ExRate">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="AmountTHB" DataField="AmountTHB">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="PostDate" DataField="FIELD03">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="DocType" DataField="FIELD04">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ComCode" DataField="FIELD05">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Period" DataField="FIELD06">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Year" DataField="FIELD07">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Currency" DataField="FIELD09">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="InterOrder" DataField="FIELD11">
                        <ItemStyle Width="140px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTCode1" DataField="FIELD12">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTBase1" DataField="FIELD13">
                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTCode2" DataField="FIELD14">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="WHTBase2" DataField="FIELD15">
                        <ItemStyle Width="100px" HorizontalAlign="Right"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Assignment" DataField="FIELD16">
                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Text" DataField="FIELD17">
                        <ItemStyle Width="220px" HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            
            </div>
            
            <table border="0" cellpadding="0" cellspacing="0" align="left">
                <tr align="left">
                    <td align="left">
                        <font color="red">
                                <spring:ValidationSummary ID="ctlValidationSummary" runat="server" Provider="ViewPost.Error"  />
                                <spring:ValidationSummary ID="ctlWorkflowValidationSummary" runat="server" Provider="WorkFlow.Error"  />
                        </font>
                    </td>
                </tr>
            </table>
        </div>
        
        
        
	    </ContentTemplate>
	    <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSimulate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPost" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReverse" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
        </Triggers>
    
    </asp:UpdatePanel>
    
</asp:Panel>

<asp:LinkButton ID="lnkDummy" runat="server" style="visibility:hidden"/>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
	TargetControlID="lnkDummy"
	PopupControlID="pnViewPostShow"
	BackgroundCssClass="modalBackground"
	CancelControlID="lnkDummy"
	DropShadow="true" 
	RepositionMode="None" 
	PopupDragHandleControlID="pnViewPostShowHeader" zIndex="10001" />


<uc1:ViewPostMessage  ID="ViewPostMessage1"  runat="server" />
<uc3:ViewPostEditDate ID="ViewPostEditDate1" runat="server" />

