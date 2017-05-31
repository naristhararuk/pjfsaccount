using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts
{
    public abstract class CriterionBase : IPhraseGenerator
    {
        public abstract string ToPhrase();
        public abstract string ToPhrase(bool ignoreLogical);
        public abstract bool IsCriterionParameterRequired();
    }
}
