using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO.DataSet;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseMileageItemService : IService<FnExpenseMileageItem, long>
    {
        void AddExpenseMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId);
        void UpdateExpenseMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId);   
        void DeleteExpenseMileageItemOnTransaction(Guid txId, long expenseMileageItemId);

        double ComputeDistanceTotal(double carMeterStart, double carMeterEnd);
        double ComputeFirstDistance(double carMeterStart, double carMeterEnd, string typeOfCar);
        double ComputeExceedDistance(double carMeterStart, double carMeterEnd, string typeOfCar);

        void SaveExpenseMileageItem(Guid txId, long expenseId);
        void PrepareDataToDataset(ExpenseDataSet ds, long mileageId);
        void UpdateMileageItemByMileageID(Guid txID, long mileageId, FnExpenseMileage mileage);
        void AddExpenseValidationMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId, long ExpDocumentID);
        void SaveExpenseValidationMileageItemOnTransaction( Guid txId, long expDocumentID,string showError,bool isCopy);
        void ValdationMileageRateByDataset(Guid txId, long expDocumentID, bool isCopy);
        void ValdationMileageRateByDataBase(long expDocumentID);
    }
}
