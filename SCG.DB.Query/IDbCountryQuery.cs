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
    public interface IDbCountryQuery : IQuery<DbCountry, short>
    {
        ISQLQuery FindByCountryCriteria(DbCountry country, bool isCount, short languageId, string sortExpression);
        IList<CountryLang> GetCountryList(DbCountry country, short languageId, int firstResult, int maxResult, string sortExpression);
        int CountByCountryCriteria(DbCountry country);
        IList<CountryLang> FindCountry(short languageId);

    }
}
