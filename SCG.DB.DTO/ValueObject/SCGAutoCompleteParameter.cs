using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    [Serializable]
    public class SCGAutoCompleteParameter
    {
        public long? CostCenterID { get; set; }
        public long? CompanyID { get; set; }
    }
}
