<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemittanceGeneral.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.RemittanceGeneral" %>
<asp:Panel ID="pnRemit" runat="server" Width="100%" BackColor="White">
    <asp:UpdatePanel ID="UpdatePanelRemit" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	        <center>
            <table id="ctlRemittedTable" runat="server" width="99%">
	            <tr>
		            <td>
			            <asp:Label ID="ctlMode" runat="server"></asp:Label>
			            <ss:BaseGridView ID="RemitGridView" runat="server" AutoGenerateColumns="False" 
				            EnableInsert="False" InsertRowCount="1" SaveButtonID="" CssClass="table">
				            <Columns>
					            <asp:TemplateField HeaderText="Payment Type">
						            <ItemTemplate>
							            <asp:DropDownList ID="ctlDdlPaymentType" runat="server" AutoPostBack="True" >
                                            <asp:ListItem>Book Note</asp:ListItem>
                                        </asp:DropDownList>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Center"  />
						            <HeaderStyle HorizontalAlign ="Center" Width="10%" />
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Currency" >
						            <ItemTemplate>
							            <asp:DropDownList ID="ctlDdlCurrency" runat="server" AutoPostBack="True" >
                                            <asp:ListItem>AUD</asp:ListItem>
                                            <asp:ListItem>USD</asp:ListItem>
                                        </asp:DropDownList>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Center" />
						            <HeaderStyle HorizontalAlign ="Center" Width="10%" />
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Foreign Currency Advanced"  >
						            <ItemTemplate>
							            <asp:Label ID="cltLblForeignAdvanced" runat="server" Text='<%# Bind("ForeignAdvance") %>'></asp:Label>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Right" />
						            <HeaderStyle HorizontalAlign ="Center" Width="20%" />
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Exchange Rate" >
						            <ItemTemplate>
							            <asp:Label ID="cltLblExchange" runat="server"  Text='<%# Bind("ExchangeRate") %>'></asp:Label>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Right" />
						            <HeaderStyle HorizontalAlign ="Center" Width="15%"/>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Foreign Currency Remitted">
						            <ItemTemplate>
							            <asp:Label ID="ctlLblForeignRemit" runat="server" Text='<%# Bind("ForeignRemit") %>'></asp:Label>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Right" />
						            <HeaderStyle HorizontalAlign ="Center" Width="15%"/>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Action">
						            <ItemTemplate>
							            <asp:LinkButton ID="ctlDelete" runat="server" Text="Delete"></asp:LinkButton>
						            </ItemTemplate>
						            <ItemStyle HorizontalAlign="Center" />
						            <HeaderStyle HorizontalAlign ="Center" Width="15%"/>
						            <FooterTemplate>
						                <asp:LinkButton ID="ctlAdd" runat="server" Text="Add"></asp:LinkButton>
						            </FooterTemplate>
						            <FooterStyle HorizontalAlign="Center"/>
					            </asp:TemplateField>
				            </Columns>
			            </ss:BaseGridView>
		            </td>
	            </tr>
            </table>
            </center>    
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel> 


