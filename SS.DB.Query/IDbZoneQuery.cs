using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbZoneQuery : IQuery<DbZone, short>
    {
        IList<DbZoneResult> GetZoneList(short languageId, int firstResult, int maxResult, string sortExpression);
        int CountZoneByCriteria();
        ISQLQuery FindZoneByCriteria(short languageId, string sortExpression, bool isCount);
        IList<DbZoneResult> FindZoneByID(short zoneId, short languageId);
        IList<DbZoneResult> FindZone(short languageId);
    }
}
