using System;
using System.Collections.Generic;
using System.Linq;
using SS.Standard.Data.NHibernate.Service.Implement;
using System.Text;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SCG.eAccounting.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query.Hibernate;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public class DbDocumentImagePostingService : ServiceBase<DbDocumentImagePosting, long>, IDbDocumentImagePostingService
    {
        public NHibernate.ISessionFactory SessionFactory { get; set; }
        public ISCGDocumentService SCGDocumentService { get; set; }
        public SS.Standard.WorkFlow.Service.IWorkFlowService WorkFlowService { get; set; }
     

        public override SS.Standard.Data.NHibernate.Dao.IDao<DbDocumentImagePosting, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.DbDocumentImagePostingDao;
        }


        //public void NotifyImageFile(long documentID)
        //{
        //        WorkFlow wf = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(documentID);
        //        WorkFlowStateEvent wfse = WorkFlowQueryProvider.WorkFlowStateEventQuery.GetByWorkFlowStateID_EventName(38, "Pay");
        //        SubmitResponse sr = new SubmitResponse(wfse.WorkFlowStateEventID);
        //        //WorkFlowService.NotifyEventFromExternal(wf.WorkFlowID, 1, 1, "Pay", sr);
        //        WorkFlowService.NotifyEvent(wf.WorkFlowID,"Pay", sr);
        //}


        #region IDbDocumentImagePostingService Members


        public void ImportFreshNewImagePosting()
        {
            ScgeAccountingDaoProvider.DbDocumentImagePostingDao.ImportFreshNewImagePosting();
        }

        #endregion
    }
}
