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
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.Standard.Security;
using System.Data;
using SCG.DB.Query;
using SS.DB.Query;
using SS.SU.DTO;
using SCG.DB.DTO;
using Spring.Transaction.Interceptor;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseMileageService : ServiceBase<FnExpenseMileage, long>, IFnExpenseMileageService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        public IFnExpenseMileageItemService FnExpenseMileageItemService { get; set; }

        public override IDao<FnExpenseMileage, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseMileageDao;
        }
        public long AddBeginRowExpenseMileageOnTransaction(FnExpenseMileage expenseMileage, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            if (ds != null)
            {
                ExpenseDataSet.FnExpenseMileageRow row = ds.FnExpenseMileage.NewFnExpenseMileageRow();

                row.ExpenseID = expenseMileage.Expense.ExpenseID;
                row.Active = true;
                row.CreDate = DateTime.Now;
                row.CreBy = UserAccount.UserID;
                row.UpdDate = DateTime.Now;
                row.UpdBy = UserAccount.UserID;
                row.UpdPgm = UserAccount.CurrentProgramCode;

                ds.FnExpenseMileage.AddFnExpenseMileageRow(row);

                return row.ExpenseMileageID;
            }
            return 0;
        }
        public void UpdateExpenseMileageOnTransaction(FnExpenseMileage expenseMileage, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            long documentId = 0;
            if (expenseMileage.Expense != null)
            {
                ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(expenseMileage.Expense.ExpenseID);
                documentId = expRow == null ? 0 : expRow.DocumentID;
            }

            if (expenseMileage.CarLicenseNo.Length.Equals(0))
                errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredCarLicenseNo"));

            if (expenseMileage.Owner.Equals(OwnerMileage.Employee))
            {
                if (expenseMileage.First100KmRate.Equals((double)0))
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredFirst100KmOverZero"));
                if (expenseMileage.Exceed100KmRate.Equals((double)0))
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredExceed100KmOverZero"));
            }
            else
            {
                if (expenseMileage.TotalAmount.Equals((double)0))
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredTotalAmountOverZero"));
            }

            if (!expenseMileage.Owner.Equals(OwnerMileage.Company) || !expenseMileage.TypeOfCar.Equals(TypeOfCar.Pickup))
            {
            // Validate CostCenter.
            if (expenseMileage.CostCenter == null)
            {
                errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            }
            else
            {
                if (ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(expenseMileage.CostCenter.CostCenterID) == null)
                {
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
                }
            }

            // Validate Account.
            if (expenseMileage.Account == null)
            {
                errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
            }
            else
            {
                if (ScgDbQueryProvider.DbAccountQuery.FindByIdentity(expenseMileage.Account.AccountID) == null)
                {
                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
                }
            }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet.FnExpenseMileageRow row = ds.FnExpenseMileage.FindByExpenseMileageID(expenseMileage.ExpenseMileageID);

            row.BeginEdit();
            if (expenseMileage.Expense != null)
            {
                row.ExpenseID = expenseMileage.Expense.ExpenseID;
            }
            row.Owner = expenseMileage.Owner;
            row.CarLicenseNo = expenseMileage.CarLicenseNo;
            row.TypeOfCar = expenseMileage.TypeOfCar;
            row.PermissionNo = expenseMileage.PermissionNo;
            row.HomeToOfficeRoundTrip = (decimal)expenseMileage.HomeToOfficeRoundTrip;
            row.PrivateUse = (decimal)expenseMileage.PrivateUse;
            row.IsOverrideLevel = expenseMileage.IsOverrideLevel;
            if (expenseMileage.OverrideCompanyId != null) 
            {
                row.OverrideCompanyId = (long)expenseMileage.OverrideCompanyId;
            }
            if (expenseMileage.CurrentCompanyId != null)
            {
                row.CurrentCompanyId = (long)expenseMileage.CurrentCompanyId;
            }

            row.OverrideLevelRemark = expenseMileage.OverrideLevelRemark;
            row.OverrideUserPersonalLevelCode = expenseMileage.OverrideUserPersonalLevelCode;
            row.CurrentUserPersonalLevelCode = expenseMileage.CurrentUserPersonalLevelCode;

            row.First100KmRate = (decimal)expenseMileage.First100KmRate;
            row.Exceed100KmRate = (decimal)expenseMileage.Exceed100KmRate;
            row.HelpingAmount = (decimal)expenseMileage.HelpingAmount;
            row.OverHelpingAmount = (decimal)expenseMileage.OverHelpingAmount;

            row.TotalAmount = (decimal)expenseMileage.TotalAmount;

            if (expenseMileage.CostCenter != null)
                row.CostCenterID = expenseMileage.CostCenter.CostCenterID;
            else
                row.SetCostCenterIDNull();

            if (expenseMileage.Account != null)
                row.AccountID = expenseMileage.Account.AccountID;
            else
                row.SetAccountIDNull();

            if (expenseMileage.IO != null)
                row.IOID = expenseMileage.IO.IOID;
            else
                row.SetIOIDNull();

            row.CreDate = DateTime.Now;
            row.CreBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdPgm = UserAccount.CurrentProgramCode;
            row.Active = true;
            row.EndEdit();

            UpdateMileageSummary(txId, expenseMileage);

            ExpenseDataSet.FnExpenseMileageRow mileageRow = ds.FnExpenseMileage.FindByExpenseMileageID(expenseMileage.ExpenseMileageID);

            ExpenseDataSet.FnExpenseDocumentRow expenseRow = ds.FnExpenseDocument.FindByExpenseID(row.ExpenseID);
            long requesterId = 0;
            if (expenseRow != null)
            {
                ExpenseDataSet.DocumentRow docRow = ds.Document.FindByDocumentID(expenseRow.DocumentID);
                if (docRow != null)
                    requesterId = docRow.RequesterID;
            }
            long invoiceId = 0;
            DataRow[] invoiceRows = ds.FnExpenseInvoice.Select(String.Format("InvoiceDocumentType = '{0}'", InvoiceType.Mileage));
            FnExpenseInvoice invoice = new FnExpenseInvoice();

            string accountCode = string.Empty;
            SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(requesterId);
            if (expenseMileage.Owner.Equals(OwnerMileage.Employee) || (expenseMileage.Owner.Equals(OwnerMileage.Company) && !expenseMileage.TypeOfCar.Equals(TypeOfCar.Pickup)))
            {
                if (invoiceRows.Length > 0)
                {
                    foreach (DataRow inv in invoiceRows)
                    {
                        invoiceId = inv.Field<long>("InvoiceID");
                        DataRow[] invoiceItemRows = ds.FnExpenseInvoiceItem.Select(String.Format("InvoiceID = {0} ", invoiceId));
                        foreach (DataRow item in invoiceRows)
                        {
                            item.Delete();
                        }
                        inv.Delete();
                    }
                }
            }

            //Add empty invoice 
            invoice.InvoiceDocumentType = InvoiceType.Mileage;
            invoice.Expense = expenseMileage.Expense;
            DbCostCenter cost = null;
            string expenseGroup = "0";
            if (!mileageRow.IsCostCenterIDNull())
            {
                cost = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(mileageRow.CostCenterID);
                expenseGroup = (cost == null ? string.Empty : cost.CostCenterCode.Substring(3, 1).Equals("0") ? "0" : "1");
            }

            DbInternalOrder io = null;
            if (expenseMileage.IO != null)
            {
                io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(expenseMileage.IO.IOID);
            }

            if (expenseMileage.Owner.Equals(OwnerMileage.Employee))
            {
                double total = (double)mileageRow.TotalAmount;
                invoice.TotalAmount = total;
                invoice.TotalBaseAmount = total;
                invoice.NetAmount = total;
                invoiceId = FnExpenseInvoiceService.AddInvoiceOnTransaction(invoice, txId);

                invoice.InvoiceID = invoiceId;

                FnExpenseInvoiceItem invoiceItemGov = new FnExpenseInvoiceItem();
                FnExpenseInvoiceItem invoiceItemExtra = new FnExpenseInvoiceItem();

                invoiceItemGov.Invoice = invoice;
                invoiceItemExtra.Invoice = invoice;

                invoiceItemGov.CostCenter = expenseMileage.CostCenter;
                invoiceItemGov.Account = expenseMileage.Account;
                invoiceItemGov.IO = expenseMileage.IO;

                invoiceItemExtra.CostCenter = expenseMileage.CostCenter;
                if (io != null && !string.IsNullOrEmpty(io.IONumber))
                {
                    string ioType = io.IONumber.Substring(4, 2);
                    if (ioType.Contains("02") || ioType.Contains("03") || ioType.Contains("04") || ioType.Contains("09"))
                    {
                        invoiceItemExtra.IO = null;
                    }
                    else
                    {
                        invoiceItemExtra.IO = io;
                    }
                }

                invoiceItemExtra.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ParameterServices.AccountMileageExtra, null);

                //switch (expenseGroup)
                //{
                //    case "0":
                //        invoiceItemExtra.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ParameterServices.AccountMileageOfficeExtra, expenseGroup, null);
                //        break;
                //    case "1":
                //        invoiceItemExtra.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(ParameterServices.AccountMileageFactoryExtra, expenseGroup, null);
                //        break;

                //    default:
                //        break;
                //}
                if (mileageRow.HelpingAmount <= mileageRow.TotalAmount)
                    invoiceItemGov.Amount = (double)mileageRow.HelpingAmount;
                else if (mileageRow.HelpingAmount > mileageRow.TotalAmount)
                    invoiceItemGov.Amount = (double)mileageRow.TotalAmount;

                invoiceItemExtra.Amount = (double)mileageRow.OverHelpingAmount;
                if (invoiceItemGov.Amount > 0)
                    FnExpenseInvoiceItemService.AddMileageInvoiceItem(invoiceItemGov, txId);

                if (invoiceItemExtra.Amount > 0)
                    FnExpenseInvoiceItemService.AddMileageInvoiceItem(invoiceItemExtra, txId);

            }
            else if (expenseMileage.Owner.Equals(OwnerMileage.Company) && (!expenseMileage.TypeOfCar.Equals(TypeOfCar.Pickup)))
            {
                invoice.TotalAmount = (double)mileageRow.HelpingAmount;
                invoice.TotalBaseAmount = (double)mileageRow.HelpingAmount;
                invoice.NetAmount = (double)mileageRow.HelpingAmount;
                invoiceId = FnExpenseInvoiceService.AddInvoiceOnTransaction(invoice, txId);

                FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem();

                invoice.InvoiceID = invoiceId;
                invoiceItem.Invoice = invoice;
                invoiceItem.CostCenter = expenseMileage.CostCenter;
                invoiceItem.Account = expenseMileage.Account;
                invoiceItem.IO = expenseMileage.IO;

                //switch (expenseGroup)
                //{
                //    case "0":
                //        accountCode = ParameterServices.AccountMileageOfficeCompany;
                //        break;
                //    case "1":

                //        accountCode = ParameterServices.AccountMileageFactoryCompany;
                //        break;

                //    default:
                //        break;
                //}

                //invoiceItem.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(accountCode, expenseGroup, null);
                invoiceItem.Amount = (double)mileageRow.HelpingAmount;
                FnExpenseInvoiceItemService.AddMileageInvoiceItem(invoiceItem, txId);
            }
        }

        public decimal CalculateDistanceAmount(decimal distanceTotal, decimal rate)
        {
            return Math.Round(distanceTotal * rate, 2, MidpointRounding.AwayFromZero);
        }
        public decimal CalculateTotalAmount(decimal first100KmAmount, decimal exceed100KmAmount)
        {
            return first100KmAmount + exceed100KmAmount;
        }
        public decimal CalculateExceed100KmRate(decimal distanceTotal, decimal distanceFirst100km)
        {
            return distanceTotal - distanceFirst100km;
        }
        public decimal CalculateHelpingAmount(decimal distanceTotal, decimal rate)
        {
            return Math.Round(distanceTotal * rate, 2, MidpointRounding.AwayFromZero);
        }
        public decimal CalculateOverHelpingAmount(decimal totalAmount, decimal helpingAmount)
        {
            decimal overHelpingAmount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero) - Math.Round(helpingAmount, 2, MidpointRounding.AwayFromZero);
            if (overHelpingAmount < 0)
                return 0;

            return overHelpingAmount;
        }
        public decimal CalculateNetDistance(decimal distanceTotal, decimal adjustTotal)
        {
            return Math.Round(distanceTotal, 2, MidpointRounding.AwayFromZero) - Math.Round(adjustTotal, 2, MidpointRounding.AwayFromZero);
        }
        public decimal CalculateReimbursementAmount(decimal totalAmount, decimal totalNetDistance, decimal totalDistance)
        {
            if (totalDistance != 0)
            {
                return Math.Round((totalAmount * totalNetDistance) / totalDistance, 2, MidpointRounding.AwayFromZero);
            }
            return 0;
        }
        public decimal CalculateRemaining(decimal reimbursementAmount, decimal totalNetAmount)
        {
            return Math.Round(reimbursementAmount, 2, MidpointRounding.AwayFromZero) - Math.Round(totalNetAmount, 2, MidpointRounding.AwayFromZero);
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpenseMileage> mileageList = ScgeAccountingQueryProvider.FnExpenseMileageQuery.GetMileageByExpenseID(expenseId);

            foreach (FnExpenseMileage mileage in mileageList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseMileageRow mileageRow = ds.FnExpenseMileage.NewFnExpenseMileageRow();
                mileageRow.ExpenseMileageID = mileage.ExpenseMileageID;

                if (mileage.Expense != null)
                    mileageRow.ExpenseID = expenseId;

                mileageRow.Owner = mileage.Owner;
                mileageRow.CarLicenseNo = mileage.CarLicenseNo;
                mileageRow.TypeOfCar = mileage.TypeOfCar;
                mileageRow.PermissionNo = mileage.PermissionNo;

                mileageRow.HomeToOfficeRoundTrip = (decimal)mileage.HomeToOfficeRoundTrip;
                mileageRow.PrivateUse = (decimal)mileage.PrivateUse;
                mileageRow.First100KmRate = (decimal)mileage.First100KmRate;
                mileageRow.Exceed100KmRate = (decimal)mileage.Exceed100KmRate;

                mileageRow.IsOverrideLevel = mileage.IsOverrideLevel;

                if (mileage.OverrideCompanyId != null)
                    mileageRow.OverrideCompanyId = (long)mileage.OverrideCompanyId;

                if (mileage.CurrentCompanyId != null)
                    mileageRow.CurrentCompanyId = (long)mileage.CurrentCompanyId;

                mileageRow.OverrideUserPersonalLevelCode = mileage.OverrideUserPersonalLevelCode;
                mileageRow.OverrideLevelRemark = mileage.OverrideLevelRemark;
                mileageRow.CurrentUserPersonalLevelCode = mileage.CurrentUserPersonalLevelCode;

                mileageRow.TotalAmount = (decimal)mileage.TotalAmount;
                mileageRow.HelpingAmount = (decimal)mileage.HelpingAmount;
                mileageRow.OverHelpingAmount = (decimal)mileage.OverHelpingAmount;

                mileageRow.Active = mileage.Active;
                mileageRow.CreBy = mileage.CreBy;
                mileageRow.CreDate = mileage.CreDate;
                mileageRow.UpdBy = mileage.UpdBy;
                mileageRow.UpdDate = mileage.UpdDate;
                mileageRow.UpdPgm = mileage.UpdPgm;

                if (mileage.CostCenter != null)
                {
                    mileageRow.CostCenterID = mileage.CostCenter.CostCenterID;
                }
                if (mileage.Account != null)
                {
                    mileageRow.AccountID = mileage.Account.AccountID;
                }
                if (mileage.IO != null)
                {
                    mileageRow.IOID = mileage.IO.IOID;
                }

                // Add perdiem row to documentDataset.
                ds.FnExpenseMileage.AddFnExpenseMileageRow(mileageRow);

                // Prepare Data to FnExpenseMileageItem Datatable. 
                FnExpenseMileageItemService.PrepareDataToDataset(ds, mileageRow.ExpenseMileageID);
            }
        }

        public void UpdateMileageSummary(Guid txId, FnExpenseMileage mileage)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = expDS.FnExpenseMileage.FindByExpenseMileageID(mileage.ExpenseMileageID);
            mileageRow.BeginEdit();

            if (mileage.Owner.Equals(OwnerMileage.Employee))
            {
                decimal first100KmAmount = ComputeTotalFirst100Km(txId, mileageRow.ExpenseMileageID);
                decimal exceed100KmAmount = ComputeTotalExceed100Km(txId, mileageRow.ExpenseMileageID);

                decimal totalDistance = first100KmAmount + exceed100KmAmount;

                decimal totalfirst100KmAmount = first100KmAmount * (decimal)mileage.First100KmRate;
                decimal totalexceed100KmAmount = exceed100KmAmount * (decimal)mileage.Exceed100KmRate;
                decimal totalAmount = CalculateTotalAmount(totalfirst100KmAmount, totalexceed100KmAmount);
                decimal helpGovRate = (decimal)ParameterServices.OtherRateForMileageCalculation;
                if(mileage.TypeOfCar.Equals(TypeOfCar.MotorCycle))
                {
                    helpGovRate = (decimal)ParameterServices.MotorcycleRateForMileageCalculation;
                }
                decimal helpingAmount = CalculateHelpingAmount(totalDistance, helpGovRate);

                mileageRow.TotalAmount = totalAmount;
                mileageRow.HelpingAmount = helpingAmount;

                mileageRow.OverHelpingAmount = CalculateOverHelpingAmount(totalAmount, helpingAmount);
            }
            else
            {
                decimal totalAmount = (decimal)mileage.TotalAmount;
                decimal totalDistance = ComputeTotalDistance(txId, mileageRow.ExpenseMileageID);
                decimal totalDistanceNet = ComputeTotalDistanceNet(txId, mileageRow.ExpenseMileageID);
                decimal helpingAmount = CalculateReimbursementAmount(totalAmount, totalDistanceNet, totalDistance);

                mileageRow.HelpingAmount = helpingAmount;
            }
            mileageRow.EndEdit();
        }

        #region For Calculate total MileageItem
        public decimal ComputeTotalFirst100Km(Guid txID, long expenseMileageId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + expenseMileageId);
            decimal total = (decimal)0;

            foreach (DataRow dr in row)
            {
                if (!string.IsNullOrEmpty(dr["DistanceFirst100Km"].ToString()))
                {
                    total += Convert.ToDecimal(dr["DistanceFirst100Km"].ToString());
                }
            }
            return total;
        }
        public decimal ComputeTotalExceed100Km(Guid txID, long expenseMileageId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + expenseMileageId);
            decimal total = 0;

            foreach (DataRow dr in row)
            {
                if (!string.IsNullOrEmpty(dr["DistanceExceed100Km"].ToString()))
                {
                    total += Convert.ToDecimal(dr["DistanceExceed100Km"].ToString());
                }
            }
            return total;
        }
        public decimal ComputeTotalDistance(Guid txID, long expenseMileageId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + expenseMileageId);
            decimal total = 0;

            foreach (DataRow dr in row)
            {
                if (!string.IsNullOrEmpty(dr["DistanceTotal"].ToString()))
                {
                    total += Convert.ToDecimal(dr["DistanceTotal"].ToString());
                }
            }
            return total;
        }
        public decimal ComputeTotalDistanceNet(Guid txID, long expenseMileageId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            DataRow[] row = expDS.FnExpenseMileageItem.Select("ExpenseMileageID=" + expenseMileageId);
            decimal total = 0;

            foreach (DataRow dr in row)
            {
                if (!string.IsNullOrEmpty(dr["DistanceNet"].ToString()))
                {
                    total += Convert.ToDecimal(dr["DistanceNet"].ToString());
                }
            }
            return total;
        }
        #endregion

        public void UpdateMileageByExpenseID(Guid txID, long expDocumentId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("ExpenseID = {0}", expDocumentId);
            DataRow[] rows = expDS.FnExpenseMileage.Select(filter);

            foreach (DataRow row in rows)
            {
                FnExpenseMileage mileage = new FnExpenseMileage();
                mileage.LoadFromDataRow(row);

                this.UpdateExpenseMileageOnTransaction(mileage, txID);
            }

        }

        //[Transaction]
        public void SaveExpenseMileage(Guid txID, long expenseId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);
            ScgeAccountingDaoProvider.FnExpenseMileageDao.Persist(expDS.FnExpenseMileage);
            ScgeAccountingDaoProvider.FnExpenseMileageItemDao.Persist(expDS.FnExpenseMileageItem);
        }

        public void ValidRemaining(Guid txID, long expDocumentId)
        {
            double remaining = 0;
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("ExpenseID = {0}", expDocumentId);
            DataRow[] rows = expDS.FnExpenseMileage.Select(filter);

            if (rows.Length > 0)
            {
                FnExpenseMileage mileage = new FnExpenseMileage();
                mileage.LoadFromDataRow(rows[0]);
                double totalInvoice = 0;

                filter = String.Format("InvoiceDocumentType = '{0}'", InvoiceType.Mileage);
                DataRow[] invoiceRows = expDS.FnExpenseInvoice.Select(filter);

                foreach (DataRow row in invoiceRows)
                {
                    FnExpenseInvoice invoice = new FnExpenseInvoice();
                    invoice.LoadFromDataRow(row);

                    totalInvoice += ((double) Math.Round(Convert.ToDecimal( invoice.NetAmount), 2, MidpointRounding.AwayFromZero));
                }

                remaining = ((double)Math.Round(Convert.ToDecimal(mileage.HelpingAmount), 2,MidpointRounding.AwayFromZero)) - ((double) Math.Round(Convert.ToDecimal( totalInvoice), 2, MidpointRounding.AwayFromZero));

                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                if (rows[0].Field<string>("Owner").Equals(OwnerMileage.Company) && rows[0].Field<string>("TypeOfCar").Equals(TypeOfCar.Pickup))
                {
                    if (remaining != 0)
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RemainingNotZero"));
                    }
                }
                if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            }
        }

        public void DeleteMileage(Guid txID, long mileageId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txID);
            ExpenseDataSet.FnExpenseMileageRow row = expDs.FnExpenseMileage.FindByExpenseMileageID(mileageId);
            row.Delete();
            FnExpenseInvoiceService.DeleteMileageInvoice(txID);
        }
    }
}
