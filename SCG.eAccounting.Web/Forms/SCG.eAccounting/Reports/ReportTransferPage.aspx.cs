using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class ReportTransferPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["dId"] != null)
            {
                long documentID = UIHelper.ParseLong(Request["dId"].ToString());
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(documentID);

                if (Request["payin"] == null)
                {
                    ReportHelper.FlushReport(this.Page, new SCGDocumentService().GeneratePDF(documentID), ReportHelper.ReportType.PDF, document.DocumentNo);
                }
                else
                {
                    if (Request["payin"].ToString().Equals("true"))
                    {
                        ReportHelper.FlushReport(this.Page, new SCGDocumentService().GeneratePayIn(documentID), ReportHelper.ReportType.PDF, document.DocumentNo);
                    }
                }
            }
        }
    }
}
