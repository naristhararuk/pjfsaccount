using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;


using System.Data;
using SS.Standard.WorkFlow.DTO;
namespace SS.Standard.WorkFlow.DAL
{
    public interface IWorkFlowsmsTokenDao : IDao<WorkFlowsmsToken, long>
    {
    }
}
