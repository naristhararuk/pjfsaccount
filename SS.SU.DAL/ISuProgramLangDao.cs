using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO.ValueObject;
using SS.SU.DTO;
using NHibernate;

namespace SS.SU.DAL
{
    public interface ISuProgramLangDao : IDao<SuProgramLang, long>  
    {
        IList<ProgramLang> FindByProgramId(short programId);
        ICriteria FindBySuProgramLangCriteria(SuProgramLang programlang);
        ISQLQuery FindBySuProgramLangQuery(SuProgramLang programLang, short roleId, short languageId, bool isCount);
        void DeleteAllProgramLang(short programId);
    }
}
