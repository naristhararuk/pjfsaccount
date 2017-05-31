using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{

    public class VOAccountCompany
    {
        public long? ID{ get; set; }  //AcountCompanyId
        public long? CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public long? AccountID{ get; set; }
        public string AccountCode { get; set; }
        public bool Active { get; set; }


    }
}
