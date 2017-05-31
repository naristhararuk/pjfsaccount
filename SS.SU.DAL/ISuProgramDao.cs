using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
namespace SS.SU.DAL
{
    public interface ISuProgramDao : IDao<SuProgram, short>  
    {
        bool IsDuplicateProgramCode(SuProgram program);
        //ICriteria FindBySuProgramCriteria(SuProgram program);
    }
}
