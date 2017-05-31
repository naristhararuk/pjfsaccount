using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbCountryLangService : IService<DbCountryLang, short>
    {
        new DbCountryLang FindByIdentity(short countryId);
        IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short countryId);
        void UpdateCountryLang(IList<DbCountryLang> countryLangList);

    }
}
