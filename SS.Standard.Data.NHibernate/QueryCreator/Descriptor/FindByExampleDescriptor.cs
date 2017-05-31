using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts;
using SS.Standard.Data.NHibernate.QueryCreator.Describer;
using SS.Standard.Data.NHibernate.QueryParts.Constants;

namespace SS.Standard.Data.NHibernate.QueryCreator.Descriptor
{
    public class FindByExampleDescriptor
    {
        private bool enableLike;
        private MatchOption? matchOption;
        private IList<OrderBase> orderParts;
        private bool ignoreCase;
        private IIdentityTraversalDescribable identityTraversalDescriber;

        public FindByExampleDescriptor()
        {
            this.enableLike = true;
            this.ignoreCase = true;
        }

        public bool EnableLike
        {
            get { return enableLike; }
            set { enableLike = value; }
        }

        public MatchOption? MatchOption
        {
            get { return matchOption; }
            set { matchOption = value; }
        }

        public IList<OrderBase> OrderParts
        {
            get { return orderParts; }
            set { orderParts = value; }
        }

        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set { ignoreCase = value; }
        }

        public IIdentityTraversalDescribable IdentityTraversalDescriber
        {
            get { return identityTraversalDescriber; }
            set { identityTraversalDescriber = value; }
        }

    }
}
