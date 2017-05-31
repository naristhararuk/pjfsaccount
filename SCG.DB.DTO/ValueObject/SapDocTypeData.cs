using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class SapDocTypeData
    {
        public string UserCPIC { get; set; }

        public string ExpPostingDM { get; set; }
        public string ExpRmtPostingDM { get; set; }

        public string ExpPostingFR { get; set; }
        public string ExpRmtPostingFR { get; set; }
        public string ExpICPostingFR { get; set; }

        public string AdvancePostingDM { get; set; }

        public string AdvancePostingFR { get; set; }

        public string RmtPosting { get; set; }
    }
}
