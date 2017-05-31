using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class AdvanceImportLogViewer : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             
        }
        protected int RequestCount()
        {
            return ScgeAccountingQueryProvider.FnEACAdvanceImportLogQuery.GetCountFnEACAdvanceImportLoglist(this.BuildCriteria());

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgeAccountingQueryProvider.FnEACAdvanceImportLogQuery.FindFnEACAdvanceImportLogListQuery(this.BuildCriteria(), startRow, pageSize, sortExpression);
        }
        public FnEACAdvanceImportLog BuildCriteria()
        {
            FnEACAdvanceImportLog fnEACAdvanceImportLog = new FnEACAdvanceImportLog();

            fnEACAdvanceImportLog.EACRequestNo = ctlEaccountRequestID.Text;
            fnEACAdvanceImportLog.EXPRequestNo = ctleExpenseRequestID.Text;
            fnEACAdvanceImportLog.Message = ctlMessage.Text;
            fnEACAdvanceImportLog.Status = ctlStatus.SelectedValue;

            return fnEACAdvanceImportLog;
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlAdvanceImportLogViewerGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }
    }
}
