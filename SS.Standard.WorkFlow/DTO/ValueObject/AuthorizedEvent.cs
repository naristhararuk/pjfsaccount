using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class AuthorizedEvent
    {
        public AuthorizedEvent() { }

        public AuthorizedEvent(long eventID , string name , string displayName , string userControlPath) 
        {
            this.EventID = eventID;
            this.Name = name;
            this.DisplayName = displayName;
            this.UserControlPath = userControlPath;
        }

        public long EventID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string UserControlPath { get; set; }
    }
}
