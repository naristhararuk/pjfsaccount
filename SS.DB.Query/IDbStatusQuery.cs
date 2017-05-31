using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbStatusQuery : IQuery<DbStatus, short> 
    {
        IList<DbStatus> GetStatusList(int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindByLanguageCriteria(string sortExpression, bool isCount);
        int GetCountStatusList();
        IList<StatusLang> FindPaymentTypeByLang(short languageID);
    }
}
