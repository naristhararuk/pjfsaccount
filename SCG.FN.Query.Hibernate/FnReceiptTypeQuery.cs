using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SCG.FN.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SCG.FN.DTO.ValueObject;
using SS.DB.DTO;
using NHibernate.Expression;

namespace SCG.FN.Query.Hibernate
{
    public class FnReceiptTypeQuery : NHibernateQueryBase<FnReceiptType,short>,IFnReceiptTypeQuery
    {
        public ISQLQuery FindByReceiptTypeCriteria(FnReceiptTypeResult receiptType, string sortExpression, bool isCount, short languageId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select r.ReceiptTypeID as ReceiptTypeID,r.ReceiptTypeCode as ReceiptTypeCode,r.RecFlag as RecFlag,a.AccID as AccID,a.AccNo as AccNo,al.AccountName as AccName,r.Active as Active ");
                sqlBuilder.Append("from FnReceiptType as r ");
                sqlBuilder.Append("left join FnReceiptTypeLang as rl on r.ReceiptTypeID = rl.ReceiptTypeID and rl.LanguageID =:LanguageID ");
                sqlBuilder.Append("left join GlAccount as a on a.AccID = r.AccID ");
                sqlBuilder.Append("left join GlAccountLang as al on a.AccID = al.AccID and al.LanguageID =:LanguageID ");
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY ReceiptTypeID,ReceiptTypeCode,RecFlag,AccNo,AccName,Active");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
                }
            }
            else
            {
                sqlBuilder.Append("select count(r.ReceiptTypeID) as ReceiptTypeCount from FnReceiptType r ");
            }

            ISQLQuery query;

            if (!isCount)
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
                queryParameterBuilder.AddParameterData("LanguageID", typeof(Int16), languageId);
                queryParameterBuilder.FillParameters(query);
                query.AddScalar("ReceiptTypeID", NHibernateUtil.Int16);
                query.AddScalar("AccID", NHibernateUtil.Int16);
                query.AddScalar("ReceiptTypeCode", NHibernateUtil.String);
                query.AddScalar("RecFlag", NHibernateUtil.String);
                query.AddScalar("AccNo", NHibernateUtil.String);
                query.AddScalar("AccName", NHibernateUtil.String); 
                query.AddScalar("Active", NHibernateUtil.Boolean);


                query.SetResultTransformer(Transformers.AliasToBean(typeof(FnReceiptTypeResult)));

                //IList<SS.SU.DTO.ValueObject.RoleLang> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.ValueObject.RoleLang)))
                //    .List<SS.SU.DTO.ValueObject.RoleLang>();
            }
            else
            {
                query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
                query.AddScalar("ReceiptTypeCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }
        public IList<FnReceiptTypeResult> GetReceiptTypeList(FnReceiptTypeResult receiptType, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<FnReceiptTypeResult>(FnQueryProvider.FnReceiptTypeQuery, "FindByReceiptTypeCriteria", new object[] { receiptType, sortExpression, false, languageId }, firstResult, maxResult, sortExpression);
        }
        public int CountByReceiptTypeCriteria(FnReceiptTypeResult receiptType)
        {
            return NHibernateQueryHelper.CountByCriteria(FnQueryProvider.FnReceiptTypeQuery, "FindByReceiptTypeCriteria", new object[] { receiptType, string.Empty, true, Convert.ToInt16(0) });
        }


        public IList<FnReceiptTypeResult> FindReceiptTypeLangByID(short receiptTypeId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageID as LanguageID,l.LanguageName as LanguageName,rl.ReceiptTypeName as ReceiptTypeName,rl.Comment as Comment,rl.Active as Active ");
            sqlBuilder.Append("from DbLanguage as l ");
            sqlBuilder.Append("left join FnReceiptTypeLang as rl on rl.LanguageID = l.LanguageID and rl.ReceiptTypeID =:ReceiptTypeID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ReceiptTypeID", typeof(Int16), receiptTypeId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("LanguageID", NHibernateUtil.Int16);
            query.AddScalar("ReceiptTypeName", NHibernateUtil.String);
            query.AddScalar("LanguageName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(FnReceiptTypeResult))).List <FnReceiptTypeResult>();

        }

        public IList<DbStatus> FindAllRecFlag(string programCode)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbStatus), "s");
            criteria.Add(Expression.Eq("s.GroupStatus", programCode));
            return criteria.List<DbStatus>();
        }
        
    }
}
