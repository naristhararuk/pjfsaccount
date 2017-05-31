using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.DAL;
using SS.DB.Helper;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.DB.BLL.Implement
{
    public partial class DbParameterGroupService : ServiceBase<DbParameterGroup, short>, IDbParameterGroupService
    {
        public override IDao<DbParameterGroup, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbParameterGroupDao;
        }

        #region IDbParameterGroupService Members

        public void AddParameterGroup(DbParameterGroup parameterGroup)
        {
            #region Validate DbParameterGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(parameterGroup.GroupName))
            {
                errors.AddError("ParameterGroup.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            else
            {
                DbParameterGroup DbParameterGroup = new DbParameterGroup();
                IList<ParameterGroup> parameterGroupList = SsDbDaoProvider.DbParameterGroupDao.FindByDbParameterGroupCriteria(parameterGroup);
                if (parameterGroupList.Count != 0)
                {
                    errors.AddError("ParameterGroup.Error", new Spring.Validation.ErrorMessage("GroupNameIsAlready"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbParameterGroupDao.Save(parameterGroup);
        }

        public void UpdateParameterGroup(DbParameterGroup parameterGroup)
        {
            #region Validate DbParameterGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(parameterGroup.GroupName))
            {
                errors.AddError("ParameterGroup.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbParameterGroupDao.SaveOrUpdate(parameterGroup);
        }
        #endregion
    }
}
