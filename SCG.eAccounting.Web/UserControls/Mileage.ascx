<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mileage.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.Mileage" EnableTheming="true" %>
<div id="divMileage" align="left">
    <table border="0" id="ctlTabMileage" runat="server" width="70%">
        <tr>
            <td valign="top">
                <fieldset id="ctlFieldSetDetailGridView" runat="server" style="text-align:left;" class="table">
                    <table border="0" width="100%">
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">
                                <asp:Label ID="ctlLblCarLicenseNo" runat="server" Text="Car License No."></asp:Label>
                                <asp:Label ID="ctlLblCarLicenseNoReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left" style="width:35%">
                                <asp:TextBox ID="ctlTxtCarLicenseNo" runat="server" SkinID="SkCtlTextboxCenter" Width="150px"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">
                                <asp:Label ID="ctlLblTypeOfCar" runat="server" Text="Type of Car"></asp:Label>
                                <asp:Label ID="ctlLblTypeOfCarReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left" style="width:35%">
                                <asp:DropDownList ID="ctlDdlTypeOfCar" runat="server" Width="152px" 
                                AutoPostBack="True" onselectedindexchanged="ctlDdlTypeOfCar_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">
                                <asp:Label ID="ctlLblTypeOfUse" runat="server" Text="Type of Use"></asp:Label>
                                <asp:Label ID="ctlLblTypeOfUseReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left" style="width:35%">
                                <asp:DropDownList ID="ctlDdlTypeOfUse" runat="server" Width="152px" ></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">
                                <asp:Label ID="ctlLblOwner" runat="server" Text="Owner"></asp:Label>
                                <asp:Label ID="ctlLblOwnerReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left" style="width:35%">
                                <asp:DropDownList ID="ctlDdlOwner" runat="server" Width="152px" 
                                    AutoPostBack="True" onselectedindexchanged="ctlDdlOwner_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">
                                <asp:Label ID="ctlLblPermissionNo" runat="server" Text="Permission No."></asp:Label>
                                <asp:Label ID="ctlLblPermissionNoReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                            </td>
                            <td align="left" style="width:35%">
                                <asp:TextBox ID="ctlTxtPermissionNo" runat="server" SkinID="SkCtlTextboxCenter" Width="150px"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td colspan="3" align="left" style="width:100%">
                                <asp:Label ID="ctlLblDistance" runat="server" Text="Distance (km.) -"></asp:Label>
                            </td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">&nbsp;</td>   
                            <td align="left" style="width:35%">                       
                                <asp:Label ID="ctlLblHomeOffice" runat="server" Text="Home to Office Round Trip"></asp:Label>&nbsp;:&nbsp;                            
                            </td>
                            <td>
                                <asp:TextBox ID="ctlTxtHomeOffice" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width:5%">&nbsp;</td>
                            <td align="left" style="width:25%">&nbsp;</td> 
                            <td align="left" style="width:35%">                                                
                                <asp:Label ID="ctlLblPrivateUse" runat="server" Text="Private Use"></asp:Label>&nbsp;:&nbsp;                           
                            </td>
                            <td>
                                <asp:TextBox ID="ctlTxtPrivateUse" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td align="left" style="width:5%">&nbsp;</td>
                        </tr>
                        
                        <asp:UpdatePanel ID="ctlUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="divAsDetermined" runat="server">
                                    <tr>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                        <td colspan="3" align="left" style="width:100%">
                                            <asp:Label ID="ctlLblAsDetermined" runat="server" Text="As Determined Rate -"></asp:Label>
                                        </td>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                        <td align="left" style="width:25%">&nbsp;</td> 
                                        <td align="left" style="width:35%">                                            
                                            <asp:Label ID="ctlLblRate" runat="server" Text="1-100 lm. Rate"></asp:Label>&nbsp;:&nbsp;                           
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ctlTxtRate" runat="server" SkinID="SkCtlTextboxRight" Width="150px"></asp:TextBox>
                                        </td>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                        <td align="left" style="width:25%">&nbsp;</td> 
                                        <td align="left" style="width:35%">                      
                                            <asp:Label ID="ctlLblExceedingRate" runat="server" Text="Exceeding 100 km. Rate"></asp:Label>&nbsp;:&nbsp;                           
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ctlTxtExceedingRate" runat="server" SkinID="SkCtlTextboxRight" Width="150px"></asp:TextBox>
                                        </td>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                    </tr>
                                </div>
                                <div id="divAmount" runat="server">
                                    <tr>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                        <td align="left" style="width:25%">
                                            <asp:Label ID="ctlLblAmount" runat="server" Text="Amount"></asp:Label>
                                            <asp:Label ID="ctlLblAmountReq" runat="server" Text="*" style="color:Red;"></asp:Label>&nbsp;:&nbsp;
                                        </td>
                                        <td align="left" style="width:35%">
                                            <asp:TextBox ID="ctlTxtAmount" runat="server" SkinID="SkCtlTextboxRight" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td align="left" style="width:5%">&nbsp;</td>
                                    </tr>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </table>                
                </fieldset>                
            </td>
        </tr>
    </table>
