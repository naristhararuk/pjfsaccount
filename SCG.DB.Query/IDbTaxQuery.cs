using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbTaxQuery : IQuery<DbTax, long>
    {
        ISQLQuery FindByTaxCriteria(DbTax tax, bool isCount, string sortExpression);
        IList<DbTax> GetTaxList(DbTax tax, int firstResult, int maxResult, string sortExpression);

        int CountByTaxCriteria(DbTax tax);
        DbTax FindbyTaxCode(string taxCode);
        IList<DbTax> GetTaxCodeActive();
        IList<DbTax> GetTaxCodeActiveByCompany(long companyID);
        DbTax GetCompanyTaxRateByCompany(long taxID, long companyID);
    }
}
