using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.BLL
{
    public interface IFnPerdiemProfileCountryService : IService<FnPerdiemProfileCountry, long>
    {
        void AddFnPerdiemProfileCountry(FnPerdiemProfileCountry ppc);
    }
}
