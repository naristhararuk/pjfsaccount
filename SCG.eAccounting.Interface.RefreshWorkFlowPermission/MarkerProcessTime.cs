using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SCG.eAccounting.Interface.RefreshWorkFlowPermission.DAL;

namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    public class MarkerProcessTime
    {
        private RefreshWorkFlow RefreshProcessInstance = default(RefreshWorkFlow);
        private Thread tRefreshProcess = default(Thread);
        private Nullable<TimeSpan> dtNow = default(TimeSpan);
        private TimeSpan dt = default(TimeSpan);
        private TimeSpan LastProcessTime { get; set; }
        public TimeSpan _MarkingQueue { get; set; }
        public bool FirstServiceStart { get; set; }
        public MarkerProcessTime()
        {
            try
            {
                RefreshProcessInstance = new RefreshWorkFlow();
                LastProcessTime = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
        public void NotifyRefreshProcess()
        {
            //DateTime CompareTo() Method
            //Value               Description 

            //Less than zero      This instance is earlier than value. 

            //Zero                This instance is the same as value. 

            //Greater than zero   This instance is later than value. 
            Log.WriteMsgLogs(" NotifyRefreshProcess : Start");
            int RefreshWorkFlowPermissionProcessTime = Factory.RefreshWorkFlowPermissionProcessTime;
            try
            {
                ThreadStart job = new ThreadStart(RefreshProcessInstance.Permission);
                while (true)
                {
                    Log.WriteMsgLogs(" MarkingQueue : " + _MarkingQueue);
                    if (_MarkingQueue != new TimeSpan(0,0,0,0))
                    {
                        dtNow = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                       // Log.ShowMessage("dt:" + dt.ToString() + " dtNow :" + dtNow.ToString() + "  result : " + dtNow.Value.CompareTo(dt));
                        if (dtNow.Value.CompareTo(_MarkingQueue) > 0 && !FirstServiceStart)
                        {

                            if (tRefreshProcess != null && tRefreshProcess.IsAlive)
                            {
                                try
                                {
                                    Log.WriteMsgLogs(" Job Reflesh Abort by : tRefreshProcess != null && tRefreshProcess.IsAlive" );
                                    tRefreshProcess.Abort();
                                    tRefreshProcess.Join(new TimeSpan(0, 1, 0));
                                    tRefreshProcess = null;
                                }
                                catch (Exception ex)
                                {

                                    Log.WriteLogs(ex);
                                }

                            }
                                tRefreshProcess = new Thread(job);
                                tRefreshProcess.Start();
                                Log.WriteMsgLogs(" Job Reflesh Started ");
                                _MarkingQueue = new TimeSpan(0, 0, 0, 0);
                          //  Log.ShowMessage("RefreshProcess Thread started ... ");
                        }
                    }
                    dtNow = null;
                    Log.WriteMsgLogs("Thread MarkingQueue process Sleep " + RefreshWorkFlowPermissionProcessTime + " Minute");
                    Thread.Sleep(new TimeSpan(0, RefreshWorkFlowPermissionProcessTime, 0));
                }
           
            }
            catch (Exception ex)
            {
                
                 Log.WriteLogs(ex);
            }
            
        }

        public void ForceThreadAbort()
        {
            try
            {
                Log.WriteMsgLogs("MarkerProcessTime : ForceThreadAbort by user or program");
                if (tRefreshProcess != null && tRefreshProcess.IsAlive)
                {
                    tRefreshProcess.Abort();
                    tRefreshProcess.Join(new TimeSpan(0, 1, 0));
                }

            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
        }

    }
}
