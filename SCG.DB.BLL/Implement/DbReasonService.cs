using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public partial class DbReasonService : ServiceBase<DbScgReason, short>, IDbReasonService
    {
        public override IDao<DbScgReason, short> GetBaseDao()
        {
            return ScgDbDaoProvider.DbReasonDao;
        }
        public void AddReason(DbScgReason reason)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(reason.ReasonCode))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeRequired"));
            }
            if (reason.DocumentTypeID == null)
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("DocumentTypeRequired"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbReasonDao.Save(reason);
        }
        public void UpdateReason(DbScgReason reason)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(reason.ReasonCode))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeRequired"));
            }
            if (reason.DocumentTypeID==null)
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("DocumentTypeRequired"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            ScgDbDaoProvider.DbReasonDao.SaveOrUpdate(reason);
        }
    }
}
