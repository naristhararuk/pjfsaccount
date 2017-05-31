using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.DB.Query
{
    public interface IDbCompanyQuery : IQuery<DbCompany, long>
    {
        ISQLQuery FindCompanyByCriteria(DbCompany company,bool? flagActive, bool isCount, string sortExpression);
        ISQLQuery FindCompanyByCriteria1(DbCompany company, short CurrentLang,bool? flagActive, bool isCount, string sortExpression);
        IList<CompanyResult> GetCompanyList(DbCompany company, bool? flagActive, int firstResult, int maxResult, string sortExpression);
        IList<CompanyResult> GetCompanyList(DbCompany company, short CurrentLang,bool? flagActive, int firstResult, int maxResult, string sortExpression);

        int CountCompanyByCriteria(DbCompany company, bool? flagActive);
        int CountCompanyByCriteria(DbCompany company, short CurrentLang, bool? flagActive);

        IList<DbCompany> FindAutoComplete(string companyCode, bool? useECC, bool? flagActive);
        DbCompany getDbCompanyByCompanyCode(string companyCode);
        Guid getMileageProfileByCompanyID(Int64 companyID);
        DbCompany getDbCompanyBankAccountByCompanyCode(string companyCode);
        bool getUseECCOfComOfUserByUserName(string userName);

        SapInstanceData GetSAPInstanceByCompanyCode(string companyCode);
        DbSapInstance GetSAPDocTypeForPosting(string companyCode);

        DbCompany GetDbCompanyByCriteria(string companyCode, bool? useECC, bool? flagActive);

        long GetFnPerdiemProfileCompany(long companyID);
    }
}
