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
using SS.Standard.Security;

namespace SCG.DB.BLL.Implement
{
    public partial class RejectReasonService : ServiceBase<DbRejectReason, int>, IRejectReasonService
    {
        public IUserAccount UserAccount { get; set; }
        public IRejectReasonDao RejectReasonDao { get; set; }
        public override IDao<DbRejectReason, int> GetBaseDao()
        {
            return ScgDbDaoProvider.RejectReasonDao;
        }
        public int AddReason(DbRejectReason reason)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(reason.ReasonCode))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeRequired"));
            }
            if (RejectReasonDao.IsDuplicateReasonCode(reason))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeIsDuplicate"));
            }
            //if (reason.DocumentTypeID.Equals(0))
            //{
            //    errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("RequestTypeIsRequired"));
            //}
            //if (reason.WorkFlowStateEventID.Equals(0))
            //{
            //    errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("WorkFlowStateEventIsRequired"));
            //}
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            reason.CreBy = UserAccount.UserID;
            reason.CreDate = DateTime.Now.Date;
            reason.UpdBy = UserAccount.UserID;
            reason.UpdDate = DateTime.Now.Date;
            reason.UpdPgm = UserAccount.CurrentProgramCode;

            return ScgDbDaoProvider.RejectReasonDao.Save(reason);
        }
        public void UpdateReason(DbRejectReason reason)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(reason.ReasonCode))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeRequired"));
            }
            if (RejectReasonDao.IsDuplicateReasonCode(reason))
            {
                errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("ReasonCodeIsDuplicate"));
            }
            //if (reason.DocumentTypeID.Equals(0))
            //{
            //    errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("RequestTypeIsRequired"));
            //}
            //if (reason.WorkFlowStateEventID.Equals(0))
            //{
            //    errors.AddError("ReasonReject.Error", new Spring.Validation.ErrorMessage("WorkFlowStateEventIsRequired"));
            //}



            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            reason.CreBy = UserAccount.UserID;
            reason.CreDate = DateTime.Now.Date;
            reason.UpdBy = UserAccount.UserID;
            reason.UpdDate = DateTime.Now.Date;
            reason.UpdPgm = UserAccount.CurrentProgramCode;

            ScgDbDaoProvider.RejectReasonDao.SaveOrUpdate(reason);
        }
    }
}
