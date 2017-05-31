using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.DB.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;
using SS.Standard.Utilities;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpensePerdiemItemService : ServiceBase<FnExpensePerdiemItem, long>, IFnExpensePerdiemItemService
    {
        #region global
        #endregion

        #region properties
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IFnExpensePerdiemService FnExpensePerdiemService { get; set; }
        public IUserAccount UserAccount { get; set; }
        #endregion

        #region method
        public override IDao<FnExpensePerdiemItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpensePerdiemItemDao;
        }
        public void AddExpensePerdiemItemTransaction(FnExpensePerdiemItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            string PerdiemType = GetPerdiemType(item, txId);

            if (!item.FromDate.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromDate"));
            if (!item.FromTime.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromTime"));
            if (!item.ToDate.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDate"));
            if (!item.ToTime.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToTime"));
            if (item.FromDate.HasValue && item.FromTime.HasValue && item.ToDate.HasValue && item.ToTime.HasValue)
            {
                DateTime FromDateTime = ConvertDateTime(item.FromDate, item.FromTime);
                DateTime ToDateTime = ConvertDateTime(item.ToDate, item.ToTime);

                if (DateTime.Compare(ToDateTime, DateTime.Now.Date) > 0)
                {
                    errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("PleaseSelectDateIsNotToday"));
                }
                if (ToDateTime < FromDateTime)
                {
                    errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDateIncorrect"));
                }
                else
                {
                    if (CalculateNetDay(CalculateTotalDay(FromDateTime, ToDateTime, PerdiemType), item.AdjustedDay) < 0)
                    {
                        errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                    }
                    if (PerdiemType == ZoneType.Foreign)
                    {
                        if (item.HalfDay < 0)
                        {
                            errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                        }
                        else
                        {
                            if (CalculateFullDay(CalculateNetDay(CalculateTotalDay(FromDateTime, ToDateTime, PerdiemType), item.AdjustedDay), item.HalfDay) < 0)
                                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                        }
                    }
                }
            }
            if (PerdiemType == ZoneType.Foreign)
            {
                if (!item.CountryID.HasValue && string.IsNullOrEmpty(item.Remark))
                {
                    errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Remark is required"));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow row = ds.FnExpensePerdiemItem.NewFnExpensePerdiemItemRow();

            row.ExpensePerdiemID = item.ExpensePerdiem.ExpensePerdiemID;
            row.FromDate = item.FromDate.Value;
            row.FromTime = ConvertDateTime(item.FromDate.Value, item.FromTime.Value);
            row.ToDate = item.ToDate.Value;
            row.ToTime = ConvertDateTime(item.ToDate, item.ToTime);
            row.AdjustedDay = (decimal)item.AdjustedDay;
            row.NetDay = (decimal)CalculateNetDay(CalculateTotalDay(row.FromTime, row.ToTime, PerdiemType), item.AdjustedDay);

            //** for Domestic and Foreign
            row.Remark = item.Remark;

            if (PerdiemType == ZoneType.Foreign)
            {
                row.HalfDay = (decimal)item.HalfDay;
                row.FullDay = (decimal)CalculateFullDay((double)row.NetDay, (double)row.HalfDay);

                if (item.CountryID.HasValue)
                    row.CountryID = item.CountryID.Value;
                if (item.CountryZoneID.HasValue)
                {
                    row.CountryZoneID = item.CountryZoneID.Value;
                }
            }
            row.CreBy = row.UpdBy = UserAccount.UserID;
            row.CreDate = row.UpdDate = DateTime.Now.Date;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;

            ds.FnExpensePerdiemItem.AddFnExpensePerdiemItemRow(row);
            FnExpensePerdiemService.UpdateExpensePerdiemCalculateTransaction(item.ExpensePerdiem.ExpensePerdiemID, txId);
        }
        public void UpdateExpensePerdiemItemTransaction(FnExpensePerdiemItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            string PerdiemType = GetPerdiemType(item, txId);

            if (!item.FromDate.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromDate"));
            if (!item.FromTime.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromTime"));
            if (!item.ToDate.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDate"));
            if (!item.ToTime.HasValue)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToTime"));
            if (item.FromDate.HasValue && item.FromTime.HasValue && item.ToDate.HasValue && item.ToTime.HasValue)
            {
                DateTime FromDateTime = ConvertDateTime(item.FromDate, item.FromTime);
                DateTime ToDateTime = ConvertDateTime(item.ToDate, item.ToTime);

                if (ToDateTime < FromDateTime)
                {
                    errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDateIncorrect"));
                }
                else
                {
                    if (CalculateNetDay(CalculateTotalDay(FromDateTime, ToDateTime, PerdiemType), item.AdjustedDay) < 0)
                    {
                        errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                    }
                    if (PerdiemType == ZoneType.Foreign)
                    {
                        if (item.HalfDay < 0)
                        {
                            errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                        }
                        else
                        {
                            if (CalculateFullDay(CalculateNetDay(CalculateTotalDay(FromDateTime, ToDateTime, PerdiemType), item.AdjustedDay), item.HalfDay) < 0)
                                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Invalid Date Information"));
                        }
                    }
                }
            }
            if (PerdiemType == ZoneType.Foreign)
            {
                if (!item.CountryID.HasValue && string.IsNullOrEmpty(item.Remark))
                {
                    errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("Remark is required"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow row = ds.FnExpensePerdiemItem.FindByPerdiemItemID(item.PerdiemItemID);

            row.ExpensePerdiemID = item.ExpensePerdiem.ExpensePerdiemID;
            row.FromDate = item.FromDate.Value;
            row.FromTime = ConvertDateTime(item.FromDate.Value, item.FromTime.Value);
            row.ToDate = item.ToDate.Value;
            row.ToTime = ConvertDateTime(item.ToDate, item.ToTime);
            row.AdjustedDay = (decimal)item.AdjustedDay;
            row.NetDay = (decimal)CalculateNetDay(CalculateTotalDay(row.FromTime, row.ToTime, PerdiemType), item.AdjustedDay);

            //for Domestic and Foreign
            row.Remark = item.Remark;

            if (PerdiemType == ZoneType.Foreign)
            {
                row.HalfDay = (decimal)item.HalfDay;
                row.FullDay = (decimal)CalculateFullDay((double)row.NetDay, (double)row.HalfDay);

                if (item.CountryID.HasValue)
                    row.CountryID = item.CountryID.Value;
                else
                    row.SetCountryIDNull();
                row.CountryZoneID = item.CountryZoneID.Value;
            }

            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now.Date;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;
            FnExpensePerdiemService.UpdateExpensePerdiemCalculateTransaction(item.ExpensePerdiem.ExpensePerdiemID, txId);
        }
        public void DeleteExpensePerdiemItemTransaction(long itemId, Guid txId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow row = expDs.FnExpensePerdiemItem.FindByPerdiemItemID(itemId);

            long expensePerdiemID = row.ExpensePerdiemID;

            row.Delete();
            FnExpensePerdiemService.UpdateExpensePerdiemCalculateTransaction(expensePerdiemID, txId);
        }
        #endregion

        #region private method
        private string GetPerdiemType(FnExpensePerdiemItem item, Guid txId)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemRow perRow = ds.FnExpensePerdiem.FindByExpensePerdiemID(item.ExpensePerdiem.ExpensePerdiemID);
            return FnExpenseDocumentService.GetExpenseType(perRow.ExpenseID, txId);
        }
        #endregion

        #region Calculate
        public DateTime ConvertDateTime(DateTime? dd, DateTime? tt)
        {
            return new DateTime(dd.Value.Year, dd.Value.Month, dd.Value.Day, tt.Value.Hour, tt.Value.Minute, 0);
        }
        public double CalculateTotalDay(DateTime fromDateTime, DateTime toDateTime, string perdiemType)
        {
            double toTalDay = (double)0;

            if (perdiemType == ZoneType.Domestic)
            {
                double totalMinutes = toDateTime.Subtract(fromDateTime).TotalMinutes;
                double totalHours = toDateTime.Subtract(fromDateTime).TotalHours;
                int intDay = ((int)totalHours / 24);
                int intHours = ((int)totalHours % 24);
                int intMinutes = ((int)totalMinutes % 60);
                double douHours = Convert.ToDouble(intHours.ToString("00") + "." + intMinutes.ToString("00"));

                if (douHours <= 6.00)
                    toTalDay = intDay;
                else if (douHours > 6.00 && douHours <= 12.00)
                    toTalDay = intDay + 0.5;
                else if (douHours > 12.00 && douHours <= 24.00)
                    toTalDay = intDay + 1;
                else if (douHours > 24.00)
                    toTalDay = intDay + 1;
            }
            else
            {
                toTalDay = toDateTime.Subtract(fromDateTime).TotalDays;
                toTalDay += 1;
            }

            return toTalDay;
        }
        public double CalculateNetDay(double toTalDay, double adjustedDay)
        {
            return (toTalDay - adjustedDay);
        }
        public double CalculateAmount(double netDay, double perdiemRate)
        {
            return netDay * perdiemRate;
        }
        public double CalculateLocalAmount(double netDay, double perdiemRate, double exchangeRateLocalCurrency)
        {
            return (netDay * perdiemRate) * exchangeRateLocalCurrency;
        }
        public double CalculateFullDay(double netDay, double halfDay)
        {
            return (netDay - halfDay);
        }
        public double CalculateTotalFullDay(long expensePerdiemID, Guid txId)
        {
            double toTalFullDay = (double)0;
            double x = (double)0;

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiemID);

            foreach (DataRow dataRow in row)
            {
                Double.TryParse(dataRow["FullDay"].ToString(), out x);
                toTalFullDay += x;
            }

            return toTalFullDay;
        }
        public double CalculateTotalHalfDay(long expensePerdiemID, Guid txId)
        {
            double toTalHalfDay = (double)0;
            double x = (double)0;

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            DataRow[] row = ds.FnExpensePerdiemItem.Select("ExpensePerdiemID = " + expensePerdiemID);

            foreach (DataRow dataRow in row)
            {
                Double.TryParse(dataRow["HalfDay"].ToString(), out x);
                toTalHalfDay += x;
            }

            return toTalHalfDay;
        }
        #endregion

        public void PrepareDataToDataset(ExpenseDataSet ds, long perdiemId)
        {
            ExpenseDataSet.FnExpensePerdiemRow row = ds.FnExpensePerdiem.FindByExpensePerdiemID(perdiemId);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(row.ExpenseID);
            string PerdiemType = expRow.ExpenseType;

            IList<FnExpensePerdiemItem> perdiemItemList = ScgeAccountingQueryProvider.FnExpensePerdiemItemQuery.GetPerdiemItemByPerdiemID(perdiemId);

            decimal totalNetDay = 0;

            foreach (FnExpensePerdiemItem item in perdiemItemList)
            {
                // Set data to perdiem item row in Dataset.
                ExpenseDataSet.FnExpensePerdiemItemRow itemRow = ds.FnExpensePerdiemItem.NewFnExpensePerdiemItemRow();
                itemRow.PerdiemItemID = item.PerdiemItemID;
                itemRow.ExpensePerdiemID = item.ExpensePerdiem.ExpensePerdiemID;
                itemRow.FromDate = item.FromDate.Value;
                itemRow.FromTime = ConvertDateTime(item.FromDate.Value, item.FromTime.Value);
                itemRow.ToDate = item.ToDate.Value;
                itemRow.ToTime = ConvertDateTime(item.ToDate, item.ToTime);
                itemRow.AdjustedDay = (decimal)item.AdjustedDay;
                itemRow.NetDay = (decimal)item.NetDay;

                totalNetDay += itemRow.NetDay;

                if (PerdiemType == ZoneType.Foreign)
                {
                    itemRow.HalfDay = (decimal)item.HalfDay;
                    itemRow.FullDay = (decimal)item.FullDay;
                    itemRow.SetCountryIDNull();
                    if (item.CountryID != null)
                        itemRow.CountryID = item.CountryID.Value;
                    itemRow.SetCountryZoneIDNull();
                    if (item.CountryZoneID != null)
                        itemRow.CountryZoneID = item.CountryZoneID.Value;
                }

                itemRow.Remark = item.Remark;


                itemRow.Active = item.Active;
                itemRow.CreBy = item.CreBy;
                itemRow.CreDate = item.CreDate;
                itemRow.UpdBy = item.UpdBy;
                itemRow.UpdDate = item.UpdDate;
                itemRow.UpdPgm = item.UpdPgm;

                // Add perdiem item row to documentDataset.
                ds.FnExpensePerdiemItem.AddFnExpensePerdiemItemRow(itemRow);
            }

            if (PerdiemType == ZoneType.Domestic)
            {
                row.TotalFullDayPerdiem = totalNetDay;
                row.TotalFullDayPerdiemAmount = totalNetDay * row.FullDayPerdiemRate;
            }
        }

        public void UpdatePerdiemItemByPerdiemID(Guid txID, long perdiemId)
        {
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("PerdiemID = {0}", perdiemId);
            DataRow[] rows = expDS.FnExpensePerdiemItem.Select(filter);

            foreach (DataRow row in rows)
            {
                FnExpensePerdiemItem item = new FnExpensePerdiemItem();
                item.LoadFromDataRow(row);

                //this.UpdateExpensePerdiemItemTransaction(item,);
            }
        }
    }
}
