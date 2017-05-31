using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;

using SS.DB.DAL;
using SS.DB.DTO;
using SS.DB.Query;

namespace SS.SU.BLL.Implement
{
    public partial class SuDivisionLangService : ServiceBase<SuDivisionLang, long>, ISuDivisionLangService
    {
        public override IDao<SuDivisionLang, long> GetBaseDao()
        {
            return DaoProvider.SuDivisionLangDao;
        }

        public IList<SuDivisionLang> FindByDivisionId(short divisionId)
        {
            return DaoProvider.SuDivisionLangDao.FindByDivisionId(divisionId);
        }

        public void UpdateDivisionLang(IList<SuDivisionLang> divisionLangList)
        {
            foreach (SuDivisionLang dl in divisionLangList)
            {
                DaoProvider.SuDivisionLangDao.DeleteByDivisionIdLanguageId(dl.Division.Divisionid, dl.Language.Languageid);
            }

            foreach (SuDivisionLang dl in divisionLangList)
            {
                SuDivisionLang divisionLang = new SuDivisionLang();
                divisionLang.Division = DaoProvider.SuDivisionDao.FindProxyByIdentity(dl.Division.Divisionid);
                divisionLang.Language = SsDbDaoProvider.DbLanguageDao.FindProxyByIdentity(dl.Language.Languageid);
                divisionLang.DivisionName = dl.DivisionName;
                divisionLang.Comment = dl.Comment;
                divisionLang.Active = dl.Active;
                divisionLang.CreBy = dl.CreBy;
                divisionLang.CreDate = DateTime.Now.Date;
                divisionLang.UpdBy = dl.UpdBy;
                divisionLang.UpdDate = DateTime.Now.Date;
                divisionLang.UpdPgm = dl.UpdPgm;

                DaoProvider.SuDivisionLangDao.Save(divisionLang);
            }
        }        
    }
}
