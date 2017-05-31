using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Constants.Manipulators
{
    public class OrderDirectionManipulator
    {
        public static string GetStringValue(OrderDirection orderDirection)
        {
            string value = string.Empty;

            switch (orderDirection)
            {
                case OrderDirection.Ascending:
                    value = "asc";
                    break;
                case OrderDirection.Descending:
                    value = "desc";
                    break;
            }

            return value;
        }
    }
}
