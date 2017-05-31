using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	[Serializable]
	public class UserCriteria
	{
		#region Constructor
        public UserCriteria()
		{

		}
		#endregion

		#region Property
		public long? UserId { get; set; }
		public string UserName { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
		public string CompanyName { get; set; }
		public string DivisionName { get; set; }
        public string Email { get; set; }
        public string UserIdNOTIN { get; set; }
        public string UserIdIN { get; set; }
        public bool IsFavoriteApprover { get; set; }
        public bool IsFavoriteInitiator { get; set; }
        public bool IsApprovalFlag { get; set; }
        public long? RequesterID { get; set; }
		#endregion
	}
}
