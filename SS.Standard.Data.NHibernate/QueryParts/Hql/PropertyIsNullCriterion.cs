using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class PropertyIsNullCriterion : CriterionBase
    {
        private LogicalOperator logicalOperator;
        private PropertyOperand propertyOperand;

        public virtual PropertyOperand PropertyOperand
        {
            get { return propertyOperand; }
            set { propertyOperand = value; }
        }

        public virtual LogicalOperator LogicalOperator
        {
            get { return logicalOperator; }
            set { logicalOperator = value; }
        }

        public override string ToPhrase(bool ignoreLogical)
        {
            StringBuilder phrase = new StringBuilder();

            if (!ignoreLogical)
            {
                phrase.Append(LogicalOperatorManipulator.GetStringValue(logicalOperator));
                phrase.Append(" ");
            }

            phrase.Append(propertyOperand.ToPhrase());
            phrase.Append(" is null ");

            return phrase.ToString();
        }

        public override string ToPhrase()
        {
            return ToPhrase(false);
        }

        public override bool IsCriterionParameterRequired()
        {
            return false;
        }

    }
}
