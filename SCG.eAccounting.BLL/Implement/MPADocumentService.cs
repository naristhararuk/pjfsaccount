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
    public class MPADocumentService : ServiceBase<MPADocument, long>, IMPADocumentService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ISCGEmailService SCGEmailService { get; set; }
        public IMPAItemService MPAItemService { get; set; }

        public override SS.Standard.Data.NHibernate.Dao.IDao<MPADocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.MPADocumentDao;
        }

        #region IMPADocumentService Members

        public System.Data.DataSet PrepareDS()
        {
            MPADocumentDataSet documentDS = new MPADocumentDataSet();

            return documentDS;
        }

        public System.Data.DataSet PrepareDS(long documentID)
        {
            MPADocumentDataSet documentDS = (MPADocumentDataSet)this.PrepareDataToDataset(documentID, false);
            documentDS.AcceptChanges();

            return documentDS;
        }

        public long AddMPADocumentTransaction(Guid txID)
        {
            //Initial Data for another table.
            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.DocumentRow documentRow = mpaDocumentDS.Document.NewDocumentRow(); //create new row to dataset.
            mpaDocumentDS.Document.AddDocumentRow(documentRow); //add new row to dataset.

            MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.NewMPADocumentRow();
            mpaDocumentRow.DocumentID = documentRow.DocumentID;
            mpaDocumentDS.MPADocument.AddMPADocumentRow(mpaDocumentRow);

            return mpaDocumentRow.MPADocumentID;
        }

        public void UpdateMPADocumentTransaction(Guid txID, MPADocument mpaDocument)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.FindByMPADocumentID(mpaDocument.MPADocumentID);

            mpaDocumentRow.BeginEdit();

            mpaDocumentRow.DocumentID = mpaDocument.DocumentID.DocumentID;

            //if (mpaDocument.StartDate < DateTime.Today)
            //{
            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateMoreOrEqualToDay"));
            //}

            if (mpaDocument.StartDate > mpaDocument.EndDate)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateLessOrEqualEndDate"));
            }

            if (mpaDocument.StartDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("StartDateIsRequired"));
            }
            else
            {
                mpaDocumentRow.StartDate = mpaDocument.StartDate;
            }

            if (mpaDocument.EndDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("EndDateIsRequired"));
            }
            else
            {
                mpaDocumentRow.EndDate = mpaDocument.EndDate;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            mpaDocumentRow.Active = mpaDocument.Active;
            mpaDocumentRow.CreBy = UserAccount.UserID;
            mpaDocumentRow.CreDate = DateTime.Now;
            mpaDocumentRow.UpdBy = UserAccount.UserID;
            mpaDocumentRow.UpdDate = DateTime.Now;
            mpaDocumentRow.UpdPgm = UserAccount.CurrentProgramCode;

            mpaDocumentRow.EndEdit();
        }

        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(MPAFieldGroup.All);
                visibleFields.Add(MPAFieldGroup.Company);
                visibleFields.Add(MPAFieldGroup.Initiator);
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
                editableFields.Add(MPAFieldGroup.All);
                editableFields.Add(MPAFieldGroup.Company);
                editableFields.Add(MPAFieldGroup.Initiator);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                editableFields = WorkFlowService.GetEditableFields(workFlow.WorkFlowID);
            }

            return editableFields;
        }

        public long SaveMPADocument(Guid txID, long mpaDocumentID)
        {
            MPADocumentDataSet mpaDocumentDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.FindByMPADocumentID(mpaDocumentID);
            long documentID = mpaDocumentRow.DocumentID;

            // Insert, Update SCGDocument.
            // Insert, Update, Delete DocumentInitiator.
            // Insert, Delete DocumentAttachment.
            documentID = SCGDocumentService.SaveSCGDocument(txID, documentID);

            // Insert, Update, Delete MPADocument.
            mpaDocumentID = ScgeAccountingDaoProvider.MPADocumentDao.Persist(mpaDocumentDS.MPADocument);

            // Insert, Update, Delete MPAItem.
            MPAItemService.SaveMPAItem(txID, documentID);

            return mpaDocumentID;
        }

        public System.Data.DataSet PrepareDataToDataset(long documentID, bool isCopy)
        {
            MPADocumentDataSet mpaDocumentDS = new MPADocumentDataSet();

            MPADocument mpaDocument = ScgeAccountingQueryProvider.MPADocumentQuery.GetMPADocumentByDocumentID(documentID);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (mpaDocument == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MPADocumentNotFound"));
            }
            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            // Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(mpaDocumentDS, documentID);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(mpaDocumentDS, documentID, isCopy);
            }

            // Set data to Document row 
            MPADocumentDataSet.MPADocumentRow mpaDocumentRow = mpaDocumentDS.MPADocument.NewMPADocumentRow();

            mpaDocumentRow.MPADocumentID = mpaDocument.MPADocumentID;

            if (!isCopy)
            {
                if (mpaDocument.DocumentID != null)
                {
                    mpaDocumentRow.DocumentID = mpaDocument.DocumentID.DocumentID;
                }
            }
            else
            {
                if (mpaDocumentDS.Document.Rows.Count > 0)
                {
                    MPADocumentDataSet.DocumentRow docRow = (MPADocumentDataSet.DocumentRow)mpaDocumentDS.Document.Rows[0];
                    mpaDocumentRow.DocumentID = docRow.DocumentID;
                }
            }

            mpaDocumentRow.StartDate = mpaDocument.StartDate;
            mpaDocumentRow.EndDate = mpaDocument.EndDate;

            mpaDocumentRow.Active = mpaDocument.Active;
            mpaDocumentRow.CreBy = mpaDocument.CreBy;
            mpaDocumentRow.CreDate = mpaDocument.CreDate;
            mpaDocumentRow.UpdBy = mpaDocument.UpdBy;
            mpaDocumentRow.UpdDate = mpaDocument.UpdDate;
            mpaDocumentRow.UpdPgm = mpaDocument.UpdPgm;

            // Add ta document row to MPADocument.
            mpaDocumentDS.MPADocument.AddMPADocumentRow(mpaDocumentRow);

            // Prepare Data to MPAItemService Datatable.
            MPAItemService.PrepareDataToDataset(mpaDocumentDS, mpaDocument.MPADocumentID);


            return mpaDocumentDS;
        }
        #endregion
    }

    #region public enum MPAFieldGroup
    public enum MPAFieldGroup
    {
        All,
        Initiator,
        Company
    }
    #endregion public enum MPAFieldGroup
}
