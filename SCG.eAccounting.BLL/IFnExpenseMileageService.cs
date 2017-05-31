using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseMileageService : IService<FnExpenseMileage, long>
    {
        long AddBeginRowExpenseMileageOnTransaction(FnExpenseMileage expenseMileage, Guid txId);
        void UpdateExpenseMileageOnTransaction(FnExpenseMileage expenseMileage, Guid txId);
        void PrepareDataToDataset(ExpenseDataSet ds, long expenseId);

        #region Calculate
        decimal CalculateDistanceAmount(decimal distanceTotal, decimal rate);
        decimal CalculateExceed100KmRate(decimal distanceTotal, decimal distanceFirst100km);
        decimal CalculateTotalAmount(decimal first100KmAmount, decimal exceed100KmAmount);
        decimal CalculateHelpingAmount(decimal distanceTotal, decimal rate);
        decimal CalculateOverHelpingAmount(decimal toTalAmount, decimal helpingAmount);

        decimal CalculateReimbursementAmount(decimal toTalAmount, decimal toTalNet, decimal toTalDistance);
        decimal CalculateNetDistance(decimal distanceTotal, decimal adjustTotal);
        decimal CalculateRemaining(decimal reimbursementAmount, decimal toTalNetAmount);          
        #endregion    

        void UpdateMileageSummary(Guid txId, FnExpenseMileage mileage);
        void UpdateMileageByExpenseID(Guid txID, long expDocumentId);

        
        void SaveExpenseMileage(Guid txID, long expenseId);
        void ValidRemaining(Guid txID, long expDocumentId);
        void DeleteMileage(Guid txID, long mileageId);
    }
}
