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
    public interface IDbCostCenterQuery : IQuery<DbCostCenter, long>
    {
        ISQLQuery FindByCostCenterCriteria(string costCenterCode, string description, long companyID, bool isCount, string sortExpression);
        IList<DbCostCenter> GetCostCenterList(string costCenterCode, string description, long companyID, int firstResult, int maxResult, string sortExpression);
        IList<DbCostCenter> FindByDbCostCenter(long costCenterID, long companyID);
        IList<AutocompleteField> FindByDbCostCenterAutoComplete(string costCenterCode, long companyID);
        int CountByCostCenterCriteria(string costCenterCode, string description, long companyID);

        DbCostCenter getDbCostCenterByCostCenterCode(string costCenterCode);


        ISQLQuery FindCostCenterCompanyByCriteria(DbCostCenter costCenter, bool IsCount, string sortExpression);
        int CountCostCenterCompany(DbCostCenter costCenter);
        IList<CostCenterCompany> FindCostCenterCompany(DbCostCenter costCenter, int firstResult, int maxResult, string sortExpression);

        DbCostCenter getCostCenterCodeByCostCenterID(long CostCenterID);
        bool IsDuplicateCostCenterCode(DbCostCenter costCenter);
        IList<DbCostCenter> FindCostCenterByCompanyID(long comId);

		IList<DbCostCenter> FindByCostCenterIDList(IList<long> costCenterIDList);
    }
}
