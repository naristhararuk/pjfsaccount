using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DTO.ValueObject
{
    public class WorkFlowStateEventWithLang
    {
        public long EventID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string UserControlPath { get; set; }

    }
}
