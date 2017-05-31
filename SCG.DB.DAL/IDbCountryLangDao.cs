using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using NHibernate;

namespace SCG.DB.DAL
{
    public interface IDbCountryLangDao : IDao<DbCountryLang, short>
    {
        IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short countryID);
        void DeleteAllCountryLang(short CountryID);
        
    }
}
