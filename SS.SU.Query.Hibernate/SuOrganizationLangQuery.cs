using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Expression;
using NHibernate.Transform;
using NHibernate;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

namespace SS.SU.Query.Hibernate
{
    public class SuOrganizationLangQuery : NHibernateQueryBase<SuOrganizationLang, long>, ISuOrganizationLangQuery
    {
        #region ISuOrganizationQuery Members (Old)
        //public IList<SuOrganization_SuOrganizationLang> FindByLanguageId(int languageId)
        //{
        //    StringBuilder strQuery = new StringBuilder();
        //    strQuery.AppendLine(" SELECT org.OrganizationID,orgLang.OrganizationName FROM SuOrganization org ");
        //    strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
        //    strQuery.AppendLine(" ON org.OrganizationID = orgLang.OrganizationID ");
        //    strQuery.AppendLine(" WHERE orgLang.LanguageID = :LanguageID ");
        //    strQuery.AppendLine(" ORDER BY orgLang.OrganizationName ");

        //    return GetCurrentSession().CreateSQLQuery(strQuery.ToString())
        //            .AddEntity(typeof(SuOrganization_SuOrganizationLang))
        //            .SetInt32("LanguageID", languageId)
        //            .List<SuOrganization_SuOrganizationLang>();
        //}

		//public IList<SuOrganization_SuOrganizationLang> FindByLanguageId(int languageId)
		//{

		//    StringBuilder strQuery = new StringBuilder();
		//    strQuery.AppendLine(" SELECT org.*,orgLang.* FROM SuOrganization org ");
		//    strQuery.AppendLine(" INNER JOIN SuOrganizationLang orgLang ");
		//    strQuery.AppendLine(" ON org.OrganizationID = orgLang.OrganizationID ");
		//    strQuery.AppendLine(" WHERE orgLang.LanguageID = :LanguageID ");
		//    strQuery.AppendLine(" ORDER BY orgLang.OrganizationName ");

		//    return GetCurrentSession().CreateSQLQuery(strQuery.ToString())
		//            .AddEntity(typeof(SuOrganization_SuOrganizationLang))
		//            .SetInt32("LanguageID", languageId)
		//            .List<SuOrganization_SuOrganizationLang>();
		//}
        #endregion
        
		#region ISuOrganizationLangQuery Members
		public IList<SuOrganizationWithLang> FindByLanguageId(short languageId)
		{
			//throw new NotImplementedException();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT org.OrganizationID, org.OrganizationName ");
            sqlBuilder.Append(" FROM SuOrganizationLang org where org.LanguageID = :LanguageId ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageId", typeof(short), languageId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("OrganizationID", NHibernateUtil.Int16)
                .AddScalar("OrganizationName", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(SuOrganizationWithLang))).List<SuOrganizationWithLang>();
		}
		public IList<OrganizationLang> FindSuOrganizationLangByOrganizationId(short organizationId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append("SELECT l.LanguageId as LanguageId, l.LanguageName as LanguageName, orgLang.OrganizationName, orgLang.OrganizationId as OrganizationId, ");
			sqlBuilder.Append(" orgLang.Id as OrganizationLangId, orgLang.Comment as Comment, orgLang.Active as Active");
			sqlBuilder.Append(" FROM DbLanguage l LEFT JOIN SuOrganizationLang orgLang on orgLang.LanguageId = l.LanguageId and orgLang.OrganizationId = :organizationId");

			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("organizationId", typeof(short), organizationId);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("LanguageId", NHibernateUtil.Int16)
				.AddScalar("LanguageName", NHibernateUtil.String)
				.AddScalar("OrganizationId", NHibernateUtil.Int16)
				.AddScalar("OrganizationName", NHibernateUtil.String)
				.AddScalar("OrganizationLangId", NHibernateUtil.Int64)
				.AddScalar("Comment", NHibernateUtil.String)
				.AddScalar("Active", NHibernateUtil.Boolean);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(OrganizationLang))).List<OrganizationLang>();
		}
		public IList<SuOrganizationLang> FindByOrganizationAndLanguage(short organizationId, short languageId)
		{
			IList<SuOrganizationLang> list = GetCurrentSession()
				.CreateQuery("FROM SuOrganizationLang ol WHERE ol.Organization.Organizationid = :OrganizationId AND ol.Language.Languageid = :LanguageId ")
				.SetInt16("OrganizationId", organizationId)
				.SetInt16("LanguageId", languageId)
				.List<SuOrganizationLang>();

			return list;
		}
		#endregion
	}
}
