using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;

namespace SS.SU.DAL
{
    public interface ISuProgramRoleDao : IDao<SuProgramRole, short>
    {

        void DeleteByProgramRoleId(short roleId, short programId);
        bool IsExist(SuProgramRole programRole);
    }
}
