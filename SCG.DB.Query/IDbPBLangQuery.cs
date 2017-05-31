using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;

using SS.Standard.Data.Query.NHibernate;
using SS.DB.DTO.ValueObject;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbPbLangQuery : IQuery<DbpbLang,long>
    {
        IList<TranslatedListItem> GetPBLangByCriteria(DbpbLang pbLang,short RoleID);
        IList<VOPB> FindPBLangByPBID(long pbID);
        DbpbLang GetPBLangByPBIDAndLanguageID(long pbID, short languageID);
        IList<TranslatedListItem> GetPBLangByCriteria(short languageID, long UserID, IList<short> userRole);
    }
}
