using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.SU.DAL;
using SS.SU.Query;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.Forms.SCG.Log.Programs
{
    public partial class EHRProfileLog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlEHRProfileLogGrid.DataCountAndBind();
                ctlUpdPanelGridView.Update();
                
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return QueryProvider.SuEHRProfileLogQuery.GeteHrProfileLogList(startRow, pageSize, sortExpression);
        }

        public int RequestCount()
        {
            return QueryProvider.SuEHRProfileLogQuery.CountEHRProfileLogByCriteria();
        }
    }
}
