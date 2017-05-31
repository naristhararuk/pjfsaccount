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
    public partial class InterfaceImageToSAPLog : BasePage
    {
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "SCLGRT04";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlImageToSAPLogGrid.DataCountAndBind();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //string date = applyDateFormat(ctlSubmitDate.Text);
            DateTime date = new DateTime();
            if (!ctlSubmitDate.Text.Equals(string.Empty))
            {
                date = (DateTime)UIHelper.ParseDate(ctlSubmitDate.DateValue);
            }
            return QueryProvider.SuImageToSAPLogQuery.GetImageToSAPLogList(ctlRequestNo.Text, date, ctlStatus.Text, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            //string date = applyDateFormat(ctlSubmitDate.Text);
            DateTime date = new DateTime();
            if (!ctlSubmitDate.Text.Equals(string.Empty))
            {
                date = (DateTime)UIHelper.ParseDate(ctlSubmitDate.DateValue);
            }
            return QueryProvider.SuImageToSAPLogQuery.GetCountImageToSAPLogList(ctlRequestNo.Text, date, ctlStatus.Text);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlImageToSAPLogGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        public string applyDateFormat(string inputDate)
        {
            if (!inputDate.Equals(string.Empty))
            {
                DateTime date = DateTime.Parse(inputDate);
                return date.ToString("MM/dd/yyyy");
            }
            else
                return string.Empty;
        }
    }
}
