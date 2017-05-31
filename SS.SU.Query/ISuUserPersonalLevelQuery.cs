using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuUserPersonalLevelQuery : IQuery<SuUserPersonalLevel, string>
    {
        IList<SuUserPersonalLevel> GetMaintainPersonalLevelListByCriteria(MaintainPersonalLevelCriteria criteria, int startRow, int pageSize, string sortExpression);
        int CountMaintainPersonalLevel(MaintainPersonalLevelCriteria criteria);
        IList<SuUserPersonalLevel> GetPLList();
    }
}
