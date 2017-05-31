using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    public static class EntryPointService
    {
        public static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new RefreshWorkFlowPermissionService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
        
    }
}
