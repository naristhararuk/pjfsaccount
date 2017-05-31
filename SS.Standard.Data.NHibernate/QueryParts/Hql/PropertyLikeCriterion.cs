using System;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class PropertyLikeCriterion : CriterionBase, ISingleParameterCriterion, IPatternMatchCriterion
    {
        private LogicalOperator logicalOperator;
        private PropertyOperand propertyOperand;
        private string parameterName;
        private bool ignoreCase;
        private MatchOption? matchOption;

        public PropertyLikeCriterion()
        {

        }

        public override string ToPhrase(bool ignoreLogical)
        {
            StringBuilder phrase = new StringBuilder();

            if (!ignoreLogical)
            {
                phrase.Append(LogicalOperatorManipulator.GetStringValue(this.logicalOperator));
                phrase.Append(" ");
            }

            if (ignoreCase)
            {
                phrase.Append(" lower({0}) ");
                phrase.Replace("{0}", this.propertyOperand.ToPhrase());
            }
            else
            {
                phrase.Append(this.propertyOperand.ToPhrase());
            }

            phrase.Append(" ");
            phrase.Append(ComparisonOperatorManipulator.GetStringValue(ComparisonOperator.Like));
            phrase.Append(" ");

            if (ignoreCase)
            {
                phrase.Append(" lower(:{1}) ");
                phrase.Replace("{1}", this.parameterName);
            }
            else
            {
                phrase.Append(":");
                phrase.Append(this.parameterName);
            }

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
                return parameterName;
            }
        }

        public virtual Type CriterionParameterType
        {
            get
            {
                return typeof(string);
            }
        }

        public virtual LogicalOperator LogicalOperator
        {
            get { return logicalOperator; }
            set { logicalOperator = value; }
        }

        public virtual PropertyOperand PropertyOperand
        {
            get { return propertyOperand; }
            set { propertyOperand = value; }
        }

        public virtual string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        public virtual bool IgnoreCase
        {
            get { return ignoreCase; }
            set { ignoreCase = value; }
        }

        public virtual MatchOption? MatchOption
        {
            get { return matchOption; }
            set { matchOption = value; }
        }

    }
}
