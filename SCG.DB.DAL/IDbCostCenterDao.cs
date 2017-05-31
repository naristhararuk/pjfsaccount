using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbCostCenterDao :IDao <DbCostCenter ,long>
    {
        //void addDbCostCenter(DbCostCenter tmpDbCosCenter);
        bool IsDuplicateCostCenterCode(DbCostCenter costCentercode);
        void SyncNewCostCenter();
        void SyncUpdateCostCenter(string costCenterCode);
        void SyncDeleteCostCenter(string costCenterCode);
    }
}
