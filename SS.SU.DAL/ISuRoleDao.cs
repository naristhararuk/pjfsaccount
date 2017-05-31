using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuRoleDao : IDao<SuRole, short>
    {
        bool RolePremission(string username);
        //IList<RoleLang> FindAllRole(short languageId);
        bool IsDuplicateRoleCode(SuRole role);
    }
}
