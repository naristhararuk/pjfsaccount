using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using SCG.eAccounting.Interface.RefreshWorkFlowPermission.DAL;

namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    partial class RefreshWorkFlowPermissionService : ServiceBase
    {
        
        public ListenerThread ListenerInstance = default(ListenerThread);
        public RefreshWorkFlowPermissionService()
        {
            InitializeComponent();
        }
        private Thread tListener = default(Thread);
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            try
            {
                Log.WriteMsgLogs("Service Start...");
                TimeSpan MarkingQueue = new TimeSpan(DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);

               // Log.ShowMessage("Preparing Factory Objects ...");
                Log.WriteMsgLogs("Factory CreateObject....");

                Factory.CreateObject();

               // Log.ShowMessage("Initial parameter for process... ");
                Log.WriteMsgLogs("Initial parameter for process....");

                Thread tListener = default(Thread);
                ListenerInstance = new ListenerThread();

                tListener = new Thread(ListenerInstance.ListenerProcess);
                tListener.Start();
                Log.WriteMsgLogs("ListenerInstance.ListenerProcess started...");

            }
            catch (Exception ex)
            {
                Log.WriteLogs(ex);
                throw ex;
            }
        }

        protected override void OnStop()
        {
            Log.WriteMsgLogs("Service Stop...");
           // ListenerInstance.ForceThreadAbort();
           
          //  tListener.Join(new TimeSpan(0, 1, 0));
           // Log.WriteMsgLogs("tListener Abort...");

            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
