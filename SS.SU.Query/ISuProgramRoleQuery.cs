using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DAL;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO.ValueObject;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuProgramRoleQuery : IQuery<SuProgramRole, short>
    {
        new IList<SuProgramRole> FindAll();
        new SuProgramRole FindByIdentity(short id);
        IList<ProgramRole> FindSuProgramRoleByRoleId(short RoleId, short LanguageId);
        ISQLQuery FindBySuProgramRoleQuery(SuProgramRole programrole, short languageId, string sortExpression, bool isCount);
        IList<ProgramRole> FindBySuProgramRole(SuProgramRole programRole, short languageId, int firstResult, int maxResults, string sortExpression);
        int CountBySuProgramRoleCriteria(SuProgramRole criteria, short languageId);
		IList<SuProgramRole> FindProgramPermission(ArrayList arrayRoleID);
        IList<ProgramInformation> FindProgramInfoByRole(SuRole role, short languageID, string sortExpression);
        IList<SuProgramRole> FindByProgramCode_UserID(string programCode, long userID);
        IList<ProgramRole> FindSuProgramRoleByProgramCode(string programCode);
    }
}
