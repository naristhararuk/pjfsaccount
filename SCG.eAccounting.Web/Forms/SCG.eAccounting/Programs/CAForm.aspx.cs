using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls.DocumentEditor;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.Web.UserControls.DocumentEditor.Components;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class CAForm : BasePage
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
            (ctlCADocumentEditor as IDocumentEditor).DsNull += NotifyDsNull;
            PopUpUpdatePanel = ctlPopUpUpdatePanel;
        }

        void NotifyDsNull()
        {
            Response.Redirect("Draft.aspx");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlCADocumentEditor.Initialize(FlagEnum.NewFlag, null);

                ctlDivReadWriteButton.Visible = true;
                ctlUpdatePanelReadWriteButton.Update();
            }
            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is ConfirmSubmit)
            {
                ctl.OnPopUpReturn += new BaseUserControl.PopUpReturnEventHandler(ctlConfirmPopup_OnNotifyPopup);
            }
        }

        #region protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        protected void ctlSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.WorkFlowID = ctlCADocumentEditor.Save();
                if (this.WorkFlowID > 0)
                    ShowComfirmPopup();
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanelValidation.Update();
            }
        }
        #endregion protected void ctlSave_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ctlCADocumentEditor.RollBackTransaction();
            ctlCADocumentEditor.Initialize(FlagEnum.NewFlag, null);
        }
        #endregion protected void ctlCancel_Click(object sender, ImageClickEventArgs e)

        protected void ctlConfirmPopup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            try
            {
                if (args.Type == PopUpReturnType.OK)
                {
                    WorkFlowStateEvent workFlowStateEvent = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetSendDraftStateEvent(WorkFlowTypeID.CAWorkFlow);
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
                Response.Redirect("DocumentView.aspx?wfid=" + this.WorkFlowID.ToString());
            }
        }

        public void ShowComfirmPopup()
        {
            ConfirmSubmit ctlConfirmPopup = LoadPopup<ConfirmSubmit>("~/UserControls/DocumentEditor/Components/ConfirmSubmit.ascx", ctlPopUpUpdatePanel);
            //ctlConfirmPopup.DisableButton = true;
            ctlConfirmPopup.ShowConfirm();
        }
    }
}