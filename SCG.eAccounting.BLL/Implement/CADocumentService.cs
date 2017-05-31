using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.WorkFlow.Service;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Query;
using SCG.eAccounting.DAL;

namespace SCG.eAccounting.BLL.Implement
{
    public class CADocumentService : ServiceBase<CADocument, long>, ICADocumentService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }

        public override SS.Standard.Data.NHibernate.Dao.IDao<CADocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.CADocumentDao;
        }

        #region IMPADocumentService Members

        public System.Data.DataSet PrepareDS()
        {
            CADocumentDataSet documentDS = new CADocumentDataSet();

            return documentDS;
        }

        public System.Data.DataSet PrepareDS(long documentID)
        {
            CADocumentDataSet documentDS = (CADocumentDataSet)this.PrepareDataToDataset(documentID, false);
            documentDS.AcceptChanges();

            return documentDS;
        }

        public long AddCADocumentTransaction(Guid txID)
        {
            //Initial Data for another table.
            CADocumentDataSet caDocumentDS = (CADocumentDataSet)TransactionService.GetDS(txID);
            CADocumentDataSet.DocumentRow documentRow = caDocumentDS.Document.NewDocumentRow(); //create new row to dataset.
            caDocumentDS.Document.AddDocumentRow(documentRow); //add new row to dataset.

            CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.NewCADocumentRow();
            caDocumentRow.DocumentID = documentRow.DocumentID;
            caDocumentDS.CADocument.AddCADocumentRow(caDocumentRow);

            return caDocumentRow.CADocumentID;
        }

        public void UpdateCADocumentTransaction(Guid txID, CADocument caDocument)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            CADocumentDataSet caDocumentDS = (CADocumentDataSet)TransactionService.GetDS(txID);
            CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.FindByCADocumentID(caDocument.CADocumentID);

            caDocumentRow.BeginEdit();

            caDocumentRow.DocumentID = caDocument.DocumentID.DocumentID;

            //if (mpaDocument.StartDate < DateTime.Today)
            //{
            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateMoreOrEqualToDay"));
            //}

            if (caDocument.StartDate > caDocument.EndDate)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateLessOrEqualEndDate"));
            }

            if (caDocument.StartDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateIsRequired"));
            }
            else
            {
                caDocumentRow.StartDate = caDocument.StartDate;
            }

            if (caDocument.EndDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EndDateIsRequired"));
            }
            else
            {
                caDocumentRow.EndDate = caDocument.EndDate;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            caDocumentRow.IsTemporary = caDocument.IsTemporary;
            caDocumentRow.CarLicenseNo = caDocument.CarLicenseNo;
            caDocumentRow.Brand = caDocument.Brand;
            caDocumentRow.Model = caDocument.Model;
            if (caDocument.IsWorkArea.HasValue)
            {
                caDocumentRow.IsWorkArea = caDocument.IsWorkArea.Value;
            }
            caDocumentRow.Remark = caDocument.Remark;
            caDocumentRow.CarType = caDocument.CarType;
            caDocumentRow.OwnerType = caDocument.OwnerType;
            caDocumentRow.Active = caDocument.Active;
            caDocumentRow.CreBy = UserAccount.UserID;
            caDocumentRow.CreDate = DateTime.Now;
            caDocumentRow.UpdBy = UserAccount.UserID;
            caDocumentRow.UpdDate = DateTime.Now;
            caDocumentRow.UpdPgm = UserAccount.CurrentProgramCode;

            caDocumentRow.EndEdit();
        }

        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(CAFieldGroup.All);
                visibleFields.Add(CAFieldGroup.Company);
                visibleFields.Add(CAFieldGroup.Initiator);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                visibleFields = WorkFlowService.GetVisibleFields(workFlow.WorkFlowID);
            }

            return visibleFields;
        }

        public IList<object> GetEditableFields(long? documentID)
        {
            IList<object> editableFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                editableFields.Add(CAFieldGroup.All);
                editableFields.Add(CAFieldGroup.Company);
                editableFields.Add(CAFieldGroup.Initiator);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                editableFields = WorkFlowService.GetEditableFields(workFlow.WorkFlowID);
            }

            return editableFields;
        }

        public long SaveCADocument(Guid txID, long caDocumentID)
        {
            CADocumentDataSet caDocumentDS = (CADocumentDataSet)TransactionService.GetDS(txID);
            CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.FindByCADocumentID(caDocumentID);
            long documentID = caDocumentRow.DocumentID;

            // Insert, Update SCGDocument.
            // Insert, Update, Delete DocumentInitiator.
            // Insert, Delete DocumentAttachment.
            documentID = SCGDocumentService.SaveSCGDocument(txID, documentID);

            // Insert, Update, Delete MPADocument.
            caDocumentID = ScgeAccountingDaoProvider.CADocumentDao.Persist(caDocumentDS.CADocument);

            // Insert, Update, Delete MPAItem.
            //MPAItemService.SaveMPADocumentItem(txID, mpaDocumentID);

            return caDocumentID;
        }

        public System.Data.DataSet PrepareDataToDataset(long documentID, bool isCopy)
        {
            CADocumentDataSet caDocumentDS = new CADocumentDataSet();

            CADocument caDocument = ScgeAccountingQueryProvider.CADocumentQuery.GetCADocumentByDocumentID(documentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (caDocument == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CADocumentNotFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(caDocumentDS, documentID);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(caDocumentDS, documentID, isCopy);
            }

            // Set data to Document row 
            CADocumentDataSet.CADocumentRow caDocumentRow = caDocumentDS.CADocument.NewCADocumentRow();

            caDocumentRow.CADocumentID = caDocument.CADocumentID;

            if (!isCopy)
            {
                if (caDocument.DocumentID != null)
                {
                    caDocumentRow.DocumentID = caDocument.DocumentID.DocumentID;
                }
            }
            else
            {
                if (caDocumentDS.Document.Rows.Count > 0)
                {
                    CADocumentDataSet.DocumentRow docRow = (CADocumentDataSet.DocumentRow)caDocumentDS.Document.Rows[0];
                    caDocumentRow.DocumentID = docRow.DocumentID;
                }
            }

            caDocumentRow.StartDate = caDocument.StartDate;
            caDocumentRow.EndDate = caDocument.EndDate;
            caDocumentRow.IsTemporary = caDocument.IsTemporary;
            if (caDocument.CarLicenseNo != null)
            {
                caDocumentRow.CarLicenseNo = caDocument.CarLicenseNo;
            }
            if (caDocument.CarLicenseNo != null)
            {
                caDocumentRow.Brand = caDocument.Brand;
            }
            if (caDocument.Model != null)
            {
                caDocumentRow.Model = caDocument.Model;
            }

            if (caDocument.IsWorkArea != null)
            {
                caDocumentRow.IsWorkArea = caDocument.IsWorkArea.Value;
            }
            if (caDocument.Remark != null)
            {
                caDocumentRow.Remark = caDocument.Remark;
            }
            caDocumentRow.CarType = caDocument.CarType;
            caDocumentRow.OwnerType = caDocument.OwnerType;

            caDocumentRow.Active = caDocument.Active;
            caDocumentRow.CreBy = caDocument.CreBy;
            caDocumentRow.CreDate = caDocument.CreDate;
            caDocumentRow.UpdBy = caDocument.UpdBy;
            caDocumentRow.UpdDate = caDocument.UpdDate;
            caDocumentRow.UpdPgm = caDocument.UpdPgm;

            // Add ta document row to MPADocument.
            caDocumentDS.CADocument.AddCADocumentRow(caDocumentRow);

            // Prepare Data to MPAItemService Datatable.
            //MPAItemService.PrepareDataToDataset(mpaDocumentDS, mpaDocument.MPADocumentID);


            return caDocumentDS;
        }

        #endregion
    }

    #region public enum CAFieldGroup
    public enum CAFieldGroup
    {
        All,
        Initiator,
        Company
    }
    #endregion public enum CAFieldGroup
}
