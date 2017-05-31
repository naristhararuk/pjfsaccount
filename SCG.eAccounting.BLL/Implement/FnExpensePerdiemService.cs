using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SS.DB.Query;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;
using Spring.Transaction.Interceptor;
using SS.DB.DTO;
using SS.SU.DTO;
using SS.Standard.WorkFlow.DTO;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpensePerdiemService : ServiceBase<FnExpensePerdiem, long>, IFnExpensePerdiemService
    {

        #region properties
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        public IFnExpenseInvoiceItemService FnExpenseInvoiceItemService { get; set; }
        public IFnExpensePerdiemItemService FnExpensePerdiemItemService { get; set; }
        public IFnExpensePerdiemDetailService FnExpensePerdiemDetailService { get; set; }
        public IFnExpensePerdiemQuery FnExpensePerdiemQuery { get; set; }
        public IUserAccount UserAccount { get; set; }
        #endregion

        #region method
        public override IDao<FnExpensePerdiem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpensePerdiemDao;
        }
        public long AddExpensePerdiemTransaction(FnExpensePerdiem expensePerdiem, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow row = ds.FnExpensePerdiem.NewFnExpensePerdiemRow();
            row.ExpenseID = expensePerdiem.Expense.ExpenseID;

            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(row.ExpenseID);
            bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;
            SuUser requester = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(GetRequesterID(expensePerdiem.Expense.ExpenseID, txId));

            string PerdiemType = expRow.ExpenseType;

            row.PerdiemRate = 0;

            IList<PerdiemRateValObj> prList = null;
            if (!isRepOffice)
            {
                prList = ScgeAccountingQueryProvider.FnPerdiemRateQuery.GetPerdiemRateByRequesterID(requester.Userid);
            }
            else
            {
                prList = ScgeAccountingQueryProvider.FnPerdiemRateQuery.GetPerdiemRateByRequesterIDForRepOffice(requester.Userid, requester.PersonalGroup);
            }

            foreach (PerdiemRateValObj obj in prList)
            {
                if (!isRepOffice)
                {
                    if (PerdiemType == ZoneType.Foreign)
                    {
                        if (obj.ZoneID.ToString() == "2")
                        {
                            row.PerdiemRateUSDHigh = (decimal)obj.ExtraPerdiemRate;
                            row.PerdiemRateGovHigh = (decimal)obj.OfficialPerdiemRate;
                        }
                        else if (obj.ZoneID.ToString() == "1")
                        {
                            row.PerdiemRateUSD = (decimal)obj.ExtraPerdiemRate;
                            row.PerdiemRateGov = (decimal)obj.OfficialPerdiemRate;
                        }
                    }
                    else
                    {
                        if (obj.ZoneID == (long)CountryZonePerdiem.DomesticZone)
                        {
                            row.PerdiemRateGov = (decimal)obj.OfficialPerdiemRate;
                            row.PerdiemRate = (decimal)(obj.ExtraPerdiemRate + obj.OfficialPerdiemRate);
                        }
                    }
                }
                else
                {
                    if (PerdiemType == ZoneType.Foreign)
                    {
                        if (obj.ZoneID.ToString() == "2")
                        {
                            row.PerdiemRateUSDHigh = (decimal)obj.StuffPerdiemRate;
                        }
                        else if (obj.ZoneID.ToString() == "7")
                        {
                            row.PerdiemRateUSDThaiZone = (decimal)obj.StuffPerdiemRate;
                        }
                        else
                        {
                            row.PerdiemRateUSD = (decimal)obj.StuffPerdiemRate;
                        }
                    }
                }
            }

            decimal exchangeRate = 0;

            if (!isRepOffice)
            {
                exchangeRate = Math.Round(expRow.ExchangeRateForUSDAdvance, 5, MidpointRounding.AwayFromZero);
            }
            else
            {
                exchangeRate = GetExchangeRatePerdiemCalculationForRepOffice(txId, expRow.ExpenseID);
            }

            row.ExchangeRate = exchangeRate;
            row.Active = true;
            row.CreBy = UserAccount.UserID;
            row.CreDate = DateTime.Now.Date;

            ds.FnExpensePerdiem.AddFnExpensePerdiemRow(row);

            return row.ExpensePerdiemID;
        }

        public decimal GetExchangeRatePerdiemCalculationForRepOffice(Guid txId, long expId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(expId);
            bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;
            string ExpenseType = string.Empty;
            if (!expRow.IsExpenseTypeNull())
            {
                ExpenseType = expRow.ExpenseType;
            }

            decimal exchangeRate = 0;

            InvoiceExchangeRate exchangeRateObj = FnExpenseDocumentService.GetAdvanceExchangeRateRepOffice(txId, ParameterServices.USDCurrencyID, expId);
            if (exchangeRateObj != null)  // reference advance document
            {
                if (exchangeRateObj.AdvanceExchangeRateAmount == 0)
                {
                    if (expRow.LocalCurrencyID != expRow.MainCurrencyID)
                    {
                        exchangeRate = (decimal)ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expRow.PBID, expRow.MainCurrencyID, expRow.LocalCurrencyID);
                    }
                }
                else
                {
                    if (expRow.LocalCurrencyID != expRow.MainCurrencyID)
                    {
                        //if (ExpenseType == ZoneType.Foreign)
                        //    exchangeRate = (decimal)ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expRow.PBID, expRow.MainCurrencyID, expRow.LocalCurrencyID);
                        //else
                        exchangeRate = (decimal)exchangeRateObj.AdvanceExchangeRateAmount;
                    }
                    else
                    {
                        exchangeRate = (decimal)exchangeRateObj.AdvanceExchangeRateAmount;
                    }
                }
            }
            else  // not reference advance document
            {
                if (expRow.LocalCurrencyID != expRow.MainCurrencyID)  // final currency != main currency
                {
                    exchangeRate = (decimal)ScgDbQueryProvider.DbPbRateQuery.GetExchangeRate(expRow.PBID, expRow.MainCurrencyID, expRow.LocalCurrencyID);
                }
                else  // final currency == main currency
                {
                    exchangeRate = 1;
                }
            }

            return exchangeRate;
        }


        public void UpdateExpensePerdiemTransaction(FnExpensePerdiem expensePerdiem, long? itemCountryZone, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(expensePerdiem.ExpensePerdiemID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);
            bool isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;

            string PerdiemType = GetPerdiemType(expensePerdiem, txId);

            if (expensePerdiem.CostCenter == null)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredCostCenter"));
            if (expensePerdiem.Account == null)
                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredAccountID"));
            ////if (String.IsNullOrEmpty(expensePerdiem.Description))
            ////    errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredDescription"));

            if (PerdiemType == ZoneType.Foreign)
            {
                if (expensePerdiem.ExchangeRate.Equals((double)0))
                    errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemExchangeRate"));

                if (!isRepOffice)
                {
                    if (expensePerdiem.PerdiemRateUSD.Equals((double)0))
                        errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDOverZero"));
                    if (expensePerdiem.PerdiemRateUSDHigh.Equals((double)0))
                        errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDHighOverZero"));
                    if (expensePerdiem.PerdiemRateGov.Equals((double)0))
                        errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateGovOverZero"));
                    if (expensePerdiem.PerdiemRateGovHigh.Equals((double)0))
                        errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateGovHighOverZero"));
                }
                else
                {
                    if (itemCountryZone.HasValue)
                    {
                        if (itemCountryZone == (long)CountryZonePerdiem.NormalZone)
                        {
                            if (expensePerdiem.PerdiemRateUSD.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDOverZero"));
                        }
                        else if (itemCountryZone == (long)CountryZonePerdiem.HighZone)
                        {
                            if (expensePerdiem.PerdiemRateUSDHigh.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDHighOverZero"));
                        }
                        else if (itemCountryZone == (long)CountryZonePerdiem.ThaiZone)
                        {
                            if (expensePerdiem.PerdiemRateUSDThaiZone.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDThaiZoneOverZero"));
                        }
                    }
                    else
                    {
                        ExpenseDataSet.FnExpensePerdiemItemRow[] perdiemItemRows = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiem.ExpensePerdiemID) as ExpenseDataSet.FnExpensePerdiemItemRow[];

                        if (perdiemItemRows.Where(t => t.CountryZoneID == (long)CountryZonePerdiem.NormalZone).Count() > 0)
                        {
                            if (expensePerdiem.PerdiemRateUSD.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDOverZero"));
                        }
                        else if (perdiemItemRows.Where(t => t.CountryZoneID == (long)CountryZonePerdiem.HighZone).Count() > 0)
                        {
                            if (expensePerdiem.PerdiemRateUSDHigh.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDHighOverZero"));
                        }
                        else if (perdiemItemRows.Where(t => t.CountryZoneID == (long)CountryZonePerdiem.ThaiZone).Count() > 0)
                        {
                            if (expensePerdiem.PerdiemRateUSDThaiZone.Equals((double)0))
                                errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateUSDThaiZoneOverZero"));
                        }
                    }
                }
            }
            else
            {
                if (expensePerdiem.PerdiemRate.Equals((double)0))
                    errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("RequiredPerdiemRateOverZero"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            decimal exchangeRate = 0;

            if (!isRepOffice)
            {
                if (!expRow.IsExchangeRateForUSDAdvanceNull() && !expRow.ExchangeRateForUSDAdvance.Equals(0))
                {
                    exchangeRate = Math.Round(expRow.ExchangeRateForUSDAdvance, 5, MidpointRounding.AwayFromZero);
                }
                else
                {
                    exchangeRate = (decimal)expensePerdiem.ExchangeRate;
                }
            }
            else
            {
                exchangeRate = GetExchangeRatePerdiemCalculationForRepOffice(txId, expRow.ExpenseID);
            }

            perdiemRow.ExpenseID = expensePerdiem.Expense.ExpenseID;
            perdiemRow.CostCenterID = expensePerdiem.CostCenter.CostCenterID;
            if (expensePerdiem.Account != null)
                perdiemRow.AccountID = expensePerdiem.Account.AccountID;
            if (expensePerdiem.IO != null)
                perdiemRow.IOID = expensePerdiem.IO.IOID;
            else
                perdiemRow.SetIOIDNull();

            perdiemRow.SaleOrder = expensePerdiem.SaleOrder;
            perdiemRow.SaleItem = expensePerdiem.SaleItem;
            perdiemRow.Description = expensePerdiem.Description;
            perdiemRow.PerdiemRate = (decimal)expensePerdiem.PerdiemRate;
            perdiemRow.ReferenceNo = expensePerdiem.ReferenceNo;
            perdiemRow.Active = true;
            perdiemRow.UpdBy = UserAccount.UserID;
            perdiemRow.UpdDate = DateTime.Now.Date;
            perdiemRow.UpdPgm = expensePerdiem.UpdPgm;
            if (PerdiemType == ZoneType.Foreign)
            {
                perdiemRow.PerdiemRateUSD = (decimal)expensePerdiem.PerdiemRateUSD;
                perdiemRow.PerdiemRateUSDHigh = (decimal)expensePerdiem.PerdiemRateUSDHigh;
                perdiemRow.PerdiemRateGov = (decimal)expensePerdiem.PerdiemRateGov;
                perdiemRow.PerdiemRateGovHigh = (decimal)expensePerdiem.PerdiemRateGovHigh;
                perdiemRow.ExchangeRate = exchangeRate;
                perdiemRow.PerdiemRateUSDThaiZone = (decimal)expensePerdiem.PerdiemRateUSDThaiZone;
            }
            else
            {
                if (perdiemRow.PerdiemRate > 0 && perdiemRow.PerdiemRateGov == 0)
                    perdiemRow.PerdiemRateGov = perdiemRow.PerdiemRate;
            }

            UpdateExpensePerdiemCalculateTransaction(perdiemRow.ExpensePerdiemID, txId); // Update Calculate Info from PerdiemItem and Create Invoice           
        }

        public void UpdateExpensePerdiemCalculateTransaction(long expensePerdiemID, Guid txId)
        {
            #region global variable

            double TotalAmount = 0;
            double TotalMainAmount = 0;
            double TotalLocalAmount = 0;

            double PerdiemRateUSD = 0;
            double PerdiemRateUSDHigh = 0;
            double PerdiemRateGov = 0;
            double PerdiemRateGovHigh = 0;
            double PerdiemRateUSDThaiZone = 0;

            double TotalFullDayPerdiem = 0;
            double FullDayPerdiemRate = 0;
            double TotalFullDayPerdiemAmount = 0;
            double TotalHalfDayPerdiem = 0;
            double HalfDayPerdiemRate = 0;
            double TotalHalfDayPerdiemAmount = 0;
            double PerdiemTotalAmount = 0;
            double TotalFullDayPerdiemHigh = 0;
            double FullDayPerdiemRateHigh = 0;
            double TotalFullDayPerdiemAmountHigh = 0;
            double TotalHalfDayPerdiemHigh = 0;
            double HalfDayPerdiemRateHigh = 0;
            double TotalHalfDayPerdiemAmountHigh = 0;
            double PerdiemTotalAmountHigh = 0;
            double PerdiemGovermentAmount = 0;
            double PerdiemPrivateAmount = 0;
            double PerdiemTaxAmount = 0;

            double PerdiemTotalAmountThaiZone = 0;
            double TotalFullDayPerdiemThaiZone = 0;
            double FullDayPerdiemRateThaiZone = 0;
            double TotalFullDayPerdiemAmountThaiZone = 0;
            double TotalHalfDayPerdiemThaiZone = 0;
            double HalfDayPerdiemRateThaiZone = 0;
            double TotalHalfDayPerdiemAmountThaiZone = 0;

            #endregion

            bool isRepOffice = false;
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(expensePerdiemID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);

            isRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiemID);

                double x = (double)0;
                double a = (double)0;
                double perdiemRate = (double)Math.Round(perdiemRow.PerdiemRate, 2, MidpointRounding.AwayFromZero);

                if (perdiemRate > 0 && perdiemRow.PerdiemRateGov == 0)
                    PerdiemRateGov = perdiemRate;
                else
                    PerdiemRateGov = (double)Math.Round(perdiemRow.PerdiemRateGov, 2, MidpointRounding.AwayFromZero);

                TotalFullDayPerdiem = (double)0;
                TotalHalfDayPerdiem = (double)0;
                TotalAmount = (double)0;
                TotalMainAmount = (double)0;
                TotalLocalAmount = (double)0;
                PerdiemGovermentAmount = (double)0;

                #region PerdiemRate
                FullDayPerdiemRate = perdiemRate;
                //HalfDayPerdiemRate = FullDayPerdiemRate / 2;
                #endregion

                foreach (DataRow dataRow in row)
                {
                    Double.TryParse(dataRow["NetDay"].ToString(), out x);
                    TotalFullDayPerdiem += x;
                    //Double.TryParse(dataRow["HalfDay"].ToString(), out a);
                    //TotalHalfDayPerdiem += a;
                    if (!isRepOffice)
                    {
                        TotalAmount += x * perdiemRate;
                        //TotalAmount += a * (perdiemRate / 2);

                        PerdiemGovermentAmount += x * PerdiemRateGov;
                        //PerdiemGovermentAmount += a * (PerdiemRateGov / 2);
                    }
                    else
                    {
                        TotalLocalAmount += x * perdiemRate;
                        //TotalLocalAmount += a * (perdiemRate / 2);

                        if (expRow.MainCurrencyID == expRow.LocalCurrencyID)
                        {
                            TotalMainAmount += (x * perdiemRate);
                            TotalMainAmount += a * (perdiemRate / 2);
                        }
                        else
                        {
                            TotalMainAmount += (x * perdiemRate) / (double)expRow.ExchangeRateForLocalCurrency;
                            //TotalMainAmount += (a * (perdiemRate / 2)) / (double)expRow.ExchangeRateForLocalCurrency;
                        }

                        TotalAmount += TotalMainAmount * (double)expRow.ExchangeRateMainToTHBCurrency;
                    }

                }

                TotalFullDayPerdiemAmount = (double)Math.Round((decimal)(TotalFullDayPerdiem * FullDayPerdiemRate), 2, MidpointRounding.AwayFromZero);
                TotalHalfDayPerdiemAmount = (double)Math.Round((decimal)(TotalHalfDayPerdiem * HalfDayPerdiemRate), 2, MidpointRounding.AwayFromZero);

                #region Summary
                PerdiemTotalAmount = (double)Math.Round((decimal)(TotalAmount), 2, MidpointRounding.AwayFromZero);// TotalFullDayPerdiemAmount + TotalHalfDayPerdiemAmount;
                if (!isRepOffice)
                {
                    if (PerdiemGovermentAmount > (PerdiemTotalAmount))
                        PerdiemGovermentAmount = PerdiemTotalAmount;

                    PerdiemTaxAmount = PerdiemTotalAmount - PerdiemGovermentAmount;
                }
                #endregion

            }
            else
            {

                #region PerdiemRate : Normal (USD/THB) & High (USD/THB)

                PerdiemRateUSD = (double)Math.Round(perdiemRow.PerdiemRateUSD, 2, MidpointRounding.AwayFromZero);
                PerdiemRateUSDHigh = (double)Math.Round(perdiemRow.PerdiemRateUSDHigh, 2, MidpointRounding.AwayFromZero);
                PerdiemRateGov = (double)Math.Round(perdiemRow.PerdiemRateGov, 2, MidpointRounding.AwayFromZero);
                PerdiemRateGovHigh = (double)Math.Round(perdiemRow.PerdiemRateGovHigh, 2, MidpointRounding.AwayFromZero);
                PerdiemRateUSDThaiZone = (double)Math.Round(perdiemRow.PerdiemRateUSDThaiZone, 2, MidpointRounding.AwayFromZero);

                #endregion

                #region FullDay/HalfDay && PerdiemGovermentAmount
                DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiemID);

                double a = (double)0;
                double b = (double)0;

                TotalFullDayPerdiem = (double)0;
                TotalHalfDayPerdiem = (double)0;
                TotalFullDayPerdiemHigh = (double)0;
                TotalHalfDayPerdiemHigh = (double)0;
                TotalFullDayPerdiemThaiZone = (double)0;
                TotalHalfDayPerdiemThaiZone = (double)0;
                PerdiemGovermentAmount = (double)0;

                #region PerdiemRate : Normal(USD) & High(USD) &Thai(USD)
                FullDayPerdiemRate = PerdiemRateUSD;
                FullDayPerdiemRateHigh = PerdiemRateUSDHigh;
                HalfDayPerdiemRate = FullDayPerdiemRate / 2;
                HalfDayPerdiemRateHigh = FullDayPerdiemRateHigh / 2;
                FullDayPerdiemRateThaiZone = PerdiemRateUSDThaiZone;
                HalfDayPerdiemRateThaiZone = FullDayPerdiemRateThaiZone / 2;
                #endregion

                #region Total PerdiemRate : Normal (USD/THB) & High (USD/THB) & Thai (USD/THB)
                double exchangeRateUSD = (double)perdiemRow.ExchangeRate;


                foreach (DataRow dataRow in row)
                {
                    if (dataRow["CountryZoneID"].ToString().Equals(((long)CountryZonePerdiem.HighZone).ToString()))
                    {
                        Double.TryParse(dataRow["FullDay"].ToString(), out a);
                        Double.TryParse(dataRow["HalfDay"].ToString(), out b);

                        TotalFullDayPerdiemHigh += a;
                        TotalHalfDayPerdiemHigh += b;
                        //PerdiemGovermentAmount += a * PerdiemRateGovHigh;
                        if (!isRepOffice)
                        {
                            PerdiemGovermentAmount += Math.Min(a * PerdiemRateGovHigh, a * FullDayPerdiemRateHigh * exchangeRateUSD);
                            PerdiemGovermentAmount +=
                                Math.Min(b * PerdiemRateGovHigh, b * HalfDayPerdiemRateHigh * exchangeRateUSD);
                        }
                    }
                    else
                    {
                        if (!isRepOffice)
                        {
                            Double.TryParse(dataRow["FullDay"].ToString(), out a);
                            Double.TryParse(dataRow["HalfDay"].ToString(), out b);

                            TotalFullDayPerdiem += a;
                            TotalHalfDayPerdiem += b;
                            //PerdiemGovermentAmount += a * PerdiemRateGov;
                            PerdiemGovermentAmount += Math.Min(a * PerdiemRateGov, a * FullDayPerdiemRate * exchangeRateUSD);
                            PerdiemGovermentAmount +=
                                Math.Min(b * PerdiemRateGov, b * HalfDayPerdiemRate * exchangeRateUSD);
                        }
                        else
                        {
                            if (dataRow["CountryZoneID"].ToString().Equals(((long)CountryZonePerdiem.ThaiZone).ToString()))
                            {
                                Double.TryParse(dataRow["FullDay"].ToString(), out a);
                                Double.TryParse(dataRow["HalfDay"].ToString(), out b);

                                TotalFullDayPerdiemThaiZone += a;
                                TotalHalfDayPerdiemThaiZone += b;
                            }
                            else
                            {
                                Double.TryParse(dataRow["FullDay"].ToString(), out a);
                                Double.TryParse(dataRow["HalfDay"].ToString(), out b);

                                TotalFullDayPerdiem += a;
                                TotalHalfDayPerdiem += b;
                            }
                        }
                    }
                }
                if (!isRepOffice)
                {
                    PerdiemGovermentAmount = (double)Math.Round((decimal)PerdiemGovermentAmount, 2, MidpointRounding.AwayFromZero);
                }

                #endregion

                #region PerdiemPrivateAmount
                if (!isRepOffice)
                {
                    DataRow[] pDetRow = ds.FnExpensePerdiemDetail.Select("ExpensePerdiemID = " + expensePerdiemID);

                    double c = (double)0;
                    double d = (double)0;

                    PerdiemPrivateAmount = (double)0;

                    foreach (DataRow dataRow in pDetRow)
                    {
                        Double.TryParse(dataRow["ExchangeRate"].ToString(), out c);
                        Double.TryParse(dataRow["Amount"].ToString(), out d);

                        PerdiemPrivateAmount += FnExpensePerdiemDetailService.CalculateAmountTHB(c, d);
                    }

                    PerdiemPrivateAmount = (double)Math.Round((decimal)PerdiemPrivateAmount, 2, MidpointRounding.AwayFromZero);
                }
                #endregion


                TotalFullDayPerdiemAmount = (double)Math.Round((decimal)(TotalFullDayPerdiem * FullDayPerdiemRate * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);
                TotalHalfDayPerdiemAmount = (double)Math.Round((decimal)(TotalHalfDayPerdiem * HalfDayPerdiemRate * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);

                TotalFullDayPerdiemAmountHigh = (double)Math.Round((decimal)(TotalFullDayPerdiemHigh * FullDayPerdiemRateHigh * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);
                TotalHalfDayPerdiemAmountHigh = (double)Math.Round((decimal)(TotalHalfDayPerdiemHigh * HalfDayPerdiemRateHigh * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);

                TotalFullDayPerdiemAmountThaiZone = (double)Math.Round((decimal)(TotalFullDayPerdiemThaiZone * FullDayPerdiemRateThaiZone * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);
                TotalHalfDayPerdiemAmountThaiZone = (double)Math.Round((decimal)(TotalHalfDayPerdiemThaiZone * HalfDayPerdiemRateThaiZone * exchangeRateUSD), 2, MidpointRounding.AwayFromZero);
                #endregion

                #region Summary
                PerdiemTotalAmount = TotalFullDayPerdiemAmount + TotalHalfDayPerdiemAmount;
                PerdiemTotalAmountHigh = TotalFullDayPerdiemAmountHigh + TotalHalfDayPerdiemAmountHigh;
                PerdiemTotalAmountThaiZone = TotalFullDayPerdiemAmountThaiZone + TotalHalfDayPerdiemAmountThaiZone;
                if (!isRepOffice)
                {
                    if (PerdiemGovermentAmount > (PerdiemTotalAmount + PerdiemTotalAmountHigh))
                        PerdiemGovermentAmount = PerdiemTotalAmount + PerdiemTotalAmountHigh;

                    PerdiemTaxAmount = PerdiemTotalAmount + PerdiemTotalAmountHigh - PerdiemGovermentAmount;

                    if (PerdiemPrivateAmount > PerdiemTaxAmount)
                        PerdiemPrivateAmount = PerdiemTaxAmount;

                    PerdiemTaxAmount -= PerdiemPrivateAmount;
                }
                #endregion
            }

            long? oldInvId = null;

            if (!perdiemRow.IsInvoiceIDNull()) oldInvId = perdiemRow.InvoiceID;

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                perdiemRow.TotalFullDayPerdiem = (decimal)TotalFullDayPerdiem; // Update Perdiem Domestic
                perdiemRow.FullDayPerdiemRate = Math.Round((decimal)FullDayPerdiemRate, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemAmount = Math.Round((decimal)TotalFullDayPerdiemAmount, 2, MidpointRounding.AwayFromZero);

                perdiemRow.TotalHalfDayPerdiem = (decimal)TotalHalfDayPerdiem;
                perdiemRow.HalfDayPerdiemRate = Math.Round((decimal)HalfDayPerdiemRate, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemAmount = Math.Round((decimal)TotalHalfDayPerdiemAmount, 2, MidpointRounding.AwayFromZero);

                perdiemRow.PerdiemTotalAmount = Math.Round((decimal)TotalAmount, 2, MidpointRounding.AwayFromZero); // Update Perdiem Domestic
                perdiemRow.PerdiemGovernmentAmount = Math.Round((decimal)PerdiemGovermentAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemTaxAmount = Math.Round((decimal)PerdiemTaxAmount, 2, MidpointRounding.AwayFromZero);

                if (isRepOffice)
                {
                    perdiemRow.PerdiemTotalAmountLocalCurrency = Math.Round((decimal)TotalLocalAmount, 2, MidpointRounding.AwayFromZero);
                    perdiemRow.PerdiemTotalAmountMainCurrency = Math.Round((decimal)TotalMainAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                perdiemRow.PerdiemRateUSD = Math.Round((decimal)PerdiemRateUSD, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemRateUSDHigh = Math.Round((decimal)PerdiemRateUSDHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemRateGov = Math.Round((decimal)PerdiemRateGov, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemRateGovHigh = Math.Round((decimal)PerdiemRateGovHigh, 2, MidpointRounding.AwayFromZero);

                perdiemRow.TotalFullDayPerdiem = Math.Round((decimal)TotalFullDayPerdiem, 2, MidpointRounding.AwayFromZero);
                perdiemRow.FullDayPerdiemRate = Math.Round((decimal)FullDayPerdiemRate, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemAmount = Math.Round((decimal)TotalFullDayPerdiemAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiem = Math.Round((decimal)TotalHalfDayPerdiem, 2, MidpointRounding.AwayFromZero);
                perdiemRow.HalfDayPerdiemRate = Math.Round((decimal)HalfDayPerdiemRate, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemAmount = Math.Round((decimal)TotalHalfDayPerdiemAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemTotalAmount = Math.Round((decimal)PerdiemTotalAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemHigh = Math.Round((decimal)TotalFullDayPerdiemHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.FullDayPerdiemRateHigh = Math.Round((decimal)FullDayPerdiemRateHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemAmountHigh = Math.Round((decimal)TotalFullDayPerdiemAmountHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemHigh = Math.Round((decimal)TotalHalfDayPerdiemHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.HalfDayPerdiemRateHigh = Math.Round((decimal)HalfDayPerdiemRateHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemAmountHigh = Math.Round((decimal)TotalHalfDayPerdiemAmountHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemTotalAmountHigh = Math.Round((decimal)PerdiemTotalAmountHigh, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemGovernmentAmount = Math.Round((decimal)PerdiemGovermentAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemPrivateAmount = Math.Round((decimal)PerdiemPrivateAmount, 2, MidpointRounding.AwayFromZero);
                perdiemRow.PerdiemTaxAmount = Math.Round((decimal)PerdiemTaxAmount, 2, MidpointRounding.AwayFromZero);

                perdiemRow.PerdiemTotalAmountThaiZone = Math.Round((decimal)PerdiemTotalAmountThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemThaiZone = Math.Round((decimal)TotalFullDayPerdiemThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.FullDayPerdiemRateThaiZone = Math.Round((decimal)FullDayPerdiemRateThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalFullDayPerdiemAmountThaiZone = Math.Round((decimal)TotalFullDayPerdiemAmountThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemThaiZone = Math.Round((decimal)TotalHalfDayPerdiemThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.HalfDayPerdiemRateThaiZone = Math.Round((decimal)HalfDayPerdiemRateThaiZone, 2, MidpointRounding.AwayFromZero);
                perdiemRow.TotalHalfDayPerdiemAmountThaiZone = Math.Round((decimal)TotalHalfDayPerdiemAmountThaiZone, 2, MidpointRounding.AwayFromZero);

                if (isRepOffice)
                {
                    double totalAmountAllZone = PerdiemTotalAmount + PerdiemTotalAmountHigh + PerdiemTotalAmountThaiZone;

                    if (expRow.IsExchangeRateForLocalCurrencyNull())
                    {
                        expRow.ExchangeRateForLocalCurrency = 0;
                    }
                    double totalAmountLocalAllZone = totalAmountAllZone;
                    perdiemRow.PerdiemTotalAmountLocalCurrency = Math.Round((decimal)totalAmountLocalAllZone, 2, MidpointRounding.AwayFromZero);

                    if (expRow.LocalCurrencyID == expRow.MainCurrencyID)
                    {
                        perdiemRow.PerdiemTotalAmountMainCurrency = Math.Round((decimal)totalAmountAllZone, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        perdiemRow.PerdiemTotalAmountMainCurrency = Math.Round(((decimal)totalAmountAllZone / expRow.ExchangeRateForLocalCurrency), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            long? invoiceId = CreateInvoiceTransaction(expensePerdiemID, txId);
            if (invoiceId.HasValue)
                perdiemRow.InvoiceID = invoiceId.Value;
            else
                perdiemRow.SetInvoiceIDNull();
            perdiemRow.UpdBy = UserAccount.UserID;
            perdiemRow.UpdDate = DateTime.Now.Date;

            if (oldInvId != null)
                DeleteInvoiceTransaction(oldInvId, txId);// Delete Old Invoice
        }
        #endregion

        #region private method
        private long? CreateInvoiceTransaction(long expensePerdiemID, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(expensePerdiemID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);

            // Add new invoice transaction
            FnExpenseInvoice invoice = new FnExpenseInvoice();
            bool IsRepOffice = expRow.IsIsRepOfficeNull() ? false : expRow.IsRepOffice;
            invoice.Expense = new FnExpenseDocument(perdiemRow.ExpenseID);
            invoice.InvoiceDocumentType = InvoiceType.Perdiem;
            invoice.Description = perdiemRow.Description;// Assign value of Invoice
            invoice.UpdPgm = perdiemRow.UpdPgm;

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                invoice.NetAmount = invoice.TotalAmount = invoice.TotalBaseAmount = (double)Math.Round(perdiemRow.PerdiemTotalAmount, 2, MidpointRounding.AwayFromZero);
                if (IsRepOffice)
                {
                    invoice.NetAmountLocalCurrency = invoice.TotalAmountLocalCurrency = invoice.TotalBaseAmountLocalCurrency = (double)Math.Round(perdiemRow.PerdiemTotalAmountLocalCurrency, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                if (!IsRepOffice)
                {
                    invoice.NetAmount = invoice.TotalAmount = invoice.TotalBaseAmount = (double)Math.Round((
                            perdiemRow.PerdiemGovernmentAmount +
                            perdiemRow.PerdiemPrivateAmount +
                            perdiemRow.PerdiemTaxAmount), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    invoice.NetAmount = invoice.TotalAmount = invoice.TotalBaseAmount = (double)Math.Round((
                            perdiemRow.PerdiemTotalAmount), 2, MidpointRounding.AwayFromZero);
                    invoice.NetAmountLocalCurrency = invoice.TotalAmountLocalCurrency = invoice.TotalBaseAmountLocalCurrency = (double)perdiemRow.PerdiemTotalAmountLocalCurrency;
                }
            }
            if (!IsRepOffice)
            {
                if (invoice.NetAmount != 0)
                {
                    long invoiceID = FnExpenseInvoiceService.AddInvoiceOnTransaction(invoice, txId);
                    try
                    {
                        DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(perdiemRow.CostCenterID);
                        DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(perdiemRow.AccountID);
                        if (expRow.ExpenseType == ZoneType.Domestic)
                        {
                            //Add invoice item transaction 2 row for Domestic               
                            //use domestic invoice to avoid currency require fields checking
                            if (perdiemRow.PerdiemGovernmentAmount > 0)
                                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(GetInvoiceItemObj(perdiemRow, account.AccountCode, invoiceID, (double)Math.Round(perdiemRow.PerdiemGovernmentAmount, 2, MidpointRounding.AwayFromZero)), txId, ZoneType.Domestic);

                            if (perdiemRow.PerdiemTaxAmount > 0)
                            {
                                FnExpenseInvoiceItem item = GetInvoiceItemObj(perdiemRow, ParameterServices.AccountPerdiem_DM, invoiceID, (double)Math.Round(perdiemRow.PerdiemTaxAmount, 2, MidpointRounding.AwayFromZero));
                                if (item.IO != null && !IsValidInternalOrder(item.IO.IONumber.Substring(4, 2)))
                                {
                                    item.IO = null;
                                }
                                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(item, txId, ZoneType.Domestic);
                            }
                        }
                        else
                        {
                            //Add invoice item transaction 3 row for Foreign               
                            //use domestic invoice to avoid currency require fields checking
                            if (perdiemRow.PerdiemGovernmentAmount > 0)
                                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(GetInvoiceItemObj(perdiemRow, account.AccountCode, invoiceID, (double)Math.Round(perdiemRow.PerdiemGovernmentAmount, 2, MidpointRounding.AwayFromZero)), txId, ZoneType.Domestic);
                            
                            if (perdiemRow.PerdiemTaxAmount > 0)
                            {
                                FnExpenseInvoiceItem item = GetInvoiceItemObj(perdiemRow, ParameterServices.AccountPerdiem, invoiceID, (double)Math.Round(perdiemRow.PerdiemTaxAmount, 2, MidpointRounding.AwayFromZero));
                                if (item.IO != null && !IsValidInternalOrder(item.IO.IONumber.Substring(4, 2)))
                                {
                                    item.IO = null;
                                }
                                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(item, txId, ZoneType.Domestic);
                            }
                            
                            if (perdiemRow.PerdiemPrivateAmount > 0)
                            {
                                FnExpenseInvoiceItem item = GetInvoiceItemObj(perdiemRow, ParameterServices.AccountInvoicePerdiem, invoiceID, (double)Math.Round(perdiemRow.PerdiemPrivateAmount, 2, MidpointRounding.AwayFromZero));
                                if (item.IO != null && !IsValidInternalOrder(item.IO.IONumber.Substring(4, 2)))
                                {
                                    item.IO = null;
                                }

                                FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(item, txId, ZoneType.Domestic);
                            }
                        }
                        return invoiceID;
                    }
                    catch
                    {
                        if (invoiceID != 0)
                            DeleteInvoiceTransaction(invoiceID, txId);
                        throw;
                    }
                }
            }
            else // RepOffice
            {
                long invoiceID = FnExpenseInvoiceService.AddInvoiceOnTransaction(invoice, txId);
                try
                {
                    DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(perdiemRow.CostCenterID);
                    DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindByIdentity(perdiemRow.AccountID);

                    if (perdiemRow.PerdiemTotalAmountLocalCurrency > 0)
                        FnExpenseInvoiceItemService.AddInvoiceItemOnTransaction(GetInvoiceItemObjForRepOffice(perdiemRow, account.AccountCode, invoiceID, (double)Math.Round(perdiemRow.PerdiemTotalAmountMainCurrency, 2, MidpointRounding.AwayFromZero), (double)Math.Round(perdiemRow.PerdiemTotalAmountLocalCurrency, 2, MidpointRounding.AwayFromZero), (double)Math.Round(perdiemRow.PerdiemTotalAmountMainCurrency * expRow.ExchangeRateMainToTHBCurrency, 2, MidpointRounding.AwayFromZero), expRow.LocalCurrencyID), txId, ZoneType.Domestic);

                    return invoiceID;
                }
                catch
                {
                    if (invoiceID != 0)
                        DeleteInvoiceTransaction(invoiceID, txId);
                    throw;
                }
            }
            return null;
        }
        private void DeleteInvoiceTransaction(long? invoiceID, Guid txId)
        {
            // delete old invoice transaction 
            if (invoiceID.HasValue)
                FnExpenseInvoiceService.DeleteInvoiceOnTransaction(invoiceID.Value, txId);
        }
        private FnExpenseInvoiceItem GetInvoiceItemObj(ExpenseDataSet.FnExpensePerdiemRow perdiemRow, string accountCode, long invoiceID, double amount)
        {
            FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem();

            invoiceItem.Invoice = new FnExpenseInvoice(invoiceID);
            invoiceItem.CostCenter = new DbCostCenter(perdiemRow.CostCenterID);
            DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(perdiemRow.CostCenterID);
            //string expenseGroup = string.Empty;
            //if (costCenter != null)
            //{
            //    expenseGroup = costCenter.CostCenterCode.Substring(3, 1).Equals("0") ? "0" : "1";
            //}
            invoiceItem.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(accountCode, null);
            if (!perdiemRow.IsIOIDNull()) invoiceItem.IO = ScgDbQueryProvider.DbIOQuery.FindByIdentity(perdiemRow.IOID);
            invoiceItem.SaleItem = perdiemRow.SaleItem;
            invoiceItem.SaleOrder = perdiemRow.SaleOrder;
            invoiceItem.Description = perdiemRow.Description;
            invoiceItem.ReferenceNo = perdiemRow.ReferenceNo;
            DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByCurrencySymbol(CurrencySymbol.THB.ToString(), true, false);
            invoiceItem.CurrencyID = (long)(currency == null ? ParameterServices.USDCurrencyID : currency.CurrencyID);
            invoiceItem.CurrencyAmount = amount;
            invoiceItem.NonDeductAmount = (double)0;
            invoiceItem.ExchangeRate = (double)1;
            invoiceItem.Amount = amount;
            invoiceItem.UpdPgm = perdiemRow.UpdPgm;

            return invoiceItem;
        }

        private FnExpenseInvoiceItem GetInvoiceItemObjForRepOffice(ExpenseDataSet.FnExpensePerdiemRow perdiemRow, string accountCode, long invoiceID, double amountMain, double amountLocal, double amountTHB, short? finalCurrencyID)
        {
            FnExpenseInvoiceItem invoiceItem = new FnExpenseInvoiceItem();
            invoiceItem.Invoice = new FnExpenseInvoice(invoiceID);
            invoiceItem.Invoice.Expense = new FnExpenseDocument(perdiemRow.ExpenseID);
            invoiceItem.CostCenter = new DbCostCenter(perdiemRow.CostCenterID);
            DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(perdiemRow.CostCenterID);
            //string expenseGroup = string.Empty;
            //if (costCenter != null)
            //{
            //    expenseGroup = costCenter.CostCenterCode.Substring(3, 1).Equals("0") ? "0" : "1";
            //}
            invoiceItem.Account = ScgDbQueryProvider.DbAccountQuery.FindAccountByAccountCodeExpenseGroup(accountCode, null);
            if (!perdiemRow.IsIOIDNull()) invoiceItem.IO = ScgDbQueryProvider.DbIOQuery.FindByIdentity(perdiemRow.IOID);
            invoiceItem.SaleItem = perdiemRow.SaleItem;
            invoiceItem.SaleOrder = perdiemRow.SaleOrder;
            invoiceItem.Description = perdiemRow.Description;
            invoiceItem.ReferenceNo = perdiemRow.ReferenceNo;
            invoiceItem.CurrencyID = (long)finalCurrencyID; //เอาค่ามาจาก LocalCurrencyID ใน expRow (สกุลเงินที่เบิก)
            invoiceItem.CurrencyAmount = amountLocal; //เอาค่าจาก Amount FinalCurrency 
            invoiceItem.LocalCurrencyAmount = amountLocal;
            invoiceItem.MainCurrencyAmount = amountMain;
            invoiceItem.NonDeductAmount = (double)0;
            invoiceItem.ExchangeRate = (double)1;
            invoiceItem.Amount = amountTHB;
            invoiceItem.UpdPgm = perdiemRow.UpdPgm;

            return invoiceItem;
        }

        private string GetPerdiemType(FnExpensePerdiem expPdm, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(expPdm.ExpensePerdiemID);
            return FnExpenseDocumentService.GetExpenseType(perRow.ExpenseID, txId);
        }
        private long GetRequesterID(long expenseId, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseDocumentRow expDocRow = ds.FnExpenseDocument.FindByExpenseID(expenseId);
            ExpenseDataSet.DocumentRow docRow = ds.Document.FindByDocumentID(expDocRow.DocumentID);

            if (!docRow.IsRequesterIDNull())
            {
                return docRow.RequesterID;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region Prepare date from DB to DataSet
        public void PrepareDataToDataset(ExpenseDataSet ds, long expenseId)
        {
            IList<FnExpensePerdiem> perdiemList = ScgeAccountingQueryProvider.FnExpensePerdiemQuery.GetPerdiemByExpenseID(expenseId);

            //#region Validate
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //if (perdiemList.Count == 0)
            //{
            //    errors.AddError("Error", new Spring.Validation.ErrorMessage("NoDocumentFound"));
            //}
            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            //#endregion

            foreach (FnExpensePerdiem perdiem in perdiemList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.NewFnExpensePerdiemRow();
                perdiemRow.ExpensePerdiemID = perdiem.ExpensePerdiemID;
                perdiemRow.ExpenseID = expenseId;
                if (perdiem.Invoice == null)
                    perdiemRow.SetInvoiceIDNull();
                else
                    perdiemRow.InvoiceID = perdiem.Invoice.InvoiceID;

                perdiemRow.Description = perdiem.Description;
                if (perdiem.PerdiemRateUSD.HasValue)
                {
                    perdiemRow.PerdiemRateUSD = (decimal)perdiem.PerdiemRateUSD;
                }
                if (perdiem.PerdiemRateUSDHigh.HasValue)
                {
                    perdiemRow.PerdiemRateUSDHigh = (decimal)perdiem.PerdiemRateUSDHigh;
                }

                perdiemRow.ExchangeRate = (decimal)perdiem.ExchangeRate;
                perdiemRow.TotalFullDayPerdiem = (decimal)perdiem.TotalFullDayPerdiem;
                perdiemRow.FullDayPerdiemRate = (decimal)perdiem.FullDayPerdiemRate;
                perdiemRow.TotalFullDayPerdiemAmount = (decimal)perdiem.TotalFullDayPerdiemAmount;
                perdiemRow.TotalHalfDayPerdiem = (decimal)perdiem.TotalHalfDayPerdiem;
                perdiemRow.HalfDayPerdiemRate = (decimal)perdiem.HalfDayPerdiemRate;
                perdiemRow.TotalHalfDayPerdiemAmount = (decimal)perdiem.TotalHalfDayPerdiemAmount;

                perdiemRow.PerdiemGovernmentAmount = (decimal)perdiem.PerdiemGovernmentAmount;
                perdiemRow.PerdiemPrivateAmount = (decimal)perdiem.PerdiemPrivateAmount;
                perdiemRow.PerdiemTaxAmount = (decimal)perdiem.PerdiemTaxAmount;
                perdiemRow.PerdiemRateGov = (decimal)perdiem.PerdiemRateGov;
                perdiemRow.PerdiemRateGovHigh = (decimal)perdiem.PerdiemRateGovHigh;
                perdiemRow.PerdiemTotalAmount = (decimal)perdiem.PerdiemTotalAmount;
                perdiemRow.PerdiemTotalAmountHigh = (decimal)perdiem.PerdiemTotalAmountHigh;

                perdiemRow.TotalFullDayPerdiemHigh = (decimal)perdiem.TotalFullDayPerdiemHigh;
                perdiemRow.FullDayPerdiemRateHigh = (decimal)perdiem.FullDayPerdiemRateHigh;
                perdiemRow.TotalFullDayPerdiemAmountHigh = (decimal)perdiem.TotalFullDayPerdiemAmountHigh;
                perdiemRow.TotalHalfDayPerdiemHigh = (decimal)perdiem.TotalHalfDayPerdiemHigh;
                perdiemRow.HalfDayPerdiemRateHigh = (decimal)perdiem.HalfDayPerdiemRateHigh;
                perdiemRow.TotalHalfDayPerdiemAmountHigh = (decimal)perdiem.TotalHalfDayPerdiemAmountHigh;

                if (perdiem.CostCenter != null)
                    perdiemRow.CostCenterID = perdiem.CostCenter.CostCenterID;

                if (perdiem.Account != null)
                    perdiemRow.AccountID = perdiem.Account.AccountID;

                if (perdiem.IO != null)
                    perdiemRow.IOID = perdiem.IO.IOID;

                perdiemRow.PerdiemRate = (decimal)perdiem.PerdiemRate;
                perdiemRow.SaleOrder = perdiem.SaleOrder;
                perdiemRow.SaleItem = perdiem.SaleItem;
                perdiemRow.ReferenceNo = perdiem.ReferenceNo;

                perdiemRow.Active = perdiem.Active;
                perdiemRow.CreBy = perdiem.CreBy;
                perdiemRow.CreDate = perdiem.CreDate;
                perdiemRow.UpdBy = perdiem.UpdBy;
                perdiemRow.UpdDate = perdiem.UpdDate;
                perdiemRow.UpdPgm = perdiem.UpdPgm;

                #region ReOffice

                perdiemRow.PerdiemRateUSDThaiZone = (decimal)perdiem.PerdiemRateUSDThaiZone;
                perdiemRow.PerdiemTotalAmountThaiZone = (decimal)perdiem.PerdiemTotalAmountThaiZone;
                perdiemRow.TotalFullDayPerdiemThaiZone = (decimal)perdiem.TotalFullDayPerdiemThaiZone;
                perdiemRow.TotalFullDayPerdiemAmountThaiZone = (decimal)perdiem.TotalFullDayPerdiemAmountThaiZone;
                perdiemRow.FullDayPerdiemRateThaiZone = (decimal)perdiem.FullDayPerdiemRateThaiZone;
                perdiemRow.HalfDayPerdiemRateThaiZone = (decimal)perdiem.HalfDayPerdiemRateThaiZone;
                perdiemRow.TotalHalfDayPerdiemThaiZone = (decimal)perdiem.TotalHalfDayPerdiemThaiZone;
                perdiemRow.TotalHalfDayPerdiemAmountThaiZone = (decimal)perdiem.TotalHalfDayPerdiemAmountThaiZone;
                perdiemRow.PerdiemTotalAmountLocalCurrency = (decimal)perdiem.PerdiemTotalAmountLocalCurrency;
                perdiemRow.PerdiemTotalAmountMainCurrency = (decimal)perdiem.PerdiemTotalAmountMainCurrency;

                #endregion

                // Add perdiem row to documentDataset.
                ds.FnExpensePerdiem.AddFnExpensePerdiemRow(perdiemRow);

                // Prepare Data to FnExpensePerdiemItem Datatable. 
                FnExpensePerdiemItemService.PrepareDataToDataset(ds, perdiemRow.ExpensePerdiemID);

                // Prepare Data to FnExpensePerdiemDetail Datatable. 
                FnExpensePerdiemDetailService.PrepareDataToDataset(ds, perdiemRow.ExpensePerdiemID);
            }
        }
        #endregion

        #region Call Persist
        //[Transaction]
        public void SaveExpensePerdiem(Guid txId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);
            ScgeAccountingDaoProvider.FnExpensePerdiemDao.Persist(expDS.FnExpensePerdiem);
            ScgeAccountingDaoProvider.FnExpensePerdiemItemDao.Persist(expDS.FnExpensePerdiemItem);
            ScgeAccountingDaoProvider.FnExpensePerdiemDetailDao.Persist(expDS.FnExpensePerdiemDetail);
        }
        #endregion

        public void ValidationDuplicateDateTimeline(long expensePerdiemID, Guid txId, FnExpensePerdiemItem perdiemItem)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(expensePerdiemID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            DateTime FromDateTime = FnExpensePerdiemItemService.ConvertDateTime(perdiemItem.FromDate, perdiemItem.FromTime);
            DateTime ToDateTime = FnExpensePerdiemItemService.ConvertDateTime(perdiemItem.ToDate, perdiemItem.ToTime);

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiemID);
                foreach (DataRow dataRow in row)
                {
                    long id = Convert.ToInt64(dataRow["perdiemItemID"]);
                    DateTime a = Convert.ToDateTime(dataRow["FromTime"]);
                    DateTime b = Convert.ToDateTime(dataRow["ToTime"]);
                    if (perdiemItem.PerdiemItemID != id)
                    {
                        if (FromDateTime >= a && FromDateTime < b)
                        {
                            errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("PediemDuplicateDateTimeline"));
                        }
                        else if (ToDateTime > a && FromDateTime <= a)
                        {
                            errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("PediemDuplicateDateTimeline"));
                        }
                    }
                    if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                }
            }
        }

        public void ValidationSaveDuplicateDateTimeline(Guid txId, string showError,long expenseId,bool isCopy)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perdiemRow = (ExpenseDataSet.FnExpensePerdiemRow)ds.FnExpensePerdiem.Select("ExpenseID = " + expenseId).FirstOrDefault();
            if (perdiemRow == null) return;
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(perdiemRow.ExpenseID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (isCopy)
                expenseId = -1;

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + perdiemRow.ExpensePerdiemID);
                foreach (DataRow dataRow in row)
                {
                    DateTime fromTime = Convert.ToDateTime(dataRow["FromTime"]);
                    DateTime toTime = Convert.ToDateTime(dataRow["ToTime"]);
                    Document checkLengthDate = FnExpensePerdiemQuery.CheckDateLength(expRow.DocumentRow.RequesterID, fromTime, expenseId);

                    if (checkLengthDate != null)
                    {
                        if (string.IsNullOrEmpty(checkLengthDate.DocumentNo))
                            checkLengthDate.DocumentNo = "Draft";

                        if (showError == "Perdiem")
                        {
                            errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("PediemAnotherDocumentDuplicateDateTimeline",new object[]{ checkLengthDate.DocumentNo }));
                        }
                        else
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PediemAnotherDocumentDuplicateDateTimeline", new object[] { checkLengthDate.DocumentNo }));
                        }
                    }
                    else 
                    {
                        ValidatePrediem date = FnExpensePerdiemQuery.GetPerdiemItemDate(expRow.DocumentRow.RequesterID, fromTime, expenseId);
                        if (date != null)
                        {
                            if (toTime > date.FromDate)
                            {
                                if (showError == "Perdiem")
                                {
                                    errors.AddError("Perdiem.Error", new Spring.Validation.ErrorMessage("PediemAnotherDocumentDuplicateDateTimeline", new object[] { date.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("PediemAnotherDocumentDuplicateDateTimeline", new object[] { date.DocumentNo }));
                                }
                            }
                        }
                    }
                    if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                }
            }
        }

        public void UpdatePerdiemItemByExpenseID(Guid txID, long perdiemId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("ExpenseID = {0}", perdiemId);
            DataRow[] rows = expDS.FnExpensePerdiem.Select(filter);

            foreach (DataRow row in rows)
            {
                FnExpensePerdiem perdiem = new FnExpensePerdiem();
                perdiem.LoadFromDataRow(row);

                //this.UpdateExpensePerdiemTransaction(perdiem, txID,);
            }
        }

        private bool IsValidInternalOrder(string ioType)
        {
            if (!string.IsNullOrEmpty(ioType) && (ioType.Contains("02") || ioType.Contains("03")
                || ioType.Contains("04") || ioType.Contains("09")))
            {
                return false;
            }
            return true;
        }
    }
}
