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
    public class SuLanguageQuery : QueryBase<SuLanguage, int>, ISuLanguageQuery
    {
        public override IQuery<SuLanguage, int> GetBaseQuery()
        {
            return QueryProvider.SuLanguageQuery;

        }
    }
  
}
