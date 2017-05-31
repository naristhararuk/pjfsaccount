using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate.Expression;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;

namespace SS.DB.DAL.Hibernate
{
    public partial class DbStatusLangDao : NHibernateDaoBase<DbStatusLang, long>, IDbStatusLangDao
    {

        #region IDbStatusLangDao Members

        public IList<DbStatusLang> FindByStatusId(short statusId)
        {
            IList<DbStatusLang> list = GetCurrentSession().CreateQuery("from DbStatusLang as sl where sl.Status.StatusID = :StatusId")
                .SetInt16("StatusId", statusId).List<DbStatusLang>();

            return list;
        }

        public bool IsDuplicateLanguage(short statusId, short languageId)
        {
            IList<DbStatusLang> list = GetCurrentSession().CreateQuery("from DbStatusLang as sl where sl.Status.StatusID = :StatusId and sl.Language.LanguageId = : LanguageId")
                 .SetInt16("StatusId", statusId)
                 .SetInt16("LanguageId", languageId)
                 .List<DbStatusLang>();

            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void DeleteByStatusIdLanguageId(short statusId)
        {
            GetCurrentSession()
                .Delete("from DbStatusLang sl where sl.Status.StatusID = :statusId "
                , new object[] { statusId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16});
        }

        #endregion
    }
}
