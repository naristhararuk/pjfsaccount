using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    public class ExportPayroll
    {
        public string CompanyCode { get; set; }
        public string EmployeeCode { get; set; }
        public string CostCenterCode { get; set; }
        public decimal totalAmount { get; set; }
        public string wagecode { get; set; }
        public string PayrollType { get; set; }
        public string PeopleID { get; set; }
    }
}
