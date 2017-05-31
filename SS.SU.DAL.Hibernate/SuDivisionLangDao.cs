using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate.Expression;
using SS.SU.DTO.ValueObject;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuDivisionLangDao : NHibernateDaoBase<SuDivisionLang, long>, ISuDivisionLangDao
	{
		#region ISuDivisionLangDao Members
		public IList<SuDivisionLang> FindByDivisionId(short divisionId)
        {
            IList<SuDivisionLang> list = GetCurrentSession().CreateQuery("from SuDivisionLang as dl where dl.Division.DivisionId = :DivisionId")
                .SetInt16("DivisionId", divisionId).List<SuDivisionLang>();

            return list;
        }
        public void DeleteByDivisionIdLanguageId(short divisionId, short languageId)
        {
            GetCurrentSession()
                .Delete("from SuDivisionLang dl where dl.Division.Divisionid = :DivisionId and dl.Language.Languageid = :LanguageId"
                , new object[] { divisionId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int16 });
        }
        
		public IList<SuDivisionLang> FindByDivisionName(short organizationId, short languageId, string divisionName)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuDivisionLang divLang ");
			sqlBuilder.AppendLine(" WHERE divLang.Language.Languageid = :languageID ");
			sqlBuilder.AppendLine(" AND divLang.Division.Organization.Organizationid = :organizationID ");
			sqlBuilder.AppendLine(" AND divLang.DivisionName = :divisionName ");

			IQuery query = GetCurrentSession().CreateQuery(sqlBuilder.ToString());
			query.SetInt16("languageID", languageId);
			query.SetInt16("organizationID", organizationId);
			query.SetString("divisionName", divisionName);
						
			return query.List<SuDivisionLang>();
		}
		#endregion
	}
}
