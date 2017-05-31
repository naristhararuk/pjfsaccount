using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NHibernate;
using NHibernate.Expression;

namespace SS.Standard.Data.NHibernate.Query
{
    public class NHibernateQueryHelper
    {
        public static IList<T> FindByCriteria<T>(object finder, string methodName, object[] parameters)
        {
            return FindPagingByCriteria<T>(finder, methodName, parameters, -1, -1, string.Empty);
        }

        public static IList<T> FindByCriteria<T>(object finder, string methodName, object[] parameters, string sortExpression)
        {
            return FindPagingByCriteria<T>(finder, methodName, parameters, -1, -1, sortExpression);
        }

        public static IList<T> FindPagingByCriteria<T>(object finder, string methodName, object[] parameters, int firstResult, int maxResults, string sortExpression)
        {
            MethodInfo methodInfo = finder.GetType().GetMethod(methodName);
            object invokeResult = methodInfo.Invoke(finder, parameters);
            IList<T> result = new List<T>();

            if (invokeResult is IQuery)
            {
                IQuery query = (IQuery)invokeResult;

                if (firstResult != -1 && maxResults != -1)
                {
                    query.SetFirstResult(firstResult);
                    query.SetMaxResults(maxResults);
                }

                result = query.List<T>();
            }
            else if (invokeResult is ICriteria)
            {
                ICriteria criteria = (ICriteria)invokeResult;

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sortExpression = sortExpression.Trim();
                    if (sortExpression.ToLower().EndsWith(" asc"))
                    {
                        sortExpression = sortExpression.Substring(0, sortExpression.Length - 4);
                        criteria.AddOrder(Order.Asc(sortExpression));
                    }
                    else if (sortExpression.ToLower().EndsWith(" desc"))
                    {
                        sortExpression = sortExpression.Substring(0, sortExpression.Length - 5);
                        criteria.AddOrder(Order.Desc(sortExpression));
                    }
                }

                if (firstResult != -1 && maxResults != -1)
                {
                    criteria.SetFirstResult(firstResult);
                    criteria.SetMaxResults(maxResults);
                }

                result = criteria.List<T>();
            }

            return result;
        }

        public static int CountByCriteria(object finder, string methodName, object[] parameters)
        {
            MethodInfo methodInfo = finder.GetType().GetMethod(methodName);
            object invokeResult = methodInfo.Invoke(finder, parameters);
            int result = 0;

            if (invokeResult is IQuery)
            {
                IQuery query = (IQuery)invokeResult;
                result = Convert.ToInt32(query.UniqueResult());
            }
            else if (invokeResult is ICriteria)
            {
                ICriteria criteria = (ICriteria)invokeResult;
                criteria.SetProjection(Projections.RowCount());
                result = Convert.ToInt32(criteria.UniqueResult());
            }

            return result;
        }
    }
}
