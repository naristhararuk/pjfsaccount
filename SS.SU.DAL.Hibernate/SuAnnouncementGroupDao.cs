using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
	public class SuAnnouncementGroupDao : NHibernateDaoBase<SuAnnouncementGroup, short>, ISuAnnouncementGroupDao
	{
		#region ISuAnnouncementGroupDao Members
		public ICriteria FindBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroup)
		{
			ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuAnnouncementGroup), "AnnouncementGroup");
			return criteria;
		}
		public IList<AnnouncementGroup> GetTranslatedList(short languageId)
		{
			StringBuilder strQuery = new StringBuilder();
			strQuery.AppendLine(" SELECT ag.AnnouncementGroupID as AnnouncementGroupId , agl.AnnouncementGroupName as AnnouncementGroupName ");
			strQuery.AppendLine(" FROM SuAnnouncementGroup ag ");
			strQuery.AppendLine(" INNER JOIN SuAnnouncementGroupLang agl ");
			strQuery.AppendLine(" ON agl.AnnouncementGroupID = ag.AnnouncementGroupID ");
			strQuery.AppendLine(" WHERE agl.LanguageID = :LanguageID ");
			strQuery.AppendLine(" ORDER BY agl.AnnouncementGroupName ");

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(strQuery.ToString());
			query.SetInt16("LanguageID", languageId);
			query.AddScalar("AnnouncementGroupId", NHibernateUtil.Int16);
			query.AddScalar("AnnouncementGroupName", NHibernateUtil.String);
			
			IList<AnnouncementGroup> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(AnnouncementGroup))).List<AnnouncementGroup>();
			
			return list;
		}
		#endregion
	}
}
