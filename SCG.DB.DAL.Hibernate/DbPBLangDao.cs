using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbPBLangDao : NHibernateDaoBase<DbpbLang, long>, IDbPBLangDao
    {
        public DbPBLangDao()
        {
        }
        public void DeleteByPBIdLanguageId(long pbId, short languageId)
        {
            GetCurrentSession()
                .Delete("from DbpbLang pb where pb.PBID.Pbid = :pbId and pb.LanguageID.Languageid = :languageId"
                , new object[] { pbId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.Int16 });
        }
    }
}
