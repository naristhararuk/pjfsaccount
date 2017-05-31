using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query.Hibernate
{
    public class DbReasonQuery : NHibernateQueryBase<DbScgReason, int>, IDbReasonQuery
    {
        public ISQLQuery FindReasonByDocumentTypeLanguageId(string documentType, short languageId, bool isCount, string sortExpression)
        {
            StringBuilder strQuery = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            ISQLQuery query;

            if (isCount)
            {
                strQuery.Append(" select count(*) as Count ");
                strQuery.Append(" from DbReason r ");
                strQuery.Append(" inner join DbReasonLang rl on r.ReasonID = rl.ReasonID and rl.LanguageID = :LanguageID ");
                strQuery.Append(" where r.DocumentTypeCode = :DocumentTypeCode ");

                parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
                parameterBuilder.AddParameterData("DocumentTypeCode", typeof(string), documentType);

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
                query.AddScalar("Count", NHibernateUtil.Int32);
            }
            else
            {
                strQuery.Append(" select r.ReasonID as ReasonID, r.ReasonCode as ReasonCode, r.DocumentTypeCode as DocumentTypeCode, r.Comment as Comment, r.Active as Active ");
                strQuery.Append(" from DbReason r ");
                strQuery.Append(" left join DbReasonLang rl on r.ReasonID = rl.ReasonID and rl.LanguageID = :LanguageID ");
                strQuery.Append(" where r.DocumentTypeCode = :DocumentTypeCode ");

                if (string.IsNullOrEmpty(sortExpression))
                {
                    strQuery.Append(" order by r.ReasonID, r.ReasonCode, r.Comment, r.Active ");
                }
                else
                {
                    strQuery.Append(String.Format(" order by {0} ", sortExpression));
                }

                parameterBuilder.AddParameterData("LanguageID", typeof(short), languageId);
                parameterBuilder.AddParameterData("DocumentTypeCode", typeof(string), documentType);

                query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
                parameterBuilder.FillParameters(query);
                query.AddScalar("ReasonID", NHibernateUtil.Int16)
                    .AddScalar("ReasonCode", NHibernateUtil.String)
                    .AddScalar("DocumentTypeCode", NHibernateUtil.String)
                    .AddScalar("Comment", NHibernateUtil.String)
                    .AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(DbScgReason)));
            }

            return query;
        }
        public IList<DbScgReason> GetReasonList(string documentType, short languageId, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<DbScgReason>(
                ScgDbQueryProvider.DbReasonQuery, "FindReasonByDocumentTypeLanguageId",
                new object[] { documentType, languageId, false, sortExpression }
                , firstResult, maxResult, sortExpression);
        }
        public int GetReasonCount(string documentType, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(
                ScgDbQueryProvider.DbReasonQuery,
                "FindReasonByDocumentTypeLanguageId",
                new object[] { documentType, languageId, true, string.Empty });
        }
        public IList<VORejectReasonLang> FindRejectReasonByDocumentTypeIDAndLanguageID(int documentTypeID,short languageID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT r.ReasonID AS ReasonID , r.ReasonCode AS ReasonCode , rl.ReasonDetail AS ReasonDetail ");
            sql.Append("FROM DbRejectReason AS r ");
            sql.Append("INNER JOIN DbRejectReasonLang AS rl ON rl.ReasonID = r.ReasonID ");
            sql.Append("WHERE r.DocumentTypeID =:documentTypeID AND rl.LanguageID =:languageID AND r.Active = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());

            QueryParameterBuilder queryParameter = new QueryParameterBuilder();
            queryParameter.AddParameterData("documentTypeID", typeof(int), documentTypeID);
            queryParameter.AddParameterData("languageID", typeof(int), languageID);
            queryParameter.FillParameters(query);

            query.AddScalar("ReasonID", NHibernateUtil.Int16);
            query.AddScalar("ReasonCode", NHibernateUtil.String);
            query.AddScalar("ReasonDetail", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }
    }
}
