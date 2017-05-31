using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;

//using SCG.FN.DTO;
//using SCG.FN.Query;

using SS.DB.DTO;
using SS.DB.Query;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.WorkFlow.DTO;
using SS.SU.Query;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class AdvanceOverDueReport : BasePage
    {
        public IList<VOAdvanceOverDueReport> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlAdvanceReportGrid.DataCountAndBind();
                ctlUpdatePanelAdvanceDocument.Update();
                ctlUpdatePanelCriteria.Update();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOAdvanceOverDueReport vo = ctlAdvanceOverDueCriteria.BindCriteria();
            vo.LanguageID = UserAccount.CurrentLanguageID;
            list = ScgeAccountingQueryProvider.SCGDocumentQuery.GetAdvanceDocumentReportList(startRow, pageSize, sortExpression, vo);
            return list;
        }

        public int RequestCount()
        {
            VOAdvanceOverDueReport vo = ctlAdvanceOverDueCriteria.BindCriteria();
            vo.LanguageID = UserAccount.CurrentLanguageID;
            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByAdvanceReportCriteria(vo);
            return count;
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ctlAdvanceReportGrid.ClearSortExpression = true;
                ctlAdvanceReportGrid.DataCountAndBind();
                ctlUpdatePanelAdvanceDocument.Update();
                showException.Text = "";
                //ctlUpdatePanelCriteria.Update();
            }
            catch (FormatException ex)
            {
                showException.Text = GetMessage("Invalid_Date_Format");
            }

        }
        protected void ctlAdvanceReportGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime dNow = DateTime.Now.Date;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlNo = e.Row.FindControl("ctlNoText") as Label;
                Label ctlRequestDateOfRemittance = e.Row.FindControl("ctlRequestDateOfRemittance") as Label;
                Label ctlOverDueDay = e.Row.FindControl("ctlOverDueDay") as Label;
                ctlNo.Text = ((ctlAdvanceReportGrid.PageSize * ctlAdvanceReportGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
                
                //if (!string.IsNullOrEmpty(ctlRequestDateOfRemittance.Text))
                //{
                //    DateTime dRequestDateOfRemittance = UIHelper.ParseDate(ctlRequestDateOfRemittance.Text).GetValueOrDefault(DateTime.MinValue);
                //    TimeSpan timeValue = dNow - dRequestDateOfRemittance;
                //    int countDays = timeValue.Days;
                //    ctlOverDueDay.Text = countDays.ToString();
                //}
            }
        }
        protected void ctlAdvanceReportGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long requesterID = UIHelper.ParseLong(ctlAdvanceReportGrid.DataKeys[rowIndex].Values["RequesterID"].ToString());
                long documentID = UIHelper.ParseLong(ctlAdvanceReportGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());
                long advanceID = UIHelper.ParseLong(ctlAdvanceReportGrid.DataKeys[rowIndex].Values["AdvanceID"].ToString());
                long expenseID = UIHelper.ParseLong(ctlAdvanceReportGrid.DataKeys[rowIndex].Values["ExpenseID"].ToString());
                string requestNo = ctlAdvanceReportGrid.DataKeys[rowIndex].Values["DocumentNo"].ToString();
                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
                if (e.CommandName == "ClickEmail")
                {
                    ctlSendFollowUpEmail.CreatorID = doc == null ? 0 : doc.CreatorID.Userid;
                    ctlSendFollowUpEmail.RequesterID = requesterID;
                    ctlSendFollowUpEmail.DocumentID = documentID;
                    ctlSendFollowUpEmail.AdvanceDocumentID = advanceID;
                    ctlSendFollowUpEmail.RequestNo = requestNo;
                    ctlSendFollowUpEmail.EmailType = EmailType.EM10.ToString();
                    ctlSendFollowUpEmail.Show();
                }
                else if (e.CommandName == "ClickSendtime")
                {
                    ctlSendFollowUpEmail.DocumentID = documentID;
                    AvAdvanceDocument scgDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(documentID);
                    if (scgDocument != null)
                    {
                        string documentNo = scgDocument.DocumentID.DocumentNo;
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Reports/EmailLogView.aspx?isDuplication=" + Boolean.FalseString + "&EmailType=EM10&RequestNo=" + documentNo + "')", true);
                        ctlEmailLog.RequestNo = documentNo;
                        ctlEmailLog.EmailType = "EM10";
                        ctlEmailLog.isDisplayCriteria = false;
                        ctlEmailLog.Show();
                        ctlModalPopupExtender.Show();
                    }
                    
                }
                else if (e.CommandName == "ClickAdvanceNo")
                {
                    WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
                    if (wf != null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')", true);
                    }

                }
                else if (e.CommandName == "ClickExpenseNo")
                {
                    WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(expenseID);
                    if (wf != null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')", true);
                    }
                }
            }
        }
        protected void ctlAdvanceReportGrid_DataBound(object sender, EventArgs e)
        {
            //VOAdvanceOverDueReport vo = ctlAdvanceOverDueCriteria.BindCriteria();
            //int FromOverDue = vo.FromOverDue;
            //int ToOverDue = vo.ToOverDue;
            //DataRow[] dr;
            //foreach (VOAdvanceOverDueReport item in list)
            //{
               
            //}
            //DataRow[] dr = ctl
            //foreach (GridViewRow row in ctlAdvanceReportGrid.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        Label ctlOverDueDay = row.FindControl("ctlOverDueDay") as Label;
            //    }
            //}        
        }
    }
}
