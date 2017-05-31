using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public interface IWorkFlowMonitor
    {
        void Initialize(long wfId);
    }
}
