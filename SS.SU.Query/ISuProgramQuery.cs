using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Service;
using SS.Standard.Data.NHibernate.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuProgramQuery : IQuery<SuProgram, short>
    {
        new IList<SuProgram> FindAll();
        new SuProgram FindByIdentity(short id);

		IList<SuProgramSearchResult> GetTranslatedList(SuProgramSearchResult criteria, short languageID, int firstResult, int maxResult, string sortExpression);
		ISQLQuery FindSuProgramSearchResult(SuProgramSearchResult programSearchResult, short languageID);
		int FindCountSuProgramSearchResult(SuProgramSearchResult programSearchResult, short languageID);
        ISQLQuery FindByProgramCriteria(SuProgram program,string sortExpression, bool isCount);
        IList<SuProgram> GetProgramList(SuProgram program,  int firstResult, int maxResult, string sortExpression);
        int CountBySuProgramCriteria(SuProgram programCriteria);
        IList<SuProgramSearchResult> FindSuProgramByLanguageId(short languageID);
       
        short FindProgramIDByProgramCode(string programCode);
    }
}
