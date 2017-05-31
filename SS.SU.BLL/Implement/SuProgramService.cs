using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
    public partial class SuProgramService : ServiceBase<SuProgram, short>, ISuProgramService
    {
        public override IDao<SuProgram, short> GetBaseDao()
        {
            return DaoProvider.SuProgramDao;
        }

        public short AddProgram(SuProgram program)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(program.ProgramCode))
            {
                errors.AddError("Program.Error", new Spring.Validation.ErrorMessage("Program_CodeRequired"));
            }
            if (DaoProvider.SuProgramDao.IsDuplicateProgramCode(program))
            {
                errors.AddError("Program.Error", new Spring.Validation.ErrorMessage("UniqueProgramCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            return DaoProvider.SuProgramDao.Save(program);
        }
        public void UpdateProgram(SuProgram program)
        {
            #region Validate SuAnnouncementGroup
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(program.ProgramCode))
            {
                errors.AddError("Program.Error", new Spring.Validation.ErrorMessage("Program_CodeRequired"));
            }
            if (DaoProvider.SuProgramDao.IsDuplicateProgramCode(program))
            {
                errors.AddError("Program.Error", new Spring.Validation.ErrorMessage("UniqueProgramCode"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            DaoProvider.SuProgramDao.SaveOrUpdate(program);
        }
    }
}
