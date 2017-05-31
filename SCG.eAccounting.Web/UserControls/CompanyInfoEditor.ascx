<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyInfoEditor.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.CompanyInfoEditor" %>
<%@ Register Src="LOV/SCG.DB/CompanyLookUp.ascx" TagName="CompanyLookUp" TagPrefix="uc1" %>
<%@ Register src="ExpenseCompanyEditor.ascx" tagname="ExpenseCompanyEditor" tagprefix="uc2" %>
<asp:Panel ID="ctlCompanyInfoEditorPanel" runat="server" Style="display: block">
    <asp:UpdatePanel ID="ctlCompanyInfoUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 70%" id="ctlCompanyInfoFieldSet" runat="server" visible="False"
                enableviewstate="True">   
                <legend id="ctlLegendCompanyInfo" runat="server" style="color: #4E9DDF"  visible="True">
              <asp:Label ID="ctlDetailHeaderCompanyInformation" runat="server" Text="$Company's Information$" /></legend>       
                <ss:BaseGridView ID="ctlCompanyInfoGrid" runat="server" AutoGenerateColumns="false" OnRowCommand="CompanyInfoGrid_RowCommand"
                    DataKeyNames="ID,AccountID" CssClass="Grid" Width="100%" OnDataBound="CompanyInfoGrid_Databound"
                     AllowSorting="false" OnRequestData="RequestData" >
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ctlHeader"  runat="server" onclick="javascript:validateCheckBoxControl(this, '0');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ctlSelect"  runat="server" onclick="javascript:validateCheckBoxControl(this, '1');" />
                            </ItemTemplate>
                              <ItemStyle Width="3%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company Code" HeaderStyle-Width="18%" SortExpression="CompanyCode">
                            <ItemTemplate>
                                <asp:Label ID="ctlCompanyCode"  runat="server" Text='<%# Bind("CompanyCode") %>'></asp:Label>
                            </ItemTemplate>
                              <ItemStyle Width="18%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company Name" HeaderStyle-Width="80%" SortExpression="CompanyName" >
                            <ItemTemplate>
                                <asp:Label ID="ctlCompanyName"  runat="server" Text='<%# Bind("CompanyName") %>'></asp:Label>
                            </ItemTemplate>
                              <ItemStyle Width="60%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField  ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ctlEdit" runat="server" ToolTip="Edit" SkinID="SkCtlGridEdit" CommandName="CompanyEdit"
                                CausesValidation="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                    </asp:TemplateField>
                        </Columns>
                </ss:BaseGridView>
                <asp:ImageButton runat="server" ID="ctlSave" ToolTip="Add" SkinID="SkAddButton"
                    OnClick="SaveCompanyInfo_Click" ImageAlign="Left" />
                <asp:ImageButton runat="server" ID="ctlDelete" ToolTip="Delete" SkinID="SkDeleteButton"
                    CausesValidation="False" OnClientClick="return confirm('Are you sure delete this row');"
                    OnClick="DeleteCompanyInfo_Click" ImageAlign="Left" />
                <asp:ImageButton runat="server" ID="ctlClose" ToolTip="Close" SkinID="SkCancelButton"
                    OnClick="CloseCompanyInfo_Click" ImageAlign="Left" />
                    
                    <font color="red">
                           
                            <asp:Label ID="ShowAdd" runat="server" Text=''></asp:Label>&nbsp;
                     </font>
            </fieldset>
            <asp:HiddenField ID="acount" runat="server" />
            

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<uc1:CompanyLookUp ID="ctlCompanyLookUp" runat="server" IsMultiple="true" />
<uc2:ExpenseCompanyEditor ID="ctlExpenseCompanyEditor" runat="server" />