</div>
<br />
<div id="divMileageAdd" align="left">
    <table border="0" id="ctlTabMileageAdd" runat="server" width="80%" class="table">
        <tr>
            <td align="center" style="width:10%">
                <asp:Label ID="ctlLblDate" runat="server" Text="Date"></asp:Label>
            </td>
            <td align="center" style="width:20%">
                <asp:Label ID="ctlLblLocationFrom" runat="server" Text="Location From"></asp:Label>
            </td>
            <td align="center" style="width:20%">
                <asp:Label ID="ctlLblLocationTo" runat="server" Text="Location To"></asp:Label>
            </td>
            <td align="center" style="width:10%">
                <asp:Label ID="ctlLblCarMeterStart" runat="server" Text="Car Meter - Start"></asp:Label>
            </td>
            <td align="center" style="width:10%">
                <asp:Label ID="ctlLblCarMeterEnd" runat="server" Text="Car Meter - End"></asp:Label>
            </td>
            <td align="center" style="width:10%">
                <asp:Label ID="ctlLblAdjust" runat="server" Text="Adjust(km.)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" style="width:10%">
                <asp:TextBox ID="ctlTxtDate" runat="server" SkinID="SkCtlTextboxCenter" Width="100px"></asp:TextBox>
            </td>
            <td align="left" style="width:20%">
                <asp:TextBox ID="ctlTxtLocationFrom" runat="server" SkinID="SkCtlTextboxLeft" Width="200px"></asp:TextBox>
            </td>
            <td align="left" style="width:20%">
                <asp:TextBox ID="ctlTxtLocationTo" runat="server" SkinID="SkCtlTextboxLeft" Width="200px"></asp:TextBox>
            </td>
            <td align="left" style="width:10%">
                <asp:TextBox ID="ctlTxtCarMeterStart" runat="server" SkinID="SkCtlTextboxRight" Width="100px"></asp:TextBox>
            </td>
            <td align="left" style="width:10%">
                <asp:TextBox ID="ctlTxtCarMeterEnd" runat="server" SkinID="SkCtlTextboxRight" Width="100px"></asp:TextBox>            
            </td>            
            <td align="left" style="width:10%">
                <asp:TextBox ID="ctlTxtAdjust" runat="server" SkinID="SkCtlTextboxRight" Width="100px"></asp:TextBox>
            </td>
        </tr>
		<tr align="left">
			<td  align="left">
				<asp:Button ID="ctlBtnAdd" runat="server" Text="Add" />
			</td>
	    </tr>
    </table>        
