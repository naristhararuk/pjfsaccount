using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuGlobalTranslateLangDao : IDao<SuGlobalTranslateLang, long>
    {
        IList<SuGlobalTranslateLang> FindByTranslateId(long translateId);
        bool IsDuplicateLanguage(long translateId, short languageId);
        //IList<GlobalTranslateLang> FindTranslateLangByTranslateId(long translateId);
        void DeleteByTranslatIdLanguageId(long translateId, short languageId);
    }
}
