using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using NHibernate;
using SS.SU.DTO.ValueObject;

namespace SS.SU.DAL
{
    public interface ISuRoleLangDao : IDao<SuRoleLang, long>
    {
        ICriteria FindBySuRoleLangCriteria(SuRoleLang rolelang);
        IList<RoleLang> FindByRoleId(short roleId);
        void DeleteAllRoleLang(short roleId);
    }
}
