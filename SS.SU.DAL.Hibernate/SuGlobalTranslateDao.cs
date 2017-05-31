using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuGlobalTranslateDao : NHibernateDaoBase<SuGlobalTranslate, long>, ISuGlobalTranslateDao
    {
        public bool IsDuplicateProgramCodeSymbol(SuGlobalTranslate translate)
        {
            IList<SuGlobalTranslate> list = GetCurrentSession().CreateQuery("from SuGlobalTranslate gt where gt.TranslateId <> :TranslateId and gt.ProgramCode = :ProgramCode and gt.TranslateSymbol = :TranslateSymbol and gt.TranslateControl = :TranslateControl")
                  .SetInt64("TranslateId", translate.TranslateId)
                  .SetString("ProgramCode", translate.ProgramCode)
                  .SetString("TranslateSymbol", translate.TranslateSymbol)
                  .SetString("TranslateControl", translate.TranslateControl)
                  .List<SuGlobalTranslate>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public ICriteria FindBySuGolbalTranslateCriteria(SuGlobalTranslate translate)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuGlobalTranslate), "t");

            /*
             Insert Criteria
             if (!string.IsNullOrEmpty(criteria))
            {
                criteria.Add(Expression.Like("..."));
            }
             */

            return criteria;
		}

		#region DeleteByProgramCodeAndControl
		public void DeleteByProgramCodeAndControl(string programCode, string translateControl)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.AppendLine(" FROM SuGlobalTranslate  WHERE ProgramCode = :programCode ");
			sqlBuilder.AppendLine(" AND TranslateControl = :translateControl ");
			//sqlBuilder.AppendLine(" AND TranslateSymbol = :translateSymbol ");
			
			GetCurrentSession()
				.Delete(sqlBuilder.ToString()
				, new object[] { programCode, translateControl }
				, new NHibernate.Type.IType[] { NHibernateUtil.String, NHibernateUtil.String });
		}
		#endregion
	}
}
