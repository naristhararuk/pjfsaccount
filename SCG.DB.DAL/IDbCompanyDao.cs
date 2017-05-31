using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbCompanyDao : IDao<DbCompany, long>
    {
        bool IsDuplicateCompanyCode(DbCompany company);
        //short FindCountryId(string countryCode);
        DbCompany FindByCompanycode(string companyCode);
        void SyncNewCompany();
        void SyncUpdateCompany(string companyCode);
        void SyncDeleteCompany(string companyCode);
        void DeleteFnPerdiemProfileCompany(long companyID);
        void UpdateFnPerdiemProfileCompany(long companyID, long perdiemProfileID, long userID);
        void AddFnPerdiemProfileCompany(long companyID, long perdiemProfileID,long userID);
    }
}
