using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;
using NHibernate;

namespace SS.SU.Query
{
   public interface ISuEHRExpenseLogQuery : IQuery <SueHrExpenseLog, long>
    {
       IList<SueHrExpenseLog> GeteHrExpenseLogList(SueHrExpenseLog EHRExpenseLogObj ,int startResult, int maxResult, string sortExpression);
       int CountEHRExpenseLogByCriteria(SueHrExpenseLog EHRExpenseLogObj);//get by เงื่อนไข
       ISQLQuery FindEHRExpenseLogResult(SueHrExpenseLog EHRExpenseLogObj, bool isCount, string sortExpression);
    }
}
