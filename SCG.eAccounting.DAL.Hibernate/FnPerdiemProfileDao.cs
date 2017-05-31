using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnPerdiemProfileDao : NHibernateDaoBase<FnPerdiemProfile, long>, IFnPerdiemProfileDao
    {
        public FnPerdiemProfileDao()
        {
        }
        public bool IsDuplicateCode(FnPerdiemProfile fnPerdiemProfile)
        {
            IList<FnPerdiemProfile> list = GetCurrentSession().CreateQuery("from FnPerdiemProfile fp where fp.PerdiemProfileName Like :perdiemProfileName")
                  .SetString("perdiemProfileName", fnPerdiemProfile.PerdiemProfileName)
                  .List<FnPerdiemProfile>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
