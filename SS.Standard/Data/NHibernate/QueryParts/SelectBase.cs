using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts
{
    public abstract class SelectBase : IPhraseGenerator
    {
        public abstract string ToPhrase();
    }
}
