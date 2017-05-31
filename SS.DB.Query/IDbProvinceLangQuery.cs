using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbProvinceLangQuery : IQuery<DbProvinceLang, long>
    {
        IList<SS.DB.DTO.ValueObject.ProvinceLang> FindByProvinceId(short provinceId);        
    }
}
