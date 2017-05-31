using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbSellingRunningService : IService<DbSellingRunning, long>
    {
        //void AddSellingRuning(long companyID, int year);
        string RetrieveNextSellingRunningNo(string companyCode, int year);
    }
}