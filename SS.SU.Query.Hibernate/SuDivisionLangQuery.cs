using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;
using NHibernate.Transform;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query.Hibernate
{
    public class SuDivisionLangQuery : NHibernateQueryBase<SuDivisionLang, long>, ISuDivisionLangQuery
	{
		#region ISuDivisionLangQuery Members
		public IList<DivisionLang> FindDivisionLangByDivisionId(short divisionId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, dl.DivisionId as DivisionId,");
            sqlBuilder.Append(" dl.Id as DivisionLangId, dl.DivisionName as DivisionName, dl.Comment as Comment, dl.Active as Active");
            sqlBuilder.Append(" from DbLanguage l left join SuDivisionLang dl on dl.LanguageId = l.LanguageId and dl.divisionId = :DivisionId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("DivisionId", typeof(short), divisionId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("DivisionId", NHibernateUtil.Int16)
                .AddScalar("DivisionLangId", NHibernateUtil.Int64)
                .AddScalar("DivisionName", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.DivisionLang))).List<DivisionLang>();
        }

        public IList<DivisionLang> FindAutoComplete(string divisionName , short languageId , short organizationId)
        {
            divisionName = "%" + divisionName + "%";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select l.LanguageId as LanguageId, l.LanguageName as LanguageName, dl.DivisionId as DivisionId,");
            sqlBuilder.Append(" dl.Id as DivisionLangId, dl.DivisionName as DivisionName, dl.Comment as Comment, dl.Active as Active");
            sqlBuilder.Append(" from SuDivision d left join SuDivisionLang dl on dl.DivisionId = d.DivisionId and dl.LanguageId = :LanguageId ");
            sqlBuilder.Append(" left join DbLanguage l on l.LanguageId = :LanguageId ");
            sqlBuilder.Append(" where dl.DivisionName Like :DivisionName");
            sqlBuilder.Append(" and d.OrganizationId = :OrganizationId");
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);
            parameterBuilder.AddParameterData("DivisionName", typeof(string), divisionName);
            parameterBuilder.AddParameterData("OrganizationId", typeof(short), organizationId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageId", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("DivisionId", NHibernateUtil.Int16)
                .AddScalar("DivisionLangId", NHibernateUtil.Int64)
                .AddScalar("DivisionName", NHibernateUtil.String)
                .AddScalar("Comment", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SU.DTO.ValueObject.DivisionLang))).List<DivisionLang>();
        }
		public IList<SuDivisionLang> FindByDivisionIdAndLanguageId(short divisionId, short languageId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuDivisionLang divLang ");
			sqlBuilder.AppendLine(" WHERE divLang.Division.Divisionid = :divisionID ");
			sqlBuilder.AppendLine(" AND divLang.Language.Languageid = :languageID ");
			
			IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
			query.SetInt16("divisionID", divisionId);
			query.SetInt16("languageID", languageId);
			
			return query.List<SuDivisionLang>();
		}
		#endregion
	}
}
