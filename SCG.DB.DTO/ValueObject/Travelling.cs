using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class Travelling
    {
        //Travelling Infomation
        public int TravellingID { get; set; }
        public long UserID { get; set; }
        public string FullNameThai { get; set; }
        public string FullNameEng { get; set; }
        public string AirlineMember { get; set; }

        //Travelling Schedule
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public string DepartureFrom { get; set; }
        public string ArrivalAt { get; set; }
        public string TravelBy { get; set; }
        public string Time { get; set; }
        public string FlightNo { get; set; }
        public string TravellingDetail { get; set; }
    }
}
