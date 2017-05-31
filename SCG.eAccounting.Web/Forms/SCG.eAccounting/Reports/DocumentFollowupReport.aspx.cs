using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
	public partial class DocumentFollowupReport : BasePage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ctlDocumentFollowupCriteria.DefaultButton = ctlSearch.ClientID;
                panelControl.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + ctlSearch.UniqueID + "','')");
                ctlUpdatePanelDocument.Update();
                ctlUpdatePanelCriteria.Update();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            //return ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentReportList(startRow, pageSize, sortExpression, ctlDocumentFollowupCriteria.isPosting, UserAccount.CurrentLanguageID, ctlDocumentFollowupCriteria.companyID, ctlDocumentFollowupCriteria.LocationFrom, ctlDocumentFollowupCriteria.LocationTo, ctlDocumentFollowupCriteria.DateFrom, ctlDocumentFollowupCriteria.DateTo, ctlDocumentFollowupCriteria.createrID, ctlDocumentFollowupCriteria.requesterID, ctlDocumentFollowupCriteria.CostCenterID, ctlDocumentFollowupCriteria.ServiceTeamID, ctlDocumentFollowupCriteria.statusValue);
            return ScgeAccountingQueryProvider.SCGDocumentQuery.GetDocumentReportList(startRow, pageSize, sortExpression, ctlDocumentFollowupCriteria.isPosting, UserAccount.CurrentLanguageID, ctlDocumentFollowupCriteria.companyID, ctlDocumentFollowupCriteria.LocationFrom, ctlDocumentFollowupCriteria.LocationTo, ctlDocumentFollowupCriteria.DateFrom, ctlDocumentFollowupCriteria.DateTo, ctlDocumentFollowupCriteria.createrID, ctlDocumentFollowupCriteria.requesterID, ctlDocumentFollowupCriteria.CostCenterID, ctlDocumentFollowupCriteria.ServiceTeamID, 1);
        }

        public int RequestCount()
        {
            //int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByDocumentReportCriteria(ctlDocumentFollowupCriteria.isPosting, UserAccount.CurrentLanguageID, ctlDocumentFollowupCriteria.companyID, ctlDocumentFollowupCriteria.LocationFrom, ctlDocumentFollowupCriteria.LocationTo, ctlDocumentFollowupCriteria.DateFrom, ctlDocumentFollowupCriteria.DateTo, ctlDocumentFollowupCriteria.createrID, ctlDocumentFollowupCriteria.requesterID, ctlDocumentFollowupCriteria.CostCenterID, ctlDocumentFollowupCriteria.ServiceTeamID, ctlDocumentFollowupCriteria.statusValue);
            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountByDocumentReportCriteria(ctlDocumentFollowupCriteria.isPosting, UserAccount.CurrentLanguageID, ctlDocumentFollowupCriteria.companyID, ctlDocumentFollowupCriteria.LocationFrom, ctlDocumentFollowupCriteria.LocationTo, ctlDocumentFollowupCriteria.DateFrom, ctlDocumentFollowupCriteria.DateTo, ctlDocumentFollowupCriteria.createrID, ctlDocumentFollowupCriteria.requesterID, ctlDocumentFollowupCriteria.CostCenterID, ctlDocumentFollowupCriteria.ServiceTeamID, 1);
            return count;
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlDocumentGrid.DataCountAndBind();
            ctlUpdatePanelDocument.Update();
            //ctlUpdatePanelCriteria.Update();
            
        }
        protected void ctlDocumentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlNo = e.Row.FindControl("ctlNoText") as Label;
                ctlNo.Text = ((ctlDocumentGrid.PageSize * ctlDocumentGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }
        protected void ctlDocumentGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ClickEmail")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long requesterID = UIHelper.ParseLong(ctlDocumentGrid.DataKeys[rowIndex].Values["RequesterID"].ToString());
                long documentID = UIHelper.ParseLong(ctlDocumentGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());
                long creatorID = UIHelper.ParseLong(ctlDocumentGrid.DataKeys[rowIndex].Values["CreatorID"].ToString());
                ctlSendFollowUpEmail1.RequesterID = requesterID;
                ctlSendFollowUpEmail1.CreatorID = creatorID;
                ctlSendFollowUpEmail1.DocumentID = documentID;
                ctlSendFollowUpEmail1.Show();
               
            }
            else if (e.CommandName == "ClickSendtime")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long documentID = UIHelper.ParseLong(ctlDocumentGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());
                ctlSendFollowUpEmail1.DocumentID = documentID;
                SCGDocument scgDocument = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(documentID);
                if (scgDocument != null)
                {
                    string documentNo = scgDocument.DocumentNo;
                    ctlEmailLog.RequestNo = documentNo;
                    ctlEmailLog.EmailType = "EM09";
                    ctlEmailLog.isDisplayCriteria = false;
                    ctlEmailLog.Show();
                    ctlModalPopupExtender.Show();
                }
            }
            else if (e.CommandName == "ClickAdvanceNo")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long documentID = UIHelper.ParseLong(ctlDocumentGrid.DataKeys[rowIndex].Values["DocumentID"].ToString());

                WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
                if (wf != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, "window.open('../Programs/DocumentView.aspx?wfid=" + wf.WorkFlowID.ToString() + "')", true);
                }

            }
        }
    }
}
