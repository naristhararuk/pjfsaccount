using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuProgramRoleService : IService<SuProgramRole, short>
    {
        void UpdateProgramRole(IList<SuProgramRole> programRoleList);
        short AddProgramRole(SuProgramRole programRole);
        void DeleteProgramRole(SuProgramRole proGramRole);
        
    }
}
