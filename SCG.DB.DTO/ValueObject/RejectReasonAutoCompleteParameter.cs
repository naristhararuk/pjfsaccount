using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class DbReasonAutoCompleteParameter
    {
        public short?   ReasonID { get; set; }
        public short?   LanguageId          { get; set; }
        public string DocumentTypeCode { get; set; }
        public DbReasonAutoCompleteParameter()
        {
        }
    }
}
