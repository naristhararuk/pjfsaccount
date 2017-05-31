using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;

using SCG.eAccounting.BLL;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;

using SS.Standard.Utilities;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.BLL.Implement
{
    public class AvAdvanceItemService : ServiceBase<AvAdvanceItem, long>, IAvAdvanceItemService
    {
        #region Overrid Method
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public override IDao<AvAdvanceItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.AvAdvanceItemDao;
        }
        #endregion

        public void PrepareDataToDataset(AdvanceDataSet advanceDs, long advanceID, bool isCopy)
        {
            IList<AvAdvanceItem> avAdvanceItemList = ScgeAccountingQueryProvider.AvAdvanceItemQuery.FindByAvAdvanceItemAdvanceID(advanceID);

            // Set data to document AvAdvanceItem datatable.
            foreach (AvAdvanceItem avAdvanceItem in avAdvanceItemList)
            {
                AdvanceDataSet.AvAdvanceItemRow itemRow = advanceDs.AvAdvanceItem.NewAvAdvanceItemRow();
                itemRow.AdvanceItemID = avAdvanceItem.AdvanceItemID;
                if (avAdvanceItem.AdvanceID != null)
                {
                    itemRow.AdvanceID = avAdvanceItem.AdvanceID.AdvanceID;
                }

                itemRow.PaymentType = avAdvanceItem.PaymentType;
                

                if (avAdvanceItem.CurrencyID != null)
                {
                    itemRow.CurrencyID = avAdvanceItem.CurrencyID.CurrencyID;
                }
                itemRow.Amount = (decimal)avAdvanceItem.Amount;
                itemRow.ExchangeRate = (decimal)avAdvanceItem.ExchangeRate;
                itemRow.AmountTHB = (decimal)avAdvanceItem.AmountTHB;
                itemRow.Active = avAdvanceItem.Active;
                itemRow.CreBy = UserAccount.UserID;
                itemRow.CreDate = DateTime.Now;
                itemRow.UpdBy = UserAccount.UserID;
                itemRow.UpdDate = DateTime.Now;
                itemRow.UpdPgm = UserAccount.CurrentProgramCode;

                
                if (avAdvanceItem.ExchangeRateTHB.HasValue)
                {
                    itemRow.ExchangeRateTHB = (decimal)avAdvanceItem.ExchangeRateTHB;
                }

                if (!isCopy)
                {
                    if (avAdvanceItem.MainCurrencyAmount.HasValue)
                    {
                        itemRow.MainCurrencyAmount = (decimal)avAdvanceItem.MainCurrencyAmount;
                    }
                }
                // Add document initiator to datatable advanceDs.
                advanceDs.AvAdvanceItem.AddAvAdvanceItemRow(itemRow);
            }
        }

        #region IAvAdvanceItemService Members
        #region long AddAdvanceItem(Guid TxID, AvAdvanceItem avAdvanceItem)
        public long AddAdvanceItem(Guid TxID, AvAdvanceItem avAdvanceItem)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            AdvanceDataSet advanceDs = (AdvanceDataSet)TransactionService.GetDS(TxID);
            //AdvanceDataSet.AvAdvanceItemRow row = advanceDs.AvAdvanceItem.NewAvAdvanceItemRow();
            AdvanceDataSet.AvAdvanceDocumentRow advRow = advanceDs.AvAdvanceDocument.FindByAdvanceID(avAdvanceItem.AdvanceID.AdvanceID);
            // Validate Payment Type is not null.
            if (string.IsNullOrEmpty(avAdvanceItem.PaymentType))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Payment Type is Required."));
            }
            //// Validate Currency is not null.
            if (advRow.AdvanceType.Equals(ZoneType.Foreign))
            {
                if (avAdvanceItem.CurrencyID == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Currency is Required."));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            AdvanceDataSet.AvAdvanceItemRow row = advanceDs.AvAdvanceItem.NewAvAdvanceItemRow();
            row.AdvanceID = avAdvanceItem.AdvanceID.AdvanceID;
            row.PaymentType = avAdvanceItem.PaymentType;
            if (avAdvanceItem.CurrencyID != null)
                row.CurrencyID = avAdvanceItem.CurrencyID.CurrencyID;
            if (avAdvanceItem.Amount != 0)
                row.Amount = Math.Round((decimal)avAdvanceItem.Amount, 2, MidpointRounding.AwayFromZero);
            if (avAdvanceItem.ExchangeRate != 0)
                row.ExchangeRate = Math.Round((decimal)avAdvanceItem.ExchangeRate, 5, MidpointRounding.AwayFromZero);
            row.AmountTHB = Math.Round((decimal)avAdvanceItem.AmountTHB, 2, MidpointRounding.AwayFromZero);
            row.Active = avAdvanceItem.Active;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;

            if (avAdvanceItem.ExchangeRateTHB.HasValue)
            {
                row.ExchangeRateTHB = (decimal)avAdvanceItem.ExchangeRateTHB;
            }
            else
            {
                row.SetExchangeRateTHBNull();
            }

            advanceDs.AvAdvanceItem.AddAvAdvanceItemRow(row);
            return row.AdvanceItemID;
        }
        #endregion

        #region void UpdateAdvanceItemTransaction(Guid txID, AvAdvanceItem avAdvanceItem)
        public void UpdateAdvanceItemTransaction(Guid txID, AvAdvanceItem avAdvanceItem, bool isRepOffice)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);
            AdvanceDataSet.AvAdvanceDocumentRow advRow = advanceDS.AvAdvanceDocument.FindByAdvanceID(avAdvanceItem.AdvanceID.AdvanceID);
            AdvanceDataSet.AvAdvanceItemRow advanceItemRow = advanceDS.AvAdvanceItem.FindByAdvanceItemID(avAdvanceItem.AdvanceItemID);

            advanceItemRow.BeginEdit();

            //// Validate Currency is not null.
            if (advRow.AdvanceType.Equals(ZoneType.Foreign))
            {
                //if (avAdvanceItem.CurrencyID == null)
                //{
                //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CurrencyIsRequired"));
                //}
                if (isRepOffice)
                {
                    if (avAdvanceItem.Amount == 0)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountIsRequired"));
                    }
                }
            }
            else
            {
                // Validate AmountTHB is not null.
                if (avAdvanceItem.AmountTHB == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("AmountTHBIsRequired"));
                }
                // Validate Amount is not null.
                if (avAdvanceItem.PaymentType == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PaymentTypeIsRequired"));
                }
            }
            if (avAdvanceItem.AdvanceID != null)
            {
                advanceItemRow.AdvanceID = avAdvanceItem.AdvanceID.AdvanceID;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            //Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(advRow.PBID);

            advanceItemRow.PaymentType = avAdvanceItem.PaymentType;
            if (advRow.AdvanceType.Equals(ZoneType.Foreign) || isRepOffice)
            {
                advanceItemRow.CurrencyID = avAdvanceItem.CurrencyID.CurrencyID;
            }
            advanceItemRow.Amount = Math.Round((decimal)avAdvanceItem.Amount, 2, MidpointRounding.AwayFromZero);
            advanceItemRow.ExchangeRate = Math.Round((decimal)avAdvanceItem.ExchangeRate, 5, MidpointRounding.AwayFromZero);
            advanceItemRow.AmountTHB = Math.Round((decimal)avAdvanceItem.AmountTHB, 2, MidpointRounding.AwayFromZero);
            advanceItemRow.Active = avAdvanceItem.Active;
            advanceItemRow.CreBy = UserAccount.UserID;
            advanceItemRow.CreDate = DateTime.Now;
            advanceItemRow.UpdBy = UserAccount.UserID;
            advanceItemRow.UpdDate = DateTime.Now;
            advanceItemRow.UpdPgm = UserAccount.CurrentProgramCode;
            if (avAdvanceItem.ExchangeRateTHB.HasValue)
            {
                advanceItemRow.ExchangeRateTHB = (decimal)avAdvanceItem.ExchangeRateTHB;
            }

            if (avAdvanceItem.MainCurrencyAmount.HasValue)
            {
                advanceItemRow.MainCurrencyAmount = (decimal)avAdvanceItem.MainCurrencyAmount;
            }
            advanceItemRow.EndEdit();

        }
        #endregion
        #region void DeleteAdvanceDocument(Guid txID)
        public void DeleteAdvanceDocument(Guid txID)
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);
            AdvanceDataSet.AvAdvanceItemDataTable deleteTable =
                (AdvanceDataSet.AvAdvanceItemDataTable)advanceDS.AvAdvanceItem.GetChanges(DataRowState.Deleted);

            if (deleteTable != null)
            {
                foreach (AdvanceDataSet.AvAdvanceItemRow row in deleteTable.Rows)
                {
                    long advanceItemID = Convert.ToInt64(row["AdvanceItemID", DataRowVersion.Original].ToString());
                    if (advanceItemID > 0)
                    {
                        AvAdvanceItem advanceItem = ScgeAccountingDaoProvider.AvAdvanceItemDao.FindProxyByIdentity(advanceItemID);
                        if (advanceItem != null)
                        {
                            ScgeAccountingDaoProvider.AvAdvanceItemDao.Delete(advanceItem);
                        }
                    }
                }
            }
        }
        #endregion
        #region void InsertAvAdvanceItem(Guid txID, long advanceID)
        public void InsertAvAdvanceItem(Guid txID, long advanceID)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);

            AvAdvanceDocument avDocument = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.FindProxyByIdentity(advanceID);
            AdvanceDataSet.AvAdvanceItemDataTable insertTable =
                (AdvanceDataSet.AvAdvanceItemDataTable)advanceDS.AvAdvanceItem.GetChanges(DataRowState.Added);

            if (insertTable != null)
            {
                foreach (AdvanceDataSet.AvAdvanceItemRow row in insertTable.Rows)
                {
                    AvAdvanceItem advanceItem = new AvAdvanceItem();
                    advanceItem.AdvanceID = avDocument;

                    if (!row.IsNull(advanceDS.AvAdvanceItem.PaymentTypeColumn.ColumnName))
                    {
                        advanceItem.PaymentType = row.PaymentType;
                    }
                    if (!row.IsNull(advanceDS.AvAdvanceItem.CurrencyIDColumn.ColumnName))
                    {
                        advanceItem.CurrencyID = new SS.DB.DTO.DbCurrency(row.CurrencyID);
                    }
                    if (!row.IsNull(advanceDS.AvAdvanceItem.AmountColumn.ColumnName))
                    {
                        advanceItem.Amount = (double)Math.Round(row.Amount, 2, MidpointRounding.AwayFromZero);
                    }
                    if (!row.IsNull(advanceDS.AvAdvanceItem.ExchangeRateColumn.ColumnName))
                    {
                        advanceItem.ExchangeRate = (double)Math.Round(row.ExchangeRate, 5, MidpointRounding.AwayFromZero);
                    }
                    if (!row.IsNull(advanceDS.AvAdvanceItem.AmountTHBColumn.ColumnName))
                    {
                        advanceItem.AmountTHB = (double)Math.Round(row.AmountTHB, 2, MidpointRounding.AwayFromZero);
                    }
                    advanceItem.Active = row.Active;
                    advanceItem.CreBy = UserAccount.UserID;
                    advanceItem.CreDate = DateTime.Now;
                    advanceItem.UpdBy = UserAccount.UserID;
                    advanceItem.UpdDate = DateTime.Now;
                    advanceItem.UpdPgm = UserAccount.CurrentProgramCode;

                    // if row.ScheduleID < 0 is new record that no data in database.
                    if (row.AdvanceItemID < 0)
                    {
                        ScgeAccountingDaoProvider.AvAdvanceItemDao.Save(advanceItem);
                    }
                }
            }
        }
        #endregion
        #region void updateGrid(Guid txID,AvAdvanceItem avAdvanceItem)
        public void updateGrid(Guid txID, AvAdvanceItem avAdvanceItem)
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);
            AdvanceDataSet.AvAdvanceItemRow advanceItemRow = advanceDS.AvAdvanceItem.FindByAdvanceItemID(avAdvanceItem.AdvanceItemID);
            advanceItemRow.BeginEdit();
            advanceItemRow.Amount = Math.Round((decimal)avAdvanceItem.Amount, 2, MidpointRounding.AwayFromZero);
            advanceItemRow.AmountTHB = Math.Round((decimal)avAdvanceItem.AmountTHB, 2, MidpointRounding.AwayFromZero);
            advanceItemRow.ExchangeRate = Math.Round((decimal)avAdvanceItem.ExchangeRate, 5, MidpointRounding.AwayFromZero);
            advanceItemRow.EndEdit();
        }
        #endregion
        #endregion

        #region saveAdvanceItem(Guid txID, long advanceid)
        public void saveAdvanceItem(Guid txID, long advanceid)
        {
            AdvanceDataSet ds = (AdvanceDataSet)TransactionService.GetDS(txID);
            //insert update delete avadvanceITem
            ScgeAccountingDaoProvider.AvAdvanceItemDao.Persist(ds.AvAdvanceItem);
        }
        #endregion

        public DataTable DeleteAdvanceItemFromTransaction(Guid txID, long advanceItemID)
        {
            AdvanceDataSet advanceDS = (AdvanceDataSet)TransactionService.GetDS(txID);
            AdvanceDataSet.AvAdvanceItemRow advanceItemRow = advanceDS.AvAdvanceItem.FindByAdvanceItemID(advanceItemID);
            advanceItemRow.Delete();
            return advanceDS.AvAdvanceItem;

        }
    }
}
