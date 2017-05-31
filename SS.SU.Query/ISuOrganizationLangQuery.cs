using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuOrganizationLangQuery : IQuery<SuOrganizationLang, long>
    {
        IList<SuOrganizationWithLang> FindByLanguageId(short languageId);
        IList<OrganizationLang> FindSuOrganizationLangByOrganizationId(short organizationId);
        IList<SuOrganizationLang> FindByOrganizationAndLanguage(short organizationId, short languageId);
       // IList<SuOrganization_SuOrganizationLang> FindByLanguageId(int languageId);
    }
}
