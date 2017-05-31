using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VoAdvanceFromTA
    {
        public long TADocumentID { get; set; }   
        public long RequesterID { get; set; }
        public long AdvanceID { get; set; }
        public long DocumentID { get; set; }

        public VoAdvanceFromTA()
        {
        }

    }
}
