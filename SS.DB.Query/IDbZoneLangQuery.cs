using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbZoneLangQuery : IQuery<DbZoneLang, long> 
    {
        //ICriteria FindZoneLangById(short Id);
        //IList<DbZoneLang> GetZoneLangList(short Id, int firstResult, int maxResult, string sortExpression);
        //int CountZoneLangByCriteria(short Id);

        DbZoneResult FindByDbZoneLangKey(short zoneID, short languageId);
    }
}
