using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;

namespace SS.Standard.Data.NHibernate.QueryParts
{
    public interface IPatternMatchCriterion
    {
        MatchOption? MatchOption
        {
            get;
            set;
        }
    }
}
