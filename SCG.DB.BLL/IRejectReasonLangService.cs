using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IRejectReasonLangService : IService<DbRejectReasonLang, long>
    {
        void UpdateRejectReasonLang(IList<DbRejectReasonLang> reasonLangList, int reasonID);
        
    }
}
