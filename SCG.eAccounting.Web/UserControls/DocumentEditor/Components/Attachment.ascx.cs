using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

using SS.Standard.UI;
using SS.Standard.Security;
using SS.Standard.Utilities;

using SS.DB.Query;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class Attachment : BaseUserControl, SCG.eAccounting.Web.UserControls.DocumentEditor.IEditorComponent
    {
        #region Property
        public ITransactionService TransactionService { get; set; }
        public IDocumentAttachmentService DocumentAttachmentService { get; set; }

        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
        public Guid TransactionID
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public bool IsEmptyData
        {
            get { return (bool)ViewState["IsEmptyData"]; }
            set { ViewState["IsEmptyData"] = value; }
        }
        #endregion

        #region OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Use for form that contain FileUpload Control.
            HtmlForm form = this.Page.Form;
            if ((form != null) && (form.Enctype.Length == 0))
            {
                form.Enctype = "multipart/form-data";
            }

            if (!Page.IsPostBack)
            {
                this.BindControl();
            }
        }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //ctlUpload.Attributes.Add("src", ResolveUrl("~/UserControls/DocumentEditor/Components/UploadFile.aspx?txID=" + this.TransactionID));
                ctlDescription.Text = GetMessage("AttechmentDescription", (ParameterServices.MaxUploadFileSize / (1024 * 1024)), ParameterServices.AllowExtension);
            }
        }
        #endregion

        #region Private Method
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCtlAttachmentGridCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlAttachmentGrid.ClientID + "', '" + ctlAttachmentGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateAttachmentGridChkBox", script.ToString(), true);
        }
        #endregion

        #region GridView Event
        protected void ctlAttachmentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long attachmentID = UIHelper.ParseLong(ctlAttachmentGrid.DataKeys[e.Row.RowIndex].Values["AttachmentID"].ToString());
                DataRowView attachment = (DataRowView)e.Row.DataItem;

                if (attachment["DocumentID"] != null)
                {
                    LinkButton ctlAttachFileName = e.Row.FindControl("ctlAttachFileName") as LinkButton;
                    string relativeFilePath = ParameterServices.RelativeUploadFilePath;

                    if (attachmentID > 0)
                    {
                        relativeFilePath += attachment["DocumentID"].ToString() + "/";
                    }
                    else
                    {
                        relativeFilePath += this.TransactionID.ToString() + "/";
                    }

                    string fullFileName = relativeFilePath + attachment["AttachFileName"].ToString();

                    ctlAttachFileName.OnClientClick = "javascript:window.open('" + fullFileName + "'); return false;";
                }

                Label ctlSequence = e.Row.FindControl("ctlSequence") as Label;
                ctlSequence.Text = ((ctlAttachmentGrid.PageIndex * ctlAttachmentGrid.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }
        protected void ctlAttachmentGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlAttachmentGrid.Rows.Count > 0)
            {
                this.RegisterScriptForGridView();

                // show delete button for new and edit mode
                if (!this.InitialFlag.Equals(FlagEnum.ViewFlag))
                {
                    ctlRemove.Style["display"] = "block";
                }
                ctlRemove.OnClientClick = String.Format("return ConfirmOperation('{0}');", GetProgramMessage("DeleteMessage"));
            }
            else
            {
                ctlRemove.Style["display"] = "none";
            }
        }
        #endregion

        #region Button Event
        protected void ctlUploaded_OnClick(object sender, EventArgs e)
        {
            this.BindControl();
            ctlUpdateAttachmentGrid.Update();
        }
        protected void ctlRemove_OnClick(object sender, ImageClickEventArgs e)
        {
            #region Old Code Comment By AO 5-Mar-2009
            //IList<DocumentAttachment> documentAttachmentList = new List<DocumentAttachment>();
            //foreach (GridViewRow row in ctlAttachmentGrid.Rows)
            //{
            //    if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
            //    {
            //        long attachmentID = UIHelper.ParseLong(ctlAttachmentGrid.DataKeys[row.RowIndex].Values["AttachmentID"].ToString());

            //        // Remove from transaction service.					
            //    }
            //}
            #endregion

            foreach (GridViewRow row in ctlAttachmentGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    long attachmentID = UIHelper.ParseLong(ctlAttachmentGrid.DataKeys[row.RowIndex].Values["AttachmentID"].ToString());

                    // Remove from transaction.
                    DocumentAttachment documentAttachment = new DocumentAttachment(attachmentID);
                    DocumentAttachmentService.DeleteAttachmentFromTransaction(this.TransactionID, documentAttachment);
                }
            }

            this.BindControl();
            ctlUpdateAttachmentGrid.Update();
        }
        #endregion

        #region IEditorComponent Members
        public void Initialize(Guid txID, long currentDocumentID, string initFlag)
        {
            this.TransactionID = txID;
            this.DocumentID = currentDocumentID;
            this.InitialFlag = initFlag;

            ctlUpload.Attributes.Add("src", ResolveUrl("~/UserControls/DocumentEditor/Components/UploadFile.aspx?txID=" + this.TransactionID));

            // Show the control as view or edit/new mode.
            if (this.InitialFlag.Equals(FlagEnum.ViewFlag))
            {
                this.BindControl();

                ctlAttachmentGrid.Columns[2].Visible = false;

                ctlUploadFile.Visible = false;
                ctlRemove.Style["display"] = "none";   
            }
            else
            {
                this.BindControl();

                ctlAttachmentGrid.Columns[2].Visible = true;

                ctlUploadFile.Visible = true;
            }
            ctlUpdateAttachmentGrid.Update();
            ctlUpdatePanelAttachment.Update();
        }
        public void BindControl()
        {
            Guid txID = this.TransactionID;
            DataSet documentDataset = TransactionService.GetDS(txID);

            if (documentDataset.Tables["DocumentAttachment"].Rows.Count > 0)
                IsEmptyData = false;
            else
                IsEmptyData = true;

            ctlAttachmentGrid.DataSource = documentDataset.Tables["DocumentAttachment"];
            ctlAttachmentGrid.DataBind();
        }
        public void ResetControlValue()
        {
            ctlAttachmentGrid.DataSource = null;
            ctlAttachmentGrid.DataBind();
            
            ctlUpdateAttachmentGrid.Update();
            ctlUpdatePanelAttachment.Update();
        }
        #endregion
    }
}