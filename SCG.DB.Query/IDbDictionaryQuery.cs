using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbDictionaryQuery : IQuery<DbDictionary,int>
    {
        DbDictionary GetVerb3ByVerb1(string verb1);
    }
}
