using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbCompanyService : IService<DbCompany, long>
    {
        void AddCompany(DbCompany company);
        void UpdateCompany(DbCompany company);
        long AddCompanyAndGetId(DbCompany company);
        DbCompany getCompanyByCode(string companyCode);
        //short FindCountryId(string countryCode);
        DbCompany  FindByCompanycode(string companyCode);
        void DeleteCompany(DbCompany company);
    }
}
