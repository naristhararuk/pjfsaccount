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
using log4net;
using SCG.DB.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public class FnRemittanceItemService : ServiceBase<FnRemittanceItem, long>, IFnRemittanceItemService
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(FnRemittanceItemService));

        #region Override Method
        public override IDao<FnRemittanceItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnRemittanceItemDao;
        }
        #endregion
        public IUserAccount UserAccount { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnRemittanceService FnRemittanceService { get; set; }
        public bool IsRepOffice { get; set; }


        public void PrepareDataToDataset(FnRemittanceDataset remittanceDataset, long remittanceID)
        {
            IList<FnRemittanceItem> remittanceItemList = ScgeAccountingQueryProvider.FnRemittanceItemQuery.FindRemittanceItemByRemittanceID(remittanceID);
            if (remittanceItemList.Count > 0)
            {
                foreach (FnRemittanceItem remittanceItem in remittanceItemList)
                {
                    FnRemittanceDataset.FnRemittanceItemRow remittanceItemRow = remittanceDataset.FnRemittanceItem.NewFnRemittanceItemRow();
                    remittanceItemRow.RemittanceItemID = remittanceItem.RemittanceItemID;
                    if (remittanceItem.Remittance != null)
                    {
                        remittanceItemRow.RemittanceID = remittanceItem.Remittance.RemittanceID;
                    }
                    remittanceItemRow.PaymentType = remittanceItem.PaymentType;
                    if (remittanceItem.Currency != null)
                    {
                        remittanceItemRow.CurrencyID = remittanceItem.Currency.CurrencyID;
                    }
                    remittanceItemRow.ForeignCurrencyAdvanced = remittanceItem.ForeignCurrencyAdvanced;
                    remittanceItemRow.ExchangeRate = remittanceItem.ExchangeRate;
                    remittanceItemRow.ForeignCurrencyRemitted = remittanceItem.ForeignCurrencyRemitted;
                    remittanceItemRow.AmountTHB = remittanceItem.AmountTHB;
                    remittanceItemRow.ExchangeRateTHB = remittanceItem.ExchangeRateTHB;
                    remittanceItemRow.ForeignAmountMainCurrencyAdvanced = remittanceItem.ForeignAmountMainCurrencyAdvanced;
                    remittanceItemRow.MainCurrencyAmount = remittanceItem.MainCurrencyAmount;
                    remittanceItemRow.IsImportFromAdvance = remittanceItem.IsImportFromAdvance;
                    remittanceItemRow.Active = remittanceItem.Active;
                    remittanceItemRow.CreBy = remittanceItem.CreBy;
                    remittanceItemRow.CreDate = remittanceItem.CreDate;
                    remittanceItemRow.UpdBy = remittanceItem.UpdBy;
                    remittanceItemRow.UpdDate = remittanceItem.UpdDate;
                    remittanceItemRow.UpdPgm = remittanceItem.UpdPgm;
                    remittanceItemRow.ForeignAmountTHBAdvanced = remittanceItem.ForeignAmountTHBAdvanced;
                    remittanceDataset.FnRemittanceItem.AddFnRemittanceItemRow(remittanceItemRow);
                }
            }
        }

        public long AddFnRemittanceItemTransaction(Guid txID, FnRemittanceItem fnRemittanceItem, bool isFromAddvance, double totalAdvance)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            FnRemittanceDataset fnRemittanceDocumentDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            DataRow[] dr;
            //FnRemittanceDataset.FnRemittanceRow remittanceRow = fnRemittanceDocumentDS.FnRemittance.FindByRemittanceID(fnRemittanceItem.Remittance.RemittanceID);
            if (fnRemittanceItem.Remittance.PB.Pbid != 0)
            {
                Dbpb dbPb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(fnRemittanceItem.Remittance.PB.Pbid));
                IsRepOffice = dbPb.RepOffice;
            }
            if (isFromAddvance)
            {
                string strSQL = " PaymentType = '{0}' AND CurrencyID = {1} ";
                dr = fnRemittanceDocumentDS.FnRemittanceItem.Select(string.Format(strSQL, fnRemittanceItem.PaymentType, fnRemittanceItem.Currency.CurrencyID));
            }
            else
            {
                if (fnRemittanceItem.Currency == null)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CurrencyIsRequire"));
                }
                if (fnRemittanceItem.ExchangeRate == 0 || fnRemittanceItem.ForeignCurrencyRemitted == 0)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExchangerateAndForeignCurrencyRemittedMustMoreThanZero"));
                }
                double totalRemittanceItemAmount = 0;
                foreach (FnRemittanceDataset.FnRemittanceItemRow remittanceItemRow in fnRemittanceDocumentDS.FnRemittanceItem.Select())
                {
                    if (!IsRepOffice)
                    {
                        totalRemittanceItemAmount += (double)Math.Round((decimal)remittanceItemRow.AmountTHB, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        totalRemittanceItemAmount += (double)Math.Round((decimal)remittanceItemRow.MainCurrencyAmount, 2, MidpointRounding.AwayFromZero);
                    }
                }
                if (!IsRepOffice)
                {
                    if (totalRemittanceItemAmount + fnRemittanceItem.AmountTHB > totalAdvance)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TotalRemittanceItemNotMoreThanTotalAdvance"));
                    }
                }
                else
                {
                    if (totalRemittanceItemAmount + fnRemittanceItem.MainCurrencyAmount > totalAdvance)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TotalRemittanceItemNotMoreThanTotalAdvance"));
                    }
                }
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                dr = fnRemittanceDocumentDS.FnRemittanceItem.Select(string.Format("CurrencyID = {0}", fnRemittanceItem.Currency.CurrencyID.ToString()));
            }
            if (dr.Length > 0)
            {
                if (isFromAddvance)
                {
                    dr[0]["ForeignAmountTHBAdvanced"] = Convert.ToDouble(dr[0]["ForeignAmountTHBAdvanced"]) + fnRemittanceItem.ForeignAmountTHBAdvanced;
                    dr[0]["ForeignCurrencyAdvanced"] = Convert.ToDouble(dr[0]["ForeignCurrencyAdvanced"]) + fnRemittanceItem.ForeignCurrencyAdvanced;

                    if (Convert.ToDouble(dr[0]["ExchangeRate"]).Equals(0))
                    {
                        dr[0]["ExchangeRate"] = fnRemittanceItem.ExchangeRate;
                    }
                    else
                    {
                        dr[0]["ExchangeRate"] = (Convert.ToDouble(dr[0]["ExchangeRate"]) + fnRemittanceItem.ExchangeRate) / 2;
                    }
                }
                else
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("DuplicateCurrency"));
                }
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                return Convert.ToInt64(dr[0]["RemittanceItemID"]);
            }
            else
            {
                FnRemittanceDataset.FnRemittanceItemRow fnRemittanceItemRow = fnRemittanceDocumentDS.FnRemittanceItem.NewFnRemittanceItemRow();

                if (fnRemittanceItem.Remittance != null)
                    fnRemittanceItemRow.RemittanceID = fnRemittanceItem.Remittance.RemittanceID;
                fnRemittanceItemRow.PaymentType = fnRemittanceItem.PaymentType;
                fnRemittanceItemRow.CurrencyID = fnRemittanceItem.Currency.CurrencyID;
                fnRemittanceItemRow.ForeignCurrencyAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
                fnRemittanceItemRow.ExchangeRate = (double)Math.Round((decimal)fnRemittanceItem.ExchangeRate, 5, MidpointRounding.AwayFromZero);
                fnRemittanceItemRow.ForeignCurrencyRemitted = (double)Math.Round((decimal)fnRemittanceItem.ForeignCurrencyRemitted, 2, MidpointRounding.AwayFromZero);
                fnRemittanceItemRow.ForeignAmountTHBAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignAmountTHBAdvanced, 2, MidpointRounding.AwayFromZero);
                fnRemittanceItemRow.ExchangeRateTHB = (double)Math.Round((decimal)fnRemittanceItem.ExchangeRateTHB, 5, MidpointRounding.AwayFromZero);
                fnRemittanceItemRow.ForeignAmountMainCurrencyAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignAmountMainCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
                
                if (isFromAddvance)
                {
                    fnRemittanceItemRow.AmountTHB = 0.00;
                    fnRemittanceItemRow.MainCurrencyAmount = 0.00;
                }
                else
                {
                    fnRemittanceItemRow.AmountTHB = (double)Math.Round((decimal)fnRemittanceItem.AmountTHB, 2, MidpointRounding.AwayFromZero);
                    fnRemittanceItemRow.MainCurrencyAmount = (double)Math.Round((decimal)fnRemittanceItem.MainCurrencyAmount, 2, MidpointRounding.AwayFromZero);
                }
                fnRemittanceItemRow.IsImportFromAdvance = isFromAddvance;
                fnRemittanceItemRow.Active = fnRemittanceItem.Active;
                fnRemittanceItemRow.CreBy = UserAccount.UserID;
                fnRemittanceItemRow.CreDate = DateTime.Now;
                fnRemittanceItemRow.UpdBy = UserAccount.UserID;
                fnRemittanceItemRow.UpdDate = DateTime.Now;
                fnRemittanceItemRow.UpdPgm = UserAccount.CurrentProgramCode;
                fnRemittanceDocumentDS.FnRemittanceItem.AddFnRemittanceItemRow(fnRemittanceItemRow);

                FnRemittanceService.UpdateTotalRemittanceAmount(txID, fnRemittanceItemRow.RemittanceID);

                return fnRemittanceItemRow.RemittanceItemID;
            }

        }
        public void UpdateRemittanceItemTransaction(Guid txtID, FnRemittanceItem fnRemittanceItem, bool isFromAddItemIngrid)
        {
            this.ValidateRemittanceItem(fnRemittanceItem, isFromAddItemIngrid);

            FnRemittanceDataset fnRemittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txtID);

            #region FnRemittanceItem
            FnRemittanceDataset.FnRemittanceItemRow fnRemittanceItemRow = fnRemittanceDS.FnRemittanceItem.FindByRemittanceItemID(fnRemittanceItem.RemittanceItemID);

            fnRemittanceItemRow.BeginEdit();
            if (fnRemittanceItem.Remittance != null)
                fnRemittanceItemRow.RemittanceID = fnRemittanceItem.Remittance.RemittanceID;
            fnRemittanceItemRow.PaymentType = fnRemittanceItem.PaymentType;
            fnRemittanceItemRow.CurrencyID = fnRemittanceItem.Currency.CurrencyID;
            fnRemittanceItemRow.ForeignCurrencyAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.ExchangeRate = (double)Math.Round((decimal)fnRemittanceItem.ExchangeRate, 5, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.ForeignCurrencyRemitted = (double)Math.Round((decimal)fnRemittanceItem.ForeignCurrencyRemitted, 2, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.ForeignAmountTHBAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignAmountTHBAdvanced, 2, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.AmountTHB = (double)Math.Round((decimal)fnRemittanceItem.AmountTHB, 2, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.ExchangeRateTHB = (double)Math.Round((decimal)fnRemittanceItem.ExchangeRateTHB, 5, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.ForeignAmountMainCurrencyAdvanced = (double)Math.Round((decimal)fnRemittanceItem.ForeignAmountMainCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
            fnRemittanceItemRow.MainCurrencyAmount = (double)Math.Round((decimal)fnRemittanceItem.MainCurrencyAmount, 2, MidpointRounding.AwayFromZero);
            // Change from default IsImportFromAdvance = true to use data from screen, prevent error by Anuwat S. on 08/05/2009
            fnRemittanceItemRow.IsImportFromAdvance = fnRemittanceItem.IsImportFromAdvance;
            fnRemittanceItemRow.Active = fnRemittanceItem.Active;
            fnRemittanceItemRow.CreBy = UserAccount.UserID;
            fnRemittanceItemRow.CreDate = DateTime.Now;
            fnRemittanceItemRow.UpdBy = UserAccount.UserID;
            fnRemittanceItemRow.UpdDate = DateTime.Now;
            fnRemittanceItemRow.UpdPgm = UserAccount.CurrentProgramCode;

            fnRemittanceItemRow.EndEdit();
            #endregion
        }

        public DataTable DeleteRemittanceItemFromTransaction(Guid txID, long fnremittrnceItemID, bool isFromAdvanceGrid)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            long remittanceId = 0;
            FnRemittanceDataset remittanceDS = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceItemRow remittanceItemRow = remittanceDS.FnRemittanceItem.FindByRemittanceItemID(fnremittrnceItemID);
            if (remittanceItemRow != null && !remittanceItemRow.IsNull("RemittanceID"))
                remittanceId = remittanceItemRow.RemittanceID;

            if (!isFromAdvanceGrid)
            {
                if (remittanceItemRow.IsImportFromAdvance)
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotDeleteItemInsertFromAdvance"));
                }

                if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            }
            remittanceItemRow.Delete();

            FnRemittanceService.UpdateTotalRemittanceAmount(txID, remittanceId);

            return remittanceDS.FnRemittanceItem;

        }

        public void UpdateRemittanceItemList(Guid txID, long remittanceId, List<FnRemittanceItem> remittanceItemList, bool isFullClearing)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            double totalDivAmount = 0;

            foreach (FnRemittanceItem item in remittanceItemList)
            {
                if (item.ForeignCurrencyAdvanced >= item.ForeignCurrencyRemitted)
                {
                    totalDivAmount += item.ForeignCurrencyAdvanced - item.ForeignCurrencyRemitted;
                }
                else
                {
                    totalDivAmount += item.ForeignCurrencyRemitted - item.ForeignCurrencyAdvanced;
                }
            }
            logger.Info("totalDivAmount" + totalDivAmount);
            foreach (FnRemittanceItem item in remittanceItemList)
            {
                if (totalDivAmount == 0 || isFullClearing)
                {
                    item.AmountTHB = item.ForeignAmountTHBAdvanced;
                }
                UpdateRemittanceItemTransaction(txID, item, !item.IsImportFromAdvance);
            }

            double sumRemittanceItemAmount = remittanceItemList.Sum(x => (double)Math.Round((decimal)x.AmountTHB, 2, MidpointRounding.AwayFromZero));
            double sumAdvanceAmount = remittanceItemList.Sum(x => (double)Math.Round((decimal)x.ForeignAmountTHBAdvanced, 2, MidpointRounding.AwayFromZero));
            logger.Info("TotalRemittance" + sumRemittanceItemAmount);
            logger.Info("TotalAdvance" + sumAdvanceAmount);
            if ((!isFullClearing && sumRemittanceItemAmount > sumAdvanceAmount) && totalDivAmount != 0)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TotalRemittanceItemNotMoreThanTotalAdvance"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            FnRemittanceService.UpdateTotalRemittanceAmount(txID, remittanceId);
        }        

        public void SaveRemittanceItem(Guid txID, long remittanceID)
        {
            FnRemittanceDataset ds = (FnRemittanceDataset)TransactionService.GetDS(txID);
            FnRemittanceDataset.FnRemittanceItemDataTable itemDt = new FnRemittanceDataset.FnRemittanceItemDataTable();
            foreach (FnRemittanceDataset.FnRemittanceItemRow row in ds.FnRemittanceItem.Select())
            {
                FnRemittanceDataset.FnRemittanceItemRow tempRow = itemDt.NewFnRemittanceItemRow();
                tempRow.RemittanceID = row.RemittanceID;
                tempRow.PaymentType = row.PaymentType;
                tempRow.CurrencyID = row.CurrencyID;
                tempRow.ForeignCurrencyAdvanced = (double)Math.Round((decimal)row.ForeignCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
                tempRow.ExchangeRate = (double)Math.Round((decimal)row.ExchangeRate, 5, MidpointRounding.AwayFromZero);
                tempRow.ForeignCurrencyRemitted = (double)Math.Round((decimal)row.ForeignCurrencyRemitted, 2, MidpointRounding.AwayFromZero);
                tempRow.ForeignAmountTHBAdvanced = (double)Math.Round((decimal)row.ForeignAmountTHBAdvanced, 2, MidpointRounding.AwayFromZero);
                tempRow.AmountTHB = (double)Math.Round((decimal)row.AmountTHB, 2, MidpointRounding.AwayFromZero);
                tempRow.ExchangeRateTHB = (double)Math.Round((decimal)row.ExchangeRateTHB, 5, MidpointRounding.AwayFromZero);
                tempRow.ForeignAmountMainCurrencyAdvanced = (double)Math.Round((decimal)row.ForeignAmountMainCurrencyAdvanced, 2, MidpointRounding.AwayFromZero);
                tempRow.MainCurrencyAmount = (double)Math.Round((decimal)row.MainCurrencyAmount, 2, MidpointRounding.AwayFromZero);
                tempRow.IsImportFromAdvance = row.IsImportFromAdvance;
                tempRow.Active = row.Active;
                tempRow.CreBy = row.CreBy;
                tempRow.CreDate = row.CreDate;
                tempRow.UpdBy = row.UpdBy;
                tempRow.UpdDate = row.UpdDate;
                tempRow.UpdPgm = row.UpdPgm;
                itemDt.AddFnRemittanceItemRow(tempRow);
            }

            //ScgeAccountingDaoProvider.FnRemittanceItemDao.Persist(ds.FnRemittanceItem);
            ScgeAccountingDaoProvider.FnRemittanceItemDao.Persist(itemDt);
        }

        #region ValidateRemittanceItem - Call This Function for Validation RemittanceItem
        public void ValidateRemittanceItem(FnRemittanceItem remittanceItem, bool isFromAddItemInGrid)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(remittanceItem.PaymentType))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RemittanceItem_Payment_IsRequired"));
            }
            if (remittanceItem.Currency == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RemittanceItem_Currency_IsRequired"));
            }
            //if (remittanceItem.ExchangeRate == 0)
            //{
            //    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RemittanceItem_ExchangeRate_IsRequired"));
            //}
            if (isFromAddItemInGrid)
            {
                if (remittanceItem.ForeignCurrencyRemitted <= 0)
                {
                    //errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RemittanceItem_ForrignCurrencyRemitted_IsRequired"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }
        #endregion
    }
}
