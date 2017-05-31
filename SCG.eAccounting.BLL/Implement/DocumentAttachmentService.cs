using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SS.Standard.Utilities;
using SS.Standard.Security;
using System.Data;
using SCG.eAccounting.Query;
using System.IO;
using SS.DB.Query;
using SS.DB.BLL.Implement;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class DocumentAttachmentService : ServiceBase<DocumentAttachment, long>, IDocumentAttachmentService
    {
		#region Property
		public ITransactionService TransactionService { get; set; }
		public IUserAccount UserAccount { get; set; }
		#endregion

		#region Override Method
		public override IDao<DocumentAttachment, long> GetBaseDao()
		{
			return ScgeAccountingDaoProvider.DocumentAttachmentDao;
		}
		#endregion

		#region Oldest Code
		public IList<DocumentAttachment> FindByActive(DocumentAttachment criteria, int firstResult, int maxResults, string sortExpression)
		{
			return NHibernateDaoHelper.FindPagingByCriteria<DocumentAttachment>(ScgeAccountingDaoProvider.DocumentAttachmentDao, "FindByActive", new object[] { criteria }, firstResult, maxResults, sortExpression);
		}
		public int CountBySDocumentAttachmentCriteria(DocumentAttachment criteria)
		{
			return NHibernateDaoHelper.CountByCriteria(ScgeAccountingDaoProvider.DocumentAttachmentDao, "FindByActive", new object[] { criteria });
		}
		public void UpdateDocumentAttachment(long id)
		{
			ScgeAccountingDaoProvider.DocumentAttachmentDao.DeleteByDocumentId(id);
		}
		public void AddDocumentAttachment(DocumentAttachment documentAttachment, HttpPostedFile fileStream)
		{
			#region Validate DocumentAttachment
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

			if (string.IsNullOrEmpty(documentAttachment.AttachFileName))
			{
				errors.AddError("Attachmen.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
			}

			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			ScgeAccountingDaoProvider.DocumentAttachmentDao.Save(documentAttachment);
		}
		#endregion
        
        #region Old Code
        public void InsertDocumentAttachment(Guid txID, long documentID)
        {
            DataSet ds = TransactionService.GetDS(txID);
            SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(documentID);
            DataTable insertTable = ds.Tables["DocumentAttachment"].GetChanges(DataRowState.Added);

            // ======================================================================
            // saveFilePath get from DbParameter.
            string saveFilePath = "~/ImageFiles/" + documentID.ToString();
            // ======================================================================
            string oldFilePath = string.Empty;
            string newFilePath = string.Empty;

            if (insertTable != null)
            {
                foreach (DataRow row in insertTable.Rows)
                {
                    if (Convert.ToInt64(row["AttachmentID"].ToString()) < 0)
                    {
                        DocumentAttachment attachment = new DocumentAttachment();
                        attachment.DocumentID = document;
                        attachment.AttachFileName = row["AttachFileName"].ToString();
                        attachment.AttachFilePath = saveFilePath;
                        oldFilePath = row["AttachFilePath"].ToString();
                        newFilePath = row["AttachFilePath"].ToString().Replace(txID.ToString(), documentID.ToString());
                        attachment.Active = (bool)row["Active"];
                        attachment.CreBy = Convert.ToInt64(row["CreBy"].ToString());
                        attachment.CreDate = DateTime.Parse(row["CreDate"].ToString());
                        attachment.UpdBy = Convert.ToInt64(row["UpdBy"].ToString());
                        attachment.UpdDate = DateTime.Parse(row["UpdDate"].ToString());
                        attachment.UpdPgm = row["UpdPgm"].ToString();

                        ScgeAccountingDaoProvider.DocumentAttachmentDao.Save(attachment);
                    }
                }

                // Move file to new filepath.
                if (!string.IsNullOrEmpty(oldFilePath) &&
                    !string.IsNullOrEmpty(newFilePath) &&
                    !newFilePath.Equals(oldFilePath))
                {
                    if (Directory.Exists(oldFilePath))
                    {
                        if (Directory.Exists(newFilePath))
                        {
                            // if already have the new file path.
                            // then move only file to the new file path.
                            string[] files = Directory.GetFiles(oldFilePath);
                            foreach (string file in files)
                            {
                                string newFile = file.Replace(oldFilePath, newFilePath);
                                if (File.Exists(newFile))
                                {
                                    File.Delete(newFile);
                                }
                                File.Move(file, newFile);
                            }
                            Directory.Delete(oldFilePath);
                        }
                        else
                        {
                            // if no new file directory.
                            // then move the old file path to the new file path.
                            Directory.Move(oldFilePath, newFilePath);
                        }
                    }
                }
            }
        }
		public void DeleteDocumentAttachment(Guid txID)
		{
			DataSet ds = TransactionService.GetDS(txID);
			DataTable deleteTable = ds.Tables["DocumentAttachment"].GetChanges(DataRowState.Deleted);

			short attachmentID;
			if (deleteTable != null)
			{
				foreach (DataRow row in deleteTable.Rows)
				{
					attachmentID = Convert.ToInt16(row["AttachmentID", DataRowVersion.Original].ToString());

					if (attachmentID > 0)
					{
						DocumentAttachment attachment = ScgeAccountingQueryProvider.DocumentAttachmentQuery.FindProxyByIdentity(attachmentID);
						if (attachment != null)
						{
							ScgeAccountingDaoProvider.DocumentAttachmentDao.Delete(attachment);
						}
					}
				}
			}
		}
		#endregion

        #region Prepare Data To Document DataSet.
        public void PrepareDataToDataset(DataSet documentDataset, long documentID)
        {
            IList<DocumentAttachment> documentAttachmentList = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(documentID);

            foreach (DocumentAttachment attachment in documentAttachmentList)
            {
                // Set data to document attachment datatable.
                DataRow attachmentRow = documentDataset.Tables["DocumentAttachment"].NewRow();
                attachmentRow["AttachmentID"] = attachment.AttachmentID;
                attachmentRow["DocumentID"] = documentID;
                attachmentRow["AttachFilePath"] = attachment.AttachFilePath;
                attachmentRow["AttachFileName"] = attachment.AttachFileName;
                attachmentRow["Active"] = attachment.Active;
                attachmentRow["CreBy"] = attachment.CreBy;
                attachmentRow["CreDate"] = attachment.CreDate;
                attachmentRow["UpdBy"] = attachment.UpdBy;
                attachmentRow["UpdDate"] = attachment.UpdDate;
                attachmentRow["UpdPgm"] = attachment.UpdPgm;

                // Add document attachment row to budgetDocumentDS.
                documentDataset.Tables["DocumentAttachment"].Rows.Add(attachmentRow);
            }
        }
        #endregion

		#region For Save Document Attachment to Transaction.
		public long AddAttachmentToTransaction(Guid TxID, DocumentAttachment documentAttachment, HttpPostedFile attachFile)
		{
			//string strMaxFileSize = SsDbQueryProvider.DbParameterQuery.getParameterByGroupNo_SeqNo("7", "1");
			int maxFileSize = ParameterServices.MaxUploadFileSize;
            int maxFileNameLength = 50;
            string[] allowExtension = ParameterServices.AllowExtension.ToUpper().Split(',');
            string[] fileExtension = attachFile.FileName.Split('.');            

			DataSet documentDataset = TransactionService.GetDS(TxID);
			DataRow row = documentDataset.Tables["DocumentAttachment"].NewRow();

			#region Validate DocumentAttachment
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (string.IsNullOrEmpty(documentAttachment.AttachFileName))
			{
				errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachmentFileIsRequired"));
			}
			else
			{
				DataRow[] rows =
					documentDataset.Tables["DocumentAttachment"].Select(
					string.Format("{0} = '{1}'", documentDataset.Tables["DocumentAttachment"].Columns["AttachFileName"].ColumnName, documentAttachment.AttachFileName));
				if (rows.Length > 0)
				{
					errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachFileNameAlreadyExist"));
				}
			}
			if (attachFile.ContentLength > maxFileSize)
			{
				errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachmentFileSizeNotAllow"));
			}
            FileInfo fileInfo = new FileInfo(attachFile.FileName);
            if (fileInfo.Name.Length > maxFileNameLength)
            {
                errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachmentFileNameLengthNotAllow"));
            }
            if (!allowExtension.Contains(fileExtension[fileExtension.Length - 1].ToUpper()))
            {
                errors.AddError("ValidationError", new Spring.Validation.ErrorMessage("AttachmentFileTypeNotAllow"));
            }

			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			row["AttachFileName"] = documentAttachment.AttachFileName;
			row["AttachFilePath"] = documentAttachment.AttachFilePath;
			row["DocumentID"] = documentAttachment.DocumentID.DocumentID;
			row["Active"] = true;
			row["CreBy"] = UserAccount.UserID;
			row["CreDate"] = DateTime.Now;
			row["UpdBy"] = UserAccount.UserID;
			row["UpdDate"] = DateTime.Now;
			row["UpdPgm"] = UserAccount.CurrentProgramCode;
			documentDataset.Tables["DocumentAttachment"].Rows.Add(row);

			return Convert.ToInt64(row["AttachmentID"].ToString());
		}
		public void DeleteAttachmentFromTransaction(Guid TxID, DocumentAttachment documentAttachment)
		{
			DataSet documentDataset = TransactionService.GetDS(TxID);
			DataRow row = documentDataset.Tables["DocumentAttachment"].Rows.Find(documentAttachment.AttachmentID);
			row.Delete();
		}
		#endregion

		#region Insert, Delete DocumentAttachment
        public void SaveDocumentAttachment(Guid txID, long documentID)
        {
			DataSet ds = TransactionService.GetDS(txID);
			DataTable insertTable = ds.Tables["DocumentAttachment"].GetChanges(DataRowState.Added);

			string saveFilePath = ParameterServices.LocalAccessUploadFilePath + documentID.ToString();
			string oldFilePath = string.Empty;
			string newFilePath = string.Empty;

			if (insertTable != null)
			{
				foreach (DataRow row in insertTable.Rows)
				{
					row.BeginEdit();
					oldFilePath = row["AttachFilePath"].ToString();
					newFilePath = row["AttachFilePath"].ToString().Replace(txID.ToString(), documentID.ToString());
					row["AttachFilePath"] = saveFilePath;
					row.EndEdit();
				}

				ds.Tables["DocumentAttachment"].Merge(insertTable);

				// Move file to new filepath.
				if (!string.IsNullOrEmpty(oldFilePath) &&
					!string.IsNullOrEmpty(newFilePath) &&
					!newFilePath.Equals(oldFilePath))
				{
					if (Directory.Exists(oldFilePath))
					{
						if (!Directory.Exists(newFilePath))
						{
							// if Directory not Exist then create new Directory.
							Directory.CreateDirectory(newFilePath);
						}

						string[] files = Directory.GetFiles(oldFilePath);
						foreach (string file in files)
						{
							string newFile = file.Replace(oldFilePath, newFilePath);
							File.Copy(file, newFile, true);
						}
						
						// Call FileDelete for Delete file in the specific directory.
						Utilities.FilesDelete(oldFilePath);
					}
				}
			}

            // Insert, Delete DocumentAttachment.
            ScgeAccountingDaoProvider.DocumentAttachmentDao.Persist(ds.Tables["DocumentAttachment"]);
		}
		#endregion

        public void ValidateDocumentAttachment(Guid txID, long documentID)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            DataSet ds = TransactionService.GetDS(txID);
            DataTable dt = ds.Tables["DocumentAttachment"];
            DataRow[] attachments = dt.Select();
            
            if(attachments.Count() == 0)
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AttachmentIsRequired"));

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
	}
}
