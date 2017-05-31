using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuDivisionLangQuery : IQuery<SuDivisionLang, long>
    {
        IList<DivisionLang> FindDivisionLangByDivisionId(short divisionId);
        IList<DivisionLang> FindAutoComplete(string divisionName , short languageId , short organizationId);
        IList<SuDivisionLang> FindByDivisionIdAndLanguageId(short divisionId, short languageId);
    }
}
