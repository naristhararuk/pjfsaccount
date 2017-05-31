using System;
using System.Collections.Generic;
using System.Text;
using Spring.Data.NHibernate.Generic;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator.Descriptor;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.Query.Callback
{
    internal class FindByExampleCallback<TDomain> : IFindHibernateCallback<TDomain>
    {
        private TDomain exampleDomain;
        private FindByExampleDescriptor findByExampleDescriptor;

        public FindByExampleCallback(TDomain exampleDomain, FindByExampleDescriptor findByExampleDescriptor)
        {
            this.exampleDomain = exampleDomain;
            this.findByExampleDescriptor = findByExampleDescriptor;
        }

        public IList<TDomain> DoInHibernate(ISession session)
        {
            Example example = Example.Create(exampleDomain);

            if (findByExampleDescriptor.EnableLike)
            {
                if (findByExampleDescriptor.MatchOption != null)
                {
                    example.EnableLike(MatchOptionManipulator.GetMatchModeValue(findByExampleDescriptor.MatchOption));
                }
                else
                {
                    example.EnableLike();
                }
            }

            if (findByExampleDescriptor.IgnoreCase)
            {
                example.IgnoreCase();
            }

            ICriteria criteria = session.CreateCriteria(typeof(TDomain));
            criteria.Add(example);

            return criteria.List<TDomain>();
        }
    }
}
