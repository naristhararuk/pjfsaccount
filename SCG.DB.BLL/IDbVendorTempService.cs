using System;
using System.Collections.Generic;
using System.Linq;
using SS.Standard.Data.NHibernate.Service;
using System.Text;
using SCG.DB.BLL.Implement;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbVendorTempService : IService<DbVendorTemp,long>
    {
        void DeleteAll();
        void CommitTempToVendor();
    }
}
