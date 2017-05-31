using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbPBLangService : IService<DbpbLang, long>
    {
        long AddPBLang(DbpbLang pb);
        void UpdatePBLang(IList<DbpbLang> pb);
        void AddPBLang(IList<DbpbLang> pb);
    }
}
