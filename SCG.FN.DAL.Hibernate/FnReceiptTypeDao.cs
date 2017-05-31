using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.FN.DAL;
using SCG.FN.DTO;
using NHibernate;
using SCG.GL.DTO;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.FN.DTO.ValueObject;
using NHibernate.Transform;

namespace SCG.FN.DAL.Hibernate
{
    public partial class FnReceiptTypeDao : NHibernateDaoBase<FnReceiptType, short>, IFnReceiptTypeDao
    {
        public FnReceiptTypeDao()
        {
        }

        public IQuery FindGlAccountByCriteria(string accNo, short languageId)
        {
            
            StringBuilder sql = new StringBuilder();
            sql.Append("select a.AccID as AccID, a.AccNo as AccNo,al.AccountName as AccName ");
            sql.Append("from GlAccount as a ");
            sql.Append("left join GlAccountlang as al on al.AccID = a.AccID and al.LanguageID = :LanguageID ");
            sql.Append("where a.AccNo like :AccNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
            queryParameterBuilder.AddParameterData("AccNo", typeof(string),"%"+ accNo +"%");
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("AccID", NHibernateUtil.Int16);
            query.AddScalar("AccNo", NHibernateUtil.String);
            query.AddScalar("AccName", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(FnReceiptTypeResult)));
            return query;
        }

        public bool IsDuplicateReceiptTypeCode(FnReceiptType fnReceiptType)
        {
            IList<FnReceiptType> list = GetCurrentSession().CreateQuery("from FnReceiptType r where r.ReceiptTypeID <> :ReceiptTypeID and r.ReceiptTypeCode = :ReceiptTypeCode")
                  .SetInt64("ReceiptTypeID", fnReceiptType.ReceiptTypeID)
                  .SetString("ReceiptTypeCode", fnReceiptType.ReceiptTypeCode)
                  .List<FnReceiptType>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
