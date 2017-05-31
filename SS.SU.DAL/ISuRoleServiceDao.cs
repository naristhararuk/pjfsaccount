using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using SS.SU.DTO;

namespace SS.SU.DAL
{
    public interface ISuRoleServiceDao : IDao<SuRoleService,long>
    {
        bool IsExist(SS.SU.DTO.SuRoleService roleService);
    }
}
