using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuDivisionLangService : IService<SuDivisionLang, long>
    {
        IList<SuDivisionLang> FindByDivisionId(short divisionId);
        void UpdateDivisionLang(IList<SuDivisionLang> divisionLangList);
    }
}
