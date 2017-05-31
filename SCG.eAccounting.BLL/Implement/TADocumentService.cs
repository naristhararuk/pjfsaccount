using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO.DataSet;
using System.Data;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using SS.SU.DTO;
using SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using SS.Standard.Security;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Service;

namespace SCG.eAccounting.BLL.Implement
{
    public class TADocumentService : ServiceBase<TADocument, long>, ITADocumentService
    {
        #region local variable
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ITADocumentTravellerService TADocumentTravellerService { get; set; }
        public ITADocumentScheduleService TADocumentScheduleService { get; set; }
        public ITADocumentAdvanceService TADocumentAdvanceService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        #endregion local variable

        #region Override Method
        public override IDao<TADocument, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.TADocumentDao;
        }
        #endregion

        #region public DataSet PrepareDS()
        public DataSet PrepareDS()
        {
            TADocumentDataSet taDocumentDS = new TADocumentDataSet();

            return taDocumentDS;
        }
        #endregion public DataSet PrepareDS()

        #region public DataSet PrepareDS(long documentID)
        public DataSet PrepareDS(long documentID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)this.PrepareDataToDataset(documentID, false);
            taDocumentDS.AcceptChanges();

            return taDocumentDS;
        }
        #endregion public DataSet PrepareDS(long documentID)

        #region public DataSet PrepareDataToDataset(long documentID,bool isCopy)
        public DataSet PrepareDataToDataset(long documentID, bool isCopy)
		{
			TADocumentDataSet taDocumentDS = new TADocumentDataSet();

			TADocument taDocument = ScgeAccountingQueryProvider.TADocumentQuery.GetTADocumentByDocumentID(documentID);

			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            
			if (taDocument == null)
			{
				errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("NoTADocumentFound"));
			}
			if (!errors.IsEmpty) 
            {
                throw new ServiceValidationException(errors);
            }

           	// Prepare Data to Document datatable.
            if (!isCopy)
            {
                SCGDocumentService.PrepareDataToDataset(taDocumentDS, documentID);
            }
            else
            {
                SCGDocumentService.PrepareDataInternalToDataset(taDocumentDS, documentID, isCopy);
            }

			// Set data to Budget Document row in budgetDocumentDS.
			TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.NewTADocumentRow();

            taDocumentRow.TADocumentID = taDocument.TADocumentID;

            if (!isCopy)
            {
                if (taDocument.DocumentID != null)
                {
                    taDocumentRow.DocumentID = taDocument.DocumentID.DocumentID;
                }
            }
            else
            {
                if (taDocumentDS.Document.Rows.Count > 0)
                {
                    TADocumentDataSet.DocumentRow docRow = (TADocumentDataSet.DocumentRow)taDocumentDS.Document.Rows[0];
                    taDocumentRow.DocumentID = docRow.DocumentID;
                }
            }

            taDocumentRow.FromDate = taDocument.FromDate;
            taDocumentRow.ToDate = taDocument.ToDate;
            taDocumentRow.IsBusinessPurpose = taDocument.IsBusinessPurpose;
            taDocumentRow.IsTrainningPurpose = taDocument.IsTrainningPurpose;
            taDocumentRow.IsOtherPurpose = taDocument.IsOtherPurpose;
            if (taDocument.OtherPurposeDescription != null)
            {
                taDocumentRow.OtherPurposeDescription = taDocument.OtherPurposeDescription;
            }
            taDocumentRow.TravelBy = taDocument.TravelBy;
            if (taDocument.Province != null)
            {
                taDocumentRow.Province = taDocument.Province;
            }
            if (taDocument.Country != null)
            {
                taDocumentRow.Country = taDocument.Country;
            }
            taDocumentRow.Ticketing = taDocument.Ticketing;

            //if (taDocument.CostCenterID != null)
            //{
            //    taDocumentRow.CostCenterID = taDocument.CostCenterID.CostCenterID;
            //}
            //if (taDocument.Account != null)
            //{
            //    taDocumentRow.AccountID = taDocument.Account.AccountID;
            //}
            //if (taDocument.IOID != null)
            //{
            //    taDocumentRow.IOID = taDocument.IOID.IOID;
            //}

			taDocumentRow.Active = taDocument.Active;
			taDocumentRow.CreBy = taDocument.CreBy;
			taDocumentRow.CreDate = taDocument.CreDate;
			taDocumentRow.UpdBy = taDocument.UpdBy;
			taDocumentRow.UpdDate = taDocument.UpdDate;
			taDocumentRow.UpdPgm = taDocument.UpdPgm;

            // Add ta document row to taDocument.
            taDocumentDS.TADocument.AddTADocumentRow(taDocumentRow);

            // Prepare Data to TADocumentTravellerService Datatable.
            TADocumentTravellerService.PrepareDataToDataset(taDocumentDS, taDocument.TADocumentID);

            // Prepare Data to TADocumentScheduleService Datatable.
            TADocumentScheduleService.PrepareDataToDataset(taDocumentDS, taDocument.TADocumentID);

            //Not Save to TADocumemtAdvance Table.
            // Prepare Data to TADocumentAdvanceService Datatable.
            ////TADocumentAdvanceService.PrepareDataToDataset(taDocumentDS, taDocument.TADocumentID);

			return taDocumentDS;
        }
        #endregion public DataSet PrepareDataToDataset(long documentID, bool isCopy)

        #region public Guid BeginTADocumentTransation()
        public Guid BeginTADocumentTransation()
        {
            DataSet ds = PrepareDS();
            Guid txID = TransactionService.Begin(ds);

            return txID;
        }
        #endregion public Guid BeginTADocumentTransation()

        #region public long AddTADocumentTransaction(Guid txID)
        public long AddTADocumentTransaction(Guid txID)
        {
            //Initial Data for another table.
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.DocumentRow documentRow = taDocumentDS.Document.NewDocumentRow(); //create new row to dataset.
            taDocumentDS.Document.AddDocumentRow(documentRow); //add new row to dataset.

            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.NewTADocumentRow();
            taDocumentRow.DocumentID = documentRow.DocumentID;
            taDocumentDS.TADocument.AddTADocumentRow(taDocumentRow);

            return taDocumentRow.TADocumentID;
        }
        #endregion public long AddTADocumentTransaction(Guid txID)

        #region public void UpdateTADocumentTransaction(Guid txID, TADocument taDocument)
        public void UpdateTADocumentTransaction(Guid txID, TADocument taDocument)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(taDocument.TADocumentID);            

            taDocumentRow.BeginEdit();       

            taDocumentRow.DocumentID = taDocument.DocumentID.DocumentID;

            if (taDocument.FromDate < DateTime.Today)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FromDateMoreOrEqualToDay"));
            }

            if (taDocument.FromDate > taDocument.ToDate)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FromDateLessOrEqualToDate"));
            }

            if (taDocument.FromDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FromDateIsRequired"));
            }
            else
            {
                taDocumentRow.FromDate = taDocument.FromDate;
            }

            if (taDocument.ToDate.Equals(DateTime.MinValue))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ToDateIsRequired"));
            }
            else
            {
                taDocumentRow.ToDate = taDocument.ToDate;
            }

            if (((!taDocument.IsBusinessPurpose) && (!taDocument.IsTrainningPurpose) && (!taDocument.IsOtherPurpose)))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PurposeIsRequired"));
            }
            else
            {
                taDocumentRow.IsBusinessPurpose = taDocument.IsBusinessPurpose;
                taDocumentRow.IsTrainningPurpose = taDocument.IsTrainningPurpose;
                taDocumentRow.IsOtherPurpose = taDocument.IsOtherPurpose;
            }

            if (taDocument.IsOtherPurpose && string.IsNullOrEmpty(taDocument.OtherPurposeDescription))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("OtherIsRequired"));
            }
            else
            {
                taDocumentRow.OtherPurposeDescription = taDocument.OtherPurposeDescription;
            }

            if ((!string.IsNullOrEmpty(taDocument.TravelBy)) && taDocument.TravelBy.Trim().ToUpper().Equals("D") && string.IsNullOrEmpty(taDocument.Province))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravelByProvinceIsRequired"));
            }
            else
            {
                taDocumentRow.Province = taDocument.Province;
            }

            if ((!string.IsNullOrEmpty(taDocument.TravelBy)) && taDocument.TravelBy.Trim().ToUpper().Equals("F") && string.IsNullOrEmpty(taDocument.Country))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravelByCountryIsRequired"));
            }
            else
            {
                taDocumentRow.Country = taDocument.Country;
            }

            if (string.IsNullOrEmpty(taDocument.TravelBy))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravelByIsRequired"));
            }
            else
            {
                taDocumentRow.TravelBy = taDocument.TravelBy;
            }

            if (string.IsNullOrEmpty(taDocument.Ticketing))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TicketingIsRequired"));
            }
            else
            {
                taDocumentRow.Ticketing = taDocument.Ticketing;            
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            taDocumentRow.Active = taDocument.Active;
            taDocumentRow.CreBy = UserAccount.UserID;
            taDocumentRow.CreDate = DateTime.Now;
            taDocumentRow.UpdBy = UserAccount.UserID;
            taDocumentRow.UpdDate = DateTime.Now;
            taDocumentRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentRow.EndEdit();
        }
        #endregion public void UpdateTADocumentTransaction(Guid txID, TADocument taDocument)

        #region public long SaveTADocument(Guid txID, long taDocumentID)
        public long SaveTADocument(Guid txID, long taDocumentID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(taDocumentID);
            long documentID = taDocumentRow.DocumentID;

            // Insert, Update SCGDocument.
            // Insert, Update, Delete DocumentInitiator.
            // Insert, Delete DocumentAttachment.
            documentID = SCGDocumentService.SaveSCGDocument(txID, documentID);

            // Insert, Update, Delete TADocument.
            taDocumentID = ScgeAccountingDaoProvider.TADocumentDao.Persist(taDocumentDS.TADocument);

            // Insert, Update, Delete TADocumentTraveller.
            TADocumentTravellerService.SaveTADocumentTraveller(txID, taDocumentID);

            // Insert, Update, Delete TADocumentSchedule.
            TADocumentScheduleService.SaveTADocumentSchedule(txID, taDocumentID);

            return taDocumentID;
        }
        #endregion public long SaveTADocument(Guid txID, long taDocumentID)

        #region public IList<object> GetVisibleFields(long? documentID)
        public IList<object> GetVisibleFields(long? documentID)
        {
            IList<object> visibleFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                visibleFields.Add(TAFieldGroup.All);
                visibleFields.Add(TAFieldGroup.Company);
                visibleFields.Add(TAFieldGroup.Initiator);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                visibleFields = WorkFlowService.GetVisibleFields(workFlow.WorkFlowID);

                //Because TA does not has special visible or editable field
                //Then return all
                //visibleFields.Add(TAFieldGroup.All);
            }

            return visibleFields;
        }
        #endregion public IList<object> GetVisibleFields(long? documentID)

        #region public IList<object> GetEditableFields(long? documentID)
        public IList<object> GetEditableFields(long? documentID)
        {
            IList<object> editableFields = new List<object>();

            if (!documentID.HasValue) // Check whether new flag then return the default editableFields.
            {
                editableFields.Add(TAFieldGroup.All);
                editableFields.Add(TAFieldGroup.Company);
                editableFields.Add(TAFieldGroup.Initiator);
            }
            else // Check whether view or edit flag then return editableFields from workflow state.
            {
                SS.Standard.WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID.Value);
                editableFields = WorkFlowService.GetEditableFields(workFlow.WorkFlowID);

                //Because TA does not has special visible or editable field
                //Then return all
                //editableFields.Add(TAFieldGroup.All);
            }

            return editableFields;
        }
        #endregion public IList<object> GetEditableFields(long? documentID)
    }

	#region public enum TAFieldGroup
	public enum TAFieldGroup
	{
		All,
        Initiator,
        Company
	}
	#endregion public enum TAFieldGroup
}
    