using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    [Serializable]
    public class VOUserProfile
    {
        #region Property
        public long? UserId{ get; set; }
        public string UserName { get; set; }   
        public long? UserGroupId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }   
        public Boolean Active { get; set; }

        public Boolean FromEHr { get; set; }
        public string PeopleID { get; set; }
        public string EmployeeCode { get; set; }
        public string CompanyCode { get; set; }
        public string CostCenterCode { get; set; }
        public long? LocationID { get; set; }
        public long? Supervisor { get; set; }
        public string SectionName { get; set; }
        public string PersonalLevel { get; set; }
        public string PersonalDescription { get; set; }
        public string PersonalGroup { get; set; }
        public string PersonalLevelGroupDescription { get; set; }
        public string PositionName { get; set; }
        public string PhoneNo { get; set; }

        
        // mobile phone 
        // approve/reject
        //ready to receive

        public Boolean EmailActive { get; set; }
        public string Email { get; set; }
        public Boolean ApprovalFlag { get; set; }
       


 
        



        #endregion
    }
}
