using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using NHibernate.Expression;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuProgramDao : NHibernateDaoBase<SuProgram, short>, ISuProgramDao
    {
        public SuProgramDao()
        {
        }
        public bool IsDuplicateProgramCode(SuProgram program)
        {
            IList<SuProgram> list = GetCurrentSession().CreateQuery("from SuProgram p where p.Programid <> :ProgramId and p.ProgramCode = :ProgramCode")
                  .SetInt64("ProgramId", program.Programid)
                  .SetString("ProgramCode",program.ProgramCode)
                  .List<SuProgram>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
