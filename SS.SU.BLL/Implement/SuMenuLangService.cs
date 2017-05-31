using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;

using SS.DB.DTO;
using SS.DB.DAL;

namespace SS.SU.BLL.Implement
{
    public partial class SuMenuLangService : ServiceBase<SuMenuLang,long>,ISuMenuLangService
    {
        public override IDao<SuMenuLang, long> GetBaseDao()
        {
            return DaoProvider.SuMenuLangDao;
        }

        public IList<SuMenuLang> FindByMenuId(short menuId)
        {
            return DaoProvider.SuMenuLangDao.FindByMenuId(menuId);
        }

        public void UpdateMenuLang(IList<SuMenuLang> menuLangList)
        {
            foreach (SuMenuLang mn in menuLangList)
            {
                DaoProvider.SuMenuLangDao.DeleteByMenuIdLanguageId(mn.Menu.Menuid, mn.Language.Languageid);
            }
            

            foreach (SuMenuLang mn in menuLangList)
            {

                SuMenuLang suMenuLang = new SuMenuLang();
                suMenuLang.Menu = DaoProvider.SuMenuDao.FindProxyByIdentity(mn.Menu.Menuid);
                suMenuLang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(mn.Language.Languageid);
                suMenuLang.MenuName = mn.MenuName;
                suMenuLang.Comment = mn.Comment;
                suMenuLang.CreBy = mn.CreBy;
                suMenuLang.CreDate = DateTime.Now.Date;
                suMenuLang.UpdBy = mn.UpdBy;
                suMenuLang.UpdDate = DateTime.Now.Date;
                suMenuLang.UpdPgm = mn.UpdPgm;
                suMenuLang.Active = mn.Active;

                DaoProvider.SuMenuLangDao.Save(suMenuLang);
            }
        }

        public void DeleteMenuLang(short menuId)
        {
            DaoProvider.SuMenuLangDao.DeleteByMenuId(menuId);

        }
    }
}
