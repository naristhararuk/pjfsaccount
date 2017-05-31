using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;
using SCG.DB.DTO;
using SS.SU.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.Query;
using SS.DB.Query;
using SS.Standard.Security;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnRemittanceAdvanceService : ServiceBase<FnRemittanceAdvance, long>, IFnRemittanceAdvanceService
	{
		#region Override Method
        public override IDao<FnRemittanceAdvance, long> GetBaseDao()
		{
            return ScgeAccountingDaoProvider.FnRemittanceAdvanceDao;
		}
		#endregion
        public IUserAccount UserAccount { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }

        public void PrepareDataToDataset(FnRemittanceDataset remittanceDataset, long remittanceID)
        {
            IList<FnRemittanceAdvance> remittanceAdvanceList = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindRemittanceAdvanceByRemittanceID(remittanceID);
            if (remittanceAdvanceList.Count > 0)
            {
                foreach (FnRemittanceAdvance remittanceAdvance in remittanceAdvanceList)
                {
                    FnRemittanceDataset.FnRemittanceAdvanceRow remittanceAdvanceRow = remittanceDataset.FnRemittanceAdvance.NewFnRemittanceAdvanceRow();
                    remittanceAdvanceRow.RemittanceAdvanceID = remittanceAdvance.RemittanceAdvanceID;

                    if (remittanceAdvance.Remittance != null)
                    {
                        remittanceAdvanceRow.RemittanceID = remittanceAdvance.Remittance.RemittanceID;
                    }
                    if (remittanceAdvance.Advance != null)
                    {
                        remittanceAdvanceRow.AdvanceID = remittanceAdvance.Advance.AdvanceID;
                    }
                    remittanceAdvanceRow.Active = remittanceAdvance.Active;
                    remittanceAdvanceRow.CreBy = remittanceAdvance.CreBy;
                    remittanceAdvanceRow.CreDate = remittanceAdvance.CreDate;
                    remittanceAdvanceRow.UpdBy = remittanceAdvance.UpdBy;
                    remittanceAdvanceRow.UpdDate = remittanceAdvance.UpdDate;
                    remittanceAdvanceRow.UpdPgm = remittanceAdvance.UpdPgm;

                    remittanceDataset.FnRemittanceAdvance.AddFnRemittanceAdvanceRow(remittanceAdvanceRow);
                }
                
            }
        }
        public long AddFnRemittanceAdvanceTransaction(Guid txID, FnRemittanceAdvance fnRemittanceAdvance)
        {
            //Initial Data for another table.
            FnRemittanceDataset fnRemittanceDocumentDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceAdvanceRow fnRemittanceAdvanceRow = fnRemittanceDocumentDS.FnRemittanceAdvance.NewFnRemittanceAdvanceRow(); ;

            if (fnRemittanceAdvance.Remittance != null)
            {
                fnRemittanceAdvanceRow.RemittanceID = fnRemittanceAdvance.Remittance.RemittanceID;
            }
            if (fnRemittanceAdvance.Advance != null)
            {
                fnRemittanceAdvanceRow.AdvanceID = fnRemittanceAdvance.Advance.AdvanceID;
            }
            fnRemittanceAdvanceRow.Active = fnRemittanceAdvance.Active;
            fnRemittanceAdvanceRow.CreBy = UserAccount.UserID;
            fnRemittanceAdvanceRow.CreDate = DateTime.Now;
            fnRemittanceAdvanceRow.UpdBy = UserAccount.UserID;
            fnRemittanceAdvanceRow.UpdDate = DateTime.Now;
            fnRemittanceAdvanceRow.UpdPgm = UserAccount.CurrentProgramCode;

            fnRemittanceDocumentDS.FnRemittanceAdvance.AddFnRemittanceAdvanceRow(fnRemittanceAdvanceRow);

            return fnRemittanceAdvanceRow.RemittanceAdvanceID;
        }

        public void UpdateRemittanceAdvanceTransaction(Guid txtID, FnRemittanceAdvance fnRemittanceAdvance)
        {
            this.ValidateRemittanceAdvance(fnRemittanceAdvance);

            FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txtID);

            #region FnRemittanceAdvance
            FnRemittanceDataset.FnRemittanceAdvanceRow fnRemittanceAdvanceRow = fnRemittanceDS.FnRemittanceAdvance.FindByRemittanceAdvanceID(fnRemittanceAdvance.RemittanceAdvanceID);

            fnRemittanceAdvanceRow.BeginEdit();

            if (fnRemittanceAdvance.Remittance != null)
            {
                fnRemittanceAdvanceRow.RemittanceID = fnRemittanceAdvance.Remittance.RemittanceID;
            }
            if (fnRemittanceAdvance.Advance != null)
            {
                fnRemittanceAdvanceRow.AdvanceID = fnRemittanceAdvance.Advance.AdvanceID;
            }
            fnRemittanceAdvanceRow.Active = fnRemittanceAdvance.Active;
            fnRemittanceAdvanceRow.CreBy = UserAccount.UserID;
            fnRemittanceAdvanceRow.CreDate = DateTime.Now;
            fnRemittanceAdvanceRow.UpdBy = UserAccount.UserID;
            fnRemittanceAdvanceRow.UpdDate = DateTime.Now;
            fnRemittanceAdvanceRow.UpdPgm = UserAccount.CurrentProgramCode;

            fnRemittanceAdvanceRow.EndEdit();
            #endregion
        }

        public DataTable DeleteRemittanceAdvanceFromTransaction(Guid txID, long advanceID,long remittanceID)
        {
            FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceAdvanceDataTable remittanceAdvanceTable = fnRemittanceDS.FnRemittanceAdvance;
            foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in remittanceAdvanceTable.Select())
            {
                if (row.RemittanceID == remittanceID && row.AdvanceID == advanceID)
                {
                    row.Delete();
                    break;
                }
            }

            return remittanceAdvanceTable;
            //return fnRemittanceDS.FnRemittanceAdvance;
        }

        //public void InsertRemittanceAdvance(Guid txID, long tempfnRemittanceID)
        //{
        //    FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
        //    FnRemittance fnRemittance = ScgeAccountingQueryProvider.FnRemittanceQuery.FindProxyByIdentity(tempfnRemittanceID);
        //    FnRemittanceDataset.FnRemittanceAdvanceDataTable insertTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)fnRemittanceDS.FnRemittanceAdvance.GetChanges(DataRowState.Added);

        //    if (insertTable != null)
        //    {
        //        foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in insertTable.Rows)
        //        {
        //            FnRemittanceAdvance remittanceAdvance = new FnRemittanceAdvance();
        //            remittanceAdvance.Remittance = fnRemittance;
        //            remittanceAdvance.Advance = new AvAdvanceDocument(row.AdvanceID);
        //            remittanceAdvance.Active = row.Active;
        //            remittanceAdvance.CreBy = row.CreBy;
        //            remittanceAdvance.CreDate = row.CreDate;
        //            remittanceAdvance.UpdBy = row.UpdBy;
        //            remittanceAdvance.UpdDate = row.UpdDate;
        //            remittanceAdvance.UpdPgm = row.UpdPgm;

        //            // if row.BudgetCostElementID < 0 is new record that no data in database.
        //            if (row.RemittanceAdvanceID < 0)
        //            {
        //                ScgeAccountingDaoProvider.FnRemittanceAdvanceDao.Save(remittanceAdvance);
        //            }
        //        }
        //    }
        //}
        //public void UpdateRemittanceAdvance(Guid txID, long tempfnRemittanceID)
        //{
        //    FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
        //    FnRemittance fnRemittance = ScgeAccountingQueryProvider.FnRemittanceQuery.FindProxyByIdentity(tempfnRemittanceID);
        //    FnRemittanceDataset.FnRemittanceAdvanceDataTable updateTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)fnRemittanceDS.FnRemittanceAdvance.GetChanges(DataRowState.Modified);
           
        //    if (updateTable != null)
        //    {
        //        foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in updateTable.Rows)
        //        {
        //            FnRemittanceAdvance remittanceAdvance;
        //            if (row.RemittanceAdvanceID < 0)
        //            {
        //                remittanceAdvance = new FnRemittanceAdvance();
        //            }
        //            else
        //            {
        //                remittanceAdvance = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindProxyByIdentity(row.RemittanceAdvanceID);
        //            }
        //            remittanceAdvance.Remittance = fnRemittance;
        //            remittanceAdvance.Advance = new AvAdvanceDocument(row.AdvanceID);
        //            remittanceAdvance.Active = row.Active;
        //            remittanceAdvance.CreBy = row.CreBy;
        //            remittanceAdvance.CreDate = row.CreDate;
        //            remittanceAdvance.UpdBy = row.UpdBy;
        //            remittanceAdvance.UpdDate = row.UpdDate;
        //            remittanceAdvance.UpdPgm = row.UpdPgm;

        //            // if row.BudgetCostElementID < 0 is new record that no data in database.
        //            if (row.RemittanceAdvanceID < 0)
        //            {
        //                ScgeAccountingDaoProvider.FnRemittanceAdvanceDao.Save(remittanceAdvance);
        //            }
        //            else
        //            {
        //                ScgeAccountingDaoProvider.FnRemittanceAdvanceDao.SaveOrUpdate(remittanceAdvance);
        //            }
        //        }
        //    }
        //}
        //public void DeleteRemittanceAdvance(Guid txID)
        //{
        //    FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
        //    FnRemittanceDataset.FnRemittanceAdvanceDataTable deleteTable = (FnRemittanceDataset.FnRemittanceAdvanceDataTable)fnRemittanceDS.FnRemittanceAdvance.GetChanges(DataRowState.Deleted);
           
        //    if (deleteTable != null)
        //    {
        //        foreach (FnRemittanceDataset.FnRemittanceItemRow row in deleteTable.Rows)
        //        {
        //            long remittanceItemID = Convert.ToInt64(row["RemittanceItemID", DataRowVersion.Original].ToString());
        //            if (remittanceItemID > 0)
        //            {
        //                FnRemittanceItem remittanceItem = ScgeAccountingQueryProvider.FnRemittanceItemQuery.FindProxyByIdentity(remittanceItemID);
        //                if (remittanceItem != null)
        //                {
        //                    ScgeAccountingDaoProvider.FnRemittanceItemDao.Delete(remittanceItem);
        //                }
        //            }
        //        }
        //    }
        //}

        public void SaveRemittanceAdvance(Guid txID, long remittanceID)
        {
            FnRemittanceDataset ds = (FnRemittanceDataset)TransactionService.GetDS(txID);

            FnRemittanceDataset.FnRemittanceAdvanceDataTable tempAdvance = new FnRemittanceDataset.FnRemittanceAdvanceDataTable();
            foreach (FnRemittanceDataset.FnRemittanceAdvanceRow row in ds.FnRemittanceAdvance.Select())
            {
                FnRemittanceDataset.FnRemittanceAdvanceRow tempRow = tempAdvance.NewFnRemittanceAdvanceRow();
                tempRow.RemittanceID = row.RemittanceID;
                tempRow.AdvanceID = row.AdvanceID;
                tempRow.Active = row.Active;
                tempRow.CreBy = row.CreBy;
                tempRow.CreDate = row.CreDate;
                tempRow.UpdBy = row.UpdBy;
                tempRow.UpdDate = row.UpdDate;
                tempRow.UpdPgm = row.UpdPgm;
                tempAdvance.AddFnRemittanceAdvanceRow(tempRow);
            }

            //ScgeAccountingDaoProvider.FnRemittanceAdvanceDao.Persist(ds.FnRemittanceAdvance);
            ScgeAccountingDaoProvider.FnRemittanceAdvanceDao.Persist(tempAdvance);
        }

        #region ValidateRemittanceAdvance - Call this function to validation of FnremittanceAdvacne
        public void ValidateRemittanceAdvance(FnRemittanceAdvance remittanceAdvance)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            //if (remittanceAdvance.Advance.AdvanceID == 0)
            //{
            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TaDocument_Required"));
            //}

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        #endregion
    }
}
