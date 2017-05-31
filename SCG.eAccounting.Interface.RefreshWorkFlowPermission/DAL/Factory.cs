using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.Utilities;
using SS.Standard.WorkFlow.Service;
using SS.Standard.Security;
using SS.DB.Query;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IWorkFlowService WorkFlowService;
        public static IUserAccount UserAccount { get; set; }
        public static IWorkFlowQuery WorkFlowQuery { get; set; }
        public static int RefreshWorkFlowPermissionProcessTime
        {
            get {
                int defaultTime = 1; // Minute
                try
                {
                    Log.WriteMsgLogs("get RefreshWorkFlowPermissionProcessTime");
                    if (ParameterServices.RefreshWorkFlowPermissionProcessTime != null && ParameterServices.RefreshWorkFlowPermissionProcessTime > 0)
                    {
                        defaultTime = ParameterServices.RefreshWorkFlowPermissionProcessTime;
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteLogs(ex);
                }
                return defaultTime;
            }
        }
        public static int RefreshWorkFlowPermissionListernerPort
        {
            get
            {
                    int defaultPort = 13777;
                    try
                    {
                        Log.WriteMsgLogs("get RefreshWorkFlowPermissionListernerPort");

                        if (ParameterServices.RefreshWorkFlowPermissionListernerPort != null && ParameterServices.RefreshWorkFlowPermissionListernerPort > 0)
                        {
                            defaultPort = ParameterServices.RefreshWorkFlowPermissionListernerPort;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.WriteLogs(ex);
                    }
                    return defaultPort;
            }
        }

         public static void CreateObject()
         {
             try
             {
                 UserAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
                 UserAccount.CurrentProgramCode = "Inteface";
                 WorkFlowService = (IWorkFlowService)ObjectManager.GetObject("WorkFlowService");
                 WorkFlowQuery = (IWorkFlowQuery)ObjectManager.GetObject("WorkFlowQuery");

                 //  Parameter = (ParameterServices)ObjectManager.GetObject("ParameterServices");

             }
             catch (Exception ex)
             {

                 Log.WriteLogs(ex);
             }
         }
    }
}
