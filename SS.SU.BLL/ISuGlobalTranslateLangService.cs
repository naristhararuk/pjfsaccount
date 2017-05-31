using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuGlobalTranslateLangService : IService<SuGlobalTranslateLang, long>
    {
        IList<SuGlobalTranslateLang> FindByTranslateId(long translateId);
        void UpdateGlobalTranslateLang(IList<SuGlobalTranslateLang> translateLangList);
    }
}
