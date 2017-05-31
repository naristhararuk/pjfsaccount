using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbCompanyDao : NHibernateDaoBase<DbCompany, long>, IDbCompanyDao
    {
        public DbCompanyDao()
        {

        }

        public DbCompany FindByCompanycode(string companycode)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbCompany), "comp");
            criteria.Add(Expression.Eq("comp.CompanyCode", companycode));
            return criteria.UniqueResult<DbCompany>();
        }

        public bool IsDuplicateCompanyCode(DbCompany company)
        {
            IList<DbCompany> list = GetCurrentSession().CreateQuery("from DbCompany p where p.CompanyID <> :CompanyID and p.CompanyCode = :CompanyCode")
                  .SetInt64("CompanyID", company.CompanyID)
                  .SetString("CompanyCode", company.CompanyCode)
                  .List<DbCompany>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void SyncNewCompany()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncNewCompanyData]");
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncUpdateCompany(string companyCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncUpdateCompanyData] :companyCode ");
            query.SetString("companyCode", companyCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncDeleteCompany(string CompanyCode)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncDeleteCompanyData] :CompanyCode ");
            query.SetString("CompanyCode", CompanyCode);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void AddFnPerdiemProfileCompany(long companyID, long perdiemProfileID,long userID)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[AddFnPerdiemProfileCompany] :companyID , :perdiemProfileID , :userID ");
            query.SetInt64("companyID", companyID);
            query.SetInt64("perdiemProfileID", perdiemProfileID);
            query.SetInt64("userID", userID);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void UpdateFnPerdiemProfileCompany(long companyID, long perdiemProfileID, long userID)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[UpdateFnPerdiemProfileCompany] :companyID , :perdiemProfileID , :userID ");
            query.SetInt64("companyID", companyID);
            query.SetInt64("perdiemProfileID", perdiemProfileID);
            query.SetInt64("userID", userID);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void DeleteFnPerdiemProfileCompany(long companyID)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec [dbo].[DeleteFnPerdiemProfileCompany] :companyID ");
            query.SetInt64("companyID", companyID);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
