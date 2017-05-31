using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DAL;
using SS.Standard.Utilities;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.SU.DTO;
using System.Data;
using SS.Standard.Security;

namespace SCG.eAccounting.BLL.Implement
{
    public class TADocumentTravellerService : ServiceBase<TADocumentTraveller, int>, ITADocumentTravellerService
    {
        #region local variable
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ITADocumentService TADocumentService { get; set; }
        #endregion local variable

        #region Overrid Method
        public override IDao<TADocumentTraveller, int> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.TADocumentTravellerDao;
        }
        #endregion

        #region public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        {
            IList<TADocumentTraveller> taDocumentTravellerList = ScgeAccountingQueryProvider.TADocumentTravellerQuery.FindTADocumentTravellerByTADocumentID(taDocumentID);
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(taDocumentID); 

            foreach (TADocumentTraveller taDocumentTraveller in taDocumentTravellerList.OrderBy(t=> t.TravellerID))
            {
                TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow = taDocumentDS.TADocumentTraveller.NewTADocumentTravellerRow();
                
                taDocumentTravellerRow.TravellerID = taDocumentTraveller.TravellerID;

                if (taDocumentTraveller.TADocumentID != null)
                {
                    taDocumentTravellerRow.TADocumentID = taDocumentTraveller.TADocumentID.TADocumentID;
                }
                if (taDocumentTraveller.UserID != null)
                {
                    taDocumentTravellerRow.UserID = taDocumentTraveller.UserID.Userid;
                }
                if (taDocumentTraveller.EmployeeNameEng != null)
                {
                    taDocumentTravellerRow.EmployeeNameEng = taDocumentTraveller.EmployeeNameEng;
                }
                if (taDocumentTraveller.AirLineMember != null)
                {
                    taDocumentTravellerRow.AirLineMember = taDocumentTraveller.AirLineMember;
                }
                if (taDocumentTraveller.CostCenterID != null)
                {
                    taDocumentTravellerRow.CostCenterID = taDocumentTraveller.CostCenterID.CostCenterID;
                }
                if (taDocumentTraveller.Account != null) 
                {
                    taDocumentTravellerRow.AccountID = taDocumentTraveller.Account.AccountID;
                }

                if (taDocumentTraveller.IOID != null)
                {
                    taDocumentTravellerRow.IOID = taDocumentTraveller.IOID.IOID;
                }

                taDocumentTravellerRow.Active = taDocumentTraveller.Active;
                taDocumentTravellerRow.CreBy = taDocumentTraveller.CreBy;
                taDocumentTravellerRow.CreDate = taDocumentTraveller.CreDate;
                taDocumentTravellerRow.UpdBy = taDocumentTraveller.UpdBy;
                taDocumentTravellerRow.UpdDate = taDocumentTraveller.UpdDate;
                taDocumentTravellerRow.UpdPgm = taDocumentTraveller.UpdPgm;

                // Add document initiator to datatable taDocumentDS.
                taDocumentDS.TADocumentTraveller.AddTADocumentTravellerRow(taDocumentTravellerRow);
            }
        }
        #endregion public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)

        #region public int AddTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)
        public int AddTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow = taDocumentDS.TADocumentTraveller.NewTADocumentTravellerRow();

            taDocumentTravellerRow.TADocumentID = taDocumentTraveller.TADocumentID.TADocumentID;
            taDocumentTravellerRow.UserID = taDocumentTraveller.UserID.Userid;
            taDocumentTravellerRow.EmployeeNameEng = taDocumentTraveller.EmployeeNameEng;
            taDocumentTravellerRow.AirLineMember = taDocumentTraveller.AirLineMember;
            taDocumentTravellerRow.Active = taDocumentTraveller.Active;
            taDocumentTravellerRow.CreBy = UserAccount.UserID;
            taDocumentTravellerRow.CreDate = DateTime.Now;
            taDocumentTravellerRow.UpdBy = UserAccount.UserID;
            taDocumentTravellerRow.UpdDate = DateTime.Now;
            taDocumentTravellerRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentDS.TADocumentTraveller.AddTADocumentTravellerRow(taDocumentTravellerRow);
            return taDocumentTravellerRow.TravellerID;
        }
        #endregion public int AddTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)

        #region public void UpdateTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)
        public void UpdateTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow = taDocumentDS.TADocumentTraveller.FindByTravellerID(taDocumentTraveller.TravellerID);
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(taDocumentTraveller.TADocumentID.TADocumentID);

            taDocumentTravellerRow.BeginEdit();

            if (taDocumentTraveller.TADocumentID != null)
            {
                taDocumentTravellerRow.TADocumentID = taDocumentTraveller.TADocumentID.TADocumentID;
            }

            if (taDocumentTraveller.UserID == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("UserIDIsRequired"));
            }
            else
            {
                taDocumentTravellerRow.UserID = taDocumentTraveller.UserID.Userid;
            }

            if (string.IsNullOrEmpty(taDocumentTraveller.EmployeeNameEng))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("FullNameEngIsRequired"));
            }
            else
            {
                taDocumentTravellerRow.EmployeeNameEng = taDocumentTraveller.EmployeeNameEng;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            taDocumentTravellerRow.AirLineMember = taDocumentTraveller.AirLineMember;

            taDocumentTravellerRow.Active = taDocumentTraveller.Active;

            if (taDocumentRow.Ticketing.Trim().ToUpper().Equals("T") && (!string.IsNullOrEmpty(taDocumentRow.Ticketing)))
            {

                if (taDocumentTraveller.CostCenterID == null || taDocumentTraveller.CostCenterID.CostCenterID <= 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CostCenterIsRequired", new object[] { taDocumentTraveller.UserName , " " }));
                }
                else
                {
                    taDocumentTravellerRow.CostCenterID = taDocumentTraveller.CostCenterID.CostCenterID;
                }

                if (taDocumentTraveller.IOID == null || taDocumentTraveller.IOID.IOID <= 0)
                {
                    taDocumentTravellerRow.IOID = 0;
                }
                else
                {
                    taDocumentTravellerRow.IOID = taDocumentTraveller.IOID.IOID;
                }

                if (taDocumentTraveller.Account == null || taDocumentTraveller.Account.AccountID <= 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExpenseGroupIsRequired", new object[] { taDocumentTraveller.UserName, " " }));
                }
                else
                {
                    taDocumentTravellerRow.AccountID = taDocumentTraveller.Account.AccountID;
                }
            }
            else {
                taDocumentTravellerRow.CostCenterID = 0;
                taDocumentTravellerRow.AccountID = 0;
                taDocumentTravellerRow.IOID = 0;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            taDocumentTravellerRow.CreBy = UserAccount.UserID;
            taDocumentTravellerRow.CreDate = DateTime.Now;
            taDocumentTravellerRow.UpdBy = UserAccount.UserID;
            taDocumentTravellerRow.UpdDate = DateTime.Now;
            taDocumentTravellerRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentTravellerRow.EndEdit();
        }
        #endregion public void UpdateTADocumentTravellerTransaction(Guid txID, TADocumentTraveller taDocumentTraveller)

        #region public void SaveTADocumentTraveller(Guid txID, long taDocumentID)
        public void SaveTADocumentTraveller(Guid txID, long taDocumentID)
		{
			TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
			
			// Insert, Delete TADocumentTraveller.
			ScgeAccountingDaoProvider.TADocumentTravellerDao.Persist(taDocumentDS.TADocumentTraveller);
        }
        #endregion public void SaveTADocumentTraveller(Guid txID, long taDocumentID)

        #region public DataTable DeleteTADocumentTravellerTransaction(Guid txID, int travellerID)
        public DataTable DeleteTADocumentTravellerTransaction(Guid txID, int travellerID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow = taDocumentDS.TADocumentTraveller.FindByTravellerID(travellerID);
            taDocumentTravellerRow.Delete();

            return taDocumentDS.TADocumentTraveller;
        }
        #endregion public DataTable DeleteTADocumentTravellerTransaction(Guid txID, int travellerID)

        public void ChangeRequesterTraveller(Guid txID, TADocumentTraveller taDocumentTraveller)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow = taDocumentDS.TADocumentTraveller.FindByTravellerID(taDocumentTraveller.TravellerID);

            taDocumentTravellerRow.BeginEdit();

            taDocumentTravellerRow.TADocumentID = taDocumentTraveller.TADocumentID.TADocumentID;
            taDocumentTravellerRow.UserID = taDocumentTraveller.UserID.Userid;
            taDocumentTravellerRow.EmployeeNameEng = taDocumentTraveller.EmployeeNameEng;
            taDocumentTravellerRow.AirLineMember = taDocumentTraveller.AirLineMember;

            if (taDocumentTraveller.CostCenterID != null)
            {
                taDocumentTravellerRow.CostCenterID = taDocumentTraveller.CostCenterID.CostCenterID;
            }

            if (taDocumentTraveller.Account != null)
            {
                taDocumentTravellerRow.AccountID = taDocumentTraveller.Account.AccountID;
            }

            if (taDocumentTraveller.IOID != null)
            {
                taDocumentTravellerRow.IOID = taDocumentTraveller.IOID.IOID;
            }

            taDocumentTravellerRow.UpdBy = UserAccount.UserID;
            taDocumentTravellerRow.UpdDate = DateTime.Now;
            taDocumentTravellerRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentTravellerRow.EndEdit();
        }
    }
}
