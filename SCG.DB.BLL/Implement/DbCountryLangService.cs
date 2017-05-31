using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;

namespace SCG.DB.BLL.Implement
{
    public partial class DbCountryLangService : ServiceBase<DbCountryLang, short>, IDbCountryLangService
    {
        public override IDao<DbCountryLang, short> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCountryLangDao;
        }
        public IList<SCG.DB.DTO.ValueObject.CountryLang> FindByCountryId(short CountryId)
        {
            return ScgDbDaoProvider.DbCountryLangDao.FindByCountryId(CountryId);
        }
        //public IList<DbCountryLang> FindByDbCountryLangCriteria(DbCountryLang criteria, int firstResult, int maxResults, string sortExpression)
        //{
        //    return NHibernateDaoHelper.FindPagingByCriteria<DbCountryLang>(ScgDbDaoProvider.DbCountryLangDao, "FindByDbCountryLangCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        //}
        public IList<DbCountryLang> FindByDbCountryLangQuery(DbCountryLang criteria, short countryId, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<DbCountryLang>(ScgDbDaoProvider.DbCountryLangDao, "FindByDbCountryLangQuery", new object[] { criteria, countryId, languageId }, firstResult, maxResults, sortExpression);
        }
        public int CountByDbCountryLangCriteria(DbCountryLang criteria, short countryId, short languageId)
        {
            return NHibernateDaoHelper.CountByCriteria(ScgDbDaoProvider.DbCountryLangDao, "FindByDbCountryLangQuery", new object[] { criteria, countryId, languageId });
        }
        public void UpdateCountryLang(IList<DbCountryLang> countryLangList)
        {
            if (countryLangList.Count > 0)
            {
                ScgDbDaoProvider.DbCountryLangDao.DeleteAllCountryLang(countryLangList[0].Country.CountryID);
            }
            foreach (DbCountryLang countryLang in countryLangList)
            {
                ScgDbDaoProvider.DbCountryLangDao.Save(countryLang);
            }
        }
       

    }
}
