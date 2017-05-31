using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class Mileage
    {
        public string CarLicenseNo { get; set; }
        public string TypeOfCar { get; set; }
        public string TypeOfUse { get; set; }
        public string Owner { get; set; }
        public string PermissionNo { get; set; }
        public string HomeOffice { get; set; }
        public string PrivateUse { get; set; }
        public string Rate { get; set; }
        public string ExceedingRate { get; set; }

        public string Date { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string CarMeterStart { get; set; }
        public string CarMeterEnd { get; set; }
        public string Adjust { get; set; }

        public string Total { get; set; }
        public string Net { get; set; }
    }
}
