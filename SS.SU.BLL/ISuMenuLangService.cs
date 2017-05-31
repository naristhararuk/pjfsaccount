using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuMenuLangService : IService<SuMenuLang,long>
    {
        IList<SuMenuLang> FindByMenuId(short menuId);
        void UpdateMenuLang(IList<SuMenuLang> menuLangList);
        void DeleteMenuLang(short menuId);
    }
}
