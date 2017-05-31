using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.Web.UserControls;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Query;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;
using SCG.eAccounting.Web.Helper;
using log4net;
using SCG.eAccounting.Web.UserControls.DocumentEditor;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class ExpenseForm : BaseUserControl
    {
        #region Property
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        private static ILog logger = log4net.LogManager.GetLogger(typeof(ExpenseForm));
        public string Mode
        {
            get
            {
                if (ViewState["FormMode"] != null)
                    return ViewState["FormMode"].ToString();
                else
                    return string.Empty;
            }
            set { ViewState["FormMode"] = value; }
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

        #endregion

        #region Override Method
        protected void Page_Init(object sender, EventArgs e)
        {
            (X as IDocumentEditor).DsNull += NotifyDsNull;
        }
        void NotifyDsNull()
        {
            Response.Redirect("Draft.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //check document type
            if (string.IsNullOrEmpty(Mode) || Mode.Equals("1"))
            {
                X.DocumentType = ZoneType.Domestic;
                X.HeaderForm = GetProgramMessage("$DMExpenseForm$");
            }
            else
            {
                X.DocumentType = ZoneType.Foreign;
                X.HeaderForm = GetProgramMessage("$FRExpenseForm$");
            }
            if (!Page.IsPostBack)
            {
                X.Initialize(FlagEnum.NewFlag, null);
                ctlDivReadWriteButton.Visible = true;
                ctlUpdatePanelReadwriteButton.Update();
            }

            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is ConfirmSubmit)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlConfirmPopup_OnNotifyPopup);
            }
        }

        //public void Initialize()
        //{
        //    ctlExpenseDocumentEditor.Initialize(FlagEnum.NewFlag, null);
        //    ctlDivReadWriteButton.Visible = true;
        //    ctlUpdatePanelReadwriteButton.Update();
        //}

        #endregion
        #region Button Event
        protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!X.RequireDocumentAttachment())
                {
                    this.WorkflowID = X.Save();
                    if (this.WorkflowID > 0)
                        ShowComfirmPopup();
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //ctlUpdatePanelValidation.Update();
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            X.RollBackTransaction();
            X.Initialize(FlagEnum.NewFlag, null);
        }

        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            //long workFlowID = 0;
            int intWorkFlowStateEventID = 0;
            try
            {
                //workFlowID = ctlExpenseDocumentEditor.Save();
                if (args.Type == PopUpReturnType.OK)
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowTypeID.ExpenseWorkFlow);
                    intWorkFlowStateEventID = workFlowStateEvent.WorkFlowStateEventID;
                    object eventData = new SubmitResponse(intWorkFlowStateEventID);

                    WorkFlowService.NotifyEvent(this.WorkflowID, workFlowStateEvent.Name, eventData);
                    Response.Redirect("SubmitResult.aspx?wfid=" + this.WorkflowID);
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                //ctlUpdatePanelValidation.Update();
            }

            // if No error then redirect to SubmitResult.aspx
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
        #endregion
    }
}