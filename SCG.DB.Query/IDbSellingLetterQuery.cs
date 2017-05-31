using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Service;

namespace SCG.DB.Query
{
    public interface IDbSellingLetterQuery : IQuery<DbSellingLetter, long>
    {
        long GetLetterIDByLetterNo(string letterNo);
    }
}
