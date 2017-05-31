using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SCG.DB.DAL;
using SS.Standard.Utilities;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;

namespace SCG.DB.BLL.Implement
{
    public partial class DbIOService : ServiceBase<DbInternalOrder, long>, IDbIOService
    {
        public override IDao<DbInternalOrder, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbIODao;
        }
        public bool IsExistIO(DbInternalOrder io)
        {
            return ScgDbDaoProvider.DbIODao.FindByIONumber(io.IOID, io.IONumber);
        }
        public void AddIO(DbInternalOrder io)
        {
            //#region Validate Internal Order
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            //if (string.IsNullOrEmpty(io.IONumber))
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Number Required"));
            //}
            //else if (ScgDbDaoProvider.DbIODao.FindByIONumber(io.IOID, io.IONumber))
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Number already exist"));
            //}
            //else if (string.IsNullOrEmpty(io.IOType))
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Type Required"));
            //}
            //else if (string.IsNullOrEmpty(io.IOText))
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Text Required"));
            //}
            //else if (string.IsNullOrEmpty(io.CostCenterCode))
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Cost Center Code Required"));
            //}
            //else if (io.EffectiveDate == null)
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Effective Date Required"));
            //}
            //else if (io.ExpireDate == null)
            //{
            //    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Expire Date Required"));
            //}
     
            //if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            //#endregion

            ScgDbDaoProvider.DbIODao.Save(io);
            ScgDbDaoProvider.DbIODao.SyncNewIO();
            
        }
        public void UpdateIO(DbInternalOrder io)
        {
           //#region Validate Internal Order
           // Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
           // if (string.IsNullOrEmpty(io.IONumber))
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Number Required"));
           // }
           // else if(ScgDbDaoProvider.DbIODao.FindByIONumber(io.IOID,io.IONumber))
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Number already exist"));
           // }
           // else if (string.IsNullOrEmpty(io.IOType))
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Type Required"));
           // }
           // else if (string.IsNullOrEmpty(io.IOText))
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("IO Text Required"));
           // }
           // else if (string.IsNullOrEmpty(io.CostCenterCode))
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Cost Center Code Required"));
           // }
           // else if (io.EffectiveDate == null)
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Effective Date Required"));
           // }
           // else if (io.ExpireDate == null)
           // {
           //     errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Expire Date Required"));
           // }
     
           // if (!errors.IsEmpty) throw new ServiceValidationException(errors);
           // #endregion


            ScgDbDaoProvider.DbIODao.Update(io);
            ScgDbDaoProvider.DbIODao.SyncUpdateIO(io.IONumber);
        }
        public void DeleteIO(DbInternalOrder io)
        {
            ScgDbDaoProvider.DbIODao.Delete(io);
            ScgDbDaoProvider.DbIODao.SyncDeleteIO(io.IONumber);
        }
    }
}
