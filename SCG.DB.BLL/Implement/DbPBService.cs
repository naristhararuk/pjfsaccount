using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SCG.DB.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Utilities;
using SCG.DB.DTO.DataSet;

namespace SCG.DB.BLL.Implement
{
    public partial class DbPBService : ServiceBase<Dbpb, long>, IDbPBService
    {
        public override IDao<Dbpb, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbPBDao;
        }

        public long AddPB(Dbpb pb, DBPbDataSet pbDataSet)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(pb.PBCode))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PBCode Required"));
            }
            if (ScgDbDaoProvider.DbPBDao.IsDuplicatePBCode(pb))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PBCode is duplicate"));
            }
            if (string.IsNullOrEmpty(pb.CompanyCode))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
            }
            if (pb.PettyCashLimit <= 0)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PettyCashLimitMustMoreThanZero"));
            }
            if (pb.RepOffice && !pb.MainCurrencyID.HasValue)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("MainCurrencyIsRequire"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            long pbid = ScgDbDaoProvider.DbPBDao.Save(pb);

            DBPbDataSet.DbPBCurrencyRow[] pbCurrencyList = pbDataSet.DbPBCurrency.Select() as DBPbDataSet.DbPBCurrencyRow[];
            foreach (DBPbDataSet.DbPBCurrencyRow currency in pbCurrencyList)
            {
                currency.BeginEdit();
                currency.PBID = pbid;
                currency.EndEdit();
            }

            ScgDbDaoProvider.DbPBCurrencyDao.Persist(pbDataSet.DbPBCurrency);

            return pbid;
           
        }
        public void UpdatePB(Dbpb pb, DBPbDataSet pbDataSet)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(pb.PBCode))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PBCode Required"));
            }
            if (ScgDbDaoProvider.DbPBDao.IsDuplicatePBCode(pb))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PBCode is duplicate"));
            }
            if (string.IsNullOrEmpty(pb.CompanyCode))
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("Company_CodeRequired"));
            }
            if (pb.PettyCashLimit<=0)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("PettyCashLimitMustMoreThanZero"));
            }
            if (pb.RepOffice && !pb.MainCurrencyID.HasValue)
            {
                errors.AddError("PB.Error", new Spring.Validation.ErrorMessage("MainCurrencyIsRequire"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion
            
            ScgDbDaoProvider.DbPBDao.SaveOrUpdate(pb);
            ScgDbDaoProvider.DbPBCurrencyDao.Persist(pbDataSet.DbPBCurrency);
        }

        public void DeletePB(Dbpb pb)
        {
            ScgDbDaoProvider.DbPBDao.Delete(pb);
           
        }
    }
}
