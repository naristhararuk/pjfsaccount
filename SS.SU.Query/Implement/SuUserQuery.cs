using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.Implement;


using SS.SU.DTO;
using SS.SU.DAL;


namespace SS.SU.Query.Implement
{
    public class SuUserQuery : QueryBase<SuUser, int>, ISuUserQuery
    {
        public override IQuery<SuUser, int> GetBaseQuery()
        {
            return QueryProvider.SuUserQuery;

        }
    }
  
}
