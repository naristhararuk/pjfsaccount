using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuRoleService : IService<SuRole, short>
    {
        new IList<SuRole> FindAll();
        void DeleteRole(SuRole role);
        new SuRole FindByIdentity(short id);
        new short Save(SuRole domain);
        new void SaveOrUpdate(SuRole domain);
        new void Update(SuRole domain);
        void AddRole(SuRole role);
        void UpdateRole(SuRole role);
    }
}
