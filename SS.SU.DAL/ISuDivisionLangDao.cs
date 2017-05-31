using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuDivisionLangDao : IDao<SuDivisionLang, long>
    {
        IList<SuDivisionLang> FindByDivisionId(short divisionId);
        IList<SuDivisionLang> FindByDivisionName(short organizationId, short languageId, string divisionName);
        //bool IsDuplicateLanguage(long Id, short languageId);
        //IList<SuDivisionLang> FindDivisionLangByDivisionId(short divisionId);
        void DeleteByDivisionIdLanguageId(short divisionId, short languageId);
    }
}
