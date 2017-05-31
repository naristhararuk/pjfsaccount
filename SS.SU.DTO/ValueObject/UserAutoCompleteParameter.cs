using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class UserAutoCompleteParameter
    {
        public long? UserID { get; set; }
        public string UserName {get;set;}

        public long? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public long? LocationID { get; set; }

        public long? CompanyId { get; set; }
        public string CompanyCode { get; set; }

        public long? CostCenterId { get; set; }
        public string CostCenterCode { get; set; }

        public bool IsApprovalFlag { get; set; }
        public bool? IsActive { get; set; }
    }
}
