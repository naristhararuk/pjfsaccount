using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts
{
    public abstract class OperandBase : IPhraseGenerator
    {
        public abstract string ToPhrase();
    }
}
