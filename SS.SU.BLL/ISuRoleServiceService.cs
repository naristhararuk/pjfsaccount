using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;

using SS.Standard.Data.NHibernate.Service;
namespace SS.SU.BLL
{
    public interface ISuRoleServiceService : IService<SuRoleService,long>
    {
        //Need to validate by new method implementing.
        void AddRoleService(SuRoleService roleService);
        //Need to validate by new method implementing.
        void DeleteRoleService(SuRoleService roleService);
        
    }
}
