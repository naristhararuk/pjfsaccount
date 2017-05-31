using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbExpenseGroupLangDao : NHibernateDaoBase<DbExpenseGroupLang, long>, IDbExpenseGroupLangDao
    {
        public void DeleteExpenseGroup(long epID, short languageId)
        {
            GetCurrentSession()
                .Delete("from DbExpenseGroupLang ep where ep.ExpenseGroupID.ExpenseGroupID = :epID and ep.LanguageID.Languageid = :languageId"
                , new object[] { epID, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.Int16 });
        }
    }
}
