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
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL.Implement
{
    public partial class DbServiceTeamService : ServiceBase <DbServiceTeam,long>, IDbServiceTeamService 
    {
        public override IDao<DbServiceTeam, long> GetBaseDao()
        {
            return ScgDbDaoProvider.DbServiceTeamDao;
        }
        public void AddServiceTeam(DbServiceTeam serviceTeam)
        {
            #region Validate

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(serviceTeam.ServiceTeamCode))
            {
                errors.AddError("ServiceTeam.Error", new Spring.Validation.ErrorMessage("ServiceTeam_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbServiceTeamDao.IsDuplicateServiceTeamCode(serviceTeam))
            {
                errors.AddError("ServiceTeam.Error", new Spring.Validation.ErrorMessage("UniqueServiceTeamCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbServiceTeamDao.Save(serviceTeam);
        }

        public void UpdateServiceTeam(DbServiceTeam serviceTeam)
        {
            #region Validate

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(serviceTeam.ServiceTeamCode))
            {
                errors.AddError("ServiceTeam.Error", new Spring.Validation.ErrorMessage("ServiceTeam_CodeRequired"));
            }
            if (ScgDbDaoProvider.DbServiceTeamDao.IsDuplicateServiceTeamCode(serviceTeam))
            {
                errors.AddError("ServiceTeam.Error", new Spring.Validation.ErrorMessage("UniqueServiceTeamCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            #endregion

            ScgDbDaoProvider.DbServiceTeamDao.Update(serviceTeam);
        }
    }
}
