using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;


namespace SCG.DB.Query
{
    public interface IDbCompanyTaxQuery : IQuery<DbCompanyTax, long>
    {
        ISQLQuery FindByCompanyTaxCriteria(DbCompanyTax tax, bool isCount, string sortExpression);
        IList<CompanyTaxRate> GetCompanyTaxList(DbCompanyTax tax, int firstResult, int maxResult, string sortExpression);
        int FindCount(DbCompanyTax taxID);
    }
}
