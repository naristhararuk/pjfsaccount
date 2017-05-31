using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO.ValueObject; 

namespace SS.SU.BLL.Implement
{
    public class SuRoleLangService : ServiceBase<SuRoleLang, long>, ISuRoleLangService
    {
        public override IDao<SuRoleLang, long> GetBaseDao()
        {
            return DaoProvider.SuRoleLangDao;
        }
        public IList<SuRoleLang> FindBySuRoleLangCriteria(SuRoleLang criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<SuRoleLang>(DaoProvider.SuRoleLangDao, "FindBySuRoleLangCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        }
        public int CountBySuRoleLangCriteria(SuRoleLang criteria)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuRoleLangDao, "FindBySuRoleLangCriteria", new object[] { criteria });
        }
        public IList<RoleLang> FindByRoleId(short roleId)
        {
            return DaoProvider.SuRoleLangDao.FindByRoleId(roleId);
        }
        public void UpdateRoleLang(IList<SuRoleLang> roleLangList)
        {
            if (roleLangList.Count > 0)
            {
                DaoProvider.SuRoleLangDao.DeleteAllRoleLang(roleLangList[0].Role.RoleID);
            }
            foreach (SuRoleLang roleLang in roleLangList)
            {
                DaoProvider.SuRoleLangDao.Save(roleLang);
            }

        }
    }
}
