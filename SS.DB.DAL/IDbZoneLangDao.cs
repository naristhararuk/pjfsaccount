using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

namespace SS.DB.DAL
{
    public interface IDbZoneLangDao : IDao<DbZoneLang, long>
    {
        IList<DbZoneResult> FindZoneLangByZoneId(short zoneId);
        void DeleteZoneLangByID(short zoneId);
    }
}