</div>
<br />
<asp:UpdatePanel ID="ctlUpdatePanelTotal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divTotalAmount" class="table" align="left" runat="server">
            <table width="100%">
                <tr>
                    <td valign="top">
                        <table width="100%" border="1">
                            <tr>
                                <td align="left" style="width:10%">
                                    <asp:Label ID="ctlLblTotalAmount" runat="server" Text="Total Amount"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td align="right" style="width:10%">
                                    <asp:Label ID="ctlLblTotalAmountData" runat="server" Text='<%# Bind("TotalAmount")%>'></asp:Label>
                                </td>
                                <td align="left" style="width:2%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" style="width:10%">
                                    <asp:Label ID="ctlLblSupport" runat="server" Text="เงินช่วยเหลือ"></asp:Label>
                                </td>
                                <td align="right" style="width:10%">
                                    <asp:Label ID="ctlLblSupportData" runat="server" Text='<%# Bind("Support")%>'></asp:Label>
                                </td>
                                <td align="left" style="width:2%">(-)</td>
                            </tr>   
                            <tr>
                                <td align="left" style="width:10%">
                                    <asp:Label ID="ctlLblOverSupport" runat="server" Text="ส่วนเกินเงินช่วยเหลือ"></asp:Label>
                                </td>
                                <td align="right" style="width:10%">
                                    <asp:Label ID="ctlLblOverSupportData" runat="server" Text='<%# Bind("OverSupport")%>'></asp:Label>
                                </td>
                                <td align="left" style="width:2%">&nbsp;</td>
                            </tr>               
                        </table>
                    </td>
                    <td>&nbsp;</td>
                    <td valign="top">
                        <table width="100%" border="1">
                            <tr>
                                <td align="left" style="width:30%">&nbsp;</td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderDistanceTotal" runat="server" Text="Distance"></asp:Label>
                                </td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderRateTotal" runat="server" Text="Rate"></asp:Label>
                                </td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderAmountTotal" runat="server" Text="Amount"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width:30%">
                                    <asp:Label ID="ctlLblOneToHundred" runat="server" Text="1-100 km."></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblOneToHundredDistance" runat="server" Text='<%# Bind("OneToHundredDistance") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblOneToHundredRate" runat="server" Text='<%# Bind("OneToHundredRate") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblOneToHundredAmount" runat="server" Text='<%# Bind("OneToHundredAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width:30%">
                                    <asp:Label ID="ctlLblExceedingHundred" runat="server" Text="Exceeding 100 km."></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblExceedingHundredDistance" runat="server" Text='<%# Bind("ExceedingHundredDistance") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblExceedingHundredRate" runat="server" Text='<%# Bind("ExceedingHundredRate") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblExceedingHundredAmount" runat="server" Text='<%# Bind("ExceedingHundredAmount") %>'></asp:Label>
                                </td>
                            </tr>  
                            <tr>
                                <td colspan="3" align="right" style="width:60%">
                                    <asp:Label ID="ctlLblTotalAmountAll" runat="server" Text="Total Amount"></asp:Label>&nbsp;:&nbsp;
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblTotalAmountAllData" runat="server" Text='<%# Bind("TotalAmountAll") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width:30%">&nbsp;</td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderTotalDistance" runat="server" Text="Distance"></asp:Label>
                                </td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderTotalRate" runat="server" Text="Rate"></asp:Label>
                                </td>
                                <td align="center" style="width:20%">
                                    <asp:Label ID="ctlLblHeaderTotalSupport" runat="server" Text="เงินช่วยเหลือ"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width:30%">
                                    <asp:Label ID="ctlLblTotalDistance" runat="server" Text="Total Distance"></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblTotalDistanceData" runat="server" Text='<%# Bind("TotalDistanceData") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblTotalRateData" runat="server" Text='<%# Bind("TotalRateData") %>'></asp:Label>
                                </td>
                                <td align="right" style="width:20%">
                                    <asp:Label ID="ctlLblTotalSupportData" runat="server" Text='<%# Bind("TotalSupportData") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divTotalAmountOpen" class="table" runat="server" align="left">
            <table width="40%" border="1">
                <tr valign="top">
                    <td align="left" style="width:20%">
                        <asp:Label ID="ctlLblTotalAmountOpen" runat="server" Text="จำนวนเงินที่สามารถเบิกได้"></asp:Label>&nbsp;:&nbsp;
                    </td>
                    <td align="right" style="width:20%">
                        <asp:Label ID="ctlLblTotalAmountOpenData" runat="server" Width="152px" Text='<%# Bind("TotalAmountOpenData") %>'></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div id="divBotton" align="left">
    <table>
        <tr>
            <td>
                <asp:Button ID="ctlBtnAddGeneralExpense" runat="server" Text="Add General Expense" />
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="ctlBtnCalculate" runat="server" Text="Calculate" />
            </td>
            <td>
                <asp:Button ID="ctlBtnOK" runat="server" Text="OK" />
            </td>
            <td>
                <asp:Button ID="ctlBtnCancel" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</div>
