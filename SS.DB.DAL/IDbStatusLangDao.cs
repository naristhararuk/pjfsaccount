using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;

namespace SS.DB.DAL
{
    public interface IDbStatusLangDao : IDao<DbStatusLang, long>
    {
        IList<DbStatusLang> FindByStatusId(short statusId);
        bool IsDuplicateLanguage(short statusId, short languageId);
        void DeleteByStatusIdLanguageId(short statusId);
    }
}
