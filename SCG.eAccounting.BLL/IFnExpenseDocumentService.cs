using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using System.Data;

namespace SCG.eAccounting.BLL
{
    public interface IFnExpenseDocumentService : IService<FnExpenseDocument, long>
    {
        DataSet PrepareExpenseDS();
        DataSet PrepareExpenseDS(long documentId);
        long AddExpenseDocumentTransaction(FnExpenseDocument exp, Guid txId);
        void UpdateExpenseDocumentTransaction(FnExpenseDocument exp, Guid txId);
        long SaveExpenseDocument(Guid txId, long expDocumentId);

        IList<object> GetEditableFields(long? documentID);
        IList<object> GetVisibleFields(long? documentID);

		// For calculate Advance ExchangeRate Amount.
		InvoiceExchangeRate GetAdvanceExchangeRate(Guid txID, short currencyID);

        //Advance Clearing
        void RefreshRemittance(Guid txID, long expID);
        void RefreshAdvance(Guid txID, long expID);

        void SetTA(Guid txID, long expID, Advance avCriteria);
        double AddExpenseAdvanceToTransaction(Guid txID, long expID, IList<Advance> advanceList,long taID);
        double DeleteExpenseAdvanceFromTransaction(Guid txID, long advanceID, double amount);
        void SetTotalAdvance(Guid txID, long taID, double totalAdvance);
        decimal CalculateTotalExpense(Guid txID, long expId, bool isRepOffice);
        void UpdateExpenseDocumentAdvanceToTransaction(FnExpenseDocument exp, Guid txId);
        void AddExpenseMPAToTransaction(Guid txID, long expID, IList<ExpensesMPA> ExpenseMPAList);
        void DeleteExpenseMPAFromTransaction(Guid txID, long MPADocumentID);
        void AddExpenseCAToTransaction(Guid txID, long expID, IList<ExpenseCA> ExpenseCAList);
        void DeleteExpenseCAFromTransaction(Guid txID, long CADocumentID);

        double GetExchangeRateForUSD(long expenseID, Guid txId);
        string GetExpenseType(long expenseID, Guid txId);

        void UpdateExpenseDocument(Guid txID, FnExpenseDocument exp);
        void CalculateDifferenceAmount(Guid txID, long expId, bool isRepOffice);

        double GetExpenseTotalVatAmount(Guid txId, long expenseId);
        double GetExpenseTotalWHTAmount(Guid txId, long expenseId);
        double GetExpenseTotalNetAmount(Guid txId, long expenseId);
        double GetExpenseTotalBaseAmount(Guid txId, long expenseId);

        void CanChangeCompany(Guid txId, long expenseId);
        DataSet PrepareInternalDataToDataset(long documentID, bool isCopy);
        void ValidateClearingAdvance(Guid txID, long expId);

        string GenerateDefaultBoxID(long documentId);

        // For calculate Advance ExchangeRate Amount (RepOffice).
        InvoiceExchangeRate GetAdvanceExchangeRateRepOffice(Guid txID, short currencyID, long expId);
    }
}
