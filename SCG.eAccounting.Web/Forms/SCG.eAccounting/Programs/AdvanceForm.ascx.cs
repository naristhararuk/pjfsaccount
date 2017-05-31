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
using System.Text;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting
{
    public partial class AdvanceForm : BaseUserControl
    {
        public IWorkFlowService WorkFlowService { get; set; }
        public string AdvanceTypeForm
        {
            get
            {
                if (ViewState["AdvanceTypeForm"] != null)
                    return ViewState["AdvanceTypeForm"].ToString();
                else
                    return string.Empty;
            }
            set { ViewState["AdvanceTypeForm"] = value; }
        }
        public long WorkflowID
        {
            get
            {
                if (ViewState["workflowID"] != null)
                    return UIHelper.ParseLong(ViewState["workflowID"].ToString());
                else
                    return 0;
            }
            set { ViewState["workflowID"] = value; }
        }

        //public string CurrenctStateDocument
        //{ 
        //    get { return ctlHiddenTaDocument.Value; }
        //    set { ctlHiddenTaDocument.Value = ctlAdvanceDocumentEditor.CurrentStateDocument; } 
        //}
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //check document type
            //must be properties for check type
            //if (Request.Params["id"] != null && Request.Params["wfid"] == null && Request.Params["id"] == "1")
            if (this.AdvanceTypeForm.Equals("1"))
            {
                ctlAdvanceDocumentEditor.AdvanceType = AdvanceTypeEnum.Domestic;
                ctlAdvanceDocumentEditor.HeaderForm = GetProgramMessage("$DMAdvanceForm$");
            }
            else
            {
                ctlAdvanceDocumentEditor.AdvanceType = AdvanceTypeEnum.International;
                ctlAdvanceDocumentEditor.HeaderForm = GetProgramMessage("$FRAdvanceForm$");
            }

            if (!Page.IsPostBack)
            {
                ctlAdvanceDocumentEditor.Initialize(FlagEnum.NewFlag, null);
                ctlAdvanceDocumentEditor.Visible = true;
                ctlUpdatePanelReadwriteButton.Update();
            }

            //สำหรับ check ว่า ถ้าเป็นการ create advance จาก TA ไม่ต้อง show confirm message ให้ send draft ไปเลย
            if (Request.QueryString["taDocumentID"] != null)
            {
                ctlHiddenTaDocument.Value = "TADocument";
            }
            else
            {
                ctlHiddenTaDocument.Value = string.Empty;
            }
            // ctlHiddenTaDocument.Value = ctlAdvanceDocumentEditor.CurrentStateDocument;

            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is ConfirmSubmit)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlConfirmPopup_OnNotifyPopup);
            }
        }
        #endregion

        #region Button Event
        protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.WorkflowID = ctlAdvanceDocumentEditor.Save();
                //if create document from ta not show message "send to workflow" else must be show message "send to workflow"
                if (this.WorkflowID > 0 && ctlHiddenTaDocument.Value != "TADocument")
                    ShowComfirmPopup();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
            if (ctlHiddenTaDocument.Value == "TADocument")
            {
                if (this.ValidationErrors.IsEmpty)
                {
                    //Modify by tom 07/06/2009, redirect to TAFrom
                    //Response.Redirect("DocumentView.aspx?wfid=" + this.WorkflowID.ToString());

                    long taDocumentID = UIHelper.ParseLong(Request.QueryString["taDocumentID"]);

                    TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.FindProxyByIdentity(taDocumentID);
                    WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(taDocument.DocumentID.DocumentID);

                    Response.Redirect("DocumentView.aspx?wfid=" + workFlow.WorkFlowID + "&eventFlag=" + PopUpReturnType.Add.ToString());
                }
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlAdvanceDocumentEditor.RollBackTransaction();
            ctlAdvanceDocumentEditor.Initialize(FlagEnum.NewFlag, null);

        }
        #endregion

        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            //long workFlowID = 0;
            int intWorkFlowStateEventID = 0;

            try
            {
                // ctlAdvanceDocumentEditor.ResultConfirm = ctlHiddenConfirm.Value;
                //workFlowID = ctlAdvanceDocumentEditor.Save();
                if (args.Type == PopUpReturnType.OK)
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowTypeID.AdvanceWorkFlowType);
                    intWorkFlowStateEventID = workFlowStateEvent.WorkFlowStateEventID;
                    object eventData = new SubmitResponse(intWorkFlowStateEventID);

                    WorkFlowService.NotifyEvent(this.WorkflowID, workFlowStateEvent.Name, eventData);
                    Response.Redirect("SubmitResult.aspx?wfid=" + this.WorkflowID);
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }

            // if No error then redirect to DocumentView.aspx
            if (this.ValidationErrors.IsEmpty)
            {
                Response.Redirect("DocumentView.aspx?wfid=" + this.WorkflowID.ToString());
            }
        }


        public void ShowComfirmPopup()
        {
            ConfirmSubmit ctlConfirmPopup = LoadPopup<ConfirmSubmit>("~/UserControls/DocumentEditor/Components/ConfirmSubmit.ascx", ctlPopUpUpdatePanel);
            ctlConfirmPopup.ShowConfirm();
        }
    }
}
