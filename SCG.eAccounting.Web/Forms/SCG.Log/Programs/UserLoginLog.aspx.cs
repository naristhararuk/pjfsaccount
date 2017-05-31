using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SS.SU.Query;
using SS.SU.DTO;

using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.Forms.SCG.Log.Programs
{
    public partial class UserLoginLog : BasePage

    {
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "SCLGRT07";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlUserLoginLogGrid.DataCountAndBind();

            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DateTime? dateFrom = UIHelper.ParseDate(ctlFromDate.DateValue);
            DateTime? dateTo = UIHelper.ParseDate(ctlToDate.DateValue);
            //long userID = UIHelper.ParseLong(ctlUserID.Text);
            //string dateFrom = applyDateFormat(ctlFromDate.Text);
            //string dateTo = applyDateFormat(ctlToDate.Text);

            return QueryProvider.SuUserLogQuery.FindSuUserLoginLogListQuery(dateFrom, dateTo, ctlUserName.Text, ctlStatus.Text, startRow, pageSize, sortExpression);
        }

        private string applyDateFormat(string inputdate)
        {
            if (!inputdate.Equals(string.Empty))
            {
                DateTime date = DateTime.Parse(inputdate);
                return date.ToString("MM/dd/yyyy");
            }
            else
                return string.Empty;
        }


        protected int RequestCount()
        {
            DateTime? dateFrom = UIHelper.ParseDate(ctlFromDate.DateValue);
            DateTime? dateTo = UIHelper.ParseDate(ctlToDate.DateValue);
            //long userID = UIHelper.ParseLong(ctlUserID.Text);
            //string dateFrom = applyDateFormat(ctlFromDate.Text);
            //string dateTo = applyDateFormat(ctlToDate.Text);

            return QueryProvider.SuUserLogQuery.GetCountUserLoginLoglist(dateFrom, dateTo, ctlUserName.Text, ctlStatus.Text);
            
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserLoginLogGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

    }
}
