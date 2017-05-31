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
using SCG.DB.DTO;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnExpenseInvoiceQuery : NHibernateQueryBase<FnExpenseInvoice, long>, IFnExpenseInvoiceQuery
    {

        public double FindExpenseInvoiceByDocumentID(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select SUM(FnExpenseInvoice.NetAmount) as SumNetAmount ");
            sqlBuilder.Append(" from FnExpenseInvoice  LEFT OUTER JOIN ");
            sqlBuilder.Append(" ( select distinct FnExpenseInvoice.ExpenseID as EI ");
            sqlBuilder.Append(" from Document LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnRemittance ON Document.DocumentID = FnRemittance.DocumentID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnRemittanceAdvance ON FnRemittance.RemittanceID = FnRemittanceAdvance.RemittanceID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnExpenseAdvance ON FnRemittanceAdvance.AdvanceID = FnExpenseAdvance.AdvanceID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnExpenseInvoice ON FnExpenseAdvance.ExpenseID = FnExpenseInvoice.ExpenseID ");
            sqlBuilder.Append(" where FnRemittance.DocumentID = :documentID ");
            sqlBuilder.Append(" )A ON FnExpenseInvoice.ExpenseID = A.EI ");
            sqlBuilder.Append(" where FnExpenseInvoice.ExpenseID in ");
            sqlBuilder.Append(" ( select distinct FnExpenseInvoice.ExpenseID ");
            sqlBuilder.Append(" from Document LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnRemittance ON Document.DocumentID = FnRemittance.DocumentID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnRemittanceAdvance ON FnRemittance.RemittanceID = FnRemittanceAdvance.RemittanceID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnExpenseAdvance ON FnRemittanceAdvance.AdvanceID = FnExpenseAdvance.AdvanceID LEFT OUTER JOIN ");
            sqlBuilder.Append(" FnExpenseInvoice ON FnExpenseAdvance.ExpenseID = FnExpenseInvoice.ExpenseID ");
            sqlBuilder.Append(" where FnRemittance.DocumentID = :documentID) ");

            return GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString())
                .AddScalar("SumNetAmount", NHibernateUtil.Double)
                .SetInt64("documentID", documentID)
                .UniqueResult<double>();

        }

        public IList<InvoiceDataForEmail> FindInvoiceDataByExpenseID(long expenseID)
        {
            string sqlCommand = @"SELECT FnExpenseInvoice.InvoiceID, ROW_NUMBER() OVER(ORDER BY FnExpenseInvoice.CreDate ASC) AS ItemNo, FnExpenseInvoice.InvoiceNo, convert(varchar, FnExpenseInvoice.InvoiceDate,103) as InvoiceDate, 
            (FnExpenseInvoice.VendorCode + (case when NULLIF(FnExpenseInvoice.VendorCode,'') is null or NULLIF(FnExpenseInvoice.VendorName,'') is null then '' else ' - ' end) + FnExpenseInvoice.VendorName) as Vendor,
            case when ISNULL(FnExpenseDocument.IsRepOffice,0) = 0 
	            then convert(varchar,cast(FnExpenseInvoice.TotalBaseAmount as money),1)   
	            else convert(varchar,cast(FnExpenseInvoice.TotalBaseAmountLocalCurrency as money),1) end as BaseAmount,
            convert(varchar,cast(FnExpenseInvoice.VatAmount as money),1) as VatAmount, 
            convert(varchar,cast(FnExpenseInvoice.WHTAmount as money),1) as WHTAmount, 
            convert(varchar,cast(FnExpenseInvoice.NetAmount as money),1) as NetAmount
            FROM FnExpenseDocument with (nolock)
            INNER JOIN FnExpenseInvoice with (nolock) ON FnExpenseDocument.ExpenseID = FnExpenseInvoice.ExpenseID 
            WHERE FnExpenseInvoice.ExpenseID = :ExpenseID
            order by FnExpenseInvoice.CreDate ";
            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlCommand);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ExpenseID", typeof(long), expenseID);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("InvoiceID", NHibernateUtil.Int64);
            query.AddScalar("ItemNo", NHibernateUtil.Int32);
            query.AddScalar("InvoiceNo", NHibernateUtil.String);
            query.AddScalar("InvoiceDate", NHibernateUtil.String);
            query.AddScalar("BaseAmount", NHibernateUtil.String);
            query.AddScalar("VatAmount", NHibernateUtil.String);
            query.AddScalar("WHTAmount", NHibernateUtil.String);
            query.AddScalar("NetAmount", NHibernateUtil.String);
            query.AddScalar("Vendor", NHibernateUtil.String);
            
            query.SetResultTransformer(Transformers.AliasToBean(typeof(InvoiceDataForEmail)));

            return query.List<InvoiceDataForEmail>();
        }
        public IList<FnExpenseInvoice> GetInvoiceByExpenseID(long expenseId)
        {
            string sql = @" SELECT * FROM  FnExpenseInvoice with (nolock) where ExpenseID = :ExpenseID and Active = 1";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ExpenseID", typeof(long), expenseId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("InvoiceID", NHibernateUtil.Int64);
            query.AddScalar("InvoiceNo", NHibernateUtil.String);
            query.AddScalar("InvoiceDocumentType", NHibernateUtil.String);
            query.AddScalar("InvoiceDate", NHibernateUtil.DateTime);
            query.AddScalar("VendorCode", NHibernateUtil.String);
            query.AddScalar("VendorName", NHibernateUtil.String);
            query.AddScalar("Street", NHibernateUtil.String);
            query.AddScalar("City", NHibernateUtil.String);
            query.AddScalar("Country", NHibernateUtil.String);
            query.AddScalar("PostalCode", NHibernateUtil.String);
            query.AddScalar("TotalAmount", NHibernateUtil.Double);
            query.AddScalar("VatAmount", NHibernateUtil.Double);
            query.AddScalar("WHTAmount", NHibernateUtil.Double);
            query.AddScalar("NetAmount", NHibernateUtil.Double);
            query.AddScalar("Description", NHibernateUtil.String);
            query.AddScalar("IsVAT", NHibernateUtil.Boolean);
            query.AddScalar("IsWHT", NHibernateUtil.Boolean);
            query.AddScalar("TaxID", NHibernateUtil.Int64);
            query.AddScalar("NonDeductAmount", NHibernateUtil.Double);
            query.AddScalar("TotalBaseAmount", NHibernateUtil.Double);
            query.AddScalar("WHTRate1", NHibernateUtil.Double);
            query.AddScalar("WHTTypeID1", NHibernateUtil.Int64);
            query.AddScalar("BaseAmount1", NHibernateUtil.Double);
            query.AddScalar("WHTAmount1", NHibernateUtil.Double);
            query.AddScalar("WHTRate2", NHibernateUtil.Double);
            query.AddScalar("WHTTypeID2", NHibernateUtil.Int64);
            query.AddScalar("BaseAmount2", NHibernateUtil.Double);
            query.AddScalar("WHTAmount2", NHibernateUtil.Double);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("CreBy", NHibernateUtil.Int64);
            query.AddScalar("CreDate", NHibernateUtil.DateTime);
            query.AddScalar("UpdBy", NHibernateUtil.Int64);
            query.AddScalar("UpdDate", NHibernateUtil.DateTime);
            query.AddScalar("UpdPgm", NHibernateUtil.String);
            query.AddScalar("WHTID1", NHibernateUtil.Int64);
            query.AddScalar("WHTID2", NHibernateUtil.Int64);
            query.AddScalar("VendorTaxCode", NHibernateUtil.String);
            query.AddScalar("VendorBranch", NHibernateUtil.String);
            query.AddScalar("BranchCode", NHibernateUtil.String);
            query.AddScalar("VendorID", NHibernateUtil.Int64);
            query.AddEntity("Expense", typeof(FnExpenseDocument));
            //field for Rep Office
            query.AddScalar("TotalAmountLocalCurrency", NHibernateUtil.Double);
            query.AddScalar("TotalBaseAmountLocalCurrency", NHibernateUtil.Double);
            query.AddScalar("NetAmountLocalCurrency", NHibernateUtil.Double);
            query.AddScalar("ExchangeRateForLocalCurrency", NHibernateUtil.Double);
            query.AddScalar("ExchangeRateMainToTHBCurrency", NHibernateUtil.Double);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(FnExpenseInvoice)));

            return query.List<FnExpenseInvoice>();
        }
	}
}
