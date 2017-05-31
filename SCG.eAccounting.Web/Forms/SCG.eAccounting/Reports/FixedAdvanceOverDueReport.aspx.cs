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
    public partial class FixedAdvanceOverDueReport : BasePage
    {
        public IList<VOFixedAdvanceOverDueReport> list;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlFixedAdvanceReportGrid.DataCountAndBind();
                ctlUpdatePanelFixedAdvanceDocument.Update();
                ctlUpdatePanelCriteria.Update();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOFixedAdvanceOverDueReport vo = ctlFixedAdvanceOverDueCriteria.BindCriteria();
            vo.LanguageID = UserAccount.CurrentLanguageID;
            list = ScgeAccountingQueryProvider.SCGDocumentQuery.GetFixedAdvanceDocumentReportList(startRow, pageSize, sortExpression, vo);
            return list;
        }

        public int RequestCount()
        {
            VOFixedAdvanceOverDueReport vo = ctlFixedAdvanceOverDueCriteria.BindCriteria();
            vo.LanguageID = UserAccount.CurrentLanguageID;
            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByFixedAdvanceReportCriteria(vo);
            return count;
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ctlFixedAdvanceReportGrid.ClearSortExpression = true;
                ctlFixedAdvanceReportGrid.DataCountAndBind();
                ctlUpdatePanelFixedAdvanceDocument.Update();
                showException.Text = "";
                //ctlUpdatePanelCriteria.Update();
            }
            catch (FormatException ex)
            {
                showException.Text = GetMessage("Invalid_Date_Format");
            }

        }
        protected void ctlFixedAdvanceReportGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime dNow = DateTime.Now.Date;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlNo = e.Row.FindControl("ctlNoText") as Label;
                //Label ctlRequestDateOfRemittance = e.Row.FindControl("ctlRequestDateOfRemittance") as Label;
                //abel ctlOverDueDay = e.Row.FindControl("ctlOverDueDay") as Label;
                ctlNo.Text = ((ctlFixedAdvanceReportGrid.PageSize * ctlFixedAdvanceReportGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }
        protected void ctlFixedAdvanceReportGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long requesterID = UIHelper.ParseLong(ctlFixedAdvanceReportGrid.DataKeys[rowIndex].Values["RequesterID"].ToString());
                long documentID = UIHelper.ParseLong(ctlFixedAdvanceReportGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());
                long advanceID = UIHelper.ParseLong(ctlFixedAdvanceReportGrid.DataKeys[rowIndex].Values["FixedAdvanceID"].ToString());
                string requestNo = ctlFixedAdvanceReportGrid.DataKeys[rowIndex].Values["DocumentNo"].ToString();
                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
                if (e.CommandName == "ClickEmail")
                {
                    ctlSendFollowUpEmail.CreatorID = doc == null ? 0 : doc.CreatorID.Userid;
                    ctlSendFollowUpEmail.RequesterID = requesterID;
                    ctlSendFollowUpEmail.DocumentID = documentID;
                    ctlSendFollowUpEmail.AdvanceDocumentID = advanceID;
                    ctlSendFollowUpEmail.RequestNo = requestNo;
                    ctlSendFollowUpEmail.EmailType = EmailType.EM15.ToString();
                    ctlSendFollowUpEmail.Show();
                }
                else if (e.CommandName == "ClickSendtime")
                {
                    ctlSendFollowUpEmail.DocumentID = documentID;
                    FixedAdvanceDocument scgDocument = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(documentID);
                    if (scgDocument != null)
                    {
                        string documentNo = scgDocument.DocumentID.DocumentNo;
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Reports/EmailLogView.aspx?isDuplication=" + Boolean.FalseString + "&EmailType=EM10&RequestNo=" + documentNo + "')", true);
                        ctlEmailLog.RequestNo = documentNo;
                        ctlEmailLog.EmailType = "EM15";
                        ctlEmailLog.isDisplayCriteria = false;
                        ctlEmailLog.Show();
                        ctlModalPopupExtender.Show();
                    }

                }
                else if (e.CommandName == "ClickFixedAdvanceNo")
                {
                    WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
                    if (wf != null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')", true);
                    }

                }
                //else if (e.CommandName == "ClickRefFixedAdvanceNo")
                //{
                //    WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
                //    if (wf != null)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')", true);
                //    }
                //}
            }
        }
        protected void ctlFixedAdvanceReportGrid_DataBound(object sender, EventArgs e)
        {
        }
    }
}