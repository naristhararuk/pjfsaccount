using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

namespace SS.Standard.WorkFlow.DAL
{
    public interface IWorkFlowResponseDao : IDao<WorkFlow.DTO.WorkFlowResponse, long>
    {
        void ResetActiveWorkFlowResponse(long wfid, int nextOrdinal);
    }
}
