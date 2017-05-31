using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;

namespace SCG.eAccounting.Query
{
	public interface IEmailLogQuery : IQuery<SuEmailLog, long>
	{
        ISQLQuery FindByEmailLogCriteria(string emailType, string sendDate, string requestNo, int status, bool isCount, string sortExpression, short lang);
        IList<VOEmailLog> GetLogList(string emailType, string sendDate, string requestNo, int status, int firstResult, int maxResult, string sortExpression, short lang);
        int CountByLogCriteria(string emailType, string sendDate, string requestNo, int status,short lang);

	}
}
