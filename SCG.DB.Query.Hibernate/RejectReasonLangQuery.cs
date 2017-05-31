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
    public class RejectReasonLangQuery : NHibernateQueryBase<DbRejectReasonLang, long>, IRejectReasonLangQuery
    {
        #region 
        
        public IList<VORejectReasonLang> FindAutoComplete(string reasonDetail, string documentType, short languageId)
        {
            reasonDetail = "%" + reasonDetail + "%";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, rl.ReasonId as ReasonId,r.ReasonCode as ReasonCode, ");
            sqlBuilder.Append(" rl.ReasonDetail as ReasonDetail, rl.Comment as Comment,rl.Active as Active,r.DocumentTypeCode as DocumentTypeCode");
            sqlBuilder.Append(" from RejectReason r  left join RejectReasonLang rl  on rl.ReasonID = r.ReasonID and rl.LanguageId = :LanguageId ");
            sqlBuilder.Append(" left join DbLanguage l on l.LanguageId = :LanguageId ");
            sqlBuilder.Append(" where rl.ReasonDetail Like :reasonDetail");
            sqlBuilder.Append(" and r.DocumentTypeCode = :documentType");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("reasonDetail", typeof(string), reasonDetail);
            parameterBuilder.AddParameterData("documentType", typeof(string), documentType);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("ReasonID", NHibernateUtil.Int16)
                .AddScalar("ReasonCode", NHibernateUtil.String)
                .AddScalar("ReasonDetail", NHibernateUtil.String)
                .AddScalar("DocumentTypeCode", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }
        public IList<VORejectReasonLang> FindReasonLangByReasonId(int reasonID)
        {

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageID as LanguageID, l.LanguageName as LanguageName, rl.ReasonID as ReasonID, ");
            sqlBuilder.Append(" rl.ReasonDetail as ReasonDetail, rl.Comment as Comment,rl.Active as Active");
            sqlBuilder.Append(" from DbLanguage l ");
            sqlBuilder.Append(" left join DbRejectReasonLang rl on l.LanguageID = rl.LanguageID and rl.ReasonID = :ReasonID ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("ReasonID",typeof(short),reasonID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("ReasonID", NHibernateUtil.Int32)
                .AddScalar("ReasonDetail", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(VORejectReasonLang))).List<VORejectReasonLang>();
        }

        #endregion

       
    }
}
