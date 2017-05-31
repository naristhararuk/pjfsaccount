using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.BLL
{
    public interface IFnPerdiemRateService : IService<FnPerdiemRate, long> 
    {
        void AddFnPerdiemRate(FnPerdiemRate fnPerdiemRate);
        void UpdateFnPerdiemRate(FnPerdiemRate fnPerdiemRate);
        void DeleteFnPerdiemRate(FnPerdiemRate fnPerdiemRate);
    }
}
