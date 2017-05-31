using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.DB.DTO;
using SS.DB.DAL;
using SS.SU.BLL;


namespace SS.DB.BLL.Implement
{
    public partial class DbStatusLangService : ServiceBase<DbStatusLang, long>, IDbStatusLangService
    {

        #region IDbStatusLangService Members
        public override IDao<DbStatusLang, long> GetBaseDao()
        {
            return SsDbDaoProvider.DbStatusLangDao;
        }
        public IList<DbStatusLang> FindByStatusId(short statusId)
        {
            return SsDbDaoProvider.DbStatusLangDao.FindByStatusId(statusId);
        }

        public void UpdateStatusLang(IList<DbStatusLang> statusLangList)
        {
            SsDbDaoProvider.DbStatusLangDao.DeleteByStatusIdLanguageId(statusLangList[0].Status.StatusID);

            foreach (DbStatusLang sl in statusLangList)
            {
                DbStatusLang statusLang = new DbStatusLang();
                statusLang.Status = SsDbDaoProvider.DbStatusDao.FindProxyByIdentity(sl.Status.StatusID);
                statusLang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(sl.Language.Languageid);
                statusLang.StatusDesc = sl.StatusDesc;
                statusLang.Comment = sl.Comment;
                statusLang.Active = sl.Active;
                statusLang.CreBy = sl.CreBy;
                statusLang.CreDate = DateTime.Now.Date;
                statusLang.UpdBy = sl.UpdBy;
                statusLang.UpdDate = DateTime.Now.Date;
                statusLang.UpdPgm = sl.UpdPgm;

                SsDbDaoProvider.DbStatusLangDao.Save(statusLang);
            }
        }

        #endregion
    }
}
