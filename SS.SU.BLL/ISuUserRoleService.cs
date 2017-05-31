using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuUserRoleService : IService<SuUserRole, long>
    {
        new IList<SuUserRole> FindAll();
        IList<SuUserRole> FindByUserIdentity(long userId);
        new void Delete(SuUserRole domain);
        new long Save(SuUserRole domain);
        new void SaveOrUpdate(SuUserRole domain);
        new void Update(SuUserRole domain);

		void UpdateUserRole(IList<SuUserRole> userRoleList);
        long AddFavoriteGroup(SuUserRole ur);
        void DeleteGroup(SuUserRole group);
    }
}
