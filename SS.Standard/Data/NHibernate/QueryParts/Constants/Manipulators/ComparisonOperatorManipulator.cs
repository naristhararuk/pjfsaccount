using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators
{
    public class ComparisonOperatorManipulator
    {
        public static string GetStringValue(ComparisonOperator comparisonOperator)
        {
            string value = string.Empty;

            switch (comparisonOperator)
            {
                case ComparisonOperator.Equal:
                    value = "=";
                    break;
                case ComparisonOperator.NotEqual:
                    value = "<>";
                    break;
                case ComparisonOperator.GreaterThan:
                    value = ">";
                    break;
                case ComparisonOperator.GreaterOrEqual:
                    value = ">=";
                    break;
                case ComparisonOperator.LessThan:
                    value = "<";
                    break;
                case ComparisonOperator.LessOrEqual:
                    value = "<=";
                    break;
                case ComparisonOperator.Like:
                    value = "like";
                    break;
            }

            return value;
        }
    }
}
