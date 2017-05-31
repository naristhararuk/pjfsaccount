using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.WorkFlow.DAL
{
    public class WorkFlowDaoProvider
    {
        public WorkFlowDaoProvider() { }

        public static IWorkFlowDao WorkFlowDao { get; set; }
        public static IWorkFlowStateDao WorkFlowStateDao { get; set; }
        public static IWorkFlowResponseDao WorkFlowResponseDao { get; set; }
        public static IWorkFlowRejectResponseDao WorkFlowRejectResponseDao { get; set; }
        public static IWorkFlowHoldResponseDao WorkFlowHoldResponseDao { get; set; }
        public static IWorkFlowHoldResponseDetailDao WorkFlowHoldResponseDetailDao { get; set; }
        public static IWorkFlowVerifyResponseDao WorkFlowVerifyResponseDao { get; set; }
        public static IWorkFlowResponseTokenDao WorkFlowResponseTokenDao { get; set; }
        public static IWorkFlowStateEventPermissionDao WorkFlowStateEventPermissionDao { get; set; }
        public static IWorkFlowsmsTokenDao WorkFlowsmsTokenDao { get; set; }

    }
}
