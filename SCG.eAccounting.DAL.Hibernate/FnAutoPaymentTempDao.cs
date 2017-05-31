using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial  class FnAutoPaymentTempDao :NHibernateDaoBase<FnAutoPaymentTemp,long>,  IFnAutoPaymentTempDao
    {

        #region IFnAutoPaymentTempDao Members

        public bool CommitToAutoPayment(long documentID)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE FnAutoPayment ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("Status = t1.status, ");
            sqlBuilder.Append("chequenumber = t2.chequenumber, ");
            sqlBuilder.Append("chequeBankName = t2.chequeBankName, ");
            sqlBuilder.Append("chequeDate = t2.chequeDate, ");
            sqlBuilder.Append("PayeeBankAccountNumber = t2.PayeeBankAccountNumber, ");
            sqlBuilder.Append("PayeeBankName = t2.PayeeBankName, ");
            sqlBuilder.Append("Amount = t1.Amount, ");
            sqlBuilder.Append("PaymentDate = t2.PaymentDate, ");
            sqlBuilder.Append("UpdDate = t2.UpdDate,");
            sqlBuilder.Append("CurrencyDoc = t2.CurrencyDoc,    ");
            sqlBuilder.Append("CurrencyPay = t2.CurrencyPay ");
            sqlBuilder.Append("FROM  FnAutoPaymentTemp t1 ");
            sqlBuilder.Append("INNER JOIN FnAutoPaymentTemp t2  ");
            sqlBuilder.Append("ON t1.ClearingDocID = t2.FIDOC and t1.companycode = t2.companycode ");
            sqlBuilder.Append("WHERE FnAutoPayment.FIDoc = t1.FIDoc and FnAutoPayment.DocumentID = :DocumentID ");
            sqlBuilder.Append(" and FnAutoPayment.companycode = t1.companycode ");
            //to do resolve.
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetInt64("DocumentID", documentID);
            query.AddScalar("TCount", NHibernateUtil.Int32).UniqueResult();

            return IsUpdateSuccess(documentID);

        }

        private bool IsUpdateSuccess(long documentID)
        {
            StringBuilder builder = new StringBuilder();
            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("documentid", typeof(long), documentID);
            builder.Append("select count(1) as ccnt from fnautopayment where status = 2 and documentid = :documentid");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(builder.ToString());
            param.FillParameters(query);
            query.AddScalar("ccnt", NHibernateUtil.Int32);
            int i = Convert.ToInt32(query.UniqueResult());
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteAll()
        {

            ISQLQuery query = GetCurrentSession().CreateSQLQuery("truncate table FnAutoPaymentTemp");
            query.AddScalar("result", NHibernateUtil.Int32);
            query.UniqueResult();

        }

        public bool IsSuccess(long documentID)
        {
            StringBuilder builder = new StringBuilder();
            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("documentid", typeof(long), documentID);
            builder.Append("select count(1) as ccnt from fnautopayment where status = 2 and documentid = :documentid");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(builder.ToString());
            param.FillParameters(query);
            query.AddScalar("ccnt", NHibernateUtil.Int32);
            int i = Convert.ToInt32( query.UniqueResult());
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }



        }
        
        #endregion


    }
}
