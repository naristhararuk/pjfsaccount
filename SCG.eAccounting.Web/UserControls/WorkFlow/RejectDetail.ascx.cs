using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class RejectDetail : SS.Standard.UI.BaseUserControl, IEventControl
    {
        public int WorkFlowStateEventID {
            get { return UIHelper.ParseInt(ViewState["WorkFlowStateEventID"].ToString()); }
            set { ViewState["WorkFlowStateEventID"] = value; }
        }

        #region Properties
        /// <summary>
        /// Get or Set show checkBox Approve Rejection
        /// </summary>
        public virtual bool isApprove { get; set; }

        /// <summary>
        /// Get or Set for choose button type
        /// reject is used for Verify Detail
        /// decline is used for Verify Detail Posting
        /// notReceive  is used for Reject Detail
        /// </summary>
        public virtual string LableDetail { get; set; }

        public int DocumentTypeID
        {
            get { return UIHelper.ParseInt(ViewState["DocumentTypeID"].ToString()); }
            set { ViewState["DocumentTypeID"] = value; }
        }

        public int WorkflowStateID
        {
            get { return UIHelper.ParseInt(ViewState["WorkflowStateID"].ToString()); }
            set { ViewState["WorkflowStateID"] = value; }
        }
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                //set show or not checkBox Approve
                //if (isApprove)
                //    ctlFieldSetCheckBox.Visible = true;
                //else
                //    ctlFieldSetCheckBox.Visible = false;

                //Set show detail label
                //if (LableDetail != null && !LableDetail.Equals(string.Empty))
                //{
                //    if (LableDetail.Equals("reject"))
                //    {
                //        ctlHeader.Text = "Reject Detail :";
                //    }
                //    else if (LableDetail.Equals("decline"))
                //    {
                //        ctlHeader.Text = "Decline Detail :";
                //    }
                //    else if (LableDetail.Equals("v"))
                //    {
                //        ctlHeader.Text = "Not Receive Detail :";
                //    }
                //}
            //}
        }
        #endregion

        public void Initialize(long workFlowID)
        {
             Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
             SS.Standard.WorkFlow.DTO.WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
             if (document != null && document.DocumentType != null)//Advance DocumentType when 1 is Domestic when 5 is Foreign
             {
                 this.DocumentTypeID = document.DocumentType.DocumentTypeID;
                 this.WorkflowStateID = wf.CurrentState.WorkFlowStateID;
                 this.BindReasonDropdown();
             }
        }

        #region IEventControl Members

        public object GetEventData(int workFlowStateEventID)
        {
            RejectDetailResponse returnObj = new RejectDetailResponse();
            returnObj.WorkFlowStateEventID = workFlowStateEventID;
            returnObj.ReasonID = UIHelper.ParseShort(ctlReasonDdl.SelectedValue);
            if (returnObj.ReasonID != 0)
            {
                DbRejectReason reason = ScgDbQueryProvider.DbRejectReasonQuery.FindProxyByIdentity(returnObj.ReasonID);
                returnObj.NeedApproveRejection = reason.RequireConfirmReject;
            }
            returnObj.Remark = ctlRemark.Text;
            return returnObj;
        }
        public void BindReasonDropdown()
        {
            ctlReasonDdl.DataSource = ScgDbQueryProvider.DbRejectReasonQuery.FindRejectReasonByDocTypeIDStateEventIDAndLanguageID(this.DocumentTypeID, this.WorkFlowStateEventID, UserAccount.CurrentLanguageID);
            ctlReasonDdl.DataTextField = "ReasonFormat";
            ctlReasonDdl.DataValueField = "ReasonID";
            ctlReasonDdl.DataBind();
        }
        #endregion
    }
}