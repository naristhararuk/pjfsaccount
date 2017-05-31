using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
   public  class ExportDocumentImage
    {
       public string FileName { get; set; }
       public string DocNumber { get; set; }
       public string CompanyCode { get; set; }
       public long DocumentID { get; set; }
       public DateTime DocumentDate { get; set; }
       public string ImageDocumentType { get; set; }
       public string FI_DOC { get; set; }
       public string STATUS { get; set; }
    }
}
