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
    public class SuRoleLangQuery : NHibernateQueryBase<SuRoleLang, short>, ISuRoleLangQuery
    {
		#region ISuRoleLangQuery Members
		public IList<SuRoleLangSearchResult> GetTranslatedList(SuRoleLangSearchResult criteria, short languageID, string roleName, long userID, int firstResult, int maxResult, string sortExpression)
		{
			return NHibernateQueryHelper.FindPagingByCriteria<SuRoleLangSearchResult>(QueryProvider.SuRoleLangQuery, 
				"FindSuRoleLangSearchResult", 
				new object[] { criteria, languageID, roleName, userID }, firstResult, maxResult, sortExpression);
		}
		
        public ISQLQuery FindSuRoleLangSearchResult(SuRoleLangSearchResult roleLangSearchResult, short languageID, string roleName, long userID)
		{
			StringBuilder strQuery = new StringBuilder();
			strQuery.AppendLine(" SELECT r.RoleID as RoleId, rl.RoleName as RoleName ");
			strQuery.AppendLine(" , lang.LanguageID as LanguageId, lang.LanguageName as LanguageName ");
			strQuery.AppendLine(" , r.Active as Active ");
			strQuery.AppendLine(" FROM SuRole r ");
			strQuery.AppendLine(" INNER JOIN SuRoleLang rl ");
			strQuery.AppendLine(" ON rl.RoleID = r.RoleID AND rl.LanguageID = :languageID ");
			strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
			strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
			strQuery.AppendLine(" WHERE rl.RoleName LIKE :roleName ");
			strQuery.AppendLine(" AND r.Active = 'true' ");
			strQuery.AppendLine(" AND r.RoleID NOT IN ");
			strQuery.AppendLine(" (SELECT ur.RoleID FROM SuUserRole ur WHERE ur.UserID = :userID) ");
			
			ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
			query.SetInt16("languageID", languageID);
			query.SetString("roleName", "%" + roleName + "%");
			query.SetInt64("userID", userID);
			query.AddScalar("RoleId", NHibernateUtil.Int16);
			query.AddScalar("RoleName", NHibernateUtil.String);
			query.AddScalar("LanguageId", NHibernateUtil.Int16);
			query.AddScalar("LanguageName", NHibernateUtil.String);
			query.AddScalar("Active", NHibernateUtil.Boolean);
			query.SetResultTransformer(Transformers.AliasToBean(typeof(SuRoleLangSearchResult)));

			return query;
		}
		
        public int FindCountSuRoleLangSearchResult(SuRoleLangSearchResult roleLangSearchResult, short languageID, string roleName, long userID)
		{
			StringBuilder strQuery = new StringBuilder();
			strQuery.AppendLine(" SELECT Count(*) as Count ");
			strQuery.AppendLine(" FROM SuRole r ");
			strQuery.AppendLine(" INNER JOIN SuRoleLang rl ");
			strQuery.AppendLine(" ON rl.RoleID = r.RoleID AND rl.LanguageID = :languageID ");
			strQuery.AppendLine(" INNER JOIN DbLanguage lang ");
			strQuery.AppendLine(" ON lang.LanguageID = :languageID ");
			strQuery.AppendLine(" WHERE rl.RoleName LIKE :roleName ");
			strQuery.AppendLine(" AND r.Active = 'true' ");
			strQuery.AppendLine(" AND r.RoleID NOT IN ");
			strQuery.AppendLine(" (SELECT ur.RoleID FROM SuUserRole ur WHERE ur.UserID = :userID) ");

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
			query.SetInt16("languageID", languageID);
			query.SetString("roleName", "%" + roleName + "%");
			query.SetInt64("userID", userID);
			query.AddScalar("Count", NHibernateUtil.Int32);
			
			return Convert.ToInt32(query.UniqueResult());
		}
		
        
        #endregion
	}
}
