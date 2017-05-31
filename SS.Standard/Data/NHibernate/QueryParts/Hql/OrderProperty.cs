using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;
using SS.Standard.Data.NHibernate.QueryParts.Constants;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class OrderProperty : OrderBase
    {
        private string alias;
        private string propertyName;
        private OrderDirection orderDirection;

        public virtual string Alias
        {
            get { return alias; }
            set { this.alias = value; }
        }

        public virtual string PropertyName
        {
            get { return propertyName; }
            set { this.propertyName = value; }
        }

        public virtual OrderDirection OrderDirection
        {
            get { return orderDirection; }
            set { orderDirection = value; }
        }

        public override string ToPhrase()
        {
            StringBuilder phrase = new StringBuilder();

            if (!string.IsNullOrEmpty(alias))
            {
                phrase.Append(alias);
                phrase.Append(".");
            }

            phrase.Append(propertyName);
            phrase.Append(" ");
            phrase.Append(OrderDirectionManipulator.GetStringValue(orderDirection));

            return phrase.ToString();
        }
    }
}
