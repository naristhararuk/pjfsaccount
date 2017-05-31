using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO
{
    public partial class MonitoringInBoxSearchCriteria
    {
        private string company;
        private string businessGroup;
        private DateTime? fromDate;
        private DateTime? toDate;


        public virtual string Company
        {
            get { return company; }
            set { company = value; }
        }

        public virtual string BusinessGroup
        {
            get { return businessGroup; }
            set { businessGroup = value; }
        }

        public virtual DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        public virtual DateTime? ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }
        
    }
}
