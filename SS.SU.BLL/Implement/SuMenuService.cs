using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;


namespace SS.SU.BLL.Implement
{
    public partial class SuMenuService : ServiceBase<SuMenu, short>, ISuMenuService
    {
        public override IDao<SuMenu, short> GetBaseDao()
        {
            return DaoProvider.SuMenuDao;
        }
        public long AddMenu(SuMenu menu)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            //Check MenuCode is unique.
            if (DaoProvider.SuMenuDao.IsDuplicateMenuCode(menu))
            {
                errors.AddError("Menu.Error", new Spring.Validation.ErrorMessage("UniqueMenuCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            SuMenu suMenu = new SuMenu();
            suMenu.Menuid = menu.Menuid;
            suMenu.MenuCode = menu.MenuCode;
            suMenu.Program = menu.Program;
            suMenu.MenuMainid = menu.MenuMainid;
            suMenu.MenuLevel = menu.MenuLevel;
            suMenu.MenuSeq = menu.MenuSeq;
            suMenu.Comment = menu.Comment;
            suMenu.CreBy = menu.CreBy;
            suMenu.CreDate = DateTime.Now.Date;
            suMenu.UpdBy = menu.UpdBy;
            suMenu.UpdDate = DateTime.Now.Date;
            suMenu.UpdPgm = menu.UpdPgm;
            suMenu.Active = menu.Active;

            return DaoProvider.SuMenuDao.Save(menu);
        }
        public void UpdateMenu(SuMenu menu)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //SuMenu menuProxy = DaoProvider.SuMenuDao.FindProxyByIdentity(menu.Menuid);
            //Check MenuCode is unique.
            if (DaoProvider.SuMenuDao.IsDuplicateMenuCode(menu))
            {
                errors.AddError("Menu.Error", new Spring.Validation.ErrorMessage("UniqueMenuCode"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.SuMenuDao.SaveOrUpdate(menu);
        }
        public IList<SuMenu> FindBySuMenuCriteria(SuMenu criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<SuMenu>(DaoProvider.SuMenuDao, "FindBySuMenuCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        }
        public int CountBySuMenuCriteria(SuMenu criteria)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuMenuDao, "FindBySuMenuCriteria", new object[] { criteria });
        }

        #region TestMasterGrid
        public void DeleteMasterRecord(string id)
        {
            SuMenu menu = DaoProvider.SuMenuDao.FindByIdentity(Convert.ToInt16(id));
            DaoProvider.SuMenuDao.Delete(menu);
        }
        public void Add(ISimpleMaster obj)
        {
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SuMenu menu = new SuMenu();
            menu.MenuCode = obj.MenuCode;
            //menu.MenuLevel = obj.MenuLevel;
            menu.Comment = obj.Comment;
            menu.Active = obj.Active;
            menu.CreBy = obj.CreBy;
            menu.CreDate = obj.CreDate;
            menu.UpdBy = obj.UpdBy;
            menu.UpdDate = obj.UpdDate;
            menu.UpdPgm = obj.UpdPgm;
            short divId = DaoProvider.SuMenuDao.Save(menu);
        }
        public void Updated(ISimpleMaster obj)
        {
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            SuMenu menu = DaoProvider.SuMenuDao.FindByIdentity(obj.Menuid);
            menu.MenuCode = obj.MenuCode;
            //menu.MenuLevel = obj.MenuLevel;
            menu.Comment = obj.Comment;
            menu.Active = obj.Active;
            menu.UpdBy = obj.UpdBy;
            menu.UpdDate = obj.UpdDate;
            menu.UpdPgm = obj.UpdPgm;
            DaoProvider.SuMenuDao.SaveOrUpdate(menu);
        }
        #endregion
    }
}
