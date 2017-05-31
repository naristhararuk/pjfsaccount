using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
 
    [Serializable]
    public class UserFavoriteInitiator
    {
        public UserFavoriteInitiator()
        {
        }

        public long UserFavoriteActorID { get; set; }
        public long UserID { get; set; }
        public short InitiatorSeq { get; set; }
        public long ActorUserID { get; set; }
        public string ActorType { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public Boolean SMS { get; set; }
        

    }
}
