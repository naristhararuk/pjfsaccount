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
    public partial class SuGlobalTranslateLangService : ServiceBase<SuGlobalTranslateLang, long>, ISuGlobalTranslateLangService
    {
        public override IDao<SuGlobalTranslateLang, long> GetBaseDao()
        {
            return DaoProvider.SuGlobalTranslateLangDao;
        }

        public IList<SuGlobalTranslateLang> FindByTranslateId(long translateId)
        {
            return DaoProvider.SuGlobalTranslateLangDao.FindByTranslateId(translateId);
        }

        public void UpdateGlobalTranslateLang(IList<SuGlobalTranslateLang> translateLangList)
        {
            foreach (SuGlobalTranslateLang tl in translateLangList)
            {
                DaoProvider.SuGlobalTranslateLangDao.DeleteByTranslatIdLanguageId(tl.Translate.TranslateId, tl.Language.Languageid);
            }

            foreach (SuGlobalTranslateLang tl in translateLangList)
            {
                SuGlobalTranslateLang globalTranslateLang = new SuGlobalTranslateLang();
                globalTranslateLang.Translate = DaoProvider.SuGlobalTranslateDao.FindProxyByIdentity(tl.Translate.TranslateId);
                globalTranslateLang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(tl.Language.Languageid);
                globalTranslateLang.TranslateWord = tl.TranslateWord;
                globalTranslateLang.Comment = tl.Comment;
                globalTranslateLang.Active = tl.Active;
                globalTranslateLang.CreBy = tl.CreBy;
                globalTranslateLang.CreDate = DateTime.Now.Date;
                globalTranslateLang.UpdBy = tl.UpdBy;
                globalTranslateLang.UpdDate = DateTime.Now.Date;
                globalTranslateLang.UpdPgm = tl.UpdPgm;

                DaoProvider.SuGlobalTranslateLangDao.Save(globalTranslateLang);
            }
        }
        //public short AddGlobalTranslateLang(SuGlobalTranslateLang translateLang)
        //{
        //    SuGlobalTranslateLang globalTranslateLang = new SuGlobalTranslateLang();
        //    globalTranslateLang.Translate = DaoProvider.SuGlobalTranslateDao.FindProxyByIdentity(translateLang.Translate.TranslateId);
        //    globalTranslateLang.Language = DaoProvider.SuLanguageDao.FindProxyByIdentity(translateLang.Language.Languageid);
        //    globalTranslateLang.TranslateWord = translateLang.TranslateWord;
        //    globalTranslateLang.Active = translateLang.Active;
        //    globalTranslateLang.CreBy = translateLang.CreBy;
        //    globalTranslateLang.CreDate = DateTime.Now.Date;
        //    globalTranslateLang.UpdBy = translateLang.UpdBy;
        //    globalTranslateLang.UpdDate = DateTime.Now.Date;
        //    globalTranslateLang.UpdPgm = translateLang.UpdPgm;

        //    return DaoProvider.SuGlobalTranslateLangDao.Save(globalTranslateLang);
        //}
        
    }
}
