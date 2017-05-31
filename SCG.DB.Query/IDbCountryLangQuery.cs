using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.DB.Query
{
    public interface IDbCountryLangQuery : IQuery<DbCountryLang, long>
    {
        ISQLQuery FindByDbCountryLangQuery(DbCountryLang countryLang, string countryCode, short languageId, bool isCount);
        IList<CountryLang> FindByDbCountryLang(DbCountryLang criteria, string countryCode, short languageId, int firstResult, int maxResults, string sortExpression);
        IList<CountryLang> FindAutoComplete(string countryName, short countryId, short languageId);
        int CountByDbCountryLangCriteria(DbCountryLang criteria, string countryId, short languageId);
        CountryLang FindByDbCountryLangKey(short countryId, short languageId);

        IList<CountryLang> GetAllCountryLangByLang(short languageId, long requesterId);
		IList<CountryLang> FindCountryLangByCountryID(short cID);
    }
}
