using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SS.Standard.Data.NHibernate.QueryParts;

namespace SS.Standard.Data.NHibernate.QueryCreator
{
    public class QueryPartCollector
    {
        public class CriterionValuePair
        {
            private object criterionKey;
            private object criterionValue;

            public object CriterionKey
            {
                get { return this.criterionKey; }
                set { this.criterionKey = value; }
            }

            public object CriterionValue
            {
                get { return this.criterionValue; }
                set { this.criterionValue = value; }
            }
        }

        private IList selectPartKeys;
        private IList<CriterionValuePair> criterionValuePairs;
        private IList orderPartKeys;
        private int firstResult;
        private int maxResults;

        public QueryPartCollector()
        {
            firstResult = -1;
            maxResults = -1;
        }

        public int FirstResult
        {
            get { return firstResult; }
            set { firstResult = value; }
        }

        public int MaxResults
        {
            get { return maxResults; }
            set { maxResults = value; }
        }

        public void InitialParametersByPageIndex(int pageIndex, int maxResults)
        {
            this.maxResults = maxResults;
            this.firstResult = pageIndex * maxResults;
        }

        public IList<CriterionValuePair> CriterionValuePairs
        {
            get 
            {
                if (criterionValuePairs == null) criterionValuePairs = new List<CriterionValuePair>();
                return criterionValuePairs; 
            }
            set 
            { 
                criterionValuePairs = value; 
            }
        }

        public void AddCriterionValuePair(object criterionKey, object criterionValue)
        {
            CriterionValuePair criterionValuePair = new CriterionValuePair();
            criterionValuePair.CriterionKey = criterionKey;
            criterionValuePair.CriterionValue = criterionValue;

            this.CriterionValuePairs.Add(criterionValuePair);
        }

        public IList SelectPartKeys
        {
            get 
            {
                if (selectPartKeys == null) selectPartKeys = new ArrayList();
                return selectPartKeys;
            }
            set { selectPartKeys = value; }
        }

        public IList OrderPartKeys
        {
            get 
            {
                if (orderPartKeys == null) orderPartKeys = new ArrayList();
                return orderPartKeys;
            }
            set { orderPartKeys = value; }
        }

    }
}
