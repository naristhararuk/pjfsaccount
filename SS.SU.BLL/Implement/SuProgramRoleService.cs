using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.Standard.Utilities;

namespace SS.SU.BLL.Implement
{
    public partial class SuProgramRoleService : ServiceBase<SuProgramRole, short>, ISuProgramRoleService
    {
        public override IDao<SuProgramRole, short> GetBaseDao()
        {
            return DaoProvider.SuProgramRoleDao;
        }
        public void UpdateProgramRole(IList<SuProgramRole> programRoleList)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            foreach (SuProgramRole pr in programRoleList)
            {
                SuProgram program = new SuProgram();
                program = DaoProvider.SuProgramDao.FindByIdentity(pr.Program.Programid);
                if (program == null)
                {
                    errors.AddError("ProgramRolelate.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
                }
            }
            foreach (SuProgramRole pr in programRoleList)
            {
                DaoProvider.SuProgramRoleDao.DeleteByProgramRoleId(pr.Role.RoleID, pr.Program.Programid);
            }

            foreach (SuProgramRole pr in programRoleList)
            {
                SuProgramRole programRole = new SuProgramRole();
                programRole.Role = DaoProvider.SuRoleDao.FindProxyByIdentity(pr.Role.RoleID);
                programRole.Program = DaoProvider.SuProgramDao.FindProxyByIdentity(pr.Program.Programid);
                programRole.AddState = pr.AddState;
                programRole.EditState = pr.EditState;
                programRole.DeleteState = pr.DeleteState;
                programRole.DisplayState = pr.DisplayState;
                programRole.Comment = pr.Comment;
                programRole.Active = pr.Active;
                programRole.CreBy = pr.CreBy;
                programRole.CreDate = DateTime.Now.Date;
                programRole.UpdBy = pr.UpdBy;
                programRole.UpdDate = DateTime.Now.Date;
                programRole.UpdPgm = pr.UpdPgm;

                DaoProvider.SuProgramRoleDao.Save(programRole);
            }
        }
        public short AddProgramRole(SuProgramRole programRole)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
       
            programRole.CreDate = DateTime.Now.Date;

            programRole.UpdDate = DateTime.Now.Date;

            if (DaoProvider.SuProgramRoleDao.IsExist(programRole))
            {
                errors.AddError("ProgramRole.Error",new Spring.Validation.ErrorMessage("This item was aready exist."));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            return DaoProvider.SuProgramRoleDao.Save(programRole);
        }

        #region ISuProgramRoleService Members


        public void DeleteProgramRole(SuProgramRole proGramRole)
        {
            try
            {
                DaoProvider.SuProgramRoleDao.Delete(proGramRole);
            }
            catch (Exception)
            {
                proGramRole.Active = false;
                DaoProvider.SuProgramRoleDao.Update(proGramRole);
            }
        }

        #endregion
    }
}
