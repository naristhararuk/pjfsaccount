using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;
using SCG.eAccounting.DTO.DataSet;
using System.Web;
using SCG.eAccounting.Query;
using SCG.DB.Query;
using SS.Standard.Security;
using Spring.Transaction.Interceptor;
using SCG.DB.DTO;
using SS.DB.Query;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseInvoiceItemService : ServiceBase<FnExpenseInvoiceItem, long>, IFnExpenseInvoiceItemService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }

        public override IDao<FnExpenseInvoiceItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseInvoiceItemDao;
        }
        public void AddInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId, string expenseType)
        {
            bool isRepOffice = false;
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = null;
            long documentId = 0;
            if (item.Invoice != null && item.Invoice.Expense != null)
            {
                expRow = ds.FnExpenseDocument.FindByExpenseID(item.Invoice.Expense.ExpenseID);
                documentId = expRow == null ? 0 : expRow.DocumentID;
                if (!expRow.IsIsRepOfficeNull())
                    isRepOffice = expRow.IsRepOffice;
            }

            // Validate CostCenter.
            if (item.CostCenter == null)
            {
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            }
            else
            {
                if (ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(item.CostCenter.CostCenterID) == null)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
                }
            }

            // Validate Account.
            if (item.Account == null)
            {
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
            }
            else
            {
                if (ScgDbQueryProvider.DbAccountQuery.FindByIdentity(item.Account.AccountID) == null)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
                }
            }

            if (!string.IsNullOrEmpty(item.SaleOrder) && item.SaleOrder.Length < 10)
            {
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("SaleOrder_Has_10_Digit"));
            }

            if (expenseType.Equals(ZoneType.Domestic))
            {
                // Validate Amount.
                if (!isRepOffice)
                {
                    if (!item.Amount.HasValue || (item.Amount.HasValue && item.Amount.Value == 0))
                    {
                        errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                    }
                }
                else
                {
                    if (!item.LocalCurrencyAmount.HasValue || (item.LocalCurrencyAmount.HasValue && item.LocalCurrencyAmount.Value == 0))
                    {
                        errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                    }
                }
            }
            else if (expenseType.Equals(ZoneType.Foreign))
            {
                // Validate Currency.
                if (!item.CurrencyID.HasValue || (item.CurrencyID.HasValue && item.CurrencyID.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCurrency"));
                }
                // Validate Currency Amount.
                if (!item.CurrencyAmount.HasValue || (item.CurrencyAmount.HasValue && item.CurrencyAmount.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                }

                // Validate ExchangeRate.
                if (!item.ExchangeRate.HasValue)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredExchangeRate"));
                }
                else if ((item.ExchangeRate.HasValue) && (item.ExchangeRate.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredExchangeRate"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();

            if (item.Invoice != null)
            {
                row.InvoiceID = item.Invoice.InvoiceID;
            }

            if (item.CostCenter != null)
                row.CostCenterID = item.CostCenter.CostCenterID;
            else
                row.SetCostCenterIDNull();

            row.AccountID = item.Account.AccountID;

            if (item.IO != null) row.IOID = item.IO.IOID;

            if (item.CurrencyID.HasValue)
                row.CurrencyID = item.CurrencyID.Value;

            if (item.ExchangeRate.HasValue)
                row.ExchangeRate = item.ExchangeRate.Value;

            if (item.CurrencyAmount.HasValue)
                row.CurrencyAmount = item.CurrencyAmount.Value;

            if (item.LocalCurrencyAmount.HasValue)
                row.LocalCurrencyAmount = (decimal)item.LocalCurrencyAmount.Value;

            if (item.MainCurrencyAmount.HasValue)
                row.MainCurrencyAmount = item.MainCurrencyAmount.Value;

            if (expenseType.Equals(ZoneType.Foreign) && !isRepOffice)
            {
                row.Amount = (double)Math.Round((decimal)(item.CurrencyAmount.Value * item.ExchangeRate.Value), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (item.Amount.HasValue)
                    row.Amount = item.Amount.Value;
            }

            row.SaleOrder = item.SaleOrder;
            row.SaleItem = item.SaleItem;
            row.Description = item.Description;
            row.ReferenceNo = item.ReferenceNo;

            row.Active = true;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;

            ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }
        public void UpdateInvoiceItemOnTransaction(FnExpenseInvoiceItem item, Guid txId, string expenseType)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            bool isRepOffice = false;
            long documentId = 0;
            if (item.Invoice != null && item.Invoice.Expense != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(item.Invoice.Expense.ExpenseID);
                documentId = expRow == null ? 0 : expRow.DocumentID;
                if (!expRow.IsIsRepOfficeNull())
                    isRepOffice = expRow.IsRepOffice;
            }
            SS.Standard.WorkFlow.DTO.WorkFlow workflow = SS.Standard.WorkFlow.Query.WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentId);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (workflow == null || (workflow != null && workflow.CurrentState.Name.Equals(WorkFlowStateFlag.Draft)))
            {
                // Validate CostCenter.
                if (item.CostCenter == null)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
                }
                else
                {
                    if (ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(item.CostCenter.CostCenterID) == null)
                    {
                        errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
                    }
                }
            }
            // Validate Account.
            if (item.Account == null)
            {
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
            }
            else
            {
                if (ScgDbQueryProvider.DbAccountQuery.FindByIdentity(item.Account.AccountID) == null)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
                }
            }

            if (!string.IsNullOrEmpty(item.SaleOrder) && item.SaleOrder.Length < 10)
            {
                errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("SaleOrder_Has_10_Digit"));
            }

            if (expenseType.Equals(ZoneType.Domestic))
            {
                // Validate Amount.
                if (!isRepOffice)
                {
                    if (!item.Amount.HasValue || (item.Amount.HasValue && item.Amount.Value == 0))
                    {
                        errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                    }
                }
                else
                {
                    if (!item.LocalCurrencyAmount.HasValue || (item.LocalCurrencyAmount.HasValue && item.LocalCurrencyAmount.Value == 0))
                    {
                        errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                    }
                }
            }
            else if (expenseType.Equals(ZoneType.Foreign))
            {
                // Validate Currency.
                if (!item.CurrencyID.HasValue || (item.CurrencyID.HasValue && item.CurrencyID.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredCurrency"));
                }
                // Validate Amount.
                if (!item.CurrencyAmount.HasValue || (item.CurrencyAmount.HasValue && item.CurrencyAmount.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredAmount"));
                }

                // Validate ExchangeRate.
                if (!item.ExchangeRate.HasValue)
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredExchangeRate"));
                }
                else if ((item.ExchangeRate.HasValue) && (item.ExchangeRate.Value == 0))
                {
                    errors.AddError("InvoiceItem.Error", new Spring.Validation.ErrorMessage("RequiredExchangeRate"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            //ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.FindByInvoiceItemID(item.InvoiceItemID);

            row.InvoiceID = item.Invoice.InvoiceID;
            if (item.CostCenter != null)
                row.CostCenterID = item.CostCenter.CostCenterID;
            else
                row.SetCostCenterIDNull();

            row.AccountID = item.Account.AccountID;

            if (item.IO != null)
                row.IOID = item.IO.IOID;
            else
                row.SetIOIDNull();

            if (item.CurrencyID.HasValue)
                row.CurrencyID = item.CurrencyID.Value;

            if (item.ExchangeRate.HasValue)
                row.ExchangeRate = item.ExchangeRate.Value;

            if (item.CurrencyAmount.HasValue)
                row.CurrencyAmount = item.CurrencyAmount.Value;

            if (item.MainCurrencyID.HasValue)
                row.MainCurrencyID = item.MainCurrencyID.Value;

            if (item.LocalCurrencyAmount.HasValue)
                row.LocalCurrencyAmount = (decimal)item.LocalCurrencyAmount.Value;

            if (item.MainCurrencyAmount.HasValue)
                row.MainCurrencyAmount = item.MainCurrencyAmount.Value;

            if (expenseType.Equals(ZoneType.Foreign) && !isRepOffice)
            {
                row.Amount = (double)Math.Round((decimal)(item.CurrencyAmount.Value * item.ExchangeRate.Value), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (item.Amount.HasValue)
                    row.Amount = item.Amount.Value;
            }

            row.SaleOrder = item.SaleOrder;
            row.SaleItem = item.SaleItem;
            row.Description = item.Description;
            row.ReferenceNo = item.ReferenceNo;
            row.VendorCodeAP = item.VendorCodeAP;

            row.Active = true;
            //row.CreBy = UserAccount.UserID;
            //row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;
            //ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }
        public void UpdateNonDeductAmountInvoiceItem(Guid txID, long invoiceId, double rateNonDeduct)
        {
            ExpenseDataSet expenseDs = (ExpenseDataSet)TransactionService.GetDS(txID);

            DataRow[] drArr = expenseDs.FnExpenseInvoiceItem.Select(string.Format("{0} = '{1}'", expenseDs.FnExpenseInvoiceItem.InvoiceIDColumn, invoiceId));

            foreach (ExpenseDataSet.FnExpenseInvoiceItemRow row in drArr)
            {
                double amount = Convert.ToDouble(row.Amount);
                double nonDeductAmount = ((amount * rateNonDeduct) / 100);
                row.NonDeductAmount = (double)Math.Round(Convert.ToDecimal(nonDeductAmount), 2, MidpointRounding.AwayFromZero);
            }
        }
        public void DeleteItemOnTransaction(Guid txId, long itemId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = expDs.FnExpenseInvoiceItem.FindByInvoiceItemID(itemId);
            row.Delete();
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long invoiceId)
        {
            IList<FnExpenseInvoiceItem> items = ScgeAccountingQueryProvider.FnExpenseInvoiceItemQuery.GetInvoiceItemByInvoiceID(invoiceId);

            //#region Validate
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //if (invoice == null)
            //{
            //    //errors.AddError("Error", new Spring.Validation.ErrorMessage("NoDocumentFound"));
            //}
            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            //#endregion

            // Set data to invoice row in Dataset.
            foreach (FnExpenseInvoiceItem item in items)
            {
                ExpenseDataSet.FnExpenseInvoiceItemRow itemRow = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();
                itemRow.InvoiceItemID = item.InvoiceItemID;
                itemRow.InvoiceID = invoiceId;
                if (item.CostCenter != null)
                {
                    itemRow.CostCenterID = item.CostCenter.CostCenterID;
                }
                if (item.Account != null)
                {
                    itemRow.AccountID = item.Account.AccountID;
                }
                if (item.IO != null)
                {
                    itemRow.IOID = item.IO.IOID;
                }
                if (item.CurrencyID.HasValue)
                    itemRow.CurrencyID = item.CurrencyID.Value;

                itemRow.Description = item.Description;

                if (item.CurrencyAmount.HasValue)
                    itemRow.CurrencyAmount = item.CurrencyAmount.Value;

                if (item.Amount.HasValue)
                    itemRow.Amount = item.Amount.Value;

                if (item.ExchangeRate.HasValue)
                    itemRow.ExchangeRate = item.ExchangeRate.Value;

                if (item.MainCurrencyID.HasValue)
                {
                    itemRow.MainCurrencyID = item.MainCurrencyID.Value;
                }

                if (item.LocalCurrencyAmount.HasValue)
                {
                    itemRow.LocalCurrencyAmount = (decimal)item.LocalCurrencyAmount.Value;
                }

                if (item.MainCurrencyAmount.HasValue)
                {
                    itemRow.MainCurrencyAmount = item.MainCurrencyAmount.Value;
                }

                itemRow.ReferenceNo = item.ReferenceNo;
                itemRow.VendorCodeAP = item.VendorCodeAP;
                itemRow.SaleOrder = item.SaleOrder;
                itemRow.SaleItem = item.SaleItem;
                itemRow.Active = item.Active;
                itemRow.CreBy = item.CreBy;
                itemRow.CreDate = item.CreDate;
                itemRow.UpdBy = item.UpdBy;
                itemRow.UpdDate = item.UpdDate;
                itemRow.UpdPgm = item.UpdPgm;

                // Add invoice row to documentDataset.
                ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(itemRow);
            }

        }

        //[Transaction]
        public void SavExpenseInvoiceItem(Guid txId, long invoiceId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);

            ScgeAccountingDaoProvider.FnExpenseInvoiceItemDao.Persist(expDS.FnExpenseInvoiceItem);
        }

        public void AddRecommendInvoiceItemOnTransaction(long invoiceId, string expenseType, IList<FnExpenseInvoiceItem> recommendList, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            short? mainCurrencyID = null;
            short? localCurrencyID = null;

            if (ds.FnExpenseDocument.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.FnExpenseDocument.Rows[0]["MainCurrencyID"].ToString()) && Convert.ToInt16(ds.FnExpenseDocument.Rows[0]["MainCurrencyID"].ToString()) > 0)
                {
                    mainCurrencyID = Convert.ToInt16(ds.FnExpenseDocument.Rows[0]["MainCurrencyID"].ToString());
                }

                if (!string.IsNullOrEmpty(ds.FnExpenseDocument.Rows[0]["LocalCurrencyID"].ToString()) && Convert.ToInt16(ds.FnExpenseDocument.Rows[0]["MainCurrencyID"].ToString()) > 0)
                {
                    localCurrencyID = Convert.ToInt16(ds.FnExpenseDocument.Rows[0]["LocalCurrencyID"].ToString());
                }
            }

            foreach (FnExpenseInvoiceItem item in recommendList)
            {
                ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();
                row.InvoiceID = invoiceId;

                if (item.CostCenter == null)
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
                if (item.Account == null)
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));

                if (!errors.IsEmpty)
                {
                    FnExpenseInvoiceService.DeleteInvoiceOnTransaction(invoiceId, txId);
                    throw new ServiceValidationException(errors);
                }

                if (item.CostCenter != null)
                    row.CostCenterID = item.CostCenter.CostCenterID;

                if (item.Account != null)
                    row.AccountID = item.Account.AccountID;

                if (item.IO != null)
                    row.IOID = item.IO.IOID;

                if (item.CurrencyID.HasValue)
                    row.CurrencyID = item.CurrencyID.Value;

                row.Description = item.Description;

                if (item.CurrencyAmount.HasValue)
                {
                    row.CurrencyAmount = item.CurrencyAmount.Value;
                }

                if (item.Amount.HasValue)
                {
                    row.Amount = item.Amount.Value;
                }

                if (item.ExchangeRate.HasValue)
                {
                    row.ExchangeRate = item.ExchangeRate.Value;
                }

                if (mainCurrencyID.HasValue)
                {
                    row.MainCurrencyID = mainCurrencyID.Value;
                }

                if (item.LocalCurrencyAmount.HasValue)
                {
                    row.LocalCurrencyAmount = (decimal)item.LocalCurrencyAmount.Value;
                }

                if (mainCurrencyID == localCurrencyID)
                {
                    row.MainCurrencyAmount = (double)row.LocalCurrencyAmount;
                }

                row.ReferenceNo = item.ReferenceNo;

                row.Active = true;
                row.CreBy = UserAccount.UserID;
                row.CreDate = DateTime.Now;
                row.UpdBy = UserAccount.UserID;
                row.UpdDate = DateTime.Now;
                row.UpdPgm = UserAccount.CurrentProgramCode;

                ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
            }
        }

        public void UpdateInvoiceItemByInvoiceID(Guid txID, long invoiceId, FnExpenseDocument expenseDocument)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseInvoiceRow invoiceRow = expDS.FnExpenseInvoice.FindByInvoiceID(invoiceId);
            ExpenseDataSet.FnExpenseInvoiceItemRow[] itemRows = (ExpenseDataSet.FnExpenseInvoiceItemRow[])expDS.FnExpenseInvoiceItem.Select();
            DbAccount account = new DbAccount();
            foreach (ExpenseDataSet.FnExpenseInvoiceItemRow row in itemRows)
            {
                account = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(row.AccountID);
            }
            string filter = String.Format("InvoiceID = {0}", invoiceId);
            DataRow[] rows = expDS.FnExpenseInvoiceItem.Select(filter);
            Dbpb pb = null;
            double totalLocalCurrencyAmount = 0;
            double totalMainCurrencyAmount = 0;
            double totalAmountTHB = 0;
            bool isRepOffice = expenseDocument.IsRepOffice.HasValue ? expenseDocument.IsRepOffice.Value : false;

            if (expenseDocument.PB != null)
            {
                pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(expenseDocument.PB.Pbid);
            }

            foreach (DataRow row in rows)
            {
                FnExpenseInvoiceItem item = new FnExpenseInvoiceItem();
                item.LoadFromDataRow(row);
                item.Invoice.Expense = expenseDocument;
                if (account.SaveAsVendor)
                {
                    if (invoiceRow.isVAT)
                    {
                        if (!invoiceRow.IsVendorIDNull())
                        {
                            item.VendorCodeAP = invoiceRow.VendorCode;
                        }
                    }
                }
                if (isRepOffice)
                {
                    if (expenseDocument.MainCurrencyID.HasValue)
                    {
                        item.MainCurrencyID = expenseDocument.MainCurrencyID.Value;
                    }

                    if (expenseDocument.ExchangeRateForLocalCurrency.HasValue && expenseDocument.ExchangeRateForLocalCurrency.Value != 0)
                    {
                        if (item.LocalCurrencyAmount.HasValue)
                        {
                            if (expenseDocument.MainCurrencyID == expenseDocument.LocalCurrencyID)
                            {
                                item.MainCurrencyAmount = item.LocalCurrencyAmount;
                            }
                            else
                            {
                                item.MainCurrencyAmount = (double)Math.Round((decimal)(item.LocalCurrencyAmount.Value / expenseDocument.ExchangeRateForLocalCurrency.Value), 2, MidpointRounding.AwayFromZero);
                            }
                            totalMainCurrencyAmount += item.MainCurrencyAmount.Value;
                            totalLocalCurrencyAmount += item.LocalCurrencyAmount.Value;
                        }
                    }

                    if (expenseDocument.ExchangeRateMainToTHBCurrency.HasValue)
                    {
                        if (item.MainCurrencyAmount.HasValue)
                        {
                            item.Amount = (double)Math.Round((decimal)(item.MainCurrencyAmount * expenseDocument.ExchangeRateMainToTHBCurrency), 2, MidpointRounding.AwayFromZero);
                            totalAmountTHB += item.Amount.Value;
                        }
                    }
                }
                else
                {
                    if (expenseDocument.ExpenseType == ZoneType.Foreign)
                    {
                        //item.ExchangeRate = (double)Math.Round(Convert.ToDecimal(expenseDocument.ExchangeRateForUSDAdvance), 4, MidpointRounding.AwayFromZero);
                        item.Amount = (double)Math.Round(Convert.ToDecimal(item.CurrencyAmount * item.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }

                    totalAmountTHB += item.Amount.Value;
                }
                UpdateInvoiceItemOnTransaction(item, txID, expenseDocument.ExpenseType);
            }
            invoiceRow.BeginEdit();
            if (isRepOffice)
            {
                invoiceRow.TotalAmountLocalCurrency = invoiceRow.TotalBaseAmountLocalCurrency = invoiceRow.NetAmountLocalCurrency = (decimal)totalLocalCurrencyAmount;
            }
            invoiceRow.TotalAmount = (decimal)totalAmountTHB;
            invoiceRow.EndEdit();
        }

        public void AddMileageInvoiceItem(FnExpenseInvoiceItem item, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseInvoiceItemRow row = ds.FnExpenseInvoiceItem.NewFnExpenseInvoiceItemRow();

            if (item.Invoice != null)
            {
                row.InvoiceID = item.Invoice.InvoiceID;
            }
            if (item.CostCenter != null)
                row.CostCenterID = item.CostCenter.CostCenterID;

            if (item.Account != null)
                row.AccountID = item.Account.AccountID;

            if (item.IO != null)
                row.IOID = item.IO.IOID;

            if (item.CurrencyID.HasValue)
                row.CurrencyID = item.CurrencyID.Value;
            if (item.Amount.HasValue)
                row.Amount = item.Amount.Value;
            if (item.ExchangeRate.HasValue)
                row.ExchangeRate = item.ExchangeRate.Value;
            if (item.CurrencyAmount.HasValue)
                row.CurrencyAmount = item.CurrencyAmount.Value;

            row.SaleOrder = item.SaleOrder;
            row.SaleItem = item.SaleItem;
            row.Description = item.Description;
            row.ReferenceNo = item.ReferenceNo;

            row.Active = true;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;

            ds.FnExpenseInvoiceItem.AddFnExpenseInvoiceItemRow(row);
        }

        public void ValidateInvoiceItem(Guid txId, long expenseId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = expDs.FnExpenseDocument.FindByExpenseID(expenseId);
            ExpenseDataSet.DocumentRow docRow = expDs.Document.FindByDocumentID(expRow.DocumentID);
            ExpenseDataSet.FnExpenseInvoiceItemRow[] itemRows = (ExpenseDataSet.FnExpenseInvoiceItemRow[])expDs.FnExpenseInvoiceItem.Select();

            foreach (ExpenseDataSet.FnExpenseInvoiceItemRow row in itemRows)
            {
                string accountCode = string.Empty;
                if (!row.IsIOIDNull())
                {
                    DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(row.IOID);
                    if (io == null || !io.CompanyID.HasValue || io.CompanyID.Value != docRow.CompanyID)
                    {
                        if (io == null)
                        {
                            io = new DbInternalOrder() { IONumber = "Unknown" };
                        }
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("InternalOrder_Is_Invalid", new object[] { io.IONumber }));
                    }
                }
                if (!row.IsAccountIDNull())
                {
                    DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(row.AccountID);


                    IList<VOAccountCompany> accountList = ScgDbQueryProvider.DbAccountCompanyQuery.FindAccountCompany(docRow.CompanyID, row.AccountID);
                    if (account == null || accountList.Count == 0)
                    {
                        if (account == null)
                        {
                            account = new DbAccount() { AccountCode = "Unknown" };
                        }
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExpenseCode_Is_Invalid", new object[] { account.AccountCode }));
                    }
                    accountCode = account.AccountCode;
                }
                if (!row.IsCostCenterIDNull())
                {
                    DbCostCenter cost = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(row.CostCenterID);

                    if (cost == null || cost.CompanyID == null || cost.CompanyID.CompanyID != docRow.CompanyID)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CostCenter_Is_Invalid", new object[] { cost.CostCenterCode }));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(accountCode))
                        {
                            DbAccount ac = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(accountCode, null);
                            if (ac == null)
                            {
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ExpenseCode_Is_Invalid_With_Costcenter", new object[] { accountCode, cost.CostCenterCode }));
                            }
                        }
                    }
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        public double ReCalculateInvoiceItem(Guid txId, long invoiceId, string expenseType)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);

            string filter = String.Format("InvoiceID = {0}", invoiceId);
            DataRow[] rows = expDS.FnExpenseInvoiceItem.Select(filter);
            double totalAmount = 0;
            foreach (ExpenseDataSet.FnExpenseInvoiceItemRow item in rows)
            {
                if (expenseType.Equals(ZoneType.Foreign))
                {
                    item.Amount = (double)Math.Round((decimal)(item.ExchangeRate * item.CurrencyAmount), 2, MidpointRounding.AwayFromZero);
                }

                totalAmount += item.Amount;
            }

            return totalAmount;
        }
    }
}
