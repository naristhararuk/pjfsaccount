using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class EmailLog : BaseUserControl
    {
        public bool isDisplayCriteria { get; set; }
        public string RequestNo
        {
            get { return ctlRequestNo.Text; }
            set { ctlRequestNo.Text = value; }
        }
        public string EmailType 
        {
            get { return ctlEmailTypeLabel.Text; }
            set { ctlEmailTypeLabel.Text = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!isDisplayCriteria)
                {
                    ctlUpdatePanelEmailLog.Visible = false;
                    ctlEmailLogGrid.DataCountAndBind();
                    ctlUpdatePanelEmailGrid.Update();
                }
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            if (!isDisplayCriteria)
            {
                return ScgeAccountingQueryProvider.EmailLogQuery.GetLogList(ctlEmailTypeLabel.Text, ctlDate.DateValue, ctlRequestNo.Text, UIHelper.ParseInt(ctlStatus.SelectedValue), startRow, pageSize, sortExpression, UserAccount.CurrentLanguageID);
            }
            else
            {
                return ScgeAccountingQueryProvider.EmailLogQuery.GetLogList(ctlEmailType.SelectedValue, ctlDate.DateValue, ctlRequestNo.Text, UIHelper.ParseInt(ctlStatus.SelectedValue), startRow, pageSize, sortExpression,UserAccount.CurrentLanguageID);
            }
        }

        public int RequestCount()
        {
            if (!isDisplayCriteria)
            {
                return ScgeAccountingQueryProvider.EmailLogQuery.CountByLogCriteria(ctlEmailTypeLabel.Text, ctlDate.DateValue, ctlRequestNo.Text, UIHelper.ParseInt(ctlStatus.SelectedValue), UserAccount.CurrentLanguageID);
            }
            else
            {
                return ScgeAccountingQueryProvider.EmailLogQuery.CountByLogCriteria(ctlEmailType.SelectedValue, ctlDate.DateValue, ctlRequestNo.Text, UIHelper.ParseInt(ctlStatus.SelectedValue), UserAccount.CurrentLanguageID);
            }
           
        }

        protected void ctlEmailLogGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlTo = e.Row.FindControl("ctlTo") as Label;
                Label ctlCC = e.Row.FindControl("ctlCC") as Label;
                if (ctlTo != null)
                {
                    ctlTo.Text = ctlTo.Text.Replace(";", "; ");
                }
                if (ctlCC != null)
                {
                    ctlCC.Text = ctlCC.Text.Replace(";", "; ");
                }
                //ctlNo.Text = ((ctlEmailLogGrid.PageSize * ctlDocumentGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlEmailLogGrid.DataCountAndBind();
            ctlUpdatePanelEmailGrid.Update();
        }
        public void Show()
        {
            ctlEmailLogGrid.DataCountAndBind();
            ctlUpdatePanelEmailGrid.Update();
        }
        
    }
}