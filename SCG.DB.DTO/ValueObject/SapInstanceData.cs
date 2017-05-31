using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class SapInstanceData
    {
        public string Code { get; set; }
        public string AliasName { get; set; }
        public string SystemID { get; set; }
        public string Client { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string SystemNumber { get; set; }
        public string MsgServerHost { get; set; }
        public string LogonGroup { get; set; }

    }
}
