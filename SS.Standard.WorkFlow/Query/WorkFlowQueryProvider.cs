using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.WorkFlow.Query.Hibernate;

namespace SS.Standard.WorkFlow.Query
{
    public class WorkFlowQueryProvider
    {
        public WorkFlowQueryProvider() { }

        public static IDocumentQuery DocumentQuery { get; set; }
        public static IDocumentTypeQuery DocumentTypeQuery { get; set; }
        public static IWorkFlowQuery WorkFlowQuery { get; set; }
        public static IWorkFlowStateQuery WorkFlowStateQuery { get; set; }
        public static IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }
        public static IWorkFlowStateTransitionQuery WorkFlowStateTransitionQuery { get; set; }
        public static IWorkFlowTypeDocumentTypeQuery WorkFlowTypeDocumentTypeQuery { get; set; }
        public static IWorkFlowResponseQuery WorkFlowResponseQuery { get; set; }
        public static IWorkFlowHoldResponseQuery WorkFlowHoldResponseQuery { get; set; }
        public static IWorkFlowHoldResponseDetailQuery WorkFlowHoldResponseDetailQuery { get; set; }
        public static IWorkFlowStateEventPermissionQuery WorkFlowStateEventPermissionQuery { get; set; }
        public static IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public static IWorkFlowRejectResponseQuery WorkFlowRejectResponseQuery { get; set; }
        public static IWorkFlowsmsTokenQuery WorkFlowsmsTokenQuery { get; set; }

    }
}

