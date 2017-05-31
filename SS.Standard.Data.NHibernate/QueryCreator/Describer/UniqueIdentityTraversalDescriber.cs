using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryCreator.Describer
{
    public class UniqueIdentityTraversalDescriber : IIdentityTraversalDescribable
    {
        private String[] identityTraversals;
        private String identityName;

        public String IdentityName
        {
            get
            { 
                return identityName; 
            }
            set 
            {
                identityTraversals = new String[] { value };
                identityName = value; 
            }
        }

        public String[] describeIdentityTraversal(Object domain)
        {
            return this.identityTraversals;
        }
    }
}
