using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
   public class CostCenterCompany
    {
       public long CostCenterID { get; set; }
       public string CostCenterCode { get; set; }
       public string Description { get; set; }
       public DateTime Valid { get; set; }
       public DateTime Expire { get; set; }
       public string CompanyCode { get; set; }
       public bool ActualPrimaryCosts { get; set; }
       public bool Active { get; set; }
       
       
    }
}
