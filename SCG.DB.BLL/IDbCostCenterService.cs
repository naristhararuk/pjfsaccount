using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbCostCenterService :IService<DbCostCenter ,long>
    {
        void DeleteCostCenter(DbCostCenter costCenter);
        void AddCostCenter(DbCostCenter costCenter);
        bool IsDuplicateCostCenterCode(DbCostCenter costCenterCode);
        void UpdateCostCenter(DbCostCenter costCenter);
        DbCostCenter getDbCostCenterByCostCenterCode (string costCenterCode);
        void UpdateCostCenterToExp(DbCostCenter costCenter);
    }
}
