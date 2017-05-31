using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators
{
    public class LogicalOperatorManipulator
    {
        public static string GetStringValue(LogicalOperator logicalOperator)
        {
            return System.Enum.GetName(typeof(LogicalOperator), logicalOperator);
        }
    }
}
