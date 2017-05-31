using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.RefreshWorkFlowPermission.DAL;
using SS.Standard.WorkFlow.Query;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;


namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    public class RefreshWorkFlow
    {
        public RefreshWorkFlow()
        {
        }
        public void Permission()
        {
           

           //  Query document that its status does not 'Complete' or 'Cancel'

            IList<SS.Standard.WorkFlow.DTO.WorkFlow> workFlows = Factory.WorkFlowQuery.GetAllActiveWorkFlow(); // WorkFlowQueryProvider.WorkFlowQuery.GetAllActiveWorkFlow();
            int i = 0;
            string workFlowErrorMsg = string.Empty;
            string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Log\";
            string FilePath = "Log_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";
            string fullPath = DirectoryPath + FilePath;
            if ((!File.Exists(fullPath)))
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                File.Create(fullPath).Close();
            }

            Log.WriteMsgLogs("All Work Flow : " + workFlows.Count.ToString(), fullPath);
            Factory.UserAccount.CurrentProgramCode = "IntefaceRefreshPermission";

            foreach (SS.Standard.WorkFlow.DTO.WorkFlow workFlow in workFlows)
            {
                try
                {
                    workFlowErrorMsg = " Reflesh WorkFlowID : " + workFlow.WorkFlowID + " Completed";
                    i++;
                    Factory.WorkFlowService.ReCalculateWorkFlowPermission(workFlow.WorkFlowID);
                    Log.WriteMsgLogs("No. : " + i + workFlowErrorMsg, fullPath);
                    //Log.ShowMessage(workFlowErrorMsg);
                    Thread.Sleep(10);

                }
                catch (Exception ex)
                {
                    Log.WriteLogs(ex);
                    continue;

                }
            }
            workFlows = null;
            Thread.CurrentThread.Abort();
            Thread.CurrentThread.Join(new TimeSpan(0, 1, 0));
            
        }

     
    }
}
