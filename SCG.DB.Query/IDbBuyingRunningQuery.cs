using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbBuyingRunningQuery : IQuery<DbBuyingRunning, long>
    {
        DbBuyingRunning GetBuyingRunningByCompanyCode_Year(string companyCode, int year);
    }
}
