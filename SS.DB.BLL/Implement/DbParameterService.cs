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
using SS.Standard.Utilities;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.DAL;
using SS.DB.Helper;
using SS.SU.BLL;
using SS.DB.Query;

namespace SS.DB.BLL.Implement
{
    public partial class DbParameterService : ServiceBase<DbParameter, short>, IDbParameterService
    {
        public override IDao<DbParameter, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbParameterDao;
        }

        #region IDbParameterService Members

        public void AddParameter(DbParameter parameter)
        {
            #region Validate DbParameter
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (parameter.SeqNo == null)
            {
                errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("RequiredSeqNo"));
            }

            if (string.IsNullOrEmpty(parameter.ParameterValue))
            {
                errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            else
            {
                DbParameter dbparameter = new DbParameter();
                IList<Parameter> parameterList = SsDbDaoProvider.DbParameterDao.FindByDbParameterCriteria(parameter);
                if (parameterList.Count != 0)
                {
                    //errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("ParameterValueIsAlready"));
                    errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("SeqNoNotAllow"));
                }
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbParameterDao.Save(parameter);
        }

        public void UpdateParameter(DbParameter parameter)
        {
            #region Validate DbParameter
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (parameter.ParameterType.ToLower().Trim() == "integer")
            {
                try
                {
                    int typeDecimal = int.Parse(parameter.ParameterValue);
                }
                catch
                {
                    errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("RequiredTypeInteger"));
                }
            }
            else if (parameter.ParameterType.ToLower().Trim() == "date")
            {
                try
                {
                    DateTime typeDate = DateTime.Parse(parameter.ParameterValue);
                }
                catch
                {
                    errors.AddError("Parameter.Error", new Spring.Validation.ErrorMessage("RequiredTypeDateTime"));
                }
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbParameterDao.SaveOrUpdate(parameter);
            ParameterServices.Neologize();
        }
        #endregion
    }
}
