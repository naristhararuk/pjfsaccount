using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.QueryDao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
    public partial class SuProgramLangService : ServiceBase<SuProgramLang, long>, ISuProgramLangService
    {
        public override IDao<SuProgramLang, long> GetBaseDao()
        {
            return DaoProvider.SuProgramLangDao;
        }
        public IList<ProgramLang> FindByProgramId(short programId)
        {
            return DaoProvider.SuProgramLangDao.FindByProgramId(programId);
        }
        public IList<SuProgramLang> FindBySuProgramLangCriteria(SuProgramLang criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<SuProgramLang>(DaoProvider.SuProgramLangDao, "FindBySuProgramLangCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        }
        public IList<ProgramLang> FindBySuProgramLangQuery(SuProgramLang criteria, short roleId, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<ProgramLang>(DaoProvider.SuProgramLangDao, "FindBySuProgramLangQuery", new object[] { criteria, roleId, languageId, false }, firstResult, maxResults, sortExpression);
        }
        public int CountBySuProgramLangCriteria(SuProgramLang criteria, short roleId, short languageId)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuProgramLangDao, "FindBySuProgramLangQuery", new object[] { criteria, roleId, languageId, true });
        }
        public void UpdateProgramLang(IList<SuProgramLang> programLangList)
        {
            if (programLangList.Count > 0)
            {
                DaoProvider.SuProgramLangDao.DeleteAllProgramLang(programLangList[0].Program.Programid);
            }
            foreach (SuProgramLang programLang in programLangList)
            {
                DaoProvider.SuProgramLangDao.Save(programLang);
            }
        }
    }
}
