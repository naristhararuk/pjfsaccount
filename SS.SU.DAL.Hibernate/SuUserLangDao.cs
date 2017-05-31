using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.DAL.Hibernate
{
	public class SuUserLangDao : NHibernateDaoBase<SuUserLang, long>, ISuUserLangDao
	{
		#region ISuUserLangDao Members
		public IList<SuUserLang> FindByUserIdAndLanguageId(long userID, short languageID)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuUserLang ul WHERE ul.User.Userid = :userID AND ul.Language.Languageid = :languageID ");

			IList<SuUserLang> list = GetCurrentSession()
				.CreateQuery(sqlBuilder.ToString())
				.SetInt64("userID", userID)
				.SetInt16("languageID", languageID)
				.List<SuUserLang>();

			return list;
			
		}
		#endregion
	}
}
