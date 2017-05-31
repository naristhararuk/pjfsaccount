using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.DB.DAL.Hibernate;
using SS.DB.DAL;

namespace SCG.DB.BLL.Implement
{
    public partial class DbPBLangService : ServiceBase<DbpbLang, long>, IDbPBLangService
    {

        public override IDao<DbpbLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbPBLangDao;
        }
        public long AddPBLang(DbpbLang PB)
        {
            long id = 0;
            id = ScgDbDaoProvider.DbPBLangDao.Save(PB);

            return id;
        }
        public void UpdatePBLang(IList<DbpbLang> PBLang)
        {

            foreach (DbpbLang pbLang in PBLang)
            {
                ScgDbDaoProvider.DbPBLangDao.DeleteByPBIdLanguageId(pbLang.PBID.Pbid, pbLang.LanguageID.Languageid);

                  
            }

            foreach (DbpbLang pbLang in PBLang)
            {
                DbpbLang dbPBlang = new DbpbLang();
                dbPBlang.PBID = ScgDbDaoProvider.DbPBDao.FindProxyByIdentity(pbLang.PBID.Pbid);
                dbPBlang.LanguageID = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(pbLang.LanguageID.Languageid);
                dbPBlang.Active = pbLang.Active;
                dbPBlang.CreBy = pbLang.CreBy;
                dbPBlang.CreDate = DateTime.Now.Date;
                dbPBlang.Description = pbLang.Description;
                dbPBlang.Comment = pbLang.Comment;
                dbPBlang.UpdBy = pbLang.UpdBy;
                dbPBlang.UpdDate = DateTime.Now.Date;
                dbPBlang.UpdPgm = pbLang.UpdPgm;
                ScgDbDaoProvider.DbPBLangDao.Save(dbPBlang);
            }

        }
        public void AddPBLang(IList<DbpbLang> PB)
        {
            foreach (DbpbLang pbLang in PB)
            {

                DbpbLang dbPBlang = new DbpbLang();
                dbPBlang.PBID = ScgDbDaoProvider.DbPBDao.FindProxyByIdentity(pbLang.PBID.Pbid);
                dbPBlang.LanguageID = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(pbLang.LanguageID.Languageid);
                dbPBlang.Active = pbLang.Active;
                dbPBlang.CreBy = pbLang.CreBy;
                dbPBlang.CreDate = DateTime.Now.Date;
                dbPBlang.Description = pbLang.Description;
                dbPBlang.Comment = pbLang.Comment;
                dbPBlang.UpdBy = pbLang.UpdBy;
                dbPBlang.UpdDate = DateTime.Now.Date;
                dbPBlang.UpdPgm = pbLang.UpdPgm;

                ScgDbDaoProvider.DbPBLangDao.Save(dbPBlang);

            }
        }
    }
}
