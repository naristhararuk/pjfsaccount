using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class PropertyComparisonCriterion : CriterionBase, ISingleParameterCriterion
    {
        private LogicalOperator logicalOperator;
        private ComparisonOperator comparisonOperator;
        private PropertyOperand propertyOperand;
        private HqlValueOperand hqlValueOperand;

        public PropertyComparisonCriterion()
        {

        }

        public virtual LogicalOperator LogicalOperator
        {
            get { return logicalOperator; }
            set { this.logicalOperator = value; }
        }

        public virtual ComparisonOperator ComparisonOperator
        {
            get { return comparisonOperator; }
            set { this.comparisonOperator = value; }
        }

        public virtual PropertyOperand PropertyOperand
        {
            get { return propertyOperand; }
            set { this.propertyOperand = value; }
        }

        public virtual HqlValueOperand HqlValueOperand
        {
            get { return hqlValueOperand; }
            set { this.hqlValueOperand = value; }
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
            phrase.Append(" ");
            phrase.Append(ComparisonOperatorManipulator.GetStringValue(comparisonOperator));
            phrase.Append(" ");
            phrase.Append(hqlValueOperand.ToPhrase());

            return phrase.ToString();
        }

        public override string ToPhrase()
        {
            return ToPhrase(false);
        }

        public override bool IsCriterionParameterRequired()
        {
            return true;
        }

        public virtual string CriterionParameterName
        {
            get
            {
                return hqlValueOperand.ParameterName;
            }
        }

        public virtual Type CriterionParameterType
        {
            get
            {
                return hqlValueOperand.OperandType;
            }
        }

    }
}
