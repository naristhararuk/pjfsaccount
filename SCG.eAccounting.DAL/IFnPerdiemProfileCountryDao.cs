using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;

namespace SCG.eAccounting.DAL
{
    public interface IFnPerdiemProfileCountryDao : IDao<FnPerdiemProfileCountry, long>
    {
        bool IsDuplicateCode(FnPerdiemProfileCountry ppc);
    }
}
