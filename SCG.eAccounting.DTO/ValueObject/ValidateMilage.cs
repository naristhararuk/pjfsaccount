using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class ValidateMilage
    {
        public double? CarMeterStart { get; set; }
        public double? CarMeterEnd { get; set; }
        public DateTime TravelDate { get; set; }
        public string DocumentNo { get; set; }
    }
}
