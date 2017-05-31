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
    public interface IFnExpenseInvoiceService : IService<FnExpenseInvoice, long>
    {
        long AddInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId);
        //long AddBeginRowInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId);
        void DeleteInvoiceOnTransaction(long invoiceId, Guid txId);
        void PrepareDataToDataset(ExpenseDataSet ds, long expenseId);
        void SaveExpenseInvoice(Guid txId, long expenseId);
        void UpdateInvoiceOnTransaction(FnExpenseInvoice invoice, Guid txId);
        void UpdateInvoiceOnTransactionNonValidation(FnExpenseInvoice invoice, Guid txId);
        string ExportFilePayroll(string month, string year, string comCode,string Ordinal);

        #region For Calculate
        void ValidateTotalBaseAmount(Guid txId, long invoiceId, FnExpenseInvoice invoice);
		void ValidateCalculateInvoice(FnExpenseInvoice invoice);
        double CalculateVATAverage(double amount, double? rateNonDeduct);
        double CalculateVATAmount(double baseAmount, double? rate);
        double CalculateNetAmount(double baseAmount, double vatAmount, double whtAmount);
        double CalculateWHTAmount(double[] baseAmount, double[] whtRate);
        double CalculateWHTAmount(double baseAmount, double whtRate);
        double CalculateBaseAmount(Guid txID, long invoiceID);
        #endregion

        void UpdateInvoiceByExpenseID(Guid txID, long expenseId);
        void DeleteMileageInvoice(Guid txID);
    }
}
