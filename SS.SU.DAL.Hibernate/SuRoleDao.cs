using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using NHibernate.Transform;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.NHibernate.QueryCreator;



namespace SS.SU.DAL.Hibernate
{
    public partial class SuRoleDao : NHibernateDaoBase<SuRole, short>, ISuRoleDao
    {
        public SuRoleDao()
        {
        }

        #region ISuRoleDao Members
        public bool RolePremission(string username)
        {
            return true;
        }

        public bool IsDuplicateRoleCode(SuRole role)
        {
            IList<SuRole> list = GetCurrentSession().CreateQuery("from SuRole p where p.RoleCode=:RoleCode and p.RoleID<>:RoleID")
                  .SetString("RoleCode", role.RoleCode)
                  .SetInt16("RoleID", role.RoleID)
                  .List<SuRole>();
            if (list.Count > 0)
            {
                return true;
            }
            return false; 
        }
        #endregion

        #region IDao<SuUser,long> Members

        #endregion

    
    }
}
