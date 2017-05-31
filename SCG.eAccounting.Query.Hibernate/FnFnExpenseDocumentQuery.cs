using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using System.Globalization;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseDocumentQuery : NHibernateQueryBase<FnExpenseDocument, long>, IFnExpenseDocumentQuery
    {
        public FnExpenseDocument GetExpenseDocumentByDocumentID(long documentID)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseDocument exp where exp.Document.DocumentID = :DocumentID")
                .SetInt64("DocumentID", documentID)
                .UniqueResult<FnExpenseDocument>();
        }

        public FnExpenseDataForEmail GetExpenseForEmailByDocumentID(long documentID)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" select doc.DocumentID as DocumentID, exp.ExpenseID, ");
            sql.AppendLine(" doc.DocumentNo as RequestNo , ");
            sql.AppendLine(" doc.Subject as Subject , ");
            sql.AppendLine(" exp.TotalExpense as TotalExpense, ");
            sql.AppendLine(" cr.Symbol as SymbolLocal, ");
            sql.AppendLine(" cr2.Symbol as SymbolMain, ");
            sql.AppendLine(" exp.TotalExpenseLocalCurrency as TotalExpenseLocal, ");
            sql.AppendLine(" exp.TotalExpenseMainCurrency as TotalExpenseMain, ");
            sql.AppendLine(" exp.IsRepOffice ,  exp.PBID ,  exp.DifferenceAmount,");
            sql.AppendLine(" exp.DifferenceAmountLocalCurrency ,exp.DifferenceAmountMainCurrency ");
            sql.AppendLine(" from FnExpenseDocument exp ");
            sql.AppendLine(" inner join Document doc on doc.DocumentID = exp.DocumentID  ");
            sql.AppendLine(" left join DbCurrency cr on cr.CurrencyID = exp.LocalCurrencyID ");
            sql.AppendLine(" left join DbCurrency cr2 on cr2.CurrencyID = exp.MainCurrencyID ");
            sql.AppendLine(" where doc.DocumentID = :documentID");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameter("documentID", documentID);

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("DocumentID", NHibernateUtil.Int64);
            query.AddScalar("ExpenseID", NHibernateUtil.Int64);
            query.AddScalar("RequestNo", NHibernateUtil.String);
            query.AddScalar("Subject", NHibernateUtil.String);
            query.AddScalar("TotalExpense", NHibernateUtil.Double);
            query.AddScalar("SymbolLocal", NHibernateUtil.String);
            query.AddScalar("SymbolMain", NHibernateUtil.String);
            query.AddScalar("TotalExpenseLocal", NHibernateUtil.Double);
            query.AddScalar("TotalExpenseMain", NHibernateUtil.Double);
            query.AddScalar("IsRepOffice", NHibernateUtil.Boolean);
            query.AddScalar("PBID", NHibernateUtil.Int64);
            query.AddScalar("DifferenceAmount", NHibernateUtil.Double);
            query.AddScalar("DifferenceAmountLocalCurrency", NHibernateUtil.Double);
            query.AddScalar("DifferenceAmountMainCurrency", NHibernateUtil.Double);
            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnExpenseDataForEmail))).UniqueResult<FnExpenseDataForEmail>();

        }

        public IList<ExportPayroll> GetExportPayrollList(DateTime date,string comCode,string Ordinal)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            DateTime startDate = date.AddMonths(-1).AddDays(15);
            DateTime endDate = date.AddDays(14);
            //sqlBuilder.Append("EXECUTE EXPENSE_PAYROLL_REPORT :com,:sdate,:edate,:ordinal");
            sqlBuilder.Append("EXECUTE EXPENSE_PAYROLL_REPORT '" + comCode + "','" + startDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "','" + endDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "','" + Ordinal+"'");
            //paramBuilder.AddParameterData("ordinal", typeof(string), Ordinal);
            //paramBuilder.AddParameterData("sdate", typeof(string), startDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            //paramBuilder.AddParameterData("edate", typeof(string), endDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            //paramBuilder.AddParameterData("comcode", typeof(string), comCode);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            //paramBuilder.FillParameters(query);
            query.AddScalar("CompanyCode", NHibernateUtil.String)
                .AddScalar("EmployeeCode", NHibernateUtil.String)
                .AddScalar("CostCenterCode", NHibernateUtil.String)
                .AddScalar("totalAmount", NHibernateUtil.Decimal)
                .AddScalar("wagecode", NHibernateUtil.String)
               .AddScalar("PayrollType", NHibernateUtil.String)
                .AddScalar("PeopleID",NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(ExportPayroll)));
            return query.List<ExportPayroll>();

        }

        public IList<ExportPayroll> GetExportPayrollListForInterface(DateTime date)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            DateTime startDate = date.AddMonths(-1).AddDays(15);
            DateTime endDate = date.AddDays(14);
            //sqlBuilder.Append("EXECUTE EXPENSE_PAYROLL_REPORT :com,:sdate,:edate,:ordinal");
            sqlBuilder.Append("EXECUTE EXPORT_PAYROLL '" + startDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "','" + endDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "'");
            //paramBuilder.AddParameterData("ordinal", typeof(string), Ordinal);
            //paramBuilder.AddParameterData("sdate", typeof(string), startDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            //paramBuilder.AddParameterData("edate", typeof(string), endDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            //paramBuilder.AddParameterData("comcode", typeof(string), comCode);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            //paramBuilder.FillParameters(query);
            query.AddScalar("CompanyCode", NHibernateUtil.String)
                .AddScalar("EmployeeCode", NHibernateUtil.String)
                .AddScalar("CostCenterCode", NHibernateUtil.String)
                .AddScalar("totalAmount", NHibernateUtil.Decimal)
                .AddScalar("wagecode", NHibernateUtil.String)
               .AddScalar("PayrollType", NHibernateUtil.String)
                .AddScalar("PeopleID", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(ExportPayroll)));
            return query.List<ExportPayroll>();

        }

        public string GetBoxIDByDocuemntID(long documentID)
        {
            FnExpenseDocument expDoc = GetExpenseDocumentByDocumentID(documentID);
            if (expDoc == null)
            {
                return string.Empty;
            }
            return expDoc.BoxID;
        }


        public IList<AdvanceData> FindAdvanceDataByExpenseID(long expenseID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select distinct Document.DocumentNo , AvAdvanceDocument.RequestDateOfAdvance, AvAdvanceDocument.Amount ");
            sqlBuilder.Append(" from FnExpenseDocument left outer join ");
            sqlBuilder.Append(" FnExpenseAdvance on FnExpenseDocument.ExpenseID = FnExpenseAdvance.ExpenseID left outer join ");
            sqlBuilder.Append(" AvAdvanceDocument on FnExpenseAdvance.AdvanceID = AvAdvanceDocument.AdvanceID left outer join ");
            sqlBuilder.Append(" Document on AvAdvanceDocument.DocumentID = Document.DocumentID ");
            sqlBuilder.Append(" WHERE FnExpenseDocument.ExpenseID = :ExpenseID");


            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ExpenseID", typeof(long), expenseID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("DocumentNo", NHibernateUtil.String);
            query.AddScalar("RequestDateOfAdvance", NHibernateUtil.DateTime);
            query.AddScalar("Amount", NHibernateUtil.Double);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceData)));

            return query.List<AdvanceData>();
        }

        public CostCenterData GetCostCenterData(long expenseID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select distinct DbCostCenter.CostCenterCode, DbCostCenter.Description ");
            sqlBuilder.Append("from FnExpenseDocument inner join ");
            sqlBuilder.Append("FnExpenseInvoice on FnExpenseDocument.ExpenseID = FnExpenseInvoice.ExpenseID inner join ");
            sqlBuilder.Append("FnExpenseInvoiceItem on FnExpenseInvoice.InvoiceID = FnExpenseInvoiceItem.InvoiceID inner join ");
            sqlBuilder.Append("DbCostCenter on FnExpenseInvoiceItem.CostCenterID = DbCostCenter.CostCenterID ");
            sqlBuilder.Append("where FnExpenseDocument.ExpenseID = :ExpenseID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ExpenseID", typeof(long), expenseID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("CostCenterCode", NHibernateUtil.String);
            query.AddScalar("Description", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(CostCenterData)));


            return query.UniqueResult<CostCenterData>();
        }

        public double GetSumAmountOfAdvanceDocument(long expenseID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select sum(AvAdvanceDocument.Amount) as SumAmount ");
            sqlBuilder.Append(" from  ");
            sqlBuilder.Append(" FnExpenseAdvance INNER JOIN ");
            sqlBuilder.Append(" AvAdvanceDocument ON FnExpenseAdvance.AdvanceID = AvAdvanceDocument.AdvanceID ");
            sqlBuilder.Append(" where FnExpenseAdvance.ExpenseID = :ExpenseID ");

            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("SumAmount", NHibernateUtil.Double)
                .SetInt64("ExpenseID", expenseID)
                .UniqueResult<double>();
        }

        public IList<FnExpenseDocument> GetTotalExpense(long remittanceID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select FnExpenseDocument.TotalExpense as TotalExpense ,FnExpenseDocument.PaymentType");
            sqlBuilder.Append(" from FnRemittance inner join ");
            sqlBuilder.Append(" FnExpenseDocument on FnRemittance.documentID = FnExpenseDocument.DocumentID ");
            sqlBuilder.Append(" where FnRemittance.RemittanceID = :remittanceID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("remittanceID", typeof(long), remittanceID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("TotalExpense", NHibernateUtil.Double);
            query.AddScalar("PaymentType", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnExpenseDocument))).List<FnExpenseDocument>();

        }

        public IList<FnExpenseDocument> GetPaymentType(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select FnExpenseDocument.PaymentType as PaymentType ");
            sqlBuilder.Append(" from Document inner join ");
            sqlBuilder.Append(" FnExpenseDocument on Document.DocumentID = FnExpenseDocument.DocumentID ");
            sqlBuilder.Append(" where Document.DocumentID = :documentID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("documentID", typeof(long), documentID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("PaymentType", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnExpenseDocument))).List<FnExpenseDocument>();

        }

        #region public IList<FnExpenseDocument> GetFnExpenseDocumentByDocumentID(long documentID)
        public IList<FnExpenseDocument> GetFnExpenseDocumentByDocumentID(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" select b.ExpenseID as ExpenseID ");
            sqlBuilder.AppendLine(" from Document a inner join ");
            sqlBuilder.AppendLine(" FnExpenseDocument b on a.DocumentID = b.DocumentID ");
            sqlBuilder.AppendLine(" where a.DocumentID = :documentID ");
            sqlBuilder.AppendLine(" and a.Active = 1 ");
            sqlBuilder.AppendLine(" and isnull(b.BoxID,'') <> '' ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("documentID", typeof(long), documentID);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("ExpenseID", NHibernateUtil.Int64);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnExpenseDocument))).List<FnExpenseDocument>();
        }
        #endregion public IList<FnExpenseDocument> GetFnExpenseDocumentByDocumentID(long documentID)

        public IList<FnExpenseDocument> FindExpenseReferenceTAByTADocumentID(long taDocumentID)
        {
            return GetCurrentSession().CreateQuery(" from FnExpenseDocument where Active = 1 and isnull(TADocumentID,'') <> '' and TADocumentID = :TADocumentID ")
                .SetInt64("TADocumentID", taDocumentID)
                .List<FnExpenseDocument>();
        }

        public IList<FnExpenseDocument> FindExpenseReferenceTAForRequesterByTADocumentID(long requesterID, long taDocumentID)
        {
            return GetCurrentSession().CreateQuery(@" from FnExpenseDocument exp where exp.Active = 1 and exp.TADocument.TADocumentID = :TADocumentID and exp.Document.RequesterID.Userid = :RequesterID ")
                    .SetInt64("TADocumentID", taDocumentID)
                    .SetInt64("RequesterID", requesterID)
                    .List<FnExpenseDocument>();
        }
    }
}
