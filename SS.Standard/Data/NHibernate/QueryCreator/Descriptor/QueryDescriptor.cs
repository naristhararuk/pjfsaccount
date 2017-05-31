using System;
using System.Text;
using System.Collections.Generic;
using SS.Standard.Data.NHibernate.QueryParts;

namespace SS.Standard.Data.NHibernate.QueryCreator.Descriptor
{
    public class QueryDescriptor
    {
        private IDictionary<object, SelectBase> selectParts;
        private FromBase fromPart;
        private IDictionary<object, CriterionBase> criterionParts;
        private IDictionary<object, OrderBase> orderParts;

        public IDictionary<object, SelectBase> SelectParts
        {
            get 
            {
                if (this.selectParts == null) this.selectParts = new Dictionary<object, SelectBase>();
                return this.selectParts; 
            }
            set { this.selectParts = value; }
        }

        public FromBase FromPart
        {
            get { return this.fromPart; }
            set { this.fromPart = value; }
        }

        public IDictionary<object, CriterionBase> CriterionParts
        {
            get 
            {
                if (this.criterionParts == null) this.criterionParts = new Dictionary<object, CriterionBase>();
                return this.criterionParts; 
            }
            set { this.criterionParts = value; }
        }

        public IDictionary<object, OrderBase> OrderParts
        {
            get
            {
                if (this.orderParts == null) this.orderParts = new Dictionary<object, OrderBase>();
                return this.orderParts;
            }
            set { this.orderParts = value; }
        }

    }
}
