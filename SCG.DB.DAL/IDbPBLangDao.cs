using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbPBLangDao : IDao<DbpbLang, long>
    {
        //short AddCurrencyLang(DbCurrencyLang currency);
       void DeleteByPBIdLanguageId(long pbId, short languageId);
    }
}
