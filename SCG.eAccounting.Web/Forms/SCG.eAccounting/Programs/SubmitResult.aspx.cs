using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.SU.Helper;

using System.Text;
using System.IO;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;


namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class SubmitResult : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlSubmitResult.Text = "Your request was submitted successfully.";
            //ctlLinkToSubmittedDocument.Text = "View submitted document.";

            string wfid = Request.QueryString["wfid"].ToString();
            WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(long.Parse(wfid));

            //ctlLinkToSubmittedDocument.Text = MessageSource.GetMessage("ViewSubmittedDocument") +" "+ workFlow.Document.DocumentNo ;
            //ctlLinkToSubmittedDocument.NavigateUrl = "DocumentView.aspx?wfid=" + Request.Params["wfid"];
            //ctlSubmitResult.Text = MessageSource.GetMessage("submittedSuccessfully");
            /*
            ctlSubmitResult.Text = "Request - ["+workFlow.Document.DocumentNo+ "] has been submitted successfully.<br/> Click ";
            ctlLinkToSubmittedDocument.Text = "here";
            ctlLinkToSubmittedDocument.NavigateUrl = "DocumentView.aspx?wfid=" + Request.Params["wfid"];
            ctlViewReq.Text = " to view the request.";

            divMessage.Style["display"] = "block";
            //AlertMessage1.Symbol = "SaveSuccessFully";
            AlertMessage1.MsgType = MessageTypeEnum.AlertMessage.Information;

            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            ctlSubmitResult.RenderControl(hw);
            ctlLinkToSubmittedDocument.RenderControl(hw);
            ctlViewReq.RenderControl(hw);
            
            AlertMessage1.Symbol = sb.ToString();

            //AlertMessage1.Controls.Add(ctlLinkToSubmittedDocument);
            AlertMessage1.Show();
            */

            ctlSubmitResult.Text = "Request - [" + workFlow.Document.DocumentNo + "] has been submitted successfully.<br/> ";
            ctlLinkToSubmittedDocument.Text = "here";
            ctlLinkToSubmittedDocument.NavigateUrl = "DocumentView.aspx?wfid=" + Request.Params["wfid"];
            ctlViewReq.Text = "to view the request.";
        }
    }
}
