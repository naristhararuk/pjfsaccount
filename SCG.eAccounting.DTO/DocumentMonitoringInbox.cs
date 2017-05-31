using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO
{
    public class DocumentMonitoringInbox
    {
        private string companyCode;

        public string CompanyCode
        {
            get { return companyCode; }
            set { companyCode = value; }
        }

        private string buName;

        public string BuName
        {
            get { return buName; }
            set { buName = value; }
        }

        private int? col1;

        public int? Col1
        {
            get { return col1; }
            set { col1 = value; }
        }

        private int? col2;

        public int? Col2
        {
            get { return col2; }
            set { col2 = value; }
        }

        private int? col3;

        public int? Col3
        {
            get { return col3; }
            set { col3 = value; }
        }

        private int? col4;

        public int? Col4
        {
            get { return col4; }
            set { col4 = value; }
        }

        private int? col5;

        public int? Col5
        {
            get { return col5; }
            set { col5 = value; }
        }

        public int? Total
        {
            get { return (Col1 ?? 0) + (Col2 ?? 0) + (Col3 ?? 0) + (Col4 ?? 0) + (Col5 ?? 0); }
        }
    }
}
