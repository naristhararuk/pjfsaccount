using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryParts;

namespace SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators
{
    internal class MatchOptionManipulator
    {
        public static MatchMode GetMatchModeValue(MatchOption? matchOption)
        {
            if (matchOption == MatchOption.End)
            {
                return MatchMode.End;
            }
            else if (matchOption == MatchOption.Exact)
            {
                return MatchMode.Exact;
            }
            else if (matchOption == MatchOption.Start)
            {
                return MatchMode.Start;
            }
            else
            {
                return MatchMode.Anywhere;
            }
        }
    }
}
