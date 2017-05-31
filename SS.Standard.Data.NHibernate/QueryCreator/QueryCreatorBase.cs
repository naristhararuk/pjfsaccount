using System;
using System.Collections;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts;
using SS.Standard.Data.NHibernate.QueryCreator.Descriptor;

namespace SS.Standard.Data.NHibernate.QueryCreator
{
    public abstract class QueryCreatorBase : IQueryCreator
    {
        protected QueryDescriptor queryDescriptor;

        public virtual QueryDescriptor QueryDescriptor
        {
            set { this.queryDescriptor = value; }
        }

        public virtual string CreateSelectClause(QueryPartCollector queryPartCollector)
        {
            StringBuilder selectClause = new StringBuilder();

            if (queryPartCollector != null && queryDescriptor != null && queryPartCollector.SelectPartKeys != null)
            {
                bool isFirstElement = true;
                foreach (object key in queryPartCollector.SelectPartKeys)
                {
                    if (isFirstElement)
                    {
                        isFirstElement = false;
                        selectClause.Append(" select ");
                    }
                    else
                    {
                        selectClause.Append(" , ");
                    }

                    SelectBase selectPart = (SelectBase)queryDescriptor.SelectParts[key];
                    selectClause.Append(selectPart.ToPhrase());
                    selectClause.Append(" ");
                }
            }

            return selectClause.ToString();
        }

        public virtual string CreateFromClause(QueryPartCollector queryPartCollector)
        {
            StringBuilder fromClause = new StringBuilder();

            if (queryDescriptor != null && queryDescriptor.FromPart != null)
            {
                fromClause.Append(" from ");
                fromClause.Append(queryDescriptor.FromPart.ToPhrase());
                fromClause.Append(" ");
            }

            return fromClause.ToString();
        }

        public virtual string CreateWhereClause(QueryPartCollector queryPartCollector)
        {
            StringBuilder whereClause = new StringBuilder();

            if (queryPartCollector != null && queryPartCollector.CriterionValuePairs != null && queryDescriptor != null && queryDescriptor.CriterionParts != null)
            {
                bool isFirstElement = true;
                foreach (QueryPartCollector.CriterionValuePair criterionValuePair in queryPartCollector.CriterionValuePairs)
                {
                    CriterionBase criterionElement = (CriterionBase)queryDescriptor.CriterionParts[criterionValuePair.CriterionKey];

                    if (isFirstElement)
                    {
                        isFirstElement = false;
                        whereClause.Append(" where ");
                        whereClause.Append(criterionElement.ToPhrase(true));
                        whereClause.Append(" ");
                    }
                    else
                    {
                        whereClause.Append(" ");
                        whereClause.Append(criterionElement.ToPhrase());
                        whereClause.Append(" ");
                    }
                }
            }

            return whereClause.ToString();
        }

        public virtual string CreateOrderByClause(QueryPartCollector queryPartCollector)
        {
            StringBuilder orderByClause = new StringBuilder();

            if (queryPartCollector != null && queryPartCollector.OrderPartKeys != null)
            {
                bool isFirstElement = true;
                foreach (object key in queryPartCollector.OrderPartKeys)
                {
                    if (isFirstElement)
                    {
                        isFirstElement = false;
                        orderByClause.Append(" order by ");
                    }
                    else
                    {
                        orderByClause.Append(" , ");
                    }

                    OrderBase orderPart = (OrderBase)queryDescriptor.OrderParts[key];
                    orderByClause.Append(orderPart.ToPhrase());
                    orderByClause.Append(" ");
                }
            }

            return orderByClause.ToString();
        }

        public virtual string CreateGroupByClause(QueryPartCollector queryPartCollector)
        {
            return string.Empty;
        }

        public virtual string CreateHavingClause(QueryPartCollector queryPartCollector)
        {
            return string.Empty;
        }
    }
}
