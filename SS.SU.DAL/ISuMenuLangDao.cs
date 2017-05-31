using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuMenuLangDao : IDao<SuMenuLang,long>
    {
        IList<SuMenuLang> FindByMenuId(short menuId);
        bool IsDuplicateMenu(short menuId, short languageId);
        //IList<GlobalTranslateLang> FindTranslateLangByTranslateId(long translateId);
        void DeleteByMenuIdLanguageId(short menuId, short languageId);
        void DeleteByMenuId(short menuId);
    }
}
