using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbLocationLangService : IService<DbLocationLang, long>
    {
        void UpdateLocationLang(IList<DbLocationLang> locationLangList);
        void AddLocationLang(DbLocationLang locationLang);
        void UpdateLocationLang(DbLocationLang locationLang);
    }
}
