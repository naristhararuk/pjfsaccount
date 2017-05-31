using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnAutoPaymentTempService : ServiceBase<FnAutoPaymentTemp, long>, IFnAutoPaymentTempService
    {
        public SS.Standard.Security.IUserAccount UserAccount { get; set; }
        public override SS.Standard.Data.NHibernate.Dao.IDao<FnAutoPaymentTemp, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnAutoPaymentTempDao;
        }

        public NHibernate.ISessionFactory SessionFactory { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public SS.Standard.WorkFlow.Service.IWorkFlowService WorkFlowService { get; set; }

        public void NotifyImageFile(long documentID)
        {
            //FnAutoPayment p = SCG.eAccounting.Query.ScgeAccountingQueryProvider.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(documentID);
            //if (p.Status != 2) return;

           
            bool b = ScgeAccountingDaoProvider.FnAutoPaymentTempDao.IsSuccess(documentID);
            if (b)
            {
                return;
            }
            bool commitSuccess = ScgeAccountingDaoProvider.FnAutoPaymentTempDao.CommitToAutoPayment(documentID);
            if (!commitSuccess)
            {
                return;
            }

            WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
            WorkFlowStateEvent wfse = WorkFlowQueryProvider
                .WorkFlowStateEventQuery
                .GetByWorkFlowStateID_EventName(wf.CurrentState.WorkFlowStateID, "Pay");
            SubmitResponse sr = new SubmitResponse(wfse.WorkFlowStateEventID);
            sr.ResponseMethod = ResponseMethod.SAP;
            //WorkFlowService.NotifyEventFromExternal(wf.WorkFlowID, 1, 1, "Pay", sr);
            //WorkFlowService.NotifyEvent(wf.WorkFlowID, "Pay", sr);
            //Using new method on 30/04/2009
            WorkFlowService.NotifyEventFromInternal(wf.WorkFlowID, "Pay", sr);
            
        }

        #region IFnAutoPaymentTempService Members



        public void ClearTemporary()
        {
            ScgeAccountingDaoProvider.FnAutoPaymentTempDao.DeleteAll();
        }

        #endregion
    }
}
