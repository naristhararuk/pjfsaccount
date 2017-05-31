using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using SCG.eAccounting.Resender.DAL;
using System.Net;
using System.Net.Sockets;
using SS.SU.DTO;
using SCG.eAccounting.BLL;
using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SS.Standard.CommunicationService.Implement;
using SS.DB.Query;
using SCG.eAccounting.DTO;
using System.ServiceProcess;

namespace SCG.eAccounting.Resender
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ResenderWindowService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
