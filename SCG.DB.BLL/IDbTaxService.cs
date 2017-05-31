using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbTaxService : IService<DbTax, long>
    {
        void AddTax(DbTax tax);
        void UpdateTax(DbTax tax);
        long FindTaxId(string taxCode);
    }
}
