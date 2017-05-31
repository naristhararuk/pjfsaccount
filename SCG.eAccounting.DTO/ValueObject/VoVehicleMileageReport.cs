using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class VoVehicleMileageReport
    {
        public long? CompanyID { get; set; }
        public string FromRequesterID { get; set; }
        public string ToRequesterID { get; set; }
        public string FromCarRegist { get; set; }
        public string ToCarRegist { get; set; }
        public string FromTaDocumentNo { get; set; }
        public string ToTaDocumentNO { get; set; }
        public string DocumentStatus { get; set; }
        public string ParameterList { get; set; }
        public string ParameterList2 { get; set; }

        public VoVehicleMileageReport()
        {}
       
    }
}
