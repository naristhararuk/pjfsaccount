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
    public partial class FixedAdvanceForm : BaseUserControl
    {
        public IWorkFlowService WorkFlowService { get; set; }
        public string FixedAdvanceTypeForm
        {
            get
            {
                if (ViewState["FixedAdvanceTypeForm"] != null)
                    return ViewState["FixedAdvanceTypeForm"].ToString();
                else
                    return string.Empty;
            }
            set { ViewState["FixedAdvanceTypeForm"] = value; }
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

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.FixedAdvanceTypeForm.Equals("1"))
            {
                ctlFixedAdvanceDocumentEditor.FixedAdvanceType = AdvanceTypeEnum.Domestic;
                ctlFixedAdvanceDocumentEditor.HeaderForm = GetProgramMessage("FixedAdvanceForm");
            }

            if (!Page.IsPostBack)
            {
                ctlFixedAdvanceDocumentEditor.Initialize(FlagEnum.NewFlag, null);
                ctlFixedAdvanceDocumentEditor.Visible = true;
                ctlUpdatePanelReadwriteButton.Update();
            }

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
                this.WorkflowID = ctlFixedAdvanceDocumentEditor.Save();
                    ShowComfirmPopup();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlFixedAdvanceDocumentEditor.RollBackTransaction();
            ctlFixedAdvanceDocumentEditor.Initialize(FlagEnum.NewFlag, null);

        }
        #endregion

        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            int intWorkFlowStateEventID = 0;

            try
            {
                if (args.Type == PopUpReturnType.OK)
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowTypeID.FixedAdvanceWorkFlow);
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
