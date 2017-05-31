using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.DB.Query;
using SS.Standard.Utilities;

namespace SS.SU.BLL.Implement
{
    public partial class SuUserRoleService : ServiceBase<SuUserRole, long>, ISuUserRoleService
    {
		#region Override Method
		public override IDao<SuUserRole, long> GetBaseDao()
		{
			return DaoProvider.SuUserRoleDao;
		}
		#endregion

		#region ISuUserRoleService Members
		public void UpdateUserRole(IList<SuUserRole> userRoleList)
		{
			#region Validate Input
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			foreach (SuUserRole userRole in userRoleList)
			{
				if (userRole.Role == null)
				{
					errors.AddError("UserRole.Error", new Spring.Validation.ErrorMessage("RoleRequired"));
				}
				if (userRole.User == null)
				{
					errors.AddError("UserRole.Error", new Spring.Validation.ErrorMessage("UserRequired"));
				}
			}
			#endregion

			// Delete existing Role in UserRole table.
			foreach (SuUserRole userRole in userRoleList)
			{
                DaoProvider.SuUserRoleDao.DeleteByRoleIdAndUserId(userRole.Role.RoleID, userRole.User.Userid);
			}
			
			// Save new Role to UserRole table.
			foreach (SuUserRole userRole in userRoleList)
			{
				this.Save(userRole);
			}
		}
		public IList<SuUserRole> FindByUserIdentity(long userId)
		{
			throw new NotImplementedException();
		}
        public long AddFavoriteGroup(SuUserRole ur)
        {
            long id = 0;
            id = DaoProvider.SuUserRoleDao.Save(ur);
            return id;
        }
        public void DeleteGroup(SuUserRole group)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ParameterServices.DefaultUserRoleID == (int)group.Role.RoleID)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Unable to remove user from default role."));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            DaoProvider.SuUserRoleDao.Delete(group);
        }
		#endregion
	}


}
