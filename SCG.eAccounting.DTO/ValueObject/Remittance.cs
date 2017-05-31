using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class Remittance
    {
        public string ForeignAdvance { get; set; }
        public string ExchangeRate { get; set; }
        public string ForeignRemit { get; set; }
    }
}
