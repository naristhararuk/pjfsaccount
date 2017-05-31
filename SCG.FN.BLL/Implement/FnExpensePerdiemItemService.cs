using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;
using System.Data;
using SCG.FN.DTO.DataSet;
using SCG.eAccounting.BLL;
using System.Web;

namespace SCG.FN.BLL.Implement
{
    public partial class FnExpensePerdiemItemService : ServiceBase<FnExpensePerdiemItem, long>, IFnExpensePerdiemItemService
    {
        public ITransactionService TransactionService { get; set; }
        public override IDao<FnExpensePerdiemItem, long> GetBaseDao()
        {
            return DaoProvider.FnExpensePerdiemItemDao;
        }

        public void AddExpensePerdiemItemOnTransaction(FnExpensePerdiemItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (item.FromDate == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromDate"));

            if (item.FromTime == null)
            {
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromTime"));
            }
            else
            {
                item.FromTime = new DateTime (item.FromDate.Year ,item.FromDate.Month ,item.FromDate.Day ,item.FromTime.Hour ,item.FromTime.Minute ,0);
            }

            if (item.ToDate == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDate"));

            if (item.ToTime == null)
            {
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToTime"));
            }
            else
            {
                item.ToTime = new DateTime(item.ToDate.Year, item.ToDate.Month, item.ToDate.Day, item.ToTime.Hour, item.ToTime.Minute, 0);
            }
               
            if (item.AdjustedDay == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredAdjustedDay"));
            if (item.AdjustedDay.Equals((decimal)0))
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredAdjustedDayNotEqualZero"));
            if (item.ToTime < item.FromTime)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDateIncorrect"));
          
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow row = ds.FnExpensePerdiemItem.NewFnExpensePerdiemItemRow();

            row.ExpensePerdiemID = item.ExpensePerdiemID.ExpensePerdiemID;
            row.FromDate = item.FromDate;
            row.FromTime = item.FromTime;
            row.ToDate = item.ToDate;
            row.ToTime = item.ToTime;
            row.AdjustedDay = item.AdjustedDay.Value;
            row.NetDay = ComputeNetDay(item.FromTime, item.ToTime, item.AdjustedDay.Value);

            row.CreBy = item.CreBy;
            row.CreDate = item.CreDate;
            row.UpdBy = item.UpdBy;
            row.UpdDate = item.UpdDate;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;

            ds.FnExpensePerdiemItem.AddFnExpensePerdiemItemRow(row);
        }
        public void UpdateExpensePerdiemItemOnTransaction(FnExpensePerdiemItem item, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (item.FromDate == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromDate"));

            if (item.FromTime == null)
            {
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredFromTime"));
            }
            else
            {
                item.FromTime = new DateTime(item.FromDate.Year, item.FromDate.Month, item.FromDate.Day, item.FromTime.Hour, item.FromTime.Minute, 0);
            }

            if (item.ToDate == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDate"));

            if (item.ToTime == null)
            {
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToTime"));
            }
            else
            {
                item.ToTime = new DateTime(item.ToDate.Year, item.ToDate.Month, item.ToDate.Day, item.ToTime.Hour, item.ToTime.Minute, 0);
            }

            if (item.AdjustedDay == null)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredAdjustedDay"));
            if (item.AdjustedDay.Equals((decimal)0))
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredAdjustedDayNotEqualZero"));
            if (item.ToTime < item.FromTime)
                errors.AddError("PerdiemItem.Error", new Spring.Validation.ErrorMessage("RequiredToDateIncorrect"));           

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow row = ds.FnExpensePerdiemItem.FindByPerdiemItemID(item.PerdiemItemID);

            row.BeginEdit();
            row.ExpensePerdiemID = item.ExpensePerdiemID.ExpensePerdiemID;
            row.FromDate = item.FromDate;
            row.FromTime = item.FromTime;
            row.ToDate = item.ToDate;
            row.ToTime = item.ToTime;
            row.AdjustedDay = item.AdjustedDay.Value;
            row.NetDay = ComputeNetDay(item.FromTime, item.ToTime, item.AdjustedDay.Value);

            row.CreBy = item.CreBy;
            row.CreDate = item.CreDate;
            row.UpdBy = item.UpdBy;
            row.UpdDate = item.UpdDate;
            row.UpdPgm = item.UpdPgm;
            row.Active = true;
            row.EndEdit();
            row.AcceptChanges();
        }
        public void DeleteExpensePerdiemItemOnTransaction(Guid txId, long itemId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpensePerdiemItemRow  row = expDs.FnExpensePerdiemItem.FindByPerdiemItemID(itemId);
            expDs.FnExpensePerdiemItem .RemoveFnExpensePerdiemItemRow(row);
        }
        public decimal ComputeTotalDay(DateTime fromDateTime, DateTime toDateTime)
        {
            decimal totalDay = 0;

            TimeSpan diffDate = toDateTime.Subtract(fromDateTime);
            return (decimal)diffDate.TotalDays;
        }
        public decimal ComputeNetDay(DateTime fromDateTime, DateTime toDateTime, decimal adjustedDay)
        {                       
            return (ComputeTotalDay(fromDateTime, toDateTime) - adjustedDay);
        }
        public decimal ComputeAmount(DateTime fromDateTime, DateTime toDateTime, decimal adjustedDay, decimal perdiemRate)
        {
           return  ComputeNetDay(fromDateTime, toDateTime, adjustedDay) * perdiemRate; 
        }
    }
}
