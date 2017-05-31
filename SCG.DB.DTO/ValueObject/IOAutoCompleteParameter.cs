using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class IOAutoCompleteParameter
    {
        public long? IOID { get; set; }
        public long? CostCenterId { get; set; }
        public string CostCenterCode { get; set; }

        public long? CompanyId { get; set; }
        public string CompanyCode { get; set; }
    }
}
