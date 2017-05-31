using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.Forms.SCG.Log.Programs
{
    public partial class SMSLog : BasePage
    {
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "SCLGRT06";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlSmsLogGrid.DataCountAndBind();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //string dateFrom = applyDateFormat(ctlFromDate.Text);
            //string dateTo = applyDateFormat(ctlToDate.Text);

            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            if (!ctlFromDate.Text.Equals(string.Empty))
            {
                dateFrom = (DateTime)UIHelper.ParseDate(ctlFromDate.DateValue);
            }
            if (!ctlToDate.Text.Equals(string.Empty))
            {
                dateTo = (DateTime)UIHelper.ParseDate(ctlToDate.DateValue);
            }

            return QueryProvider.SuSmsLogQuery.GetSmsLogList(dateFrom, dateTo, ctlPhoneNo.Text, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            //string dateFrom = applyDateFormat(ctlFromDate.Text);
            //string dateTo = applyDateFormat(ctlToDate.Text);

            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            if (!ctlFromDate.Text.Equals(string.Empty))
            {
                dateFrom = (DateTime)UIHelper.ParseDate(ctlFromDate.DateValue);
            }
            if (!ctlToDate.Text.Equals(string.Empty))
            {
                dateTo = (DateTime)UIHelper.ParseDate(ctlToDate.DateValue);
            }

            return QueryProvider.SuSmsLogQuery.GetCountSmsLogList(dateFrom, dateTo, ctlPhoneNo.Text);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlSmsLogGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        public string applyDateFormat(string inputDate)
        {
            if (!inputDate.Equals(string.Empty))
            {
                DateTime date = DateTime.Parse(inputDate);
                return date.ToString("MM/dd/yyyy hh:mm:ss.ff tt");
            }
            else
                return string.Empty;
        }
    }
}
