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
    public partial class EHRExpenseLog : BasePage
    {

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "SCLGRT02";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              ctlEHRExpenseLogGrid.DataCountAndBind();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
         
            SueHrExpenseLog query = new SueHrExpenseLog();
            query.Status = ctlStatus.SelectedValue ;
            query.ExpenseRequestNo = ctlExpenseRequestNo.Text;
            try
            {
                query.LastDate = (DateTime)UIHelper.ParseDate(ctlDate.DateValue);
                
            }
            catch (Exception)
            {

                query.LastDate = new DateTime(); 
            }

           

            return QueryProvider.SuEHRExpenseLogQuery.GeteHrExpenseLogList(query, startRow, pageSize, sortExpression);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlEHRExpenseLogGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        protected int RequestCount()
    {
        SueHrExpenseLog query = new SueHrExpenseLog();
        query.Status = ctlStatus.SelectedValue;
        query.ExpenseRequestNo = ctlExpenseRequestNo.Text;
        try
        {
            query.LastDate = (DateTime)UIHelper.ParseDate(ctlDate.DateValue);
            
        }
        catch (Exception)
        {
            query.LastDate = new DateTime();
          
        }
       
        return QueryProvider.SuEHRExpenseLogQuery.CountEHRExpenseLogByCriteria(query);
    }

       
    }
}
