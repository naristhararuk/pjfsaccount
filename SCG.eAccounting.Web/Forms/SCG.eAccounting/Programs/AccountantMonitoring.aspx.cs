using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using System.Data;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using System.Web.Script.Serialization;
using SS.DB.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class AccountantMonitoring : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlTimer.Interval = ParameterServices.RefreshInterval;
                SetBuDropDown();
                AccountantMonitoringInboxGraph_Viewer.Visible = false;
            }

            PreviewTable();
            PreviewGraph();
        }

        public void SetBuDropDown()
        {
            ctlBUDropdown.DataSource = ScgDbQueryProvider.DbBuQuery.FindBUALL();
            ctlBUDropdown.DataTextField = "BuName";
            ctlBUDropdown.DataValueField = "BuCode";
            ctlBUDropdown.DataBind();
            ctlBUDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
        }

        protected object ctlMonitoringInBoxGrid_RequestData()
        {
            MonitoringInBoxSearchCriteria Criteria = new MonitoringInBoxSearchCriteria();

            Criteria.Company = ctlCompanyField.CompanyCode;
            Criteria.BusinessGroup = ctlBUDropdown.SelectedValue;
            Criteria.FromDate = ctlRequestDateFrom.Value;
            Criteria.ToDate = ctlRequestDateTo.Value;

            IList<DocumentMonitoringInbox> Jlis = ScgeAccountingQueryProvider.DbMonitoringInboxQuery.DataMonitoringInBox(Criteria);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Jlis);

            return json;
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            PreviewGraph();
            PreviewTable();
        }

        protected void time_Tick(object sender, EventArgs e)
        {
            ctlSearch_Click(this, null);
        }

        public void PreviewGraph()
        {
            List<ReportParameters> rptParam = new List<ReportParameters>();

            string col1 = string.Format("{0}-{1} {2}", ParameterServices.Column1_OverdueDayFrom, ParameterServices.Column1_OverdueDayTo, this.GetProgramMessage("Day"));
            string col2 = string.Format("{0}-{1} {2}", ParameterServices.Column2_OverdueDayFrom, ParameterServices.Column2_OverdueDayTo, this.GetProgramMessage("Day"));
            string col3 = string.Format("{0}-{1} {2}", ParameterServices.Column3_OverdueDayFrom, ParameterServices.Column3_OverdueDayTo, this.GetProgramMessage("Day"));
            string col4 = string.Format("{0}-{1} {2}", ParameterServices.Column4_OverdueDayFrom, ParameterServices.Column4_OverdueDayTo, this.GetProgramMessage("Day"));
            string col5 = string.Format("{0} {1} {2}", this.GetProgramMessage("Morethan"), ParameterServices.Column4_OverdueDayTo, this.GetProgramMessage("Day"));

            rptParam.Add(new ReportParameters() { Name = "Column1Header", Value = col1 });
            rptParam.Add(new ReportParameters() { Name = "Column2Header", Value = col2 });
            rptParam.Add(new ReportParameters() { Name = "Column3Header", Value = col3 });
            rptParam.Add(new ReportParameters() { Name = "Column4Header", Value = col4 });
            rptParam.Add(new ReportParameters() { Name = "Column5Header", Value = col5 });

            rptParam.Add(new ReportParameters() { Name = "CompanyCode", Value = ctlCompanyField.CompanyCode });
            rptParam.Add(new ReportParameters() { Name = "BuCode", Value = ctlBUDropdown.SelectedValue });
            rptParam.Add(new ReportParameters() { Name = "FromDate", Value = ctlRequestDateFrom.Value.HasValue ? ctlRequestDateFrom.Value.ToString() : string.Empty });
            rptParam.Add(new ReportParameters() { Name = "ToDate", Value = ctlRequestDateFrom.Value.HasValue ? ctlRequestDateTo.Value.ToString() : string.Empty });

            AccountantMonitoringInboxGraph_Viewer.InitializeReport();
            AccountantMonitoringInboxGraph_Viewer.Parameters = rptParam;
            AccountantMonitoringInboxGraph_Viewer.Visible = true;
            AccountantMonitoringInboxGraph_Viewer.ShowReport();
        }

        public void PreviewTable() 
        {
            var json = ctlMonitoringInBoxGrid_RequestData();

            string col1 = string.Format("{0}-{1} {2}", ParameterServices.Column1_OverdueDayFrom, ParameterServices.Column1_OverdueDayTo, this.GetProgramMessage("Day"));
            string col2 = string.Format("{0}-{1} {2}", ParameterServices.Column2_OverdueDayFrom, ParameterServices.Column2_OverdueDayTo, this.GetProgramMessage("Day"));
            string col3 = string.Format("{0}-{1} {2}", ParameterServices.Column3_OverdueDayFrom, ParameterServices.Column3_OverdueDayTo, this.GetProgramMessage("Day"));
            string col4 = string.Format("{0}-{1} {2}", ParameterServices.Column4_OverdueDayFrom, ParameterServices.Column4_OverdueDayTo, this.GetProgramMessage("Day"));
            string col5 = string.Format("{0} {1} {2}", this.GetProgramMessage("Morethan"), ParameterServices.Column4_OverdueDayTo, this.GetProgramMessage("Day"));
            string col6 = this.GetProgramMessage("Total");

            string scriptFormat = string.Format("CreateDataTable({0},'{1}','{2}','{3}','{4}','{5}','{6}')", json, col1, col2, col3, col4, col5, col6);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Testscript", scriptFormat, true);
        }
    }
}