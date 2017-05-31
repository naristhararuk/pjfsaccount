using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class TADocumentObj
    {
        public long? WorkflowID { get; set; }
        public long? DocumentID { get; set; }
        public string DocumentNo { get; set; }
        public string Creator { get; set; }
        public string CreateDate { get; set; }
        public string DocumentTypeName { get; set; }
        public string Subject { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public long? TADocumentID { get; set; }
        public bool? IsBusinessPurpose { get; set; }
        public bool? IsTrainningPurpose { get; set; }
        public bool? IsOtherPurpose { get; set; }
        public string OtherPurposeDescription { get; set; }
        public string Ticketing { get; set; }

        public long? UserID { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }

        public string TravellerID { get; set; }
        public string EmployeeNameEng { get; set; }
        public string AirLineMember { get; set; }

        public int? ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public string DepartureFrom { get; set; }
        public string ArrivalAt { get; set; }
        public string TravelBy { get; set; }
        public string Time { get; set; }
        public string FlightNo { get; set; }
        public string TravellingDetail { get; set; }

        //TA Advance Tab
        public int? TADocumentAdvanceID { get; set; }
        public long? AdvanceID { get; set; }
        public string AdvanceNo { get; set; }
        public string AdvanceStatus { get; set; }
        public string Description { get; set; }
        public long? RequesterID { get; set; }
        public string RequesterName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal AmountTHB { get; set; }
        public double AdvanceMainCurrencyAmount { get; set; }
    }
}
