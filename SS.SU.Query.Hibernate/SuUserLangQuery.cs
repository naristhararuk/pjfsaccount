using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;

namespace SS.SU.Query.Hibernate
{
	public class SuUserLangQuery : NHibernateQueryBase<SuUserLang, long>, ISuUserLangQuery
	{

		#region ISuUserLangQuery Members
		public IList<UserLang> FindUserLangByUserId(long userId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append("SELECT l.LanguageId as LanguageId, l.LanguageName as LanguageName, ul.UserID as UserId, ");
			sqlBuilder.Append(" ul.Id as UserLangId, ul.FirstName as FirstName, ul.LastName as LastName, ul.Comment as Comment, ul.Active as Active");
			sqlBuilder.Append(" FROM DbLanguage l LEFT JOIN SuUserLang ul on ul.LanguageId = l.LanguageId and ul.UserId = :UserId");

			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("UserId", typeof(long), userId);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("LanguageId", NHibernateUtil.Int16)
				.AddScalar("LanguageName", NHibernateUtil.String)
				.AddScalar("UserId", NHibernateUtil.Int64)
				.AddScalar("UserLangId", NHibernateUtil.Int64)
				.AddScalar("FirstName", NHibernateUtil.String)
				.AddScalar("LastName", NHibernateUtil.String)
				.AddScalar("Comment", NHibernateUtil.String)
				.AddScalar("Active", NHibernateUtil.Boolean);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(UserLang))).List<UserLang>();
		}
		public IList<SuUserLang> FindByUserAndLanguage(long userId, short languageId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuUserLang ul WHERE ul.User.Userid = :userID AND ul.Language.Languageid = :languageID ");
			
			IList<SuUserLang> list = GetCurrentSession()
				.CreateQuery(sqlBuilder.ToString())
				.SetInt64("userID", userId)
				.SetInt16("languageID", languageId)
				.List<SuUserLang>();
				
			return list;
		}
		#endregion
	}
}
