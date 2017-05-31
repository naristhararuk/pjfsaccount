using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using System.IO;
using System.Data;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class UploadFile : BasePage
    {
        public Guid TransactionID
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public ITransactionService TransactionService { get; set; }
        public IDocumentAttachmentService DocumentAttachmentService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["txID"]))
                {
                    this.TransactionID = new Guid(Request["txID"]);
                }
            }
             
        }
        protected void ctlAttach_OnClick(object sender, ImageClickEventArgs e)
        {
            #region Old Code Comment By AO 5-Mar-2009
            //long documentid = 0;
            //if (!string.IsNullOrEmpty(DocumentId))
            //{
            //    documentid = UIHelper.ParseLong(DocumentId);
            //}

            //HttpPostedFile file;
            //IList<DocumentAttachment> list = new List<DocumentAttachment>();
            //try
            //{
            //    DocumentAttachment documentAttachment = new DocumentAttachment();
            //    SCGDocument document = new SCGDocument(documentid);

            //    if (ctlFileUpload.HasFile)
            //    {
            //        if (!string.IsNullOrEmpty(FilePath))
            //        {
            //            FileInfo info = new FileInfo(ctlFileUpload.PostedFile.FileName);
            //            string fileName = info.Name;
            //            file = ctlFileUpload.PostedFile;

            //            documentAttachment.AttachFileName = fileName;
            //            documentAttachment.AttachFilePath = FilePath + "\\" + documentid;

            //            documentAttachment.Document = document;
            //            documentAttachment.Active = true;
            //            documentAttachment.CreBy = UserAccount.UserID;
            //            documentAttachment.CreDate = DateTime.Now;
            //            documentAttachment.UpdBy = UserAccount.UserID;
            //            documentAttachment.UpdDate = DateTime.Now;
            //            documentAttachment.UpdPgm = ProgramCode;

            //            // Add to transaction service.
            //            DocumentAttachmentService.AddDocumentAttachment(documentAttachment, file);

            //            // Save new file to stored directory.
            //            string storePath = string.Empty;
            //            storePath = AppDomain.CurrentDomain.BaseDirectory + FilePath.Replace("~", string.Empty).Replace("/", "\\") + "\\" + documentid + "\\";
            //            if (Directory.Exists(storePath))
            //            {
            //                file.SaveAs(storePath + documentAttachment.AttachFileName);
            //            }
            //            else
            //            {
            //                if (!Directory.Exists(storePath))
            //                {
            //                    Directory.CreateDirectory(storePath);
            //                }
            //                file.SaveAs(storePath + documentAttachment.AttachFileName);
            //            }

            //            GetDocumentAttachment();
            //            ctlUpdatePanelAttachment.Update();
            //        }
            //        else
            //        {
            //            file = null;
            //        }
            //    }

            //}
            //catch (ServiceValidationException ex)
            //{
            //    ValidationErrors.MergeErrors(ex.ValidationErrors);
            //}
            #endregion
            //if (!string.IsNullOrEmpty(Request["txID"]))
            //{
            //Guid txID = new Guid(Request["txID"]);
            DataSet ds = TransactionService.GetDS(this.TransactionID);
            DocumentAttachment documentAttachment = new DocumentAttachment();

            string filePath = ParameterServices.LocalAccessUploadFilePath;

            if (ctlFileUpload.HasFile)
            {
                long documentID = 0;
                if (ds.Tables["Document"].Rows.Count > 0)
                {
                    documentID = UIHelper.ParseLong(ds.Tables["Document"].Rows[0]["DocumentID"].ToString());
                }

                FileInfo info = new FileInfo(ctlFileUpload.PostedFile.FileName);
                string fileName = info.Name;
                // Get information of documentAttachment.
                //documentAttachment.AttachFileName = fileName;
                documentAttachment.AttachFileName = fileName.Replace("#", "_").Replace("%", "_");
                documentAttachment.AttachFilePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\") + "\\" + this.TransactionID.ToString();
                documentAttachment.DocumentID = new SCGDocument(documentID);

                // Save document Attachment Data.
                try
                {
                    DocumentAttachmentService.AddAttachmentToTransaction(this.TransactionID, documentAttachment, ctlFileUpload.PostedFile);
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }

                if (this.ValidationErrors.IsEmpty)
                {
                    try
                    {
                        // Save new file to stored directory.
                        string storePath = string.Empty;
                        storePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\") + "\\" + this.TransactionID.ToString() + "\\";
                        if (Directory.Exists(storePath))
                        {
                            ctlFileUpload.SaveAs(storePath + documentAttachment.AttachFileName);
                        }
                        else
                        {
                            if (!Directory.Exists(storePath))
                            {
                                Directory.CreateDirectory(storePath);
                            }
                            ctlFileUpload.SaveAs(storePath + documentAttachment.AttachFileName);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    //Page.RegisterClientScriptBlock("uploadfile", "<script>parent.uploadfile();</script>");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "uploadfile", "parent.uploadfile();", true);
                }
            }
            else
            {
                this.ValidationErrors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachmentFileIsRequired"));
            }
            //}
        }
    }
}
