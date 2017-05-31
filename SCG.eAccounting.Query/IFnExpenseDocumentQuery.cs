using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.eAccounting.DTO;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO.ValueObject;


namespace SCG.eAccounting.Query
{
    public interface IFnExpenseDocumentQuery : IQuery<FnExpenseDocument, long>
	{
        FnExpenseDocument GetExpenseDocumentByDocumentID(long documentID);
        FnExpenseDataForEmail GetExpenseForEmailByDocumentID(long documentID);
        CostCenterData GetCostCenterData(long expenseID);
        IList<FnExpenseDocument> GetTotalExpense(long remittanceID);
        IList<FnExpenseDocument> GetPaymentType(long documentID);
        IList<AdvanceData> FindAdvanceDataByExpenseID(long expenseID);
        double GetSumAmountOfAdvanceDocument(long expenseID);

        /// <summary>
        /// To Get All item of payroll that will be export.
        /// </summary>
        /// <param name="date">month and year specified in datetime object.</param>
        /// <returns>List of export payroll for payroll export machanism.</returns>
        IList<ExportPayroll> GetExportPayrollList(DateTime date,string comCode,string Ordinal);
        string GetBoxIDByDocuemntID(long DocumentID);
        IList<FnExpenseDocument> GetFnExpenseDocumentByDocumentID(long documentID);
        IList<FnExpenseDocument> FindExpenseReferenceTAByTADocumentID(long taDocumentID);
        IList<FnExpenseDocument> FindExpenseReferenceTAForRequesterByTADocumentID(long requesterID, long taDocumentID);
        IList<ExportPayroll> GetExportPayrollListForInterface(DateTime date);
    }
}
