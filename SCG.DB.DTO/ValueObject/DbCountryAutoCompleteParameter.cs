using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class DbCountryAutoCompleteParameter
    {
        public short? CountryId { get; set; }
        public short? LanguageId { get; set; }
    }
}
