using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;
//using SS.Standard.Data.Query;
//using SS.Standard.Data.Query.NHibernate;
using SS.Standard.Data.NHibernate.Query;

namespace SS.SU.Query
{
    public interface ISuProgramLangQuery : IQuery<SuProgramLang, long>
    {
        new IList<SuProgramLang> FindAll();
        new SuProgramLang FindByIdentity(long id);
        

    }
}
