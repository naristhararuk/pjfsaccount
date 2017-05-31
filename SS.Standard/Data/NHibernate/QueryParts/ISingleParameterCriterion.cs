using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts
{
    public interface ISingleParameterCriterion
    {
        string CriterionParameterName
        {
            get;
        }

        Type CriterionParameterType
        {
            get;
        }
    }
}
