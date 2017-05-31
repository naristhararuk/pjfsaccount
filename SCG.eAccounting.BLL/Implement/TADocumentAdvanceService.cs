using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using System.Data;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;

namespace SCG.eAccounting.BLL.Implement
{
    public class TADocumentAdvanceService : ServiceBase<TADocumentAdvance, int>, ITADocumentAdvanceService
    {
        #region local variable
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ITADocumentService TADocumentService { get; set; }
        #endregion local variable

        #region Overrid Method
        public override IDao<TADocumentAdvance, int> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.TADocumentAdvanceDao;
        }
        #endregion

        #region public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        {
            IList<TADocumentAdvance> taDocumentAdvanceList = ScgeAccountingQueryProvider.TADocumentAdvanceQuery.FindTADocumentAdvanceByTADocumentID(taDocumentID);

            foreach (TADocumentAdvance taDocumentAdvance in taDocumentAdvanceList)
            {
                TADocumentDataSet.TADocumentAdvanceRow taDocumentAdvanceRow = taDocumentDS.TADocumentAdvance.NewTADocumentAdvanceRow();

                taDocumentAdvanceRow.TADocumentAdvanceID = taDocumentAdvance.TADocumentAdvanceID;

                if (taDocumentAdvance.TADocument != null)
                {
                    taDocumentAdvanceRow.TADocumentID = taDocumentAdvance.TADocument.TADocumentID;
                }
                if (taDocumentAdvance.Advance != null)
                {
                    taDocumentAdvanceRow.AdvanceID = taDocumentAdvance.Advance.AdvanceID;
                }
                
                taDocumentAdvanceRow.Active = taDocumentAdvance.Active;
                taDocumentAdvanceRow.CreBy = taDocumentAdvance.CreBy;
                taDocumentAdvanceRow.CreDate = taDocumentAdvance.CreDate;
                taDocumentAdvanceRow.UpdBy = taDocumentAdvance.UpdBy;
                taDocumentAdvanceRow.UpdDate = taDocumentAdvance.UpdDate;
                taDocumentAdvanceRow.UpdPgm = taDocumentAdvance.UpdPgm;

                // Add document initiator to datatable taDocumentDS.
                taDocumentDS.TADocumentAdvance.AddTADocumentAdvanceRow(taDocumentAdvanceRow);
            }
        }
        #endregion public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)

        #region public int AddTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)
        public int AddTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentAdvanceRow taDocumentAdvanceRow = taDocumentDS.TADocumentAdvance.NewTADocumentAdvanceRow();

            taDocumentAdvanceRow.TADocumentID = taDocumentAdvance.TADocument.TADocumentID;
            if (taDocumentAdvance.Advance != null)
            {
                taDocumentAdvanceRow.AdvanceID = taDocumentAdvance.Advance.AdvanceID;
            }
            taDocumentAdvanceRow.Active = taDocumentAdvance.Active;
            taDocumentAdvanceRow.CreBy = UserAccount.UserID;
            taDocumentAdvanceRow.CreDate = DateTime.Now;
            taDocumentAdvanceRow.UpdBy = UserAccount.UserID;
            taDocumentAdvanceRow.UpdDate = DateTime.Now;
            taDocumentAdvanceRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentDS.TADocumentAdvance.AddTADocumentAdvanceRow(taDocumentAdvanceRow);
            return taDocumentAdvanceRow.TADocumentAdvanceID;
        }
        #endregion public int AddTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)

        #region public void UpdateTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)
        public void UpdateTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentAdvanceRow taDocumentAdvanceRow = taDocumentDS.TADocumentAdvance.FindByTADocumentAdvanceID(taDocumentAdvance.TADocumentAdvanceID);

            taDocumentAdvanceRow.BeginEdit();

            if (taDocumentAdvance.TADocument != null)
            {
                taDocumentAdvanceRow.TADocumentID = taDocumentAdvance.TADocument.TADocumentID;
            }

            if (taDocumentAdvance.Advance != null)
            {
                taDocumentAdvanceRow.AdvanceID = taDocumentAdvance.Advance.AdvanceID;
            }

            taDocumentAdvanceRow.Active = taDocumentAdvance.Active;
            taDocumentAdvanceRow.CreBy = UserAccount.UserID;
            taDocumentAdvanceRow.CreDate = DateTime.Now;
            taDocumentAdvanceRow.UpdBy = UserAccount.UserID;
            taDocumentAdvanceRow.UpdDate = DateTime.Now;
            taDocumentAdvanceRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentAdvanceRow.EndEdit();
        }
        #endregion public void UpdateTADocumentAdvanceTransaction(Guid txID, TADocumentAdvance taDocumentAdvance)

        #region public void SaveTADocumentAdvance(Guid txID, long taDocumentID)
        public void SaveTADocumentAdvance(Guid txID, long taDocumentID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);

            // Insert, Delete TADocumentTraveller.
            ScgeAccountingDaoProvider.TADocumentAdvanceDao.Persist(taDocumentDS.TADocumentAdvance);
        }
        #endregion public void SaveTADocumentAdvance(Guid txID, long taDocumentID)

        #region public DataTable DeleteTADocumentAdvanceTransaction(Guid txID, long taDocumentID , long AdvanceID)
        public void DeleteTADocumentAdvanceTransaction(Guid txID, long taDocumentID , long advanceID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentAdvanceDataTable taDocumentAdvanceDT = taDocumentDS.TADocumentAdvance;

            foreach (TADocumentDataSet.TADocumentAdvanceRow row in taDocumentAdvanceDT)
            {
                if (row.TADocumentID.Equals(taDocumentID) && row.AdvanceID.Equals(advanceID))
                {
                    row.Delete();
                    break;
                }
            }
        }
        #endregion public DataTable DeleteTADocumentAdvanceTransaction(Guid txID, long taDocumentID , long AdvanceID)
    }
}
