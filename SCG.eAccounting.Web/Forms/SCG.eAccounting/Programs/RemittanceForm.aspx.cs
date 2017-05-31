using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.UserControls;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class RemittanceForm : BasePage
    {
        
        public IWorkFlowService WorkFlowService { get; set; }
        public long WorkFlowID
        {
            get
            {
                if (ViewState["WorkFlowID"] != null)
                {
                    return UIHelper.ParseLong(ViewState["WorkFlowID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set { ViewState["WorkFlowID"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            PopUpUpdatePanel = ctlPopUpUpdatePanel;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlRemittanceDocumentEditor.Initialize(FlagEnum.NewFlag, null);

                ctlDivReadWriteButton.Visible = true;
                ctlUpdatePanelReadwriteButton.Update();
            }
            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is ConfirmSubmit)
            {
                ctl.OnPopUpReturn += new BaseUserControl.PopUpReturnEventHandler(ctlConfirmPopup_OnNotifyPopup);
            }
        }

        protected void ctlRemittanceSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.WorkFlowID = ctlRemittanceDocumentEditor.Save();
                if (this.WorkFlowID > 0)
                    ShowComfirmPopup();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }
        protected void ctlRemittanceCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlRemittanceDocumentEditor.RollBackTransaction();
            ctlRemittanceDocumentEditor.Initialize(FlagEnum.NewFlag, null); //, visibleFields, editableFields);
        }
        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            try
            {
                if (args.Type == PopUpReturnType.OK)
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowTypeID.RemittanceWorkFlow);
                    int intWorkFlowStateEventID = workFlowStateEvent.WorkFlowStateEventID;
                    object eventData = new SubmitResponse(intWorkFlowStateEventID);

                    WorkFlowService.NotifyEvent(this.WorkFlowID, workFlowStateEvent.Name, eventData);
                    Response.Redirect("SubmitResult.aspx?wfid=" + this.WorkFlowID);
                }

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }

            // if No error then redirect to SubmitResult.aspx
            if (this.ValidationErrors.IsEmpty)
            {
                //Page.RegisterStartupScript("comfirm", "<script>return confirm('Do you want to send your document to WorkFlow')</script>");
                Response.Redirect("DocumentView.aspx?wfid=" + this.WorkFlowID.ToString());
            }
        }

        public void ShowComfirmPopup()
        {
            ConfirmSubmit ctlConfirmPopup = LoadPopup<ConfirmSubmit>("~/UserControls/DocumentEditor/Components/ConfirmSubmit.ascx", ctlPopUpUpdatePanel);
            ctlConfirmPopup.ShowConfirm();
        }
    }
}
