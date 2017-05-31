using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.Query.Implement;
using SS.Standard.Data.Query;
using SS.Standard.Data.NHibernate;
using SS.SU.DTO;

namespace SS.SU.Query.Implement
{

    public class SuOrganizationQuery : QueryBase<SuOrganization, int>, ISuOrganizationQuery
    {
        public override IQuery<SuOrganization, int> GetBaseQuery()
        {
            return QueryProvider.SuOrganizationQuery;

        }

        #region ISuOrganizationQuery Members

        public SuOrganization FindByLanguageId(int languageId)
        {
            GetCurrentSession().CreateQuery(" FROM Company WHERE CountryId = :CountryId ORDER BY Name ").SetInt32("CountryId", countryId).List<Company>();
        }

        #endregion
    }
}
