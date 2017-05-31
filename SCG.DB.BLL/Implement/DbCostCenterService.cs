using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SCG.DB.DAL;
using SCG.DB.DTO;
using Spring.Validation;

using SS.Standard.Security;
using SCG.DB.BLL;
using SCG.DB.Query;
namespace SCG.DB.BLL.Implement
{
    public class DbCostCenterService:ServiceBase<DbCostCenter,long>,IDbCostCenterService
    {

        public IDbCompanyService DbCompanyService { get; set; }
        public override SS.Standard.Data.NHibernate.Dao.IDao<DbCostCenter, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbCostCenterDao;
        }
        //Delete with validation.
        public void DeleteCostCenter(DbCostCenter costCenter)
        {
            ValidationErrors errors = new ValidationErrors();
            if (string.IsNullOrEmpty(costCenter.CostCenterID.ToString()))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage( "Couldn't find costcenter ID"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            try
            {
                ScgDbDaoProvider.DbCostCenterDao.Delete(costCenter);
                ScgDbDaoProvider.DbCostCenterDao.SyncDeleteCostCenter(costCenter.CostCenterCode);
            }
            catch (Exception)
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Delete Fail."));
                throw new ServiceValidationException(errors);
            }
        }

        #region IDbCostCenterService Members


        public void UpdateCostCenter(DbCostCenter costCenter)
        {
            ValidationErrors errors = new ValidationErrors();


            if (string.IsNullOrEmpty(costCenter.CostCenterID.ToString()))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Couldn't find costcenter ID"));
            }

            if (string.IsNullOrEmpty(costCenter.CompanyCode.ToString()))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("CompanyCode can't be null."));
            }

            DateTime valid = costCenter.Valid;
            DateTime expire = costCenter.Expire;

            if (valid > expire)
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Expire date must be more than Valid date."));

            }

            if (ScgDbQueryProvider.DbCostCenterQuery.IsDuplicateCostCenterCode(costCenter))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("DuplicationCostCenterCode"));
            }

            try
            {
                DbCompany company = DbCompanyService.FindByCompanycode(costCenter.CompanyCode);
                costCenter.CompanyID = company;
            }
            catch (NullReferenceException)
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Couldn't find Company."));
            }
         

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            
            ScgDbDaoProvider.DbCostCenterDao.Update(costCenter);
            ScgDbDaoProvider.DbCostCenterDao.SyncUpdateCostCenter(costCenter.CostCenterCode);
        }
        public void UpdateCostCenterToExp(DbCostCenter costCenter)
        {
            ScgDbDaoProvider.DbCostCenterDao.SyncUpdateCostCenter(costCenter.CostCenterCode);
        }
        public bool IsDuplicateCostCenterCode(DbCostCenter costCenterCode)
        {
            return ScgDbDaoProvider.DbCostCenterDao.IsDuplicateCostCenterCode(costCenterCode);
        }

        public void AddCostCenter(DbCostCenter costCenter)
        {
            ValidationErrors errors = new ValidationErrors();
            try
            {
                DbCompany company = DbCompanyService.FindByCompanycode(costCenter.CompanyCode);
                costCenter.CompanyID.CompanyID = company.CompanyID;
            }
            catch (NullReferenceException)
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Couldn't find Company."));
            }

            if (string.IsNullOrEmpty(costCenter.CostCenterID.ToString()))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Couldn't find costcenter ID"));
            }

            if (string.IsNullOrEmpty(costCenter.CompanyCode.ToString()))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("CompanyCode can't be null."));
            }

            DateTime valid = costCenter.Valid;
            DateTime expire = costCenter.Expire;

            if (valid > expire)
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("Expire date must be more than Valid date."));
            }

            if (ScgDbQueryProvider.DbCostCenterQuery.IsDuplicateCostCenterCode(costCenter))
            {
                errors.AddError("CostCenter.Error", new ErrorMessage("DuplicationCostCenterCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbCostCenterDao.Save(costCenter);
            ScgDbDaoProvider.DbCostCenterDao.SyncNewCostCenter();
        }

        public DbCostCenter getDbCostCenterByCostCenterCode(string costCenterCode)
        {
            return ScgDbQueryProvider.DbCostCenterQuery.getDbCostCenterByCostCenterCode(costCenterCode);
        }

        #endregion
    }
}
