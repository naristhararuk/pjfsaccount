using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuProgramLangService : IService<SuProgramLang, long>
    {
        new IList<SuProgramLang> FindAll();
        new void Delete(SuProgramLang domain);
        new SuProgramLang FindByIdentity(long id);
        new long Save(SuProgramLang domain);
        new void SaveOrUpdate(SuProgramLang domain);
        new void Update(SuProgramLang domain);
        IList<ProgramLang> FindByProgramId(short programId);
        void UpdateProgramLang(IList<SuProgramLang> programLangList);
        IList<SuProgramLang> FindBySuProgramLangCriteria(SuProgramLang criteria, int firstResult, int maxResults, string sortExpression);
        IList<ProgramLang> FindBySuProgramLangQuery(SuProgramLang criteria, short roleId ,short languageId, int firstResult, int maxResults, string sortExpression);
        int CountBySuProgramLangCriteria(SuProgramLang criteria, short roleId, short languageId);
    }
}
