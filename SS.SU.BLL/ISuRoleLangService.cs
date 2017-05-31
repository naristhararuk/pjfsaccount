using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuRoleLangService : IService<SuRoleLang, long>
    {
        IList<SuRoleLang> FindBySuRoleLangCriteria(SuRoleLang criteria, int firstResult, int maxResults, string sortExpression);
        int CountBySuRoleLangCriteria(SuRoleLang criteria);
        IList<RoleLang> FindByRoleId(short roleId);
        void UpdateRoleLang(IList<SuRoleLang> roleLangList);
    }
}
