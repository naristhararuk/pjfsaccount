using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryCreator.Describer
{
    public interface IIdentityTraversalDescribable
    {
        String[] describeIdentityTraversal(Object domain);
    }
}
