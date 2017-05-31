using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;

namespace SS.DB.BLL
{
    public interface IDbZoneLangService : IService<DbZoneLang, long>
    {
        IList<DbZoneResult> FindZoneLangByZoneID(short zoneId);
        void UpdateZoneLang(IList<DbZoneLang> zoneLangList);
        long AddZoneLang(DbZone zone, DbZoneLang zoneLang);
        void UpdateZoneLang(DbZone zone);
    }
}
