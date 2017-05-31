<%@ Page Title="Monitoring Inbox" Language="C#" AutoEventWireup="true" MasterPageFile="~/ProgramsPages.Master"
    CodeBehind="AccountantMonitoring.aspx.cs" Inherits="SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs.AccountantMonitoring"
    StylesheetTheme="Default" EnableEventValidation="false" EnableTheming="true" %>

<%@ Register Src="~/UserControls/Shared/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ReportViewers.ascx" TagName="ReportViewers" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Shared/SCGLoading.ascx" TagName="SCGLoading" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOV/SCG.DB/CompanyField.ascx" TagName="CompanyField"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 142px;
        }
        .style4
        {
            width: 58px;
        }
        
        .wijmo-wijgrid-groupheaderrow .wijgridtd.wijdata-type-string
        {
            text-align: center;
        }
        /*Parent row*/
        .wijgridth .wijmo-wijgrid-headertext
        {
            text-align: center;
        }
        .wijgridth:first-child .wijmo-wijgrid-headertext
        {
            text-align: center;
        }
        /*Footer*/
        .wijmo-wijgrid-footerrow .wijmo-wijgrid-innercell
        {
            text-align: center;
        }
        .wijmo-wijgrid-footerrow .wijgridtd:first-child .wijmo-wijgrid-innercell
        {
            text-align: left;
        }
        .ajax__calendar_container
        {
            z-index: 500;
        }
        .wijmo-wijgrid tr.wijmo-wijgrid-row td.wijdata-type-currency, .wijmo-wijgrid tr.wijmo-wijgrid-row td.wijdata-type-number, .wijmo-wijgrid tr.wijmo-wijgrid-row td.wijdata-type-datetime
        {
            text-align: center !important;
        }
        .wijmo-wijgrid-headertext
        {
            font-size: 12px;
        }
          .wijmo-wijgrid .wijmo-wijgrid-headerrow .wijmo-c1basefield
        {
        	position:fixed !important;
        }
        .wijmo-wijgrid
        {
        	position:static !important;
        }
        .wijmo-wijgrid-headertext
        {
        	position:static !important;
        }
        .wijmo-wijgrid .wijmo-wijgrid-headerrow TH .wijmo-wijgrid-innercell
        {
        	position:static !important;
        }
    </style>
    <link href="../../../Scripts/Newjs/css/jquery-wijmo.css" rel="stylesheet" type="text/css" />
    <link href="../../../Scripts/Newjs/css/jquery.wijmo-pro.all.3.20152.78.min.css" rel="stylesheet"
        type="text/css" />
    <script src="../../../Scripts/Newjs/required.js" type="text/javascript"></script>
    <script type="text/javascript">

        function CreateDataTable(dataMOdel, col1, col2, col3, col4, col5, col6) {
            var viewModel;
            var data = [];
            data = dataMOdel;
            var col1fromTable = col1;
            var col2fromTable = col2;
            var col3fromTable = col3;
            var col4fromTable = col4;
            var col5fromTable = col5;
            var totalHeaderText = col6;

            requirejs.config({
                baseUrl: "../../../Scripts/Newjs/amd-js/",
                paths: {
                    "jquery": "jquery-1.11.1.min",
                    "jquery-ui": "jquery-ui-1.11.0.custom.min",
                    "jquery.ui": "jquery-ui",
                    "jquery.mousewheel": "jquery.mousewheel.min",
                    "globalize": "globalize.min"
                }
            });

            function CustomCellFormatter(args) {
                if (typeof args.row.groupByValue != "undefined")
                    args.$cell.css("cursor", "pointer");
                if (typeof args.formattedValue != "undefined" && args.formattedValue != null && args.formattedValue != "") {
                    args.$cell.css("cursor", "pointer");
                }
            }

            require(["wijmo.wijgrid"], function () {
                $("#sample-grid").wijgrid({
                    allowSorting: true,
                    allowColSizing: false,
                    columnsAutogenerationMode: "none",
                    allowPaging: false,
                    data: data,
                    allowEditing: false,
                    showFooter: true,
                    cellClicked: function (e, args) {
                        var colNumber;
                        var currentURL = window.location.href;
                        var newURL = currentURL.replace("AccountantMonitoring.aspx", "MonitoringDocument.aspx");
                        if (typeof args.cell.row().groupByValue != "undefined") {
                            colNumber = args.cell._ci;
                            if (colNumber != 0) {
                                window.open(newURL + "?comCode=&colnumber=" + colNumber + "&BUName=" + args.cell.row().groupByValue, "_blank");
                            } else {
                                return;
                            }
                        }
                        else if (args.cell.column().dataKey != "CompanyCode" && typeof args.cell.row().data != 'undefined' && args.cell.value() != null && args.cell.value() != "") {
                            colNumber = args.cell._ci;
                            if (colNumber != 0) {
                                window.open(newURL + "?comCode=" + args.cell.row().data.CompanyCode + "&colnumber=" + colNumber + "&BUName=", "_blank");
                            } else {
                                return;
                            }
                        }
                    },
                    columns: [
					{
					    headerText: "BU",
					    dataKey: "BuName",
					    visible: false,
					    groupInfo: {
					        position: "header",
					        outlineMode: "startCollapsed",
					        headerText: "{0}"
					    }
					},
					{ headerText: "BU", dataKey: "CompanyCode", textAlignment: "center", footerText: "BU รวม", cellFormatter: CustomCellFormatter, ensurePxWidth: true, width: 250 },
					{ headerText: col1fromTable, dataKey: "Col1", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 },
					{ headerText: col2fromTable, dataKey: "Col2", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 },
					{ headerText: col3fromTable, dataKey: "Col3", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 },
					{ headerText: col4fromTable, dataKey: "Col4", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 },
                    { headerText: col5fromTable, dataKey: "Col5", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 },
                    { headerText: totalHeaderText, dataKey: "Total", aggregate: "sum", footerText: "{0}", cellFormatter: CustomCellFormatter, dataType: "number", dataFormatString: "n0", textAlignment: "center", ensurePxWidth: true, width: 180 }
                ]
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="A" runat="server">
    <fieldset id="ctlFieldSearch" runat="server" class="table">
        <legend id="ctlLegendSearchCriteria" style="color: #4E9DDF">
            <asp:Label ID="ctlSearchCriteriaHeader" runat="server" Text="$Criteria$" />
        </legend>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="ctlBuName" runat="server" Text="$BuName$" />
                </td>
                <td colspan="3">
                    <table>
                        <td id="Bucode" colspan="3">
                            <asp:DropDownList ID="ctlBUDropdown" runat="server" SkinID="SkGeneralDropdown" Width="252px">
                            </asp:DropDownList>
                        </td>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" class="style2">
                    <asp:Label ID="ctlCompany" runat="server" Text="$Company$" />
                </td>
                <td colspan="3">
                    <uc4:CompanyField ID="ctlCompanyField" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="cltFromDate" runat="server" Text="$FromDate$" />
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <uc1:calendar ID="ctlRequestDateFrom" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="cltToDate" runat="server" Text="$ToDate$" />
                            </td>
                            <td>
                                <uc1:calendar ID="ctlRequestDateTo" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:ImageButton runat="server" ID="ctlSearch" ToolTip="Search" SkinID="SkSearchButton"
                        OnClick="ctlSearch_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <div align="left">
        <ajaxToolkit:TabContainer ID="ctlTabContainer" runat="server" ActiveTabIndex="0">
            <ajaxToolkit:TabPanel runat="server" ID="tapDataPanel" HeaderText="Data">
                <HeaderTemplate>
                    <asp:Label ID="tabData" SkinID="SkFieldCaptionLabel" runat="server" Text="$Data$"></asp:Label>
                </HeaderTemplate>
                <ContentTemplate>
                    <center>
                        <table id="sample-grid" style="width: 100%">
                        </table>
                    </center>
                    <asp:Timer ID="ctlTimer" runat="server" Enabled="False" OnTick="time_Tick">
                    </asp:Timer>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" ID="ctlGraph" HeaderText="Graph">
                <HeaderTemplate>
                    <asp:Label ID="tapGraph" SkinID="SkFieldCaptionLabel" runat="server" Text="$Graph$"></asp:Label></HeaderTemplate>
                <ContentTemplate>
                    <div>
                        <uc2:ReportViewers ID="AccountantMonitoringInboxGraph_Viewer" runat="server" ReportName="AccountantMonitoringInboxGraph"
                            ReportHeight="450" SetLayoutHeightType="Pixel" ReportWidth="95" SetLayoutWidthType="Percentage"
                            DocumentMapCollapsed="true" HideParameterOnForm="true" />
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
</asp:Content>
