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

namespace SS.SU.Query.Hibernate
{
	public class SuAnnouncementGroupLangQuery : NHibernateQueryBase<SuAnnouncementGroupLang, short>, ISuAnnouncementGroupLangQuery
	{
		#region ISuAnnouncementGroupLangQuery Members
		public IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId)
		{
			IList<SuAnnouncementGroupLang> list = GetCurrentSession()
				.CreateQuery("FROM SuAnnouncementGroupLang as agl where agl.AnnouncementGroup.AnnouncementGroupid = :announcementGroupId")
				.SetInt16("announcementGroupId", announcementGroupId).List<SuAnnouncementGroupLang>();
			return list;
		}
		public IList<AnnouncementGroupLang> FindAnnouncementGroupLangByAnnouncementGroupId(short announcementGroupId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append("SELECT l.LanguageId as LanguageId, l.LanguageName as LanguageName, agl.AnnouncementGroupId as AnnouncementGroupId,");
			sqlBuilder.Append(" agl.Id as AnnouncementGroupLangId, agl.AnnouncementGroupName as AnnouncementGroupName, agl.Comment as Comment, agl.Active as Active");
			sqlBuilder.Append(" FROM DbLanguage l LEFT JOIN SuAnnouncementGroupLang agl on agl.LanguageId = l.LanguageId and agl.AnnouncementGroupId = :announcementGroupId");
			
			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("announcementGroupId", typeof(short), announcementGroupId);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("LanguageId", NHibernateUtil.Int16)
				.AddScalar("LanguageName", NHibernateUtil.String)
				.AddScalar("AnnouncementGroupId", NHibernateUtil.Int16)
				.AddScalar("AnnouncementGroupLangId", NHibernateUtil.Int16)
				.AddScalar("AnnouncementGroupName", NHibernateUtil.String)
				.AddScalar("Comment", NHibernateUtil.String)
				.AddScalar("Active", NHibernateUtil.Boolean);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(AnnouncementGroupLang))).List<AnnouncementGroupLang>();
		}
        public IList<SuAnnouncementGroupLang> FindByAnnouncementLanguageId(short languageId)
        {
            IList<SuAnnouncementGroupLang> list = GetCurrentSession()
                .CreateQuery("FROM SuAnnouncementGroupLang as agl where agl.Language.Languageid = :languageId")
                .SetInt16("languageId", languageId).List<SuAnnouncementGroupLang>();

            foreach (SuAnnouncementGroupLang item in list)
            {
                Console.WriteLine(item.AnnouncementGroup);
            }
            return list;
        }
		public IList<SuAnnouncementGroupLang> FindByAnnouncementGroupIdAndLanguageId(short languageId, short announcementGroupId)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuAnnouncementGroupLang as agl ");
			sqlBuilder.AppendLine(" where agl.Language.Languageid = :languageId ");
			sqlBuilder.AppendLine(" and agl.AnnouncementGroup.AnnouncementGroupid = :announcementGroupId ");

			IList<SuAnnouncementGroupLang> list = GetCurrentSession()
				.CreateQuery(sqlBuilder.ToString())
				.SetInt16("languageId", languageId)
				.SetInt16("announcementGroupId", announcementGroupId)
				.List<SuAnnouncementGroupLang>();

			foreach (SuAnnouncementGroupLang item in list)
			{
				Console.WriteLine(item.AnnouncementGroup);
			}
			return list;
		}
		#endregion
	}
}
