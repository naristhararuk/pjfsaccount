using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.QueryParts.Sql
{
    public class FieldIsNullCriterion : CriterionBase
    {
        private LogicalOperator logicalOperator;
        private FieldOperand fieldOperand;

        public virtual LogicalOperator LogicalOperator
        {
            get { return logicalOperator; }
            set { this.logicalOperator = value; }
        }

        public virtual FieldOperand FieldOperand
        {
            get { return fieldOperand; }
            set { this.fieldOperand = value; }
        }

        public override string ToPhrase(bool ignoreLogical)
        {
            StringBuilder phrase = new StringBuilder();

            if (!ignoreLogical)
            {
                phrase.Append(LogicalOperatorManipulator.GetStringValue(logicalOperator));
                phrase.Append(" ");
            }

            phrase.Append(fieldOperand.ToPhrase());
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
