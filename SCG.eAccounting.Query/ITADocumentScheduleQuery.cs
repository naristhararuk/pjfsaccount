using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.eAccounting.DTO;
using NHibernate;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query
{
    public interface ITADocumentScheduleQuery : IQuery<TADocumentSchedule, int>
    {
        IList<TADocumentObj> GetTADocumentScheduleList(int firstResult, int maxResult, string sortExpression);
        int CountByTADocumentScheduleCriteria();
        ISQLQuery FindByTADocumentScheduleCriteria(bool isCount, string sortExpression);
        IList<TADocumentSchedule> FindTADocumentScheduleByTADocumentID(long taDocumentID);
    }
}
