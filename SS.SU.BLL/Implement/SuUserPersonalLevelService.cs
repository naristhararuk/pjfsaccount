using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DAL;
using SS.Standard.Utilities;



namespace SS.SU.BLL.Implement
{
    public class SuUserPersonalLevelService : ServiceBase<SuUserPersonalLevel, string>, ISuUserPersonalLevelService
    {
        public override IDao<SuUserPersonalLevel, string> GetBaseDao()
        {
            return DaoProvider.SuUserPersonalLevelDao;
        }
        public void AddSuUserPersonalLevel(SuUserPersonalLevel suUserPersonalLevel)
        {
            CheckDataValueUpdate(suUserPersonalLevel, true);
            DaoProvider.SuUserPersonalLevelDao.Save(suUserPersonalLevel);
        }

        public void UpdateSuUserPersonalLevel(SuUserPersonalLevel suUserPersonalLevel)
        {
            CheckDataValueUpdate(suUserPersonalLevel, false);
            DaoProvider.SuUserPersonalLevelDao.Update(suUserPersonalLevel);
        }
        private void CheckDataValueUpdate(SuUserPersonalLevel suUserPersonalLevel, bool newmode)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(suUserPersonalLevel.PersonalLevel))
            {
                errors.AddError("SuUserPersonalLevel.Error", new Spring.Validation.ErrorMessage("Personal Level Required"));
            }
            if (DaoProvider.SuUserPersonalLevelDao.IsDuplicateCode(suUserPersonalLevel) && newmode)
            {
                errors.AddError("SuUserPersonalLevel.Error", new Spring.Validation.ErrorMessage("Personal Level is Duplicate"));
            }
            if (string.IsNullOrEmpty(suUserPersonalLevel.Description))
            {
                errors.AddError("SuUserPersonalLevel.Error", new Spring.Validation.ErrorMessage("Description Required"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
    }
}
