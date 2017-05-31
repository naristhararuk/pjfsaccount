using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class SuOrganizationWithLang
    {
        public long OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }
}
