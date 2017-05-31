using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using NHibernate;

namespace SS.SU.DAL
{
    public interface ISuGlobalTranslateDao : IDao<SuGlobalTranslate, long>
    {
        bool IsDuplicateProgramCodeSymbol(SuGlobalTranslate translate);
        ICriteria FindBySuGolbalTranslateCriteria(SuGlobalTranslate translate);
        
        void DeleteByProgramCodeAndControl(string programCode, string translateControl);
    }
}
