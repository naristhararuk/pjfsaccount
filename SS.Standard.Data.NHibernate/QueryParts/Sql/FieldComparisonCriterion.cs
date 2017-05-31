using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.QueryParts.Sql
{
    public class FieldComparisonCriterion : CriterionBase, ISingleParameterCriterion
    {
        private LogicalOperator logicalOperator;
        private ComparisonOperator comparisonOperator;
        private FieldOperand fieldOperand;
        private SqlValueOperand sqlValueOperand;

        public FieldComparisonCriterion()
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

        public virtual FieldOperand FieldOperand
        {
            get { return fieldOperand; }
            set { this.fieldOperand = value; }
        }

        public virtual SqlValueOperand SqlValueOperand
        {
            get { return sqlValueOperand; }
            set { this.sqlValueOperand = value; }
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
            phrase.Append(" ");
            phrase.Append(ComparisonOperatorManipulator.GetStringValue(comparisonOperator));
            phrase.Append(" ");
            phrase.Append(sqlValueOperand.ToPhrase());

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
                return sqlValueOperand.ParameterName;
            }
        }

        public virtual Type CriterionParameterType
        {
            get
            {
                return sqlValueOperand.OperandType;
            }
        }

    }
}
