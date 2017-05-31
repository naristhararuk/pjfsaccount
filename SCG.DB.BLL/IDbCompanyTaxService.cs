using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Service;

namespace SCG.DB.BLL
{
    public interface IDbCompanyTaxService : IService<DbCompanyTax, long>
    {
        void AddCompanyTax(DbCompanyTax company);
        void UpdateCompanyTax(DbCompanyTax company);
        void DeleteCompanyTax(DbCompanyTax company);
    }
}
