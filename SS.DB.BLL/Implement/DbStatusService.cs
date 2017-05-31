using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.DB.DTO;
using SS.DB.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.DB.BLL.Implement
{
    public partial class DbStatusService : ServiceBase<DbStatus, short>, IDbStatusService
    {

        #region IDbStatusService Members
        public override IDao<DbStatus, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbStatusDao;
        }

        public IList<DbStatus> FindByDbStatusCriteria(DbStatus criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<DbStatus>(SsDbDaoProvider.DbStatusDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        }

        public int CountByDbStatusCriteria(DbStatus criteria)
        {
            return NHibernateDaoHelper.CountByCriteria(SsDbDaoProvider.DbStatusDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria });
        }

        public short AddStatus(DbStatus status)
        {
            #region Validate DbStatus
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(status.GroupStatus))
            {
                errors.AddError("Status.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            else
            {
                IList<DbStatus> statusList = SsDbDaoProvider.DbStatusDao.FindByStatusCriteria(status);
                if (statusList.Count != 0)
                {
                    errors.AddError("Status.Error", new Spring.Validation.ErrorMessage("StatusDuplicated"));
                }
            }
            if (string.IsNullOrEmpty(status.Status))
            {
                errors.AddError("Status.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            DbStatus dbstatus = new DbStatus();
            dbstatus.GroupStatus = status.GroupStatus;
            dbstatus.Status = status.Status;
            dbstatus.Comment = status.Comment;
            dbstatus.Active = status.Active;
            dbstatus.CreBy = status.CreBy;
            dbstatus.CreDate = DateTime.Now.Date;
            dbstatus.UpdBy = status.UpdBy;
            dbstatus.UpdDate = DateTime.Now.Date;
            dbstatus.UpdPgm = status.UpdPgm;

            return SsDbDaoProvider.DbStatusDao.Save(dbstatus);
        }

        public void UpdateStatus(DbStatus status)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DbStatus dbstatus = SsDbDaoProvider.DbStatusDao.FindByIdentity(status.StatusID);
            dbstatus.GroupStatus = status.GroupStatus;
            dbstatus.Status = status.Status;
            dbstatus.Comment = status.Comment;
            dbstatus.Active = status.Active;
            dbstatus.UpdBy = status.UpdBy;
            dbstatus.UpdDate = DateTime.Now.Date;
            dbstatus.UpdPgm = status.UpdPgm;

            SsDbDaoProvider.DbStatusDao.SaveOrUpdate(dbstatus);
        }

        #endregion
    }
}
