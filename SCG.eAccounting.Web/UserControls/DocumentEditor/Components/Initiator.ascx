<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" EnableTheming="true"
    CodeBehind="Initiator.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.Initiator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="AddInitiatorLookup.ascx" TagName="AddInitiatorLookup" TagPrefix="uc1" %>
<%@ Register Src="../../LOV/SCG.DB/UserAutoCompleteLookup.ascx" TagName="UserAutoCompleteLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../LOV/SCG.DB/UserProfileLookup.ascx" TagName="UserProfileLookup"
    TagPrefix="uc3" %>
<%@ Register src="~/UserControls/Shared/SCGLoading.ascx" tagname="SCGLoading" tagprefix="uc4" %>

<fieldset id="ctlFieldMain">
    <legend id="ctlLegendDetailGridView" style="color: #4E9DDF" class="table">
        <asp:Label ID="ctlGridHeader" runat="server" Text="Initiation"></asp:Label>
    </legend>

    <script type="text/javascript">
        //this script use for event user press Enter on keyboard
        nn = (document.layers) ? true : false;
        ie = (document.all) ? true : false;
        document.onkeydown = keyDown;
        if (nn) document.captureEvents(Event.KEYDOWN);

        function keyDown(e) {
            var evt = (e) ? e : (window.event) ? window.event : null;
            if (evt) {
                var key = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0));
                if (key == 13) {
                    handleReturn();
                }
            }
        }
        function handleReturn() {
            return false;
            //    alert(element.name);
            //    var str='';
            //    for(i=0; i<document.form1.elements.length; i++){
            //        var el=document.getElementById(document.form1.elements[i].id)
            //    }
        }
        function process(el) {
            element = el;
        }
        window.onkeydown = process(this);
    </script>

    <input id="HiddenDocument" runat="server" type="hidden" />
    <asp:UpdatePanel ID="InitiatorUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <asp:ImageButton ID="btnAddInitiatorItem" runat="server" OnClick="ctlLooupInitiator_Click" SkinID="SkAddButton"/>
                <asp:ImageButton ID="btnAddInitiatorAutoItem" runat="server" OnClick="btnAddInitiatorAutoItem_Click" SkinID="SkCtlFavorite"/>
            <center>
                <asp:UpdateProgress ID="ctlUpdateProgressInitiator" runat="server" AssociatedUpdatePanelID="InitiatorUpdatePanel"
                    DynamicLayout="true" EnableViewState="true">
                    <ProgressTemplate>
                        <uc4:SCGLoading ID="SCGLoading1" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Label ID="ctlMode" runat="server" Style="display: none;"></asp:Label>
            </center>
            <div class="InitiatorReorderList">
                <table style="width: 100%;" align="center" class="table">
                    <tr>
                        <td colspan="5" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center" class="GridHeader">
                        <td style="width: 10%">
                            <asp:Label ID="ctlNoLabel" runat="server" Text="$No.$"></asp:Label>
                        </td>
                        <td style="width: 30%">
                             <asp:Label ID="ctlInitiator" runat="server" Text="$Initiator$"></asp:Label>
                        </td>
                        <td style="width: 35%">
                            <asp:Label ID="ctlEmailLabel" runat="server" Text="$Email$"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="ctlTypeLabel" runat="server" Text="$Type$"></asp:Label>
                        </td>
                        <td style="width: 10%">
                            <asp:Label ID="ctlSMSLabel" runat="server" Text="$SMS$"></asp:Label>
                        </td>
                    </tr>
                </table>
                <ajaxToolkit:ReorderList ID="InitiatorReorderList" runat="server" PostBackOnReorder="true"
                    CallbackCssStyle="InitiatorCallbackStyle" DataKeyField="initiatorID" SortOrderField="Seq"
                    ShowInsertItem="true" EnableViewState="true" OnDeleteCommand="InitiatorReorderList_DeleteCommand"
                    OnItemReorder="InitiatorReorderList_ItemReorder" Width="100%" class="GridItem" 
                    OnItemDataBound="InitiatorReorderList_ItemDataBound">
                    <ItemTemplate>
                        <div class="InitiatorItemArea">
                            <table style="width: 100%; border-color: white;" cellpadding="0" cellspacing="0"
                                align="center" id="ctlReorderTable" runat="server" border="0">
                                <tr>
                                    <td style="width: 8%" align="left">
                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("Seq") %>' />
                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("UserID") %>' Style="visibility: hidden;" />
                                    </td>
                                    <td style="width: 34%" align="left">
                                        <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("EmployeeName"))) %>' />&nbsp;&nbsp;
                                        <%--										<asp:Label ID="Label3" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("LastName"))) %>' />
--%>
                                    </td>
                                    <td style="width: 37%" align="left">
                                        <asp:Label ID="Label2" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Email"))) %>' />
                                    </td>
                                    <td style="width: 17%" align="center">
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" SelectedIndex='<%# FindInitiatorTypeIndex(Eval("InitiatorType")) %>'
                                            DataValueField="InitiatorType" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Accept" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="CC" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="width: 10%" align="center">
                                        <asp:CheckBox ID="SMSCheckBox" runat="server" Enabled="false" Checked='<%# Eval("SMS") %>' />
                                    </td>
                                    <td style="width: 5%" align="center">
                                        <asp:ImageButton ID="ImageDeleteButton" runat="server" SkinID="SkCtlGridDelete" CommandName="Delete" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="InitiatorItemArea" id="ctlDivAddInitiatorhhh" runat="server">
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Seq") %>' ValidationGroup="edit" />
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("UserID") %>' ValidationGroup="edit" />
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Type") %>' ValidationGroup="edit" />
                        </div>
                    </EditItemTemplate>
                    <ReorderTemplate>
                        <asp:Panel ID="Panel2" runat="server" CssClass="InitiatorReorderCue" />
                    </ReorderTemplate>
                    <DragHandleTemplate>
                        <div class="InitiatorDragHandle">
                        </div>
                    </DragHandleTemplate>
                    <InsertItemTemplate>
                        <!-- bottom border is workaround for IE7 Beta issue where bg doesn't render -->
                        <div style="padding-left: 25px; border-bottom: thin solid transparent;" id="ctlDivAddInitiator"
                            runat="server">
                            <asp:Panel ID="panel1" runat="server" DefaultButton="CtlAddInitiatorButton" HorizontalAlign="Left">
                                <asp:Button ID="CtlAddInitiatorButton" Visible="false" runat="server" CommandName="Insert"
                                    Text="Add" />
                                <asp:Button ID="CtlAutoInitiatorButton" Visible="false" runat="server" CommandName="Insert"
                                    Text="Auto" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="add"
                                        ErrorMessage="Please enter some text" ControlToValidate="TextBox1" />--%></asp:Panel>
                        </div>
                    </InsertItemTemplate>
                </ajaxToolkit:ReorderList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<uc3:UserProfileLookup ID="ctlInitiatorLookup" runat="server" isMultiple="true" />
